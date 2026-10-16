using UnityEngine;

public class GuestMovement_LM : MonoBehaviour
{
	public InputSet currentGuestInputs;

	[SerializeField]
	private LocalMultiplayerData localData;

	private GameObject guest;

	[SerializeField]
	private GuestInputs_LM inputConnection_LM;

	[SerializeField]
	private UDPConnectionManager udpConnection;

	public bool disableInputs;

	private Vector3 targetPosition;

	[SerializeField]
	private InputAnimationController inputAnimationController;

	private float sendTimer;

	private const float sendInterval = 0.05f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void MoveMainCharacterInGuest()
	{
	}

	private void MoveGuestCharacter()
	{
	}

	private void SendGuestPositionToGuest()
	{
	}

	private void GetGuestPositionInGuest(byte[] netPositionBytes)
	{
	}

	private void SetGuestInputs(InputSet inputSet)
	{
	}

	private void OnDestroy()
	{
	}
}
