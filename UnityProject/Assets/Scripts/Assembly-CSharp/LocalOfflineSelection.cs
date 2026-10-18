using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Independent local character previews, using each player's saved combat bindings.</summary>
public sealed class LocalOfflineSelection : MonoBehaviour
{
    private readonly string[] viewOrder = { "Rina", "Amon", "Diana", "Ren", "Vinea", "Eliot", "Aura", "Sora", "Dante", "Agata", "Raven", "Maya", "?" };
    private readonly string[] selected = new string[2];
    private readonly bool[] ready = new bool[2], attackHeld = new bool[2], cancelHeld = new bool[2];
    private readonly int[] direction = new int[2];
    private readonly float[] repeatAt = new float[2];
    private readonly Camera[] cameras = new Camera[2];
    private readonly TMP_Text[] names = new TMP_Text[2], hints = new TMP_Text[2];
    private readonly Button[] readyButtons = new Button[2];
    private readonly List<Button> sharedButtons = new List<Button>();
    private List<string> roster;
    private CameraController originalController;
    private Canvas overlay;
    private GameSpeed speed;
    private TMP_Text speedLabel, stageLabel, timeLabel;
    private Image stageImage;
    private bool starting;
    private bool previousNavigation;

    private void Start()
    {
        if (!SystemManager.i || SystemManager.i.mode != GameMode.LocalOffline) { Destroy(this); return; }
        roster = new List<string>(CharactersManager.i.playableCharacters);
        originalController = CameraController.cameraController;
        var originalCamera = originalController ? originalController.GetComponent<Camera>() : null;
        if (!originalCamera || roster.Count == 0) { Debug.LogError("Local selection needs preview views and a playable roster.", this); return; }
        speed = FindAnyObjectByType<GameSpeed>();
        if (speed) speed.SetSpeed(Mathf.Clamp(PlayerPrefs.GetInt("GameSpeed", 1), 0, speed.speedList.Count - 1));
        if (EventSystem.current)
        {
            previousNavigation = EventSystem.current.sendNavigationEvents;
            EventSystem.current.sendNavigationEvents = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
        originalController.enabled = false;
        originalController.randomBanner.SetActive(false);
        cameras[0] = originalCamera;
        var right = new GameObject("Player2SelectionCamera", typeof(Camera));
        right.transform.SetParent(transform, false);
        cameras[1] = right.GetComponent<Camera>();
        cameras[1].CopyFrom(originalCamera);
        cameras[1].depth = originalCamera.depth + .01f;
        for (int p = 0; p < 2; p++) cameras[p].rect = new Rect(p * .5f, 0f, .5f, 1f);

        // Replace only selection presentation; the existing back control and fade remain active.
        var canvas = SelectionController.i.nameText.canvas;
        foreach (string child in new[] { "LeftBar", "Time", "Level", "Trophies", "Crystals", "ShopButton", "Unlock", "ComingSoon", "CharNotAvailable", "RandomBanner" })
        {
            var target = canvas.transform.Find(child);
            if (target) target.gameObject.SetActive(false);
        }
        CreateUI();
        for (int p = 0; p < 2; p++)
        {
            string choice = PlayerPrefs.GetString(p == 0 ? "Character" : "CharacterCPU", p == 0 ? "Sora" : "Amon");
            selected[p] = roster.Contains(choice) ? choice : roster[0];
            SaveChoice(p);
            var input = LocalControls.ReadPlayer(p);
            attackHeld[p] = input.a; cancelHeld[p] = input.b;
            UpdatePlayerUI(p);
            MoveCamera(p, true);
        }
        RefreshShared();
    }

    private void Update()
    {
        if (!overlay || starting) return;
        for (int p = 0; p < 2; p++)
        {
            var input = LocalControls.ReadPlayer(p);
            int next = input.x > .55f ? 1 : input.x < -.55f ? -1 : 0;
            if (!ready[p] && next != 0 && (next != direction[p] || Time.unscaledTime >= repeatAt[p]))
            {
                CycleCharacter(p, next);
                repeatAt[p] = Time.unscaledTime + (next != direction[p] ? .4f : .18f);
            }
            direction[p] = next;
            // A mapped mouse click on a UI button must not also confirm a character.
            bool pointerClick = !(p == 0 ? LocalControls.Data.player1 : LocalControls.Data.player2).useGamepad &&
                LocalControls.Profile(p).attack.StartsWith("Mouse") && EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (input.a && !attackHeld[p] && !pointerClick) SetReady(p, !ready[p]);
            if (input.b && !cancelHeld[p]) SetReady(p, false);
            attackHeld[p] = input.a; cancelHeld[p] = input.b;
        }
    }

    private void LateUpdate()
    {
        if (!overlay) return;
        MoveCamera(0, false); MoveCamera(1, false);
    }

    public void CycleCharacter(int player, int step)
    {
        if (starting || ready[player]) return;
        int index = (roster.IndexOf(selected[player]) + step % roster.Count + roster.Count) % roster.Count;
        selected[player] = roster[index];
        SaveChoice(player);
        UpdatePlayerUI(player);
    }

    private void SaveChoice(int player)
    {
        // Existing battle creation already consumes these two preferences.
        if (player == 0) SelectionController.i.ChangeCharacter(selected[player]);
        else SelectionController.i.cpuController.ChangeCharacter(selected[player]);
        originalController.randomBanner.SetActive(false);
    }

    public void SetReady(int player, bool value)
    {
        if (starting) return;
        ready[player] = value;
        UpdatePlayerUI(player);
        if (ready[0] && ready[1])
        {
            starting = true;
            foreach (var button in overlay.GetComponentsInChildren<Button>()) button.interactable = false;
            PlayerPrefs.Save();
            SelectionController.i.start();
            if (SelectionController.i.ready && selected[1] != selected[0])
                PlayCharacterSelectionAnimation(selected[1]);
        }
    }

    private void PlayCharacterSelectionAnimation(string character)
    {
        foreach (var model in SelectionController.i.Characters)
        {
            if (!model || model.name != character) continue;
            var animator = model.GetComponent<Animator>();
            if (animator) animator.SetTrigger("Cheer");
            return;
        }
    }

    private void MoveCamera(int player, bool instant)
    {
        int index = System.Array.IndexOf(viewOrder, selected[player]);
        if (index < 0 || index >= originalController.views.Length || !originalController.views[index]) return;
        Transform target = originalController.views[index];
        Vector3 position = target.position;
        var model = SelectionController.i.Characters.Find(character => character && character.name == selected[player]);
        if (model)
        {
            // Authored full-screen views place the model beside the old sidebar.
            // Center it in the narrower viewport and leave room for the header.
            Vector3 right = target.right;
            position += right * Vector3.Dot(model.transform.position - position, right);
            float depth = Vector3.Dot(model.transform.position - position, target.forward);
            position -= target.forward * Mathf.Max(0f, depth) * .35f;
        }
        float t = instant ? 1f : 1f - Mathf.Exp(-6f * Time.unscaledDeltaTime);
        cameras[player].transform.position = Vector3.Lerp(cameras[player].transform.position, position, t);
        cameras[player].transform.rotation = Quaternion.Slerp(cameras[player].transform.rotation, target.rotation, t);
    }

    private void UpdatePlayerUI(int player)
    {
        string character = selected[player];
        names[player].text = character == "Sora" ? "Siro" : character == "Amon" ? "Blaze" : character == "?" ? "Aleatorio" : character;
        var profile = LocalControls.Profile(player);
        var controls = player == 0 ? LocalControls.Data.player1 : LocalControls.Data.player2;
        string confirm = controls.useGamepad ? LocalControls.GamepadButtonLabel(profile.attackButton) : profile.attack;
        string cancel = controls.useGamepad ? LocalControls.GamepadButtonLabel(profile.jumpButton) : profile.jump;
        string movement = controls.useGamepad ? "Stick / cruceta" : profile.left + " / " + profile.right;
        hints[player].text = ready[player] ? "LISTO  ·  " + cancel + ": cambiar" : movement + ": personaje   ·   " + confirm + ": listo";
        readyButtons[player].GetComponentInChildren<TMP_Text>().text = ready[player] ? "Cambiar" : "Listo";
        readyButtons[player].GetComponent<Image>().color = ready[player] ? new Color(.1f, .45f, .32f, .95f) : new Color(.12f, .24f, .36f, .95f);
        foreach (var button in sharedButtons) button.interactable = !ready[0] && !ready[1];
    }

    private void CycleStage(int step)
    {
        var stages = StagesManager.i.playableStages;
        int index = stages.IndexOf(StagesManager.i.actualStage);
        StagesManager.i.actualStage = stages[(index + step + stages.Count) % stages.Count];
        if (StageSelector.stageSelector) StageSelector.stageSelector.ChangeStage();
        RefreshShared();
    }

    private void CycleSpeed(int step)
    {
        if (speed) speed.ChangeSpeed(step);
        RefreshShared();
    }

    private void RefreshShared()
    {
        int stage = StagesManager.i.actualStage;
        stageLabel.text = stage == 7 ? "Mapa aleatorio" : "Mapa " + (stage + 1);
        stageImage.sprite = Resources.Load<Sprite>("Images/Stages/" + stage);
        speedLabel.text = "Game Speed: " + (speed ? speed.speedList[PlayerPrefs.GetInt("GameSpeed", 1)] : "Normal");
        timeLabel.text = "Tiempo: " + SelectionController.i.timeText.text + "  >";
    }

    private void CreateUI()
    {
        var go = new GameObject("LocalSelectionUI", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        overlay = go.GetComponent<Canvas>(); overlay.renderMode = RenderMode.ScreenSpaceOverlay; overlay.sortingOrder = 50;
        var scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
        var divider = Panel("Divider", go.transform, new Vector2(.5f, .5f), Vector2.zero, new Vector2(3, 1000));
        divider.GetComponent<Image>().raycastTarget = false;
        var header = Panel("SharedSettings", go.transform, new Vector2(.5f, 1), new Vector2(0, -80), new Vector2(580, 152));
        Label("SharedTitle", header, "AJUSTES DE LA PARTIDA", new Vector2(0, 59), new Vector2(480, 25), 17);
        var preview = Panel("StagePreview", header, new Vector2(.5f, .5f), new Vector2(-140, -12), new Vector2(108, 65));
        stageImage = preview.GetComponent<Image>(); stageImage.color = Color.white;
        stageImage.preserveAspect = true; stageImage.raycastTarget = false;
        stageLabel = Label("MapName", header, "", new Vector2(-140, 29), new Vector2(230, 24), 18);
        sharedButtons.Add(Button("PreviousMap", header, "<", new Vector2(-248, -12), new Vector2(45, 46), () => CycleStage(-1)));
        sharedButtons.Add(Button("NextMap", header, ">", new Vector2(-32, -12), new Vector2(45, 46), () => CycleStage(1)));
        speedLabel = Label("SpeedName", header, "", new Vector2(143, 20), new Vector2(260, 30), 20);
        sharedButtons.Add(Button("PreviousSpeed", header, "<", new Vector2(68, -20), new Vector2(50, 36), () => CycleSpeed(-1)));
        sharedButtons.Add(Button("NextSpeed", header, ">", new Vector2(218, -20), new Vector2(50, 36), () => CycleSpeed(1)));
        Label("SpeedHint", header, "Velocidad", new Vector2(143, -20), new Vector2(100, 24), 15);
        var time = Button("MatchTime", header, "", new Vector2(0, -59), new Vector2(245, 26), () => { SelectionController.i.ChangeTime(); RefreshShared(); });
        timeLabel = time.GetComponentInChildren<TMP_Text>(); timeLabel.fontSizeMax = 17;
        sharedButtons.Add(time);
        for (int p = 0; p < 2; p++)
        {
            int player = p;
            float x = p == 0 ? .25f : .75f;
            var badge = Panel("Player" + (p + 1), go.transform, new Vector2(x, 1), new Vector2(0, -182), new Vector2(290, 46));
            Label("Title", badge, "JUGADOR " + (p + 1), Vector2.zero, new Vector2(280, 40), 23);
            var card = Panel("Player" + (p + 1) + "Choice", go.transform, new Vector2(x, 0), new Vector2(0, 89), new Vector2(540, 162));
            names[p] = Label("Character", card, "", new Vector2(0, 46), new Vector2(330, 43), 32);
            Button("Previous", card, "<", new Vector2(-225, 42), new Vector2(60, 48), () => CycleCharacter(player, -1));
            Button("Next", card, ">", new Vector2(225, 42), new Vector2(60, 48), () => CycleCharacter(player, 1));
            hints[p] = Label("Controls", card, "", new Vector2(0, 1), new Vector2(515, 35), 18);
            readyButtons[p] = Button("Ready", card, "Listo", new Vector2(0, -48), new Vector2(230, 43), () => SetReady(player, !ready[player]));
        }
    }

    private RectTransform Panel(string name, Transform parent, Vector2 anchor, Vector2 position, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        var rect = go.GetComponent<RectTransform>(); rect.SetParent(parent, false);
        rect.anchorMin = rect.anchorMax = anchor; rect.anchoredPosition = position; rect.sizeDelta = size;
        go.GetComponent<Image>().color = new Color(.025f, .045f, .075f, .9f);
        return rect;
    }

    private TMP_Text Label(string name, Transform parent, string value, Vector2 position, Vector2 size, float fontSize)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        var rect = go.GetComponent<RectTransform>(); rect.SetParent(parent, false); rect.sizeDelta = size; rect.anchoredPosition = position;
        var text = go.GetComponent<TextMeshProUGUI>(); text.font = SelectionController.i.nameText.font;
        text.text = value; text.fontSize = fontSize; text.alignment = TextAlignmentOptions.Center;
        text.enableAutoSizing = true; text.fontSizeMin = 12; text.fontSizeMax = fontSize; text.color = Color.white; text.raycastTarget = false;
        return text;
    }

    private Button Button(string name, Transform parent, string value, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        var rect = Panel(name, parent, new Vector2(.5f, .5f), position, size);
        var button = rect.gameObject.AddComponent<Button>(); button.targetGraphic = rect.GetComponent<Image>();
        button.navigation = new Navigation { mode = Navigation.Mode.None };
        button.onClick.AddListener(action); Label("Label", rect, value, Vector2.zero, size, 24);
        return button;
    }

    private void OnDestroy()
    {
        if (overlay) Destroy(overlay.gameObject);
        if (cameras[1]) Destroy(cameras[1].gameObject);
        if (cameras[0]) cameras[0].rect = new Rect(0, 0, 1, 1);
        if (originalController) originalController.enabled = true;
        if (EventSystem.current) EventSystem.current.sendNavigationEvents = previousNavigation;
    }
}
