using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpectatorManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public string room;

	private bool ready;

	public TMP_Text msg;

	private void Awake()
	{
	}

	public void CheckPlayersScene()
	{
	}

	public override void OnJoinedRoom()
	{
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
	}

	public override void OnConnectedToMaster()
	{
	}

	public void Connect()
	{
	}

	public void EnterCode()
	{
	}
}
