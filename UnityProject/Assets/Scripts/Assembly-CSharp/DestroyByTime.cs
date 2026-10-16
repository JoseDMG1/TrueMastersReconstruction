using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
	public float time;

	public GameObject _finalExplotion;

	private void Start()
	{
		if (_finalExplotion) Invoke(nameof(FinalAnimation), time);
		Destroy(gameObject, time);
	}

	private void FinalAnimation()
	{
		if (_finalExplotion) Instantiate(_finalExplotion, transform.position, transform.rotation);
	}

	public void ForceDestroy()
	{
		if (GameController.i && GameController.i.online) return;
		FinalAnimation(); Destroy(gameObject);
	}

	public void PhotonDestroy()
	{
		if (!GameController.i || !GameController.i.online) Destroy(gameObject);
	}
}
