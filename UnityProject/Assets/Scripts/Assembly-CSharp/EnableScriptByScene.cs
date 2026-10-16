using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableScriptByScene : MonoBehaviour
{
	[SerializeField]
	private MonoBehaviour script;

	public bool disableOnSceneExit;

	public int sceneIndex;

	private void Start()
	{
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	private void OnDestroy()
	{
	}
}
