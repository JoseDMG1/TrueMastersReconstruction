using UnityEngine;
// Corrected ARM64 0x1227560..0x1227680: destroy only the same-layer prison.
public class DestroyIcePrison : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var prison = IcePrisonController.i;
        if (prison != null && prison.gameObject.layer == animator.gameObject.layer)
            prison.gameObject.GetComponent<DestroyByTime>().ForceDestroy();
    }
}
