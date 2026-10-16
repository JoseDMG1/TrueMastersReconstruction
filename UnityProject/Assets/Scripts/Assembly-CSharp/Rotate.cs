using UnityEngine;
using Photon.Pun;

public class Rotate : MonoBehaviour
{
	public float delay;

	private bool start;

	public float speed = 1f;

	private void Start()
	{
		if (GameController.i && GameController.i.online && !GetComponent<PhotonView>().IsMine)
			Destroy(this);
		Invoke("Init", delay);
	}

	public void Init()
	{
		start = true;
	}

	private void FixedUpdate()
	{
		if (start) transform.Rotate(0f, speed, 0f);
	}
}
