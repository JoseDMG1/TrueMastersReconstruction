using UnityEngine;

public class DestroyAtkController : MonoBehaviour
{
	public AttacksController_LM atkController;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void DestroyAndSendDestroyMsg(GameObject attack, bool byTime = false)
	{
	}

	private void DestroyAttack(GameObject attack)
	{
	}

	private void SendDestroyAttackMessage(int attackID)
	{
	}

	private void OnDestroyAtkReceived(byte[] message)
	{
	}
}
