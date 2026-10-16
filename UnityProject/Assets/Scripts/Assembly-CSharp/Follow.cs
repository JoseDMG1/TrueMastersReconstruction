using UnityEngine;

public class Follow : MonoBehaviour
{
	public float speed;

	private float originalSpeed;

	public float startSpeed;

	public bool startFaster;

	public float startTime;

	public float stopAfter;

	public GameObject target;

	private void Start()
	{
		if (GameController.i.online) { Destroy(this); return; }
		target = BattleSceneSingleton.i.MainCharacter.layer == gameObject.layer
			? BattleSceneSingleton.i.Opponent : BattleSceneSingleton.i.MainCharacter;
		speed *= 52.75f;
		if (startFaster)
		{
			originalSpeed = speed; speed *= startSpeed; Invoke(nameof(RecoverSpeed), startTime);
		}
		if (stopAfter > 0f) Destroy(this, stopAfter);
	}

	public void RecoverSpeed()
	{
		speed = originalSpeed;
	}

	private void Update()
	{
		if (!target) return;
		float direction = AngleDir(transform.forward, target.transform.position - transform.position, transform.up);
		if (direction > .1f) transform.Rotate(0f, speed * Time.deltaTime, 0f);
		else if (direction < -.1f) transform.Rotate(0f, -speed * Time.deltaTime, 0f);
	}

	public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		return Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
	}
}
