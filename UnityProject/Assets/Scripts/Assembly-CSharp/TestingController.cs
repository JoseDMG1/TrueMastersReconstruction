using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TestingController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CLerpToZeroCoroutine_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float startValue;

		public float speed;

		public Action<float> onValueChanged;

		private float _003Cvalue_003E5__2;

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
		public _003CLerpToZeroCoroutine_003Ed__4(int _003C_003E1__state)
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
	private TMP_Text testingText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Test()
	{
	}

	[IteratorStateMachine(typeof(_003CLerpToZeroCoroutine_003Ed__4))]
	public IEnumerator LerpToZeroCoroutine(float startValue, float speed, Action<float> onValueChanged)
	{
		return null;
	}
}
