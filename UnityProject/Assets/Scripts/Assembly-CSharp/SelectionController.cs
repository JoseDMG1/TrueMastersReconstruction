using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectionController : MonoBehaviour
{
	public static SelectionController i;

	private int actualCharacterIndex;

	public string actualCharacterName;

	public TMP_Text nameText;

	public TMP_Text buttonText;

	public bool hostReady;

	public bool guestReady;

	public bool inBattle;

	public TMP_Text unlockName;

	public GameObject UnlockScreen;

	public GameObject beforeArrow;

	public GameObject afterArrow;

	public TMP_Text crystalsView;

	public TMP_Text trophiesView;

	public GameObject beforeArrowStage;

	public GameObject afterArrowStage;

	public GameObject beforeArrowSpeed;

	public GameObject afterArrowSpeed;

	public bool ready;

	public GameObject Price;

	public GameObject PriceTrophy;

	public List<GameObject> Characters;

	public CPUController cpuController;

	public GameObject comingSoonScreen;

	public int trophyRequirement = 5;

	public int crystalsPrice = 200;

	public TMP_Text timeText;

	public List<string> times;

	public int actualTime;

	public UnityEvent onStart;

	public GameObject charNotAvialable;

	private void Awake()
	{
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		if (onStart == null) onStart = new UnityEvent();
		actualTime = PlayerPrefs.GetInt("TimeOption", 0);
		timeText.text = times[actualTime];
		SetTime();
		SetDefaultCharacters();
	}

	public void SetOnlineCode()
	{
		Debug.LogWarning("Online selection requires the unreconstructed network flow.", this);
	}

	public void ChangeTime()
	{
		actualTime = actualTime + 1 >= 4 ? 0 : actualTime + 1;
		PlayerPrefs.SetInt("TimeOption", actualTime);
		timeText.text = times[actualTime];
		SetTime();
	}

	public void SetTime()
	{
		int[] seconds = { 300, 420, 540, 180 };
		if (actualTime >= 0 && actualTime < 4) PlayerPrefs.SetInt("Timer", seconds[actualTime]);
	}

	private void Start()
	{
		RefreshCharsAndStages();
		actualCharacterIndex = CharactersManager.i.characters.IndexOf(PlayerPrefs.GetString("Character"));
		ChangeCharacter(PlayerPrefs.GetString("Character"));
		if (SystemManager.i.mode == GameMode.LocalMultiplayer) ChangeCharacter("Sora");
	}

	private void OnDestroy()
	{
		onStart.RemoveAllListeners();
		if (i == this) i = null;
	}

	public void ResetPlayerProperties()
	{
		Debug.LogWarning("Photon player properties are unavailable in the offline reconstruction.", this);
	}

	public void RefreshCharsAndStages()
	{
		CharactersManager.i.SetPlayableCharacters();
		StagesManager.i.SetPlayableStages();
	}

	private void SetDefaultCharacters()
	{
		if (PlayerPrefs.HasKey("Character")) return;
		PlayerPrefs.SetString("Character", "Sora");
		PlayerPrefs.SetString("CharacterCPU", "Amon");
	}

	public void nextCharacter()
	{
		actualCharacterIndex = CharactersManager.i.characters.IndexOf(PlayerPrefs.GetString("Character"));
		if (actualCharacterIndex < CharactersManager.i.characters.Count - 1)
			ChangeCharacter(CharactersManager.i.characters[++actualCharacterIndex]);
	}

	public void previousCharacter()
	{
		actualCharacterIndex = CharactersManager.i.characters.IndexOf(PlayerPrefs.GetString("Character"));
		if (actualCharacterIndex > 0) ChangeCharacter(CharactersManager.i.characters[--actualCharacterIndex]);
	}

	public void ChangeCharacter(string character)
	{
		actualCharacterName = character;
		CameraController.cameraController.Show(character);
		nameText.text = character == "Amon" ? "Blaze" : character == "Sora" ? "Siro" : character;
		PlayerPrefs.SetString("Character", character);
		unlockName.text = character;
		bool trophy = character == "Dante" || character == "Agata" || character == "Raven" || character == "Maya";
		Price.SetActive(!trophy);
		PriceTrophy.SetActive(trophy);
		CheckIfLocked();
		if (SystemManager.i.mode == GameMode.LocalMultiplayer)
		{
			bool available = character == "Sora" || character == "Aura" || character == "Vinea" ||
				character == "Eliot" || character == "Diana" || character == "Ren";
			charNotAvialable.SetActive(!available);
			if (!available) { PriceTrophy.SetActive(false); UnlockScreen.SetActive(false); }
		}
	}

	public void RemoveButtons()
	{
		Destroy(afterArrow); Destroy(beforeArrow); Destroy(beforeArrowStage); Destroy(afterArrowStage);
	}

	public void OnlineMode()
	{
		Debug.LogError("Network ready handshake has not been reconstructed.", this);
	}

	public void start()
	{
		if (!CharactersManager.i.IsCharUnlocked(actualCharacterName)) return;
		if (ready || SystemManager.i.mode == GameMode.LocalMultiplayer) return;
		onStart.Invoke();
		RemoveButtons();
		UnlockAnim();
		// Native start() 0x12ADA68..0x12ADAF4: tournament enters its
		// bracket scene first. The ordinary duel helper always loads arena5.
		if (SystemManager.i.mode == GameMode.Tournament)
		{
			LevelChanger.i.FadetoLevelDelay(6, 1.7f);
			StagesManager.i.actualStage = Random.Range(0, 7);
			TournamentController.i.Reset();
			return;
		}
		PlayerPrefs.SetInt("Stage", StagesManager.i.RemoveRandom(StagesManager.i.actualStage));
		GoToBattle();
		ready = true;
	}

	public void GoToBattle()
	{
		if (inBattle) return;
		if (SystemManager.i.mode == GameMode.Online || SystemManager.i.mode == GameMode.Spectator)
		{ Debug.LogError("Online battle loading has not been reconstructed.", this); return; }
		LevelChanger.i.FadetoLevelDelay(5, 1.7f);
		inBattle = true;
	}

	public void CheckIfLocked()
	{
		comingSoonScreen.SetActive(false);
		UnlockScreen.SetActive(!CharactersManager.i.IsCharUnlocked(actualCharacterName));
	}

	public void UnlockCharacter()
	{
		if (actualCharacterName == "Dante" || actualCharacterName == "Agata" || actualCharacterName == "Raven" || actualCharacterName == "Maya")
		{ UnlockTrophyCharacter(); return; }
		if (SystemManager.i.tmPlayer.crystals < crystalsPrice) return;
		CrystalsNotification.i.SpendCrystals(crystalsPrice);
		Unlock();
	}

	public void UnlockTrophyCharacter()
	{
		if (SystemManager.i.tmPlayer.trophies < trophyRequirement) return;
		CrystalsNotification.i.SpendTrophies(trophyRequirement);
		Unlock();
	}

	public void Unlock()
	{
		var field = typeof(TMPlayerCharacters).GetField(actualCharacterName.ToLowerInvariant());
		if (field == null) return;
		field.SetValue(SystemManager.i.tmPlayer.characters, 1);
		CharactersManager.i.SetPlayableCharacters();
		if (cpuController != null) cpuController.characters = CharactersManager.i.playableCharacters;
		UnlockScreen.SetActive(false);
		UnlockAnim();
		SystemManager.i.SaveData("characters");
	}

	public void UnlockAnim()
	{
		foreach (var character in Characters)
			if (character.name == actualCharacterName) character.GetComponent<Animator>().SetTrigger("Cheer");
	}
}
