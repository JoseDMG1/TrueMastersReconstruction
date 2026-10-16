using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class SyncTimers : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CGetTimerOffsetRoutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SyncTimers _003C_003E4__this;

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
		public _003CGetTimerOffsetRoutine_003Ed__16(int _003C_003E1__state)
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
	public LocalMultiplayerData localData;

	private float t1_guest;

	private float t2_host;

	private float t3_guest;

	private float t4_host;

	public float timeOffset;

	public UnityEvent<float> onTimeOffsetCalculated;

	public float timerOffsetAvg;

	private List<float> offsetSamples;

	private int offsetSampleCount;

	private int currentSampleIndex;

	private bool waitingSampleComplete;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void GetAvgTimerOffset()
	{
	}

	[IteratorStateMachine(typeof(_003CGetTimerOffsetRoutine_003Ed__16))]
	private IEnumerator GetTimerOffsetRoutine()
	{
		return null;
	}

	public void GetTimerOffset()
	{
	}

	public void SendTime(NetFastEvent timeType)
	{
	}

	public float GetHostTime()
	{
		return 0f;
	}

	private void OnTimeReceived(byte[] message)
	{
	}

	private void CalculateTimeOffset()
	{
	}

	private void SendOffsetTimerToHost()
	{
	}
}
