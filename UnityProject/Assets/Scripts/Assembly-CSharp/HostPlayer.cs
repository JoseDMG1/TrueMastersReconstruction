using UnityEngine;

public class HostPlayer : MonoBehaviour
{
	public static HostPlayer hostPlayer;

	private void Start()
	{
		// Native 0x1297194: layer9 and HostPlayer tag.
		hostPlayer = this;
		gameObject.layer = 9;
		gameObject.tag = "HostPlayer";
	}

	private void OnDestroy()
	{
		if (hostPlayer == this) hostPlayer = null;
	}
}
