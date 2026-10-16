using Photon.Pun;
using UnityEngine;

public class SummonOnCollision : MonoBehaviour
{
	public GameObject Create;

	public bool SummonOnCollider;

	public bool ForceNetworkSumon;

	private PhotonView PV;

	private void Start()
	{
		PV = GetComponent<PhotonView>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (GameController.i && GameController.i.online) return;
		if (!collider || !collider.GetComponent<Description>() || collider.gameObject.layer == gameObject.layer) return;
		Summon(collider);
	}

	public void ChangeColor(GameObject obj, Material newMat)
	{
		var changer = obj.GetComponent<ChangeRendererMat>();
		if (changer) changer.newMat = newMat;
		foreach (var particles in obj.GetComponentsInChildren<ParticleSystem>())
		{
			var renderer = particles.GetComponent<ParticleSystemRenderer>();
			renderer.material = newMat; renderer.trailMaterial = newMat;
		}
	}

	public void Summon(Collider collider)
	{
		if (!Create) return;
		Transform source = SummonOnCollider && collider ? collider.transform : transform;
		Vector3 position = source.position;
		if (!SummonOnCollider && IsFireball()) position += Vector3.up * 1.5f;
		var obj = Instantiate(Create, position, source.rotation);
		var changer = GetComponent<ChangeRendererMat>();
		if (changer) changer.ChangeColor(obj);
	}

	public bool IsFireball()
	{
		var description = GetComponent<Description>();
		return description && description.category == "Fireball";
	}

	[PunRPC]
	public void NetworkSummon(bool ignoreSumonOnCollider)
	{
		// Native name notwithstanding, offline DestroyObject uses this local creation path.
		if (GameController.i && GameController.i.online)
		{
			Debug.LogWarning("Network collision summoning is unavailable.", this); return;
		}
		Transform source = transform;
		if (SummonOnCollider)
		{
			var opponent = GameController.i.MainCharacter.layer == gameObject.layer
				? GameController.i.OpponentCharacter : GameController.i.MainCharacter;
			if (opponent) source = opponent.transform;
		}
		if (!Create) return;
		var position = source.position;
		if (IsFireball()) position += Vector3.up * 1.5f;
		var obj = Instantiate(Create, position, transform.rotation);
		var changer = GetComponent<ChangeRendererMat>();
		if (changer) changer.ChangeColor(obj);
	}
}
