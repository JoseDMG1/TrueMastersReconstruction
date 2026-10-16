using UnityEngine;

public class DestroyByDeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if ((!GameController.i || !GameController.i.online) && other.name == "DeathZone") Destroy(gameObject);
	}
}
