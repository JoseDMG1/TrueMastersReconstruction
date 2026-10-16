using UnityEngine;

public class TurnAttack : MonoBehaviour
{
	private GameObject target;

	public float delay;

	public float duration;

	private bool turning;

	public bool right;

	public float speed = 1f;

	private void Start()
	{
		// Native chooses by the Player layer (not PlayerAtk).
		if (BattleSceneSingleton.i)
			target = gameObject.layer == LayerMask.NameToLayer("Player")
				? BattleSceneSingleton.i.Opponent : BattleSceneSingleton.i.MainCharacter;
		speed *= 50f;
		Invoke(nameof(Init), delay);
		Invoke(nameof(finish), delay + duration);
	}

	private void Update()
	{
		if (!turning) return;
		// Native reads the target position but turns at the authored fixed rate;
		// it does not clamp or steer toward a newly calculated direction.
		transform.Rotate(0f, (right ? speed : -speed) * Time.deltaTime, 0f);
	}

	public void Init()
	{
		turning = true;
	}

	public void finish()
	{
		turning = false;
	}
}
