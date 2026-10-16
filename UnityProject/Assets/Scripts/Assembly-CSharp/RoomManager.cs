using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CShareCodeToFriend_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RoomManager _003C_003E4__this;

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
		public _003CShareCodeToFriend_003Ed__17(int _003C_003E1__state)
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

	public GameObject quickPlayBtn;

	public GameObject createPrivateBtn;

	public GameObject share;

	public GameObject join;

	public Text code;

	public TMP_InputField inputCode;

	public bool privateCreation;

	public Text connectionText;

	public PhotonRoom photonRoom;

	private void Start()
	{
	}

	public void ResetUI()
	{
	}

	public void PlayWithFriends()
	{
	}

	public void QuickMatch()
	{
	}

	public void ActivateQuickAndCreatePriv()
	{
	}

	public void JoinRoom()
	{
	}

	public void CreatePrivate()
	{
	}

	public void ShareCode()
	{
	}

	[IteratorStateMachine(typeof(_003CShareCodeToFriend_003Ed__17))]
	private IEnumerator ShareCodeToFriend()
	{
		return null;
	}

	public void FinishedSharing()
	{
	}
}
