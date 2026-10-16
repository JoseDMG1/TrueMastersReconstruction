using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class OnlineChat : MonoBehaviour
{
	public Text chatText;

	public InputField input;

	public GameObject iconMsg;

	public GameObject iconAlertMsg;

	public bool chatInit;

	public GameObject TypingAlert;

	public Text typingAlertText;

	public string yourUsername;

	private void Start()
	{
	}

	public void SendText()
	{
	}

	public void CloseChat()
	{
	}

	public void Typing()
	{
	}

	[PunRPC]
	public void ShowTypingAlert(string CharUsername)
	{
	}

	[PunRPC]
	public void HideTypingAlert()
	{
	}

	[PunRPC]
	public void ReciveMSG(string msg, string opUsername)
	{
	}

	public void OpenChat()
	{
	}

	private void Update()
	{
	}
}
