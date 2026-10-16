using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
	public Text NameLabel;

	public Text StatusLabel;

	public Text Health;

	[HideInInspector]
	public string FriendId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public void Awake()
	{
	}

	public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
	{
	}
}
