using UnityEngine;

public class DestroyByTime_Basic : MonoBehaviour
{
	public float delay = 1f;

	private void Start()
	{
		Destroy(gameObject, delay);
	}

	private void Update()
	{
	}
}
