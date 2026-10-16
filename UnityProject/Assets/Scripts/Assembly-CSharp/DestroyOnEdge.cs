using UnityEngine;

public class DestroyOnEdge : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (transform.position.magnitude > 33f) Destroy(gameObject);
	}
}
