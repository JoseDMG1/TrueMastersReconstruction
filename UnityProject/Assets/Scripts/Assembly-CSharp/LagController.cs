using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class LagController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CLagPulseGuestRoutine_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LagController _003C_003E4__this;

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
		public _003CLagPulseGuestRoutine_003Ed__22(int _003C_003E1__state)
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
	private sealed class _003CLagPulseHostRoutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LagController _003C_003E4__this;

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
		public _003CLagPulseHostRoutine_003Ed__16(int _003C_003E1__state)
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

	public float currentLag;

	public float avgLag;

	public float guestTimeWhenFirstPulse;

	public float hostTimeWhenFirstPulse;

	public float hostTimeWhenSecondPulse;

	private bool receivedFirstPulse;

	private List<float> lagSamples;

	private float lagTimer;

	private bool secondLagPulse;

	[SerializeField]
	private TCPConnection TCPConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	private bool activeLagCoroutine;

	public UnityEvent<float, float> onLagDataUpdated;

	private int lagSamplesCount;

	private void Start()
	{
	}

	public void Test()
	{
	}

	[IteratorStateMachine(typeof(_003CLagPulseHostRoutine_003Ed__16))]
	private IEnumerator LagPulseHostRoutine()
	{
		return null;
	}

	private void SendLagPulseToGuest()
	{
	}

	private void SendSecondLagPulseToGuest()
	{
	}

	private void OnLagDataReceived(byte[] message)
	{
	}

	private void OnLagPulseReceived(byte[] message)
	{
	}

	private void SendLagDataToHost()
	{
	}

	[IteratorStateMachine(typeof(_003CLagPulseGuestRoutine_003Ed__22))]
	private IEnumerator LagPulseGuestRoutine()
	{
		return null;
	}

	private float CalculateAverageLag()
	{
		return 0f;
	}
}
