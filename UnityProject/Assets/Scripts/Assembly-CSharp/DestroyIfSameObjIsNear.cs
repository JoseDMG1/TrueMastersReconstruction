using System.Collections.Generic;
using UnityEngine;

public class DestroyIfSameObjIsNear : MonoBehaviour
{
	public float limitDistance = 2f;

	private void Start()
	{
		if (AttacksManager.i) DestroyNearAttack(AttacksManager.i.GetOwnAttacks(gameObject.layer));
	}

	private void DestroyNearAttack(List<GameObject> attackList)
	{
		var description = GetComponent<Description>();
		if (!description) return;
		foreach (var attack in new List<GameObject>(attackList))
		{
			if (!attack || !Utilities.i.AreObjetsNear(attack, gameObject, limitDistance)) continue;
			var other = attack.GetComponent<Description>();
			// Native 0x1227944/0x1227964 compares Description +0x40.
			if (!other || other.effect != description.effect) continue;
			if (attack == gameObject) break;
			var collision = attack.GetComponent<DestroyOnCollision>(); if (collision) collision.DestroyObject(false);
		}
	}
}
