using UnityEngine;

public class FollowOwner : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		BattleSceneSingleton scene = BattleSceneSingleton.i;
		if (!scene || !scene.MainCharacter || !scene.Opponent) return;
		GameObject owner = gameObject.layer == scene.MainCharacter.layer ? scene.MainCharacter : scene.Opponent;
		transform.position = owner.transform.position;
		transform.rotation = owner.transform.rotation;
	}
}
