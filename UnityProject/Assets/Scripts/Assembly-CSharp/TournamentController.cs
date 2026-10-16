using System.Collections.Generic;
using UnityEngine;

// Persistent tournament state owner, reconstructed from native 0x12B81B0..0x12B8684.
public class TournamentController : MonoBehaviour
{
    public static TournamentController i;
    public List<int> checkpoints;
    public int round;
    public bool loseTournament;
    public List<bool> losers;
    public List<string> characters;
    public int battle;
    public int times;
    public int stage;
    public int tourneyLevel;

    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) { Destroy(this); return; }
        Reset();
    }
    private void OnDestroy() { if (i == this) i = null; }
    public void Reset()
    {
        // Native serialized lists are cleared/reused. Initialize only missing working-export lists.
        if (checkpoints == null) checkpoints = new List<int>();
        if (losers == null) losers = new List<bool>();
        if (characters == null) characters = new List<string>();
        checkpoints.Clear();
        losers.Clear();
        for (int index = 0; index < 9; index++) { checkpoints.Add(0); losers.Add(false); }
        round = 0;
        battle = 1;
        stage = 7;
        loseTournament = false;
        characters.Clear();
    }
    public void PayTournament(int fee)
    {
        if (SystemManager.i.tmPlayer.crystals < fee)
        {
            Notifications.i.Log("You need more crystals", 5f, "alert");
            return;
        }
        CrystalsNotification.i.SpendCrystals(fee);
        Reset();
        // Offline flow sets the native skip flag without requesting an advertisement.
        if (GoogleAdsManager.i != null) GoogleAdsManager.i.skipAd = true;
        LevelChanger.i.FadeToLevel(4);
        SystemManager.i.mode = GameMode.Tournament;
        SystemManager.i.RefreshPlayingMode();
        Notifications.i.Remove();
        PlayerPrefs.SetInt("TourneyTimes", PlayerPrefs.GetInt("TourneyTimes") + 1);
    }
}
