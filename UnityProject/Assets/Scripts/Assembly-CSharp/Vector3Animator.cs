using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Vector3Animator : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CAnimateVector3_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Vector3Animator _003C_003E4__this;

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
		public _003CAnimateVector3_003Ed__8(int _003C_003E1__state)
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

	public AnimationCurve curveX;

	public AnimationCurve curveY;

	public AnimationCurve curveZ;

	public Vector3 startValue;

	public Vector3 endValue;

	public float duration;

	private Vector3 animatedVector;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003CAnimateVector3_003Ed__8))]
	private IEnumerator AnimateVector3()
	{
		return null;
	}
}
