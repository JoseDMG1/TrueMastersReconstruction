using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>Reliable, framed TCP channel used by the two-player LAN mode.</summary>
public class TCPConnection : MonoBehaviour
{
	private const int MaxFrameSize = 1024 * 1024;
	private readonly object sync = new object();
	private readonly Queue<byte[]> bytesQueue = new Queue<byte[]>();
	private TcpListener tcpListener;
	private TcpClient tcpClient;
	private NetworkStream networkStream;
	private Thread acceptThread;
	private Thread receiveThread;
	private volatile bool running;
	private volatile bool isConnected;
	private bool isServer;
	private string pendingClientAddress;

	public LocalMultiplayerData localData;
	public UnityEvent<byte[]> onMessageReceived = new UnityEvent<byte[]>();
	public UnityEvent<string> onConnect = new UnityEvent<string>();
	public UnityEvent<string> onClientConnected = new UnityEvent<string>();
	public int port = 7777;

	private void Start() { }

	private void Update()
	{
		while (true)
		{
			byte[] message;
			lock (sync)
			{
				if (bytesQueue.Count == 0) break;
				message = bytesQueue.Dequeue();
			}
			if (onMessageReceived != null) onMessageReceived.Invoke(message);
		}
		string clientAddress = null;
		lock (sync) { clientAddress = pendingClientAddress; pendingClientAddress = null; }
		if (clientAddress != null && onClientConnected != null) onClientConnected.Invoke(clientAddress);
	}

	public void Init()
	{
		if (localData != null && localData.isHost) StartServer(port);
	}

	// Wire format: event byte, then a sequence of (type byte, value) pairs.
	// TCP framing is a 4-byte little-endian payload length followed by payload.
	public byte[] CreateMessage(NetFastEvent netEvent, params object[] parameters)
	{
		using (MemoryStream ms = new MemoryStream())
		using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
		{
			writer.Write((byte)netEvent);
			if (parameters != null)
			{
				foreach (object value in parameters)
				{
					if (value is int) { writer.Write((byte)ParamType.Int); writer.Write((int)value); }
					else if (value is float) { writer.Write((byte)ParamType.Float); writer.Write((float)value); }
					else if (value is string) { writer.Write((byte)ParamType.String); WriteString(writer, (string)value); }
					else if (value is bool) { writer.Write((byte)ParamType.Bool); writer.Write((bool)value); }
					else if (value is double) { writer.Write((byte)ParamType.Double); writer.Write((double)value); }
					else throw new ArgumentException("Unsupported LAN message parameter type.");
				}
			}
			return ms.ToArray();
		}
	}

	public void CreateAndSendMessage(NetFastEvent netEvent, params object[] parameters) { SendByteMessage(CreateMessage(netEvent, parameters)); }
	public void SendShortMessage(NetFastEvent netEvent) { SendByteMessage(new byte[] { (byte)netEvent }); }
	public bool ParseShortMessage(byte[] data, NetFastEvent expectedEvent) { return data != null && data.Length == 1 && data[0] == (byte)expectedEvent; }

	public object[] ParseMessage(byte[] data, NetFastEvent expectedEvent, params ParamType[] expectedTypes)
	{
		if (data == null || data.Length == 0 || data[0] != (byte)expectedEvent) return null;
		try
		{
			using (MemoryStream ms = new MemoryStream(data, 1, data.Length - 1))
			using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
			{
				object[] result = new object[expectedTypes.Length];
				for (int i = 0; i < expectedTypes.Length; i++)
				{
					if ((ParamType)reader.ReadByte() != expectedTypes[i]) return null;
					switch (expectedTypes[i])
					{
						case ParamType.Int: result[i] = reader.ReadInt32(); break;
						case ParamType.Float: result[i] = reader.ReadSingle(); break;
						case ParamType.String: result[i] = ReadString(reader); break;
						case ParamType.Bool: result[i] = reader.ReadBoolean(); break;
						case ParamType.Double: result[i] = reader.ReadDouble(); break;
						default: return null;
					}
				}
				return ms.Position == ms.Length ? result : null;
			}
		}
		catch { return null; }
	}

	private void ProcessMessages() { Update(); }
	public string GetLocalIPAddress()
	{
		try
		{
			foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
				if (address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(address)) return address.ToString();
		}
		catch (SocketException) { }
		return string.Empty;
	}
	public void SendNetMessage(string message) { SendByteMessage(Encoding.UTF8.GetBytes(message ?? string.Empty)); }
	public void SendByteMessage(byte[] payload)
	{
		if (!isConnected || networkStream == null || payload == null || payload.Length > MaxFrameSize) return;
		try
		{
			lock (sync)
			{
				byte[] header = BitConverter.GetBytes(payload.Length);
				networkStream.Write(header, 0, header.Length);
				networkStream.Write(payload, 0, payload.Length);
				networkStream.Flush();
			}
		}
		catch (IOException) { CloseConnection(); }
		catch (SocketException) { CloseConnection(); }
	}

