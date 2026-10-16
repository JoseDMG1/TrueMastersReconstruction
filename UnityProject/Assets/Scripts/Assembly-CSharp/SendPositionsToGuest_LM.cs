using UnityEngine;

public class SendPositionsToGuest_LM : MonoBehaviour
{
	[SerializeField]
	private LocalMultiplayerData localData;

	[SerializeField]
	private UDPConnectionManager udpConnection;

	private Vector3 lastHostPosition;

	private Vector3 lastGuestPosition;

	public Vector3 NetHostPlayerPos { get; private set; }

	public Vector3 NetGuestPlayerPos { get; private set; }

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SendUpdatedPositions()
	{
	}

	private void SendPosition(PlayerType playerType, Vector3 position)
	{
	}

	private void ProcessPositionData(byte[] data)
	{
	}

	private void OnDestroy()
	{
	}

	private byte[] StructToBytes(PlayerPosition playerPos)
	{
		return null;
	}

	private PlayerPosition BytesToPlayerPosition(byte[] data)
	{
		return default;
	}
}
