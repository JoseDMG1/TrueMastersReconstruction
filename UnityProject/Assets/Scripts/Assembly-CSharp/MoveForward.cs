using UnityEngine;

public class MoveForward : MonoBehaviour
{
	public float speed;

	private void Start()
	{
	}

	private void Update()
	{
		// Corrected native 0x1246FA0: weak motion is world-space; normal motion is local.
		if (GetComponent<ConfigAttackLayer>().playerWeak)
			transform.position += (-transform.up * 0.35f + transform.forward * 0.25f) * speed * Time.deltaTime;
		else
			transform.Translate(0f, 0f, speed * Time.deltaTime);
	}
}
