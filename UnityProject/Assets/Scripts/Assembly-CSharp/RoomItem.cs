using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
	public TMP_Text roomName_UI;

	public string roomName;

	public string ip;

	public string password;

	public bool isLocked;

	public GameObject lockIcon;

	public PasswordManager_LM passwordManager;

	public void SetRoomName()
	{
	}

	public void ConnectToHost()
	{
	}
}
