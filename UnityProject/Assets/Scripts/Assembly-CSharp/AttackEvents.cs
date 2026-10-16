using UnityEngine;
using UnityEngine.Events;

public class AttackEvents : MonoBehaviour
{
	public UnityEvent AttackHit;

	private void OnTriggerEnter(Collider other)
	{
		var description = other.GetComponent<Description>();
		if (description == null || description.Type != "Char") return;
		AttackHit.Invoke();
	}
}
