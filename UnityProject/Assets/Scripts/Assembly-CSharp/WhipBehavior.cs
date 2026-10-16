using UnityEngine;
// Corrected ARM64 0x12DD2AC..0x12DD344.
public class WhipBehavior : StateMachineBehaviour
{
    public bool left;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var whip = animator.gameObject.GetComponent<WhipController>();
        if (left) whip.LeftWhipAtk(); else whip.RightWhipAtk();
    }
}
