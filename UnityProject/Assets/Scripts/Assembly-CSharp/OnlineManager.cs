using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;

public class OnlineManager : MonoBehaviourPunCallbacks, IInRoomCallbacks, ILobbyCallbacks
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass30_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CCheckIfHaveFriend_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CCheckIfAlreadyFriends_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass32_0
	{
		public Task DBTask;

		internal bool _003CSetOnlineCode_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCheckIfAlreadyFriends_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string friendID;

		private _003C_003Ec__DisplayClass31_0 _003C_003E8__1;

		public OnlineManager _003C_003E4__this;

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
		public _003CCheckIfAlreadyFriends_003Ed__31(int _003C_003E1__state)
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
	private sealed class _003CCheckIfHaveFriend_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string friendID;

		private _003C_003Ec__DisplayClass30_0 _003C_003E8__1;

		public OnlineManager _003C_003E4__this;

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
		public _003CCheckIfHaveFriend_003Ed__30(int _003C_003E1__state)
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
	private sealed class _003CSetOnlineCode_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string code;

		private _003C_003Ec__DisplayClass32_0 _003C_003E8__1;

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
		public _003CSetOnlineCode_003Ed__32(int _003C_003E1__state)
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

	public static OnlineManager i;

	public bool hostReady;

	public bool guestReady;

	public List<string> regions;

	public string OpUsername;

	public string OpUserID;

	public bool alreadyFriends;

	public string spectatorCode;

	public List<RoomInfo> rooms;

	private void Start()
	{
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
	}

	[PunRPC]
	public void SetGuestReady(int stage)
	{
	}

	public override void OnRegionListReceived(RegionHandler regionHandler)
	{
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
	}

	public void SetPlayerScene(int scene)
	{
	}

	public bool ArePlayersSameScene(int scene)
	{
		return false;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	public void SetRoomPropAsString(string propertie, string value)
	{
	}

	public void SetRoomPropAsInt(string propertie, int value)
	{
	}

	public void SetRoomPropAsFloat(string propertie, float value)
	{
	}

	public string GetRoomPropAsString(string propertie)
	{
		return null;
	}

	public int GetRoomPropAsInt(string propertie)
	{
		return 0;
	}

	public float GetRoomPropAsFloat(string propertie)
	{
		return 0f;
	}

	public string GetPlayerPropAsString(Player player, string propertie)
	{
		return null;
	}

	public int GetPlayerPropAsInt(Player player, string propertie)
	{
		return 0;
	}

	public float GetPlayerPropAsFloat(Player player, string propertie)
	{
		return 0f;
	}

	public void SetPlayerPropAsString(Player player, string propertie, string value)
	{
	}

	public void SetPlayerPropAsInt(Player player, string propertie, int value)
	{
	}

	public void SetPlayerPropAsFloat(Player player, string propertie, float value)
	{
	}

	public void CleanConnection()
	{
	}

	[IteratorStateMachine(typeof(_003CCheckIfHaveFriend_003Ed__30))]
	public IEnumerator CheckIfHaveFriend(string friendID)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CCheckIfAlreadyFriends_003Ed__31))]
	public IEnumerator CheckIfAlreadyFriends(string friendID)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CSetOnlineCode_003Ed__32))]
	public IEnumerator SetOnlineCode(string code)
	{
		return null;
	}

	public bool IsPlayer1()
	{
		return false;
	}

	public bool IsPlayer2()
	{
		return false;
	}

	public bool IsSpectator()
	{
		return false;
	}

	public string GetOnlineCode()
	{
		return null;
	}

	public int GetPlayerNumber(Player player)
	{
		return 0;
	}
}
