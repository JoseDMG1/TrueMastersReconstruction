using UnityEngine;

public class RotateByMove : MonoBehaviour
{
	public Transform targetMovement;

	public float limitAngle;

	public float x_fix;

	public float z_fix;

	public float minSpeed;

	public float maxSpeed;

	public float transitionSpeed;

	public bool invertLimit;

	public bool bodyLimit;

	public float bodyAngleLimit;

	private Quaternion targetRot;

	private Vector3 lastPosition;

	private Vector3 dir;

	public float x;

	private float y;

	public float z;

	private float mag;

	private float targetMag;

	private void Update()
	{
	}

	public void GetMovement()
	{
	}

	public void SetLimit()
	{
	}
}
