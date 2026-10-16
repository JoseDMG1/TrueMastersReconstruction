using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection_LM : MonoBehaviour
{
	[SerializeField]
	private TCPConnection TCPConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	private bool hostReady;

	private bool guestReady;

	private void Start()
	{
	}

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	private void HandlerMessagesFromGuest(byte[] message)
	{
	}

	private void HandleMessagesFromHost(byte[] payload)
	{
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	private void SetUpStartButtonEvent()
	{
	}

	private void TryToStartGame(bool fromOpponentMessage = false)
	{
	}

	private void SendCharacterSelection()
	{
	}

	private void SetCharacterSelection()
	{
	}
}
