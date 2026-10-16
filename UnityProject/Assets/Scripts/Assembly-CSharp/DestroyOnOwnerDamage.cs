using UnityEngine;

public class DestroyOnOwnerDamage : MonoBehaviour
{
	private GameObject owner;

	private void Start()
	{
		owner = GameController.i.GetAttackOwner(gameObject.layer);
		var damage = owner.GetComponent<DamageOnCollision>();
		if (damage && damage.onDamageRecived != null) damage.onDamageRecived.AddListener(DestroyByHit);
	}

	public void DestroyByHit()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (!owner) return;
		var damage = owner.GetComponent<DamageOnCollision>();
		if (damage && damage.onDamageRecived != null) damage.onDamageRecived.RemoveListener(DestroyByHit);
	}
}
