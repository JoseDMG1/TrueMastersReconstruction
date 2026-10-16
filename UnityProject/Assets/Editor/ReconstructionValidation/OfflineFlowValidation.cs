using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Exercises real recovered scenes and serialized Button callbacks in Play Mode.
// This is a reconstruction acceptance probe, not proof of original-game fidelity.
[InitializeOnLoad]
public static class OfflineFlowValidation
{
    const string ActiveKey = "TrueMasters.OfflineFlowProbe";
    const string ResultKey = "TrueMasters.OfflineFlowResult";
    [Serializable] public class Observation { public string step, scene, detail; public double seconds; }
    [Serializable] public class Report {
        public string generatedUtc, unityVersion, result;
        public List<Observation> observations = new List<Observation>();
        public List<string> errors = new List<string>();
        public List<string> editorInfrastructureErrors = new List<string>();
        public string limitation = "Only menu-to-arena startup and input-ready state exercised; no full match or equivalence assertion.";
    }
    static Report report;
    static double started, entered;
    static int step;
    static bool finishing;

    static OfflineFlowValidation() {
        EditorApplication.playModeStateChanged += ModeChanged;
        EditorApplication.update += Update;
        Application.logMessageReceived += RecordError;
    }
    public static void Run() {
        SessionState.SetBool(ActiveKey, true);
        SessionState.EraseString(ResultKey);
        EditorSceneManager.OpenScene("Assets/Scenes/DesarrolladoPor.unity");
        EditorApplication.EnterPlaymode();
    }
    static void ModeChanged(PlayModeStateChange state) {
        if (!SessionState.GetBool(ActiveKey, false)) return;
        if (state == PlayModeStateChange.EnteredPlayMode) {
            report = new Report { generatedUtc = DateTime.UtcNow.ToString("O"), unityVersion = Application.unityVersion };
            started = entered = EditorApplication.timeSinceStartup;
            step = 0;
            Observe("boot", "Entered Play Mode with original boot scene");
        }
        if (state == PlayModeStateChange.EnteredEditMode && SessionState.GetString(ResultKey, "") != "") {
            SessionState.SetBool(ActiveKey, false);
            EditorApplication.Exit(SessionState.GetString(ResultKey, "") == "passed" ? 0 : 1);
        }
    }
    static void RecordError(string message, string stack, LogType type) {
        if (report == null || finishing || !SessionState.GetBool(ActiveKey, false)) return;
        if (type == LogType.Exception || type == LogType.Error || type == LogType.Assert) {
            string error = message + "\n" + stack;
            // Known editor-only indexing failure: retain it separately rather than
            // mislabel it as an exception from the reconstructed player.
            if (stack.Contains("UnityEditor.Search.SearchDatabase") && stack.Contains("UnityEditor.Search.SearchInit.IndexationOnStartup")) {
                if (!report.editorInfrastructureErrors.Contains(error)) report.editorInfrastructureErrors.Add(error);
                return;
            }
            if (!report.errors.Contains(error)) report.errors.Add(error);
        }
    }
    static void Observe(string name, string detail) {
        report.observations.Add(new Observation { step = name, scene = SceneManager.GetActiveScene().name, detail = detail, seconds = EditorApplication.timeSinceStartup - started });
        Debug.Log("OFFLINE_FLOW_PROBE " + name + ": " + detail);
    }
    static void Update() {
        if (!SessionState.GetBool(ActiveKey, false) || !EditorApplication.isPlaying || report == null || finishing) return;
        try {
            double now = EditorApplication.timeSinceStartup;
            string scene = SceneManager.GetActiveScene().name;
            if (now - started > 100) { Finish("timeout"); return; }
            if (report.errors.Count > 0 && now - entered > 3) { Finish("runtime_errors"); return; }
            bool transitionReady = LoadingScreen.i != null && !LoadingScreen.i.loading;
            if (step == 0 && scene == "StartMenu" && transitionReady && now - entered > 1) {
                InvokeSerializedButton("SinglePlayerMode"); step = 1; entered = now;
            } else if (step == 1 && scene == "HowToPlay" && transitionReady && now - entered > 1) {
                InvokeSerializedPointerClick("Continue"); step = 2; entered = now;
            } else if ((step == 1 || step == 2) && scene == "Character Selection" && transitionReady && now - entered > 2) {
                Observe("selection", "Character=" + PlayerPrefs.GetString("Character") + "; CPU=" + PlayerPrefs.GetString("CharacterCPU") + "; CpuMode=" + PlayerPrefs.GetInt("CpuMode", 0));
                InvokeSerializedPointerClick("start"); step = 3; entered = now;
            } else if (step == 3 && scene == "Bending Battle" && now - entered > 1 && GameController.i != null) {
                var game = GameController.i;
                if (game.MainCharacter == null || game.OpponentCharacter == null) throw new InvalidOperationException("Arena has no two spawned fighters");
                if (!game.gameStarted) return;
                Observe("arena-ready", game.MainCharacter.name + " vs " + game.OpponentCharacter.name + "; input ready=" + game.gameStarted);
                Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
            }
        } catch (Exception e) { RecordError(e.Message, e.StackTrace, LogType.Exception); Finish("probe_failed"); }
    }
    static void InvokeSerializedButton(string method) {
        var candidates = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(b => b.IsInteractable() && Enumerable.Range(0, b.onClick.GetPersistentEventCount()).Any(i => b.onClick.GetPersistentMethodName(i) == method)).ToArray();
        if (candidates.Length != 1) throw new InvalidOperationException("Expected one active button callback " + method + ", found " + candidates.Length);
        Observe("button", candidates[0].name + " -> " + method);
        candidates[0].onClick.Invoke();
    }
    static void InvokeSerializedPointerClick(string method) {
        var candidates = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(t => t.triggers.Any(e => e.eventID == EventTriggerType.PointerClick &&
                Enumerable.Range(0, e.callback.GetPersistentEventCount()).Any(i => e.callback.GetPersistentMethodName(i) == method))).ToArray();
        if (candidates.Length != 1 || EventSystem.current == null) throw new InvalidOperationException("Expected one active pointer-click callback " + method + ", found " + candidates.Length);
        Observe("pointer-click", candidates[0].name + " -> " + method);
        ExecuteEvents.Execute(candidates[0].gameObject, new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left }, ExecuteEvents.pointerClickHandler);
    }
    static void Finish(string result) {
        if (finishing) return;
        finishing = true;
        report.result = result;
        SessionState.SetString(ResultKey, result);
        Application.logMessageReceived -= RecordError;
        var destination = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Evidence/offline-flow-validation.json"));
        File.WriteAllText(destination, JsonUtility.ToJson(report, true));
        Debug.Log("OFFLINE_FLOW_PROBE_COMPLETE " + result);
        EditorApplication.ExitPlaymode();
    }
}
