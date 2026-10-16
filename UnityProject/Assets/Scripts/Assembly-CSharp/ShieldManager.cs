using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
	public List<GameObject> shieldList;

	public GameObject ShieldPrefab;

	private IShield currentShieldLogic;

	private LocalMultiplayerData localData;

	private TCPConnection tcpConnection;

	public PlayerController_LM playerController;

	public bool isOn;

	public AvatarSwapper avatarSwapper;

	private void Awake()
	{
	}

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void StartShield()
	{
	}

	public void FinishShield()
	{
	}

	public void ShowShieldHitAnimation()
	{
	}

	public void ShieldBreak()
	{
	}

	public void SendShieldBehavior(ShieldBehavior shieldBehavior)
	{
	}

	public void GetShieldBehavior(byte[] data)
	{
	}
}
