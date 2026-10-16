using UnityEngine;

public class jumpSimulator : MonoBehaviour
{
	public AnimationCurve animCurve;

	private float timeElapsed;

	public float duration;

	public float force;

	public float delay;

	private void Start()
	{
		if (GameController.i && GameController.i.online)
		{
			var view = GetComponent<Photon.Pun.PhotonView>();
			if (view && !view.IsMine) Destroy(this);
		}
	}

	private void Update()
	{
		if (timeElapsed > duration)
		{
			var position = transform.position;
			position.y = animCurve.Evaluate(1f) * force;
			transform.position = position;
			Destroy(this);
		}
		// Native increments the clock only after this gate; preserve authored delay semantics.
		if (timeElapsed < delay) return;
		var current = transform.position;
		current.y = animCurve.Evaluate(timeElapsed / duration) * force;
		transform.position = current;
		timeElapsed += Time.deltaTime;
	}
}
