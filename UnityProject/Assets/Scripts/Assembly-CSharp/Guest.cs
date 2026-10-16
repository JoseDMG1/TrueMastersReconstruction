using Photon.Pun;
using UnityEngine;

public class Guest : MonoBehaviour
{
	public static Guest guest;

	public bool guestReady;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	[PunRPC]
	public void SetGuestReady()
	{
	}
}
