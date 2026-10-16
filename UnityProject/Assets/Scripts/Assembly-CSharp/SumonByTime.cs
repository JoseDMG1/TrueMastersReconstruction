using UnityEngine;

public class SumonByTime : MonoBehaviour
{
	public GameObject create;

	public float time = 1f;

	public bool attack;

	public bool destroyAfterSumon;

	public bool copyParentRotation = true;

	public bool ignoreIfCollides;

	public bool ignoreIfWeak;

	private void Start()
	{
		Invoke(nameof(Sumon), time);
	}

	private void Update()
	{
	}

	private void Sumon()
	{
		if (ignoreIfWeak && GetComponent<ConfigAttackLayer>().playerWeak) return;
		if (ignoreIfCollides && !GetComponent<BoxCollider>().enabled) return;
		if (GameController.i.IsOutsideStage(transform.position, 27f)) return;
		// Original network branch requires server ownership. Offline reconstruction only.
		if (Photon.Pun.PhotonNetwork.IsConnected)
		{
			Debug.LogWarning("Network timed projectile creation is unavailable.", this);
			return;
		}
		// Native uses a zero Quaternion when rotation copying is disabled.
		Quaternion rotation = copyParentRotation ? transform.rotation : new Quaternion(0f, 0f, 0f, 0f);
		var obj = Instantiate(create, transform.position, rotation);
		if (attack) obj.layer = gameObject.layer;
		// No AttackLayerSetUp dispatch exists in this native producer.
		if (destroyAfterSumon) Destroy(gameObject);
	}
}
