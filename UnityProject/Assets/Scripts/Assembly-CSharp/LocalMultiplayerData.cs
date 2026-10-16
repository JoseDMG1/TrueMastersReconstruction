using UnityEngine;
using UnityEngine.Events;

public class LocalMultiplayerData : MonoBehaviour
{
	public CharacterName HostCharacter;

	public CharacterName GuestCharacter;

	public int stage;

	public int gameSpeed;

	public bool isHost;

	public int timer;

	public int guestScene;

	public int hostScene;

	public bool battleStarted;

	public UnityEvent onPlayerTypeChanged;

	public UnityEvent onPlayersInSameScene;

	public UnityEvent onPlayersReachBattleScene;

	public bool PlayersInSameScene()
	{
		return false;
	}

	public void SetHostScene(int scene)
	{
	}

	public void SetGuestScene(int scene)
	{
	}

	private void CheckAndTriggerSceneEvents()
	{
	}

	public void ChangePlayerType(bool hostType)
	{
	}
}
