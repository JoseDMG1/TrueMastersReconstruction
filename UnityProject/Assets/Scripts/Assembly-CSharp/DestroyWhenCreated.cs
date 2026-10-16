using UnityEngine;

public class DestroyWhenCreated : MonoBehaviour
{
	public GameObject target;

	private void Start()
	{
		Destroy(target);
	}
}
