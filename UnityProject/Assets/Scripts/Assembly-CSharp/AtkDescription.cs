using UnityEngine;

public class AtkDescription : MonoBehaviour
{
	public int id;

	public PlayerType owner;

	public int damage;

	public int hp;

	public AtkEffect effect;

	public AttacksController_LM attacksController;

	public FxController fxController;

	public bool indestructible;

	public bool unblockable;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
