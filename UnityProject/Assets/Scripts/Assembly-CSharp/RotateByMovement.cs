using UnityEngine;

public class RotateByMovement : MonoBehaviour
{
	public float OGangleLimit;

	public float OGSpeed;

	public float angleLimit;

	public float speed;

	public float iddle;

	private Vector3 lastPosition;

	private Vector3 dir;

	public Quaternion targetRot;

	private float mag;

	private float min;

	private float max;

	public Quaternion originalLocalRot;

	public float speedTransition;

	public Transform SpineBone;

	private float spineLimit;

	public bool bodyLimit;

	public float bodyLimitAngle;

	public float bodyCollitionAngle;

	public float spineAngle;

	private float x;

	private float targetX;

	private float z;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void GetMovement()
	{
	}

	public void CheckBodyCollition()
	{
	}

	public void Rotate()
	{
	}

	private static float WrapAngle(float angle)
	{
		return 0f;
	}
}
