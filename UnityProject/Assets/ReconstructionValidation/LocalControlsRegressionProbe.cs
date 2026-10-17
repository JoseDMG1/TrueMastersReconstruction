#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Explicit opt-in regression fixture. Never runs in an ordinary game launch.
public class LocalControlsRegressionProbe : MonoBehaviour
{
    [Serializable] private class Report
    {
        public bool passed;
        public string scope = "Controls menu callbacks, navigation isolation, stick direction capture and analog/digital composition; no simulated hardware input.";
        public List<string> checks = new List<string>();
        public List<string> errors = new List<string>();
    }
    private const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;
    private Report report = new Report();
    private string output;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, "-rea-controls-regression");
        if (index < 0 || index + 1 >= args.Length) return;
        var root = new GameObject("LocalControlsRegressionProbe");
        DontDestroyOnLoad(root);
        root.AddComponent<LocalControlsRegressionProbe>().output = args[index + 1];
    }
    private IEnumerator Start()
    {
        LocalControlsMenu menu = null;
        float deadline = Time.realtimeSinceStartup + 30f;
        while (!menu && Time.realtimeSinceStartup < deadline)
        {
            menu = FindFirstObjectByType<LocalControlsMenu>();
            yield return null;
        }
        if (!menu) { report.errors.Add("Controls menu was not installed."); Finish(); yield break; }
        // Let the menu's Start create its canvas.
        yield return null;
        // Scene activation precedes completion of the incoming loading fade.
        // Opening Controls during that interval is intentionally blocked.
        while (LoadingScreen.i && LoadingScreen.i.loading && Time.realtimeSinceStartup < deadline)
            yield return null;
        if (LoadingScreen.i && LoadingScreen.i.loading)
        {
            report.errors.Add("Initial menu loading did not finish."); Finish(); yield break;
        }
        menu.enabled = false;
        string snapshot = JsonUtility.ToJson(LocalControls.Data);
        bool hadPrefs = PlayerPrefs.HasKey("LocalControls.v1");
        string prefSnapshot = PlayerPrefs.GetString("LocalControls.v1", "");
        bool hadAlternate = PlayerPrefs.HasKey("AlternateKeyboardControls");
        int alternateSnapshot = PlayerPrefs.GetInt("AlternateKeyboardControls", 0);
        bool navigationSnapshot = EventSystem.current.sendNavigationEvents;
        bool loadingSnapshot = LoadingScreen.i && LoadingScreen.i.loading;
        try
        {
            var combine = typeof(LocalControls).GetMethod("CombinePadDirection", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (float axis in new[] { -1f, -.4f, 0f, .4f, 1f })
            {
                float result = (float)combine.Invoke(null, new object[] { axis, false, false });
                Check(Mathf.Approximately(axis, result), "Idle D-pad preserves analog " + axis);
            }
            Check((float)combine.Invoke(null, new object[] { 0f, true, false }) == -1f, "Negative digital direction works");
            Check((float)combine.Invoke(null, new object[] { 0f, false, true }) == 1f, "Positive digital direction works");
            Call(menu, "TogglePanel", true);
            Check(!EventSystem.current.sendNavigationEvents, "A/B submit and cancel disabled while controls panel is open");
            var panel = (GameObject)Field(menu, "panel").GetValue(menu);
            Canvas.ForceUpdateCanvases();
            var visibleButtons = panel.GetComponentsInChildren<Button>();
            bool separated = true;
            for (int a = 0; a < visibleButtons.Length; a++)
            for (int b = a + 1; b < visibleButtons.Length; b++)
                if (ScreenRect(visibleButtons[a]).Overlaps(ScreenRect(visibleButtons[b]))) separated = false;
            Check(separated, "Visible menu buttons have separate click targets without overlap");
            Field(menu, "player").SetValue(menu, 0);
            Call(menu, "SelectInputMode", false);
            Check(!((Button)Field(menu, "previousDeviceButton").GetValue(menu)).gameObject.activeSelf &&
                !((Button)Field(menu, "stickButton").GetValue(menu)).gameObject.activeSelf, "Keyboard mode hides unrelated gamepad options");
            string attackBefore = LocalControls.Data.player1.defaults.attack;
            Call(menu, "BeginCapture", 4);
            Call(menu, "SelectPlayer", 1);
            Check((int)Field(menu, "row").GetValue(menu) == -1 && LocalControls.Data.player1.defaults.attack == attackBefore,
                "Switching player cancels capture without changing the previous binding");
            var match = typeof(LocalControlsMenu).GetMethod("StickMatchesDirection", BindingFlags.NonPublic | BindingFlags.Static);
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            for (int player = 0; player < 2; player++)
            for (int alternate = 0; alternate < 2; alternate++)
            {
                Field(menu, "player").SetValue(menu, player);
                Field(menu, "alternate").SetValue(menu, alternate != 0);
                var data = player == 0 ? LocalControls.Data.player1 : LocalControls.Data.player2;
                data.useGamepad = true;
                var profile = alternate == 0 ? data.defaults : data.alternate;
                for (int direction = 0; direction < 4; direction++)
                {
                    Check((bool)match.Invoke(null, new object[] { direction, directions[direction] }), "P" + (player + 1) + " preset " + alternate + " accepts stick direction " + direction);
                    Check(!(bool)match.Invoke(null, new object[] { direction, -directions[direction] }), "Opposite stick direction does not bind " + direction);
                    Call(menu, "BeginCapture", direction);
                    Call(menu, "SetCaptured", -1);
                }
                Check(profile.upButton == -1 && profile.downButton == -1 && profile.leftButton == -1 && profile.rightButton == -1, "Four stick directions saved for P" + (player + 1) + " preset " + alternate);
                foreach (int button in new[] { 0, 1 })
                {
                    Call(menu, "BeginCapture", 4);
                    Call(menu, "SetCaptured", button);
                    Check(profile.attackButton == button && panel.activeSelf && (int)Field(menu, "row").GetValue(menu) == -1,
                        "P" + (player + 1) + " preset " + alternate + " assigns " + (button == 0 ? "A" : "B") + " and keeps panel open");
                }
                var saved = JsonUtility.FromJson<LocalControlsData>(PlayerPrefs.GetString("LocalControls.v1"));
                var savedPlayer = player == 0 ? saved.player1 : saved.player2;
                Check((alternate == 0 ? savedPlayer.defaults : savedPlayer.alternate).attackButton == 1, "Binding persisted to preferences");
            }
            if (LoadingScreen.i)
            {
                LoadingScreen.i.loading = true;
                Call(menu, "RefreshTransitionVisibility");
                var canvas = (Canvas)Field(menu, "controlsCanvas").GetValue(menu);
                Check(!canvas.gameObject.activeSelf && !panel.activeSelf && (int)Field(menu, "row").GetValue(menu) == -1,
                    "Loading hides the full controls overlay and cancels capture");
                LoadingScreen.i.loading = false;
                Call(menu, "RefreshTransitionVisibility");
                Check(canvas.gameObject.activeSelf && ((Button)Field(menu, "controlsButton").GetValue(menu)).gameObject.activeSelf,
                    "Controls entry returns when loading ends without reopening the panel");
            }
            else report.errors.Add("Loading screen controller missing");
            Call(menu, "TogglePanel", false);
            Check(EventSystem.current.sendNavigationEvents == navigationSnapshot, "Navigation restored on close");
        }
        catch (Exception exception) { report.errors.Add(exception.ToString()); }
        finally
        {
            if (LoadingScreen.i) LoadingScreen.i.loading = loadingSnapshot;
            Call(menu, "TogglePanel", false);
            typeof(LocalControls).GetField("cached", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, JsonUtility.FromJson<LocalControlsData>(snapshot));
            if (hadPrefs) PlayerPrefs.SetString("LocalControls.v1", prefSnapshot); else PlayerPrefs.DeleteKey("LocalControls.v1");
            if (hadAlternate) PlayerPrefs.SetInt("AlternateKeyboardControls", alternateSnapshot); else PlayerPrefs.DeleteKey("AlternateKeyboardControls");
            PlayerPrefs.Save();
            menu.enabled = true;
        }
        Finish();
    }
    private static FieldInfo Field(object target, string name) { return target.GetType().GetField(name, Private); }
    private static Rect ScreenRect(Button button)
    {
        var corners = new Vector3[4];
        ((RectTransform)button.transform).GetWorldCorners(corners);
        return new Rect(corners[0].x + .1f, corners[0].y + .1f, corners[2].x - corners[0].x - .2f, corners[2].y - corners[0].y - .2f);
    }
    private static void Call(object target, string name, params object[] args) { target.GetType().GetMethod(name, Private).Invoke(target, args); }
    private void Check(bool condition, string label) { if (condition) report.checks.Add(label); else report.errors.Add(label); }
    private void Finish()
    {
        report.passed = report.errors.Count == 0;
        File.WriteAllText(output, JsonUtility.ToJson(report, true));
        Application.Quit(report.passed ? 0 : 1);
    }
}
#endif
