using UnityEngine;

public class Recover : StateMachineBehaviour
{
	public bool keepStunned = true;

	public bool keepWeak;

	public bool keepBlocking;

	public bool keepColliderSmall;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x12A7384; the constructor defaults keepStunned to true.
		if (!keepStunned) animator.SetBool("Stunned", false);
		if (!keepWeak) animator.SetBool("Weak", false);
		if (!keepBlocking)
		{
			animator.SetBool("Blocking", false);
			animator.ResetTrigger("Block");
		}
		if (!keepColliderSmall)
		{
			var collider = animator.GetComponentInParent<ColliderController>();
			if (collider) collider.recoverCollider();
		}
	}
}
