using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CPUController : MonoBehaviour
{
	public List<string> characters;

	private int actualCharacter;

	public TMP_Text nameText;

	private void Start()
	{
		characters = CharactersManager.i.characters;
		ChangeCharacter(PlayerPrefs.GetString("CharacterCPU", "Amon"));
	}

	public void nextCharacter()
	{
		actualCharacter = characters.IndexOf(PlayerPrefs.GetString("CharacterCPU"));
		if (actualCharacter < characters.Count - 1) ChangeCharacter(characters[++actualCharacter]);
	}

	public void previousCharacter()
	{
		actualCharacter = characters.IndexOf(PlayerPrefs.GetString("CharacterCPU"));
		if (actualCharacter > 0) ChangeCharacter(characters[--actualCharacter]);
	}

	public void ChangeCharacter(string character)
	{
		nameText.text = character == "Amon" ? "Blaze" : character == "Sora" ? "Siro" : character;
		PlayerPrefs.SetString("CharacterCPU", character);
	}
}
