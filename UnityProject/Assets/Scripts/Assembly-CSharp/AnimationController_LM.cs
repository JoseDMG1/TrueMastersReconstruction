using UnityEngine;

public class AnimationController_LM : MonoBehaviour, IBattleTask
{
	public TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void PlayAndSendAnimation(NetAnimation animation, GameObject player, float transitionSpeed = 0.1f)
	{
	}

	private void GetAndPlayAnimation(byte[] data)
	{
	}
}
