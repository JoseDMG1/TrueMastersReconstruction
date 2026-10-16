using UnityEngine;

public class ChangeStartRotation : MonoBehaviour
{
	public Vector3 addRotation;

	private void Start()
	{
		// Native 0x121E708 uses the Vector3 Rotate overload in local space.
		transform.Rotate(addRotation);
	}
}
