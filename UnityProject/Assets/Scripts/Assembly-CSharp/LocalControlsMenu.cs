using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Runtime controls screen for local keyboard and controller setup.</summary>
public class LocalControlsMenu : MonoBehaviour
{
    private static LocalControlsMenu instance;
    private static Font font;
    private GameObject panel;
    private Canvas controlsCanvas;
    private Text status;
    private int player, row = -1;
    private bool alternate;
    private bool waitingForPadRelease;
    private bool navigationOverridden, previousNavigationEvents;
    private Text[] bindingTexts = new Text[7];
    private Button[] bindingButtons = new Button[7];
    private Text deviceLabel, modeLabel, deviceHelp;
    private Image inputIndicatorBackground;
    private Button controlsButton, player1Tab;
    private Button player2Tab, defaultPresetButton, alternatePresetButton, keyboardButton, gamepadButton;
    private Button previousDeviceButton, nextDeviceButton, stickButton;
    private float nextPadNavigation;
    private float padFeedbackUntil;
    private static readonly string[] rowNames = { "Mover arriba", "Mover abajo", "Mover a la izquierda", "Mover a la derecha", "Ataque", "Salto / esquiva", "Super / especial" };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install() { SceneManager.sceneLoaded += SceneLoaded; EnsureForScene(SceneManager.GetActiveScene()); }
    private static void SceneLoaded(Scene scene, LoadSceneMode mode) { EnsureForScene(scene); }
    private static void EnsureForScene(Scene scene)
    {
        if (Application.isMobilePlatform)
        {
            if (instance) { Destroy(instance.gameObject); instance = null; }
            return;
        }
        if (scene.name != "StartMenu")
        {
            if (instance) { Destroy(instance.gameObject); instance = null; }
            return;
        }
        if (instance) return;
        var root = new GameObject("LocalControlsMenu");
        instance = root.AddComponent<LocalControlsMenu>();
    }

