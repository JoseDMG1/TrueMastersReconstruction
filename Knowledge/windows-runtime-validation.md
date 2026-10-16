# Windows player: observed acceptance runs

The original target remains the provided APK and separately validated companion ARM64 library; the Windows player is a reconstruction testing artifact, not the original game executable.

## Build and initial smoke

`Evidence/windows-build-validation.json` at2026-10-15T21:59:38Z reports Development Windows build success,0errors,21warnings,333.68s,543207977bytes. A later build at22:06:44Z succeeded0errors,16warnings,30.96s,543210963bytes. Neither build establishes complete offline functionality.

The first hidden player launched at22:04:46Z did not complete the probe while unfocused (`runInBackground:0`). It was terminated after checking exact process path/start time. The opt-in `OfflinePlayerProbe.Launch` now sets Application.runInBackground=true; normal interactive launch is unaffected. This corrects the test harness, not original gameplay.

`Evidence/offline-player-probe-20261015-170651.json` is the executed smoke, generated22:06:57Z, Direct3D11, Unity6000.6.5f1, result passed, errors=[]:

- Original boot→StartMenu→HowToPlay→Character Selection→Bending Battle through serialized Button/EventTrigger callbacks.
- Sora vsAmon, CpuMode0, arena-ready17.96s after probe start.
- InputReader directional input moved controlled fighter2.276128units; MobileInputs.enabled=true.
- Explicit camera close-pose assertion reached position error0/rotation error0.
- Actual A control triggered, player andCPU each produced1 observed projectile; AI enabled/on=true.
- No damage events occurred during12scaled-seconds observation. This does not validate impacts or combat damage.
- No full round, respawn, result or Survival was exercised in this smoke.

Black screenshots taken immediately at scene-ready can capture a fade/transition, so they are not proof of an entirely black game. The final `.combat.png` was opened and showed the rendered fighter/arena/UI, but black text rectangles and the erroneous offline WAITING FOR YOUR OPPONENT overlay. Rendering/fidelity did not pass merely because functional smoke passed.

## Changes after the smoke

Original recovered TextMeshPro shaders explicitly contained DummyShaderTextExporter bodies, rendering the SDF atlas as an opaque texture. `Tools/restore-tmp-shaders.cjs` substitutes corresponding official same-named shader bodies from installed Unity6000.6.5f1 UGUI2.6.0 TMP Essential Resources.unitypackage. Existing shader .meta GUIDs, fonts, materials and serialized references are preserved; includes are copied alongside. Hashes/backups and limits are in `Evidence/tmp-shader-restoration.json`. These compatible shaders are replacement implementations, not original binary shader source recovery. Needs a new graphics run.

Offline waiting overlay: Bending Battle scene WaitingMsg object151 has ShowInOnlineMode component1402, whose recovered script was empty. Correct native `Evidence/native-disassembly-ShowInOnlineMode.txt` VA0x12AF4E0 sets active when PhotonNetwork.IsConnected OR SystemManager.mode==Online and inactive otherwise. The owner was restored; no arbitrary scene object deletion or GameController fallback was added. Needs a new build/run.

GameEvents.Start duplicate-owner guard restored from VA0x1236230, with owner-aware OnDestroy cleanup added as reconstruction lifecycle behavior. EntityId migration remains separate and intact.

## Extended round probe

Opt-in `-rea-round-probe` extends `-rea-offline-probe <absolute report path>`. It deliberately moves CPU inside an actual recovered DeathZone Collider.bounds and lets existing physics callbacks/animations score and respawn. It checks three points, result presentation, native-derived wallet change and repeated InitResults idempotence. This is a synthetic fall fixture; it does not establish attack-caused falls. Source: Tools/unity-validation/OfflinePlayerProbe.cs. Compilation may take earlier snapshots than concurrently edited files; use actual build/log timestamps to scope claims.

First executed round run `Evidence/offline-round-probe-20261015-171025.json` reached all three points and two respawns, then the probe checked results too early (2s, whereas native chain is DelayWin1s then results2.5s). This was a probe timing failure; game scripts were not altered to satisfy it. Wait corrected to4scaled-seconds.

Second executed round run `Evidence/offline-round-probe-20261015-171155.json` **passed** with errors=[]: two Amon respawns at approximately(-7,5,0), third point3-0, rankS, display6, wallet delta6, repeated InitResults no additional transaction. Build preceding this run succeeded22:11:42Z,19.19s,0errors. It still does not prove attack-caused falls, all characters or Survival.

