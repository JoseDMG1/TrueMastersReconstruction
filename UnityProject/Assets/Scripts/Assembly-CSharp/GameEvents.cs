using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	public static GameEvents i;

	public event Action<EntityId> onAttackLayerSetUp;

	public event Action<int> OnPlayerDamaged;

	public event Action<int> OnPlayerPrisonHit;

	public event Action<int> OnPlayerPrisonTrap;

	public event Action<EntityId> onSummonedInOpp;

	private void Start()
	{
		// Native VA 0x1236230: a duplicate must not replace the event owner.
		if (i == null) i = this;
		else if (i != this)
		{
			Debug.Log("Instance already exists, destroying object!");
			Destroy(this);
		}
	}

	private void OnDestroy() { if (i == this) i = null; }

	public void AttackLayerSetUp(EntityId atkID) { onAttackLayerSetUp?.Invoke(atkID); }

	public void PlayerDamaged(int layer) { OnPlayerDamaged?.Invoke(layer); }

	public void PlayerPrisonHit(int layer) { OnPlayerPrisonHit?.Invoke(layer); }

	public void PlayerPrisonTrap(int layer) { OnPlayerPrisonTrap?.Invoke(layer); }

	public void SummonedInOpp(EntityId atkID) { onSummonedInOpp?.Invoke(atkID); }
}
