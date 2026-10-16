using UnityEngine;

public class RotateToOpponent : MonoBehaviour
{
	public GameObject target;

	private void Start()
	{
		// Native 0x12A953C aims once at launch, including the target's height.
		if (gameObject.layer == 9)
			target = GuestPlayer.guestPlayer ? GuestPlayer.guestPlayer.gameObject : null;
		else
			target = HostPlayer.hostPlayer ? HostPlayer.hostPlayer.gameObject : null;
		if (target)
			transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
	}

	private void Update()
	{
	}
}
