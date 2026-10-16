using UnityEngine;

public class StopGo : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x12C5CD0 checks next state, not the current state.
		var next = animator.GetNextAnimatorStateInfo(0);
		if (next.IsName("hit1") || next.IsName("hit2") || next.IsName("Fall 3"))
			animator.GetComponent<AnimationMovement>().stopGo();
	}
}
