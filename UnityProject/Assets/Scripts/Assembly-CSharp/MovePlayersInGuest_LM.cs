using UnityEngine;

public class MovePlayersInGuest_LM : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private LocalMultiplayerData localData;

	[SerializeField]
	private UDPConnectionManager udpConnection;

	private GameObject host;

	private GameObject guest;

	private Vector3 hostTargetPosition;

	private Vector3 guestTargetPosition;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	private void Update()
	{
	}

	private void GetPlayersPosition(byte[] data)
	{
	}

	private void MovePlayer(GameObject player, Vector3 targetPosition)
	{
	}
}
