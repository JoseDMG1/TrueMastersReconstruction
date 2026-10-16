using Photon.Pun;
using UnityEngine;

public class GoForward : MonoBehaviour
{
	public float speed;

	private PhotonView PV;

	public float delay;

	public bool playerWeak;

	public float stopAfterTime;

	public bool stop;

	private void Start()
	{
		PV = GetComponent<PhotonView>();
		speed *= 52.75f;
		if (GameController.i && GameController.i.online) { Destroy(this); return; }
		if (stopAfterTime != 0f) Invoke(nameof(Stop), stopAfterTime);
		if (delay != 0f) { stop = true; Invoke(nameof(RestoreStop), delay); }
	}

	public void Stop()
	{
		stop = true;
	}

	public void RestoreStop()
	{
		stop = false;
	}

	private void Update()
	{
		if (stop) return;
		var configuration = GetComponent<ConfigAttackLayer>();
		if (configuration && configuration.playerWeak)
			transform.position += (-transform.up * .35f + transform.forward * .25f) * speed * Time.deltaTime;
		else transform.position += transform.forward * speed * Time.deltaTime;
	}

	private void IsPlayerIsOk()
	{
		var owner = gameObject.layer == 9 ? HostPlayer.hostPlayer.gameObject : GuestPlayer.guestPlayer.gameObject;
		var state = owner.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		string falling = gameObject.layer == 9 ? "Fall 3" : "Fall3";
		if (state.normalizedTime < .5f && (state.IsName("hit1") || state.IsName("hit2") || state.IsName(falling)
			|| state.IsName("RightHit") || state.IsName("LeftHit") || state.IsName("Falling2") || state.IsName("FallingToLanding")))
			playerWeak = true;
	}
}
