using Photon.Pun;
using UnityEngine;

public class RCPManager : MonoBehaviour
{
	public static RCPManager i;

	public PhotonView pv;

	public bool player1Ready;

	public bool player2Ready;

	private void Start()
	{
	}

	[PunRPC]
	public void SetHostReady(string OpUsername, string OpUserID)
	{
	}

	[PunRPC]
	public void SetGuestReady(string OpUsername, string OpUserID)
	{
	}

	[PunRPC]
	public void Player2Ready()
	{
	}

	[PunRPC]
	public void TimeOverRPC()
	{
	}
}
