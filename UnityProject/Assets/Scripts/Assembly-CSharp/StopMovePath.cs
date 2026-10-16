using UnityEngine;

public class StopMovePath : MonoBehaviour
{
	public GameObject owner;

	public MovePath movePath;

	private DamageOnCollision damageOwner;

	private void Start()
	{
		owner = GameController.i.GetAttackOwner(gameObject.layer);
		damageOwner = owner.GetComponent<DamageOnCollision>();
		damageOwner.onDamageRecived.AddListener(StopMove);
	}

	private void StopMove()
	{
		movePath.numberOfJumps = movePath.maxJumps;
	}

	private void OnDestroy()
	{
		// Retain the exact publisher captured at Start, even if owner is later reassigned.
		if (damageOwner != null) damageOwner.onDamageRecived.RemoveListener(StopMove);
	}

	private void Update()
	{
	}
}
