using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
	public delegate void YesCallback();

	public static Notifications i;

	protected YesCallback yesCallback;

	public Animator barAnim;

	public Text barText;

	public Image barBG;

	public GameObject popup;

	public TMP_Text popupText;

	public GameObject okBtn;

	public GameObject yesBtn;

	public GameObject noBtn;

	private void Awake()
	{
		// Native 0x129F8D0.
		if (i == null) i = this;
		else if (i != this)
		{
			Debug.Log("Instance already exists, destroying object!");
			Destroy(this);
		}
	}

	public void Log(string notification, float time = 5f, string type = "info")
	{
		// 0x129FA00: restarting an alert cancels its previous removal timer.
		barAnim.ResetTrigger("On");
		barAnim.ResetTrigger("Off");
		barText.text = notification;
		SetColor(type);
		CancelInvoke();
		barAnim.SetTrigger("On");
		Invoke(nameof(Remove), time);
	}

	public void OpenQuestionPopup(string text, YesCallback callback)
	{
		okBtn.SetActive(false);
		yesBtn.SetActive(true);
		noBtn.SetActive(true);
		yesCallback = callback;
		LerpManager.i.LerpScale(popup, Vector3.zero, Vector3.one, 0.5f, LerpEasing.EasyOut);
		popupText.text = text;
	}

	public void Yes()
	{
		yesCallback();
		ClosePopup();
	}

	public void OpenPopup(string text)
	{
		okBtn.SetActive(true);
		yesBtn.SetActive(false);
		noBtn.SetActive(false);
		LerpManager.i.LerpScale(popup, Vector3.zero, Vector3.one, 0.5f, LerpEasing.EasyOut);
		popupText.text = text;
	}

	public void ClosePopup()
	{
		LerpManager.i.LerpScale(popup, Vector3.one, Vector3.zero, 0.25f, LerpEasing.EasyOut);
	}

	public void Remove()
	{
		barAnim.ResetTrigger("On");
		barAnim.SetTrigger("Off");
	}

	public void SetColor(string type)
	{
		// Native 0x129FB08. Unknown types retain the existing color.
		switch (type)
		{
			case "info": barBG.color = new Color(0f, 0f, 0f, 1f); break;
			case "warn": barBG.color = new Color(0.7f, 0.5f, 0f, 1f); break;
			case "alert": barBG.color = new Color(0.5f, 0f, 0f, 1f); break;
			case "success": barBG.color = new Color(0f, 0.5f, 0f, 1f); break;
		}
	}

	private void OnDestroy() { if (i == this) i = null; }
}
