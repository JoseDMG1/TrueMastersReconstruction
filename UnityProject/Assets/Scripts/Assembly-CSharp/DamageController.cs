using UnityEngine;
using UnityEngine.Events;

public class DamageController : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private TCPConnection tcpConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	public MovePlayersInHost_LM playersMovement;

	[SerializeField]
	private HitController hitController;

	public AnimationController_LM animationController;

	public UnityEvent<float> onPlayerHPChanged;

	public Special_LM special_LM;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void PlayAndSendHitAnimation(GameObject player, GameObject atk)
	{
	}

	public void SendPlayerHp(int playerID, float hp)
	{
	}

	public void ResetHP(PlayerController_LM playerController)
	{
	}

	public void UpdateHealthUI(int playerID, float hp)
	{
	}

	private void GetPlayerHp(byte[] data)
	{
	}
}
