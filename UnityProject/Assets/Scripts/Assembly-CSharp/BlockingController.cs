using UnityEngine;

public class BlockingController : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private TCPConnection tcpConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void BlockingLogic(GameObject player, InputSet inputs)
	{
	}

	private void SendUnblock(int playerId)
	{
	}

	private void GetUnblock(byte[] data)
	{
	}
}
