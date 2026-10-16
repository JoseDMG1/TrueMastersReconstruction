using UnityEngine;

public class HomingAttack : MonoBehaviour
{
	public GameObject target;

	public float delay;

	private bool homing;

	public float horizontalSpeed = 40f;

	public float verticalSpeed;

	private void Start()
	{
		// Native 0x1239E88 resolves ownership by attack layer, not root identity.
		if (BattleSceneSingleton.i)
		{
			var scene = BattleSceneSingleton.i;
			// Offline SummonAttack copies the fighter layer (9/8), while the native
			// name-based route also supports the authored PlayerAtk/EnemyAtk layers.
			bool playerOwned = gameObject.layer == LayerMask.NameToLayer("PlayerAtk")
				|| (scene.MainCharacter && gameObject.layer == scene.MainCharacter.layer);
			target = playerOwned ? scene.Opponent : scene.MainCharacter;
		}
		Invoke(nameof(StartHoming), delay);
	}

	private void Update()
	{
		if (!homing || !target) return;
		Vector3 targetDirection = target.transform.position - transform.position;
		if (horizontalSpeed > 0f)
		{
			float side = AngleDir(transform.forward, targetDirection, transform.up);
			if (side > .1f) transform.Rotate(0f, horizontalSpeed * Time.deltaTime, 0f);
			else if (side < -.1f) transform.Rotate(0f, -horizontalSpeed * Time.deltaTime, 0f);
		}
		if (verticalSpeed > 0f)
		{
			float side = AngleDir(transform.forward, targetDirection, transform.right);
			// Preserve the original asymmetric comparisons at 0x123A154/0x123A18C.
			if (side < .1f) transform.Rotate(-verticalSpeed * Time.deltaTime, 0f, 0f);
			else if (side > -.1f) transform.Rotate(verticalSpeed * Time.deltaTime, 0f, 0f);
		}
	}

	public void StartHoming()
	{
		homing = true;
	}

	public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		return Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
	}
}
