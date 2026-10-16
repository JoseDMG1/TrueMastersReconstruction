using System.Collections.Generic;
using UnityEngine;

public class AttacksController_LM : MonoBehaviour, IBattleTask
{
	public List<GameObject> attackList;

	public DestroyAtkController destroyAtkController;

	public TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	[SerializeField]
	private FxController fxController;

	public Special_LM special_LM;

	private int currentAttackID;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void CreateAndSendAttack(GameObject attackPrefab, int playerLayer, int playerID, Vector3 pos, Quaternion rot)
	{
	}

	private void CreateAtk(GameObject attackPrefab, int atkLayer, Vector3 pos, Quaternion rot, int id, int playerID)
	{
	}

	public int GetAtkLayer(int parentLayer)
	{
		return 0;
	}

	public int GetParentLayer(int atkLayer)
	{
		return 0;
	}

	private void SendCreateAttackMessage(string atkName, int atkID, Vector3 pos, Quaternion rot, int atkLayer, int playerID)
	{
	}

	public void OnAttackReceived(byte[] message)
	{
	}

	private int ChangeAtkLayer(int atkLayer)
	{
		return 0;
	}
}
