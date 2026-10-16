using UnityEngine;

public class ShadowHandController : MonoBehaviour
{
	public Animator shadowHandAnimator;

	private Animator anim;

	public ParticleSystem handFX;

	public AudioSource HandFXSound;

	public ParticleSystem floorDodgeFX;

	public AudioSource floorFXSound;

	public GameObject ShadowHandFloorFX;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void ShieldOn()
	{
		shadowHandAnimator.SetBool("On", true);
		LerpManager.i.LerpScale(ShadowHandFloorFX, Vector3.zero,
			Vector3.one * 15f, .5f, LerpEasing.EasyInOut, 0f);
	}

	public void StopEnemy()
	{
		GameController.i.OpponentCharacter.GetComponent<Animator>().Play("PrisonFree", 0, 0f);
	}

	public void ShieldOff()
	{
		if (!shadowHandAnimator.GetBool("On")) return;
		shadowHandAnimator.SetBool("On", false);
		LerpManager.i.LerpScale(ShadowHandFloorFX, Vector3.one * 15f,
			Vector3.zero, .5f, LerpEasing.EasyInOut, 0f);
	}

	public void ShadowBlockHit()
	{
		shadowHandAnimator.Play("ShadowBlockHit", 0, 0f);
	}

	public void ShowHandFX()
	{
		handFX.Play();
		HandFXSound.Play();
	}

	public void ShadowFloorFX()
	{
		floorDodgeFX.Play();
		floorFXSound.Play();
	}
}
