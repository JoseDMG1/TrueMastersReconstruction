using Photon.Pun;
using UnityEngine;

public class FinishMenu : MonoBehaviour
{
	public GameObject playAgain_btn;

	public GameObject wait_btn;

	public GameObject rematch_btn;

	public bool invitationSent;

	public GameObject iconChat;

	public OnlineChat chat;

	public bool SpectatorReady;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void exitGame()
	{
		Application.Quit();
	}

	[PunRPC]
	public void OpponentReadyRematch()
	{
	}

	[PunRPC]
	public void SpectatorLoading()
	{
	}

	private void ArePlayersInSelectionScene()
	{
	}

	public void SendInvitation()
	{
	}

	public void Rematch()
	{
	}

	public void reboot()
	{
		// 0x128BD48: offline restart without the original store-rating popup gate.
		GameController.i.Restart();
	}

	public void OpenChat()
	{
	}
}