The earlier round `.combat.png` was opened: TMP shader replacement renders names, clock, score and menu labels legibly. WAITING overlay remained until ShowInOnlineMode was included in the next build; ExitMenu visible at startup was traced to its ChangeStartScale component authored with zero scale and still-stub Awake. Owner restored from native0x121E73C (see initial-scale-reconstruction.md), without hiding arbitrary UI objects. Next graphics probe must validate that change.

New opt-ins `-rea-impact-probe` and `-rea-survival-probe` were added for a real recovered-projectile collider overlap and two CPU replacements followed by Survival host loss, respectively. They are probes pending execution, not established game coverage.

## Impact and Survival follow-up

`Evidence/offline-impact-probe-20261015-171347.json` **passed**, errors=[]: the actual recovered AirAtk overlap produced damage delta.5 and exactly1 damage event; normal physics rounds, two respawns,3-0 resultsS/reward6 and idempotence also passed. A natural hit had already changed receiver damage to.5 before the synthetic overlap. This establishes this projectile/collision path only, not aiming/every move or elemental roster. The final `.combat.png` was opened: names/clock/score legible, WAITING overlay absent and ExitMenu hidden. Soil/other materials still need fidelity work; the scene is not visually accepted as complete.

`Evidence/offline-survival-probe-20261015-171441.json` observed Dante→Rina→Diana CPU replacements at points1/2 and consistent current owner references. Host loss then exposed a **probe** NullReferenceException in OfflinePlayerProbe.Update when it read the intentionally destroyed CPU's layer. The observer now snapshots and validates both fighter references before counting attacks; game damage/scoring was not changed for that failure. A new Survival run is needed for result/reward/record acceptance.

Material hypothesis rejected: `Evidence/material-loaded-validation.json` at22:15:54Z loads Dirt/Dirt1 in real Unity with Standard shader, authored brown colors, normal texture and metallic. Their map-form serialized saved properties load correctly. Do not rewrite every material YAML into sequences or infer corruption merely from that representation. Source main texture is explicitly null. The white floor's cause remains unresolved; inspect runtime renderer identities/light/material ownership before editing resources.

`Tools/Run-OfflineProbe.ps1 -Scenario Smoke|Rounds|ImpactAndRounds|Survival` launches a hidden development player with fresh report/log and launch manifest. It rejects a simultaneous owned player because both would share reconstruction preferences/save data. Returned PID does not mean completion. Source hash of EXE is recorded, while game logic resides also in the matching Data directory.

`Evidence/offline-survival-probe-20261015-171702.json` **passed**, errors=[] after the observer null fix. CPU Dante→Eliot→Aura changed ownership consistently at points1/2. Controlled fighter loss produced score2-1, rank2, wallet delta4, stored record2. Natural short combat observation emitted1 damage event and CPU4 projectiles in that run. This is bounded evidence for those randomly selected replacements; all roster/difficulty combinations remain unverified. Build preceding the run succeeded22:16:47Z,18.59s,0errors.

Launch manifests now also include the exact Assembly-CSharp.dll digest because the native Unity EXE remains identical across game script builds. Check the Data directory as well as EXE identity.

## Tournament acceptance

`Evidence/offline-tournament-probe-20261015-172112.json` **passed**: original menu TourneyAlert/EnterTournament, fee50 debited, Character Selection callback routed to the Tournament bracket, three arenas and synthetic3-0 wins, bracket restored between wins, finalround6/7losers/player1 champion. Original reconstruction save bytes were restored by the eligibility fixture. This validates the winning path; real aiming/combat, loss/replay and every level remain unverified.

SelectionController.start needed its native tournament branch at0x12ADA68..0x12ADAF4: delayed scene6 at1.7s (float readVA0x651AC4), actualStage random0..6, TournamentController.Reset. Ordinary GoToBattle remains scene5. The earlier implementation had omitted that branch. Do not put a fake bracket transition in the probe.

`Evidence/offline-tournament-probe-20261015-172532.json` **passed** after restoring StarsFX and TransladeObject. In addition to the same three wins, prize observation at80.82s reports trophies delta1/expected1 and replay menu active. Prize is native tourneyLevel, not a guessed constant. Exact gameplay Assembly-CSharp hash in launch manifest:9dd83898788b4d35baf8241de7ce9f2e8b1b347acfc78e1164fbc8553beb1d7c. Original save bytes and captured preferences restored. Stage skybox and FadeAudio restoration compiled in this build; audible fidelity was not assessed.

