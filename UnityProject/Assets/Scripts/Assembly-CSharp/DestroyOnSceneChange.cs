using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
	[Header("Target Scene Index for Destruction")]
	public int targetSceneIndex;

	private void Start()
	{
	}

	private void OnSceneChanged(Scene current, Scene next)
	{
	}

	private void OnDestroy()
	{
	}
}
