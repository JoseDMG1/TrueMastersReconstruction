using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneSingleton : MonoBehaviour
{
	public static BattleSceneSingleton i;

	public InputReader inputReader;

	public GameObject MainCharacter;

	public GameObject Opponent;

	public GameObject PlayerControlledByHost;

	public GameObject PlayerControlledByGuest;

	public List<GameObject> players;

	public Counter counter;

	public CameraManager cameraManager;

	public AIToggle aiToggle;

	public StatsController statsController;

	public SpecialAtk SpecialAtk;

	public Image specialButtonOutline;

	public Image specialButtonFill;

	public Text specialButtonText;

	public TMP_Text mainCharScore;

	public TMP_Text opponentScore;

	public Score scoreManager;

	public GameObject WinnerText;

	public Button PlayAgain_LM;

	public Button PlayAgain_LM_ExitMenu;

	public TMP_Text finishText;

	public AudioSource timeOutSound;

	public GameObject finishMenu;

	private void Awake()
	{
		// Native 0x12170B4.
		i = this;
	}

	private void OnDestroy()
	{
		if (i == this) i = null;
	}

	public GameObject GetOpponent(int playerID)
	{
		return players[playerID == 0 ? 1 : 0];
	}

	public bool IsMainCharacter(int playerID)
	{
		return MainCharacter.GetComponent<PlayerController_LM>().playerID == playerID;
	}

	public float DistanceBetweenPlayers()
	{
		return Vector3.Distance(MainCharacter.transform.position, Opponent.transform.position);
	}
}
