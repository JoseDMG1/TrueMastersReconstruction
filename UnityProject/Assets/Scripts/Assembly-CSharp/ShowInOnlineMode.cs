using UnityEngine;
using Photon.Pun;

public class ShowInOnlineMode : MonoBehaviour
{
	private void Start()
	{
		// Native 0x12AF4E0: network-only presentation hides itself offline.
		gameObject.SetActive(PhotonNetwork.IsConnected || SystemManager.i.mode == GameMode.Online);
	}
}
