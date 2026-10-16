using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public OnlineCustomProperties customProperties;

	public RoomManager roomManager;

	public Text regionText;

	public Text connectionText;

	public bool regionChanged;

	private void Awake()
	{
	}

	public override void OnEnable()
	{
	}

	public override void OnDisable()
	{
	}

	public override void OnConnectedToMaster()
	{
	}

	public void DelayJoinRoom()
	{
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
	}

	public void CreateRoom()
	{
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
	}

	public override void OnJoinedRoom()
	{
	}

	public void StartAsSpectator()
	{
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	public override void OnCreatedRoom()
	{
	}
}
