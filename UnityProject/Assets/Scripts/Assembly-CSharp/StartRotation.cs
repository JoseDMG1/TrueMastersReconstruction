using UnityEngine;

public class StartRotation : MonoBehaviour
{
	public Quaternion initialRotation;

	public float rot;

	public float delay;

	private void Start()
	{
		// Native 0x12B3448: one delayed, relative yaw adjustment.
		Invoke(nameof(Rotate), delay);
	}

	private void Rotate()
	{
		transform.Rotate(0f, rot, 0f);
	}

	private void FixedUpdate()
	{
	}
}
