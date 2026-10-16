using UnityEngine;

public class MoveRock : MonoBehaviour
{
	public float speed;

	public float sumonSpeed = .18f;

	private bool ready;

	public float rotateY = 4f;

	public float rotateZ = 1f;

	public float rotateX;

	public bool playerWeak;

	private Vector3 dir;

	private void FixedUpdate()
	{
		// Native 0x129C85C: displacement is per physics tick, not per second.
		if (GetComponent<ConfigAttackLayer>().playerWeak)
		{
			transform.position += (-transform.up * .35f + transform.forward * .25f) * speed;
			return;
		}
		if (transform.position.y >= 4f || ready)
		{
			ChangeDirection();
			ready = true;
			transform.position += dir * speed;
			transform.GetChild(0).Rotate(rotateX, rotateY, rotateZ);
		}
		else transform.position += transform.up * speed * sumonSpeed;
	}

	private void ChangeDirection()
	{
		if (ready) return;
		Transform target;
		if (BattleSceneSingleton.i && BattleSceneSingleton.i.MainCharacter && BattleSceneSingleton.i.Opponent)
			target = gameObject.layer == BattleSceneSingleton.i.MainCharacter.layer
				? BattleSceneSingleton.i.Opponent.transform : BattleSceneSingleton.i.MainCharacter.transform;
		else
			target = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		// Normalize before dropping Y: the native horizontal speed retains elevation's effect.
		dir = (target.position - transform.position).normalized;
		dir.y = 0f;
	}

	private void IsPlayerIsOk()
	{
		var owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.transform : GuestPlayer.guestPlayer.transform;
		var state = owner.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		string falling = gameObject.layer == 9 ? "Fall 3" : "Fall3";
		if (state.normalizedTime < .5f && (state.IsName("hit1") || state.IsName("hit2") || state.IsName(falling)
			|| state.IsName("RightHit") || state.IsName("LeftHit") || state.IsName("Falling2") || state.IsName("FallingToLanding")))
			playerWeak = true;
	}
}
