using Photon.Pun;
using UnityEngine;

public class SummonAttack : MonoBehaviour
{
	public string follow;

	private PhotonView PV;

	public Material newMat;

	private bool jumpAtk;

	private void Start()
	{
		PV = GetComponent<PhotonView>();
	}

	public void ChangeColor(GameObject obj)
	{
		var changer = obj.GetComponent<ChangeRendererMat>();
		if (changer != null) { changer.newMat = newMat; changer.ChangeColor(obj); }
	}

	public void Create(GameObject creation)
	{
		var obj = Instantiate(creation, transform.position, transform.rotation);
		obj.layer = gameObject.layer;
		if (newMat != null) ChangeColor(obj);
	}

	public void CreateAsChild(GameObject creation)
	{
		var obj = Instantiate(creation, transform.position, transform.rotation);
		obj.layer = gameObject.layer;
		obj.transform.parent = transform;
	}

	public void CreateAwakeAtk(GameObject creation)
	{
		if (GameController.i.online || !GetComponent<AnimatorStatesManager>().awakening || jumpAtk) return;
		jumpAtk = true;
		bool leaf = GetComponent<Description>().element == "Leaf";
		Invoke(nameof(RestoreJumpAtk), leaf ? 2f : 3f);
		if (leaf && GameController.i.GetDistancesBetweenPlayers() < 5f) return;
		CreateAttack(creation);
	}

	public void CreateAwakeAtkNet(GameObject creation)
	{
		if (GameController.i.online) Debug.LogWarning("Network awakening attack is unavailable.", this);
	}

	private void RestoreJumpAtk()
	{
		jumpAtk = false;
	}

	public void CreateNetworkAttack(GameObject creation)
	{
		if (GameController.i.online) Debug.LogWarning("Network projectile creation is unavailable.", this);
	}

	public void CreateSimpleAtk(GameObject creation)
	{
		if (WaterShieldExists(creation)) return;
		var obj = Instantiate(creation, transform.position, transform.rotation);
		obj.layer = gameObject.layer;
		GameEvents.i.AttackLayerSetUp(obj.GetEntityId());
	}

	public void CreateBranchAttack(GameObject creation)
	{
		Instantiate(creation, transform.position, transform.rotation);
	}

	public void CreateAttack(GameObject creation)
	{
		if (GameController.i.online || WaterShieldExists(creation)) return;
		var obj = Instantiate(creation, transform.position, transform.rotation);
		obj.layer = gameObject.layer;
		GameEvents.i.AttackLayerSetUp(obj.GetEntityId());
		if (newMat != null) ChangeColor(obj);
	}

	private bool WaterShieldExists(GameObject creation)
	{
		if (creation.name != "WaterShieldLeft" && creation.name != "WaterShieldRight") return false;
		foreach (var attack in AttacksManager.i.GetOwnAttacks(gameObject.layer))
			if (attack != null && attack.name == creation.name + "(Clone)" && attack.layer == gameObject.layer) return true;
		return false;
	}

	public void CreateNetworkKickAttack(GameObject creation)
	{
		if (GameController.i.online) Debug.LogWarning("Network kick attack is unavailable.", this);
	}

	public void CreateKickAttack(GameObject creation)
	{
		if (GameController.i.online) return;
		var target = CompareTag("HostPlayer") ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		var description = creation.GetComponent<Description>();
		// Native 0x12B5CB8/0x12B5E08 reads Description +0x40 (effect).
		string effect = description.effect;
		if (effect != "LeftAttack" && effect != "RightAttack") return;
		float direction = effect == "LeftAttack" ? -1f : 1f;
		Vector3 position = target.position + target.right * (5f * direction);
		if (position.magnitude > 23f) position = target.position + target.right * (3f * direction);
		var rotation = transform.rotation * Quaternion.Euler(0f, direction * 90f, 0f);
		var obj = Instantiate(creation, position, rotation);
		obj.layer = gameObject.layer;
	}
}
