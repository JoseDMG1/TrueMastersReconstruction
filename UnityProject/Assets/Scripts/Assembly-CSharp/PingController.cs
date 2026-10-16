using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PingController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CPingIP_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PingController _003C_003E4__this;

		public string ip;

		private Ping _003Cping_003E5__2;

		private float _003Ctimeout_003E5__3;

		private float _003Ctimer_003E5__4;

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
		public _003CPingIP_003Ed__19(int _003C_003E1__state)
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

	public TMP_Text localIPText;

	public TMP_InputField inputIP;

	public TMP_InputField inputPort;

	public TMP_Text resultText;

	public TCPConnection tcpConnection;

	public UDPConnectionManager udpConnection;

	private void Start()
	{
	}

	public void Ping()
	{
	}

	public void TCPConnect()
	{
	}

	public void TCPStart()
	{
	}

	public void TCPSend()
	{
	}

	private void GetTCPMessage(byte[] data)
	{
	}

	private void GetUDPMessage(byte[] data)
	{
	}

	public void UDPRestart()
	{
	}

	public void UDPSend()
	{
	}

	public void UDP_BroadCast()
	{
	}

	public void UDPConnect()
	{
	}

	public void CloseConnections()
	{
	}

	private string GetLocalIP()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CPingIP_003Ed__19))]
	private IEnumerator PingIP(string ip)
	{
		return null;
	}
}
