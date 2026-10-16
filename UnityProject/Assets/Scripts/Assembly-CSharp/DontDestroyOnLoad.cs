using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	private void Start()
	{
		Object.DontDestroyOnLoad(transform.gameObject);
	}

	private void Update()
	{
	}
}
