using UnityEngine;

public class RecoverReduceCollider : StateMachineBehaviour
{
	public bool reduce;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x1279294.
		var collider = animator.GetComponent<ColliderController>();
		if (reduce) collider.reduceCollider();
		else { collider.recoverCollider(); animator.SetBool("Weak", false); }
	}
}
