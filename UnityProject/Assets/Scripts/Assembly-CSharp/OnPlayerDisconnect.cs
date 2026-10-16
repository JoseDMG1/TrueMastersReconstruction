using Photon.Pun;
using Photon.Realtime;

public class OnPlayerDisconnect : MonoBehaviourPunCallbacks
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
	}

	public void OnPlayerDisconnected(NetworkPlayer player)
	{
	}

	[PunRPC]
	public void PlayerDisconnected()
	{
	}
}
