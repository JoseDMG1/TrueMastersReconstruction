using UnityEngine;
using UnityEngine.UI;

public class ActivateOnClick : MonoBehaviour
{
	public GameObject element;
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		if (button == null) button = GetComponentInParent<Button>();
		if (button == null)
		{
			Debug.LogError("ActivateOnClick requires a Button on this object or its parent.", this);
			return;
		}
		button.onClick.AddListener(Activate);
	}

	public void Activate()
	{
		if (element != null) element.SetActive(true);
		else Debug.LogError("ActivateOnClick has no target assigned.", this);
	}

	private void OnDestroy()
	{
		if (button != null) button.onClick.RemoveListener(Activate);
	}
}
