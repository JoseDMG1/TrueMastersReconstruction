using UnityEngine;

public class LockToTarget : MonoBehaviour
{
	private GameObject target;

	public float delay;

	private void Start()
	{
		// Native 0x1243C08: retain the opposing root, then aim once after delay.
		var scene = BattleSceneSingleton.i;
		if (!scene) return;
		bool playerOwned = gameObject.layer == LayerMask.NameToLayer("PlayerAtk")
			|| (scene.MainCharacter && gameObject.layer == scene.MainCharacter.layer);
		target = playerOwned ? scene.Opponent : scene.MainCharacter;
		Invoke(nameof(Lock), delay);
	}

	private void Lock()
	{
		// Native 0x1243CE0 deliberately discards height and does not track each frame.
		if (!target) return;
		Vector3 direction = target.transform.position - transform.position;
		direction.y = 0f;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}
