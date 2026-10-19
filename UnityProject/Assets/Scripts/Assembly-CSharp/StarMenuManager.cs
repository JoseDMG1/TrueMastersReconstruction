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
        ConfigureTwoPlayerMenu();
        ConfigureDesktopMenu();
        PriceText.text = TournamentPrice.ToString();
        if (OnlineManager.i != null) OnlineManager.i.spectatorCode = null;
    }

    private void ConfigureDesktopMenu()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        foreach (var social in FindObjectsByType<SocialMedia>(FindObjectsInactive.Include))
            social.gameObject.SetActive(false);
        foreach (var updateNotice in FindObjectsByType<UpdateController>(FindObjectsInactive.Include))
            updateNotice.gameObject.SetActive(false);
#endif
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

    public void Online2Players() { ShowMultiplayerUI(true); }

    private void ConfigureTwoPlayerMenu()
    {
        if (!multiplayer_UI) return;
        var local = multiplayer_UI.transform.Find("Local");
        if (!local) return;
        SetButtonLabel(local.gameObject, "Local LAN");
        var offline = multiplayer_UI.transform.Find("Local Offline");
        if (Application.isMobilePlatform)
        {
            if (offline) offline.gameObject.SetActive(false);
            return;
        }
        if (!offline)
        {
            offline = CreateLocalOfflineOption(local).transform;
        }
        var offlineRect = offline as RectTransform;
        if (offlineRect) offlineRect.anchoredPosition = new Vector2(0f, -175f);
        var online = multiplayer_UI.transform.Find("Online") as RectTransform;
        var localRect = local as RectTransform;
        var back = multiplayer_UI.transform.Find("Back") as RectTransform;
        if (online) online.anchoredPosition = new Vector2(0f, -80f);
        if (localRect) localRect.anchoredPosition = new Vector2(0f, -270f);
        if (back) back.anchoredPosition = new Vector2(0f, -365f);
    }

    private GameObject CreateLocalOfflineOption(Transform local)
    {
        var localRect = local as RectTransform;
        var option = new GameObject("Local Offline", typeof(RectTransform), typeof(Image), typeof(Button));
        option.transform.SetParent(local.parent, false);
        var rect = option.GetComponent<RectTransform>();
        rect.anchorMin = localRect.anchorMin; rect.anchorMax = localRect.anchorMax; rect.pivot = localRect.pivot;
        rect.sizeDelta = localRect.sizeDelta; rect.localScale = localRect.localScale;
        rect.anchoredPosition = new Vector2(0f, -175f);
        var image = option.GetComponent<Image>();
        var sourceImage = local.GetComponent<Image>();
        if (sourceImage) { image.sprite = sourceImage.sprite; image.color = sourceImage.color; image.type = sourceImage.type; }
        var button = option.GetComponent<Button>();
        var sourceButton = local.GetComponent<Button>();
        if (sourceButton) { button.colors = sourceButton.colors; button.transition = sourceButton.transition; button.navigation = sourceButton.navigation; }
        button.targetGraphic = image;
        button.onClick.AddListener(LocalOfflineMode);
        var sourceTmp = local.GetComponentInChildren<TMP_Text>(true);
        if (sourceTmp)
        {
            var label = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            label.transform.SetParent(option.transform, false);
            var labelRect = label.GetComponent<RectTransform>(); labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = Vector2.zero; labelRect.offsetMax = Vector2.zero;
            var text = label.GetComponent<TextMeshProUGUI>();
            text.font = sourceTmp.font; text.fontSize = sourceTmp.fontSize; text.color = sourceTmp.color;
            text.alignment = sourceTmp.alignment; text.enableAutoSizing = true; text.fontSizeMin = 22; text.fontSizeMax = 46;
            text.text = "Local Offline"; text.raycastTarget = false;
        }
        else
        {
            var sourceLegacy = local.GetComponentInChildren<Text>(true);
            var label = new GameObject("Label", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(option.transform, false);
            var labelRect = label.GetComponent<RectTransform>(); labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = Vector2.zero; labelRect.offsetMax = Vector2.zero;
            var text = label.GetComponent<Text>();
            if (sourceLegacy) { text.font = sourceLegacy.font; text.fontSize = sourceLegacy.fontSize; text.color = sourceLegacy.color; text.alignment = sourceLegacy.alignment; }
            text.text = "Local Offline"; text.resizeTextForBestFit = true; text.raycastTarget = false;
        }
        return option;
    }

    private static void SetButtonLabel(GameObject target, string label)
    {
        var tmp = target.GetComponentInChildren<TMP_Text>(true);
        if (tmp) { tmp.text = label; tmp.enableAutoSizing = true; tmp.fontSizeMin = 22; tmp.fontSizeMax = 46; }
        else
        {
            var legacy = target.GetComponentInChildren<Text>(true);
            if (legacy) { legacy.text = label; legacy.resizeTextForBestFit = true; }
        }
    }

    public void LocalOfflineMode()
    {
        SystemManager.i.ChangePlayingMode(GameMode.LocalOffline);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Character"))) PlayerPrefs.SetString("Character", "Sora");
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterCPU"))) PlayerPrefs.SetString("CharacterCPU", "Amon");
        PlayerPrefs.Save();
        LevelChanger.i.FadeToLevel(4);
    }

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

