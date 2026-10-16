using UnityEngine;

public class ShieldMannager : MonoBehaviour
{
	private Animator anim;

	public ParticleSystem shield;

	public AudioSource sound;

	private bool On;

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
		sound.volume = .3f; sound.pitch = 1f; sound.Play(); shield.Play(); On = true;
	}

	public void shieldOff()
	{
		if (!On) return;
		sound.volume = .2f; sound.pitch = 1.5f; sound.Play(); shield.Stop(); On = false;
	}
}
