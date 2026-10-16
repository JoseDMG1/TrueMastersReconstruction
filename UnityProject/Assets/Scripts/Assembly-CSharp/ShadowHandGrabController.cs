using UnityEngine;

public class ShadowHandGrabController : MonoBehaviour
{
	private Animator anim;

	public float disapearTime;

	public GameObject magicCircle;

	public GameObject darkFire;

	public GameObject explotion;

	private void Start()
	{
		anim = GetComponent<Animator>(); Invoke(nameof(ForceDisapear), disapearTime);
	}

	public void ForceDisapear()
	{
		anim.SetTrigger("Off");
		if (magicCircle) LerpManager.i.LerpScale(magicCircle, magicCircle.transform.localScale, Vector3.zero, 1f, (LerpEasing)2, 0f);
		if (darkFire) LerpManager.i.LerpScale(darkFire, darkFire.transform.localScale, Vector3.zero, 1f, (LerpEasing)2, 0f);
	}

	public void Explote()
	{
		if (explotion) Instantiate(explotion, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
