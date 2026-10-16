using UnityEngine;

public class MoveLight : MonoBehaviour
{
	public float speed;

	private bool ready;

	public bool playerWeak;

	private Vector3 dir;

	public float timeToStart;

	public bool moving;

	public float position;

	public bool GoToOpponent = true;

	private void Start()
	{
		transform.position += transform.forward * position;
		Invoke(nameof(StartMov), timeToStart);
	}

	private void FixedUpdate()
	{
		// Native 0x129BA90 uses fixed-tick displacement, including the weak branch.
		if (GetComponent<ConfigAttackLayer>().playerWeak)
		{
			transform.position += (-transform.up + transform.forward * .1f) * speed;
			return;
		}
		if (!moving) return;
		ChangeDirection();
		ready = true;
		transform.position += transform.forward * speed;
	}

	private void StartMov()
	{
		moving = true;
	}

	private void ChangeDirection()
	{
		if (!GoToOpponent || ready) return;
		bool host = gameObject.layer == 9;
		var target = host ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		dir = (target.position - transform.position).normalized;
		// Native computes dir but moves along forward after copying the owner's rotation.
		transform.rotation = (host ? HostPlayer.hostPlayer.transform : GuestPlayer.guestPlayer.transform).rotation;
		dir.y = 0f;
	}
}
