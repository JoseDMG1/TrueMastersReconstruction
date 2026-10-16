using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveObjectLocal2 : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMoveOverTime_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MoveObjectLocal2 _003C_003E4__this;

		private float _003CtimeElapsed_003E5__2;

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
		public _003CMoveOverTime_003Ed__7(int _003C_003E1__state)
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
				_003CtimeElapsed_003E5__2 = 0f;
			else if (_003C_003E1__state != 1)
				return false;
			_003C_003E1__state = -1;
			MoveObjectLocal2 owner = _003C_003E4__this;
			if (_003CtimeElapsed_003E5__2 < owner.duration)
			{
				_003CtimeElapsed_003E5__2 += Time.deltaTime;
				owner.gameObject.transform.localPosition = Vector3.Lerp(owner.startPosition, owner.positionEnd, Mathf.Clamp01(_003CtimeElapsed_003E5__2 / owner.duration));
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			owner.gameObject.transform.localPosition = owner.positionEnd;
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
			throw new NotSupportedException();
		}
	}

	public Vector3 AddVector;

	public float duration = 1f;

	private Vector3 startPosition;

	private Vector3 positionEnd;

	public float delay;

	private void Start()
	{
		Invoke("StartMoving", delay);
	}

	private void StartMoving()
	{
		startPosition = gameObject.transform.localPosition;
		positionEnd = startPosition + AddVector;
		StartCoroutine(MoveOverTime());
	}

	[IteratorStateMachine(typeof(_003CMoveOverTime_003Ed__7))]
	private IEnumerator MoveOverTime()
	{
		return new _003CMoveOverTime_003Ed__7(0) { _003C_003E4__this = this };
	}
}
