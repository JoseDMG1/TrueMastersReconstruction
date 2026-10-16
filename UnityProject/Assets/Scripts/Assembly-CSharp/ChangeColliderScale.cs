using UnityEngine;

public class ChangeColliderScale : MonoBehaviour
{
	public BoxCollider col;

	public Vector3 startScale;

	public Vector3 targetScale;

	public float duration = 1f;

	public float delay;

	private float timeElapsed;

	private void Start()
	{
		enabled = false;
		Invoke(nameof(EnableDelay), delay);
	}

	private void EnableDelay()
	{
		enabled = true;
	}

	private void Update()
	{
		// Native 0x121C778: the size belongs to BoxCollider, not transform.localScale.
		if (timeElapsed > duration) { col.size = targetScale; Destroy(this); }
		col.size = Vector3.Lerp(startScale, targetScale, timeElapsed / duration);
		timeElapsed += Time.deltaTime;
	}
}
