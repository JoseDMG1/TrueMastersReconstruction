using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PingPhoton : MonoBehaviour
{
	public Text ping;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void CheckPing()
	{
	}

	[PunRPC]
	public void SlowGame(float time)
	{
	}
}
