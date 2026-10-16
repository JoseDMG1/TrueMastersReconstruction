using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TCP_Timer : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStartLagTimer_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TCP_Timer _003C_003E4__this;

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
		public _003CStartLagTimer_003Ed__16(int _003C_003E1__state)
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
	private LocalMultiplayerData localData;

	[SerializeField]
	private TCPConnection TCPConnection;

	public float timer;

	private float lagTimer;

	private float lag;

	private bool firstTimerCall;

	private bool secondTimerCall;

	private bool startTimer;

	public UnityEvent<float> onTimerUpdated;

	private void Start()
	{
	}

	private void initHostActions()
	{
	}

	private void OnDestroy()
	{
	}

	private void SendSecondTimer()
	{
	}

	private void Update()
	{
	}

	public void SendTimerToGuest()
	{
	}

	private void ReceiveTimerFromHost(byte[] timerBytes)
	{
	}

	[IteratorStateMachine(typeof(_003CStartLagTimer_003Ed__16))]
	private IEnumerator StartLagTimer()
	{
		return null;
	}
}
