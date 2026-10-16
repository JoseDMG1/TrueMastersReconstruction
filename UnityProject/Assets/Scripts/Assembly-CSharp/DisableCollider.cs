using UnityEngine;
using UnityEngine.Events;

public class DisableCollider : MonoBehaviour
{
	public float time;

	public UnityEvent onColliderDisable;

	private void Start()
	{
		Invoke(nameof(Disable), time);
	}

	public void Disable()
	{
		var collider = GetComponent<Collider>();
		if (collider) collider.enabled = false;
		onColliderDisable?.Invoke();
	}
}
