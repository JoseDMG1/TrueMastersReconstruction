using Photon.Pun;
using UnityEngine;

public class MasterInputs : MonoBehaviour
{
	private string horizontal;

	private string vertical;

	private string fire;

	private string jump;

	private PhotonView PV;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	[PunRPC]
	private void Attack()
	{
	}
}
