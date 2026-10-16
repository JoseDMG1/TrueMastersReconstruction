using UnityEngine;

public class DestroyAtkSimple : MonoBehaviour
{
	public GameObject explosion;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Attack") && other.gameObject.layer != gameObject.layer)
		{
			var incoming = other.GetComponent<AtkDescription>();
			var own = GetComponent<AtkDescription>();
			if (incoming && own && incoming.damage > own.hp) DestroyAtk();
		}
		if (other.gameObject.layer == (gameObject.layer == 11 ? 8 : 9)) DestroyAtk();
	}

	private void DestroyAtk()
	{
		if (explosion) Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
