using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CreateRoomManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CManualBroadCastCO_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CreateRoomManager _003C_003E4__this;

		private string _003Csubnet_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CManualBroadCastCO_003Ed__11(int _003C_003E1__state)
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

	public TMP_InputField roomNameInput;

	public TMP_InputField passwordInput;

	public LocalMultiplayerManager UIManager;

	public UDPConnectionManager udpManager;

	private string currentRoomName;

	private string currentPassword;

	public bool broadCastRoom;

	private void Start()
	{
	}

	public void Create()
	{
	}

	private bool ValidateFields()
	{
		return false;
	}

	public void ManualBroadCast()
	{
	}

	[IteratorStateMachine(typeof(_003CManualBroadCastCO_003Ed__11))]
	private IEnumerator ManualBroadCastCO()
	{
		return null;
	}

	private void BroadcastRoomInfo()
	{
	}

	public void Test()
	{
	}

	public void CloseConnection()
	{
	}
}
