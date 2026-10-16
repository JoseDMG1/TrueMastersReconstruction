using UnityEngine;

public class ChangeMoveSpeed : MonoBehaviour
{
	public float newSpeed = 10f;

	public float delay = 3f;

	private void Start()
	{
		Invoke(nameof(ChangeSpeed), delay);
	}

	private void ChangeSpeed()
	{
		GetComponent<MoveForward>().speed = newSpeed;
	}

	private void Update()
	{
	}
}
