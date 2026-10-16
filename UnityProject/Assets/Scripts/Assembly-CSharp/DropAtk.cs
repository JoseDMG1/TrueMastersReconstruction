using UnityEngine;

public class DropAtk : MonoBehaviour
{
	public float speed = .8f;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		// Native 0x128AB9C drops only when the attack owner's weak flag is set.
		var config = GetComponent<ConfigAttackLayer>();
		if (config && config.playerWeak)
			transform.position -= transform.up * speed * .25f;
	}
}
