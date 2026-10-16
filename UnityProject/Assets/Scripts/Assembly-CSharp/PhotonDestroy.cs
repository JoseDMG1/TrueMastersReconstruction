using Photon.Pun;
using UnityEngine;

public class PhotonDestroy : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
	}

	[PunRPC]
	public void SendDestroy()
	{
	}
}
