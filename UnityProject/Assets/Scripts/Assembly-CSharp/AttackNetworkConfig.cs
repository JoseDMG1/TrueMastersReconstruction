using Photon.Pun;

public class AttackNetworkConfig : MonoBehaviourPunCallbacks, IPunObservable
{
	private PhotonView PV;

	private void Start()
	{
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}
}
