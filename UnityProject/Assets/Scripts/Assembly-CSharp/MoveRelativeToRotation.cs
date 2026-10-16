using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveRelativeToRotation : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMoveToPosition_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MoveRelativeToRotation _003C_003E4__this;

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
		public _003CMoveToPosition_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			MoveRelativeToRotation owner = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new WaitForSeconds(owner.delay);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003CelapsedTime_003E5__2 = 0f;
					break;
				case 2:
					break;
				default:
					return false;
			}
			_003C_003E1__state = -1;
			if (_003CelapsedTime_003E5__2 < owner.moveDuration)
			{
				_003CelapsedTime_003E5__2 += Time.deltaTime;
				owner.transform.position = Vector3.Lerp(owner.startPosition, owner.finalPosition, _003CelapsedTime_003E5__2 / owner.moveDuration);
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			owner.transform.position = owner.finalPosition;
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

	public Vector3 targetOffset;

	public float moveDuration = 1f;

	public float delay;

	private Vector3 startPosition;

	private Vector3 finalPosition;

	private void Start()
	{
		startPosition = transform.position;
		finalPosition = startPosition + transform.TransformDirection(targetOffset);
		StartCoroutine(MoveToPosition());
	}

	[IteratorStateMachine(typeof(_003CMoveToPosition_003Ed__6))]
	private IEnumerator MoveToPosition()
	{
		return new _003CMoveToPosition_003Ed__6(0) { _003C_003E4__this = this };
	}
}
