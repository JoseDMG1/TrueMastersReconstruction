using UnityEngine;

public class PingPongMove : MonoBehaviour
{
	public Vector3 start;

	public Vector3 finish;

	public float duration;

	public float destroyTime;

	private void Start()
	{
		FirstMove();
	}

	private void FirstMove()
	{
		if (!LerpManager.i) return;
		// Native starts the local tween on the shared manager, easing enum 1.
		LerpManager.i.LerpLocalPosition(gameObject, start, finish, duration, (LerpEasing)1, 0f);
		Invoke(nameof(SecondMove), duration);
	}

	private void SecondMove()
	{
		if (!LerpManager.i) return;
		LerpManager.i.LerpLocalPosition(gameObject, finish, start, duration, (LerpEasing)1, 0f);
		Invoke(nameof(FirstMove), duration);
	}

	private void Update()
	{
	}
}
