using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScoreController_LM : MonoBehaviour, IBattleTask
{
	[CompilerGenerated]
	private sealed class _003CEndBattleCO_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScoreController_LM _003C_003E4__this;

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
		public _003CEndBattleCO_003Ed__12(int _003C_003E1__state)
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
	private sealed class _003CPlayTimeOut_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScoreController_LM _003C_003E4__this;

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
		public _003CPlayTimeOut_003Ed__19(int _003C_003E1__state)
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

	public TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	public AnimationController_LM animationController;

	public bool battleFinish;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void UpdateScore()
	{
	}

	public void SendScore(int player0Score, int player1Score)
	{
	}

	public void GetScore(byte[] data)
	{
	}

	public void UpdateScoreUI(int player0Score, int player1Score)
	{
	}

	public void EndBattle()
	{
	}

	[IteratorStateMachine(typeof(_003CEndBattleCO_003Ed__12))]
	private IEnumerator EndBattleCO()
	{
		return null;
	}

	private void ShowFinishMenu()
	{
	}

	private GameObject GetWinner()
	{
		return null;
	}

	public bool isDraw()
	{
		return false;
	}

	public NetAnimation GetVictoryAnimation(int playerID)
	{
		return NetAnimation.Movement;
	}

	public void PlayAndSendTimeOut()
	{
	}

	public void GetTimeOut(byte[] data)
	{
	}

	[IteratorStateMachine(typeof(_003CPlayTimeOut_003Ed__19))]
	private IEnumerator PlayTimeOut()
	{
		return null;
	}
}
