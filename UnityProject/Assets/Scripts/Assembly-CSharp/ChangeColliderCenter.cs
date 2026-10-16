using UnityEngine;

public class ChangeColliderCenter : MonoBehaviour
{
	public BoxCollider col;

	public Vector3 startCenter;

	public Vector3 targetCenter;

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
		// Native 0x121C5F8: completion is strictly greater, Destroy targets this component.
		if (timeElapsed > duration) { col.center = targetCenter; Destroy(this); }
		col.center = Vector3.Lerp(startCenter, targetCenter, timeElapsed / duration);
		timeElapsed += Time.deltaTime;
	}
}
