using UnityEngine;

public class ColliderController : MonoBehaviour
{
	public BoxCollider boxCollider;

	public bool smallCollider;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider>();
	}

	public void reduceCollider()
	{
		smallCollider = true;
		if (!boxCollider) boxCollider = GetComponent<BoxCollider>();
		boxCollider.center = new Vector3(0f, .5f, 0f);
		boxCollider.size = new Vector3(1f, 1.1f, 1f);
	}

	public void recoverCollider()
	{
		smallCollider = false;
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.center = new Vector3(0f, 1f, 0f);
		boxCollider.size = new Vector3(1f, 2f, 1f);
	}
}
