using System.Collections.Generic;
using UnityEngine;

public class LeafShieldController : MonoBehaviour
{
	public GameObject leafShield;

	public List<Animator> leafAnimatorList;

	public Vector3 leafSize;

	public ParticleSystem leafShieldExplotion;

	private void Start()
	{
	}

	public void ShieldOn()
	{
		leafShield.GetComponent<ChangeSize>().ChangeSizeTo(leafSize);
	}

	public void ShieldOff()
	{
		leafShield.GetComponent<ChangeSize>().ChangeSizeTo(Vector3.zero);
	}

	public void ShieldHit()
	{
		// Native 0x1240FA0 calls Animator.Play(string), not SetTrigger.
		foreach (Animator leaf in leafAnimatorList) leaf.Play("Hit");
		leafShieldExplotion.Play();
	}

	private void Update()
	{
	}
}
