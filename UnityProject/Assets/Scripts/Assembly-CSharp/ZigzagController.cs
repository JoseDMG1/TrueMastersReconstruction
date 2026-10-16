using UnityEngine;

public class ZigzagController : MonoBehaviour
{
	[Header("Zigzag Parameters")]
	public float moveDistance = 1f;

	public float switchTime = 1f;

	private int direction = 1;

	private float timer;

	private void Start()
	{
		timer = switchTime * .5f;
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		// Native motion is per Update; only the reversal timer uses deltaTime.
		transform.position += transform.right * moveDistance * direction;
		if (timer <= 0f)
		{
			direction = -direction;
			timer = switchTime;
		}
	}
}
