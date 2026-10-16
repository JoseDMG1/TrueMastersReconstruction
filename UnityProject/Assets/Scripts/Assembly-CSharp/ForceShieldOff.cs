using UnityEngine;

public class ForceShieldOff : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x123287C: unconditional interruption, unlike StopBlocking's
		// BlockHit exit exception. Dispatch uses Description.element (+0x28).
		animator.SetBool("Blocking", false);
		switch (animator.GetComponent<Description>().element)
		{
			case "Air": animator.GetComponent<AirShieldController>().shieldOff(); break;
			case "Water": animator.GetComponent<WaterShieldController>().shieldOff(); break;
			case "Earth": animator.GetComponent<ShieldController>().shieldOff(); break;
			case "Fire": animator.GetComponent<ShieldMannager>().shieldOff(); break;
			case "Light": animator.GetComponent<LightShieldManager>().shieldOff(); break;
			case "Shadow": animator.GetComponent<ShadowHandController>().ShieldOff(); break;
		}
		// Native has no Leaf dispatch; retain that distinction.
		Transform grab = animator.transform.Find("ShadowHandGrab(Clone)");
		if (grab) grab.GetComponent<ShadowHandGrabController>().ForceDisapear();
		// The original also sends StopNetBlock to Photon peers when online
		// and locally owned. That service path remains unreconstructed.
	}
}
