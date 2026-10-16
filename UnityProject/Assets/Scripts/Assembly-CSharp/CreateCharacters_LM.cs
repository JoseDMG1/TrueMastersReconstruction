using UnityEngine;

public class CreateCharacters_LM : MonoBehaviour, IBattleTask
{
	[SerializeField]
	private TCPConnection TCPConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	[SerializeField]
	private DamageController damageController;

	[SerializeField]
	private AttacksController_LM attacksController;

	[SerializeField]
	private Special_LM specialController;

	[SerializeField]
	private ScoreController_LM scoreController;

	public void Init()
	{
	}

	public void Reset()
	{
	}

	public void SpawnCharacters()
	{
	}
}
