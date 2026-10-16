#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Opt-in acceptance probe in development builds only. Normal play is unaffected.
public sealed class OfflinePlayerProbe : MonoBehaviour
{
    [Serializable] public class Observation { public string step, scene, detail; public double seconds; }
    [Serializable] public class Report {
        public string generatedUtc, unityVersion, result, graphicsDevice;
        public List<Observation> observations = new List<Observation>();
        public List<string> errors = new List<string>();
        public string limitation = "Startup, movement, attack spawning and short CPU observation only; no full-match or visual equivalence assertion.";
    }
    [Serializable] public class RenderItem { public string path, shader, material, texture; public Vector3 center, size; public Color color; }
    [Serializable] public class LightingItem { public string path, type; public float intensity; public bool enabled; }
    [Serializable] public class RenderReport { public List<RenderItem> renderers = new List<RenderItem>(); public List<LightingItem> lights = new List<LightingItem>(); }
    [Serializable] public class MoveInputContract { public string fighter, state, controller, controllerSha256, transitionId; public int direction, variant; public bool defaultEntry; }
    [Serializable] public class MoveInputContracts { public string source, sourceSha256; public MoveInputContract[] routes; }
    sealed class ShadowPowerObservation { public TrapPowerUp owner; public float damage, defense; public Vector3 scale; public bool seen; }
    static readonly System.Reflection.FieldInfo shadowPoweredField=typeof(TrapPowerUp).GetField("powered",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
    Report report;
    string output;
    double started;
    double nextDiagnostic;
    bool done;
    int playerProjectiles, opponentProjectiles, hitEvents;
    readonly HashSet<EntityId> seen = new HashSet<EntityId>();
    bool observeSpecialEffects, sawGrassHealing;
    float maxWingsScale;
    GameEvents creationEventOwner;
    Action<EntityId> creationObserver;
    bool observeMoves;
    bool observeAim;
    float maximumAimError;
    int aimFrames;
    readonly HashSet<EntityId> liveAimTargets = new HashSet<EntityId>();
    readonly HashSet<EntityId> liveFollowTargets = new HashSet<EntityId>();
    IEnumerator ObserveLiveAim(GameController game) {
        while(observeAim && game && game.MainCharacter && game.OpponentCharacter) {
            yield return new WaitForEndOfFrame();
            if(!observeAim)yield break;
            var root=game.MainCharacter.transform;
            var direction=game.OpponentCharacter.transform.position-root.position;direction.y=0;
            if(!game.animMainChar.GetCurrentAnimatorStateInfo(0).IsName("Freeze") && direction.sqrMagnitude>.00001f) {
                maximumAimError=Mathf.Max(maximumAimError,Vector3.Angle(root.forward,direction));aimFrames++;
            }
            foreach(var homing in UnityEngine.Object.FindObjectsByType<HomingAttack>(FindObjectsInactive.Exclude)) {
                if(!homing.target)continue;
                var scene=BattleSceneSingleton.i;
                bool own=homing.gameObject.layer==LayerMask.NameToLayer("PlayerAtk") || homing.gameObject.layer==scene.MainCharacter.layer;
                var expected=own?scene.Opponent:scene.MainCharacter;
                if(homing.target!=expected){Fail("Live homing selected its owner: "+homing.name);yield break;}
                liveAimTargets.Add(homing.gameObject.GetEntityId());
            }
            foreach(var follow in UnityEngine.Object.FindObjectsByType<Follow>(FindObjectsInactive.Exclude)) {
                if(!follow.target)continue;
                var scene=BattleSceneSingleton.i;
                var expected=follow.gameObject.layer==scene.MainCharacter.layer?scene.Opponent:scene.MainCharacter;
                if(follow.target!=expected){Fail("Live Follow selected its owner: "+follow.name);yield break;}
                liveFollowTargets.Add(follow.gameObject.GetEntityId());
            }
        }
    }
    readonly Dictionary<EntityId, Vector3> moveOrigins = new Dictionary<EntityId, Vector3>();
    readonly Dictionary<EntityId, float> moveTravel = new Dictionary<EntityId, float>();
    readonly Dictionary<EntityId, string> moveLabels = new Dictionary<EntityId, string>();
    readonly HashSet<string> moveSpawnNames = new HashSet<string>();
    readonly Dictionary<EntityId, Quaternion> expectedRotations = new Dictionary<EntityId, Quaternion>();
    readonly Dictionary<EntityId, float> rotationDue = new Dictionary<EntityId, float>();
    readonly Dictionary<EntityId, float> rotationErrors = new Dictionary<EntityId, float>();
    readonly Dictionary<EntityId, ShadowPowerObservation> shadowPowers = new Dictionary<EntityId, ShadowPowerObservation>();
    readonly List<string> shadowPowerResults = new List<string>();
    readonly Dictionary<EntityId,Vector3> specialRockOrigins = new Dictionary<EntityId,Vector3>();
    readonly Dictionary<EntityId,float> specialRockTravel = new Dictionary<EntityId,float>();
    TMPlayer savedPlayer;
    readonly Dictionary<string, byte[]> savedFiles = new Dictionary<string, byte[]>();
    readonly Dictionary<string, string> savedStringPrefs = new Dictionary<string, string>();
    readonly Dictionary<string, int> savedIntPrefs = new Dictionary<string, int>();
    readonly HashSet<string> absentPrefs = new HashSet<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Launch() {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, "-rea-offline-probe");
        if (index < 0) return;
        if (index + 1 >= args.Length) throw new ArgumentException("-rea-offline-probe requires an absolute report path");
        // Hidden acceptance runs must keep ticking without keyboard focus.
        // This opt-in setting does not change normal interactive play.
        Application.runInBackground = true;
        var probe = new GameObject("Offline acceptance probe").AddComponent<OfflinePlayerProbe>();
        DontDestroyOnLoad(probe.gameObject);
        probe.output = Path.GetFullPath(args[index + 1]);
        probe.started = Time.realtimeSinceStartupAsDouble;
        probe.report = new Report { generatedUtc = DateTime.UtcNow.ToString("O"), unityVersion = Application.unityVersion, graphicsDevice = SystemInfo.graphicsDeviceType.ToString() };
        Application.logMessageReceived += probe.Record;
        probe.StartCoroutine(probe.Run());
    }
    void Record(string message, string stack, LogType type) {
        if (done || report == null) return;
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) {
            string value = message + "\n" + stack;
            if (!report.errors.Contains(value)) report.errors.Add(value);
        }
    }
    void Update() {
        if (done || report == null) return;
        double maximumSeconds = Environment.GetCommandLineArgs().Contains("-rea-desktop-controls-probe") ? 300 : Environment.GetCommandLineArgs().Any(a => a == "-rea-ability-aim-probe" || a == "-rea-shadow-roster-probe" || a == "-rea-roster-probe" || a == "-rea-specials-probe" || a == "-rea-defense-probe" || a == "-rea-moves-probe") ? 1200 : 180;
        if (Time.realtimeSinceStartupAsDouble - started > maximumSeconds) { Finish("timeout"); return; }
        if (report.errors.Count > 0) { Finish("runtime_errors"); return; }
        if(observeMoves) {
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveRock>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveLight>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveWater>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveObject>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveObjectLocal>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MoveForward>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<MovePath>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var moving in UnityEngine.Object.FindObjectsByType<FollowPath>(FindObjectsInactive.Exclude)) TrackMove(moving);
            foreach(var rotation in UnityEngine.Object.FindObjectsByType<StartRotation>(FindObjectsInactive.Exclude)) {
                var id=rotation.gameObject.GetEntityId();
                if(expectedRotations.ContainsKey(id) && Time.time>=rotationDue[id])
                    rotationErrors[id]=Mathf.Min(rotationErrors[id],Quaternion.Angle(expectedRotations[id],rotation.transform.rotation));
            }
            foreach(var sample in shadowPowers.Values) {
                var owner=sample.owner;
                if(sample.seen || !owner || shadowPoweredField==null || !(bool)shadowPoweredField.GetValue(owner)) continue;
                var description=owner.GetComponent<Description>();
                bool matches=Mathf.Abs(description.Damage-Mathf.Min(sample.damage+1,3))<.001f && Mathf.Abs(description.Defense-sample.defense-1)<.001f;
                shadowPowerResults.Add(owner.name+": owner powered=True; native damage/defense="+matches+"; damage="+sample.damage+"->"+description.Damage+"; defense="+sample.defense+"->"+description.Defense+"; FX="+(owner.powerUpFX && owner.powerUpFX.activeSelf)+"; scale at creation="+sample.scale+"; scale now="+(owner.ScaleObject ? owner.ScaleObject.transform.localScale : Vector3.one)+"; operation-entry scale not sampled, multiplier not accepted");
                sample.seen=true;
            }
        }
        if(observeSpecialEffects) {
            foreach(var rock in UnityEngine.Object.FindObjectsByType<MoveSpecialRock>(FindObjectsInactive.Exclude)) {
                var id = rock.GetEntityId();
                if(!specialRockOrigins.ContainsKey(id)) { specialRockOrigins[id] = rock.transform.position; specialRockTravel[id] = 0; }
                specialRockTravel[id] = Mathf.Max(specialRockTravel[id],Vector3.Distance(specialRockOrigins[id],rock.transform.position));
            }
            if(GameController.i && GameController.i.MainCharacter) {
                var grass = GameController.i.MainCharacter.GetComponent<GrassSpecialController>();
                if(grass && grass.wings) maxWingsScale = Mathf.Max(maxWingsScale,grass.wings.transform.localScale.magnitude);
                if(grass && StatsController.i && StatsController.i.healFxP1) sawGrassHealing |= StatsController.i.healFxP1.isPlaying;
            }
        }
        if (Time.realtimeSinceStartupAsDouble >= nextDiagnostic && SceneManager.GetActiveScene().name == "Tournament") {
            nextDiagnostic = Time.realtimeSinceStartupAsDouble + 10;
            var tournament = TournamentManager.i;
            var controller = TournamentController.i;
            Observe("tournament-state", "timeScale=" + Time.timeScale + "; manager=" + (tournament ? tournament.name : "null") +
                "; round=" + (tournament ? tournament.round : -1) + "; persisted round=" + (controller ? controller.round : -1) +
                "; battle=" + (controller ? controller.battle : -1) + "; lost=" + (controller && controller.loseTournament) +
                "; players=" + (tournament ? string.Join(";", tournament.players.Select(p => p.player + ":cp=" + p.checkpoint + ":lost=" + p.loser + ":op=" + (p.opponent ? p.opponent.name : "null"))) : "missing"));
        }
        if (GameController.i && GameController.i.gameStarted) {
            var controlled = GameController.i.MainCharacter;
            var opponent = GameController.i.OpponentCharacter;
            // Survival deliberately removes the CPU when the controlled fighter
            // loses. Observing attacks must not dereference that expired object.
            if (controlled == null || opponent == null) return;
            foreach (var attack in UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude)) {
                if (!seen.Add(attack.GetEntityId())) continue;
                if (attack.Type == "Char" || attack.Damage <= 0f) continue;
                if(observeMoves && attack.gameObject.layer == controlled.layer) moveSpawnNames.Add(attack.name);
                if (attack.gameObject.layer == controlled.layer) playerProjectiles++;
                if (attack.gameObject.layer == opponent.layer) opponentProjectiles++;
            }
        }
    }
    void TrackMove(MonoBehaviour moving) {
        if(!moving.isActiveAndEnabled) return;
        if(!GameController.i) return;
        bool own=false;
        for(var parent=moving.transform; parent; parent=parent.parent) {
            if(parent.gameObject.layer==GameController.i.OpponentCharacter.layer) return;
            if(parent.gameObject.layer==GameController.i.MainCharacter.layer) { own=true; break; }
        }
        if(!own) return;
        var id = moving.GetEntityId();
        if(!moveOrigins.ContainsKey(id)) { moveOrigins[id] = moving.transform.position; moveTravel[id] = 0; moveLabels[id] = moving.GetType().Name+":"+moving.name; }
        moveTravel[id] = Mathf.Max(moveTravel[id], Vector3.Distance(moveOrigins[id],moving.transform.position));
    }
    void ValidateReportedPrefabFixes(string fighter) {
        if(fighter == "Sora") {
            var sora = Resources.Load<GameObject>("photonprefabs/Sora");
            var extraShield = sora ? sora.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sphere (2)") : null;
            if(!extraShield || extraShield.gameObject.activeSelf) { Fail("Sora shield prefab still enables the duplicate Sphere (2)"); return; }
            var twister = Resources.Load<GameObject>("photonprefabs/AirAtkTwister");
            var lifetime = twister ? twister.GetComponentsInChildren<DestroyNetObjByTime>(true).FirstOrDefault() : null;
            if(!lifetime || Mathf.Abs(lifetime.time - 2.7f) > .01f) { Fail("Sora AirAtkTwister lifetime differs from its authored 2.7 second effect"); return; }
            foreach(string name in new[]{"AirAtkL2", "AirAtkR2"}) {
                var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
                var move = prefab ? prefab.GetComponentInChildren<MoveAttack>(true) : null;
                if(!move || !move.aimAtTarget) { Fail("Sora " + name + " does not aim its side attack at the opponent"); return; }
            }
            Observe("reported-prefab-check", "Sora duplicate shield disabled; AirAtkTwister restored to authored lifetime=" + lifetime.time + "s; ChargeAir sphere runtime contraction is checked after the actual move; AirAtkL2/R2 aim at opponent");
        }
        if(fighter == "Vinea") {
            foreach(string name in new[]{"WaterAtkL", "WaterAtkR"}) {
                var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
                var move = prefab ? prefab.GetComponentInChildren<MoveAttack>(true) : null;
                if(!move || !move.aimAtTarget) { Fail("Vinea " + name + " does not aim at the opponent"); return; }
            }
            foreach(string name in new[]{"IceAtk", "IceAtkS5"}) {
                var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
                var crystal = prefab ? prefab.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "IceRock") : null;
                var expected = name == "IceAtk" ? new Vector3(1f, .08333334f, .33333334f) : new Vector3(1f, .16666667f, .33333334f);
                if(!crystal || Vector3.Distance(crystal.localScale, expected) > .001f) { Fail("Vinea " + name + " crystal scale differs from the reduced dimensions"); return; }
            }
            Observe("reported-prefab-check", "Vinea left/right water attacks aim at opponent; both ice crystal prefabs use reduced scale");
        }
        if(fighter == "Amon") {
            var prefab = Resources.Load<GameObject>("photonprefabs/FireDownAtk2");
            var move = prefab ? prefab.GetComponentInChildren<MoveAttack>(true) : null;
            if(!move || !move.aimAtTarget) { Fail("Blaze FireDownAtk2 disk does not aim at opponent"); return; }
            Observe("reported-prefab-check", "Blaze FireDownAtk2 aims at opponent");
        }
        if(fighter == "Diana") {
            foreach(string name in new[]{"RockL2", "RockR2"}) {
                var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
                var move = prefab ? prefab.GetComponentInChildren<MoveAttack>(true) : null;
                if(!move || !move.aimAtTarget) { Fail("Diana " + name + " does not aim at the opponent"); return; }
            }
            foreach(string name in new[]{"RockL", "RockR"}) {
                var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
                if(!prefab || !prefab.GetComponent<MoveRock>()) { Fail("Diana " + name + " basic side rock lacks MoveRock"); return; }
            }
            var fixture = new GameObject("DianaMoveRockTargetProbe");
            try {
                fixture.layer = BattleSceneSingleton.i.MainCharacter.layer;
                fixture.transform.position = new Vector3(0,4.5f,0);
                var rock = fixture.AddComponent<MoveRock>();
                typeof(MoveRock).GetMethod("ChangeDirection",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(rock,null);
                var intended = (BattleSceneSingleton.i.Opponent.transform.position-fixture.transform.position).normalized;
                intended.y = 0f;
                var actual = (Vector3)typeof(MoveRock).GetField("dir",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(rock);
                if(Vector3.Distance(actual,intended) > .001f) { Fail("Diana basic side rock aimed away from the opponent"); return; }
            } finally { UnityEngine.Object.Destroy(fixture); }
            Observe("reported-prefab-check", "Diana side RockL2/RockR2 aim at opponent; RockL/R MoveRock resolves the opposing fighter by layer");
        }
    }
    void Observe(string step, string detail) {
        report.observations.Add(new Observation { step = step, detail = detail, scene = SceneManager.GetActiveScene().name, seconds = Time.realtimeSinceStartupAsDouble - started });
        Debug.Log("OFFLINE_PLAYER_PROBE " + step + ": " + detail);
    }
    IEnumerator SceneReady(string name) {
        while (SceneManager.GetActiveScene().name != name || LoadingScreen.i == null || LoadingScreen.i.loading) yield return null;
        var fade = LevelChanger.i != null ? LevelChanger.i.animator : null;
        var black = fade != null ? fade.GetComponentsInChildren<Image>(true).FirstOrDefault(i => i.name == "BlackFade") : null;
        double fadeDeadline = Time.realtimeSinceStartupAsDouble + 3;
        while (black != null && black.enabled && black.color.a > .01f && Time.realtimeSinceStartupAsDouble < fadeDeadline) yield return null;
        yield return null;
        Observe("scene", name);
        if (black != null) Observe("capture-fade", "enabled=" + black.enabled + "; alpha=" + black.color.a);
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, "." + name.Replace(' ', '-') + ".png"));
        if (name == "Bending Battle")
        {
            var hud = new System.Text.StringBuilder();
            foreach (var graphic in UnityEngine.Object.FindObjectsByType<Graphic>(FindObjectsInactive.Exclude))
            {
                if (!graphic.enabled || graphic.color.a < .01f) continue;
                var corners = new Vector3[4]; graphic.rectTransform.GetWorldCorners(corners);
                var canvas = graphic.canvas;
                for (int c = 0; c < corners.Length; c++) corners[c] = RectTransformUtility.WorldToScreenPoint(canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null, corners[c]);
                string hierarchy = graphic.name;
                for (var parent = graphic.transform.parent; parent; parent = parent.parent) hierarchy = parent.name + "/" + hierarchy;
                var img = graphic as Image;
                var tmp = graphic as TMPro.TMP_Text;
                var txt = graphic as Text;
                hud.AppendLine(hierarchy + " | corners=" + string.Join(";", corners.Select(v => v.ToString())) + " | type=" + graphic.GetType().Name + " | content=" + (tmp ? tmp.text : txt ? txt.text : img && img.sprite ? img.sprite.name : "none") + " | color=" + graphic.color);
            }
            File.WriteAllText(Path.ChangeExtension(output, ".hud.txt"), hud.ToString());
        }
        // Finish capture before the next callback starts FadeOut.
        yield return new WaitForEndOfFrame();
    }
    void Click(string method, bool trigger, string path = null) {
        if (!trigger) {
            var candidates = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Exclude)
                .Where(b => b.IsInteractable() && Enumerable.Range(0, b.onClick.GetPersistentEventCount()).Any(i => b.onClick.GetPersistentMethodName(i) == method)).ToArray();
            if (candidates.Length != 1) { Fail("Button callback " + method + " count=" + candidates.Length); return; }
            candidates[0].onClick.Invoke();
        } else {
            var candidates = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude)
                .Where(t => (path == null || ObjectPath(t.transform) == path) && t.triggers.Any(e => e.eventID == EventTriggerType.PointerClick && Enumerable.Range(0, e.callback.GetPersistentEventCount()).Any(i => e.callback.GetPersistentMethodName(i) == method))).ToArray();
            if (candidates.Length != 1 || EventSystem.current == null) { Fail("Pointer callback " + method + " count=" + candidates.Length); return; }
            ExecuteEvents.Execute(candidates[0].gameObject, new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left }, ExecuteEvents.pointerClickHandler);
        }
        Observe("click", method);
    }
    EventTrigger ActiveAttackZone() {
        var zones=UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude)
            .Where(t=>t.triggers.Any(e=>e.eventID==EventTriggerType.PointerEnter && Enumerable.Range(0,e.callback.GetPersistentEventCount()).Any(i=>e.callback.GetPersistentMethodName(i)=="SetAButton"))).ToArray();
        if(zones.Length!=1 || !EventSystem.current) { Fail("Expected one active SetAButton pointer zone, got "+zones.Length); return null; }
        return zones[0];
    }
    IEnumerator Run() {
        Observe("boot", "Development Windows player, original boot scene");
        yield return SceneReady("StartMenu");
        if(Environment.GetCommandLineArgs().Contains("-rea-graphics-probe")) {CaptureFixture();PlayerPrefs.SetInt("userQualityGraphics",1);}
        if (Environment.GetCommandLineArgs().Any(a => a == "-rea-specials-probe" || a == "-rea-defense-probe" || a == "-rea-prison-probe" || a == "-rea-moves-probe" || a == "-rea-earth-prison-probe" || a == "-rea-dragonwood-probe" || a == "-rea-maya-production-probe" || a == "-rea-desktop-controls-probe" || a == "-rea-vinea-left-evade-probe" || a == "-rea-ability-aim-probe" || a == "-rea-reported-failures-probe" || a == "-rea-shadow-roster-probe")) {
            yield return CombatTrip(Environment.GetCommandLineArgs().Contains("-rea-defense-probe"), Environment.GetCommandLineArgs().Contains("-rea-prison-probe"));
            if (!done) Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
            yield break;
        }
        if (Environment.GetCommandLineArgs().Contains("-rea-menu-probe")) {
            yield return MenuTrip();
            if (!done) Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
            yield break;
        }
        if (Environment.GetCommandLineArgs().Contains("-rea-roster-probe")) {
            yield return RosterTrip();
            if (!done) Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
            yield break;
        }
        if (Environment.GetCommandLineArgs().Contains("-rea-tournament-probe")) {
            yield return TournamentTrip();
            if (!done) Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
            yield break;
        }
        bool survival = Environment.GetCommandLineArgs().Contains("-rea-survival-probe");
        Click(survival ? "SurvivalMode" : "SinglePlayerMode", false);
        yield return SceneReady("HowToPlay");
        Click("Continue", true);
        yield return SceneReady("Character Selection");
        Observe("selection", PlayerPrefs.GetString("Character") + " vs " + PlayerPrefs.GetString("CharacterCPU") + "; CpuMode=" + PlayerPrefs.GetInt("CpuMode", 0));
        Click("start", true);
        yield return SceneReady("Bending Battle");
        while (GameController.i == null || !GameController.i.gameStarted) yield return null;
        var game = GameController.i;
        if (game.MainCharacter == null || game.OpponentCharacter == null) { Fail("Two fighters were not spawned"); yield break; }
        Observe("arena-ready", game.MainCharacter.name + " vs " + game.OpponentCharacter.name);
        CaptureRenderState();
        if (GameEvents.i != null) GameEvents.i.OnPlayerDamaged += CountHit;

        // Exercise the reconstructed input owner rather than moving a transform.
        var reader = BattleSceneSingleton.i.inputReader;
        reader.dPad = true;
        reader.keyboard = false;
        var original = game.MainCharacter.transform.position;
        reader.SetXpos(1f);
        yield return new WaitForSeconds(.35f);
        reader.SetXpos(0f);
        float moved = Vector3.Distance(original, game.MainCharacter.transform.position);
        Observe("movement", "distance=" + moved + "; MobileInputs.enabled=" + game.MainCharacter.GetComponent<MobileInputs>().enabled);
        if (moved < .2f) { Fail("Directional input did not move the controlled fighter"); yield break; }

        // Independent assertion of the explicitly corrected camera transition.
        var camera = BattleSceneSingleton.i.cameraManager;
        camera.ChangeCameraParent(game.MainCharacter);
        camera.ChangeCameraToClose();
        yield return new WaitForSeconds(camera.transitionDuration + .2f);
        float cameraDistance = Vector3.Distance(camera.transform.localPosition, new Vector3(0f, 3f, -4f));
        float cameraAngle = Quaternion.Angle(camera.transform.localRotation, Quaternion.Euler(15f, 0f, 0f));
        Observe("camera", "position error=" + cameraDistance + "; angle error=" + cameraAngle);
        if (cameraDistance > .01f || cameraAngle > .1f) { Fail("Camera transition did not reach the target pose"); yield break; }

        var button = UnityEngine.Object.FindObjectsByType<ButtonAttack>(FindObjectsInactive.Exclude).OrderBy(b => b.attackType).FirstOrDefault();
        if (button == null) { Fail("No active attack control"); yield break; }
        Observe("attack-control", button.name + "; type=" + button.attackType + "; letter=" + button.letter);
        button.OnButtonPress();
        yield return new WaitForSeconds(12f);
        var ai = game.OpponentCharacter.GetComponent<AI>();
        Observe("combat-observation", "player projectiles=" + playerProjectiles + "; CPU projectiles=" + opponentProjectiles + "; hit events=" + hitEvents + "; AI enabled=" + ai.enabled + "; on=" + ai.on);
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".combat.png"));
        yield return new WaitForEndOfFrame();
        if (playerProjectiles == 0) { Fail("Attack control produced no observed player projectile"); yield break; }
        if (!ai.enabled || !ai.on) { Fail("CPU was not active after countdown"); yield break; }
        if (Environment.GetCommandLineArgs().Contains("-rea-impact-probe")) {
            yield return Impact();
            if (done) yield break;
        }
        if (Environment.GetCommandLineArgs().Contains("-rea-round-probe")) {
            yield return RoundTrip();
            if (done) yield break;
        }
        if (survival) {
            yield return SurvivalTrip();
            if (done) yield break;
        }
        if (Environment.GetCommandLineArgs().Contains("-rea-graphics-probe")) yield return ProjectorComparison();
        Finish(report.errors.Count == 0 ? "passed" : "runtime_errors");
    }
    IEnumerator ProjectorComparison() {
        yield return OfflineShadowQualityProbe.Run(GameController.i.MainCharacter.name,Observe,Fail,true);
        if(done)yield break;
        yield return OfflineDynamicShadowProbe.Run(output,Observe,Fail);
        if(done)yield break;
        // Diagnostic only: change live component state, retain both captures,
        // and restore exact original enable flags. No asset or product override.
        var projectors = UnityEngine.Object.FindObjectsByType<Projector>(FindObjectsInactive.Include);
        var enabled = projectors.Select(p => p.enabled).ToArray();
        float priorTimeScale = Time.timeScale;
        try {
            Time.timeScale = 0;
            foreach (var p in projectors)
                Observe("projector", ObjectPath(p.transform) + "; enabled=" + p.enabled + "; shader=" + (p.material && p.material.shader ? p.material.shader.name : "null"));
            ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".projectors-original.png"));
            yield return new WaitForEndOfFrame();
            foreach (var p in projectors) p.enabled = false;
            yield return null;
            ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".projectors-disabled.png"));
            yield return new WaitForEndOfFrame();
            if (projectors.Length > 0 && projectors.All(p => p.material && p.material.HasProperty("_ShadowTex"))) {
                var originals = projectors.Select(p => p.material).ToArray();
                var instances = originals.Select(m => new Material(m)).ToArray();
                var cookie = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                try {
                    cookie.wrapMode = TextureWrapMode.Clamp;
                    cookie.SetPixels(new[] {Color.white, Color.white, Color.white, Color.white}); cookie.Apply();
                    for(int i=0; i<projectors.Length; i++) { instances[i].SetTexture("_ShadowTex", cookie); projectors[i].material = instances[i]; projectors[i].enabled = enabled[i]; }
                    yield return null;
                    ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".projectors-white-cookie.png"));
                    yield return new WaitForEndOfFrame();
                    cookie.SetPixels(new[] {Color.black, Color.black, Color.black, Color.black}); cookie.Apply();
                    yield return null;
                    ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".projectors-black-cookie.png"));
                    yield return new WaitForEndOfFrame();
                    Observe("projector-cookie", "Captured controlled white/black cookies on temporary per-projector material instances; original materials restored. Images require inspection for neutrality, darkening and clipping.");
                } finally {
                    for(int i=0; i<projectors.Length; i++) { if(projectors[i]) projectors[i].material = originals[i]; UnityEngine.Object.Destroy(instances[i]); }
                    UnityEngine.Object.Destroy(cookie);
                }
            }
        } finally {
            for (int i=0; i<projectors.Length; i++) if(projectors[i]) projectors[i].enabled = enabled[i];
            Time.timeScale = priorTimeScale;
        }
        Observe("graphics-comparison", "Captured same frozen scene with original projectors and all projectors disabled; original enable flags restored. Images require inspection; no automatic visual-equivalence assertion.");
        report.limitation = "Bounded combat smoke plus controlled projector A/B captures. Diagnostic component states restored; captures alone do not establish original game rendering fidelity.";
    }
    void CaptureFixture() {
        savedPlayer = JsonUtility.FromJson<TMPlayer>(JsonUtility.ToJson(SystemManager.i.tmPlayer));
        foreach (string key in new[] { "Character", "CharacterCPU", "Joystick" }) {
            if (PlayerPrefs.HasKey(key)) savedStringPrefs.Add(key, PlayerPrefs.GetString(key)); else absentPrefs.Add(key);
        }
        foreach (string key in new[] { "Stage", "TourneyTimes", "Timer", "TimeOption", "GameSpeed", "CpuMode", "RateGame", "ControllerInverted", "AlternateKeyboardControls", "userQualityGraphics", "userFrameCap" }) {
            if (PlayerPrefs.HasKey(key)) savedIntPrefs.Add(key, PlayerPrefs.GetInt(key)); else absentPrefs.Add(key);
        }
        foreach (string suffix in new[] { "", ".bak", ".tmp" }) {
            string file = Path.Combine(Application.persistentDataPath, "reconstruction-player.json" + suffix);
            savedFiles.Add(file, File.Exists(file) ? File.ReadAllBytes(file) : null);
        }
    }
    IEnumerator MenuTrip() {
        CaptureFixture();
        var data = SystemManager.i.tmPlayer;
        data.tmPremium = 0;
        data.stages.stage1 = 0;
        data.crystals = 299;
        StagesManager.i.SetPlayableStages();
        int translated = typeof(Translation).GetFields().Count(f => f.FieldType == typeof(string) && f.GetValue(TransladeController.i.translation) != null);
        Observe("translation", "Assigned native translation fields=" + translated);
        if (translated != 129) { Fail("Native translation assignment coverage differs"); yield break; }
        Click("SinglePlayerMode", false);
        yield return SceneReady("HowToPlay");
        Click("Continue", true);
        yield return SceneReady("Character Selection");
        Click("OpenStageSelector", true, "Canvas/LeftBar/StageSelector/StageButton");
        yield return new WaitForSeconds(.6f);
        Click("nextStage", true);
        if (StagesManager.i.actualStage != 1 || StagesManager.i.IsStageUnlocked(1)) { Fail("Locked stage1 fixture not selected"); yield break; }
        Click("Unlock", true);
        if (data.crystals != 299 || data.stages.stage1 != 0 || StagesManager.i.IsStageUnlocked(1)) { Fail("Insufficient stage currency changed wallet or ownership"); yield break; }
        Observe("stage-rejected", "299 crystals retained; stage1 remains locked");
        data.crystals = 300;
        Click("Unlock", true);
        var persisted = SystemManager.i.LoadPlayerData();
        if (data.crystals != 0 || data.stages.stage1 != 1 || !StagesManager.i.IsStageUnlocked(1) || persisted.crystals != 0 || persisted.stages.stage1 != 1) { Fail("Stage purchase did not debit exactly300 and persist ownership"); yield break; }
        Observe("stage-purchase", "Exactly300 crystals debited; stage1 owned and JSON save read back");
        Click("SelectStage", true);
        Click("start", true);
        yield return SceneReady("Bending Battle");
        while (GameController.i == null || !GameController.i.gameStarted) yield return null;
        var menuCpu=GameController.i.OpponentCharacter.GetComponent<AI>();
        menuCpu.on=false;menuCpu.enabled=false;menuCpu.CancelInvoke();menuCpu.StopAllCoroutines();
        var settings = UnityEngine.Object.FindFirstObjectByType<SettingsOptions>();
        if (settings == null) { Fail("Battle settings owner missing"); yield break; }
        Click("OpenSettings", false);
        yield return new WaitForSeconds(.6f);
        var settingsRect = settings.anim.transform as RectTransform;
        if(settingsRect == null) { Fail("Settings animation has no RectTransform"); yield break; }
        double slideDeadline = Time.realtimeSinceStartupAsDouble+3;
        while(Mathf.Abs(settingsRect.anchoredPosition.x-(-160.4f))>1f && Time.realtimeSinceStartupAsDouble<slideDeadline) yield return null;
        Observe("settings-open-pose", "x=" + settingsRect.anchoredPosition.x + "; normalizedTime=" + settings.anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if(Mathf.Abs(settingsRect.anchoredPosition.x-(-160.4f))>1f) { Fail("Settings did not reach authored open pose"); yield break; }
        if (!settings.closeBG.activeSelf || !settings.anim.GetCurrentAnimatorStateInfo(0).IsName("SettingsSlideIn")) { Fail("Settings did not show its background and native open animation state"); yield break; }
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".settings-open.png"));
        yield return new WaitForEndOfFrame();
        yield return ControlOptions(GameController.i);
        if(done) yield break;
        Click("CloseSettings", true);
        yield return new WaitForSeconds(.6f);
        slideDeadline = Time.realtimeSinceStartupAsDouble+3;
        while(Mathf.Abs(settingsRect.anchoredPosition.x-(-723f))>1f && Time.realtimeSinceStartupAsDouble<slideDeadline) yield return null;
        Observe("settings-close-pose", "x=" + settingsRect.anchoredPosition.x + "; normalizedTime=" + settings.anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if(Mathf.Abs(settingsRect.anchoredPosition.x-(-723f))>1f) { Fail("Settings did not reach authored closed pose"); yield break; }
        if (settings.closeBG.activeSelf || !settings.anim.GetCurrentAnimatorStateInfo(0).IsName("SettingsSlideOut")) { Fail("Settings did not hide its background and enter native close animation state"); yield break; }
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".settings-closed.png"));
        yield return new WaitForEndOfFrame();
        Observe("settings", "Recovered Button.onClick played SettingsSlideIn; EventTrigger PointerClick played SettingsSlideOut; background toggled");
        report.limitation = "Native translation count,299/300 stage purchase, JSON persistence, settings poses and touch control option callbacks/drag/Dpad tested. CPU disabled for control fixture; temporary data/preferences restored. No physical gamepad, every joystick mode, every settings option or full visual fidelity acceptance.";
    }
    IEnumerator ControlOptions(GameController game) {
        var settings=UnityEngine.Object.FindFirstObjectByType<JoystickSettings>();
        var reader=BattleSceneSingleton.i.inputReader;
        if(!settings || !reader) { Fail("Control settings/reader owner missing"); yield break; }
        reader.keyboard=false;
        Click("DpadOn",true);
        yield return null;
        bool dpad=PlayerPrefs.GetString("Joystick")=="Dpad" && settings.dpadToggle.isOn && !settings.joystickToggle.isOn && !settings.controllerToggle.isOn && settings.dpad.activeInHierarchy && !settings.joystick.activeSelf && reader.dPad && settings.buttonA.activeSelf && settings.buttonB.activeSelf;
        if(!dpad) { Fail("Dpad callback did not set original mode/UI/input contract"); yield break; }
        var right=settings.dpad.GetComponentsInChildren<EventTrigger>().SingleOrDefault(t=>t.name=="Right");
        if(!right) { Fail("Active Dpad Right zone missing"); yield break; }
        double deadline=Time.realtimeSinceStartupAsDouble+8;
        while(game.animMainChar.GetBool("Stunned") && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
        if(game.animMainChar.GetBool("Stunned")) { Fail("Control fixture did not recover before Dpad movement"); yield break; }
        Vector3 before=game.MainCharacter.transform.position;
        ExecuteEvents.Execute(right.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerEnterHandler);
        yield return new WaitForSeconds(.25f);
        float dpadInput=reader.inputSet.x;
        ExecuteEvents.Execute(right.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler);
        float dpadDistance=Vector3.Distance(before,game.MainCharacter.transform.position);
        Observe("control-dpad","native mode/UI="+dpad+"; right input="+dpadInput+"; released="+reader.inputSet.x+"; fighter travel="+dpadDistance);
        if(dpadInput<.9f || reader.inputSet.x!=0 || dpadDistance<.2f) { Fail("Actual Dpad pointer route did not move/release fighter"); yield break; }
        bool prior=settings.invertedToggle.isOn;
        ExecuteEvents.Execute(settings.invertedToggle.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerClickHandler);
        if(settings.inverted==prior || PlayerPrefs.GetInt("ControllerInverted")!=(prior ? 0 : 1)) { Fail("Dynamic inversion toggle did not persist native value"); yield break; }
        Observe("control-inversion","toggle="+settings.invertedToggle.isOn+"; native pref="+PlayerPrefs.GetInt("ControllerInverted"));
        Click("ControllerOn",true);
        yield return null;
        bool controller=PlayerPrefs.GetString("Joystick")=="Controller" && settings.controllerToggle.isOn && !settings.dpad.activeSelf && !settings.joystick.activeSelf && settings.buttonA.activeSelf && settings.buttonB.activeSelf;
        if(!controller) { Fail("Controller callback UI/preferences differ from native contract"); yield break; }
        Observe("control-controller","UI/preferences accepted; A/B stay active; physical axis-to-locomotion bridge remains unverified");
        Click("JoystickOn",true);
        yield return null;
        var stick=reader.joystick;
        var background=stick ? stick.transform.Find("Background") as RectTransform : null;
        if(!stick || !background || reader.dPad || !settings.joystick.activeInHierarchy || settings.dpad.activeSelf || PlayerPrefs.GetString("Joystick")!="Joystick") { Fail("Joystick callback did not restore original active touch route"); yield break; }
        var canvas=stick.GetComponentInParent<Canvas>();
        Camera camera=canvas.renderMode==RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
        Vector2 center=RectTransformUtility.WorldToScreenPoint(camera,background.position);
        Vector2 radius=background.sizeDelta*.5f*canvas.scaleFactor;
        var pointer=new PointerEventData(EventSystem.current){position=center};
        ExecuteEvents.Execute(stick.gameObject,pointer,ExecuteEvents.pointerDownHandler);
        if(stick.Direction.magnitude>.01f) { Fail("Joystick center did not produce zero input"); yield break; }
        pointer.position=center+new Vector2(radius.x*.5f,0);
        ExecuteEvents.Execute(stick.gameObject,pointer,ExecuteEvents.dragHandler);
        yield return null;
        Vector2 half=stick.Direction, forwarded=new Vector2(reader.inputSet.x,reader.inputSet.y);
        pointer.position=center+new Vector2(radius.x*2f,0);
        ExecuteEvents.Execute(stick.gameObject,pointer,ExecuteEvents.dragHandler);
        before=game.MainCharacter.transform.position;
        yield return new WaitForSeconds(.25f);
        Vector2 full=stick.Direction;
        float distance=Vector3.Distance(before,game.MainCharacter.transform.position);
        ExecuteEvents.Execute(stick.gameObject,pointer,ExecuteEvents.pointerUpHandler);
        yield return null;
        Observe("control-joystick","half="+half+"; reader="+forwarded+"; clamped="+full+"; fighter travel="+distance+"; released="+stick.Direction+"; reader released="+reader.inputSet.x);
        if(Vector2.Distance(half,new Vector2(.5f,0))>.03f || Vector2.Distance(forwarded,half)>.03f || Vector2.Distance(full,Vector2.right)>.03f || distance<.2f || stick.Direction.magnitude>.001f || Mathf.Abs(reader.inputSet.x)>.001f) { Fail("Real joystick pointer/drag/clamp/release/locomotion contract failed"); yield break; }
    }
    IEnumerator SampleClips(Animator animator, float seconds, HashSet<string> clips) {
        float finish = Time.time + seconds;
        while (Time.time < finish) {
            foreach (var clip in animator.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
            yield return null;
        }
    }
    void DefensePointer(bool enter) {
        var type = enter ? EventTriggerType.PointerEnter : EventTriggerType.PointerExit;
        var candidates = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude)
            .Where(t => t.triggers.Any(e => e.eventID == type && Enumerable.Range(0,e.callback.GetPersistentEventCount()).Any(i => e.callback.GetPersistentMethodName(i) == "Jumping"))).ToArray();
        if(candidates.Length != 1 || EventSystem.current == null) { Fail("Defense pointer binding count=" + candidates.Length); return; }
        var data = new PointerEventData(EventSystem.current);
        if(enter) ExecuteEvents.Execute(candidates[0].gameObject,data,ExecuteEvents.pointerEnterHandler);
        else ExecuteEvents.Execute(candidates[0].gameObject,data,ExecuteEvents.pointerExitHandler);
        Observe("defense-input", type + ": " + ObjectPath(candidates[0].transform));
    }
    IEnumerator DefenseHits(GameController game, string fighter) {
        var target = game.MainCharacter;
        var receiver = target.GetComponent<DamageOnCollision>();
        var anim = game.animMainChar;
        var collider = target.GetComponent<Collider>();
        var prefab = Resources.Load<GameObject>("PhotonPrefabs/AirAtk");
        if(!collider || !prefab || !receiver) { Fail("Defense fixture dependencies unavailable"); yield break; }
        foreach(int strength in new[] {1,3}) {
            DefensePointer(true);
            if(done) yield break;
            double deadline = Time.realtimeSinceStartupAsDouble+3;
            while(!anim.GetBool("Blocking") && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            if(!anim.GetBool("Blocking")) { Fail("Actual defense input did not enter Blocking for " + fighter); yield break; }
            float before = receiver.Damage;
            var projectile = Instantiate(prefab,collider.bounds.center,game.OpponentCharacter.transform.rotation);
            projectile.layer = game.OpponentCharacter.layer;
            // Explicit synthetic frontal/strength fixture on the recovered attack.
            // Reaction/physics are product code; no direct damage/animation calls.
            var attack = projectile.GetComponent<Description>();
            attack.effect = "Front"; attack.Damage = strength;
            deadline = Time.realtimeSinceStartupAsDouble+3;
            while(receiver.Damage == before && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            float delta = receiver.Damage-before;
            bool blocking = anim.GetBool("Blocking");
            Observe("defense-hit", fighter + "; synthetic strength=" + strength + "; effect=Front; delta=" + delta + "; Blocking=" + blocking + "; state hash=" + anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
            if(projectile) Destroy(projectile);
            DefensePointer(false);
            float expected = strength == 1 ? .5f : 1.5f;
            if(Mathf.Abs(delta-expected)>.001f || blocking != (strength == 1)) { Fail("Defense collision differs from native low/block-break rule for " + fighter); yield break; }
            yield return new WaitForSeconds(2f);
        }
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".defense-"+fighter+".png"));
        yield return new WaitForEndOfFrame();
    }
    IEnumerator PrisonAttack(GameController game) {
        bool sawCrush = false, sawCorrectCrushTarget = false;
        var charOwner = game.MainCharacter.GetComponent<BranchPrisonCharController>();
        string crushName = charOwner.crushAttack.name+"(Clone)";
        branchPrisonController prison = null;
        creationEventOwner = GameEvents.i;
        creationObserver = id => {
            var created = UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude)
                .FirstOrDefault(d => d.gameObject.GetEntityId().Equals(id));
            if(!created || created.name != crushName || created.gameObject.layer != game.MainCharacter.layer) return;
            sawCrush = true;
            var destroy = created.GetComponent<DestroyWhenCreated>();
            sawCorrectCrushTarget = prison && destroy && destroy.target == prison.gameObject;
            Observe("prison-crush-created", created.name + "; layer=" + created.gameObject.layer + "; correct cage target=" + sawCorrectCrushTarget);
        };
        creationEventOwner.onAttackLayerSetUp += creationObserver;
        var reader = BattleSceneSingleton.i.inputReader;
        reader.SetYpos(-1f);
        yield return new WaitForSeconds(.1f);
        var attackZone=ActiveAttackZone();
        if(!attackZone) yield break;
        ExecuteEvents.Execute(attackZone.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerEnterHandler);
        yield return new WaitForSeconds(.15f);
        ExecuteEvents.Execute(attackZone.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler);
        reader.SetYpos(0f);
        Observe("prison-input", "Maya DownAttack0 via InputReader and actual SetAButton pointer zone; native variant preserved");
        double deadline = Time.realtimeSinceStartupAsDouble+4;
        while(prison == null && Time.realtimeSinceStartupAsDouble<deadline) {
            prison = UnityEngine.Object.FindObjectsByType<branchPrisonController>(FindObjectsInactive.Exclude).FirstOrDefault(p => p.gameObject.layer == game.MainCharacter.layer);
            yield return null;
        }
        if(prison == null) { Fail("Maya down attack did not create branch prison"); yield break; }
        var attackCollider = prison.GetComponent<Collider>();
        Observe("prison-created", prison.name + "; collider enabled=" + attackCollider.enabled + "; owner bound=" + (charOwner.actualPrisonAtk == prison) + "; owner Weak=" + game.animMainChar.GetBool("Weak") + "; Stunned=" + game.animMainChar.GetBool("Stunned"));
        while(prison && !attackCollider.enabled && !prison.enemyTrapped && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
        if(!prison || (!attackCollider.enabled && !prison.enemyTrapped)) { Fail("Branch prison showed neither an active collider nor a completed real trap"); yield break; }
        // Native opponent placement can complete trigger entry and disable the
        // collider within the same physics tick that enables it.
        var opponent = game.OpponentCharacter;
        if(!prison.enemyTrapped) {
            opponent.transform.position = attackCollider.bounds.center;
            var body = opponent.GetComponent<Rigidbody>();
            if(body) { body.position = attackCollider.bounds.center; body.linearVelocity = Vector3.zero; }
            Physics.SyncTransforms();
            Observe("prison-overlap", "Synthetic CPU placement inside enabled recovered attack collider; real physics callbacks required");
        } else Observe("prison-overlap", "Native opponent placement already produced enemyTrapped; collider enable/disable window was not directly sampled; no synthetic CPU repositioning");
        deadline = Time.realtimeSinceStartupAsDouble+3;
        while(prison && !prison.enemyTrapped && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
        if(!prison || !prison.enemyTrapped || prison.hitCharacter.hitCharacter != opponent) { Fail("Prison trigger did not retain the CPU and trap state"); yield break; }
        Observe("prison-trapped", "enemyTrapped=true; RefHitCharacter points to CPU; collider enabled=" + attackCollider.enabled + "; owner Weak=" + game.animMainChar.GetBool("Weak") + "; Stunned=" + game.animMainChar.GetBool("Stunned"));
        bool sawExplosionAnimation = false;
        deadline = Time.realtimeSinceStartupAsDouble+5;
        string lastClip = null;
        while(Time.realtimeSinceStartupAsDouble<deadline) {
            sawExplosionAnimation |= game.animMainChar.GetCurrentAnimatorStateInfo(0).IsName("PrisonExplosion");
            string clip = string.Join(",",game.animMainChar.GetCurrentAnimatorClipInfo(0).Select(c=>c.clip ? c.clip.name : "null"));
            if(clip != lastClip) { Observe("prison-animation",clip); lastClip = clip; }
            if(sawCrush && !prison) break;
            yield return null;
        }
        Observe("prison-crush", "PrisonExplosion state=" + sawExplosionAnimation + "; crush spawned=" + sawCrush + "; prison destroyed=" + !prison + "; retained owner cleared=" + (charOwner.actualPrisonAtk == null));
        if(!sawExplosionAnimation || !sawCrush || !sawCorrectCrushTarget || prison || charOwner.actualPrisonAtk != null) { Fail("Maya prison did not finish its animation/crush/cleanup chain"); yield break; }
        creationEventOwner.onAttackLayerSetUp -= creationObserver;
        creationObserver = null; creationEventOwner = null;
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".prison-complete.png"));
        yield return new WaitForEndOfFrame();
    }
    IEnumerator CombatTrip(bool defensesOnly, bool prisonOnly) {
        var args = Environment.GetCommandLineArgs();
        bool earthPrison=args.Contains("-rea-earth-prison-probe");
        bool dragonwood=args.Contains("-rea-dragonwood-probe");
        bool mayaProduction=args.Contains("-rea-maya-production-probe");
        bool abilityAim=args.Contains("-rea-ability-aim-probe");
        bool reportedFailures=args.Contains("-rea-reported-failures-probe");
        bool flightMatrix=args.Contains("-rea-flight-matrix-probe"),awakeJumps=args.Contains("-rea-awake-jumps-probe");int awakeIndex=0;
        bool shadowRoster=args.Contains("-rea-shadow-roster-probe");
        bool desktopControls=args.Contains("-rea-desktop-controls-probe"); int desktopCase=0;
        bool vineaLeftEvade=args.Contains("-rea-vinea-left-evade-probe");
        report.limitation = prisonOnly
            ? "Attempts Maya down-attack prison, synthetic CPU positioning and real trap/crush callbacks; observations and result delimit completion. No natural aiming claim."
            : defensesOnly
            ? "Attempts real B pointer defense with recovered attack overlaps and explicit Front/strength fixtures; observations and result delimit completion. No natural aiming or shield-meter claim."
            : "Attempts selected fighters' accelerated charge, actual Special UI, natural awakening completion and phase1 special; observations and result delimit completion. No full moveset, natural charging or aiming claim.";
        CaptureFixture();
        if(shadowRoster)PlayerPrefs.SetInt("userQualityGraphics",1);
        foreach(var field in typeof(TMPlayerCharacters).GetFields())
            if(field.FieldType == typeof(int)) field.SetValue(SystemManager.i.tmPlayer.characters, 1);
        CharactersManager.i.SetPlayableCharacters();
        Observe("combat-fixture", "Temporary fighter ownership, CPU disabled after countdown. Synthetic charge or frontal attack strength where reported; actual UI/physics/animator paths. Save/preferences restored.");
        Click("SinglePlayerMode", false);
        yield return SceneReady("HowToPlay");
        Click("Continue", true);
        string[] roster = { "Sora", "Vinea", "Diana", "Amon", "Aura", "Eliot", "Ren", "Rina", "Dante", "Agata", "Raven", "Maya" };
        if(flightMatrix)roster=new[]{"Sora"};
        if(awakeJumps)roster=OfflineFlightMatrixProbe.Load().jumps.Select(j=>j.fighter).ToArray();
        if(prisonOnly) roster = new[] {"Maya"};
        if(earthPrison) roster = new[] {"Diana", "Diana"};
        if(desktopControls) roster = new[] {"Sora", "Sora"};
        if(vineaLeftEvade) roster = new[] {"Vinea"};
        if(mayaProduction) roster = new[] {"Maya"};
        if(reportedFailures) roster = new[] {"Sora", "Vinea", "Diana", "Amon"};
        if(dragonwood) roster = new[] {"Maya"};
        int earthCase=0;
        int only = Array.IndexOf(args, "-rea-fighter");
        if(only >= 0) {
            string[] allFighters = {"Sora", "Vinea", "Diana", "Amon", "Aura", "Eliot", "Ren", "Rina", "Dante", "Agata", "Raven", "Maya"};
            if(only+1 >= args.Length || (!roster.Contains(args[only+1]) && !(desktopControls && allFighters.Contains(args[only+1])))) { Fail("Unknown selected special fighter"); yield break; }
            roster = new[] {args[only+1]};
        }
        foreach(string fighter in roster) {
            yield return SceneReady("Character Selection");
            SelectionController.i.ChangeCharacter(fighter);
            SelectionController.i.cpuController.ChangeCharacter("Amon");
            Observe("special-select", fighter);
            Click("start", true);
            yield return SceneReady("Bending Battle");
            while(GameController.i == null || !GameController.i.gameStarted) yield return null;
            var game = GameController.i;
            var cpu = game.OpponentCharacter.GetComponent<AI>();
            cpu.on = false; cpu.enabled = false; cpu.CancelInvoke(); cpu.StopAllCoroutines();
            var reader = BattleSceneSingleton.i.inputReader;
            reader.dPad = true; reader.keyboard = false; reader.SetXpos(0); reader.SetYpos(0);
            var anim = game.animMainChar;
            if(vineaLeftEvade) {
                reader.SetXpos(-1f);
                yield return new WaitForSecondsRealtime(.25f);
                game.Jumping(true);
                yield return new WaitForSecondsRealtime(.3f);
                var clips = new HashSet<string>();
                double until = Time.realtimeSinceStartupAsDouble + 1.1;
                bool entered = false;
                while(Time.realtimeSinceStartupAsDouble < until) {
                    entered |= anim.GetCurrentAnimatorStateInfo(0).IsName("JumpL");
                    foreach(var clip in anim.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
                    if(entered) ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".vinea-left-evade.png"));
                    yield return null;
                }
                game.Jumping(false); reader.SetXpos(0);
                Observe("vinea-left-evade", "input left+B; JumpL state=" + entered + "; clips=" + string.Join(",",clips) + "; mirrored controller state used");
                if(!entered || clips.Count == 0) { Fail("Vinea left evade did not enter JumpL"); yield break; }
                game.Restart();
                continue;
            }
            if(flightMatrix) {
                report.limitation="Authored projectile profile fixtures, both sides, controlled moving/elevated targets, projectile edge and real physical synthetic owner interruption. Not natural normal-input routes or guaranteed hits for every trajectory.";
                yield return OfflineFlightMatrixProbe.Run(game,Path.ChangeExtension(output,".matrix.json"),Observe,Fail);
                if(done)yield break;game.Restart();continue;
            }
            if(awakeJumps) {
                report.limitation="Five authored awake-evade events through actual Special and evade UGUI, accelerated charge; controlled target. Raven tracking and Maya stationary trap semantics; detailed sidecars delimit impacts and observation scope.";
                yield return OfflineAwakeJumpProbe.Run(game,awakeIndex,Path.ChangeExtension(output,".jump-"+awakeIndex+".json"),()=>Click("Special",true),DefensePointer,Observe,Fail);
                awakeIndex++;if(done)yield break;game.Restart();continue;
            }
            if(shadowRoster) {
                report.limitation="Twelve roster GPU shadow checks, controlled BasicAttack poses and native preference/event quality matrix. CPU disabled; preferences/save restored. Not full gameplay, quality-button resolution switching, advanced shadow features or original visual parity.";
                yield return OfflineShadowQualityProbe.Run(fighter,Observe,Fail,fighter=="Sora");
                if(done)yield break;
                yield return OfflineDynamicShadowProbe.Run(Path.ChangeExtension(output,".shadows-"+fighter+".json"),Observe,Fail,anim);
                if(done)yield break;
                Observe("shadow-roster-accepted",fighter);
                game.Restart();continue;
            }
            if(desktopControls) {
                report.limitation="Actual OS keyboard/mouse input in focused Windows player, two layouts, CPU disabled and charge accelerated. Save/preferences restored. Bounded Sora movement/attack/evade/awakening/special coverage, not full roster or natural charging.";
#if UNITY_STANDALONE_WIN
                yield return OfflineDesktopControlsProbe.Run(game,desktopCase++==1,Path.ChangeExtension(output,".controls-"+desktopCase+".png"),Observe,Fail);
#else
                Fail("Desktop OS input probe is Windows-only");
#endif
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(earthPrison) {
                report.limitation="Diana real alternating down inputs; centered trap and synthetic miss repositioning in fresh scenes; actual wall closure/reversal, collision callbacks and expiry observed. CPU disabled, save/preferences restored. Not natural aiming, all release/weak branches, Ren or graphical parity.";
                yield return OfflineEarthPrisonProbe.Run(game,Observe,Fail,earthCase++==1);
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(mayaProduction) {
                report.limitation="Controlled animation-collider states/elements with animation events disabled, collision branch fixtures, recovered Maya producers and floor beam motion. CPU disabled, synthetic targets and temporarily overridden/restored layer pairs. FlowerBeam trigger enabled by fixture. Not natural Light attack input, flower activation, aiming or visual parity.";
                yield return OfflineMayaProductionProbe.Run(game,Observe,Fail);
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(dragonwood) {
                report.limitation="Maya real down input alternation creates DragonWood; root speed/travel, head path and follower movement observed. CPU disabled, save/preferences restored. Synthetic damage and any unaccepted collision/listener paths are stated in observations; no complete moveset or visual parity claim.";
                yield return OfflineDragonwoodProbe.Run(game,Observe,Fail);
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(args.Contains("-rea-moves-probe") || abilityAim) {
                if(abilityAim && !args.Contains("-rea-moving-target-probe")) { yield return OfflineAbilityAimProbe.Run(game,fighter,Observe,Fail,fighter=="Sora"); if(done)yield break; }
                yield return DirectionalMoves(game, fighter);
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(prisonOnly) {
                yield return PrisonAttack(game);
                if(done) yield break;
                game.Restart();
                continue;
            }
            if(defensesOnly) {
                yield return DefenseHits(game,fighter);
                if(done) yield break;
                game.Restart();
                continue;
            }
            var manager = game.MainCharacter.GetComponent<AnimatorStatesManager>();
            var special = SpecialAtk.specialAtk;
            maximumAimError=0;aimFrames=0;liveAimTargets.Clear();liveFollowTargets.Clear();observeAim=true;
            StartCoroutine(ObserveLiveAim(game));
            if(!game.SpecialButton.activeInHierarchy || special == null) { Fail("Special control is unavailable for " + fighter); yield break; }
            if(anim.GetInteger("NumSpecials") != 0) { Fail("Expected initial awakening phase for " + fighter); yield break; }
            special.ChargeSpecial(1f); special.ChargeSpecial(1f);
            if(!special.charged) { Fail("Native charge owner did not arm " + fighter); yield break; }
            Click("Special", true);
            var awakeningClips = new HashSet<string>();
            yield return SampleClips(anim, 4f, awakeningClips);
            Observe("awakening", fighter + "; clips=" + string.Join(",", awakeningClips) + "; awakening=" + manager.awakening + "; discharging=" + special.decharging + "; fill=" + special.chargeImg.fillAmount);
            if(!manager.awakening || !special.decharging) { Fail("Awakening callback/discharge did not start for " + fighter); yield break; }
            yield return AwakenedNormalAttack(game, fighter, roster);
            if(done) yield break;
            double deadline = Time.realtimeSinceStartupAsDouble + 45;
            while((special.decharging || anim.GetInteger("NumSpecials") != 1) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
            if(special.decharging || manager.awakening || anim.GetInteger("NumSpecials") != 1) { Fail("Natural awakening completion did not restore special phase for " + fighter); yield break; }
            Observe("awakening-complete", fighter + "; NumSpecials=" + anim.GetInteger("NumSpecials") + "; charged=" + special.charged);
            if(fighter == "Maya") game.MainCharacter.GetComponent<DamageOnCollision>().GetDamage(5.5f);
            special.ChargeSpecial(1f); special.ChargeSpecial(1f);
            if(anim.GetBool("Stunned") || !special.charged) { Fail("Special cannot enter after awakening for " + fighter); yield break; }
            int before = playerProjectiles;
            specialRockOrigins.Clear(); specialRockTravel.Clear(); sawGrassHealing = false; maxWingsScale = 0; observeSpecialEffects = true;
            maximumAimError=0;aimFrames=0;liveAimTargets.Clear();liveFollowTargets.Clear();observeAim=true;
            StartCoroutine(ObserveLiveAim(game));
            Click("Special", true);
            var specialClips = new HashSet<string>();
            yield return SampleClips(anim, 8f, specialClips);
            observeAim=false;
            Observe("special-aim",fighter+"; frames="+aimFrames+"; maximumDegrees="+maximumAimError+"; followEntities="+liveFollowTargets.Count);
            if(aimFrames==0 || maximumAimError>.15f){Fail(fighter+" lost opponent facing during awakening/super");yield break;}
            observeSpecialEffects = false;
            Observe("special-action", fighter + "; clips=" + string.Join(",", specialClips) + "; player attack entities=" + (playerProjectiles-before) + "; damage=" + game.MainCharacter.GetComponent<DamageOnCollision>().Damage + "; stunned=" + anim.GetBool("Stunned"));
            if(!specialClips.Any(c => c.IndexOf("Special", StringComparison.OrdinalIgnoreCase) >= 0)) { Fail("No special animation observed for " + fighter); yield break; }
            if(fighter == "Diana" || fighter == "Ren") {
                int moving = specialRockTravel.Count(pair => pair.Value > .5f);
                Observe("special-rocks", fighter + "; observed=" + specialRockOrigins.Count + "; moved beyond0.5=" + moving + "; max travel=" + (specialRockTravel.Count > 0 ? specialRockTravel.Values.Max() : 0));
                if(moving == 0) { Fail("Earth special produced no observed moving rock for " + fighter); yield break; }
            }
            if(fighter == "Maya") {
                int destroyed = StatsController.i.lifePlayer.GetComponentsInChildren<Image>().Count(image => image.CompareTag("Destroyed"));
                Observe("special-healing", "Maya; healing particle observed=" + sawGrassHealing + "; max wings scale magnitude=" + maxWingsScale + "; destroyed life crystals remaining=" + destroyed);
                if(game.MainCharacter.GetComponent<DamageOnCollision>().Damage != 0 || !sawGrassHealing || maxWingsScale < .5f || destroyed != 0) { Fail("Maya special did not restore damage/life presentation and grow wings"); yield break; }
            }
            ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".special-"+fighter+".png"));
            yield return new WaitForEndOfFrame();
            game.Restart();
        }
        if(abilityAim || flightMatrix || awakeJumps || args.Contains("-rea-moves-probe") || earthPrison || dragonwood || mayaProduction || desktopControls || shadowRoster) yield break;
        report.limitation = prisonOnly
            ? "Maya down attack entered through actual pointer input; native opponent placement and real physics trap, prison animation, crush creation and cleanup tested. Any synthetic CPU placement is explicitly reported; no claim of general natural aiming or every branch-prison outcome. Save/preferences restored."
            : defensesOnly
            ? "Selected fighters entered defense through actual B PointerEnter/Exit, received synthetic recovered AirAtk overlaps with effect Front and strengths1/3, and matched native block/break damage rules. CPU disabled; save/preferences restored. Not natural aiming, shield-meter reachability or all defense branches."
            : "All twelve fighters exercised through actual Special UI: accelerated charge fixture, one normal attack during active awakening, natural decharge/NumSpecials transition and one phase1 special. Awake and super-facing are observed in separate post-LateUpdate windows. CPU disabled; save/preferences restored. Not all awakened attack directions/alternations, natural charging, moving-target impacts or original visual fidelity.";
    }
    IEnumerator AwakenedNormalAttack(GameController game, string fighter, string[] roster) {
        var reader = BattleSceneSingleton.i.inputReader;
        var anim = game.animMainChar;
        var manager = game.MainCharacter.GetComponent<AnimatorStatesManager>();
        if(!manager || !manager.awakening || SpecialAtk.specialAtk == null || !SpecialAtk.specialAtk.decharging) {
            Fail(fighter + " left awakening before its normal-attack aiming check"); yield break;
        }
        var contractAsset = Resources.Load<TextAsset>("MoveInputContracts");
        if(!contractAsset) { Fail("Missing retained controller input contracts for awakened attack"); yield break; }
        var contracts = JsonUtility.FromJson<MoveInputContracts>(contractAsset.text);
        int direction = Array.IndexOf(roster, fighter) % 5;
        var axes = new[] {Vector2.zero, Vector2.up, Vector2.down, Vector2.right, Vector2.left};
        var names = new[] {"BasicAttack", "UpAttack", "DownAttack", "RightAttack", "LeftAttack"};
        int variant = anim.GetInteger(names[direction]);
        var route = contracts.routes.SingleOrDefault(r => r.fighter == fighter && r.direction == direction && r.variant == variant);
        if(route == null) { Fail("No authored awakened attack input route for " + fighter); yield break; }
        game.MainCharacter.transform.position = new Vector3(0, 0, 5);
        float bearing = ((Array.IndexOf(roster, fighter) * 47) % 280) - 140;
        game.OpponentCharacter.transform.position = game.MainCharacter.transform.position + Quaternion.Euler(0, bearing, 0) * Vector3.forward * 12;
        foreach(var body in new[]{game.MainCharacter.GetComponent<Rigidbody>(), game.OpponentCharacter.GetComponent<Rigidbody>()})
            if(body) { body.linearVelocity = Vector3.zero; body.angularVelocity = Vector3.zero; }
        reader.SetXpos(axes[direction].x); reader.SetYpos(axes[direction].y);
        yield return new WaitForSeconds(.06f);
        yield return new WaitForEndOfFrame();
        Vector3 toOpponent = game.OpponentCharacter.transform.position - game.MainCharacter.transform.position; toOpponent.y = 0;
        float entryError = Vector3.Angle(game.MainCharacter.transform.forward, toOpponent);
        if(entryError > .1f) { Fail(fighter + " did not face its opponent before awakened " + route.state); yield break; }
        yield return null;
        var attackZone = ActiveAttackZone();
        if(!attackZone) yield break;
        int expectedHash = Animator.StringToHash(route.state);
        var clips = new HashSet<string>();
        maximumAimError = 0; aimFrames = 0; liveAimTargets.Clear(); liveFollowTargets.Clear(); observeAim = true;
        ExecuteEvents.Execute(attackZone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        double stateDeadline = Time.realtimeSinceStartupAsDouble + 3;
        bool entered = false;
        while(Time.realtimeSinceStartupAsDouble < stateDeadline) {
            foreach(var clip in anim.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
            if(anim.GetCurrentAnimatorStateInfo(0).shortNameHash == expectedHash) { entered = true; break; }
            yield return null;
        }
        ExecuteEvents.Execute(attackZone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
        reader.SetXpos(0); reader.SetYpos(0);
        if(!entered) { observeAim = false; Fail(fighter + " failed to enter awakened normal state " + route.state + "; clips=" + string.Join(",", clips)); yield break; }
        stateDeadline = Time.realtimeSinceStartupAsDouble + 8;
        while(Time.realtimeSinceStartupAsDouble < stateDeadline &&
              (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == expectedHash || anim.GetBool("Stunned") || anim.IsInTransition(0))) {
            foreach(var clip in anim.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
            yield return null;
        }
        observeAim = false;
        float maximumError = maximumAimError;
        bool stillAwakening = manager.awakening && SpecialAtk.specialAtk && SpecialAtk.specialAtk.decharging;
        bool recovered = anim.GetCurrentAnimatorStateInfo(0).shortNameHash != expectedHash && !anim.GetBool("Stunned");
        Observe("awakened-normal-aim", fighter + "; state=" + route.state + "; direction=" + direction + "; variant=" + variant + "; entryDegrees=" + entryError + "; frames=" + aimFrames + "; maximumDegrees=" + maximumError + "; awakeningActive=" + stillAwakening + "; recovered=" + recovered + "; clips=" + string.Join(",", clips));
        if(aimFrames == 0 || maximumError > .15f) { Fail(fighter + " lost opponent facing during awakened " + route.state); yield break; }
        if(!stillAwakening) { Fail(fighter + " awakening expired before its normal attack completed"); yield break; }
        if(!recovered) Fail(fighter + " failed to recover from awakened normal " + route.state);
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output, ".awakened-" + fighter + ".png"));
        yield return new WaitForEndOfFrame();
    }
    IEnumerator DirectionalMoves(GameController game, string fighter) {
        report.limitation = "Ten neutral/cardinal A/X input routes per selected fighter; actual attack controls and animator callbacks, CPU disabled. Position reset between moves is synthetic. State entry/exit and creation/motion observations do not establish damage/aiming, every dependency, visuals, awakened variants or complete movesets.";
        var reader = BattleSceneSingleton.i.inputReader;
        var anim = game.animMainChar;
        var names = new[] {"BasicAttack", "UpAttack", "DownAttack", "RightAttack", "LeftAttack"};
        var axes = new[] {Vector2.zero, Vector2.up, Vector2.down, Vector2.right, Vector2.left};
        bool movingTargetOnly=Environment.GetCommandLineArgs().Contains("-rea-moving-target-probe");
        bool reportedFailuresOnly=Environment.GetCommandLineArgs().Contains("-rea-reported-failures-probe");
        if(movingTargetOnly) report.limitation="Sora UpAttack followed by natural UpAttack2 input; controlled moving CPU root with original collider/damage active. Flight trace validates target binding, per-frame native steering and contact-correlated damage. Not all roster, free CPU motion, height/edge/interrupt or original runtime parity.";
        var contractAsset=Resources.Load<TextAsset>("MoveInputContracts");
        if(!contractAsset) { Fail("Missing retained controller input contracts"); yield break; }
        var contracts=JsonUtility.FromJson<MoveInputContracts>(contractAsset.text);
        if(reportedFailuresOnly && contracts.routes.Count(r=>r.fighter==fighter)==10) ValidateReportedPrefabFixes(fighter);
        if(done) yield break;
        if(shadowPoweredField==null) { Fail("Shadow power owner state field unavailable; no power observation inferred from FX alone"); yield break; }
        if(contracts.routes == null || contracts.routes.Count(r=>r.fighter==fighter)!=10) { Fail("Incomplete input contracts for "+fighter); yield break; }
        Observe("move-input-contract", fighter+"; source="+contracts.source+"; sha256="+contracts.sourceSha256);
        for(int direction=movingTargetOnly?1:0; direction<(movingTargetOnly?2:names.Length); direction++) {
            int firstVariant = anim.GetInteger(names[direction]);
            for(int variantIndex=0; variantIndex<2; variantIndex++) {
            int variant = reportedFailuresOnly ? (firstVariant + variantIndex) % 2 : variantIndex;
            if(reportedFailuresOnly) {
                bool selected = fighter == "Sora"
                    ? (direction == 2 && variant <= 1) || ((direction == 3 || direction == 4) && variant <= 1)
                    : fighter == "Amon" ? direction == 2 && variant <= 1
                    : (fighter == "Vinea" || fighter == "Diana") && (direction == 3 || direction == 4) && variant <= 1;
                if(!selected) continue;
            }
            var attackZone=ActiveAttackZone();
            if(!attackZone) yield break;
            double deadline = Time.realtimeSinceStartupAsDouble+15;
            while((anim.GetBool("Stunned") || anim.IsInTransition(0)) && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            if(anim.GetBool("Stunned") || anim.IsInTransition(0)) { Fail(fighter+" input did not recover before "+names[direction]+variant); yield break; }
            if(anim.GetInteger(names[direction])!=variant) { Fail(fighter+" native alternation differs before "+names[direction]+variant+"; value="+anim.GetInteger(names[direction])); yield break; }
            // Isolated spatial fixture only; state/attack parameters are never forced.
            game.MainCharacter.transform.position = new Vector3(0,0,5);
            game.OpponentCharacter.transform.position = game.MainCharacter.transform.position + Quaternion.Euler(0,direction*50+variant*20+35,0)*Vector3.forward*13;
            OfflineMovingTargetProbe flight=null;
            if(movingTargetOnly && fighter=="Sora" && variant==1) {
                flight=OfflineMovingTargetProbe.Prepare(game,Path.ChangeExtension(output,".flight.json"));
                game.MainCharacter.transform.position=new Vector3(0,0,-8);
                var controlledBody=game.MainCharacter.GetComponent<Rigidbody>();
                if(controlledBody) controlledBody.position=game.MainCharacter.transform.position;
                flight.PlaceTarget(new Vector3(0,0,17));
            }
            foreach(var body in new[]{game.MainCharacter.GetComponent<Rigidbody>(), game.OpponentCharacter.GetComponent<Rigidbody>()}) if(body && !body.isKinematic) {body.linearVelocity=Vector3.zero;body.angularVelocity=Vector3.zero;}
            reader.SetXpos(axes[direction].x); reader.SetYpos(axes[direction].y);
            yield return new WaitForSeconds(.06f);
            yield return new WaitForEndOfFrame();
            Vector3 aimDirection=game.OpponentCharacter.transform.position-game.MainCharacter.transform.position;aimDirection.y=0;
            float facingError=Vector3.Angle(game.MainCharacter.transform.forward,aimDirection);
            Observe("aim-move-entry",fighter+"; direction="+direction+"; variant="+variant+"; degrees="+facingError);
            if(facingError>.1f){Fail(fighter+" not facing opponent before move");yield break;}
            // Resume in the regular coroutine phase before dispatching UGUI input.
            yield return null;
            var createdNames = new List<string>();
            var contract=contracts.routes.Single(r=>r.fighter==fighter && r.direction==direction && r.variant==variant);
            string expected = contract.state;
            creationEventOwner = GameEvents.i;
            creationObserver = id => {
                var created = UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude).FirstOrDefault(d=>d.gameObject.GetEntityId().Equals(id));
                if(created && created.gameObject.layer == game.MainCharacter.layer) {
                    createdNames.Add(created.name);
                    if(flight) flight.Bind(created.gameObject);
                    if(reportedFailuresOnly) {
                        string moveAttackName = fighter == "Sora" && direction == 3 && variant == 1 ? "AirAtkR2"
                            : fighter == "Sora" && direction == 4 && variant == 1 ? "AirAtkL2"
                            : fighter == "Vinea" && direction == 3 && variant == 1 ? "WaterAtkR"
                            : fighter == "Vinea" && direction == 4 && variant == 1 ? "WaterAtkL"
                            : fighter == "Diana" && direction == 3 && variant == 1 ? "RockL2"
                            : fighter == "Diana" && direction == 4 && variant == 1 ? "RockR2"
                            : fighter == "Amon" && direction == 2 && variant == 1 ? "FireDownAtk2" : null;
                        var mover = created.GetComponent<MoveAttack>();
                        if(moveAttackName != null && created.name.Contains(moveAttackName)) {
                            if(!mover || !mover.aimAtTarget) { Fail(fighter + " spawned " + moveAttackName + " without aimAtTarget"); return; }
                            typeof(MoveAttack).GetMethod("ChangeDirection",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(mover,null);
                            var target = created.gameObject.layer == BattleSceneSingleton.i.MainCharacter.layer ? BattleSceneSingleton.i.Opponent.transform : BattleSceneSingleton.i.MainCharacter.transform;
                            var intended = target.position - created.transform.position; intended.y = 0f;
                            var expectedForward = intended.sqrMagnitude > .0001f ? (Quaternion.LookRotation(intended,Vector3.up) * Quaternion.Euler(0,mover.startRotation,0) * Vector3.forward) : Vector3.zero;
                            float error = expectedForward.sqrMagnitude > .0001f ? Vector3.Angle(created.transform.forward,expectedForward) : 180f;
                            Observe("reported-live-aim", fighter + "; projectile=" + created.name + "; target=" + target.name + "; authored offset=" + mover.startRotation + "; heading error=" + error);
                            if(error > .5f) { Fail(fighter + " " + created.name + " is not facing the opposing fighter"); return; }
                        }
                        if(fighter == "Diana" && (created.name.Contains("RockL(Clone)") || created.name.Contains("RockR(Clone)"))) {
                            var rock = created.GetComponent<MoveRock>();
                            if(!rock || created.gameObject.layer != BattleSceneSingleton.i.MainCharacter.layer) { Fail("Diana side rock did not use the controlled fighter layer"); return; }
                            typeof(MoveRock).GetMethod("ChangeDirection",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(rock,null);
                            var target = BattleSceneSingleton.i.Opponent.transform;
                            var intended = (target.position - created.transform.position).normalized; intended.y = 0f;
                            var actual = (Vector3)typeof(MoveRock).GetField("dir",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(rock);
                            float error = Vector3.Distance(actual,intended);
                            Observe("reported-live-rock-aim", created.name + "; target=" + target.name + "; direction error=" + error);
                            if(error > .001f) { Fail("Diana side rock did not aim at the opposing fighter"); return; }
                        }
                    }
                    var rotation=created.GetComponent<StartRotation>();
                    if(rotation && rotation.enabled) {
                        expectedRotations[id]=created.transform.rotation*Quaternion.Euler(0,rotation.rot,0);
                        rotationDue[id]=Time.time+rotation.delay+.03f;
                        rotationErrors[id]=float.PositiveInfinity;
                    }
                    var power=created.GetComponent<TrapPowerUp>();
                    if(power && power.enabled && power.powerUpFX && !power.powerUpFX.activeSelf)
                        shadowPowers[id]=new ShadowPowerObservation{owner=power,damage=created.Damage,defense=created.Defense,scale=power.ScaleObject ? power.ScaleObject.transform.localScale : Vector3.one};
                }
            };
            creationEventOwner.onAttackLayerSetUp += creationObserver;
            moveOrigins.Clear(); moveTravel.Clear(); moveLabels.Clear(); moveSpawnNames.Clear(); observeMoves=true;
            expectedRotations.Clear(); rotationDue.Clear(); rotationErrors.Clear();
            shadowPowers.Clear();shadowPowerResults.Clear();
            int expectedHash=Animator.StringToHash(expected);
            var clips = new HashSet<string>();
            int before=playerProjectiles;
            maximumAimError=0;aimFrames=0;liveAimTargets.Clear();liveFollowTargets.Clear();observeAim=true;
            StartCoroutine(ObserveLiveAim(game));
            ExecuteEvents.Execute(attackZone.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerEnterHandler);
            Observe("aim-input-dispatch",fighter+"; state="+expected+"; attack="+anim.GetBool("Attack")+"; screen="+reader.inputSet.a+"; keyboard="+reader.keyboard+"; blocked="+reader.blockInputs+"; started="+game.gameStarted);
            deadline=Time.realtimeSinceStartupAsDouble+3;
            bool entered=false;
            while(Time.realtimeSinceStartupAsDouble<deadline) {
                foreach(var clip in anim.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
                if(anim.GetCurrentAnimatorStateInfo(0).shortNameHash==expectedHash) { entered=true; break; }
                yield return null;
            }
            ExecuteEvents.Execute(attackZone.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler);
            reader.SetXpos(0);reader.SetYpos(0);
            if(!entered) { Fail(fighter+" did not enter "+expected+"; clips="+string.Join(",",clips)); yield break; }
            double aimCaptureDue=Time.realtimeSinceStartupAsDouble+.8;
            bool aimCaptured=false;
            deadline=Time.realtimeSinceStartupAsDouble+14;
            while((anim.GetCurrentAnimatorStateInfo(0).shortNameHash==expectedHash || anim.GetBool("Stunned") || anim.IsInTransition(0)) && Time.realtimeSinceStartupAsDouble<deadline) {
                foreach(var clip in anim.GetCurrentAnimatorClipInfo(0)) if(clip.clip) clips.Add(clip.clip.name);
                if(!aimCaptured && (createdNames.Count>0 || Time.realtimeSinceStartupAsDouble>=aimCaptureDue)) {
                    ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".aim-"+fighter+"-"+(direction*2+variant)+".png"));
                    aimCaptured=true;
                }
                yield return null;
            }
            // Let delayed projectile producers run after the character leaves the state.
            yield return new WaitForSeconds(2f);
            if(reportedFailuresOnly) {
                string required = fighter == "Sora" && direction == 2 && variant == 0 ? "AirAtkTwister"
                    : fighter == "Sora" && direction == 3 && variant == 1 ? "AirAtkR2"
                    : fighter == "Sora" && direction == 4 && variant == 1 ? "AirAtkL2"
                    : fighter == "Vinea" && direction == 3 && variant == 1 ? "WaterAtkR"
                    : fighter == "Vinea" && direction == 4 && variant == 1 ? "WaterAtkL"
                    : fighter == "Diana" && direction == 3 && variant == 1 ? "RockL2"
                    : fighter == "Diana" && direction == 4 && variant == 1 ? "RockR2"
                    : fighter == "Amon" && direction == 2 && variant == 1 ? "FireDownAtk2" : null;
                if(required != null && !createdNames.Concat(moveSpawnNames).Any(n => n.Contains(required))) {
                    Fail(fighter + " reported-failure route did not spawn " + required + "; observed=" + string.Join(",", createdNames.Concat(moveSpawnNames)));
                    yield break;
                }
                if(fighter == "Sora" && direction == 2 && variant == 0 && UnityEngine.Object.FindObjectsByType<MoveWater>(FindObjectsInactive.Exclude).Any(m => m.name == "AirAtkTwister(Clone)")) {
                    Fail("Sora AirAtkTwister remained active more than two seconds after its down attack");
                    yield break;
                }
                if(fighter == "Sora" && direction == 2 && variant == 1) {
                    var chargeSphere = UnityEngine.Object.FindObjectsByType<ChangeScale>(FindObjectsInactive.Exclude).FirstOrDefault(c => c.name == "Spheres" && c.transform.parent && c.transform.parent.name.StartsWith("ChargeAir"));
                    if(!chargeSphere || chargeSphere.transform.localScale.sqrMagnitude > .0025f) {
                        Fail("Sora ChargeAir sphere did not contract to zero after the second down-attack animation; scale=" + (chargeSphere ? chargeSphere.transform.localScale.ToString() : "not found"));
                        yield break;
                    }
                    Observe("reported-charge-air", "Sora second down-attack ChargeAir sphere scale reached " + chargeSphere.transform.localScale + " while the 8 second cleanup lifetime remains available for trailing audio/particles");
                }
            }
            if(flight) {
                string error=flight.Complete(Observe);
                Destroy(flight.gameObject);
                if(error!=null) {Fail(error);yield break;}
            }
            observeAim=false;
            Observe("aim-live-move",fighter+"; state="+expected+"; frames="+aimFrames+"; maximumDegrees="+maximumAimError+"; homingEntities="+liveAimTargets.Count+"; followEntities="+liveFollowTargets.Count);
            if(aimFrames==0 || maximumAimError>.15f){Fail(fighter+" lost opponent facing during "+expected);yield break;}
            observeMoves=false;
            creationEventOwner.onAttackLayerSetUp -= creationObserver;
            creationObserver=null;creationEventOwner=null;
            bool recovered=anim.GetCurrentAnimatorStateInfo(0).shortNameHash!=expectedHash && !anim.GetBool("Stunned");
            Observe("directional-move", fighter+"; state="+expected+"; input="+axes[direction]+"; type="+variant+"; recovered="+recovered+"; clips="+string.Join(",",clips)+"; event spawned="+string.Join(",",createdNames)+"; polled spawned="+string.Join(",",moveSpawnNames)+"; polled attack entities="+(playerProjectiles-before)+"; motion="+string.Join(",",moveTravel.Select(pair=>moveLabels[pair.Key]+"="+pair.Value.ToString("F3")))+"; yaw objects="+expectedRotations.Count+"; yaw errors="+string.Join(",",rotationErrors.Values));
            if(shadowPowers.Count>0) Observe("shadow-power-observation",fighter+"; state="+expected+"; watched="+shadowPowers.Count+"; observed="+shadowPowerResults.Count+"; "+string.Join(";",shadowPowerResults)+"; no observed power-up does not establish absence or full trap acceptance");
            if(!recovered) { Fail(fighter+" did not recover after "+expected); yield break; }
            if(rotationErrors.Values.Any(error=>error>.5f)) { Fail(fighter+" authored projectile yaw not observed for "+expected); yield break; }
            }
        }
    }
    IEnumerator RosterTrip() {
        CaptureFixture();
        foreach (var field in typeof(TMPlayerCharacters).GetFields())
            if (field.FieldType == typeof(int)) field.SetValue(SystemManager.i.tmPlayer.characters, 1);
        CharactersManager.i.SetPlayableCharacters();
        Observe("roster-fixture", "Temporary ownership for twelve shipped fighter prefabs; save/preferences restored after test");
        Click("SinglePlayerMode", false);
        yield return SceneReady("HowToPlay");
        Click("Continue", true);
        string[] roster = { "Sora", "Vinea", "Diana", "Amon", "Aura", "Eliot", "Ren", "Rina", "Dante", "Agata", "Raven", "Maya" };
        foreach (string fighter in roster) {
            yield return SceneReady("Character Selection");
            var selection = SelectionController.i;
            selection.ChangeCharacter(fighter);
            selection.cpuController.ChangeCharacter("Amon");
            Observe("roster-select", fighter + " vs Amon");
            Click("start", true);
            yield return SceneReady("Bending Battle");
            while (GameController.i == null || !GameController.i.gameStarted) yield return null;
            var game = GameController.i;
            if (game.MainCharacter == null || game.MainCharacter.name != fighter + "(Clone)") { Fail("Wrong roster fighter spawned: requested=" + fighter + "; actual=" + (game.MainCharacter ? game.MainCharacter.name : "null")); yield break; }
            CaptureRenderState();
            int ownBefore = playerProjectiles, cpuBefore = opponentProjectiles, hitsBefore = hitEvents;
            if (GameEvents.i != null) GameEvents.i.OnPlayerDamaged += CountHit;
            var attack = UnityEngine.Object.FindObjectsByType<ButtonAttack>(FindObjectsInactive.Exclude).OrderBy(b=>b.attackType).FirstOrDefault();
            if (attack == null) { Fail("Roster fighter has no active attack control"); yield break; }
            attack.OnButtonPress();
            yield return new WaitForSeconds(4f);
            Observe("roster-basic", fighter + "; element=" + game.MainCharacter.GetComponent<Description>().element + "; player projectiles=" + (playerProjectiles-ownBefore) + "; CPU projectiles=" + (opponentProjectiles-cpuBefore) + "; hits=" + (hitEvents-hitsBefore) + "; animator=" + game.animMainChar.GetCurrentAnimatorClipInfo(0).FirstOrDefault().clip?.name);
            if (GameEvents.i != null) GameEvents.i.OnPlayerDamaged -= CountHit;
            ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".roster-"+fighter+".png"));
            yield return new WaitForEndOfFrame();
            game.Restart();
        }
        report.limitation = "All twelve shipped fighter prefabs selected through the recovered menu and sampled for one basic attack without exceptions. Projectile telemetry is not full-move/special/defense or visual equivalence acceptance. Temporary ownership data restored.";
    }
    IEnumerator TournamentTrip() {
        // Isolated eligibility fixture: preserve the reconstruction save bytes,
        // temporarily supply owned characters/currency, then restore on Finish.
        CaptureFixture();
        var data = SystemManager.i.tmPlayer;
        data.crystals = Mathf.Max(data.crystals, 1000);
        data.characters.aura = data.characters.eliot = data.characters.ren = data.characters.rina = 1;
        CharactersManager.i.SetPlayableCharacters();
        Observe("tournament-fixture", "Temporary character eligibility and currency; original reconstruction save bytes will be restored");
        var menu = UnityEngine.Object.FindFirstObjectByType<StarMenuManager>();
        int fee = menu.TournamentPrice;
        float wallet = data.crystals;
        float originalTrophies = data.trophies;
        Click("TourneyAlert", false);
        yield return new WaitForSeconds(.6f);
        Click("EnterTournament", false);
        yield return SceneReady("Character Selection");
        if (SystemManager.i.mode != GameMode.Tournament || Mathf.Abs(data.crystals - (wallet-fee)) > .01f) { Fail("Tournament entry did not debit the caller-supplied fee and set mode"); yield break; }
        Observe("tournament-entry", "fee=" + fee + "; wallet delta=" + (data.crystals-wallet));
        Click("start", true);
        yield return SceneReady("Tournament");
        for (int battle=1; battle<=3; battle++) {
            yield return SceneReady("Bending Battle");
            while (GameController.i == null || !GameController.i.gameStarted) yield return null;
            var game = GameController.i;
            var score = Score.score;
            CaptureRenderState();
            Observe("tournament-arena", "battle=" + battle + "; persisted battle=" + TournamentController.i.battle + "; CPU=" + game.OpponentCharacter.name);
            var zone = UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsInactive.Exclude)
                .Where(c => c.CompareTag("DeathZone") && c.enabled).OrderByDescending(c => c.bounds.size.x*c.bounds.size.z).FirstOrDefault();
            if (zone == null) { Fail("Tournament arena has no DeathZone"); yield break; }
            game.OpponentCharacter.GetComponent<AI>().on = false;
            for (int point=1; point<=3; point++) {
                var fighter = game.OpponentCharacter;
                fighter.transform.position = zone.bounds.center;
                var body = fighter.GetComponent<Rigidbody>();
                if (body != null) body.linearVelocity = Vector3.zero;
                double deadline = Time.realtimeSinceStartupAsDouble + 12;
                while (score.playerPoints < point && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                if (score.playerPoints != point) { Fail("Tournament synthetic fall did not score exactly once"); yield break; }
                if (point < 3) {
                    while ((fighter.transform.position.y<0 || zone.bounds.Contains(fighter.transform.position)) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                    if (fighter.transform.position.y<0) { Fail("Tournament fighter did not respawn"); yield break; }
                    yield return new WaitForSeconds(1f);
                }
            }
            double resultDeadline = Time.realtimeSinceStartupAsDouble+8;
            while (!ResultsController.resultsController.GetComponent<Animator>().GetBool("In") && Time.realtimeSinceStartupAsDouble < resultDeadline) yield return null;
            if (!score.battleFinish || TournamentController.i.loseTournament || !ResultsController.resultsController.GetComponent<Animator>().GetBool("In")) { Fail("Tournament victory did not initialize the winning result"); yield break; }
            Observe("tournament-win", "battle=" + battle + "; score=" + score.playerPoints + "-" + score.enemyPoints);
            game.Restart();
            yield return SceneReady("Tournament");
        }
        double championDeadline=Time.realtimeSinceStartupAsDouble+20;
        while (TournamentController.i.round<6 && Time.realtimeSinceStartupAsDouble<championDeadline) yield return null;
        int losers=TournamentController.i.losers.Count(value=>value);
        Observe("tournament-final", "round=" + TournamentController.i.round + "; losers=" + losers + "; player1 lost=" + TournamentController.i.losers[1]);
        if (TournamentController.i.round!=6 || losers!=7 || TournamentController.i.losers[1]) { Fail("Tournament final bracket differs from the three-match winning path"); yield break; }
        double trophyDeadline = Time.realtimeSinceStartupAsDouble + 20;
        int trophyReward = TournamentController.i.tourneyLevel;
        while (data.trophies < originalTrophies + trophyReward && Time.realtimeSinceStartupAsDouble < trophyDeadline) yield return null;
        Observe("tournament-prize", "trophies delta=" + (data.trophies-originalTrophies) + "; expected=" + trophyReward + "; replay menu=" + TournamentManager.i.menu.activeSelf);
        if (Mathf.Abs(data.trophies-originalTrophies-trophyReward) > .01f || !TournamentManager.i.menu.activeSelf) { Fail("Tournament champion prize/menu did not match native tourneyLevel award"); yield break; }
        ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".champion.png"));
        yield return new WaitForEndOfFrame();
        report.limitation="Paid tournament menu/selection/bracket and three synthetic physics-fall wins checked with temporary eligibility data. Original save bytes restored; natural combat, loss/replay paths and visual fidelity unverified.";
    }
    void CaptureRenderState() {
        var state = new RenderReport();
        foreach (var renderer in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude)) {
            if (!renderer.enabled || renderer is ParticleSystemRenderer) continue;
            foreach (var material in renderer.sharedMaterials) {
                if (material == null) continue;
                state.renderers.Add(new RenderItem { path = ObjectPath(renderer.transform), material = material.name,
                    shader = material.shader ? material.shader.name : null,
                    texture = material.mainTexture ? material.mainTexture.name : null,
                    color = material.HasProperty("_Color") ? material.GetColor("_Color") : Color.clear,
                    center = renderer.bounds.center, size = renderer.bounds.size });
            }
        }
        foreach (var light in UnityEngine.Object.FindObjectsByType<Light>(FindObjectsInactive.Exclude))
            state.lights.Add(new LightingItem { path = ObjectPath(light.transform), type = light.type.ToString(), intensity = light.intensity, enabled = light.enabled });
        File.WriteAllText(Path.ChangeExtension(output, ".renderers.json"), JsonUtility.ToJson(state, true));
    }
    static string ObjectPath(Transform tr) {
        string value = tr.name;
        while (tr.parent != null) { tr = tr.parent; value = tr.name + "/" + value; }
        return value;
    }
    IEnumerator SurvivalTrip() {
        var game = GameController.i;
        var score = Score.score;
        if (SystemManager.i.mode != GameMode.Survival || score.playerPoints != 0 || score.enemyPoints != 0) {
            Fail("Survival fixture requires fresh Survival at 0-0"); yield break;
        }
        var zone = UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsInactive.Exclude)
            .Where(c => c.CompareTag("DeathZone") && c.enabled).OrderByDescending(c => c.bounds.size.x * c.bounds.size.z).FirstOrDefault();
        if (zone == null) { Fail("No DeathZone for Survival fixture"); yield break; }
        float wallet = SystemManager.i.tmPlayer.crystals;
        for (int point = 1; point <= 2; point++) {
            var old = game.OpponentCharacter;
            old.GetComponent<AI>().on = false;
            old.transform.position = zone.bounds.center;
            var body = old.GetComponent<Rigidbody>();
            if (body != null) body.linearVelocity = Vector3.zero;
            Observe("survival-fall-fixture", "CPU=" + old.name + "; expected point=" + point);
            double deadline = Time.realtimeSinceStartupAsDouble + 12;
            while ((score.playerPoints < point || game.OpponentCharacter == old) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
            if (score.playerPoints != point || game.OpponentCharacter == old || game.OpponentCharacter == null) { Fail("Survival did not award point and replace defeated CPU"); yield break; }
            var replacement = game.OpponentCharacter;
            if (BattleSceneSingleton.i.Opponent != replacement || GuestPlayer.guestPlayer.gameObject != replacement || game.guestPlayer != replacement) { Fail("Survival replacement ownership disagrees across consumers"); yield break; }
            Observe("survival-replacement", "point=" + point + "; replacement=" + replacement.name + "; layer=" + replacement.layer);
            yield return new WaitForSeconds(1.5f);
        }
        var host = game.MainCharacter;
        host.transform.position = zone.bounds.center;
        var hostBody = host.GetComponent<Rigidbody>();
        if (hostBody != null) hostBody.linearVelocity = Vector3.zero;
        Observe("survival-loss-fixture", "Controlled fighter positioned inside DeathZone");
        double resultDeadline = Time.realtimeSinceStartupAsDouble + 10;
        while ((ResultsController.resultsController == null || !ResultsController.resultsController.GetComponent<Animator>().GetBool("In")) && Time.realtimeSinceStartupAsDouble < resultDeadline) yield return null;
        var result = ResultsController.resultsController;
        if (result == null || !result.GetComponent<Animator>().GetBool("In")) { Fail("Survival result was not initialized"); yield break; }
        float awarded = SystemManager.i.tmPlayer.crystals - wallet;
        Observe("survival-results", "score=" + score.playerPoints + "-" + score.enemyPoints + "; rank=" + result.rank.text + "; wallet delta=" + awarded + "; record=" + SystemManager.i.tmPlayer.survivalRecord);
        if (score.enemyPoints != 1 || Mathf.Abs(awarded - 4f) > .01f || SystemManager.i.tmPlayer.survivalRecord < 2f) { Fail("Survival score, reward or record differs from native-derived expectation"); yield break; }
        report.limitation = "Synthetic Survival falls, CPU replacement ownership and two-point record/reward checked; combat difficulty, all roster selections and visual fidelity unverified.";
    }
    IEnumerator Impact() {
        var game = GameController.i;
        var target = game.OpponentCharacter;
        var ai = target.GetComponent<AI>();
        ai.on = false;
        ai.enabled = false;
        // Synthetic collision fixture with the recovered attack prefab; no call
        // to damage methods or manually raised damage events.
        var prefab = Resources.Load<GameObject>("PhotonPrefabs/AirAtk");
        var receiver = target.GetComponent<DamageOnCollision>();
        var collider = target.GetComponent<Collider>();
        if (prefab == null || receiver == null || collider == null) { Fail("Impact fixture dependencies unavailable"); yield break; }
        double deadline = Time.realtimeSinceStartupAsDouble + 5;
        while (receiver.Validations("Attack") && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
        if (receiver.Validations("Attack")) { Fail("Impact target remained invulnerable"); yield break; }
        float damage = receiver.Damage;
        int previousHits = hitEvents;
        var projectile = Instantiate(prefab, collider.bounds.center, game.MainCharacter.transform.rotation);
        projectile.layer = game.MainCharacter.layer;
        Observe("impact-fixture", "Recovered AirAtk instantiated inside CPU collider; damage before=" + damage);
        deadline = Time.realtimeSinceStartupAsDouble + 3;
        while (hitEvents == previousHits && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
        Observe("impact", "damage delta=" + (receiver.Damage - damage) + "; event delta=" + (hitEvents - previousHits));
        if (projectile != null) Destroy(projectile);
        if (hitEvents <= previousHits || receiver.Damage <= damage) { Fail("Recovered projectile collision did not emit damage and increase damage accumulator"); yield break; }
        report.limitation = "Includes a synthetic recovered-projectile overlap, not proof that player aiming or every elemental attack works.";
    }
    IEnumerator RoundTrip() {
        // Deliberate physics fixture, not a claim that attacks caused the falls.
        // Existing trigger callbacks, animation respawn and result owners run normally.
        var game = GameController.i;
        var score = Score.score;
        if (SystemManager.i.mode != GameMode.SinglePlayer || score.playerPoints != 0 || score.enemyPoints != 0) {
            Fail("Round fixture requires a fresh singleplayer match at 0-0"); yield break;
        }
        var zones = UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsInactive.Exclude)
            .Where(c => c.CompareTag("DeathZone") && c.enabled).ToArray();
        if (zones.Length == 0) { Fail("No active recovered DeathZone collider"); yield break; }
        var zone = zones.OrderByDescending(c => c.bounds.size.x * c.bounds.size.z).First();
        var ai = game.OpponentCharacter.GetComponent<AI>();
        ai.on = false;
        float wallet = SystemManager.i.tmPlayer.crystals;
        for (int point = 1; point <= 3; point++) {
            var fighter = game.OpponentCharacter;
            fighter.transform.position = zone.bounds.center;
            var body = fighter.GetComponent<Rigidbody>();
            if (body != null) body.linearVelocity = Vector3.zero;
            Observe("fall-fixture", "CPU positioned inside recovered " + zone.name + "; expected player point=" + point);
            double deadline = Time.realtimeSinceStartupAsDouble + 12;
            while (score.playerPoints < point && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
            if (score.playerPoints != point || score.enemyPoints != 0) { Fail("Physics fall did not award exactly one opponent point"); yield break; }
            Observe("point", score.playerPoints + "-" + score.enemyPoints);
            if (point < 3) {
                while ((fighter.transform.position.y < 0 || zone.bounds.Contains(fighter.transform.position)) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                if (fighter.transform.position.y < 0 || zone.bounds.Contains(fighter.transform.position)) { Fail("Nonfinal fall did not respawn CPU above arena"); yield break; }
                Observe("respawn", fighter.name + "; position=" + fighter.transform.position);
                yield return new WaitForSeconds(1f);
            }
        }
        // Death schedules DelayWin after1s, then FoeWin schedules results2.5s later.
        yield return new WaitForSeconds(4f);
        if (!score.battleFinish) { Fail("Three points did not mark battle finished"); yield break; }
        var result = ResultsController.resultsController;
        if (result == null || !result.GetComponent<Animator>().GetBool("In")) { Fail("Result presentation was not initialized"); yield break; }
        float expected = GameController.i.practiceMode ? 0 : 6;
        switch (PlayerPrefs.GetInt("CpuMode", 0)) {
            case 1: expected = Mathf.Ceil(expected * 1.5f); break;
            case 2: expected = Mathf.Ceil(expected * 2f); break;
            case 3: expected = Mathf.Ceil(expected * 2.5f); break;
            case 4: expected = Mathf.Ceil(expected * 3f); break;
        }
        float awarded = SystemManager.i.tmPlayer.crystals - wallet;
        Observe("results", "rank=" + result.rank.text + "; display=" + result.crystalsView.text + "; wallet delta=" + awarded + "; expected=" + expected);
        if (Mathf.Abs(awarded - expected) > .01f) { Fail("Result currency differs from native-derived expectation"); yield break; }
        result.InitResults();
        if (Mathf.Abs(SystemManager.i.tmPlayer.crystals - wallet - expected) > .01f) { Fail("Repeated result callback awarded currency twice"); yield break; }
        Observe("result-idempotence", "Repeated InitResults did not award additional currency");
        report.limitation = "Startup, movement, camera, attack spawning and synthetic physics falls through recovered DeathZone, respawn and three-point results exercised; attack-caused damage, Survival and visual equivalence unverified.";
    }
    void CountHit(int layer) { hitEvents++; }
    void Fail(string message) { report.errors.Add(message); Finish("probe_failed"); }
    void Finish(string result) {
        if (done) return;
        done = true;
        report.result = result;
        Application.logMessageReceived -= Record;
        if (GameEvents.i != null) GameEvents.i.OnPlayerDamaged -= CountHit;
        if(creationEventOwner != null && creationObserver != null) creationEventOwner.onAttackLayerSetUp -= creationObserver;
        creationObserver = null; creationEventOwner = null;
        if (savedPlayer != null) {
            if (SystemManager.i != null) SystemManager.i.tmPlayer = savedPlayer;
            foreach (var entry in savedFiles) {
                if (entry.Value != null) File.WriteAllBytes(entry.Key, entry.Value);
                else if (File.Exists(entry.Key)) File.Delete(entry.Key);
            }
            foreach (var entry in savedStringPrefs) PlayerPrefs.SetString(entry.Key, entry.Value);
            foreach (var entry in savedIntPrefs) PlayerPrefs.SetInt(entry.Key, entry.Value);
            foreach (var key in absentPrefs) PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Observe("fixture-restored", "Original reconstruction save bytes restored");
        }
        Directory.CreateDirectory(Path.GetDirectoryName(output));
        File.WriteAllText(output, JsonUtility.ToJson(report, true));
        Debug.Log("OFFLINE_PLAYER_PROBE_COMPLETE " + result);
        Application.Quit(result == "passed" ? 0 : 1);
    }
}
#endif
