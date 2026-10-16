using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class IPConnection : MonoBehaviour
{
	private TcpListener tcpListener;

	private TcpClient tcpClient;

	private NetworkStream stream;

	public int port;

	private readonly Queue<string> messageQueue;

	public UnityEvent<string> onMessageReceived;

	private Thread listenerThread;

	private Thread clientThread;

	private bool isRunning;

	public void Start()
	{
	}

	public void StartServer()
	{
	}

	private void ListenForClients()
	{
	}

	private void HandleClient(TcpClient client)
	{
	}

	public void ConnectToServer(string ip)
	{
	}

	private void ListenForMessages()
	{
	}

	public void SendNetMessage(string message)
	{
	}

	private void Update()
	{
	}

	private void OnApplicationQuit()
	{
	}

	private void OnDestroy()
	{
	}

	public void CloseConnection()
	{
	}

	public void SendNetEventMessage(NetMessage netMessage)
	{
	}

	public string GetLocalIPAddress()
	{
		return null;
	}
}
