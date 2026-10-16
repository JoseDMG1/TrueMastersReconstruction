using UnityEngine;

public class EnableComponentsOnCollition : MonoBehaviour
{
	[SerializeField]
	private Behaviour[] components;

	private void OnTriggerEnter(Collider other)
	{
	}
}
