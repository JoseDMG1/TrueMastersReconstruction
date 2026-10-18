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

	// Some authored projectiles have a Follow component that steers their forward
	// axis. Their legacy mover advanced on transform.right, so correct aiming still
	// produced sideways or owner-directed travel.
	public bool aimAtTarget;

	// Optional authored flight direction, captured in world space when summoned.
	// Air's lateral tornadoes charge in place, then travel without homing.
	public Vector3 fixedLocalDirection;
	private Vector3 fixedWorldDirection;

	public float startRotation;

	private void Start()
	{
		if (fixedLocalDirection.sqrMagnitude > .0001f)
		{
			fixedWorldDirection = transform.TransformDirection(fixedLocalDirection).normalized;
			var follow = GetComponent<Follow>();
			if (follow) follow.enabled = false;
		}
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
			if (fixedWorldDirection.sqrMagnitude > .0001f)
			{
				transform.position += fixedWorldDirection * speed;
				return;
			}
			if (changeDirectionAtStart || aimAtTarget) ChangeDirection();
			ready = true;
			transform.position += (aimAtTarget ? transform.forward : transform.right) * speed;
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
		Transform target = null;
		if (BattleSceneSingleton.i && BattleSceneSingleton.i.MainCharacter && BattleSceneSingleton.i.Opponent)
			target = gameObject.layer == BattleSceneSingleton.i.MainCharacter.layer
				? BattleSceneSingleton.i.Opponent.transform : BattleSceneSingleton.i.MainCharacter.transform;
		else
			target = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;

		if (aimAtTarget && target)
		{
			dir = target.position - transform.position;
			dir.y = 0f;
			if (dir.sqrMagnitude > 1e-6f) transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
		}
		else
		{
			Transform owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.transform : GuestPlayer.guestPlayer.transform;
			transform.rotation = owner.rotation;
			dir = (target.position - transform.position).normalized;
			dir.y = 0f;
		}
		transform.Rotate(0f, startRotation, 0f);
	}
}
