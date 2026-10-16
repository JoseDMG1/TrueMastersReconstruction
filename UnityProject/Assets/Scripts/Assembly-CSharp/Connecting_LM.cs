using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Connecting_LM : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStartGuestConnection_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Connecting_LM _003C_003E4__this;

		public string friendsIP;

		public int port;

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
		public _003CStartGuestConnection_003Ed__21(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CStartHostConnectionCO_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Connecting_LM _003C_003E4__this;

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
		public _003CStartHostConnectionCO_003Ed__27(int _003C_003E1__state)
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

	public TMP_Text ping_text;

	public TMP_Text tcp_text;

	public TMP_Text udp_text;

	public TMP_Text attemps;

	public int numberOfAttemps;

	public TMP_Text ip_text;

	public TMP_Text ClientType;

	public UDPConnectionManager udpConnection;

	public TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	public SyncTimers syncTimers;

	public string friendIP;

	private bool PingDone;

	private bool TCPHostReceived;

	private bool UDPGuestReceived;

	private bool UDPHostReceived;

	private bool TCPGuestReceived;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void JoinGame(string friendsIPLastPart, int port)
	{
	}

	private string GetLocalIP()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CStartGuestConnection_003Ed__21))]
	private IEnumerator StartGuestConnection(string friendsIP, int port)
	{
		return null;
	}

	public void ConnectionResults(string result)
	{
	}

	public void StartServerHost(int port)
	{
	}

	public void StartServerGuest(int port)
	{
	}

	public void StartHostConnection(string guestIP)
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CStartHostConnectionCO_003Ed__27))]
	public IEnumerator StartHostConnectionCO()
	{
		return null;
	}

	public string GetFullIPFromLastNumbers(string lastPart)
	{
		return null;
	}

	public void SendSyncTimers()
	{
	}

	public void GetSyncTimers(byte[] data)
	{
	}

	private string GetLastNumberOfIP(string ip)
	{
		return null;
	}

	public void SendTCPMsg(bool host)
	{
	}

	public void GetTCPMsg(byte[] data)
	{
	}

	public void Close()
	{
	}

	public void CleanUI()
	{
	}

	public void SendUDPMsg(bool host)
	{
	}

	public void GetUDPMsg(byte[] data)
	{
	}

	public void ConnectionTimeOut()
	{
	}
}
