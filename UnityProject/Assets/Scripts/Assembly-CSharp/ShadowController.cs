using DynamicShadowProjector;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
	private DrawTargetObject shadowConfig;
	private GraphicsManager graphicsOwner;

	private void Start()
	{
		// Native 0x12BD11C: retain the publisher for symmetric cleanup.
		graphicsOwner = GraphicsManager.i;
		if (graphicsOwner) graphicsOwner.graphicsChange += ChangeShadow;
		shadowConfig = GetComponent<DrawTargetObject>();
		var light = GameObject.FindGameObjectWithTag("MainLight");
		if (shadowConfig && light) shadowConfig.targetDirection = light.transform;
		ChangeShadow();
	}

	private void OnDestroy()
	{
		if (graphicsOwner) graphicsOwner.graphicsChange -= ChangeShadow;
	}

	private void ChangeShadow()
	{
		// Native 0x12BD260 compares exactly 1, not a quality threshold.
		gameObject.SetActive(PlayerPrefs.GetInt("userQualityGraphics", 0) == 1);
	}
}
