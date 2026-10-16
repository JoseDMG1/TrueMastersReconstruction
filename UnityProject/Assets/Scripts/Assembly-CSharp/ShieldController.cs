using UnityEngine;

public class ShieldController : MonoBehaviour
{
	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
	}

	public void shieldOn()
	{
		// Native 0x12AEDD0: the Earth shield is child 3, not a spawned attack.
		transform.GetChild(3).GetComponent<Animator>().SetBool("On", true);
		anim.SetBool("Blocking", true);
	}

	public void shieldOff()
	{
		transform.GetChild(3).GetComponent<Animator>().SetBool("On", false);
		anim.SetBool("Blocking", false);
	}
}
