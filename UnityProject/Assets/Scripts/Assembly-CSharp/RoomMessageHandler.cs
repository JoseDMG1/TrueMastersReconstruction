using UnityEngine;

public class RoomMessageHandler : MonoBehaviour
{
	public LocalMultiplayerManager UI_Manager;

	public TCPConnection TCPConnection;

	public UDPConnectionManager udpConnection;

	public SyncTimers syncTimers;

	[SerializeField]
	private LocalMultiplayerData localMultiplayerData;

	public int maxPlayers;

	public int currentPlayers;

	[SerializeField]
	private CreateRoomManager createRoomManager;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void Handler(byte[] message)
	{
	}

	private void SendRoomMessage(int eventCode)
	{
	}

	public void ConnectToHost(string hostIP)
	{
	}

	public void ConnectToGuest(string guestIP)
	{
	}

	private void GoToCharacterSelection(float offsetTimer)
	{
	}

	public void Test()
	{
	}
}
