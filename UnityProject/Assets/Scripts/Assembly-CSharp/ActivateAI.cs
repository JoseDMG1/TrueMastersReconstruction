using UnityEngine;
public class ActivateAI : StateMachineBehaviour
{
 public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 {
  var ai = animator.GetComponent<AI>();
  if (ai) ai.Active = true;
 }
}
