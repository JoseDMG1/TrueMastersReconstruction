using UnityEngine;

public class MoveWater : MonoBehaviour
{
	public float speed;

	private bool ready;

	public bool playerWeak;

	private Vector3 dir;

	public float timeToStart;

	public bool moving;

	public float position;

	public float positionV;

	public float positionZ;

	public bool changeDirectionAtStart;

	private void Start()
	{
		// Native 0x129DA44: apply authored offsets before the delayed start.
		transform.position += transform.right * position + transform.up * positionV + transform.forward * positionZ;
		Invoke(nameof(StartMov), timeToStart);
		speed *= 50f;
	}

	private void Update()
	{
		// Native reads ConfigAttackLayer.playerWeak, not this legacy field.
		if (GetComponent<ConfigAttackLayer>().playerWeak)
		{
			transform.position += (-transform.up * .35f + transform.forward * .1f) * speed * Time.deltaTime;
			return;
		}
		if (!moving) return;
		if (changeDirectionAtStart) ChangeDirection();
		ready = true;
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	private void StartMov()
	{
		moving = true;
	}

	private void ChangeDirection()
	{
		if (ready) return;
		Transform opponent = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		Transform owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.transform : GuestPlayer.guestPlayer.transform;
		dir = (opponent.position - transform.position).normalized;
		// Native copies the owner's rotation; the calculated dir is not LookAt.
		transform.rotation = owner.rotation;
		dir.y = 0f;
	}

	private void IsPlayerIsOk()
	{
		// Retained native helper 0x129E17C has no mapped direct callers.
		var owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.gameObject : GuestPlayer.guestPlayer.gameObject;
		var state = owner.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		string falling = gameObject.layer == 9 ? "Fall 3" : "Fall3";
		if (state.normalizedTime < .5f && (state.IsName("hit1") || state.IsName("hit2") || state.IsName(falling)
			|| state.IsName("RightHit") || state.IsName("LeftHit") || state.IsName("Falling2") || state.IsName("FallingToLanding")))
			playerWeak = true;
	}
}
