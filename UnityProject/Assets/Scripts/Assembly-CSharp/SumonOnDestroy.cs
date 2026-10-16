using UnityEngine;

public class SumonOnDestroy : MonoBehaviour
{
	public GameObject obj;

	private void OnDestroy()
	{
		if (obj) Instantiate(obj, transform.position, Quaternion.identity);
	}
}
