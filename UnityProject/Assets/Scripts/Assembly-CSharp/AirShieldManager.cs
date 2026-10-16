using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirShieldManager : MonoBehaviour, IShield
{
	[CompilerGenerated]
	private sealed class _003CChangeSizeCoroutine_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AirShieldManager _003C_003E4__this;

		public float duration;

		public Vector3 newSize;

		private Vector3 _003CinitialSize_003E5__2;

		private float _003Celapsed_003E5__3;

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
		public _003CChangeSizeCoroutine_003Ed__6(int _003C_003E1__state)
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
	private sealed class _003CHitAnimationCoroutine_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AirShieldManager _003C_003E4__this;

		private Vector3 _003CfullSize_003E5__2;

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
		public _003CHitAnimationCoroutine_003Ed__7(int _003C_003E1__state)
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

	public GameObject ShieldExplosionPrefab;

	private void Start()
	{
	}

	public void StartShield()
	{
	}

	public void FinishShield()
	{
	}

	public void OnHit()
	{
	}

	public void ShieldBreak()
	{
	}

	[IteratorStateMachine(typeof(_003CChangeSizeCoroutine_003Ed__6))]
	private IEnumerator ChangeSizeCoroutine(Vector3 newSize, float duration)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CHitAnimationCoroutine_003Ed__7))]
	private IEnumerator HitAnimationCoroutine()
	{
		return null;
	}
}
