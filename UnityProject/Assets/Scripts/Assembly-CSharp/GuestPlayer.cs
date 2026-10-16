using UnityEngine;

public class GuestPlayer : MonoBehaviour
{
	public static GuestPlayer guestPlayer;

	private void Start()
	{
		// Native 0x1297078: layer8 and GuestPlayer tag.
		guestPlayer = this;
		gameObject.layer = 8;
		gameObject.tag = "GuestPlayer";
	}

	private void OnDestroy()
	{
		if (SystemManager.i.mode != GameMode.Survival && guestPlayer == this) guestPlayer = null;
	}
}
