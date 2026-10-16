using UnityEngine;

public class AddToAttackManager : MonoBehaviour
{
	private void Start()
	{
		AddAttackToList();
	}

	private void OnDestroy()
	{
		if (AttacksManager.i == null) return;
		if (gameObject.layer == 9) AttacksManager.i.MainCharAttacks.Remove(gameObject);
		else AttacksManager.i.OpponentAttacks.Remove(gameObject);
	}

	private void AddAttackToList()
	{
		if (gameObject.layer == 9) AttacksManager.i.MainCharAttacks.Add(gameObject);
		else AttacksManager.i.OpponentAttacks.Add(gameObject);
	}
}
