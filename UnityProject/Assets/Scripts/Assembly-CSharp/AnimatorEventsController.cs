using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimatorEventsController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CLerpToZeroInTimeCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float totalTime;

		public float startSpeed;

		public Action<float> onValueChanged;

		private float _003Celapsed_003E5__2;

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
		public _003CLerpToZeroInTimeCoroutine_003Ed__9(int _003C_003E1__state)
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

	public PlayerController_LM playerController;

	public Coroutine verticalCoroutine;

	private Coroutine horizontalCoroutine;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	public void Restore(int playerLayer)
	{
	}

	public void CreateFX(GameObject creation)
	{
	}

	public void CreatePlayerFX(GameObject creation)
	{
	}

	[IteratorStateMachine(typeof(_003CLerpToZeroInTimeCoroutine_003Ed__9))]
	public IEnumerator LerpToZeroInTimeCoroutine(float startSpeed, float totalTime, Action<float> onValueChanged)
	{
		return null;
	}

	public void GoHorizontal(string initialSpeedAndTime)
	{
	}

	public void GoVertical(string initialSpeedAndTime)
	{
	}

	public void reduceCollider()
	{
	}

	public void recoverCollider()
	{
	}

	public void StopJump()
	{
	}

	public void StartAwakening()
	{
	}

	public void StartSpecial()
	{
	}

	public void CreateAttack(GameObject attackPrefab)
	{
	}

	public void NoDelay()
	{
	}

	public void CreateSideAttack(GameObject attackPrefab)
	{
	}

	public void CreateLandAtk()
	{
	}
}
