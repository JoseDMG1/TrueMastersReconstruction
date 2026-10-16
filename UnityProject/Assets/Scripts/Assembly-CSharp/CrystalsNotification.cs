using UnityEngine;
using UnityEngine.UI;

public class CrystalsNotification : MonoBehaviour
{
	public static CrystalsNotification i;

	public GameObject BonusAdGlobe;

	public GameObject AdGlobe;

	public Text numCrystals;

	public Text numCrystalsBonus;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) Destroy(this);
	}

	private void Update()
	{
	}

	public void ShowBonusSmall(float crystals)
	{
		AdGlobe.SetActive(true);
		numCrystals.text = "AD WACHED +" + crystals;
		WinCrystals(crystals);
		Invoke(nameof(Hide), 5f);
	}

	public void ShowBonus(float crystals)
	{
		BonusAdGlobe.SetActive(true);
		numCrystalsBonus.text = "AD WATCHED +" + crystals;
		WinCrystals(crystals);
		Invoke(nameof(Hide), 5f);
	}

	public void Hide()
	{
		AdGlobe.SetActive(false);
		BonusAdGlobe.SetActive(false);
	}

	public void WinCrystals(float crystals)
	{
		SystemManager.i.tmPlayer.crystals += crystals;
		SystemManager.i.SaveData("crystals");
		if (CrystalManager.i != null) CrystalManager.i.UpdateCrystals();
	}

	public void SpendCrystals(float crystals)
	{
		SystemManager.i.tmPlayer.crystals -= crystals;
		SystemManager.i.SaveData("crystals");
		if (CrystalManager.i != null) CrystalManager.i.UpdateCrystals();
	}

	public void WinTrophies(float trophies)
	{
		SystemManager.i.tmPlayer.trophies += trophies;
		SystemManager.i.SaveData("trophies");
		if (TrophiesManager.i != null) TrophiesManager.i.UpdateTrophies();
	}

	public void SpendTrophies(float trophies)
	{
		SystemManager.i.tmPlayer.trophies -= trophies;
		SystemManager.i.SaveData("trophies");
		if (TrophiesManager.i != null) TrophiesManager.i.UpdateTrophies();
	}
}
