using UnityEngine;

public class CopyPatherLayer : MonoBehaviour
{
	private void Awake()
	{
		if (transform.parent) gameObject.layer = transform.parent.gameObject.layer;
	}

	private void Update()
	{
	}
}
