using UnityEngine;

public class SlowSpeed : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x12BEEFC; entry callback is a RET.
		animator.GetComponent<AnimationMovement>().goHorizontal(.1f);
	}
}
