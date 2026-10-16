using UnityEngine;
using UnityEngine.Events;

public class GuestInputs_LM : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private UDPConnectionManager udpConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	public UnityEvent<InputSet> OnGuestInputs;

	private float sendTimer;

	private const float sendInterval = 0.05f;

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

	private void SendGuestInputs(InputSet inputs)
	{
	}

	private void GetGuestInputs(byte[] inputsBytes)
	{
	}
}
