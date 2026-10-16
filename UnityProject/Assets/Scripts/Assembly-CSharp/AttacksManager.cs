using System.Collections.Generic;
using UnityEngine;

public class AttacksManager : MonoBehaviour
{
	public List<GameObject> MainCharAttacks;

	public List<GameObject> OpponentAttacks;

	public static AttacksManager i;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) Destroy(this);
		if (MainCharAttacks == null) MainCharAttacks = new List<GameObject>();
		if (OpponentAttacks == null) OpponentAttacks = new List<GameObject>();
	}

	public List<GameObject> GetOwnAttacks(int layer)
	{
		return layer == 8 ? OpponentAttacks : MainCharAttacks;
	}
}
