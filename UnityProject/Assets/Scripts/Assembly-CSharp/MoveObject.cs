using UnityEngine;

public class MoveObject : MonoBehaviour
{
	public Vector3 AddVector;

	public float speed = .5f;

	private Vector3 positionEnd;

	private bool moveFinished = true;

	public float delay;

	public float finishMove = .55f;

	public float moveForward;

	public float moveRight;

	public float moveUp;

	private void Start()
	{
		Invoke(nameof(StartMoving), delay);
	}

	private void FixedUpdate()
	{
		if (moveFinished) return;
		// Native 0x129C010: clamped interpolation factor per tick, no deltaTime.
		transform.position = Vector3.Lerp(transform.position, positionEnd, speed);
	}

	private void StartMoving()
	{
		moveFinished = false;
		positionEnd = transform.position;
		if (AddVector.sqrMagnitude >= 1e-10f) positionEnd += AddVector;
		positionEnd += transform.forward * moveForward + transform.right * moveRight + transform.up * moveUp;
		Invoke(nameof(Finish), finishMove);
	}

	private void Finish()
	{
		moveFinished = true;
	}
}
