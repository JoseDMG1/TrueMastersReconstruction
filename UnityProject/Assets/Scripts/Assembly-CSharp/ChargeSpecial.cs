using UnityEngine;

public class ChargeSpecial : StateMachineBehaviour
{
	private string lastAtk;

	private string newAtk;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Native 0x122042C uses the next state and next clip, rather than
		// the stateInfo argument (which describes the entered state).
		if (GameController.i.online && !animator.GetComponent<Photon.Pun.PhotonView>().IsMine) return;
		if (animator.GetNextAnimatorStateInfo(0).IsName(lastAtk)) return;
		var clips = animator.GetNextAnimatorClipInfo(0);
		// Reconstruction guard: imported transitions can briefly have no clip.
		if (clips.Length == 0 || !clips[0].clip) return;
		newAtk = clips[0].clip.name;
		lastAtk = newAtk;
		if (SpecialAtk.specialAtk) SpecialAtk.specialAtk.ChargeSpecial(.03f);
	}
}
