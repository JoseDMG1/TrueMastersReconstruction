using Photon.Pun;

public class SendInputsToMaster : MonoBehaviourPunCallbacks, IPunObservable
{
	private string horizontal;

	private string vertical;

	private string fire;

	private string jump;

	private float x;

	private float z;

	private float a;

	private float j;

	private float xr;

	private float zr;

	private float ar;

	private float jr;

	private PhotonView PV;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}
}
