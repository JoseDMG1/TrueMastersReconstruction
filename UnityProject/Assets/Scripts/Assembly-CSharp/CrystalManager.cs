using TMPro;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
	public static CrystalManager i;

	public TMP_Text number;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		UpdateCrystals();
	}

	private void OnDestroy()
	{
		if (i == this) i = null;
	}

	public void UpdateCrystals()
	{
		number.text = SystemManager.i.tmPlayer.crystals.ToString();
	}
}
