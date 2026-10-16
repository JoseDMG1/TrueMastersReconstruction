using Photon.Pun;
using UnityEngine;

public class OnlineMovingAnim : MonoBehaviour
{
	public Animator anim;

	public Vector3 lastPosition;

	public float targetHorizontal;

	public float targetVertical;

	public float smoothSpeed;

	public float distance;

	public float Limdistance;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SetByLastPosition()
	{
	}

	private void SetAnimSmooth()
	{
	}

	private void SendOnlinePosition()
	{
	}

	[PunRPC]
	public void UpdateOnlinePosition(Vector3 newPosition)
	{
	}

	[PunRPC]
	public void SetOnlinePosition(Vector3 onlinePos)
	{
	}
}
