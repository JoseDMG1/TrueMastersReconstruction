using UnityEngine;

public class Stunned : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x12B447C.
		animator.SetBool("Stunned", true);
	}
}
