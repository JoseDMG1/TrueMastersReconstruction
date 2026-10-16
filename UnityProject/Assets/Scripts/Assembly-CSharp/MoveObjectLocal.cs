using UnityEngine;

public class MoveObjectLocal : MonoBehaviour
{
	public Vector3 AddVector;

	public float speed = .5f;

	private Vector3 positionEnd;

	private bool moveFinished = true;

	public float delay;

	public float finishMove = .55f;

	private void Start()
	{
		Invoke(nameof(StartMoving), delay);
	}

	private void Update()
	{
		if (!moveFinished)
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, positionEnd, speed * Time.deltaTime);
	}

	private void StartMoving()
	{
		moveFinished = false;
		positionEnd = gameObject.transform.localPosition;
		if (AddVector != Vector3.zero) positionEnd = gameObject.transform.localPosition + AddVector;
		speed *= 52.75f;
		Invoke(nameof(Finish), finishMove);
	}

	private void Finish()
	{
		moveFinished = true;
	}
}
