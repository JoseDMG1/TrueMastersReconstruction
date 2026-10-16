using UnityEngine;

public class sumonOnCollider : MonoBehaviour
{
	public GameObject sumon;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponent<Description>()) return;
		if (Photon.Pun.PhotonNetwork.IsConnected)
		{
			if (GetComponent<Photon.Pun.PhotonView>().IsMine) NetworkSumon(other.transform);
		}
		else Sumon(other.transform);
	}

	public void NetworkSumon(Transform otherTransform)
	{
		Photon.Pun.PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", sumon.name),
			otherTransform.position, otherTransform.rotation);
	}

	public void Sumon(Transform otherTransform)
	{
		Instantiate(sumon, otherTransform.position, otherTransform.rotation);
	}
}
