using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

public class UDPConnectionManager : MonoBehaviour
{
	private UdpClient udpClient;

	private IPEndPoint remoteEndPoint;

	public int port;

	private readonly Queue<byte[]> byteQueue;

	public UnityEvent<byte[]> onRawDataReceived;

	public string targetIP;

	public string lastClientIP;

	private void Start()
	{
	}

	public void EnableMulticast()
	{
	}

	public void BroadCast(int customPort)
	{
	}

	public void StartServer(int customPort)
	{
	}

	private void Update()
	{
	}

	public void SendRawData(byte[] data)
	{
	}

	private void ListenForMessages()
	{
	}

	private void OnMessageReceived(IAsyncResult result)
	{
	}

	public byte[] StructToBytes(NetMessage netMessage)
	{
		return null;
	}

	public string GetLocalIPAddress()
	{
		return null;
	}

	private string GetBroadcastAddress()
	{
		return null;
	}

	private void OnApplicationQuit()
	{
	}

	private void OnDestroy()
	{
	}

	public void ChangeRemoteEndPoint(string ip, int customPort)
	{
	}

	public void CloseConnection()
	{
	}
}
