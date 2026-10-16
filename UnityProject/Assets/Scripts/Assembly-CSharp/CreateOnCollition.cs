using UnityEngine;

public class CreateOnCollition : MonoBehaviour
{
	public GameObject prefab;

	public bool createOnCollider;

	private FxController fxController;

	private void Start()
	{
		var description = GetComponent<AtkDescription>();
		if (description) fxController = description.fxController;
	}

	private void OnTriggerEnter(Collider other)
	{
		// This component belongs to local multiplayer's AtkDescription contract.
		// Native creation authority is LocalMultiplayerData.isHost, not an attack layer guess.
		if (!fxController || !fxController.localData || !fxController.localData.isHost) return;
		Transform source = createOnCollider && other ? other.transform : transform;
		fxController.CreateAndSendFX(prefab, source.position, source.rotation);
	}
}
