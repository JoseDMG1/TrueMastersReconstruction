using Photon.Pun;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
	[SerializeField]
	public float delay;

	public bool onlySummonExplotion;

	public bool stopAtkFirst;

	public bool exceptPlayer;

	public bool byTheSameObject;

	public GameObject explotion;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (GameController.i && GameController.i.online) return;
		if (!other) return;
		var incoming = other.GetComponent<Description>();
		var own = GetComponent<Description>();
		if (!incoming || !own) return;
		// Native 0x128A254 reads Description +0x40 (effect); usage
		// 0x28168C8 resolves ShadowTramp. No layer test precedes this branch.
		if (byTheSameObject && incoming.effect == "ShadowTramp") Destroy(gameObject);
		if (exceptPlayer && incoming.Type == "Char") return;
		if (incoming.category == "shieldBreaker" && own.category == "waterShield")
		{
			var box = GetComponent<BoxCollider>(); if (box) box.enabled = false;
			if (!onlySummonExplotion) StopAttack();
			return;
		}
		if (incoming.Damage <= own.Defense) return;
		if (!onlySummonExplotion) StopAttack();
	}

	[PunRPC]
	public void DestroyObject(bool ignoreSumonOnCollider)
	{
		var summon = GetComponent<SummonOnCollision>();
		if (summon) summon.NetworkSummon(ignoreSumonOnCollider);
		if (!onlySummonExplotion) StopAttack();
	}

	[PunRPC]
	public void DisableShield()
	{
		var box = GetComponent<BoxCollider>(); if (box) box.enabled = false;
		var summon = GetComponent<SummonOnCollision>(); if (summon) summon.NetworkSummon(false);
		if (!onlySummonExplotion) StopAttack();
	}

	public void DeleteObj()
	{
		if (explotion) Instantiate(explotion, transform.position, transform.rotation);
		if (GameController.i && GameController.i.online)
		{
			Debug.LogWarning("Network projectile deletion is unavailable.", this); return;
		}
		Destroy(gameObject, delay);
	}

	public void PhotonDestroy()
	{
		if (GameController.i && GameController.i.online)
		{
			Debug.LogWarning("Network projectile deletion is unavailable.", this); return;
		}
		Destroy(gameObject);
	}

	public void StopAttack()
	{
		if (!stopAtkFirst) { DeleteObj(); return; }
		foreach (var particles in GetComponentsInChildren<ParticleSystem>()) particles.Stop();
		var movement = GetComponent<MoveAttack>(); if (movement) movement.enabled = false;
		Invoke(nameof(DeleteObj), 3f);
	}
}
