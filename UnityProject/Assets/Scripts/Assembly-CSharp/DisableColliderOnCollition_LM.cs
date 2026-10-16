using UnityEngine;

public class DisableColliderOnCollition_LM : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
		// Native 0x1228E74 ignores the incoming collider; it checks its own box/tag.
		var box = GetComponent<BoxCollider>();
		if (box.CompareTag("Attack") || box.CompareTag("Player")) box.enabled = false;
	}
}
