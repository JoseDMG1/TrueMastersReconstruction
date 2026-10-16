using UnityEngine;

public class ChangeCollider : MonoBehaviour
{
	public Vector3 newSize;

	public float delay = 1f;

	private void Start()
	{
		Invoke(nameof(ChangeSize), delay);
	}

	private void ChangeSize()
	{
		GetComponent<BoxCollider>().size = newSize;
	}
}
