using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChangeColliderScale_LM : MonoBehaviour
{
	[SerializeField]
	private BoxCollider col;

	[SerializeField]
	private Vector3 startScale;

	[SerializeField]
	private Vector3 targetScale;

	[SerializeField]
	private float duration;

	[SerializeField]
	private float delay;

	private float timeElapsed;

	private bool running;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void StartScaling()
	{
	}

	private void Update()
	{
	}
}
