using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DestroyOnCollision_LM : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDestroyCO_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DestroyOnCollision_LM _003C_003E4__this;

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
		public _003CDestroyCO_003Ed__5(int _003C_003E1__state)
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

	[Tooltip("Seconds to wait before destroying after collision")]
	public float destroyDelay;

	private AttacksController_LM attacksController;

	private AtkDescription atkDescription;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
	}

	[IteratorStateMachine(typeof(_003CDestroyCO_003Ed__5))]
	private IEnumerator DestroyCO()
	{
		return null;
	}
}
