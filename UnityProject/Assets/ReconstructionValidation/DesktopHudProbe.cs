#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class DesktopHudProbe
{
    [Serializable] private class ButtonSample
    {
        public string name;
        public bool visible, passive;
        public float x, y, width, height;
    }
    [Serializable] private class Report
    {
        public string mode;
        public List<ButtonSample> buttons = new List<ButtonSample>();
        public bool halfChargeVisible;
        public List<float> cameraDistances = new List<float>();
    }
    public static IEnumerator Run(GameController game, string output, Action<string, string> observe, Action<string> fail)
    {
        bool split = SystemManager.i.mode == GameMode.LocalOffline;
        string label = split ? "local" : "single";
        string prefix = Path.ChangeExtension(output, null) + ".hud-" + label;
        var report = new Report { mode = label };
        var hud = game.GetComponent<DesktopCombatHud>();
        if (!hud) { fail("Desktop combat HUD missing"); yield break; }
        var canvas = hud.transform.Find("DesktopCombatHUD");
        if (!canvas) { fail("Desktop combat HUD canvas missing"); yield break; }
        yield return null;
        Canvas.ForceUpdateCanvases();
        if (split)
        {
            foreach (var camera in UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude))
            {
                if (!camera.enabled || Mathf.Abs(camera.rect.width - .5f) > .001f || camera.rect.height != 1f) continue;
                var player = camera.rect.x < .25f ? game.MainCharacter : game.OpponentCharacter;
                float distance = Vector3.ProjectOnPlane(camera.transform.position - player.transform.position, Vector3.up).magnitude;
                report.cameraDistances.Add(distance);
                float expected = (PlayerPrefs.GetInt("Cam", 0) != 0 ? 6f : 4f) * 1.6f * 1.25f;
                if (Mathf.Abs(distance - expected) > .02f) fail("Local camera did not use the updated zoom distance");
            }
            if (report.cameraDistances.Count != 2) fail("Expected two local camera viewports");
        }
        for (int player = 0; player < (split ? 2 : 1); player++)
        {
            var row = canvas.Find("Player" + (player + 1) + "ActionButtons");
            if (!row) { fail("HUD row missing for player " + (player + 1)); continue; }
            float minimum = split && player == 1 ? Screen.width * .5f : 0f;
            float maximum = split && player == 0 ? Screen.width * .5f : Screen.width;
            float previousRight = minimum;
            foreach (string name in new[] { "Attack", "Shield", "Super" })
            {
                var target = row.Find(name + "Indicator");
                if (!target) { fail("Missing " + name + " for player " + (player + 1)); continue; }
                var image = target.GetComponent<Image>();
                var rect = ScreenRect(target as RectTransform);
                var sample = new ButtonSample { name = "P" + (player + 1) + " " + name,
                    visible = image && image.enabled && image.sprite && image.color.a >= .5f && image.gameObject.activeInHierarchy,
                    passive = target.GetComponentsInChildren<Selectable>(true).Length == 0 && target.GetComponentsInChildren<EventTrigger>(true).Length == 0,
                    x = rect.x, y = rect.y, width = rect.width, height = rect.height };
                foreach (var graphic in target.GetComponentsInChildren<Graphic>(true)) sample.passive &= !graphic.raycastTarget;
                report.buttons.Add(sample);
                if (!sample.visible || !sample.passive || rect.x < previousRight || rect.xMin < minimum || rect.xMax > maximum || rect.yMin < 0 || rect.yMax > Screen.height)
                    fail("HUD indicator invisible, interactive, overlapping or outside its viewport: " + sample.name);
                float expected = (split ? 88f : 112f) * canvas.GetComponent<Canvas>().scaleFactor;
                if (Mathf.Abs(rect.width - expected) > 1f) fail("HUD indicator size mismatch: " + sample.name);
                previousRight = rect.xMax;
            }
        }
        var buttons = ButtonsController.i;
        var firstAttack = canvas.Find("Player1ActionButtons/AttackIndicator").GetComponent<Image>();
        var firstShield = canvas.Find("Player1ActionButtons/ShieldIndicator").GetComponent<Image>();
        if (firstAttack.sprite != buttons.buttonAttackA.currentButtonCircle.sprite || firstShield.sprite != buttons.buttonBCircle.sprite)
            fail("HUD did not retain the original Android action-button sprites");
        ScreenCapture.CaptureScreenshot(prefix + ".png");
        yield return new WaitForEndOfFrame();
        yield return null;

        var meter = BattleSceneSingleton.i.SpecialAtk;
        float oldFill = meter.chargeImg.fillAmount;
        bool oldReady = meter.charged;
        var opponentDamage = game.OpponentCharacter.GetComponent<DamageOnCollision>();
        float oldSecondCharge = opponentDamage.specialCharge;
        try
        {
            meter.chargeImg.fillAmount = .5f; meter.charged = false;
            if (split) opponentDamage.specialCharge = 5f;
            yield return null;
            yield return null;
            report.halfChargeVisible = true;
            for (int player = 1; player <= (split ? 2 : 1); player++)
            {
                var row = canvas.Find("Player" + player + "ActionButtons");
                var fill = row.Find("SuperIndicator/SuperCharge").GetComponent<Image>();
                var value = row.Find("SuperChargeValue").GetComponent<Text>();
                var valueRect = ScreenRect(value.rectTransform);
                float minimum = split && player == 2 ? Screen.width * .5f : 0;
                float maximum = split && player == 1 ? Screen.width * .5f : Screen.width;
                bool valid = Mathf.Abs(fill.fillAmount - .5f) < .01f && value.text == "50%" &&
                    valueRect.xMin >= minimum && valueRect.xMax <= maximum && valueRect.yMin >= 0;
                report.halfChargeVisible &= valid;
            }
            if (!report.halfChargeVisible) fail("Super charge fill/value not visible at 50% in " + label);
            ScreenCapture.CaptureScreenshot(prefix + ".half.png");
            yield return new WaitForEndOfFrame();
            yield return null;
        }
        finally
        {
            meter.chargeImg.fillAmount = oldFill; meter.charged = oldReady;
            if (opponentDamage) opponentDamage.specialCharge = oldSecondCharge;
        }
        File.WriteAllText(prefix + ".json", JsonUtility.ToJson(report, true));
        observe("desktop-hud-" + label, JsonUtility.ToJson(report));
    }
    private static Rect ScreenRect(RectTransform rect)
    {
        var corners = new Vector3[4]; rect.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
}
#endif
