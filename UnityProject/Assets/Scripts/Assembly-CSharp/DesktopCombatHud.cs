using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Non-interactive PC copies of the action buttons used by the Android HUD.</summary>
public sealed class DesktopCombatHud : MonoBehaviour
{
    private sealed class PlayerIndicator
    {
        public int playerIndex;
        public GameObject fighter;
        public Image attackCircle;
        public Image attackBackground;
        public Image dodgeCircle;
        public Image specialCircle;
        public Image specialCharge;
        public Text attackLetter;
        public Text dodgeLetter;
        public Text specialLetter;
        public Color attackRest;
        public Color dodgeRest;
        public Color specialRest;
        public Color attackLetterRest;
        public Color dodgeLetterRest;
        public Color specialLetterRest;
        public bool attackHeld;
        public bool dodgeHeld;
        public bool specialHeld;
        public float attackPulseUntil;
        public float dodgePulseUntil;
        public float specialPulseUntil;
    }

    private Canvas canvas;
    private PlayerIndicator first;
    private PlayerIndicator second;
    private static Font hudFont;

    public void Configure(GameObject player1, GameObject player2)
    {
        if (Application.isMobilePlatform || !player1) return;
        StartCoroutine(ConfigureAfterAwake(player1, player2));
    }

    private IEnumerator ConfigureAfterAwake(GameObject player1, GameObject player2)
    {
        // GameController creates this HUD in Awake. ButtonsController's Awake
        // may not have run yet, so wait before reading its original graphics.
        yield return null;
        if (!player1) yield break;
        hudFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        var canvasObject = new GameObject("DesktopCombatHUD", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        canvasObject.transform.SetParent(transform, false);
        canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 180;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = .5f;

        bool splitScreen = player2;
        first = CreateIndicator(canvasObject.transform, player1, 0, false, splitScreen);
        if (player2) second = CreateIndicator(canvasObject.transform, player2, 1, true, true);
    }

    private void Update()
    {
        var game = GameController.i;
        if (!canvas || !game) return;
        bool overlayMenuOpen = (Score.score && Score.score.battleFinish) ||
            (game.resumeButton && game.resumeButton.activeInHierarchy) ||
            (BattleSceneSingleton.i && BattleSceneSingleton.i.finishMenu && BattleSceneSingleton.i.finishMenu.activeInHierarchy) ||
            (LoadingScreen.i && LoadingScreen.i.loading);
        canvas.enabled = !overlayMenuOpen;
        if (overlayMenuOpen || !game.gameStarted || Time.timeScale <= 0f) return;
        UpdateIndicator(first);
        UpdateIndicator(second);
    }

    private PlayerIndicator CreateIndicator(Transform parent, GameObject fighter, int player, bool rightViewport, bool splitScreen)
    {
        var indicator = new PlayerIndicator { playerIndex = player, fighter = fighter };
        float diameter = (splitScreen ? 98f : 124f) * 1.25f;
        float gap = splitScreen ? 12f : 18f;
        float slot = diameter + gap;
        float anchorX = rightViewport ? .5f : 0f;

        var row = new GameObject("Player" + (player + 1) + "ActionButtons", typeof(RectTransform));
        row.transform.SetParent(parent, false);
        var rowRect = row.GetComponent<RectTransform>();
        rowRect.anchorMin = rowRect.anchorMax = new Vector2(anchorX, 0f);
        rowRect.pivot = Vector2.zero;
        rowRect.anchoredPosition = new Vector2(28f, 24f);
        rowRect.sizeDelta = new Vector2(diameter * 3f + gap * 2f, diameter + 30f);

        var buttons = ButtonsController.i;
        var attackCircleSource = buttons && buttons.buttonAttackA ? buttons.buttonAttackA.currentButtonCircle : null;
        var attackTextSource = buttons && buttons.buttonAttackA ? buttons.buttonAttackA.currentButtonLetter : null;
        var dodgeCircleSource = buttons ? buttons.buttonBCircle : null;
        var dodgeTextSource = buttons ? buttons.buttonBLetter : null;
        var meter = BattleSceneSingleton.i ? BattleSceneSingleton.i.SpecialAtk : null;
        var specialCircleSource = meter && meter.circle ? meter.circle : attackCircleSource;
        var specialTextSource = meter ? meter.letter : null;
        var chargeSource = meter ? meter.chargeImg : null;

        // Copy just the original graphics. Mobile button hierarchies may be
        // inactive, animated, or reference graphics outside their own root.
        // Desktop indicators own a simple fixed layout with no button scripts.
        indicator.attackCircle = CreateActionVisual(row.transform, "Attack", attackCircleSource, attackTextSource,
            0, diameter, slot, "A", null, out indicator.attackLetter, out _);
        var attackRadial = buttons && buttons.buttonAttackA ? buttons.buttonAttackA.currentRadialFX : null;
        var attackFillSource = attackRadial ? attackRadial.radialImage : null;
        if (attackFillSource && attackCircleSource)
        {
            // Reuse Android's full radial graphic, including its overhang beyond the outline.
            var background = new GameObject("AttackAlternateBackground", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(indicator.attackCircle.transform, false);
            background.transform.SetAsFirstSibling();
            var rect = background.GetComponent<RectTransform>();
            var corners = new Vector3[4];
            attackFillSource.rectTransform.GetWorldCorners(corners);
            var sourceRect = attackCircleSource.rectTransform;
            Vector2 min = sourceRect.InverseTransformPoint(corners[0]);
            Vector2 max = sourceRect.InverseTransformPoint(corners[2]);
            rect.anchorMin = new Vector2((min.x - sourceRect.rect.xMin) / sourceRect.rect.width,
                (min.y - sourceRect.rect.yMin) / sourceRect.rect.height);
            rect.anchorMax = new Vector2((max.x - sourceRect.rect.xMin) / sourceRect.rect.width,
                (max.y - sourceRect.rect.yMin) / sourceRect.rect.height);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            indicator.attackBackground = background.GetComponent<Image>();
            indicator.attackBackground.sprite = attackFillSource.sprite;
            indicator.attackBackground.material = attackFillSource.material;
            indicator.attackBackground.color = new Color(1f, 1f, 1f, .5f);
            indicator.attackBackground.raycastTarget = false;
            background.SetActive(false);
        }
        indicator.dodgeCircle = CreateActionVisual(row.transform, "Shield", dodgeCircleSource, dodgeTextSource,
            1, diameter, slot, "B", null, out indicator.dodgeLetter, out _);
        indicator.specialCircle = CreateActionVisual(row.transform, "Super", specialCircleSource, specialTextSource,
            2, diameter, slot, "S", chargeSource, out indicator.specialLetter, out indicator.specialCharge);

        indicator.attackRest = indicator.attackCircle ? indicator.attackCircle.color : Color.white;
        indicator.dodgeRest = indicator.dodgeCircle ? indicator.dodgeCircle.color : Color.white;
        indicator.specialRest = indicator.specialCircle ? indicator.specialCircle.color : Color.white;
        indicator.attackLetterRest = indicator.attackLetter ? indicator.attackLetter.color : Color.white;
        indicator.dodgeLetterRest = indicator.dodgeLetter ? indicator.dodgeLetter.color : Color.white;
        indicator.specialLetterRest = indicator.specialLetter ? indicator.specialLetter.color : Color.white;
        return indicator;
    }

    private static Image CreateActionVisual(Transform row, string name, Image sourceCircle, Text sourceLetter,
        int index, float diameter, float slot, string letter, Image chargeSource, out Text text, out Image charge)
    {
        var root = new GameObject(name + "Indicator", typeof(RectTransform), typeof(Image));
        root.transform.SetParent(row, false);
        var rect = root.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.zero;
        rect.anchoredPosition = new Vector2(index * slot, 28f);
        rect.sizeDelta = new Vector2(diameter, diameter);
        var circle = root.GetComponent<Image>();
        if (sourceCircle)
        {
            circle.sprite = sourceCircle.sprite;
            circle.material = sourceCircle.material;
            circle.type = sourceCircle.type;
            circle.preserveAspect = true;
        }
        circle.color = new Color(1f, 1f, 1f, .85f);
        circle.raycastTarget = false;

        charge = null;
        if (chargeSource)
        {
            var fill = new GameObject("SuperCharge", typeof(RectTransform), typeof(Image));
            fill.transform.SetParent(root.transform, false);
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(.04f, .04f);
            fillRect.anchorMax = new Vector2(.96f, .96f);
            fillRect.offsetMin = fillRect.offsetMax = Vector2.zero;
            charge = fill.GetComponent<Image>();
            charge.sprite = chargeSource.sprite;
            charge.material = chargeSource.material;
            charge.type = Image.Type.Filled;
            charge.fillMethod = Image.FillMethod.Radial360;
            charge.fillOrigin = chargeSource.fillOrigin;
            charge.fillClockwise = chargeSource.fillClockwise;
            charge.fillAmount = 0f;
            charge.raycastTarget = false;
        }
        float originalDiameter = sourceCircle ? sourceCircle.rectTransform.sizeDelta.x : 200f;
        int size = sourceLetter ? Mathf.RoundToInt(sourceLetter.fontSize * diameter / Mathf.Max(1f, originalDiameter))
            : Mathf.RoundToInt(diameter * .5f);
        text = MakeText(root.transform, name + "Letter", letter, new Vector2(.5f, .5f), Vector2.zero,
            new Vector2(diameter, diameter), Mathf.Max(size, 28), TextAnchor.MiddleCenter, Color.white);
        if (sourceLetter)
        {
            text.font = sourceLetter.font;
            text.fontStyle = sourceLetter.fontStyle;
            text.material = sourceLetter.material;
        }
        return circle;
    }

    private void UpdateIndicator(PlayerIndicator indicator)
    {
        if (indicator == null || !indicator.fighter) return;
        InputSet input = LocalControls.ReadPlayer(indicator.playerIndex);
        float now = Time.unscaledTime;
        if (input.a && !indicator.attackHeld) indicator.attackPulseUntil = now + .18f;
        if (input.b && !indicator.dodgeHeld) indicator.dodgePulseUntil = now + .18f;
        if (input.s && !indicator.specialHeld) indicator.specialPulseUntil = now + .24f;
        indicator.attackHeld = input.a;
        indicator.dodgeHeld = input.b;
        indicator.specialHeld = input.s;

        var animator = indicator.fighter.GetComponent<Animator>();
        bool alternateAttack = animator && animator.GetInteger("BasicAttack") != 0;
        if (indicator.attackBackground) indicator.attackBackground.gameObject.SetActive(alternateAttack);
        if (indicator.attackLetter) indicator.attackLetter.text = alternateAttack ? "X" : "A";
        SetPressed(indicator.attackCircle, indicator.attackLetter,
            input.a || now < indicator.attackPulseUntil,
            alternateAttack ? Color.black : indicator.attackRest,
            alternateAttack ? Color.black : indicator.attackLetterRest);
        SetPressed(indicator.dodgeCircle, indicator.dodgeLetter,
            input.b || now < indicator.dodgePulseUntil, indicator.dodgeRest, indicator.dodgeLetterRest);

        float amount;
        bool ready;
        if (indicator.playerIndex == 0 && !LocalOfflinePlayerInput.IsLocalBattle)
        {
            var meter = BattleSceneSingleton.i ? BattleSceneSingleton.i.SpecialAtk : null;
            amount = meter && meter.chargeImg ? meter.chargeImg.fillAmount : 0f;
            ready = meter && meter.charged;
        }
        else
        {
            var damage = indicator.fighter.GetComponent<DamageOnCollision>();
            amount = damage ? Mathf.Clamp01(damage.specialCharge / LocalOfflinePlayerInput.SpecialCapacity) : 0f;
            ready = LocalOfflinePlayerInput.IsSpecialReady(indicator.fighter);
        }
        amount = Mathf.Clamp01(amount);
        bool unlocked = LocalOfflinePlayerInput.IsLocalBattle || IsSpecialUnlocked(indicator.fighter);
        bool active = input.s || now < indicator.specialPulseUntil || ready;
        SetPressed(indicator.specialCircle, indicator.specialLetter, active,
            indicator.specialRest, indicator.specialLetterRest);
        if (indicator.specialLetter) indicator.specialLetter.color = unlocked ? indicator.specialLetter.color : new Color(1f, 1f, 1f, .4f);
        if (indicator.specialCharge)
        {
            indicator.specialCharge.fillAmount = ready ? 1f : amount;
            indicator.specialCharge.color = ready ? new Color(.35f, .94f, .48f, .86f) : new Color(.13f, .76f, .97f, .72f);
        }
    }

    private static void SetPressed(Image circle, Text letter, bool pressed, Color circleRest, Color letterRest)
    {
        if (circle) circle.color = pressed ? new Color(0f, 0f, 0f, .55f) : circleRest;
        if (letter) letter.color = pressed ? new Color(0f, 0f, 0f, .65f) : letterRest;
    }

    private static bool IsSpecialUnlocked(GameObject fighter)
    {
        if (!fighter) return false;
        string name = fighter.name.Replace("(Clone)", string.Empty);
        string key;
        switch (name)
        {
            case "Rina": case "Amon": key = "Rina"; break;
            case "Dante": case "Agata": key = "Dante"; break;
            case "Eliot": case "Vinea": key = "Eliot"; break;
            case "Sora": case "Aura": key = "Aura"; break;
            case "Diana": case "Ren": key = "Ren"; break;
            default: return false;
        }
        return PlayerPrefs.HasKey(key);
    }

    private static Text MakeText(Transform parent, string name, string value, Vector2 anchor, Vector2 position,
        Vector2 size, int fontSize, TextAnchor alignment, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor; rect.pivot = anchor; rect.anchoredPosition = position; rect.sizeDelta = size;
        var text = go.GetComponent<Text>(); text.font = hudFont; text.text = value; text.fontSize = fontSize;
        text.alignment = alignment; text.color = color; text.raycastTarget = false;
        return text;
    }
}
