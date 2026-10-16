using UnityEngine;

public class InputAnimationController : MonoBehaviour
{
	public AnimationController_LM animationController;

	public void StartAnimation(InputSet currentInputs, GameObject player)
	{
	}

	private NetAnimation GetAnimationName(string playerElement, string animationType)
	{
		return NetAnimation.Movement;
	}

	private void HandleHorizontalInput(InputSet currentInputs, string playerElement, string movesetIndex, GameObject player)
	{
	}

	private void HandleVerticalInput(InputSet currentInputs, string playerElement, string movesetIndex, GameObject player)
	{
	}
}
