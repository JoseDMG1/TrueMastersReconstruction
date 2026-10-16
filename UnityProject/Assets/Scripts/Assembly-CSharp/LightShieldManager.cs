using UnityEngine;

public class LightShieldManager : MonoBehaviour
{
	private Animator anim;

	public ParticleSystem shield;

	public AudioSource sound;

	private bool On;

	public LightWeapon weapon;

	private void Start()
	{
		anim = GetComponent<Animator>();
		shield.Stop();
	}

	private void Update()
	{
	}

	public void shieldOn()
	{
		if (On) return;
		weapon.LightShield();
		sound.Play();
		shield.Play();
		On = true;
	}

	public void shieldOff()
	{
		if (!On) return;
		weapon.DarkShield();
		sound.Stop();
		shield.Stop();
		On = false;
	}
}
