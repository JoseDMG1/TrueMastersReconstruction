using UnityEngine;

public class DropWhenOwnerWeak : MonoBehaviour
{
	private bool drop;

	public float speed = 1f;

	private void Start()
	{
		InvokeRepeating(nameof(CheckOwner), .25f, .25f);
	}

	private void Update()
	{
		if (!drop) return;
		var follow = GetComponent<FollowOwner>();
		if (follow) follow.enabled = false;
		// Native per-render-frame displacement, without deltaTime.
		transform.position -= transform.up * speed * .35f;
	}

	public void CheckOwner()
	{
		Component owner = gameObject.layer == 8 ? (Component)GuestPlayer.guestPlayer : HostPlayer.hostPlayer;
		if (!owner) return;
		var animator = owner.GetComponent<Animator>();
		// Once latched, the original never clears drop or resumes following.
		if (animator && animator.GetBool("Weak")) drop = true;
	}
}
