using UnityEngine;

public class RotateSelf : MonoBehaviour
{
	public float y;

	public float x;

	public float z;

	public bool rotate;

	public float delay;

	private void Start()
	{
		if (delay >= 0f) Invoke(nameof(StartRotation), delay);
	}

	private void FixedUpdate()
	{
		// Native field order y,x,z is also its Rotate argument order. No deltaTime here.
		if (rotate) transform.Rotate(y, x, z);
	}

	public void StartRotation()
	{
		rotate = true;
	}
}
