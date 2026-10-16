using Photon.Pun;
using UnityEngine;

public class SetNetworkPosition : MonoBehaviour
{
	public PhotonView PV;

	public Vector3 netPosition;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[PunRPC]
	public void CorrectPosition(Vector3 newPosition)
	{
	}
}
