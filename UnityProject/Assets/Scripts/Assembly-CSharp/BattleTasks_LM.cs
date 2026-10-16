using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleTasks_LM : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStarGameCO_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BattleTasks_LM _003C_003E4__this;

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
		public _003CStarGameCO_003Ed__18(int _003C_003E1__state)
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

	public List<IBattleTask> battleTasks;

	[SerializeField]
	private LocalMultiplayerData localData;

	[SerializeField]
	private CreateCharacters_LM createCharacters_LM;

	[SerializeField]
	private GuestInputs_LM guestInputs_LM;

	[SerializeField]
	private StartCounter_LM startCounter_LM;

	[SerializeField]
	private AnimationController_LM animationController_LM;

	[SerializeField]
	private MovePlayersInHost_LM movePlayersInHost_LM;

	[SerializeField]
	private MovePlayersInGuest_LM movePlayersInGuest_LM;

	[SerializeField]
	private DamageController damageController;

	[SerializeField]
	private AttacksController_LM attacksController_LM;

	[SerializeField]
	private FxController FxController;

	[SerializeField]
	private BlockingController blockingController_LM;

	[SerializeField]
	private Special_LM special_LM;

	[SerializeField]
	private ScoreController_LM scoreController_LM;

	[SerializeField]
	private MenuController_LM menuController_LM;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void StartGame()
	{
	}

	[IteratorStateMachine(typeof(_003CStarGameCO_003Ed__18))]
	private IEnumerator StarGameCO()
	{
		return null;
	}

	public void ResetAll()
	{
	}
}