    private void Start()
    {
        if (!EventSystem.current)
        {
            var events = new GameObject("ControlsEventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            events.transform.SetParent(transform, false);
        }
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        alternate = playerData.useAlternate;
        BuildCanvas();
    }
    private void Update()
    {
        if (!RefreshTransitionVisibility()) return;
        if (row < 0)
        {
            if (panel && panel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) { TogglePanel(false); return; }
            // Controller input inside this screen is for detection/rebinding.
            // In particular A must not submit whichever UI button has focus.
            if (!panel || !panel.activeSelf) NavigateWithController();
            if (row < 0) UpdateGamepadStatus();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { row = -1; status.text = ""; Refresh(); return; }
        if (playerData.useGamepad)
        {
            int id = Mathf.Clamp(playerData.joystick, 1, 8);
            bool pressed = LocalControls.TryGetPressedPadButton(id, out int button);
            Vector2 axes = LocalControls.ReadPadAxes(id);
            if (waitingForPadRelease)
            {
                if (!pressed && (row >= 4 || axes.sqrMagnitude < .0625f)) waitingForPadRelease = false;
                else SetInputIndicator("SUELTA LOS BOTONES Y CENTRA EL STICK PARA CONTINUAR", new Color(.45f, .31f, .08f));
                return;
            }
            if (row < 4 && StickMatchesDirection(row, axes))
            {
                SetCaptured(-1);
                padFeedbackUntil = Time.unscaledTime + 2f;
                return;
            }
            if (pressed)
            {
                SetCaptured(button);
                SetInputIndicator("BOTON " + LocalControls.GamepadButtonLabel(button) + " DETECTADO Y ASIGNADO", new Color(.08f, .48f, .28f));
                padFeedbackUntil = Time.unscaledTime + 2f;
                return;
            }
            string waiting = LocalControls.IsGamepadConnected(id)
                ? (row < 4 ? "Mueve el STICK IZQUIERDO en esa direccion o pulsa un boton." : "Mando " + id + " conectado. Pulsa un boton para asignarlo.")
                : "No se detecta el mando " + id + ". Revisa la conexion o cambia el numero de mando.";
            SetInputIndicator(waiting.ToUpperInvariant(), LocalControls.IsGamepadConnected(id) ? new Color(.1f, .34f, .22f) : new Color(.55f, .18f, .16f));
        }
        else
        {
            // Clicking a different UI option cancels/switches context; it must
            // not silently replace the previous binding with Mouse0.
            if (Input.GetMouseButtonDown(0) && PointerOverOtherButton()) return;
            if (Input.GetMouseButtonDown(0)) { SetCapturedKey("Mouse0"); return; }
            if (Input.GetMouseButtonDown(1)) { SetCapturedKey("Mouse1"); return; }
            if (Input.GetMouseButtonDown(2)) { SetCapturedKey("Mouse2"); return; }
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKeyDown(key) && key != KeyCode.Escape && key != KeyCode.Mouse0 && key != KeyCode.Mouse1 && key != KeyCode.Mouse2)
                { SetCapturedKey(key.ToString()); return; }
        }
    }
    private void LateUpdate() { RefreshTransitionVisibility(); }
    private bool RefreshTransitionVisibility()
    {
        bool loading = LoadingScreen.i && LoadingScreen.i.loading;
        if (controlsCanvas) controlsCanvas.gameObject.SetActive(!loading);
        if (!loading) return true;
        // A menu click may start the fade after Update. LateUpdate hides the
        // entire overlay before rendering and also blocks its pointer targets.
        CancelCapture();
        if (panel) panel.SetActive(false);
        if (controlsButton) controlsButton.gameObject.SetActive(true);
        RestoreNavigation();
        return false;
    }
    private void UpdateGamepadStatus()
    {
        if (Time.unscaledTime < padFeedbackUntil) return;
        if (!playerData.useGamepad)
        {
            SetInputIndicator("TECLADO Y MOUSE ACTIVOS\nHaz clic en una accion para cambiarla.", new Color(.18f, .22f, .27f));
            return;
        }
        int id = Mathf.Clamp(playerData.joystick, 1, 8);
        if (LocalControls.TryGetPressedPadButton(id, out int button))
        {
            string label = "ENTRADA DETECTADA: BOTON " + LocalControls.GamepadButtonLabel(button);
            status.text = label;
            SetInputIndicator(label, new Color(.08f, .48f, .28f));
            padFeedbackUntil = Time.unscaledTime + .6f;
            return;
        }
        Vector2 axes = LocalControls.ReadPadAxes(id);
        if (Mathf.Abs(axes.x) > .2f || Mathf.Abs(axes.y) > .2f)
        {
            string label = "ENTRADA DETECTADA: PALANCA X " + axes.x.ToString("+0.00;-0.00;0.00") + " / Y " + axes.y.ToString("+0.00;-0.00;0.00");
            status.text = label;
            SetInputIndicator(label, new Color(.08f, .48f, .28f));
            padFeedbackUntil = Time.unscaledTime + .6f;
            return;
        }
        bool connected = LocalControls.IsGamepadConnected(id);
        string idle = connected
            ? "MANDO CONECTADO\nPulsa un boton o mueve el stick."
            : "MANDO NO DETECTADO\nConectalo o elige otro con < y >.";
        status.text = idle;
        SetInputIndicator(idle, connected ? new Color(.1f, .34f, .22f) : new Color(.55f, .18f, .16f));
    }
    private void SetInputIndicator(string message, Color color)
    {
        if (status) status.text = message;
        if (inputIndicatorBackground) inputIndicatorBackground.color = color;
    }
    private static bool StickMatchesDirection(int direction, Vector2 axes)
    {
        return direction == 0 ? axes.y > .65f : direction == 1 ? axes.y < -.65f :
            direction == 2 ? axes.x < -.65f : direction == 3 && axes.x > .65f;
    }
    private LocalPlayerControls playerData { get { return player == 0 ? LocalControls.Data.player1 : LocalControls.Data.player2; } }
    private LocalControlProfile profile { get { return alternate ? playerData.alternate : playerData.defaults; } }

    private void BuildCanvas()
    {
        var canvasGo = new GameObject("ControlsCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGo.transform.SetParent(transform, false);
        var canvas = canvasGo.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 400;
        controlsCanvas = canvas;
        var scaler = canvasGo.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
        controlsButton = CreateControlsIconButton(canvasGo.transform);
        panel = new GameObject("ControlsPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvasGo.transform, false);
        var rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
        rect.anchoredPosition = Vector2.zero; rect.sizeDelta = new Vector2(1180, 648);
        panel.GetComponent<Image>().color = new Color(.035f, .055f, .08f, .995f);
        var muted = new Color(.66f, .74f, .82f);
        var idle = new Color(.10f, .14f, .19f);
        TextAt(panel.transform, "CONFIGURACION DE CONTROLES", 32, 20, 900, 36, 26, Color.white);
        TextAt(panel.transform, "Elige un jugador y su dispositivo. Haz clic en una accion para cambiar su control.", 32, 60, 960, 22, 14, muted);
        ButtonAt(panel.transform, "CERRAR", 1044, 24, 104, 38, () => TogglePanel(false), idle);

        TextAt(panel.transform, "1. JUGADOR", 32, 96, 436, 18, 12, muted);
        player1Tab = ButtonAt(panel.transform, "JUGADOR 1", 32, 120, 214, 38, () => SelectPlayer(0), idle);
        player2Tab = ButtonAt(panel.transform, "JUGADOR 2", 254, 120, 214, 38, () => SelectPlayer(1), idle);
        TextAt(panel.transform, "PERFIL DE CONTROLES", 650, 96, 498, 18, 12, muted);
        defaultPresetButton = ButtonAt(panel.transform, "PREDETERMINADOS", 650, 120, 238, 38, () => SelectPreset(false), idle);
        alternatePresetButton = ButtonAt(panel.transform, "ALTERNATIVOS", 896, 120, 252, 38, () => SelectPreset(true), idle);

        TextAt(panel.transform, "2. DISPOSITIVO", 32, 176, 436, 18, 12, muted);
        keyboardButton = ButtonAt(panel.transform, "TECLADO Y MOUSE", 32, 200, 214, 38, () => SelectInputMode(false), idle);
        gamepadButton = ButtonAt(panel.transform, "MANDO", 254, 200, 214, 38, () => SelectInputMode(true), idle);
        modeLabel = TextAt(panel.transform, "", 490, 200, 658, 38, 14, muted);
        Box(panel.transform, "Divider", 32, 256, 1116, 1, new Color(1, 1, 1, .1f));

        Box(panel.transform, "BindingsCard", 32, 274, 670, 320, new Color(.055f, .08f, .11f));
        TextAt(panel.transform, "3. ACCION", 52, 284, 240, 20, 12, muted);
        TextAt(panel.transform, "HAZ CLIC PARA CAMBIAR", 310, 284, 370, 20, 12, muted);
        for (int i = 0; i < 7; i++)
        {
            float y = 312 + i * 40;
            TextAt(panel.transform, rowNames[i], 52, y, 240, 34, 16, Color.white);
            int captureRow = i;
            bindingButtons[i] = ButtonAt(panel.transform, "", 310, y, 370, 34, () => BeginCapture(captureRow), idle);
            bindingTexts[i] = bindingButtons[i].GetComponentInChildren<Text>();
        }

        Box(panel.transform, "DeviceCard", 730, 274, 418, 320, new Color(.055f, .08f, .11f));
        TextAt(panel.transform, "DISPOSITIVO Y PRUEBA", 750, 290, 378, 24, 16, Color.white);
        TextAt(panel.transform, "DISPOSITIVO SELECCIONADO", 750, 326, 378, 18, 12, muted);
        previousDeviceButton = ButtonAt(panel.transform, "<", 750, 350, 32, 38, () => CycleJoystick(-1), idle);
        nextDeviceButton = ButtonAt(panel.transform, ">", 1096, 350, 32, 38, () => CycleJoystick(1), idle);
        deviceLabel = TextAt(panel.transform, "", 790, 344, 298, 50, 14, Color.white);
        deviceLabel.alignment = TextAnchor.MiddleCenter;
        var indicator = Box(panel.transform, "InputIndicator", 750, 406, 378, 72, new Color(.1f, .34f, .22f));
        inputIndicatorBackground = indicator.GetComponent<Image>();
        inputIndicatorBackground.raycastTarget = false;
        status = Label(indicator.transform, "", new Vector2(.5f, .5f), Vector2.zero, new Vector2(350, 62), 14, TextAnchor.MiddleCenter, Color.white);
        status.fontStyle = FontStyle.Bold;
        deviceHelp = TextAt(panel.transform, "", 750, 490, 378, 46, 14, muted);
        stickButton = ButtonAt(panel.transform, "USAR SOLO EL STICK IZQUIERDO", 750, 548, 378, 34, ResetDirections, idle);
        TextAt(panel.transform, "El perfil seleccionado se usa al jugar. Los cambios se guardan automaticamente por jugador. Esc: cancelar o cerrar.", 32, 612, 1116, 22, 13, muted);
        panel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
        Refresh();
    }
    private GameObject Box(Transform parent, string name, float x, float y, float width, float height, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image)); go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>(); rect.anchorMin = rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1); rect.anchoredPosition = new Vector2(x, -y); rect.sizeDelta = new Vector2(width, height);
        go.GetComponent<Image>().color = color;
        return go;
    }
    private Text TextAt(Transform parent, string text, float x, float y, float width, float height, int size, Color color)
    {
        return Label(parent, text, new Vector2(0, 1), new Vector2(x + width / 2, -y - height / 2), new Vector2(width, height), size, TextAnchor.MiddleLeft, color);
    }
    private Button ButtonAt(Transform parent, string text, float x, float y, float width, float height, Action action, Color color)
    {
        return MakeButton(parent, text, new Vector2(0, 1), new Vector2(x + width / 2, -y - height / 2), new Vector2(width, height), action, color);
    }
    private Button CreateControlsIconButton(Transform parent)
    {
        var entry = new GameObject("Controls", typeof(RectTransform), typeof(ControlsMenuIconGraphic), typeof(Button));
        entry.transform.SetParent(parent, false);
        var rect = entry.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(1, 0);
        rect.anchoredPosition = new Vector2(-72, 88);
        rect.sizeDelta = new Vector2(82, 82);
        var button = entry.GetComponent<Button>();
        button.targetGraphic = entry.GetComponent<ControlsMenuIconGraphic>();
        var colors = button.colors;
        colors.normalColor = colors.selectedColor = Color.white;
        colors.highlightedColor = new Color(1f, .88f, .65f);
        colors.pressedColor = new Color(.7f, .7f, .7f);
        colors.fadeDuration = .12f;
        button.colors = colors;
        button.onClick.AddListener(() => TogglePanel(true));

        var mainMenu = FindFirstObjectByType<StarMenuManager>();
        var source = mainMenu && mainMenu.base_UI ? mainMenu.base_UI.GetComponentInChildren<TMP_Text>(true) : null;
        if (source)
        {
            var label = new GameObject("ControlsLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
            label.transform.SetParent(entry.transform, false);
            var labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = labelRect.anchorMax = new Vector2(.5f, .5f);
            labelRect.anchoredPosition = new Vector2(0, -57);
            labelRect.sizeDelta = new Vector2(124, 22);
            var text = label.GetComponent<TextMeshProUGUI>();
            text.font = source.font;
            text.fontSharedMaterial = source.fontSharedMaterial;
            text.fontSize = 16;
            text.characterSpacing = 1.5f;
            text.alignment = TextAlignmentOptions.Center;
            text.text = "CONTROLES";
            text.raycastTarget = false;
        }
        else Label(entry.transform, "CONTROLES", new Vector2(.5f, .5f), new Vector2(0, -57), new Vector2(124, 22), 15, TextAnchor.MiddleCenter, Color.white);
        return button;
    }
    private Button MakeButton(Transform parent, string title, Vector2 anchor, Vector2 pos, Vector2 size, Action action, Color color)
    {
        var go = new GameObject(title + "Button", typeof(RectTransform), typeof(Image), typeof(Button)); go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin = rt.anchorMax = anchor; rt.anchoredPosition = pos; rt.sizeDelta = size;
        go.GetComponent<Image>().color = color; var button = go.GetComponent<Button>(); button.onClick.AddListener(() => action());
        var txt = Label(go.transform, title, new Vector2(.5f, .5f), Vector2.zero, size - new Vector2(10, 4), 15, TextAnchor.MiddleCenter, Color.white); txt.raycastTarget = false;
        return button;
    }
    private Text Label(Transform parent, string value, Vector2 anchor, Vector2 pos, Vector2 size, int fontSize, TextAnchor align, Color color)
    {
        var go = new GameObject("Label", typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin = rt.anchorMax = anchor; rt.anchoredPosition = pos; rt.sizeDelta = size;
        var t = go.GetComponent<Text>(); t.font = font; t.text = value; t.fontSize = fontSize; t.alignment = align; t.color = color; t.raycastTarget = false; return t;
    }
    private void TogglePanel(bool open)
    {
        if (open && LoadingScreen.i && LoadingScreen.i.loading) return;
        row = -1;
        waitingForPadRelease = false;
        padFeedbackUntil = 0f;
        panel.SetActive(open);
        controlsButton.gameObject.SetActive(!open);
        if (open)
        {
            if (EventSystem.current)
            {
                previousNavigationEvents = EventSystem.current.sendNavigationEvents;
                navigationOverridden = true;
                EventSystem.current.sendNavigationEvents = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
            Refresh();
        }
        else { RestoreNavigation(); SetSelected(controlsButton); }
    }
    private void RestoreNavigation()
    {
        if (navigationOverridden && EventSystem.current) EventSystem.current.sendNavigationEvents = previousNavigationEvents;
        navigationOverridden = false;
    }
    private void OnDestroy() { RestoreNavigation(); }
    private void CancelCapture()
    {
        row = -1; waitingForPadRelease = false; padFeedbackUntil = 0f;
    }
    private void SelectPlayer(int selectedPlayer)
    {
        CancelCapture(); player = selectedPlayer; alternate = playerData.useAlternate; Refresh();
    }
    private void SelectInputMode(bool gamepad)
    {
        CancelCapture(); playerData.useGamepad = gamepad; playerData.inputModeConfigured = true;
        if (gamepad && !LocalControls.IsGamepadConnected(playerData.joystick))
            for (int id = 1; id <= 8; id++)
                if (LocalControls.IsGamepadConnected(id)) { playerData.joystick = id; break; }
        SaveRefresh();
    }
    private bool PointerOverOtherButton()
    {
        if (!EventSystem.current) return false;
        var pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var hits = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hits);
        foreach (var hit in hits) if (hit.gameObject.GetComponentInParent<Button>()) return true;
        return false;
    }
    private void SelectPreset(bool useAlternate)
    {
        CancelCapture();
        alternate = useAlternate;
        playerData.useAlternate = useAlternate;
        if (player == 0) LocalControls.SetAlternateActive(useAlternate); else LocalControls.Save();
        Refresh();
    }
    private static void SetSelected(Button button)
    {
        if (EventSystem.current && button) EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
    private void NavigateWithController()
    {
        if (!EventSystem.current || !panel) return;
        Button[] buttons = panel.activeSelf ? panel.GetComponentsInChildren<Button>(true) : FindObjectsByType<Button>(FindObjectsInactive.Exclude);
        var active = new System.Collections.Generic.List<Button>();
        foreach (var button in buttons)
            if (button && button.isActiveAndEnabled && button.interactable && button.GetComponentInParent<Canvas>()) active.Add(button);
        if (active.Count == 0) return;
        int index = active.FindIndex(b => b.gameObject == EventSystem.current.currentSelectedGameObject);
        if (index < 0) { SetSelected(panel.activeSelf ? player1Tab : controlsButton); return; }
        float axis = 0f, horizontal = 0f;
        for (int id = 1; id <= 8; id++)
        {
            Vector2 padAxes = LocalControls.ReadPadAxes(id);
            float value = padAxes.y;
            if (Mathf.Abs(value) > Mathf.Abs(axis)) axis = value;
            value = padAxes.x;
            if (Mathf.Abs(value) > Mathf.Abs(horizontal)) horizontal = value;
        }
        if (Time.unscaledTime >= nextPadNavigation && (Mathf.Abs(axis) > .65f || Mathf.Abs(horizontal) > .65f))
        {
            var currentRect = active[index].transform as RectTransform;
            Vector2 current = RectTransformUtility.WorldToScreenPoint(null, currentRect.position);
            Vector2 direction = Mathf.Abs(axis) >= Mathf.Abs(horizontal)
                ? new Vector2(0f, axis > 0f ? 1f : -1f)
                : new Vector2(horizontal > 0f ? 1f : -1f, 0f);
            float bestScore = float.MaxValue;
            for (int i = 0; i < active.Count; i++)
            {
                if (i == index) continue;
                var candidateRect = active[i].transform as RectTransform;
                Vector2 delta = RectTransformUtility.WorldToScreenPoint(null, candidateRect.position) - current;
                float forward = Vector2.Dot(delta, direction);
                if (forward <= 4f) continue;
                float score = delta.magnitude + Mathf.Abs(Vector2.Dot(delta, new Vector2(-direction.y, direction.x))) * 1.5f;
                if (score < bestScore) { bestScore = score; index = i; }
            }
            SetSelected(active[index]); nextPadNavigation = Time.unscaledTime + .22f;
        }
        bool submit = false;
        for (int id = 1; id <= 8; id++)
        {
            if (LocalControls.PadButtonDown(id, 0)) submit = true;
        }
        if (submit) active[index].onClick.Invoke();
    }
    private void CycleJoystick(int step)
    {
        CancelCapture();
        var connected = new System.Collections.Generic.List<int>();
        for (int id = 1; id <= 8; id++) if (LocalControls.IsGamepadConnected(id)) connected.Add(id);
        if (connected.Count > 0)
        {
            int index = connected.IndexOf(playerData.joystick);
            playerData.joystick = connected[index < 0 ? 0 : (index + step + connected.Count) % connected.Count];
        }
        SaveRefresh();
    }
    private void SaveRefresh() { LocalControls.Save(); Refresh(); }
    private void Refresh()
    {
        if (panel == null) return;
        SetChoice(player1Tab, player == 0); SetChoice(player2Tab, player == 1);
        SetChoice(defaultPresetButton, !alternate); SetChoice(alternatePresetButton, alternate);
        SetChoice(keyboardButton, !playerData.useGamepad); SetChoice(gamepadButton, playerData.useGamepad);
        if (modeLabel) modeLabel.text = playerData.useGamepad ? "Movimiento con el stick izquierdo y botones que asignes." : "Puedes combinar teclas del teclado y botones del mouse.";
        if (previousDeviceButton) previousDeviceButton.gameObject.SetActive(playerData.useGamepad);
        if (nextDeviceButton) nextDeviceButton.gameObject.SetActive(playerData.useGamepad);
        if (stickButton) stickButton.gameObject.SetActive(playerData.useGamepad);
        if (deviceHelp) deviceHelp.text = playerData.useGamepad
            ? "Pulsa un boton o mueve el stick para ver la respuesta del mando."
            : "Haz clic en una accion. Para asignar un clic del mouse, pulsa en un espacio vacio.";
        if (deviceLabel)
        {
            string[] names = LocalControls.ConnectedJoysticks(); int id = playerData.joystick;
            string name = id > 0 && id <= names.Length && !string.IsNullOrEmpty(names[id - 1]) ? names[id - 1] : "No detectado";
            deviceLabel.text = playerData.useGamepad ? "Mando " + id + "\n" + name : "Teclado y mouse del equipo";
        }
        string[] values = playerData.useGamepad
            ? new[] { DirectionLabel("arriba", profile.upButton), DirectionLabel("abajo", profile.downButton), DirectionLabel("izquierda", profile.leftButton), DirectionLabel("derecha", profile.rightButton), PadLabel(profile.attackButton), PadLabel(profile.jumpButton), PadLabel(profile.specialButton) }
            : new[] { KeyLabel(profile.up), KeyLabel(profile.down), KeyLabel(profile.left), KeyLabel(profile.right), KeyLabel(profile.attack), KeyLabel(profile.jump), KeyLabel(profile.special) };
        for (int i = 0; i < bindingTexts.Length; i++) if (bindingTexts[i])
        {
            bindingTexts[i].text = row == i ? "Esperando entrada... (Esc cancela)" : values[i];
            bindingButtons[i].image.color = row == i ? new Color(.45f, .31f, .08f) : new Color(.10f, .14f, .19f);
        }
        if (row < 0) UpdateGamepadStatus();
    }
    private static void SetChoice(Button button, bool selected)
    {
        if (!button) return;
        button.image.color = selected ? new Color(.07f, .39f, .52f) : new Color(.10f, .14f, .19f);
        var label = button.GetComponentInChildren<Text>();
        if (label) { label.fontStyle = selected ? FontStyle.Bold : FontStyle.Normal; label.color = selected ? Color.white : new Color(.68f, .75f, .82f); }
    }
    private string PadLabel(int button)
    {
        string[] names = LocalControls.ConnectedJoysticks(); int id = playerData.joystick;
        bool xbox = id > 0 && id <= names.Length && names[id - 1] != null &&
            (names[id - 1].IndexOf("xbox", StringComparison.OrdinalIgnoreCase) >= 0 || names[id - 1].IndexOf("xinput", StringComparison.OrdinalIgnoreCase) >= 0);
        return "Boton " + (xbox ? LocalControls.GamepadButtonLabel(button) : button.ToString());
    }
    private static string KeyLabel(string key)
    {
        switch (key)
        {
            case "Mouse0": return "Clic izquierdo"; case "Mouse1": return "Clic derecho"; case "Mouse2": return "Clic central";
            case "UpArrow": return "Flecha arriba"; case "DownArrow": return "Flecha abajo";
            case "LeftArrow": return "Flecha izquierda"; case "RightArrow": return "Flecha derecha";
            case "Space": return "Espacio"; case "LeftShift": return "Shift izquierdo"; case "RightShift": return "Shift derecho";
            case "Return": return "Enter"; default: return string.IsNullOrEmpty(key) ? "Sin asignar" : key;
        }
    }
    private void BeginCapture(int captureRow)
    {
        row = captureRow;
        int id = Mathf.Clamp(playerData.joystick, 1, 8);
        waitingForPadRelease = playerData.useGamepad && (LocalControls.TryGetPressedPadButton(id, out int _) ||
            (row < 4 && LocalControls.ReadPadAxes(id).sqrMagnitude > .0625f));
        string prompt = waitingForPadRelease ? "SUELTA EL BOTON Y PULSA EL QUE QUIERAS ASIGNAR" :
            playerData.useGamepad ? (row < 4 ? "MUEVE EL STICK IZQUIERDO O PULSA UN BOTON..." : "ESPERANDO BOTON DEL MANDO...") : "ESPERANDO TECLA O BOTON DEL MOUSE...";
        SetInputIndicator(prompt, new Color(.45f, .31f, .08f));
        Refresh();
    }
    private string DirectionLabel(string direction, int button) { return "Stick " + direction + (button < 0 ? "" : " / " + PadLabel(button)); }
    private void ResetDirections()
    {
        CancelCapture();
        profile.upButton = profile.downButton = profile.leftButton = profile.rightButton = -1;
        SaveRefresh(); SetInputIndicator("MOVIMIENTO ASIGNADO AL STICK IZQUIERDO", new Color(.08f, .48f, .28f));
        padFeedbackUntil = Time.unscaledTime + 2f;
    }
    private void SetCaptured(int button)
    {
        if (row == 0) profile.upButton = button; else if (row == 1) profile.downButton = button;
        else if (row == 2) profile.leftButton = button; else if (row == 3) profile.rightButton = button;
        else if (row == 4) profile.attackButton = button; else if (row == 5) profile.jumpButton = button; else if (row == 6) profile.specialButton = button;
        row = -1; LocalControls.Save(); SetInputIndicator(button < 0 ? "STICK IZQUIERDO ASIGNADO" : "BOTON " + LocalControls.GamepadButtonLabel(button) + " DETECTADO Y ASIGNADO", new Color(.08f, .48f, .28f));
        padFeedbackUntil = Time.unscaledTime + 2f; Refresh();
    }
    private void SetCapturedKey(string key)
    {
        switch (row)
        {
            case 0: profile.up = key; break; case 1: profile.down = key; break; case 2: profile.left = key; break; case 3: profile.right = key; break;
            case 4: profile.attack = key; break; case 5: profile.jump = key; break; case 6: profile.special = key; break;
        }
        row = -1; LocalControls.Save(); SetInputIndicator("Asignacion guardada: " + KeyLabel(key), new Color(.08f, .48f, .28f));
        padFeedbackUntil = Time.unscaledTime + 2f; Refresh();
    }
}
