using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMoveObjectToTarget_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MoveToTarget _003C_003E4__this;

		private float _003CelapsedTime_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMoveObjectToTarget_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state == 0)
			{
				_003CelapsedTime_003E5__2 = 0f;
			}
			else if (_003C_003E1__state != 1)
			{
				return false;
			}
			_003C_003E1__state = -1;
			MoveToTarget owner = _003C_003E4__this;
			if (!(_003CelapsedTime_003E5__2 < owner.moveDuration))
			{
				return false;
			}
			_003CelapsedTime_003E5__2 += Time.deltaTime;
			float progress = _003CelapsedTime_003E5__2 / owner.moveDuration;
			float x = owner.xCurve.Evaluate(progress) * owner.XForce;
			float y = owner.yCurve.Evaluate(progress) * owner.YForce;
			float z = owner.zCurve.Evaluate(progress) * owner.ZForce;
			owner.transform.position = owner.startPosition + owner.transform.TransformDirection(new Vector3(x, y, z));
			_003C_003E2__current = null;
			_003C_003E1__state = 1;
			return true;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public float moveDuration = 2f;

	public AnimationCurve xCurve;

	public AnimationCurve yCurve;

	public AnimationCurve zCurve;

	public float YForce;

	public float XForce;

	public float ZForce;

	private Vector3 startPosition;

	private void Start()
	{
		startPosition = transform.position;
		StartCoroutine(MoveObjectToTarget());
	}

	[IteratorStateMachine(typeof(_003CMoveObjectToTarget_003Ed__9))]
	private IEnumerator MoveObjectToTarget()
	{
		return new _003CMoveObjectToTarget_003Ed__9(0) { _003C_003E4__this = this };
	}
}
