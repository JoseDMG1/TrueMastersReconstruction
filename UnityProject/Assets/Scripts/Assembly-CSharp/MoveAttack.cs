using UnityEngine;

public class MoveAttack : MonoBehaviour
{
	public float speed;

	public float speedFistMove;

	private bool ready;

	public bool playerWeak;

	private Vector3 dir;

	public float timeToStart;

	private bool moving;

	public Vector3 StartPosition;

	public Vector3 EndPosition;

	private Vector3 EndPosition2;

	public bool changeDirectionAtStart;

	public float startRotation;

	private void Start()
	{
		transform.position += transform.forward * StartPosition.x + transform.up * StartPosition.y + transform.right * StartPosition.z;
		EndPosition2 = transform.position + transform.forward * EndPosition.x + transform.up * EndPosition.y + transform.right * EndPosition.z;
		Invoke(nameof(StartMov), timeToStart);
	}

	private void FixedUpdate()
	{
		if (GetComponent<ConfigAttackLayer>().playerWeak)
			transform.position = transform.position - transform.up * speed * 0.35f + transform.right * speed * 0.1f;
		else if (moving)
		{
			if (changeDirectionAtStart) ChangeDirection();
			ready = true;
			transform.position += transform.right * speed;
		}
		else transform.position = Vector3.Lerp(transform.position, EndPosition2, speedFistMove);
	}

	private void StartMov()
	{
		moving = true;
	}

	private void ChangeDirection()
	{
		if (ready) return;
		Transform target = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		dir = (target.position - transform.position).normalized;
		Transform owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.transform : GuestPlayer.guestPlayer.transform;
		transform.rotation = owner.rotation;
		dir.y = 0f;
		transform.Rotate(0f, startRotation, 0f);
	}
}
