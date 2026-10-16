using TMPro;
using UnityEngine;

public class TrophiesManager : MonoBehaviour
{
	public static TrophiesManager i;

	public TMP_Text number;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		UpdateTrophies();
	}

	private void OnDestroy()
	{
		if (i == this) i = null;
	}

	public void UpdateTrophies()
	{
		number.text = SystemManager.i.tmPlayer.trophies.ToString();
	}
}
