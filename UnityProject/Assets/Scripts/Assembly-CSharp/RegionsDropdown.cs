using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;

public class RegionsDropdown : MonoBehaviourPunCallbacks
{
	public Dropdown dropdown;

	public List<string> regions;

	public RoomManager roomManager;

	public Text connectingText;

	private void Start()
	{
	}

	public void ConnectToRegion(int regionIndex)
	{
	}
}
