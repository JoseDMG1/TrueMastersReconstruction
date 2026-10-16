using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class FriendsConnection : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public Task DBTask;

		public Task DBTask2;

		internal bool _003CSendFriendInvitationPopup_003Eb__0()
		{
			return false;
		}

		internal bool _003CSendFriendInvitationPopup_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass26_0
	{
		public Task DBTask2;

		internal bool _003CSendCodeToGuest_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSendCodeToGuest_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string code;

		private _003C_003Ec__DisplayClass26_0 _003C_003E8__1;

		public FriendsConnection _003C_003E4__this;

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
		public _003CSendCodeToGuest_003Ed__26(int _003C_003E1__state)
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
	private sealed class _003CSendFriendInvitationPopup_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		public FriendsConnection _003C_003E4__this;

		private TMInvitation _003CmyInvitation_003E5__2;

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
		public _003CSendFriendInvitationPopup_003Ed__13(int _003C_003E1__state)
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

	public static FriendsConnection i;

	public List<string> regions;

	public TMP_Text alert;

	public TMP_Text serverText;

	public TMP_Dropdown dropdown;

	public GameObject regionMenu;

	public GameObject loading;

	public CounterUI counter;

	public string hostCode;

	public string hostRegion;

	private void Awake()
	{
	}

	public void CounterDone()
	{
	}

	private void OnDestroy()
	{
	}

	[IteratorStateMachine(typeof(_003CSendFriendInvitationPopup_003Ed__13))]
	public IEnumerator SendFriendInvitationPopup()
	{
		return null;
	}

	public void Connect()
	{
	}

	public void CheckConnection()
	{
	}

	public override void OnConnectedToMaster()
	{
	}

	public void DelayJoinRoom()
	{
	}

	public override void OnRegionListReceived(RegionHandler regionHandler)
	{
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
	}

	public override void OnJoinedRoom()
	{
	}

	public void ConnectToRegion(int regionIndex)
	{
	}

	public void CreateCode()
	{
	}

	public void GuestReady()
	{
	}

	public void GuestHasDeclined()
	{
	}

	public void ConnectToHostRoom(string code)
	{
	}

	[IteratorStateMachine(typeof(_003CSendCodeToGuest_003Ed__26))]
	public IEnumerator SendCodeToGuest(string code)
	{
		return null;
	}
}
