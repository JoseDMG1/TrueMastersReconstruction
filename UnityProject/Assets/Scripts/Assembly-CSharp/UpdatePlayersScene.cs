using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatePlayersScene : MonoBehaviour
{
	[SerializeField]
	private TCPConnection TCPConnection;

	[SerializeField]
	private LocalMultiplayerData localData;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void CheckIfPlayersAreInSameScene()
	{
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	private void NotifyOpponent(int indexScene)
	{
	}

	private void MessageHandler(byte[] message)
	{
	}
}
