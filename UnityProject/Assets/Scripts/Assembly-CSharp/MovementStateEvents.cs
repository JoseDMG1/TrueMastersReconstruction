using UnityEngine;
using UnityEngine.Events;

public class MovementStateEvents : StateMachineBehaviour
{
	public UnityEvent<int> onMovementStateEnter;

	public UnityEvent<int> onMovementStateExit;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x1244A68 is a local-multiplayer component callback.
		onMovementStateEnter?.Invoke(animator.gameObject.layer);
		var player = animator.GetComponent<PlayerController_LM>();
		if (!player) return;
		player.stuned = false;
		player.isBlocking = false;
		var shield = animator.GetComponent<ShieldManager>();
		if (shield) shield.FinishShield();
		player.weak = false;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onMovementStateExit?.Invoke(animator.gameObject.layer);
	}
}
