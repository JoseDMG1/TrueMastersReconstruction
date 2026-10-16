using UnityEngine;

public class IncreaseCollider : MonoBehaviour
{
	public float time;

	private void Start()
	{
		Invoke(nameof(IncreaseSize), time);
	}

	private void IncreaseSize()
	{
		// Native 0x12972CC preserves X/Z and sets height to four.
		var collider = GetComponent<BoxCollider>();
		var size = collider.size;
		collider.size = new Vector3(size.x, 4f, size.z);
	}
}
