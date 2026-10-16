using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController_LM : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDropAllAttacksCo_003Ed__37 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameObject atk;

		private float _003CdropSpeed_003E5__2;

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
		public _003CDropAllAttacksCo_003Ed__37(int _003C_003E1__state)
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

	public int playerID;

	public TCPConnection TCPConnection;

	public LocalMultiplayerData localData;

	public DamageController damageController;

	public AttacksController_LM attacksController;

	public Special_LM specialController;

	public ScoreController_LM scoreController;

	public bool inmune;

	public PlayerElement playerElement;

	public int moveSetIndex;

	public int specialIndex;

	public PlayerType controlledBy;

	public GameObject landAtk;

	public bool isBlocking;

	public float hp;

	public float maxHp;

	public bool stuned;

	public bool weak;

	public bool awake;

	public float playerSpecialCharge;

	public int score;

	public UnityEvent onDamage;

	public List<GameObject> playerFXList;

	public List<GameObject> landFXList;

	public bool unfrozable;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void MakeItUnfrozable()
	{
	}

	public void RestoreFrozenState()
	{
	}

	public void GetCleanFXList(byte[] data)
	{
	}

	private void CleanFXList()
	{
	}

	public void SendCleanFX()
	{
	}

	private void Update()
	{
	}

	public void ChangeMoveSet()
	{
	}

	public void ChangeSpecial()
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
	}

	public void DropAllAttacks()
	{
	}

	[IteratorStateMachine(typeof(_003CDropAllAttacksCo_003Ed__37))]
	private IEnumerator DropAllAttacksCo(GameObject atk)
	{
		return null;
	}

	public void Reset()
	{
	}

	public void Falling()
	{
	}

	private void RestoreInmune()
	{
	}
}
