using UnityEngine;

public class AirShieldController : MonoBehaviour
{
	public GameObject shield;

	public bool IncreaseSize;

	public bool DecreaseSize;

	public float speed;

	public float dspeed;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		// Native 0x127D880: per physics tick lerps and volume increments,
		// without deltaTime. Retain the original .6 shrink target.
		var audio = shield.GetComponent<AudioSource>();
		if (shield.transform.localScale.y < 1f && IncreaseSize)
		{
			shield.SetActive(true);
			shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, Vector3.one, speed);
			if (audio.volume < .05f) audio.volume += .005f;
			if (shield.transform.localScale.y > 1f) { IncreaseSize = false; audio.volume = .05f; }
		}
		if (shield.transform.localScale.y > 0f && DecreaseSize)
		{
			shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, Vector3.one * .6f, dspeed);
			audio.volume -= .01f;
			if (shield.transform.localScale.y < .7f && !anim.GetNextAnimatorStateInfo(0).IsName("BlockHit") && !anim.GetCurrentAnimatorStateInfo(0).IsName("BlockHit"))
			{
				shield.SetActive(false); DecreaseSize = false; audio.volume = 0f;
			}
		}
	}

	public void shieldOn()
	{
		IncreaseSize = true; DecreaseSize = false;
	}

	public void shieldOff()
	{
		IncreaseSize = false; DecreaseSize = true;
	}
}
