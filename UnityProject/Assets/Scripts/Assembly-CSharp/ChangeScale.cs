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
	}

	private void Start()
	{
	}
}
