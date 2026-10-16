using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Offline methods recovered from ARM64; see Knowledge/menu-reconstruction.md.
public class StarMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject tourneyAlert;
    public GameObject points;
    private bool missingCharacters;
    public Text version;
    public int TournamentPrice;
    public TMP_Text PriceText;
    public GameObject blockButtons;
    public RotateSelf isoLogo;
    public GameObject base_UI;
    public GameObject multiplayer_UI;

    private void Start()
    {
        // 0x12C2A48: network LeaveRoom/Disconnect is outside the offline profile.
        TournamentController.i.Reset();
        SystemManager.i.ChangePlayingMode(GameMode.StartMenu);
        PriceText.text = TournamentPrice.ToString();
        if (OnlineManager.i != null) OnlineManager.i.spectatorCode = null;
    }

    public override void OnConnectedToMaster()
    {
        Unavailable(nameof(OnConnectedToMaster));
    }

    public void ShowVersion()
    {
        version.enabled = true;
        version.text = "Version " + Application.version;
    }

    public void ShowMultiplayerUI(bool showMultiplayer)
    {
        multiplayer_UI.SetActive(showMultiplayer);
        base_UI.SetActive(!showMultiplayer);
    }

    public void TourneyAlert()
    {
        // Native 0x12C2C90: tournament requires these four ownership records.
        if (AllCharactersUnlocked())
        {
            tourneyAlert.SetActive(true);
            points.SetActive(true);
        }
        else Notifications.i.Log("You must unlock Aura, Eliot, Ren and Rina to play this mode", 7f, "info");
    }
    public void CloseTourneyAlert()
    {
        // 0x12C2E44; generic slot 0x2814D38 resolves IncreaseSize.
        tourneyAlert.GetComponent<IncreaseSize>().Decrease();
        points.GetComponent<IncreaseSize>().Decrease();
    }
    public void EnterTournament()
    {
        // 0x12C2EC4; currency and entry are owned by TournamentController.
        TournamentController.i.PayTournament(TournamentPrice);
    }
    public void PlayTeddies()
    {
        // Exact native literal0x2817B88. Only an explicit user UI action opens it.
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.murallagames.teddies");
    }

    public bool AllCharactersUnlocked()
    {
        // 0x12C2D54. Offline has no authenticated Firebase user, so it uses
        // the local ownership records. No premium entitlement is synthesized.
        var player = SystemManager.i.tmPlayer;
        if (FirebaseController.i != null && FirebaseController.i.auth != null &&
            FirebaseController.i.auth.CurrentUser != null && (player.tmPremium == 2 || player.tmPremium == 3)) return true;
        var owned = player.characters;
        return owned.aura == 1 && owned.eliot == 1 && owned.ren == 1 && owned.rina == 1;
    }

    public void GoToScene(int scene)
    {
        LevelChanger.i.FadeToLevel(scene);
    }

    public void SurvivalMode()
    {
        LevelChanger.i.FadeToLevel(3);
        SystemManager.i.ChangePlayingMode(GameMode.Survival);
        Notifications.i.Remove();
    }

    public void SinglePlayerMode()
    {
        LevelChanger.i.FadeToLevel(3);
        SystemManager.i.ChangePlayingMode(GameMode.SinglePlayer);
        Notifications.i.Remove();
    }

    public void Online2Players() { Unavailable(nameof(Online2Players)); }

    public void GoToLocalMultiplayer()
    {
        LevelChanger.i.FadeToLevel(11);
    }

    public void Connect() { Unavailable(nameof(Connect)); }
    public void CheckConnection() { Unavailable(nameof(CheckConnection)); }

    private void Unavailable(string method)
    {
        Debug.LogError("StarMenuManager." + method + " is not reconstructed; no network, currency or entitlement action was performed.", this);
    }
}

