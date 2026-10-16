using UnityEngine;

public class WaterShieldController : MonoBehaviour
{
	private Animator anim;

	public GameObject RightShield;

	public GameObject LeftShield;

	public GameObject FrontShield;

	public ParticleSystem dust;

	public bool RightShieldOn;

	public bool LeftShieldOn;

	private void Start()
	{
		anim = GetComponent<Animator>();
		InvokeRepeating(nameof(CheckShields), .5f, .5f);
	}

	private void CheckShields()
	{
		// Native 0x12BBEBC retains either flag while at least one matching
		// shield exists; it clears both only when neither shield was found.
		bool found = false;
		foreach (GameObject attack in AttacksManager.i.GetOwnAttacks(gameObject.layer))
		{
			if (attack.name == "WaterShieldLeft(Clone)" && attack.layer == gameObject.layer)
			{
				LeftShieldOn = true;
				found = true;
			}
			if (attack.name == "WaterShieldRight(Clone)" && attack.layer == gameObject.layer)
			{
				RightShieldOn = true;
				found = true;
			}
		}
		if (!found) { LeftShieldOn = false; RightShieldOn = false; }
	}

	private void Update()
	{
	}

	public void WLeftShieldOn()
	{
		LeftShield.SetActive(true);
	}

	public void WLeftShieldOff()
	{
		LeftShield.SetActive(false);
	}

	public void shieldOn()
	{
		Debug.Log("FrontShield on");
		FrontShield.GetComponent<Animator>().SetBool("On", true);
		anim.SetBool("Blocking", true);
		if (!dust.isPlaying) dust.Play();
	}

	public void shieldOff()
	{
		Debug.Log("FrontShield off");
		FrontShield.GetComponent<Animator>().SetBool("On", false);
		anim.SetBool("Blocking", false);
		if (dust.isPlaying) dust.Stop();
	}

	public void ActiveRightShield()
	{
		RightShieldOn = true;
	}

	public void ActiveLeftShield()
	{
		LeftShieldOn = true;
	}

	public void DeactiveRightShield()
	{
		RightShieldOn = false;
	}

	public void DeactiveLeftShield()
	{
		LeftShieldOn = false;
	}
}
