using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StartCounter_LM : MonoBehaviour, IBattleTask
{
	[CompilerGenerated]
	private sealed class _003CPrepareCounter_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StartCounter_LM _003C_003E4__this;

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
		public _003CPrepareCounter_003Ed__6(int _003C_003E1__state)
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
	private sealed class _003CStartCounterRoutine_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StartCounter_LM _003C_003E4__this;

		public float startCounterTime;

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
		public _003CStartCounterRoutine_003Ed__7(int _003C_003E1__state)
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

	[SerializeField]
	private TCPConnection TCPConnection;

	[SerializeField]
	private SyncTimers syncTimers;

	[SerializeField]
	private LocalMultiplayerData localData;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	[IteratorStateMachine(typeof(_003CPrepareCounter_003Ed__6))]
	private IEnumerator PrepareCounter()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CStartCounterRoutine_003Ed__7))]
	private IEnumerator StartCounterRoutine(float startCounterTime)
	{
		return null;
	}

	private void GetStartCounter(byte[] startCounterBytes)
	{
	}

	public void StartCounter()
	{
	}

	private void SendStartCounterToGuest(float startCounterTime)
	{
	}
}
