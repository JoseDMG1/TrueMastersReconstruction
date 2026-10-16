using UnityEngine;

public class ChangeStartScale : MonoBehaviour
{
	public Vector3 scale;

	private void Awake()
	{
		// Native 0x121E73C: apply this component's serialized scale once.
		transform.localScale = scale;
	}
}
