using UnityEngine;

public class StopBlocking : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool("Blocking", true);
		SetShield(animator, true);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("BlockHit")) return;
		animator.SetBool("Blocking", false);
		SetShield(animator, false);
	}

	private static void SetShield(Animator animator, bool active)
	{
		// Native dispatch 0x12B34D4/0x12B3B98, based on Description.element.
		switch (animator.GetComponent<Description>().element)
		{
			case "Air": if (active) animator.GetComponent<AirShieldController>().shieldOn(); else animator.GetComponent<AirShieldController>().shieldOff(); break;
			case "Fire": if (active) animator.GetComponent<ShieldMannager>().shieldOn(); else animator.GetComponent<ShieldMannager>().shieldOff(); break;
			case "Earth": if (active) animator.GetComponent<ShieldController>().shieldOn(); else animator.GetComponent<ShieldController>().shieldOff(); break;
			case "Water": if (active) animator.GetComponent<WaterShieldController>().shieldOn(); else animator.GetComponent<WaterShieldController>().shieldOff(); break;
			case "Light": if (active) animator.GetComponent<LightShieldManager>().shieldOn(); else animator.GetComponent<LightShieldManager>().shieldOff(); break;
			case "Shadow": if (active) animator.GetComponent<ShadowHandController>().ShieldOn(); else animator.GetComponent<ShadowHandController>().ShieldOff(); break;
			case "Leaf": if (active) animator.GetComponent<LeafShieldController>().ShieldOn(); else animator.GetComponent<LeafShieldController>().ShieldOff(); break;
		}
		// The original also sends Photon RPCs when online; that service path
		// remains outside this offline reconstruction.
	}
}
