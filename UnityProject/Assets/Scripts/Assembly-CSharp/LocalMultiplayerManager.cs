using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LocalMultiplayerManager : MonoBehaviour
{
	public GameObject start_UI;

	public GameObject create_UI;

	public GameObject Waiting_UI;

	public GameObject roomsNotFound;

	public GameObject searchRoomUI;

	public GameObject roomListUI;

	public UnityEvent onResetUI;

	private void Start()
	{
	}

	public void ShowStartUI()
	{
	}

	public void ShowCreateUI()
	{
	}

	public void ShowWaitingUI()
	{
	}

	public void ShowSearchRoomUI()
	{
	}

	public void ShowRoomsNotFound()
	{
	}

	public void ShowRoomsListUI()
	{
	}

	private void ToggleUI(GameObject uiToShow)
	{
	}

	public void GoToMainMenu()
	{
		if (LevelChanger.i != null) LevelChanger.i.MainMenu();
		else SceneManager.LoadScene(1);
	}
}
