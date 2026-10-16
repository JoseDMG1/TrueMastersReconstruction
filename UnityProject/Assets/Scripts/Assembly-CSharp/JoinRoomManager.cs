using System.Collections.Generic;
using UnityEngine;

public class JoinRoomManager : MonoBehaviour
{
	public UDPConnectionManager udpManager;

	public LocalMultiplayerManager UI_Manager;

	public List<RoomData> roomDataList;

	public GameObject roomItemPrefab;

	public GameObject roomContainer;

	public PasswordManager_LM passwordManager;

	public void InitSearchRooms()
	{
	}

	public void HandleIncomingMessage(byte[] message)
	{
	}

	public void SearchRooms()
	{
	}

	public void CheckRooms()
	{
	}

	public void RoomsNotFound()
	{
	}

	public void CloseRoomServer()
	{
	}

	public void InitRoomList()
	{
	}

	public void Test()
	{
	}
}
