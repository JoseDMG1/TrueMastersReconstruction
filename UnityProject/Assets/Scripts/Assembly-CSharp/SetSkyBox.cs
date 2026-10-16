using UnityEngine;

public class SetSkyBox : MonoBehaviour
{
	public Material skybox;

	private void Awake()
	{
		// Native VA 0x12AE268: stage owns the selected skybox assignment.
		RenderSettings.skybox = skybox;
	}

	private void Update()
	{
	}
}
