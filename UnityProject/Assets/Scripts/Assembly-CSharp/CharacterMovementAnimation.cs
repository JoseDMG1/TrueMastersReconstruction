using UnityEngine;

public class CharacterMovementAnimation : MonoBehaviour
{
	public Animator animator;

	private Vector3 lastPosition;

	[SerializeField]
	private float smoothingSpeed;

	private float smoothedHorizontal;

	private float smoothedVertical;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