Earlier interim log reads appeared to remain at bracket initialization, but final reports proved normal progression. No tournament progression patch was made based on that unconfirmed stall hypothesis.

Roster probe added next: twelve exact shipped fighter names from the recovered roster, temporary ownership records, one basic input sample/character, telemetry and screenshots, restoring save/preferences. This is deliberately bounded prefab/animation smoke and must not be promoted into complete move/special/defense acceptance.

## Roster material-cache defect and menu fixture

`Evidence/offline-roster-probe-20261015-172833.json` failed while selecting Agata
after nine completed basic samples. LightWeapon.Light dereferenced mat.color on
a null material. Source scene and immutable export audit establish Agata's sword
list already contains a serialized null, while its renderer reference is valid.
The runtime cache is now rebuilt from renderer.material in LightWeapon.Start;
callbacks remain unchanged. This deliberately repairs exported data/cache
ownership instead of filtering failed consumers. See elemental-owners-reconstruction.md
and Evidence/lightweapon-material-boundary-audit.json. A repeat roster run was
launched at22:33:52Z; final result must be checked separately.

`Evidence/offline-menu-probe-20261015-173301.json` verified129 assigned translation
fields, rejected a stage1 purchase with299 crystals without changing ownership
or currency, then debited exactly300 and read ownership/currency back from the
reconstruction JSON save. It reached the battle scene, then **the probe failed**
because it searched for OpenSettings as EventTrigger. The real binding is
Button.onClick; CloseSettings separately uses EventTrigger PointerClick. No
settings product code was changed in response. The fixture restored original
save bytes/preferences. The source probe now follows each exact binding and
requires a new build/run before settings acceptance.

Tools/inspect-offline-menu-bindings.mjs now identifies components by actual
official UGUI script GUID and retains Button/EventTrigger binding kind, property
and event identity. Evidence/offline-menu-binding-inventory.json is the parsed
result; a bare persistent-call method name is not sufficient to infer control type.

Graphics diagnostic added: -Scenario Graphics performs the ordinary short
combat smoke, freezes time, captures the same scene with original Projector
enable flags and with all projectors disabled, then restores flags/timeScale in
finally. This is an opt-in diagnostic only, not a product graphics change. Its
purpose is to test the dummy projector shader as the suspected source of white
ground/body fragments. Results and image inspection are required before claiming
causality or making a shader repair.

Follow-up runtime results:

- `Evidence/offline-roster-probe-20261015-173352.json` passed with errors=[]:
  all twelve fighters selected/spawned, one basic input each, each produced
  at least one player attack entity. This includes Agata after the cache repair.
  Original save bytes/preferences restored. No full special/defense acceptance.
- `Evidence/offline-menu-probe-20261015-174045.json` passed with errors=[]:
 129 translations,299/300 purchase boundary and JSON readback, settings opened
 via Button.onClick and closed via EventTrigger PointerClick. Fixture restored.
- `Evidence/offline-graphics-probe-20261015-173810.json` passed; both original
 and projectors-disabled PNGs were opened. Same frozen Sora/Vinea scene shows
 white ground with the dummy shader and textured ground when only projectors
 are disabled. This establishes causation for the examined ground; it does not
 establish Dante's body appearance or all stages.
- Approximate dynamic-projector shader replacement applied with original GUID,
 backup and manifest Evidence/projector-shader-restoration.json. MIT references
 and limitations are in Tools/shader-candidates/dynamic-shadow-projector/README.md.
 The texture-generation plugin remains stubbed. Static projector candidate was
 not applied. No stage materials or scene lights were changed.
- Latest build succeeded22:40:41Z,62.34s,0errors/24warnings,543296849bytes.
 Gameplay assembly SHA is8b5c8da7eec11ba52bc29fa0b9e4cf2836313b4913fb6f99126b07290d94cf75.
 Repaired graphics/cookie probe `Evidence/offline-graphics-probe-20261015-174126.json`
 passed, errors=[]. Images opened; textured ground restored. Full-frame pixel
 comparison shows neutral fallback and white cookie exactly match disabled
 projectors, while black cookie darkens251499pixels and brightens0 aboveRGB2.
 See stage-render-reconstruction.md and the matching .pixel-comparison.json.
 This is bounded projector functionality, not restored dynamic texture generation.
