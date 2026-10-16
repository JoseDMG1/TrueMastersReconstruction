using UnityEngine;

public class RotPart : MonoBehaviour
{
	public Transform target;

	public float speed;

	public float limitAngle;

	public bool setValueByParent;

	public bool limitXAxis;

	public float angle;

	public float velocity;

	private Vector3 targetLastPosition;

	public float targetAngleH;

	public float targetAngleV;

	public Vector3 targetRotation;

	public Vector3 originalRotation;

	public float limitMag;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
	}

	public void SetSpeed()
	{
	}

	public void SetParentValues()
	{
	}

	public void GetTargetMovement()
	{
	}

	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		return 0f;
	}
}
