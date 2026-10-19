using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChangeOnClick : MonoBehaviour
{
	public int level;
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		if (button == null) button = GetComponentInParent<Button>();
		if (button == null)
		{
			Debug.LogError("LevelChangeOnClick requires a Button on this object or its parent.", this);
			return;
		}
		for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
		{
			string method = button.onClick.GetPersistentMethodName(i);
			if (!string.IsNullOrEmpty(method) && method != "HideCrystalsNotification")
				return;
		}
		button.onClick.AddListener(ChangeLevel);
	}

	public void ChangeLevel()
	{
		if (level == 1 && LevelChanger.i != null)
		{
			LevelChanger.i.MainMenu();
			return;
		}
		if (LevelChanger.i != null) LevelChanger.i.FadeToLevel(level);
		else SceneManager.LoadScene(level);
	}

	private void OnDestroy()
	{
		if (button != null) button.onClick.RemoveListener(ChangeLevel);
	}
}
