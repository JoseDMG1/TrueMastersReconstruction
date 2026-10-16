using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FallMovement : MonoBehaviour
{
	public enum Mode
	{
		forward = 0,
		up = 1
	}

	[CompilerGenerated]
	private sealed class _003CAnimateSpeed_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FallMovement _003C_003E4__this;

		private float _003CelapsedTime_003E5__2;

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
		public _003CAnimateSpeed_003Ed__8(int _003C_003E1__state)
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

	public AnimationCurve speedCurve;

	public float maxSpeed;

	public float duration;

	private float speed;

	public Mode mode;

	public void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CAnimateSpeed_003Ed__8))]
	private IEnumerator AnimateSpeed()
	{
		return null;
	}
}
