using UnityEngine;

public class BonePhysics : MonoBehaviour
{
	private Vector3 targetlastPosition;

	public float targetHorizontal;

	public float targetVertical;

	public float lastTargetHorizontal;

	public float iddleSpeed;

	public float Limdistance;

	public float distance;

	public float maxHorizontalMove;

	public float maxVerticalMove;

	public float minVerticalMove;

	public float twist;

	public Transform target;

	public float limitBoneAngle;

	public float boneAngle;

	public float horizontal;

	public float vertical;

	public float velocity;

	public float maxVelocity;

	public float horizontalAngle;

	public float minAngle;

	public float maxAngle;

	public float maxDistance;

	public float minDistance;

	public bool running;

	public bool iddle;

	public float runningSpeed;

	private void FixedUpdate()
	{
	}

	private void SetByLastPosition()
	{
	}

	private void SmoothAnim()
	{
	}
}
