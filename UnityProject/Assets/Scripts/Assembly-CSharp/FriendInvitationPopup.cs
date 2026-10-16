using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FriendInvitationPopup : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Task DBTask;

		public Task DBTask2;

		internal bool _003CGuestDeclineInvitation_003Eb__0()
		{
			return false;
		}

		internal bool _003CGuestDeclineInvitation_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Task DBTask;

		public Task DBTask2;

		internal bool _003CUpdateGuestReady_003Eb__0()
		{
			return false;
		}

		internal bool _003CUpdateGuestReady_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGuestDeclineInvitation_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendInvitationPopup _003C_003E4__this;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

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
		public _003CGuestDeclineInvitation_003Ed__10(int _003C_003E1__state)
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
	private sealed class _003CUpdateGuestReady_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendInvitationPopup _003C_003E4__this;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

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
		public _003CUpdateGuestReady_003Ed__9(int _003C_003E1__state)
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

	public static FriendInvitationPopup i;

	public TMP_Text friendName;

	public string hostID;

	public GameObject boxPopup;

	private void Start()
	{
	}

	public void OpenInvitation(string username, string userID)
	{
	}

	public void AcceptInvitation()
	{
	}

	public void DeclineInvitation()
	{
	}

	public void CloseInvitation()
	{
	}

	[IteratorStateMachine(typeof(_003CUpdateGuestReady_003Ed__9))]
	public IEnumerator UpdateGuestReady()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CGuestDeclineInvitation_003Ed__10))]
	public IEnumerator GuestDeclineInvitation()
	{
		return null;
	}
}
