using System.IO;
using UnityEngine;

public class MovePlayersInHost_LM : MonoBehaviour, IBattleTask
{
	public InputSet hostInputs;

	public InputSet guestInputs;

	public LocalMultiplayerData localData;

	[SerializeField]
	private GuestInputs_LM guestInputsReader;

	[SerializeField]
	private InputAnimationController inputAnimationController;

	public GameObject host;

	public GameObject guest;

	public bool disableHostInputs;

	public bool disableGuestInputs;

	[SerializeField]
	private UDPConnectionManager udpConnection;

	[SerializeField]
	private BlockingController blockingController;

	private float sendTimer;

	private const float sendInterval = 0.05f;

	private float playerSpeed;

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

	private void HandlePlayerInput(GameObject player, ref bool disableInputs, InputSet inputs)
	{
	}

	private void SetHostInputs(InputSet inputs)
	{
	}

	private void SetGuestInputs(InputSet inputs)
	{
	}

	private void MovePlayerInHost(GameObject player, InputSet currentInputs)
	{
	}

	private void SendPlayersPositionToGuest()
	{
	}

	private Vector3 RoundVector3(Vector3 vector, int decimals)
	{
		return default;
	}

	private void WriteVector3(BinaryWriter writer, Vector3 vector)
	{
	}
}
