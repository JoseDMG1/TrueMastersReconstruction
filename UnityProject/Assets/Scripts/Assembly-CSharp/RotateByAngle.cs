using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RotateByAngle : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CRotateAfterDelay_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RotateByAngle _003C_003E4__this;

		private float _003CtimeElapsed_003E5__2;

		private Quaternion _003CstartRotation_003E5__3;

		private Quaternion _003CendRotation_003E5__4;

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
		public _003CRotateAfterDelay_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			RotateByAngle owner = _003C_003E4__this;
			switch (_003C_003E1__state)
			{
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new WaitForSeconds(owner.delay);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003CtimeElapsed_003E5__2 = 0f;
					_003CstartRotation_003E5__3 = owner.transform.localRotation;
					_003CendRotation_003E5__4 = _003CstartRotation_003E5__3 * Quaternion.Euler(owner.targetRotation);
					break;
				case 2:
					_003C_003E1__state = -1;
					break;
				default:
					return false;
			}
			if (_003CtimeElapsed_003E5__2 < owner.duration)
			{
				_003CtimeElapsed_003E5__2 += Time.deltaTime;
				float progress = Mathf.Clamp01(_003CtimeElapsed_003E5__2 / owner.duration);
				owner.transform.localRotation = Quaternion.Slerp(_003CstartRotation_003E5__3, _003CendRotation_003E5__4, progress);
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			owner.transform.localRotation = _003CendRotation_003E5__4;
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

	public Vector3 targetRotation = new Vector3(0f, 90f, 0f);

	public float duration = 1f;

	public float delay = 1f;

	private void Start()
	{
		StartCoroutine(RotateAfterDelay());
	}

	[IteratorStateMachine(typeof(_003CRotateAfterDelay_003Ed__4))]
	private IEnumerator RotateAfterDelay()
	{
		return new _003CRotateAfterDelay_003Ed__4(0) { _003C_003E4__this = this };
	}
}
