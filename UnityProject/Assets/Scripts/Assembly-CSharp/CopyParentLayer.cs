using UnityEngine;

public class CopyParentLayer : MonoBehaviour
{
	private void Start()
	{
		if (transform.parent) gameObject.layer = transform.parent.gameObject.layer;
	}
}