	public void StartServer(int customPort)
	{
		CloseConnection();
		try
		{
			port = customPort;
			tcpListener = new TcpListener(IPAddress.Any, port);
			tcpListener.Start(1);
			isServer = true;
			running = true;
			acceptThread = new Thread(AcceptClient) { IsBackground = true, Name = "LAN TCP accept" };
			acceptThread.Start();
		}
		catch (SocketException ex) { running = false; Debug.LogError("LAN host could not listen on port " + port + ": " + ex.Message); CloseConnection(); }
	}

	private void AcceptClient()
	{
		try
		{
			TcpClient client = tcpListener.AcceptTcpClient();
			client.NoDelay = true;
			if (!running) { client.Close(); return; }
			tcpClient = client;
			BeginReceiving(client);
			lock (sync) pendingClientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
		}
		catch (SocketException) { }
		catch (ObjectDisposedException) { }
	}

	public void ConnectToHost(string serverIP, int customPort)
	{
		StartCoroutine(ConnectCoroutine(serverIP, customPort));
	}
	private System.Collections.IEnumerator ConnectCoroutine(string serverIP, int customPort)
	{
		CloseConnection();
		TcpClient client = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true };
		IAsyncResult attempt = null;
		Exception error = null;
		try { attempt = client.BeginConnect(IPAddress.Parse(serverIP), customPort, null, null); }
		catch (Exception ex) { error = ex; }
		float started = Time.realtimeSinceStartup;
		while (attempt != null && !attempt.IsCompleted && Time.realtimeSinceStartup - started < 8f) yield return null;
		if (error == null && attempt != null && attempt.IsCompleted)
		{
			try { client.EndConnect(attempt); tcpClient = client; BeginReceiving(client); if (onConnect != null) onConnect.Invoke(serverIP); yield break; }
			catch (Exception ex) { error = ex; }
		}
		client.Close();
		if (onConnect != null) onConnect.Invoke("ERROR:" + (error == null ? "Connection timed out" : error.Message));
	}

	private void BeginReceiving(TcpClient client)
	{
		networkStream = client.GetStream();
		isConnected = true;
		receiveThread = new Thread(() => ReceiveLoop(client)) { IsBackground = true, Name = "LAN TCP receive" };
		receiveThread.Start();
	}
	private void ReceiveLoop(TcpClient client)
	{
		try
		{
			NetworkStream stream = client.GetStream();
			byte[] header = new byte[4];
			while (running || isConnected)
			{
				if (!ReadExact(stream, header, 4)) break;
				int length = BitConverter.ToInt32(header, 0);
				if (length < 1 || length > MaxFrameSize) break;
				byte[] payload = new byte[length];
				if (!ReadExact(stream, payload, length)) break;
				lock (sync) bytesQueue.Enqueue(payload);
			}
		}
		catch (IOException) { }
		catch (SocketException) { }
		finally { isConnected = false; try { client.Close(); } catch { } }
	}
	private static bool ReadExact(Stream stream, byte[] buffer, int length)
	{
		int offset = 0;
		while (offset < length) { int read = stream.Read(buffer, offset, length - offset); if (read <= 0) return false; offset += read; }
		return true;
	}
	public void HandleMessage(byte[] data) { if (data != null) lock (sync) bytesQueue.Enqueue(data); }
	public void SendMessage(NetworkStream stream, params int[] values)
	{
		if (stream == null || values == null) return;
		using (MemoryStream ms = new MemoryStream()) using (BinaryWriter writer = new BinaryWriter(ms)) { foreach (int value in values) writer.Write(value); byte[] data = ms.ToArray(); stream.Write(data, 0, data.Length); }
	}
	private void OnApplicationQuit() { CloseConnection(); }
	private void OnDestroy() { CloseConnection(); }
	public void CloseConnection()
	{
		running = false; isConnected = false;
		try { if (tcpListener != null) tcpListener.Stop(); } catch { }
		try { if (networkStream != null) networkStream.Close(); } catch { }
		try { if (tcpClient != null) tcpClient.Close(); } catch { }
		tcpListener = null; networkStream = null; tcpClient = null; isServer = false;
		lock (sync) { bytesQueue.Clear(); pendingClientAddress = null; }
	}
	private static void WriteString(BinaryWriter writer, string value) { byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty); writer.Write(bytes.Length); writer.Write(bytes); }
	private static string ReadString(BinaryReader reader) { int length = reader.ReadInt32(); if (length < 0 || length > MaxFrameSize) throw new InvalidDataException(); byte[] bytes = reader.ReadBytes(length); if (bytes.Length != length) throw new EndOfStreamException(); return Encoding.UTF8.GetString(bytes); }
}
