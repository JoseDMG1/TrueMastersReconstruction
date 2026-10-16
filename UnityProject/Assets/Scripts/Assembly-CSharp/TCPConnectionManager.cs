using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TCPConnectionManager : MonoBehaviour
{
	private TcpListener tcpListener;

	private TcpClient tcpClient;

	private NetworkStream stream;

	private Thread listenerThread;

	private Thread clientThread;

	private readonly Queue<string> messageQueue;

	public UnityEvent<string> onMessageReceived;

	public int port;

	private void Start()
	{
	}

	private void Update()
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

	public void ConnectToServer(string serverIp)
	{
	}

	private void ReceiveMessages()
	{
	}

	public void SendNetMessage(string message)
	{
	}

	private void OnDestroy()
	{
	}
}
