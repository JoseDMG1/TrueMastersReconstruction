using UnityEngine;

public class CreateObject : MonoBehaviour
{
	public GameObject obj;

	public float delay = 1f;

	public Vector3 addPosition;

	private void Start()
	{
		Invoke("Create", delay);
	}

	private void Create()
	{
		if (GetComponent<ConfigAttackLayer>().playerWeak) return;
		addPosition = transform.forward * addPosition.z + transform.right * addPosition.x + transform.up * addPosition.y;
		GameObject created = Instantiate(obj, transform.position + addPosition, transform.rotation);
		created.layer = gameObject.layer;
		ChangeRendererMat color = GetComponent<ChangeRendererMat>();
		if (color) color.ChangeColor(created);
	}
}
