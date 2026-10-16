using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController cameraController;

	public float transitionSpeed = 2f;

	private Transform currentView;

	public Transform[] views;

	public GameObject randomBanner;

	private void Awake()
	{
		cameraController = this;
		if (currentView == null) currentView = transform;
	}

	public void Show(string character)
	{
		// Native 0x12802D0 uses this order, which differs from the carousel.
		string[] order = { "Rina", "Amon", "Diana", "Ren", "Vinea", "Eliot", "Aura", "Sora", "Dante", "Agata", "Raven", "Maya", "?" };
		randomBanner.SetActive(character == "?");
		int index = System.Array.IndexOf(order, character);
		if (index >= 0) currentView = views[index];
	}

	private void LateUpdate()
	{
		if (currentView == null) return;
		float t = Time.deltaTime * transitionSpeed;
		transform.position = Vector3.Lerp(transform.position, currentView.position, t);
		Vector3 from = transform.eulerAngles, to = currentView.eulerAngles;
		transform.eulerAngles = new Vector3(Mathf.LerpAngle(from.x, to.x, t),
			Mathf.LerpAngle(from.y, to.y, t), Mathf.LerpAngle(from.z, to.z, t));
	}
}
