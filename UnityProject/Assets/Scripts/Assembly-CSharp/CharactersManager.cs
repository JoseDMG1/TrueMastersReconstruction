using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
	public static CharactersManager i;

	public List<string> characters;

	public List<string> playableCharacters;

	public string lastCharacter;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		characters = new List<string>();
		SetCharacters();
		SetPlayableCharacters();
	}

	private void SetCharacters()
	{
		characters.AddRange(new[] { "Sora", "Vinea", "Diana", "Amon", "Aura", "Eliot", "Ren", "Rina", "Dante", "Agata", "Raven", "Maya", "?" });
	}

	public void SetPlayableCharacters()
	{
		playableCharacters = new List<string>(characters);
		RemoveBlockCharacters();
	}

	private void RemoveBlockCharacters()
	{
		// The offline reconstruction build can opt into the full roster for moveset
		// review without editing or granting anything in the player's saved account.
		if (SystemManager.i && SystemManager.i.unlock) return;
		var player = SystemManager.i.tmPlayer;
		if (player.tmPremium == 2 || player.tmPremium == 3) return;
		var c = player.characters;
		string[] names = { "Aura", "Eliot", "Ren", "Rina", "Dante", "Agata", "Raven", "Maya" };
		int[] owned = { c.aura, c.eliot, c.ren, c.rina, c.dante, c.agata, c.raven, c.maya };
		for (int index = 0; index < names.Length; index++)
			if (owned[index] != 1) playableCharacters.Remove(names[index]);
	}

	public string RemoveRandom(string character)
	{
		if (character == "?")
		{
			var candidates = new List<string>(playableCharacters);
			candidates.Remove("?");
			if (candidates.Count >= 2) candidates.Remove(lastCharacter);
			character = candidates[Random.Range(0, candidates.Count)];
		}
		lastCharacter = character;
		return character;
	}

	public bool IsCharUnlocked(string charName)
	{
		return playableCharacters.Contains(charName);
	}
}
