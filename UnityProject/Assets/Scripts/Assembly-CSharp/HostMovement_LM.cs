using UnityEngine;

public class HostMovement_LM : MonoBehaviour
{
	public InputSet currentInputs;

	[SerializeField]
	private LocalMultiplayerData localData;

	private GameObject host;

	[SerializeField]
	private UDPConnectionManager udpConnection;

	[SerializeField]
	private InputAnimationController inputAnimationController;

	private float sendTimer;

	private const float sendInterval = 0.05f;

	private Vector3 targetPosition;

	public bool disableInputs;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void MoveHostCharacter()
	{
	}

	private void SetHostInputs(InputSet hostInputs)
	{
	}

	private void SendHostPositionToGuest()
	{
	}

	private void GetHostPositionInGuest(byte[] netPositionBytes)
	{
	}

	private void OnDestroy()
	{
	}
}
