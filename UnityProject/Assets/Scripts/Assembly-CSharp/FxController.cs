using UnityEngine;

public class FxController : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void CreateAndSendFX(GameObject fxPrefab, Vector3 pos, Quaternion rot)
	{
	}

	private void CreateFX(GameObject fxPrefab, Vector3 pos, Quaternion rot)
	{
	}

	public void SendFxToGuest(string fxName, Vector3 pos, Quaternion rot)
	{
	}

	public void OnFXReceived(byte[] message)
	{
	}
}
