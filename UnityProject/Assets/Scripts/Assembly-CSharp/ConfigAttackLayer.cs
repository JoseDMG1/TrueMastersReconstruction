using UnityEngine;

public class ConfigAttackLayer : MonoBehaviour
{
	public bool playerWeak;

	private void Awake()
	{
		// Offline creators assign the owner layer after Instantiate. Native Update is RET.
		if (GameController.i && GameController.i.online)
			Debug.LogWarning("Network projectile ownership has not been reconstructed.", this);
	}

	public void DestroyAttack()
	{
		if (GameController.i && GameController.i.online) return;
		var summon = GetComponent<SummonOnCollision>();
		if (summon) summon.Summon(GetComponent<Collider>());
		Destroy(gameObject);
	}

	private void Update()
	{
	}
}
