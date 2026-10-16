using Photon.Pun;
using UnityEngine;

public class TrapPowerUp : MonoBehaviour
{
	public float limitDistance = 4f;

	public GameObject powerUpFX;

	public GameObject TrapFX;

	private bool powered;

	private bool directionChanged;

	public float startDelay = .5f;

	public float reboundDelay = .5f;

	public GameObject ScaleObject;

	public bool Jump;

	public Animator animator;

	private void Start()
	{
		directionChanged = true;
		Invoke(nameof(RestoreDirectionChanged), startDelay);
	}

	private void Update()
	{
		// Photon authority/RPC transport is outside the offline reconstruction.
		if (GameController.i && GameController.i.online) return;
		if (!directionChanged) CheckIfTrapNear();
	}

	public void CheckIfTrapNear()
	{
		if (GameController.i && GameController.i.online) return;
		var attacks = gameObject.layer == 9 ? AttacksManager.i.MainCharAttacks : AttacksManager.i.OpponentAttacks;
		foreach (var attack in attacks)
		{
			if (!attack || !Utilities.i.AreObjetsNear(attack, gameObject, limitDistance)) continue;
			if (attack.GetComponent<Description>().effect != "ShadowTramp") continue;
			Instantiate(TrapFX, transform.position, transform.rotation);
			ChangeDirection();
			PowerUpAttack();
		}
	}

	public void PowerUpAttack()
	{
		if (powered) return;
		var description = GetComponent<Description>();
		description.Damage += 1f;
		description.Defense += 1f;
		description.Damage = Mathf.Min(description.Damage, 3f);
		if (ScaleObject) ScaleObject.transform.localScale *= 1.25f;
		powerUpFX.SetActive(true);
		if (Jump)
		{
			GetComponent<jumpSimulator>().enabled = true;
			animator.SetTrigger("Jump");
		}
		powered = true;
	}

	public void ChangeDirection()
	{
		var opponent = gameObject.layer == 9 ? GuestPlayer.guestPlayer.gameObject : HostPlayer.hostPlayer.gameObject;
		transform.LookAt(opponent.transform);
		float yaw = Utilities.i.QuaternionToVector3(transform.rotation).y;
		transform.rotation = Quaternion.Euler(0f, yaw, 0f);
		directionChanged = true;
		Invoke(nameof(RestoreDirectionChanged), reboundDelay);
	}

	private void RestoreDirectionChanged()
	{
		directionChanged = false;
	}

	[PunRPC]
	public void ShowOnlineFX()
	{
		// Local receiver body only; no Photon RPC transport is reconstructed.
		PowerUpAttack();
		Instantiate(TrapFX, transform.position, transform.rotation);
	}
}
