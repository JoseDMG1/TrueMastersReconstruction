using System;

[Serializable]
public class TMPlayer
{
	public string userID;

	public long lastSessionDate;

	public string lastDevice;

	public float playedTime;

	public string username;

	public float crystals;

	public float trophies;

	public float survivalRecord;

	public TMPlayerCharacters characters;

	public TMPlayerStages stages;

	public string playingMode;

	public int tmPremium;

	// ARM64 0x12B73E4: creates the two zero-initialized ownership records.
	public TMPlayer()
	{
		characters = new TMPlayerCharacters();
		stages = new TMPlayerStages();
	}
}
