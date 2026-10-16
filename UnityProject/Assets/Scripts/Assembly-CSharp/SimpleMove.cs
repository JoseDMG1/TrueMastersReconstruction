using UnityEngine;

public class SimpleMove : MonoBehaviour
{
	public float speed;

	public Vector3 dir;

	public float delay;

	private bool moving;

	private void Start()
	{
		Invoke(nameof(StartMoving), delay);
	}

	public void StartMoving()
	{
		moving = true;
	}

	private void Update()
	{
		if (moving) transform.Translate(dir * speed * Time.deltaTime);
	}
}
