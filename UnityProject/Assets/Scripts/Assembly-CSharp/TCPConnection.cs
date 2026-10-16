using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TCPConnection : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass30_0
	{
		public TCPConnection _003C_003E4__this;

		public bool success;

		public Exception error;

		internal void _003CConnectCoroutine_003Eb__0(IAsyncResult ar)
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CConnectCoroutine_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TCPConnection _003C_003E4__this;

		public string serverIP;

		public int port;

		private _003C_003Ec__DisplayClass30_0 _003C_003E8__1;

		private float _003Ctimeout_003E5__2;

		private float _003Ctimer_003E5__3;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CConnectCoroutine_003Ed__30(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public LocalMultiplayerData localData;

	private TcpListener tcpListener;

	private TcpClient tcpClient;

	private NetworkStream networkStream;

	private byte[] buffer;

	private List<byte> accumulator;

	private bool isServer;

	private bool isConnected;

	private readonly Queue<byte[]> bytesQueue;

	public UnityEvent<byte[]> onMessageReceived;

	public UnityEvent<string> onConnect;

	public UnityEvent<string> onClientConnected;

	public int port;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void Init()
	{
	}

	private void Update()
	{
	}

	public byte[] CreateMessage(NetFastEvent netEvent, params object[] parameters)
	{
		return null;
	}

	public void CreateAndSendMessage(NetFastEvent netEvent, params object[] parameters)
	{
	}

	public void SendShortMessage(NetFastEvent netEvent)
	{
	}

	public bool ParseShortMessage(byte[] data, NetFastEvent expectedEvent)
	{
		return false;
	}

	public object[] ParseMessage(byte[] data, NetFastEvent expectedEvent, params ParamType[] expectedTypes)
	{
		return null;
	}

	private void ProcessMessages()
	{
	}

	public string GetLocalIPAddress()
	{
		return null;
	}

	public void SendNetMessage(string message)
	{
	}

	public void SendByteMessage(byte[] payload)
	{
	}

	public void StartServer(int customPort)
	{
	}

	public void ConnectToHost(string serverIP, int port)
	{
	}

	public void HandleMessage(byte[] data)
	{
	}

	public void SendMessage(NetworkStream stream, params int[] values)
	{
	}

	[IteratorStateMachine(typeof(_003CConnectCoroutine_003Ed__30))]
	private IEnumerator ConnectCoroutine(string serverIP, int port)
	{
		return null;
	}

	private void OnApplicationQuit()
	{
	}

	public void CloseConnection()
	{
	}
}
