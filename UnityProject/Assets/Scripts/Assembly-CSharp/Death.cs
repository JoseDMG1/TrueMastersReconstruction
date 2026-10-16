using UnityEngine;

public class Death : StateMachineBehaviour
{
    public bool death;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Native 0x12267C8 writes AnimatorStatesManager.death at offset0x40.
        var states = animator.gameObject.GetComponent<AnimatorStatesManager>();
        if (states != null) states.death = death;
    }
}
