using System.Collections;
using UnityEngine;

public class ChangeScale : MonoBehaviour
{
	public Vector3 startScale;

	public Vector3 targetScale;

	public float duration;

	[Header("Optional")]
	public float delay;

	public LerpEasing easing;

	private void Awake()
	{
		transform.localScale = startScale;
	}

	private void Start()
	{
		StartCoroutine(AnimateScale());
	}

	private IEnumerator AnimateScale()
	{
		if (delay > 0f) yield return new WaitForSeconds(delay);

		if (duration <= 0f)
		{
			transform.localScale = targetScale;
			yield break;
		}

		float elapsed = 0f;
		while (elapsed < duration)
		{
			float amount = elapsed / duration;
			int curveIndex = (int)easing;
			if (LerpManager.i != null && LerpManager.i.animCurves != null
				&& curveIndex >= 0 && curveIndex < LerpManager.i.animCurves.Length
				&& LerpManager.i.animCurves[curveIndex] != null)
				amount = LerpManager.i.AddEasing(elapsed, duration, easing);
			else if (easing == LerpEasing.EasyOut)
				amount = Mathf.Sin(amount * Mathf.PI * 0.5f);
			else if (easing == LerpEasing.EasyInOut)
				amount = Mathf.SmoothStep(0f, 1f, amount);

			transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, amount);
			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = targetScale;
	}
}
