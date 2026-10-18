# CURRENT: HUD CORRECTED AND ALTERNATE CONTROLS UPDATED

- Latest build: 2026-10-16 08:34:45 UTC, succeeded with0 errors. Final smoke run offline-smoke-probe-20261016-033453 passed; screenshot inspected. Eye aspect, both HUD backgrounds and settings gear corrected.
- Default: arrows/Z attack/X evade-shield/C super. Alternate: WASD/left click attack/SPACE evade-shield-jump/RIGHT CLICK super. New physical mapping not separately exercised.
- Public repository omits local Evidence, reference videos/screenshots, build outputs, caches, backups and third-party tool installers. Photon service identifiers cleared in public snapshot. Historical sections below describe earlier builds and mappings.

<!-- FLIGHT_MATRIX_CURRENT_START -->
# CURRENT: 368 FLIGHT CASES AND FIVE REAL AWAKE EVADE ROUTES PASSED

- User asked to extend the opponent-aiming checks with the REA skill. Applied retained original ARM64/metadata, corrected local Capstone and Unity YAML. No connected REA MCP or installed rea CLI was available; these are local observations. This continuation changes opt-in development validation only; authored combat owners/prefab values remain preserved.
- Latest player: `Builds/Windows/TrueMastersOffline.exe`. Build `Evidence/windows-build-validation-flight-final-20261016-025525.json` Succeeded,0 errors/37 warnings. Gameplay DLL SHA256 `5f27b00b4a836b051af33902ee8adb6e255876858fa950c075296ff39ecbd0ad`. All three final runtimes below match this DLL. Controls remain arrows/Z/X/C and alternate WASD/left click/LEFT Shift/Space.
- FINAL `Evidence/offline-flightmatrix-probe-20261016-025820.json` and `.matrix.json` PASSED/errors=[]:46 authored prefab profiles across both fighter layers and four cases (moving target, target height7, projectile edge near33, physical owner interruption),368cases.33 enabled Follow occurrences comprise32live root profiles plus FlowerBeam's dormant descendant;13 additional MoveLight profiles retain once-only aim/fixed heading.24,712 live samples,11,861 turning samples,5,852 fixed movement checks,3,858 weak-movement samples. Maximum rotation error0 and translation error0.00000190735.52 cases correlated actual projectile contact with target damage. These are prefab production fixtures in Sora/Amon scene, NOT368 natural input routes or guaranteed hits for every geometry.
- Interruption uses an authored enemy AirAtk collider explicitly overlapped with the owner, with real DamageOnCollision callbacks.84 registered cases acquired weakness;8 unregistered cases correctly did not (ShockAtk R, ShockAtk L2, BigDemonAtk, FlowerBeam, both sides). No invented cancellation. Edge cases with authored DestroyOnEdge crossed radius33 before destruction. CPU/owner movement/root motion/rigidbody dynamics, actor resets and runtime timer extension are fixture-controlled and restored; no direct hit/damage invocation. FlowerBeam's nested Follow stays inside inactive Beam/FastWaterAtk ancestors in8cases; not counted as live homing.
- FINAL `Evidence/offline-awakejumps-probe-20261016-030659.json` PASSED/errors=[]:all5 authored actual UGUI awake evade routes entered/recovered while awakening/decharge remained active. Raven roll->ShadowSharkAtk, JumpR->ShadowWolfAtk and JumpL->ShadowBirdAtk all selected the opponent and matched native yaw; Shark/Wolf each had1contact and Damage1, Bird had0contact/Damage0 in this layout. Maya JumpL/JumpR created stationary flowerBonfire traps with horizontal travel0. Native Awake spawn offsets included; charge accelerated, target movement controlled. Five captures inspected directly; captures occur after recovery, not complete VFX/trajectory footage. Existing exposure/UI clipping/approximate shaders remain visual limits.
- FINAL `Evidence/offline-movingtarget-probe-20261016-030909.json` PASSED/errors=[] on same DLL: actual Sora UpAttack then UpAttack2,145live projectile samples, target travel2m/drift0, rotation error0, physical contact/damage event at frame1279, Damage0->1. Movement fixture and original save restoration recorded. Exact current acceptance `Evidence/moving-target-flight-acceptance.json`; older ten-route Sora regression remains historical on its own DLL.
- Main hash-bound manifest: `Evidence/flight-matrix-acceptance.json`, linking all final runs/build/DLL/native listings/46prefab assets/validation sources/5awake captures and current MovingTarget acceptance. Reproduce one fresh build, sequential FlightMatrix, AwakeJumps and MovingTarget runs, audit-moving-target with explicit final runtime/build, then audit-flight-matrix with explicit matrix/awake/build/moving paths. Read `Knowledge/flight-matrix-reconstruction.md`. Never build over a running owned player or interrupt fixture restoration.
- Observer history:023147 awake failed uninitialized target baseline/missing authored1m spawn offset;023728 passed five routes on provisional DLL. Matrix024100 completed368cases with48 observer failures from delayed MoveWater yaw reset, disabled legacy components, inactive/stop-transition sampling and registry expectations. Corrected observer compositions rather than combat tuning. Focused025625 passed32cases before final025820. No unaccepted early report is substituted for final evidence.
- Scope/next: these profiles originate from the12-fighter closure, but matrix actors are held Sora/Amon and no natural all-roster moving-target hit acceptance is claimed. Preserve once-only, curved, local, area/trap and raw-dot semantics. Next natural Light UpAttack2 impact, real simultaneous water/snake geometry, free CPU/knockback/height/edge interactions, close-range awake gating/cooldowns, natural charge and attack-caused match wins. Original visual/runtime parity, performance, online and Android remain unaccepted. Previous120-route/12-special/12-awakened-normal checkpoint below retains its own older DLL identity. RAG rebuilt after this checkpoint.

<!-- FLIGHT_MATRIX_CURRENT_END -->

<!-- ABILITY_AIM_CURRENT_START -->
# PREVIOUS: AIMING CHECKPOINT AND FIRST MOVING-TARGET IMPACT

- User priority: every playable fighter's skills should aim toward the opponent like the supplied videos. Applied REA skill with retained original ARM64/metadata/corrected Capstone and Unity YAML; no connected REA server tools. These are local native observations and reconstructed-player tests. Static map covers12fighters/132normal Attack+Special states/164referenced prefabs/109source owners. Read Knowledge/ability-aiming-reconstruction.md and Knowledge/ability-aiming-map.md.
- Restored/corrected18owners: LookAt, HomingAttack, LockToTarget, RotateToOpponent, TurnAttack, FollowOwner, Rotate, MoveToTarget, MoveRelativeToRotation, MoveObjectLocal2, CreateObject, CreateObjAnim, EnableByDistance, ChangeStartRotation, DropAtk, DropWhenOwnerWeak, PingPongMove, RotateByAngle. Preserve authored once-only aiming, curved turns, local/world paths, owner-following, weak descent and stage-boundary semantics; do not replace every move with continuous homing. Original prefab/meta/export data unchanged.
- Current normal projectile steering is Follow+GoForward (already restored and now checked):55enabled Follow reference occurrences across45states/33prefabs;85GoForward occurrences across62states/45prefabs. HomingAttack/LockToTarget inventory15/6 profiles has ZERO references in the132state closure: isolated profile restoration is not a newly homing normal moveset. Explicit compatibility adaptation recognizes fighter9/8 ownership as well as PlayerAtk11 for those legacy owners; reference guards/debug-log omission and public tween entry adaptation are documented.
- FINAL `Evidence/offline-abilityaim-probe-20261016-012136.json` PASSED:120/120 actual UGUI normal routes and live orientation checks;96 root/facing cases;60 homing,24 lock and12 curve profile fixtures; all grouped Follow/motion/local cases. Maximum facing error0.02797646°. Live Follow assertions accumulated48 move/super entity samples (not globally unique). CPU disabled, synthetic stationary target placements, fixture restoration recorded.
- FINAL `Evidence/offline-specials-probe-20261016-013210.json` PASSED on the same DLL:12 awakenings,12 natural decharge transitions,12 phase1 supers and12 normal attacks executed during active awakening. Each entered/recovered from a real normal state with awakening active; maximum facing error0.02797646°. Supers have separate361–374 frame facing samples; maximum error0.02797646°. Charge accelerated by fixture. Maya healing/wings and Earth moving rocks passed; fixture restoration recorded.
- REA/YAML event audit `Evidence/awakened-attack-event-audit.json`: `CreateAwakeAtk` is linked to Raven roll/JumpR/JumpL and Maya JumpR/JumpL only. This is not proof other fighters lack every awakening bonus. Dynamic aim/impact of those five jump projectiles remains pending; one awakened normal route per fighter was tested.
- Latest build `Evidence/windows-build-validation-moving-target-20261016-021430.json`: Succeeded,0 errors/35 warnings. Gameplay DLL SHA256 `6b67e19907e4fc2ffdf4894eeddf86a2f26cda6962b9467fdb8ba7520c2776ab`. This continuation changes the opt-in validation harness only; the prior120-route and12-special acceptance remains bound to the earlier `93a66d4f...` assembly and archived build. Player `Builds/Windows/TrueMastersOffline.exe`. Controls: arrows/Z attack/X evade-shield/C super; alternate WASD/left click/LEFT Shift evade-shield/Space super.
- Repaired black particle quads exposed by Sora capture: both Custom/AdditiveTintable exports were opaque dummy passes. Approximate Built-In additive/tinted/transparent passes now preserve vertex alpha and authored tint, no depth write, optional soft particles/fog. Original GPU shader code NOT recovered. Shader backups Evidence/ability-particle-shader-backup; reproducer Tools/restore-ability-particle-shaders.cjs; manifest Evidence/ability-particle-shader-reconstruction.json. Final Sora dust shows no opaque black rectangles. Current acceptance has120 move captures,12 reviewed contact sheets,12 awakened and12 super captures. Captured poses can precede delayed effects: do not infer missing attacks from quiet frames.
- Hash-bound acceptance `Evidence/ability-aiming-acceptance.json` includes both final runtimes/build/DLL,20 source owners (18 restored + Follow/GoForward), native listings/iterators, event audit, shader sources/manifests,120 move/12 awakened/12 super captures and twelve reviewed current-build normal contact sheets. Reproduce with one fresh build, sequential AbilityAim and Specials runs, `Tools/Build-AbilityContactSheets.ps1`, then `Tools/audit-ability-acceptance.mjs` with explicit report paths. Never build over an owned running player or interrupt a fixture before restoration.
- History: Sora000113 failed observer UGUI dispatch from end-of-frame; resume normal coroutine phase before input. Sora000519 passed earlier10routes. Baseline all12 report001030 passed120moves but predates live Follow assertions and particle repair; its black-quad images are NOT final visuals. Current012136/013210 runs supersede002256/003331. Earlier generic probe limitation strings predate the additional orientation assertions; use exact observations and current scope boundaries.
- Limits: one awakened normal direction per fighter, not every direction/alternation; all-roster targets were synthetic and stationary. The controlled moving-target impact below covers one Sora route. No exhaustive moving-target/height/edge/interrupt/hit outcome, natural charge, original VFX/shader/audio parity, performance, online or Android acceptance. Supplied videos are from an older five-element build. Shader repair is approximate. Full game unfinished.
- Next within this user priority: test moving-target/height/edge/interrupt flight and impact, natural Light UpAttack2 impacts, simultaneous water/snake geometry, then attack-caused match wins; dynamically test Raven/Maya awake-jump projectiles. Preserve area/trap/self-heal semantics. Menus/audio/advanced shadows/Android remain lower priority; online/backend deferred. Rebuild RAG after documentation updates.

- MOVING-TARGET FLIGHT PASSED: `Evidence/offline-movingtarget-probe-20261016-021459.json` and `.flight.json`. Actual Sora UpAttack then UpAttack2 UGUI routes; original AirAtkStrong2 uses Follow+GoForward.153 live frames,10 corrective turn frames (9 while the CPU moved), target path2m, target drift0 and maximum native-contract rotation error0. At frame1274 the projectile's actual OnTriggerEnter contact and CPU onDamageRecived both occurred; Damage0->1. No forced overlap/direct damage/forced attack state. CPU locomotion/root motion/rigidbody dynamics are controlled temporarily; collider, Animator reactions, DamageOnCollision and projectile owners remain active. Root-facing, move recovery, movement-fixture cleanup and save restoration passed. Reproduce `Tools/Run-OfflineProbe.ps1 -Scenario MovingTarget`, then `Tools/audit-moving-target.mjs` with explicit runtime/build paths. Hashes/native provenance are in `Evidence/moving-target-flight-acceptance.json`; read `Knowledge/moving-target-aim-probe.md` for scope. Old020414 assertion/unsupported position inference is superseded; its trace did not record enough positions to attribute the failure. Removed that unconditional diagnostic gate from ordinary Moves/AbilityAim. Remaining moving-target/height/edge/interrupt acceptance across other attacks is pending; original runtime parity is not claimed.

- Current-build ordinary Sora regression `Evidence/offline-abilityaim-probe-20261016-021618.json` PASSED/errors=[]:10/10 actual normal routes, maximum facing error0.02797646degrees, prior component-profile groups and save restoration. Same `6b67e199...` DLL as MovingTarget. Both exact runtimes are linked in `Evidence/moving-target-flight-acceptance.json`; no new full12-fighter regression is claimed for this validation-only build.


<!-- ABILITY_AIM_CURRENT_END -->

# PREVIOUS: NATIVE SHADOW QUALITY AND TWELVE-FIGHTER GPU ACCEPTANCE

- Restored ShadowController from retained original native evidence using the REA skill workflow (local Capstone/metadata/YAML, not connected REA server Evidence). Start subscribes to GraphicsManager.graphicsChange, binds DrawTargetObject direction to the MainLight TAG, and applies quality immediately. Native exact equality enables shadows only for userQualityGraphics=1 (MIN); 0/2/3 disable them. Inactive controllers retain their subscription; OnDestroy removes it. Retained publisher/null guards are explicit reconstruction safeguards.
- Corrected DrawTargetObject projection: look along light.forward with the selected target axis as up. OnEnable preserves the target offset cached by Awake rather than recaching after target movement. Reproducer Tools/restore-shadow-core.cjs includes both corrections. No original export/prefab edits.
- FINAL Evidence/offline-shadowroster-probe-20261015-234058.json passed/errors=[]: all12 normal fighters,60 quality cases,12 inactive-target-follow cases,12 automatic GPU animation cases and25 cookie checks. Maximum relative-offset error6.848052E-07. Direction assertions passed; listener creation/destruction cleanup passed. Thirteen authored controllers include Sora's dormant alternate, which remains inactive and is NOT alternate/awakened runtime acceptance.
- FINAL ordinary Graphics regression Evidence/offline-graphics-probe-20261015-234404.json passed/errors=[]: Vinea controlled BasicAttack changed52bones/1770pixels via automatic rendering. Texture release/recreation and quality/follow checks passed. Both final scenarios record actual fixture restoration.
- Current build Evidence/windows-build-validation-shadow-quality-20261016-044023.json: Succeeded,0errors/33warnings. Log Evidence/unity-windows-build-20261015-233939.log. Gameplay assembly SHA256 11c5bf91564a87b8978c0afd036a6ef070aafb8ee2285cf898c9435f1952c618. Player Builds/Windows/TrueMastersOffline.exe. Controls: arrows/Z attack/X evade-shield/C super; alternate WASD/left-click attack/LEFT Shift evade-shield/Space super.
- Exact source/native/build/runtime/image hashes: Evidence/shadow-quality-roster-contract.json and Evidence/dynamic-shadow-core-contract.json. Reproduce with ShadowRoster and Graphics scenarios, then Tools/audit-shadow-quality.mjs and Tools/audit-shadow-core.mjs using explicit final report/build paths. Knowledge/shadow-quality-reconstruction.md explains contract and boundaries.
- Historical failures/provisionals retained: roster232922 failed tag/dormant observer assumptions; roster233218 and Graphics233515 predate explicit direction/follow assertions and exposed a visually incorrect orientation. They are superseded by234058/234404. Older ordinary Graphics had not wired CaptureFixture despite restoration claims: those claims were unsupported. Snapshot wiring and quality/frame-cap preference capture are now explicit; old GPU observations retain their bounded value.
- Scope: quality tests dispatch actual preference/event route, not resolution UI buttons. GPU checks use controlled normal-form BasicAttack poses with animation events disabled/restored. Basic Built-In shadow plugin remains partial; mask/receiver shaders approximate, advanced blur/mips/supersampling/view clipping gated/incomplete, DrawSceneObject/MipmappedShadowFallback unfinished. No original visual parity, full movesets, performance or Android acceptance.
- Next: natural Light UpAttack2 impacts and simultaneous water/snake geometry, natural attack-caused match completion, remaining active gameplay stubs, options/menu/audio/visual work and Android. Neutral shield and broader physical controls/pause/focus coverage remain pending. Full game unfinished; online/backend deferred. RAG rebuilt after this checkpoint.

# PREVIOUS: BASIC DYNAMIC SHADOW CAPTURE PASSED GPU/AUTOMATIC-RENDER ACCEPTANCE

- Resumed after explicit user continua. Rebuilt cleanly after interrupted build. Restored authored basic Built-In ShadowTextureRenderer/DrawTargetObject path: per-owner textures/materials, hierarchy/layer mask capture, target following, camera sync and texture cleanup/recreation. Opaque/transparent/cutout mask shaders are new approximations; receiver shader was already approximate. No prefab/meta/original-export edits. Backups Evidence/shadow-core-backup; reproducer Tools/restore-shadow-core.cjs.
- Scope is PARTIAL plugin reconstruction: all12fighter prefabs inventory13basic profiles, no blur/mips/supersampling/view clipping. Advanced helper bodies, DrawSceneObject, MipmappedShadowFallback and ShadowController coordination remain incomplete. Unsupported profiles are gated off. No original runtime/visual parity, all-roster GPU or performance acceptance.
- Final `Evidence/offline-graphics-probe-20261015-232058.json`: passed/errors=[] on Diana/Vinea scene. Two distinct128x128 GPU cookies bound correctly; dark/white pixels1578/14738 and991/15324. Controlled Vinea BasicAttack .1/.6 changed52bones and1741pixels with AUTOMATIC camera rendering, no Camera.Render call. Animation events disabled during fixture and restored. Texture disable released RT/stopped camera; enable recreated RT. Original save/preferences restored.
- Captures inspected: automatic pose1/pose2 differ; scene contains projected contact shadow. Receiver enabled/disabled captures are qualitative, not registered pixel A/B, because actor pose settles between them. Exact report/source/shader/image hashes: `Evidence/dynamic-shadow-core-contract.json`. Static inventory plus selected final runtime: Tools/audit-shadow-core.mjs with exact final runtime/build report args.
- Current Windows build `Evidence/windows-build-validation-shadow-core-20261016-042030.json`: Succeeded0errors32warnings; log unity-windows-build-20261015-231950.log; gameplay assembly SHA256 b8dc399ba2439279e11c62e9dabdc7e4790dcdd084218b8f4f7276f14effd40e. Last source change was validation-only removal of manual Camera.Render. Controls remain arrows/Z/X/C and WASD/left-click/LEFT Shift/Space.
- Earlier Graphics222728/223137 animation failures retained: invalid observer selected auxiliary animator Valen, state/time0 and changedBones0. Selecting parent Animator with an actual runtime controller established valid poses. Provisional report231818 passed with explicit camera Render; final232058 additionally proved automatic capture. Do not attribute observer failures to gameplay skinning or call provisional/manual run final acceptance.
- Read Knowledge/dynamic-shadow-core.md for native method identities, reconstruction choices, limitations and captures. REA skill applied using retained original ARM64/metadata/Capstone and YAML; no connected REA server tools, setup or new package acquisition. RAG rebuilt after checkpoint.
- Next: all-roster GPU/quality-toggle coordination and advanced shadow features if required, remaining active gameplay stubs, natural Light UpAttack2 impacts and concurrent water/snake geometry, attack-caused match completion, visuals/audio/menu polish and Android. Full game remains unfinished; online/backend deferred.

# HISTORICAL USER PAUSE: RESUMED AT USER REQUEST

- User paused development here; a subsequent explicit continua resumed it. Owned Editor build PID10980 was stopped; leave Unity Hub helper alone. Log Evidence/unity-windows-build-20261015-223335.log is an interrupted build, not acceptance. Build outputs may be incomplete; rebuild before launching a player.
- Product changes on disk: partial authored Built-In dynamic shadow capture in ShadowTextureRenderer and DrawTargetObject; approximate opaque/transparent/cutout mask shaders. Original backups Evidence/shadow-core-backup. Tools/restore-shadow-core.cjs reproduces from backups. Advanced blur/mips/supersampling/view clipping remain unsupported; numerous private helpers remain stubs. Read Knowledge/dynamic-shadow-core.md.
- Last completed build Evidence/windows-build-validation-shadow-core-20261016-033055.json succeeded0errors32warnings, assembly464fda603cdf8e936e7a485d21d3349de43fb9355ed6138f980bb7af8b2d52bb. This predates the latest validation-only animator selection fix. Current output may have been touched by the interrupted build.
- Runtime Graphics222728 and223137 FAILED animation assertion. Both generated distinct128x128 silhouette cookies bound to each projector. Instrumented223137 selected auxiliary animator Valen with state hashes0/0, normalized times0/0 and changedBones0; thus it did not establish distinct combat poses. No dynamic animation/lifecycle or receiver screenshot acceptance yet.
- Latest unexecuted correction: OfflineDynamicShadowProbe now chooses a parent Animator with runtimeAnimatorController, verifies BasicAttack states and changed bones, then checks changed pixels and resource recreation. This source fix has not completed a build/probe. Resume by rebuilding with Tools/Build-OfflinePlayer.ps1, confirming fresh success, then Run-OfflineProbe.ps1 -Scenario Graphics. Inspect results before modifying gameplay capture code; earlier failure was an invalid observer pose fixture.
- Static Tools/audit-shadow-core.mjs inventoried12fighters/13profiles, all within supported basic profile. Evidence/dynamic-shadow-core-contract.json currently has STATIC data only; do not report runtime success. Run audit with exact passing runtime/build paths only after acceptance. New native listings and slot files retained. No original exports or prefab data changed. REA skill applied using local retained binary/metadata/Capstone, no connected REA MCP output.
- Shadow documentation draft exists but final success/failure ledger, RAG update and exact current-source checkpoint remain pending. Earlier fully accepted collider/animation milestone below remains historical and valid for its own assembly.

# PREVIOUS: ANIMATION WINDOW AND COLLIDER GEOMETRY RESTORED; FLOWER ROUTE RESOLVED IN BOUNDED CASE

- Restored `DisableColliderAnim` from native: scaled delay; wait .02 while own layer1; source layer8 selects HostPlayer animator, otherwise GuestPlayer; poll every .05. Only Light/UpAttack2 changes own box: disabled strictly at normalizedTime (.2,.45), enabled outside within matching state; nonmatching state/element leaves geometry enabled state unchanged. 25 prefabs / 26 serialized instances inventoried. Six controlled animation-window cases, both delayed/layer-binding branches and both state/element filters passed. Actor elements/states controlled, animation events disabled/restored: NOT natural Dante/Agata input acceptance.
- Maya flower pending assumption corrected for Grass UpAttack: its original clip creates child FlowerBeam at3.3666666 and damaging FlowerSpecial at3.4833333. Actual upward InputReader + authored UGUI attack callback produced child FlowerBeam with disabled root collider/Description across140 observed frames until natural expiry; separate FlowerSpecial had active damage/collider. No manual enable in this route. This supports a visual/damage split, not a missing root enable function. The older isolated producer fixture still enables FlowerBeam explicitly and remains separate coverage. No prefab edits or invented enable owner.
- Restored `ChangeColliderCenter` / `ChangeColliderScale`: default duration1; disabled until delayed Invoke; center/BoxCollider.size interpolated from authored values; elapsed increments AFTER assignment; strict elapsed>duration schedules Destroy(this), then finishes native update. Nine authored configs from WaterWaveAtk/WaterAtkStrong/ShadowSnakeAtk passed isolated live-component geometry/lifecycle checks, max/final errors0. These do not establish concurrent writer ordering or natural impact geometry.
- Final `Evidence/offline-mayaproduction-probe-20261015-221526.json`: passed/errors=[], includes natural flower observation, 6 window samples, 9 geometry cases, previous14 physical collision branches, 4 producer cases and2 floor beams. Save/preferences restored. Earlier reports220829/221117 are superseded by this final source checkpoint.
- Current Windows build: `Evidence/windows-build-validation-collider-geometry-20261016-031515.json`, Succeeded, 0 errors / 27 warnings. Log unity-windows-build-20261015-221440.log; gameplay assembly SHA256 a68322f582c151a460047b77a4cb3db9b67f2a4213a94a329c166499ed4630fd. Controls remain default arrows/Z/X/C and alternate WASD/left-click/LEFT Shift/Space.
- Evidence: `Evidence/animation-collider-native-contract.json` and `Evidence/collider-geometry-native-contract.json`; explanations `Knowledge/animation-collider-reconstruction.md` and `Knowledge/collider-geometry-reconstruction.md`. Reproduce MayaProduction after build and run Tools/audit-animation-collider.mjs / Tools/audit-collider-geometry.mjs with explicit final runtime + archived build paths. RAG rebuilt.
- REA skill applied using retained native input, corrected Capstone listing, resolved generic/literal slots, binary float constants, format-aware YAML and reconstructed runtime checks. No connected REA MCP tools were available; these are local observations, not REA server Evidence. No original export edits/setup/download or external messages.
- Next: natural Light UpAttack2 input/impact acceptance and real simultaneous ShadowSnake/water geometry, remaining active stubs, dynamic shadow texture generation, natural attack-caused wins, menus/options/graphics/audio and Android. Full game remains unfinished; no original runtime/visual parity or online acceptance. Do not reintroduce the old assumption that FlowerBeam must have an active damage root in the accepted Grass UpAttack route.

# PREVIOUS: COLLIDER DISABLING RESTORED FROM NATIVE; FOURTEEN PHYSICS CASES PASSED

- Restored `DisableColliderOnCollition` and `_LM` from corrected ARM64 listings and resolved metadata slots. The first filters incoming layers 8/9 different from own layer, uses OWN Description when onlyByPlayers=true, then disables serialized col. LM ignores incoming collider and tests its OWN BoxCollider tag Attack/Player. Both Start methods are genuine RET. Twelve serialized prefab references were inventoried with Unity YAML parsing.
- Deliberate adaptation: `LeftWaterAtk2` authors col fileID 0 in both original export and editable project. Skip the null reference rather than reproducing the native exception; no invented fallback or prefab edit. This is not native parity. A dedicated missing-reference fixture passes.
- `Evidence/offline-mayaproduction-probe-20261015-220052.json`: passed/errors=[], 14 real-physics synthetic branch cases, plus existing 4 recovered Maya producer cases and 2 floor-beam motion checks. Physics layer-pair overrides are restored. CPU disabled, save/preferences restored. FlowerBeam still explicitly enabled by fixture; natural activation remains unaccepted. Earlier 13-case report215824 predates the null safeguard and is historical.
- Current Windows player build `Evidence/windows-build-validation-collider-disable-20261016-030029.json`: Succeeded, 0 errors / 25 warnings; log `Evidence/unity-windows-build-20261015-215958.log`. Gameplay assembly SHA2562429660093c1c48b88c13e59460c032ece40c3622ff5832e6a3270437cd06a54. Latest build includes left-Shift-only controls.
- Evidence/contract: `Evidence/collider-disable-native-contract.json`. Reproduce audit with `node Tools/audit-collider-disable.mjs Evidence/offline-mayaproduction-probe-20261015-220052.json Evidence/windows-build-validation-collider-disable-20261016-030029.json`; runtime scenario remains MayaProduction. Read `Knowledge/collider-disable-reconstruction.md` for provenance, native constants, adaptation and coverage limits.
- REA skill applied from C:/Users/Jose/.agents/skills/reverse-engineer-anything/SKILL.md. No connected REA MCP tools were available; reused retained original binary, local Capstone/corrected map, metadata decoders and ISIL. These are local-tool observations, not claimed REA server Evidence. No setup, download, original-export edits or agent delegation.
- Next: natural FlowerBeam activation ownership, remaining active stubs such as DisableColliderAnim where actually used, dynamic shadow texture generation, natural attack-caused wins, menus/options/visuals/audio and Android. Remaining controls coverage includes neutral shield and broader roster/pause/focus. Full game remains unfinished; online/backend remains deferred.
- Older CURRENT statements below are historical snapshots. Consult this checkpoint for latest binary and collision implementation.

# PREVIOUS: ALTERNATE EVADE/SHIELD USES LEFT SHIFT ONLY

- Removed RightShift from InputReader and GameController. Default Z/X/C remains; alternate WASD, left click attack, left Shift evade/shield, Space super.
- Current Windows player rebuilt successfully: Evidence/windows-build-validation-controls-leftshift-20261016-025334.json, 0 errors / 25 warnings. Assembly SHA256: a5522362d4371d0aa830647ea32ea323474e7343ccb148d1116308b910039490. Build log: Evidence/unity-windows-build-20261015-215304.log.
- The previous OS probe below already exercised left Shift. The latest alias removal has build acceptance only; the previous runtime report and its hashes identify the earlier assembly.

# PREVIOUS: Z/X/C AND LEFT-CLICK/SHIFT/SPACE CONTROLS PASSED WINDOWS INPUT PROBE

- Current bindings: arrows move, Z attack, X evade/shield, C super; alternate WASD move, left click attack, Shift evade/shield, Space super. Only left Shift is bound. The preceding OS probe exercised left Shift; the removal of the right Shift alias is a subsequent source change.
- Rebuilt Windows player is current: `Evidence/windows-build-validation-controls-20261016-024758.json`, Succeeded, 0 errors / 25 warnings. Build log: `Evidence/unity-windows-build-20261015-214727.log`. Gameplay assembly SHA256: ff47aa8fb7ee511de062e70e4f9324a6d6f25a6dfce7252a0cf39ef74db39779.
- `Evidence/offline-desktopcontrols-probe-20261015-214806.json`: passed/errors=[], both layouts on fresh Sora scenes, eight cardinal movement/release cases, attack, left directional evade, awakening and phase1 Special through actual Windows OS input. CPU disabled; super charge accelerated. Toggle persisted across scene reload without inversion changes; hover did not attack. Both option screenshots inspected and readable. Original save/preferences restored.
- `Evidence/offline-menu-probe-20261015-215010.json`: passed/errors=[] on the same assembly. Existing dpad/joystick movement, release, inversion, settings and purchase fixture regression passed. Exact source hashes, reports and screenshots: `Evidence/desktop-controls-acceptance.json`.
- Pending controls coverage: neutral shield, other evade directions, remaining fighters, natural charge, physical focus-loss/pause and OS checkbox click. Visual clipping of MUT/INV and overexposure remain. Full offline reconstruction and Android remain unfinished. Next controls work should cover these gaps before claiming all controls/roster acceptance.
- The sections below preserve earlier checkpoints; their pending rebuild statements and old bindings are historical.

# HISTORICAL: DESKTOP CONTROLS IMPLEMENTED BEFORE REVALIDATION

This continuation supersedes the build-only keyboard section below. User priority remains working desktop controls before other reconstruction work. Full game is still unfinished.

- Current bindings in `InputReader`: default arrows move, Z attack, X dodge/shield, C super; alternate WASD move, left click attack, Shift dodge/shield, Space super. Either Shift key is accepted. Alternate super is rising-edge-triggered. Earlier real OS acceptance below and in `Evidence/desktop-controls-acceptance.json` covers older bindings only; this remap has not been rebuilt or exercised.
- `Builds/Windows/TrueMastersOffline.exe` still contains an older mapping until rebuilt. Updated `Tools/unity-validation/OfflineDesktopControlsProbe.cs` and the evidence generator to exercise and require the current bindings on the next acceptance run.
- Fixed product owners: InputReader retains screen inputs separately, combines desktop inputs, samples before MobileInputs, suppresses unpressed desktop touch-hover state, ignores mouse actions over actionable UI, and clears controls on pause/focus loss/disable. GameController guards authored PointerEnter attack/dodge callbacks against unpressed desktop hover. JoystickSettings uses an independent ToggleEvent (clone had inherited Invert), persists AlternateKeyboardControls, widens and relocates the new WASD + RATÓN option and extends BG2. Both new-option screenshots inspected: readable and inside panel, alternate checked state visible. Existing MUT/INV clipping and overexposure remain visual gaps.
- Historical actual OS input report: Evidence/offline-desktopcontrols-probe-20261015-212735.json, passed/errors=[], tested the previous arrows/X/Space/Z and WASD/left/right/middle mapping on two fresh Sora scenes. Treat as evidence for the prior mapping only. Previous gameplay assembly58d126df01b2452accb83cf0be52329c002b4e95e1cc355d25de8c8635a19b3d. Rebuild and repeat the OS probe with the current bindings before recording new acceptance.
- Regression Evidence/offline-menu-probe-20261015-213006.json passed/errors=[] on same assembly: dpad/joystick actual pointer movement/release, inversion independence, mode callbacks, settings poses, stage purchase/JSON fixture. Final build Evidence/windows-build-validation-controls-20261016-023127.json succeeded0errors/25warnings, log unity-windows-build-20261015-213057.log. Only validation runner Windows guard changed after the OS/menu probes; product owner sources unchanged. Current exe/data are updated.
- Reproduce Windows input probe: Tools/Run-OfflineProbe.ps1 -Scenario DesktopControls. It temporarily shows/focuses its own player window, sends OS key/mouse events with mapped scan codes, verifies focus before each press and releases injected inputs in finally. Do not run concurrent players writing shared saves/preferences. Validation helper is Windows-only; call is excluded from other target compilation paths. Earlier failed probes021730/212340 retained; final scan-code/focus driver changes preceded acceptance, exact old phase1 delivery cause not isolated. No special rules were modified merely for the observer.
- Documentation/evidence: Knowledge/keyboard-controls.md; Evidence/desktop-controls-acceptance.json records exact paths, hashes and limits; Tools/document-desktop-controls.cjs regenerates from explicit report paths. RAG updated. System clock moved5hours during session: select exact path/launch/hash, not newest timestamp alone. No owned Editor/player remains; Hub helper is not an Editor and must be left alone.
- Next work: expand physical controls/neutral block/dodge directions across remaining roster and pause/focus cases, then natural attack-caused matches. Other documented priorities remain natural FlowerBeam activation and DisableColliderOnCollition stub, remaining active stubs, dynamic shadow texture generation, menus/economy/options, visuals/audio and Android. Online/backend remains deferred.

# PREVIOUS ACCEPTANCE - 2026-10-16 06:58 UTC

## User-prioritized keyboard controls - 2026-10-16 07:12 UTC

This is the newest implementation. User asked for default arrows/X/SPACE/Z and an optional alternate A/W/S/D plus left/right/middle mouse bindings.

- `InputReader` now reads the requested desktop bindings after countdown unblock: arrows/WASD supply movement axes, X/left click holds normal attack, SPACE/right click routes to the existing Jump animator action, and Z/middle click triggers the charged super on press. Actions use existing GameController APIs; movement retains InputSet/MobileInputs. `JoystickSettings` clones the existing inversion toggle at runtime as a persisted “Teclas alt.” option. Key: PlayerPrefs `AlternateKeyboardControls`. Inversion, joystick, dpad and mobile controls are preserved; desktop bindings skip mobile platforms.
- Unity6000.6.5f1 Windows Development build succeeded 0errors/23warnings: `Evidence/windows-build-validation-20261016-071122.json`, log `Evidence/unity-windows-build-20261016-021043.log`. Current exe/data build outputs updated. This was build-only validation. No live keyboard/mouse probe was run; real event delivery, toggle visibility and intended dodge animation are unaccepted.
- Contract rationale: scene `InputReader` starts with blockInputs=true; Counter disables after GO and enables `MobileInputs` on controlled fighter. `InputReader.Update` must not dispatch during countdown. Controller default arrows preserve requested default and do not depend on changing global InputManager axes. Existing Space semantics call GameController.Jumping -> Animator bool Jump; validate whether this represents the desired dodge before semantic changes. New runtime-created Toggle clones the authored invertedToggle at an additional row; visual interaction has not been inspected.
- Knowledge: `Knowledge/keyboard-controls.md`. Next priority: build/run live desktop control probe and inspect settings toggle in-game. All other offline completion, graphics/shadows/audio and Android remain unfinished. Keep away from Hub processes.

This section supersedes the licensing blocker and unexecuted Maya fixture below. User confirmed Personal license in Unity Hub; retry worked without licensing/configuration changes. Normal Unity Editor builds are AVAILABLE. Do not ask user to activate again on historical exit198 alone.

- Latest build `Evidence/windows-build-validation-20261016-065640.json`: Succeeded,0errors/23warnings. Current EXE Builds/Windows/TrueMastersOffline.exe; assembly SHA2562f7417dd77fd43c569c7de8d38835dca362d1d019ffaa125ce374de06e4e477d. Full game remains unfinished.
- Final `Evidence/offline-mayaproduction-probe-20261016-015656.json`: passed,errors=[]. Four actual recovered-prefab collision producers reject missing Description and create correct child with target position/rotation errors0 and no parent. Layer0/synthetic remote targets; FlowerBeam root collider explicitly enabled in fixture because isolated prefab does not self-activate. Natural flower activation remains pending. Both floor beams48frames/5reversals, max lateral errors.00002658367/.0000270009. Actual direction/timer captured at Update entry, displacement and reversal checked in LateUpdate. Save/preferences restored. No product source changes after the two native owners restored earlier.
- Corrected validation assumptions only: await leaf root trigger activation; FlowerBeam producer fixture explicitly activates root collider; find ZigzagController on actual child; capture state at operation entry instead of assuming sampler starts on owner's first frame. Previous failures retained. Earlier passing015512 report had its final limitation overwritten by Specials text; final015656 retains correct Maya scope after runner guard fix.
- Native contract regenerated with explicit final probe; Knowledge/RAG updated. No owned Editor/player remains; leave Hub/helper processes alone. No original-runtime, natural aiming, full flower lifecycle, complete combat or visual fidelity acceptance.
- Continue next with natural FlowerBeam activation/creator contracts and active collision owner DisableColliderOnCollition (still stub), plus other concrete pending items in the checkpoint below. License is no longer a blocker.

# PREVIOUS CONTINUATION - 2026-10-16 06:50 UTC

This continuation supersedes the Maya producer pending item below. Full editable offline reconstruction remains unfinished.

- Restored `sumonOnCollider` (Description filter, local creation at incoming collider pose, original Photon ownership/network branch represented) and `ZigzagController` (native defaults, half-period initial timer, per-Update lateral movement, timed reversals). LeafFrontHit/LeafAtkL/R produce SlashFX; FlowerBeam produces GameObject/CircleExplotion. No scene/prefab/material/settings modifications. Online remains unsupported against exported Photon stubs.
- Evidence: `Evidence/maya-production-native-contract.json`; reproducible format-aware `Tools/audit-maya-production.mjs`; explanation `Knowledge/maya-production-reconstruction.md`. Corrected native dumps were generated using the retained VA/raw map. REA skill read from C:/Users/Jose/.agents/skills/reverse-engineer-anything/SKILL.md. No setup/configuration/download repeated. No agents spawned.
- Full gameplay C# plus new helper compiled using installed Roslyn and retained Unity player response file: exit0, no C# errors, existing warnings. Log `Evidence/maya-production-compile.log`. Output under Evidence is a compile artifact only, NOT installed into the Windows player. Earlier helper GetInstanceID obsolete errors were corrected to GetEntityId.
- **EDITOR LICENSE BLOCKS BUILD/RUNTIME:** attempt PID10368, `Evidence/unity-windows-build-20261016-014129.log`, stopped before compilation with no valid Unity Editor license, exit198. Existing `windows-build-validation.json` and Windows EXE are historical, NOT these changes. No fresh runtime acceptance. User must activate Editor license through Unity Hub before another normal build. No credentials/configuration were changed to address licensing.
- Prepared `OfflineMayaProductionProbe.cs`, builder copy and launcher scenario `MayaProduction`: real physics on recovered prefab instances with synthetic Description/no-Description targets and target pose/child parenting checks, plus actual floor beam lateral samples/reversals. Helper is UNEXECUTED. Uses default layer0 self collision in fixture, since actor self collision is disabled. No natural aiming or directional input coverage implied.
- Next: after license activation, build using Tools/Build-OfflinePlayer.ps1, confirm fresh build completion/report then run Tools/Run-OfflineProbe.ps1 -Scenario MayaProduction. Refine observer assumptions if necessary without altering native gameplay rules. Other pending work (awakened/defense routes, attack-caused wins, active stubs, shadows, menus, visuals/audio, Android) remains in the checkpoint below.
- Prior gameplay source manifest `Evidence/working-gameplay-source-checkpoint.json` predates these two script changes; retained as historical. New contract records current SHA256 for both owners. RAG rebuilt after this continuation.

# PREVIOUS CHECKPOINT - 2026-10-16 00:30 UTC

This section supersedes implementation/pending statements in the historical sections below. User requires REA skill plus RAG, editable Unity reconstruction, OFFLINE FIRST. THE COMPLETE GAME IS STILL UNFINISHED. Continue implementation and verification; do not repeat APK acquisition, setup or full exports.

## Workspace, evidence and authority

- Actual game root: C:/Users/Jose/Desktop/TrueMastersReconstruction. Unity project: UnityProject; gameplay: UnityProject/Assets/Scripts/Assembly-CSharp. REA source repository is separate: C:/Users/Jose/Desktop/REA; no REA source changes were made.
- Skill applied: C:/Users/Jose/Desktop/REA/.agents/skills/reverse-engineer-anything/SKILL.md. It permits independent workers; all three current workers completed and no longer write files. REA MCP is absent; retained native evidence and local tools were reused, without setup/configuration changes.
- Unity6000.6.5f1: C:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Unity.exe. Windows/WebGL installed; Android module absent. Current Windows launcher: Builds/Windows/TrueMastersOffline.exe, keep matching _Data directory.
- Original APK/videos: C:/Users/Jose/Desktop/true masters videos gameplay y men?. Videos have sampled contact sheets, NOT exhaustive frame/audio acceptance. Original APK lacked ABI split; validated supplemental same-version native input is retained. AssetRipper exports under Evidence/RecoveredUnityProject and Evidence/RecoveredUnityProject-WithNative are IMMUTABLE.
- Exported C#/DLL bodies were stubs, not recovered original logic. Current C# is native-derived reimplementation. Native libil2cpp SHA256 d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c.
- CRITICAL: Cpp2IL PRINTED DISASSEMBLY HAS WRONG VA/raw mapping, delta0x4000. Use Evidence/cpp2il-native-address-map.json and Tools/disassemble-type.ps1 -TypeName Exact. Named ISIL supports API names; verify branches/constants with corrected ARM64. resolve-native-usage.cjs and resolve-native-generic.cjs retain literal/type provenance.
- No owned editor/player remains at checkpoint; leave user Unity Hub PID10040 alone (its executable is Unity Hub/resources/unity.exe). Verify executable identity before closing anything. PowerShell Get-Content -Encoding UTF8 avoids misleading accent decoding in this UTF-8 handoff.

## Earlier implemented baseline retained

Menus/selection/arenas, spawning, movement, active AI Lv1/2/3, projectile/damage owners, shields, score/results/rewards, Death/Respawn, Survival and Tournament/bracket/prizes are reconstructed in bounded offline paths. Prior runtime tests covered boot, synthetic physics rounds3-0/rankS/reward6/idempotence, Survival replacements, paid tournament entry/three synthetic wins, all12 basic selections and stage purchase299/300+JSON readback. See Knowledge/windows-runtime-validation.md, combat-runtime-validation.md and corresponding *reconstruction.md files. These were not natural attack-caused victories or complete game acceptance.

Official UGUI/TMP replaced exported plugin stubs; broken exported InternalAPI bridge was backed up. WAITING offline visibility, ExitMenu scale, GameEvents lifecycle and Agata material-cache ownership were repaired. Dummy dynamic projector shader white floor was confirmed/repaired via GPU A/B with GUID/backup retained. Dynamic shadow texture generation remains STUBBED; other dummy shaders/graphics fidelity remain pending.

The prior session also fixed Description.effect(+0x40) versus ReleaseBy(+0x38), related effect consumers/ShadowTramp literal, Animator.Play0x2364318 versus SetTrigger, prison/whip/roster callbacks and ForceShieldOff. Canonical six-API mapping: Tools/animator-native-call-contracts.json. WhipOff and several helpers are genuine native RET. Raven Torus mesh alone was made readable, preserving geometry/material/meta; eleven body meshes were not swept. Full native and backup evidence remains on disk.

## New implementation in this continuation

- Projectile motion/production: MoveRock, MoveLight, MoveWater, SumonByTime, StartRotation. Preserve fixed-tick versus deltaTime semantics, owner-rotation versus LookAt distinction, native delays/offsets/defaults and zero authored speed where present. SumonByTime does not emit AttackLayerSetUp. Sources/contracts in Knowledge/directional-move-dependencies.md and water-motion-reconstruction.md.
- Earth unit: PrisonController, SumonInOpponent, MoveObject. Actual opponent positioning, trap events, four-wall closure/reverse, natural deletion, retained listener owner. DestroyPrisonHit is a tail branch, NOT RET. Shared SumonInOpponent also affects Maya branch prison and SumonThunderAtk2. Knowledge/earth-prison-reconstruction.md.
- Maya DragonWood: MoveForward, ChangeMoveSpeed, MovePath, PathManager, FollowPath, TransformData, StopMovePath. Root speed0->13 after1.5; head oscillation,60-point history,12followers; exact damage publisher lifecycle retained. PathManager explicitly rejects invalid spacing rather than native unbounded loop. Knowledge/dragonwood-path-reconstruction.md.
- Raven: MoveObjectLocal, TrapPowerUp, jumpSimulator and only Utilities.QuaternionToVector3. Native trap detection/damage/defense/scale semantics and delays preserved. Exact scale-at-operation-entry and local motion still need dedicated acceptance. Knowledge/shadow-path-trap-reconstruction.md.
- Fire: IncreaseCollider and ChangeCollider restored (height4 preserving X/Z; delayed newSize). Native-derived but exact geometry/overlap timing not yet separately accepted. Knowledge/fire-collider-reconstruction.md.
- Controls: JoystickSettings, Joystick, VariableJoystick restored; InputReader GetAxis corrected; MobileInputs uses native locals rather than overwriting legacy x/z. Joystick/Dpad actually respond to pointer input. Native Controller dispatch writes legacy fields, while locomotion reads InputReader: physical gamepad bridge remains UNKNOWN, not invented. AxisOptions getter is an explicit inferred unused contract. Knowledge/joystick-settings-reconstruction.md.
- Current563 gameplay C#/meta identities and aggregate SHA256 c872acc63995359307adc4d659be93923c1aec025db7c723c0b0c9c37b5f2cfb: Evidence/working-gameplay-source-checkpoint.json. Validation/editor scripts and plugin binaries are outside that manifest.

## Executed acceptance and exact build boundaries

All reports below have errors=[] and passed, within stated synthetic fixtures. Windows reconstruction only; no original Android runtime or full fidelity claim.

- offline-moves-probe-20261015-185330.json: ONE CLEAN120-route run,10 each/all12fighters, natural variant alternation and controller-specific entry/exit.42 sampled motion records,12authored yaw instances.603.95s real. Evidence/directional-moves-acceptance.json preserves counts/source hash. Assembly8e23df4e037e2daafa8b079e934870f9ee9b1694a913d0b9f4f6315760a22bf8; predates later controls restoration. CPU disabled and actor pose reset; not full awakened moves/aiming/damage wins.
- offline-earthprison-probe-20261015-190624.json: fresh Diana caught/miss cases. Real trap/PrisonFree, closure3.201 on all4walls, no reverse, expiry5.770964s; synthetic sideways miss then real reversal3.347..3.605 and expiry4.013809s. Assembly317849cd908478f430f65e7a2c80e63b9025d1b4451812844a213641d6880acb.
- offline-menu-probe-20261015-190827.json:129translations,299/300 stage purchase+JSON, settled settings poses, original mode callbacks/inversion. Dpad right1->0 movedfighter1.708534; Fixed joystick half(.5,0) forwarded, clamp(1,0), travel1.790758 and releasezero. Exact save and captured preference presence/values restored. Same317849 assembly. No physical controller/all options/all joystick modes acceptance.
- offline-dragonwood-probe-20261015-190932.json: baseline5.201756s/243samples, speed13at1.501787, roottravel48.37587, head81.85461, Yrange3.999998,60history,12followers each171matching moving poses. Earlier direct GetDamage did NOT accept StopMovePath collision.
- offline-prison-probe-20261015-191606.json: actual Maya pointer input, native opponent placement WITHOUT synthetic relocation in this run; actual trap/PrisonExplosion, synchronous BranchCrushAtk layer9/correct cage target, cage destroyed and owner cleared. Assemblya8286d2228001b41dbf5896de8c5e6ab2e92ae47abc89c9961762e5ba9df5f05.
- offline-dragonwood-probe-20261015-192028.json: repeated full motion/chain checks THEN recovered AirAtk synthetic overlap caused NATURAL onDamageRecived. StopMovePath jumps3->6, disableJump, localY delta-.2999997 over3Updates versus-.3. No direct hit/event/parameter mutation. Assembly7d99e897ea66370c8268deb123965ed1c1f8cc40162c85d05345022256818c25.
- offline-moves-probe-20261015-192535.json: fresh Raven10routes, owner powered flag and native damage/defense/FX observations (Snake/Wolf/Bat/Bird). Exact scale multiplication remains unaccepted: creation scale is not operation-entry scale. Same final assembly as latest build below. Child motion samples are WORLD-space, not independent local-curve proof.

Latest build: Evidence/windows-build-validation-20261016-002455.json (archived exact current report), succeeded2026-10-16T00:24:55Z,27.240208s,543442877bytes,0errors/22warnings. Log unity-windows-build-20261015-192409.log. Latest gameplay assembly14657c9e903a432a7dfd0deb73d40e227be51693f6af71771c86bfd7157e45b4. Since00:05:44 build only validation code changed; do not silently attribute older report assemblies to the latest binary. Warnings remain, including legacy particle shader fallbacks. CPU/peak process-family memory and equivalent performance comparisons were not measured.

## Tools and retained failure lessons

- Tools/Build-OfflinePlayer.ps1 generates120input contracts, copies probe/helpers/editor builder and launches hidden Unity; RETURNS PID, NOT completion. Confirm fresh report, compiler log and process exit. Latest report can remain stale after compile failure. Never build while a player uses that output.
- Tools/Run-OfflineProbe.ps1 -Scenario Smoke|Rounds|ImpactAndRounds|Survival|Tournament|Roster|Menu|Graphics|Specials|Defenses|Prison|Moves|EarthPrison|Dragonwood. -Fighter supports Specials/Defenses/Moves. One owned player at a time (shared save/prefs). Launch manifests retain EXE and gameplay assembly hashes.
- Probe source owner: Tools/unity-validation/OfflinePlayerProbe.cs plus OfflineEarthPrisonProbe.cs/OfflineDragonwoodProbe.cs. Assets/ReconstructionValidation copies are generated. Opt-in development probes only; ordinary play is unaffected. CaptureFixture restores reconstruction-player.json/.bak/.tmp and explicit pref presence/values, including Joystick/ControllerInverted. Legacy player.save is not used.
- Actual visible attack is SetAButton PointerEnter/Exit; hidden X ButtonAttack is NOT an active visible button. Native ChangeAtk alternates variants. Vinea/Eliot UpAttack names are inverted versus other controllers. Tools/build-move-input-contracts.cjs derives ordered entries/defaults from retained format-aware audit and checks current controller SHA; it does not guess by suffix.
- Frame polling can miss a same-physics-tick collider enable/trap/disable or short-lived crush. Creation observer uses synchronous GameEvents/EntityId; real trap with exact target is accepted separately from directly observed enabled timing. Source-only RET claims must distinguish tail branches.
- Retained earlier failures184231/184752 were observer assumptions;191027 was collider polling;191346 compiler scope error was corrected;192136 scale comparison used wrong baseline and is superseded by192535 owner-state observations. No product behavior was altered merely to pass these observers. Detailed history: Knowledge/directional-runtime-validation.md and specific owner docs.
- Evidence/roster-moveset-audit.json preserves an EARLIER callback-source snapshot. Controller routes remain hash-validated; its lexical stub flags/source hashes are NOT a current completeness inventory. Preserve that retained snapshot before regenerating a new report.
- RAG rebuilt51files/4556chunks;24structured findings. Lexical bilingual BM25, no embeddings: node Tools/rag/rag.mjs build or query "..." --limit n. Index Knowledge/generated/rag-index.json; runtime-findings.json owns observed/inferred/unknown status. Rebuild after Knowledge edits and verify fresh observed retrieval. Complete Evidence stays on disk.

## Concrete next work

1. Restore remaining active Maya producers sumonOnCollider and ZigzagController (leaf hit/flower/floor beam), following real prefab/clip dependencies and corrected native. Add actual collision-generated children/beam trajectory acceptance, not just spawn counts.
2. Extend defense side/back/awakened cases and all awakened directional variants; actual aiming/attack-caused wins, natural charging, CPU difficulties/pairings. Earth weak/later-damage release, Ren trap and Maya prison miss/interruption remain pending. Legacy Pro25 matchup and LM/LAN owners still have stubs; active CPU routes use Lv1-3.
3. Raven exact operation-entry scale/no-stack/distance/cooldown/owner/jump and LOCAL child movement; long delays10.5/20.5s exceed current route windows. Timed fire collider dimensions/overlap deserve direct sampling. Do not treat world child travel as local-owner proof.
4. Complete menu option effects/economy/character unlock persistence, reload/unknown control modes, graphics/FPS/audio settings, all stages, loss/replay/higher tournaments. Physical gamepad is unresolved; preference switch alone is not playability.
5. Graphics: dynamic shadow texture generation still stubbed, remaining dummy shaders, clipped settings labels and overexposed bodies require original-video comparisons. Floor shader repair is approximate; no arbitrary material/light sweep. Audio not heard/aligned; full video understanding/fidelity not established.
6. Android module/device build remains absent; online Photon/Firebase/backend and local-network LM are not reconstructed. Do not manufacture credentials/server behavior. Keep current handoff, structured Knowledge and RAG synchronized after future changes.


---

# Traspaso: reconstrucción de True Masters en Unity

## LEER PRIMERO — actualización del 15/10/2026, sesión de continuación

Esta sección reemplaza los estados de implementación y pendientes del registro histórico que se conserva más abajo. El usuario pidió actualizar este archivo para que otro agente continúe. Los tres trabajadores guardaron sus checkpoints y terminaron; **no hay agentes escribiendo código**. La compilación Windows lanzada por el principal puede seguir activa: consultar el estado anotado abajo antes de abrir otro Unity.

**Objetivo vigente:** reconstrucción completa, editable y programable en Unity, usando skill REA y base RAG; prioridad explícita del usuario: completar primero OFFLINE. El juego entero todavía NO está terminado. Ya se verificó el arranque completo hasta la arena; NO se verificó una partida completa, movimiento controlado, impactos, reaparición, victoria, recompensa ni fidelidad visual.

### Rutas y cómo retomar

- Raíz del trabajo: `C:\Users\Jose\Desktop\TrueMastersReconstruction`.
- Proyecto editable: `UnityProject`; código: `UnityProject/Assets/Scripts/Assembly-CSharp`.
- Unity instalado: `C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Unity.exe`. Módulos Windows/WebGL; Android no instalado.
- Skill usada: `C:\Users\Jose\Desktop\REA\.agents\skills\reverse-engineer-anything\SKILL.md`. Leerla antes de retomar REA. Se reutilizaron evidencias existentes; no repetir setup, descargas ni exportaciones.
- REA está en otra carpeta (`C:\Users\Jose\Desktop\REA`); no introducir allí los archivos del juego. No se editaron fuentes del repositorio REA en esta continuación.
- Los dos exports de AssetRipper en Evidence permanecen inmutables. Trabajar exclusivamente sobre UnityProject y herramientas/informes de la reconstrucción.

### Lo que ya tenemos y lo que se añadió

Se conserva todo lo documentado en el registro histórico: APK/video originales, nativo suplementario validado, 12 escenas, 380 prefabs, 300 animaciones, recursos, proyecto Unity importable, menús offline y herramientas de evidencia. **Los scripts/DLL exportados eran stubs**; cada recuperación de lógica C# es una reimplementación desde ARM64, no código fuente original recuperado.

Avances de esta continuación:

1. **Daño y colisiones:** DamageOnCollision, ConfigAttackLayer, DestroyOnCollision, SummonOnCollision, SummonAttack.CreateKickAttack, ColliderController y dos helpers angulares de Utilities restaurados. Se mantienen contratos separados Description/AtkDescription. El daño ordinario al receptor incrementa Damage en .5; Description.Damage controla reacción/fuerza, no resta directamente HP. Ver `Knowledge/projectile-reconstruction.md` para ramas y constantes exactas.
2. **Identidad Unity nueva:** Unity6000.6 convierte GetInstanceID en error de compilación. GameEvents y SummonAttack usan EntityId, sin hash ni conversiones con pérdida. Eventos de capa del jugador conservan int. No reintroducir GetInstanceID ni cambiar todo evento int indiscriminadamente.
3. **Puntuación/resultados:** ScoreController y ResultsController restaurados; Score usa HostPlayer.hostPlayer/GuestPlayer.guestPlayer (no .i). Puntos, victoria3, timeout/desempate por cristales, rangos, recompensas y récord Survival se derivaron del nativo. Base normal=player-enemy+3; Survival=2*player; práctica=0. CpuMode1..4 multiplica1.5/2/2.5/3 y ceil después del rango. Survival>10 puntos añade20 aparte del valor mostrado. Hay guardas nuevas de una sola transacción. **No se ejecutaron aún esas fórmulas ni una victoria en runtime.** Ver `Knowledge/scoring-reconstruction.md`.
4. **AI/controles:** AI.Lv2 y Lv3, FastAttack/Floor, EvadeAttackDelay, Legend/Hard/Normal y dispatch Pro restaurados. Corregido HasSpecial de Dante/Agata→clave Dante; callback Lv1→Block. Quedan25 métodos legacy de emparejamientos Pro como stubs explícitos; las rutas NewAI/CPUSelection/Survival/Tournament auditadas usan Lv1–Lv3. DesktopInputs permite ataque con Counter.value<0 y usa ReferenceEquals para conservar el campo administrado negativo después de destruirse el Counter, como el acceso nativo. No sustituir por `counter && ...` que bloquea al destruirse. Ver `Knowledge/combat-reconstruction.md`.
5. **Controlador/escenario:** GameController.EnableSpecial offline implementado con claves exactas; StageController.Awake instancia el escenario recuperado según Stage y asocia hijo music. TagInChildren examina hijos inmediatos, incluidos inactivos, sin recursión. Ver `Knowledge/offline-battle-integration.md`.
6. **Lifecycle/movimiento de ataques:** DestroyNetObjByTime, DestroyByTime, DestroyByTime_Basic, CopyParentLayer/CopyPatherLayer, DestroyByDeathZone, cleanup por daño/borde/weak/duplicados, DestroyWhenCreated, SumonOnDestroy, DestroyAtkSimple, ShadowHandGrabController, StopParticles, RotateSelf, SimpleMove, GoForward, Follow y CameraShake restaurados según informe. **RemoveGravity es no-op offline en el nativo; no desactivar gravedad por su nombre.** Inventario nuevo: `Evidence/projectile-component-reference-inventory.json` y `Tools/inventory-projectile-components.cjs` (380 prefabs,186 identidades script). Distingue refs de componentes de cantidad de archivos.
7. **Notificaciones/menú:** Notifications restaurado (bar On/Off, timers, colores, popup y callback Yes con LerpManager). StarMenuManager.TourneyAlert/CloseTourneyAlert/EnterTournament/AllCharactersUnlocked implementados desde nativo. Offline comprueba propiedad local de Aura/Eliot/Ren/Rina; no fabrica login/premium. **TournamentController.PayTournament sigue stub, por tanto entrar al torneo todavía no funciona.** Cerrar alerta depende de IncreaseSize, también stub. Ver `Knowledge/notifications-reconstruction.md`.
8. **Tutorial sprite:** SpriteUIAnimation restaurado desde coroutine MoveNext0x12C1BF4: espera m_Speed, aplica/wrap/incrementa sprite y repite. Nuevo dueño de un solo loop evita corutinas huérfanas y StopCoroutine(null). Campos/meta conservados. Cambio posterior al smoke de arranque exitoso: compilar/verificarlo. Dumps `native-disassembly-SpriteUIAnimation.txt` y `native-disassembly-SpriteUIAnimation__Func_PlayAnimUI_d__11.txt`.

### Verificación ejecutada y artefactos

**Prueba PlayMode exitosa**, reporte `Evidence/offline-flow-validation.json`, generado **2026-10-15T21:47:42.1140039Z**, Unity6000.6.5f1; log `Evidence/unity-offline-flow-20261015-164727.log`:

- DesarrolladoPor → StartMenu → HowToPlay → Character Selection → Bending Battle.
- Se invocaron callbacks realmente serializados: Button SinglePlayerMode; EventTrigger PointerClick Continue y start. Estos dos últimos NO son Button.onClick.
- Preferencias observadas: Character=Sora, CharacterCPU=Amon, CpuMode=0.
- A39.43s del probe: Sora(Clone) vs Amon(Clone), gameStarted=True.
- errors=[] del juego. Se conserva por separado editorInfrastructureErrors con ArgumentOutOfRangeException de UnityEditor.Search.SearchDatabase/SearchInit.IndexationOnStartup; no ocultar ese error del editor ni confundirlo con código del jugador.
- Esta prueba solo establece startup hasta controles habilitados. NO valida render, movimiento, ataques, daño, ronda, respawn ni resultado. Las últimas modificaciones Notifications/StarMenuManager/SpriteUIAnimation son posteriores a ese reporte; no extender su cobertura a ellas.

Herramientas nuevas:

- `Tools/Validate-OfflineFlow.ps1`: copia probe editor y lanza Unity oculto, log fechado; retorna PID, **no espera**. `Tools/unity-validation/OfflineFlowValidation.cs` es su fuente; copia en Assets/Editor/ReconstructionValidation. Espera LoadingScreen.loading=false antes de pulsar (de otro modo el handler rechaza la transición). Reporte y salida sobreviven cambio de dominio mediante SessionState.
- `Tools/Build-OfflinePlayer.ps1`: copia BuildOfflinePlayer.cs al editor y OfflinePlayerProbe.cs a Assets/ReconstructionValidation, y lanza build Windows Development. Mantiene las12 escenas y sus índices; no eliminar escenas online cambiando índices que usa la lógica offline.
- `Tools/unity-validation/OfflinePlayerProbe.cs`: solo UNITY_EDITOR/DEVELOPMENT_BUILD, opt-in `-rea-offline-probe <ruta absoluta reporte.json>`. Recorre menús, captura PNG, prueba movimiento por InputReader, aserción independiente de pose final de cámara, un control de ataque, observación12sCPU, errores y conteo de proyectiles por capa. **Es herramienta nueva aún no ejecutada en un player; puede requerir corregir el propio probe.** No confundir fallo de herramienta con fallo del juego. La enumeración usa Description (no DamageOnCollision, que normalmente es receptor del fighter). No prueba partida completa ni paridad visual.
- `Tools/unity-validation/BuildOfflinePlayer.cs`: espera guardar Evidence/windows-build-validation.json. Primer intento falló por int→uint en los campos de totalErrors/totalWarnings de MI wrapper; ya corregidos a int. No fue error del juego.

**Compilación Windows en curso al preparar este texto:** PID10736, inicio local15/10/2026 16:53:46 (UTC21:53:46.9289510Z), comando BuildOfflinePlayer.Run. Log `Evidence/unity-windows-build-20261015-165346.log`. Salida prevista `Builds/Windows/TrueMastersOffline.exe`. Consultar proceso, log y reporte fresco antes de abrir Unity, reconstruir o ejecutar. La compilación usa el checkpoint de código disponible al inicio, no incorpora futuras ediciones. El estado final comprobado se agrega al cierre de este handoff.

Después de build exitoso, lanzar el EXE **oculto**, con gráficos (sin -nographics) y argumentos `-screen-fullscreen 0 -screen-width 960 -screen-height 540 -logFile "<Evidence/player-probe-fecha.log>" -rea-offline-probe "<Evidence/offline-player-probe.json>"`. No se ejecutó aún ese comando. Confirmar reporte/log nuevos y abrir las capturas; no declarar jugable por compilar. WaitForEndOfFrame no se reanuda en editor batchmode, por eso se prepara player para probar cámara/render; referencia oficial: https://docs.unity.cn/Manual/CLIBatchmodeCoroutines.html . No alterar el juego solo para que una prueba de editor pase.

### Corrección importante de integración UI

La primera prueba runtime crasheó por TypeLoadException TMPHelpURL. Además de los DLL UI/TMP retirados antes, quedaba el stub `Assets/Plugins/Unity.InternalAPIEngineBridge.004.dll`, duplicado con el asmdef real de UGUI2.6.0. Se movieron exclusivamente ese DLL y meta del proyecto de trabajo a `WorkingBackups/ui-bridge-20261015`; SHA/procedencia en `Evidence/ui-bridge-migration.json`. Búsqueda de referencias a su GUID en assets serializados no encontró usos. Exports originales intactos.

Los siguientes arranques ya superaron el crash. `Tools/unity-audit/restore-ui-package.mjs` incluye ahora también ese tercer plugin para futuras recuperaciones. Tres tests existentes de preservación YAML/PPtrs/IDs ejecutados con `node --openssl-legacy-provider --test Tools/unity-audit/restore-ui-package.test.mjs`:3/3 pasaron. No volver a copiar el stub ni intentar reparar TMPHelpURL dentro de los DLL originales.

La auditoría previa `Evidence/unity-import-validation.json` (21:16Z,12scenas,0missing scripts) **sigue siendo histórica**; no se volvió a ejecutar para todo este código. El smoke exitoso sí prueba una compilación posterior más flujo, con el alcance indicado. Shaders recuperados siguen aproximados; warnings de fallback Particles/Additive (Soft) ausente permanecen.

### Lo que falta — orden recomendado

1. **Terminar build/verificar checkpoint actual.** Revisar PID10736 por identidad+hora, log y reporte, no lanzar dos Unity sobre el proyecto. Ejecutar y depurar player probe; inspeccionar PNG de menú/arena, materiales/TMP/cámara. Su resultado aún es desconocido.
2. **Respawn/caída/muerte y ronda no final — CRÍTICO.** Respawn.cs, Death.cs, SurvivalController.cs, NormalMode.cs permanecen stubs. Segunda tarea del agente SOLO produjo investigación, no código. Leer `Knowledge/respawn-reconstruction-checkpoint.md` y dumps nuevos. Siete clips FallingToLanding{Shadow,Light,Grass,Fire,Earth,Air,Water} invocan respawn. Host vuelve(7,5,0), Guest normal(-7,5,0); Survival reemplazaCPU mediante algoritmo nativo pendiente. Death.OnStateEnter solo escribe flag en AnimatorStatesManager+0x40, no es llamada directa a CheckPoints. Resolver ese campo antes de programar. ScoreController.OnTriggerEnter sí llama CheckPoints; auditar estados/eventos/reinicios completos. RespawnOff bloquea1s en Survival.
3. **Rutas de animación realmente usadas.** AnimatorEventsController y callbacks de estados todavía NO se editaron en la segunda tarea. Nuevo dump `native-disassembly-AnimatorEventsController.txt`. Hallazgo: CreateAttack0x1214018 dirige a AttacksController_LM y playerController/isLocal; StartAwakening/StartSpecial usan Special_LM. No asumir que esa clase es la ruta Sora/Amon normal. Auditar YAML de clips/controllers para SummonAttack/AnimationMovement/AnimatorStatesManager y Recover/MovementStateEvents/Stunned/StopGo/SlowSpeed/ChargeSpecial. Restaurar productores antes de parches downstream.
4. **Final y recompensa real.** Probar tres puntos, caída, timeout/empate, finishMenu, wallet antes/después y recarga, transacción única, reinicio/menú. Scoring implementado no equivale a ciclo de ronda funcional. Restaurar dependencias que los probes revelen. Ver límites en scoring-reconstruction.md.
5. **Componentes aún vacíos de proyectiles/UI.** IncreaseSize, CreateOnCollition, DisableCollider, ChangeParticleSize y ChangeStartPosition siguen stubs: segunda tarea SOLO analizó. Hay5dumps+3corutinas y sección Handoff al final de projectile-reconstruction.md con VAs, constantes pendientes0x651C24/0x651BC8/0x651B4C/0x651B50 y slot genérico0x2816A40. IncreaseSize es dueño de cierre de alertas. También pendientes MoveObject/MoveObjectLocal2/FollowOwner y controladores elementales; usar inventario para elegir rutas reales.
6. **Offline completo.** Survival real y reemplazo deCPU/record; TournamentController/TournamentManager/CharactersTournament/TourneyLevel/HideOnTourney y menú/fee/bracket; restantes personajes, elementos, habilidades y desbloqueos; local multiplayer offline si pertenece al alcance. No reducir «todo el juego» al duelo Sora/Amon. Los25 matchups legacy de AI permanecen explícitos; resolver alcanzabilidad antes de dedicar esfuerzo.
7. **Fidelidad y entrega Unity.** Comparar capturas/mecánicas contra muestras de video/APK, audio, shaders, cámara, FX y todos los menús. No hubo validación Android ni módulo instalado, ni partida en teléfono. Online/Photon/Firebase/ads continúan fuera de prioridad y muchos SDK DLL son stubs; no conectar a servicios originales.
8. **Actualizar evidencia/RAG/cobertura.** Knowledge/game-coverage.json y game-spec.md siguen especificación de obligaciones, no inventario actualizado de código completo. No usar sus frases antiguas de recuperación nativa como estado actual. Mantener estado observado/inferido/desconocido, provenance y alcance de cada prueba.

### Herramientas y RAG: detalles que evitan repetir errores

- Regla fundamental permanece: Disassembly de Cpp2IL es incorrecto por desfase0x4000 VA/raw. ISIL orienta llamadas, ARM64 mapeado confirma ramas/constantes. Usar Tools/disassemble-type.ps1 y resolve-native-usage/generic.
- Tipos anidados requieren el nombre EXACTO del mapa, por ejemplo `SpriteUIAnimation/<Func_PlayAnimUI>d__11`, no el nombre C# escapado de AssetRipper. El script actual puede producir un archivo vacío si no encuentra tipo; comprobar longitud/cobertura y no tratarlo como ausencia de lógica. Se produjo un dump vacío con el nombre equivocado y luego el dump correcto mencionado arriba.
- Tools/inspect-ai-flow.cjs ahora admite tipo opcional además de método; es ayuda, no autoridad: su seguimiento sencillo de registros puede calcular slots falsos al reutilizar registros. Volver al dump completo si falla. No derivar inexistencia de un fallo de esta ayuda.
- RAG es BM25 lexical local con alias bilingües, sin embeddings, no recuperación automática de fuente. `node Tools/rag/rag.mjs build`, luego `query "..." --limit 3` o `context`. Invalida índice al cambiar Knowledge: el último agente recibió rechazo por índice viejo, no resultados. Al cerrar este handoff se reconstruye y registra el conteo nuevo. Repetir build si se vuelve a editar Knowledge.
- No se inicializó git del juego. No publicar APK/assets ni tocar repo REA. Configuración y copias originales deben preservarse.

## Registro histórico del checkpoint anterior

**Desde aquí se conserva el texto anterior como referencia de fuentes y trabajos anteriores. Sus afirmaciones de «vacío», «pendiente», «sin procesos activos» o conteos de implementación pueden estar superadas por la actualización de arriba.**

Checkpoint del 15 de octubre de 2026. Leer este archivo antes de editar o repetir análisis.

## Objetivo y decisiones del usuario

El usuario quiere comprender detalladamente todo el juego y sus menús para reconstruirlo como proyecto editable y programable en Unity. Confirmó que desea **REA y RAG**. Luego eligió explícitamente **completar primero el modo sin conexión**. El multijugador, las cuentas Photon/Firebase y sus servidores quedan para una fase posterior. No desplegar servicios ni conectar el proyecto a servicios originales para probarlo.

El usuario pidió pausar, después continuar y finalmente preparar este resumen para que otro agente siga. **El trabajo queda detenido en este checkpoint**, sin agentes escribiendo ni procesos Unity propios activos. No se ha completado el juego ni una partida offline funcional verificada.

## Ubicación y autoridad de las fuentes

- Proyecto de trabajo: `C:\Users\Jose\Desktop\TrueMastersReconstruction`.
- Proyecto para Unity: `UnityProject` dentro de esa carpeta.
- Código editable: `UnityProject/Assets/Scripts/Assembly-CSharp`.
- Fuentes originales, solo lectura: `C:\Users\Jose\Desktop\true masters videos gameplay y menú`.
- Repositorio/herramienta REA: `C:\Users\Jose\Desktop\REA`. **No es el proyecto del juego**. No introducir allí la reconstrucción; `git status --short` seguía limpio al preparar este traspaso.
- No se inicializó un repositorio Git para el juego. `.gitignore` excluye Evidence y artefactos generados; revisar la política antes de versionar recursos recuperados. No publicar automáticamente.

Archivos proporcionados:

| Fuente | Datos |
|---|---|
| `apk compilado.apk` | 132765204 bytes, SHA-256 `51e274bd08fd72ff2be562960d21f31871c0318a1e99d9a5c193e4f5d6024c4d` |
| `true masters gameplay multiplayer with a friend.mp4` | 848,6 segundos |
| `true masters gameplay singleplayer all charactersmp4.mp4` | 1003,2 segundos |
| `true masters gameplay singleplayer mode survival (with the principal menú options).mp4` | 264,5 segundos |

Los videos fueron revisados mediante muestras a lo largo de su duración (cada 8 segundos) y capturas dirigidas más densas de menús/transiciones/tutorial. **No se escuchó audio ni se observó cada fotograma**. `Knowledge/video-analysis.md`, `video-findings.json` y `video-frames/` conservan cobertura, tiempos y límites. Las siete hojas iniciales también están en `Evidence/video-samples/`.

## APK completo, recuperación y limitación crítica de código

El APK suministrado es `com.FacuRodquin.BendingBattle`, versión 2.99 / código 299. Su manifiesto requiere `base__abi`; no contenía bibliotecas `.so`. El usuario solo tenía ese APK.

Se obtuvo un XAPK público desde `https://d.apkpure.com/b/XAPK/com.FacuRodquin.BendingBattle?version=latest`. Se conserva en `Evidence/acquired/TrueMasters-2.99.xapk`, SHA-256 `7cd68325fdaad1e7daf26044ad489bc025917dfa8355425bd139ca24d4c97a5e`, coincidente con el publicado por el distribuidor. Contiene un base de 120227865 bytes, SHA `9ed8ac8c7996e5692494b6ef0c0f8437decba06565273f0ddcfb866687be05eb`, y `config.arm64_v8a.apk`, SHA `ff93433b1333759cbe5c0f2f4538551afd550978be517f62c2a1892ffa01b633`.

**Los APK base no son idénticos**, pero se verificó coincidencia exacta de `data.unity3d`, `global-metadata.dat`, `boot.config` y GUID de compilación. Ambos son Unity **6000.3.10f1**. Ver `Evidence/public-base-comparison.json`. Se mantuvo procedencia separada; no se modificó el APK del usuario.

El conjunto extraído complementario está en `Evidence/acquired/merged-public-apk/`. `lib/arm64-v8a/libil2cpp.so` tiene SHA `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.

AssetRipper 2.0.0 portátil produjo dos snapshots **inmutables**:

1. `Evidence/RecoveredUnityProject/ExportedProject`: sin nativo, pierde campos UI.
2. `Evidence/RecoveredUnityProject-WithNative/ExportedProject`: campos y referencias recuperados correctamente. Es la base copiada a `UnityProject`.

Recuperado: **12 escenas, 380 prefabs, 300 animaciones**, texturas, modelos, materiales y audio; segundo export tiene 563 archivos C# y 38 DLL de plugins. La auditoría de 12 escenas y 6 prefabs UI encontró **570 textos y 247 llamadas persistentes**, con referencias a sus fuentes. Ver `Evidence/native-recovered-project-inventory.json`, `unity-menu-audit.json`, `Knowledge/unity-menu-analysis.md` y `unity-menu-findings.json`.

**Los scripts y DLL exportados eran stubs, no código funcional recuperado**. La opción Level3 de AssetRipper Free no reconstruyó cuerpos. Cpp2IL `dll_il_recovery` terminó con código 0, pero una auditoría encontró 4126 métodos de Assembly-CSharp con exactamente `ldnull; throw` y 20 sin cuerpo. No usar esos DLL como lógica restaurada. `Evidence/cpp2il-ilrecovery-audit.json` demuestra esto.

Se recuperó implementación nativa consultable mediante Cpp2IL ISIL y desensamblado ARM64 corregido; la reconstrucción de C# se está haciendo explícitamente desde esa evidencia.

## Advertencia técnica imprescindible: direcciones nativas

**La sección `Disassembly:` impresa por Cpp2IL es inválida para este ELF**: interpreta dirección virtual como offset de archivo, con diferencia `0x4000` en el segmento de código. No fundamentar lógica en esas instrucciones. Además, los `AddressAttribute.RVA` generados representan offset de archivo en este binario, no VA.

- `Evidence/cpp2il-native-address-map.json`: 4146 métodos propios, VA/offset/longitud estimada y offsets de campos.
- `Evidence/Cpp2IL-ISIL/IsilDump/Assembly-CSharp`: ISIL levantado correctamente en general, pero con instrucciones no implementadas y ramas inválidas. **Confirmar condiciones, comparaciones, SIMD y constantes con ARM64 corregido**.
- `Evidence/native-disassembly-*.txt`: instrucciones Capstone desde bytes mapeados correctamente.
- `Tools/disassemble-type.ps1 -TypeName NOMBRE`: genera esos dumps usando Capstone 5.0.6 portátil y el mapa. Admite tipos de corutinas si se usa su nombre exacto del mapa.
- `Tools/resolve-native-usage.cjs SLOT_VA...`: resuelve slots RELA de cadenas/tipos. Los slots de datos del ISIL no siempre coinciden con los del desensamblado correcto: tomar los de ARM64.
- `Tools/resolve-native-generic.cjs SLOT_VA...`: herramienta nueva, resuelve usos kind6 a método genérico y argumentos de tipo desde el registro `0x279CAF0`; valida límites. Verifica diferencias como `AddComponent<HostPlayer>` frente a `GetComponent<T>`.

No confundir nombres de clases/campos/métodos con comportamiento ejecutado. Mantener separadas observaciones, inferencias y decisiones nuevas de implementación.

## RAG local operativo

`Tools/rag/rag.mjs` implementa recuperación BM25 local, alias español/inglés y ensamblado de contexto con citas. No contiene embeddings ni un LLM propio; el agente usa el contexto recuperado para razonar/escribir. Conserva fuentes, hashes, tiempos, VA y estados observed/inferred/unknown. Rechaza índices obsoletos. La salida generada vive en `Knowledge/generated`, excluida del corpus.

```powershell
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" build
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" query "controles ataque defensa" --limit 4
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" context "GameController inicio de combate" --limit 4
```

`Tools/ingest-isil.mjs` ya incorporó 60 tipos / 575 métodos seleccionados, en 1941 fragmentos, con mapa de direcciones y exclusión explícita de la sección inválida. No repetir recuperaciones si las fuentes no cambiaron. `Knowledge/implementation-findings.json` conserva IR y algunas instrucciones corregidas. Último índice anterior al checkpoint: aproximadamente 4300 fragmentos; se reconstruye al cerrar este traspaso.

Pruebas ya ejecutadas en fases anteriores: RAG/procedencia/frescura, ingesta ISIL y auditores YAML; aprobadas. La migración UI tuvo tres pruebas propias. **No equivalen a pruebas del juego**. Documentación de uso en `Tools/rag/README.md`, `Tools/ingest-isil-README.md` y `Tools/unity-audit/README.md`.

## Unity, migración y validaciones reales

Editor instalado: `C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Unity.exe`. Solo se encontraron módulos Windows standalone y WebGL; Android Build Support/JDK de Unity no están instalados. Java en PATH era 8. No es necesario solucionar JADX para continuar esta ruta IL2CPP ya recuperada.

Se abrió una copia de trabajo en Unity 6000.6.5f1. El export original 6000.3.10f1 sigue inmutable. Se eliminó del manifest la dependencia obsoleta `com.unity.modules.vr`; Unity añadió sus dependencias compatibles al importar.

Un atributo `MovedFrom(null)` en la DLL ficticia Unity.VectorGraphics provocaba fallo en el API updater. Se corrigió **solo la copia de trabajo**, retirando ese atributo inválido con Mono.Cecil. `Tools/Repair-PlaceholderMetadata.ps1` usa una utilidad .NET 8 y el SDK que ya trae Unity, con backups y hashes en `Evidence/import-repairs/`. El primer intento con Mono.Cecil desde Windows PowerShell no pudo escribir por incompatibilidad de runtime; fue sustituido por la utilidad .NET. No se alteraron métodos de esa DLL.

Se sustituyeron **solo los plugins UI ficticios** `UnityEngine.UI.dll` y `Unity.TextMeshPro.dll` por el paquete oficial instalado localmente **com.unity.ugui 2.6.0**. Migración mediante parser YAML y spans: 1970 referencias en 43 archivos, incluidas 16 en animaciones Counter/Fade_In/Fade_Out. Se verificó igualdad semántica excepto las referencias intencionales y cero referencias a los dos GUID retirados. Reporte `Evidence/ui-package-migration-applied.json`; backups `Evidence/WorkingBackups/ui-package-2026-10-15T21-13-32-627Z/`; herramienta `Tools/unity-audit/restore-ui-package.mjs`.

**Los demás plugins exportados (Photon, Firebase, anuncios, etc.) siguen siendo placeholders**. No declarar soporte online. Los shaders exportados son `DummyShaderTextExporter`, aproximaciones; no declarar fidelidad visual. La auditoría inicial detectó un slot de material ausente en Character Selection.

Validación realmente ejecutada: importación/compilación batch y deserialización de 12 escenas sin PlayMode. El último `Evidence/unity-import-validation.json` marca UTC **2026-10-15T21:16:05.4094786Z**, `compilationSucceeded: true`, 12 escenas y cero scripts sin asociación. **Es anterior a las últimas ediciones de GameController, combate, presentación, Score y proyectiles**. No se ha compilado el checkpoint actual ni jugado una partida.

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\Validate-Unity.ps1"
```

Esto inicia Unity oculto y devuelve PID/log; **no espera a que termine**. Comprobar finalización, errores y sentinel `REA_RECONSTRUCTION_IMPORT_AUDIT_COMPLETE`, además de `generatedUtc` nuevo. No confundir el JSON de una corrida anterior con la nueva. `Tools/unity-validation/RecoveryValidation.cs` abre escenas en editor, no PlayMode. No quedaron procesos Unity activos al checkpoint. `npx` falla por shim PowerShell: usar `npx.cmd` o `node scripts/rea.mjs`, sin cambiar políticas persistentes.

## Estado del código reconstruido

Todos los nombres siguientes están en `UnityProject/Assets/Scripts/Assembly-CSharp`. Conservar los `.meta` para mantener GUIDs.

### Menús, selección y persistencia

Ver `Knowledge/menu-reconstruction.md`: StarMenuManager, SystemManager (boot, modo y persistencia), LevelChanger, LoadingScreen, HowToPlayManager, GameModeVisibility, DontDestroyOnLoad, SelectionController, CPUController, CharactersManager, StagesManager, StageSelector, StageSelectorButton, GameSpeed, SurvivalRecord/SurvivalView, CameraController, TMPlayer y modelos de propiedad según cambios guardados. CrystalsNotification, CrystalManager y TrophiesManager restauran gasto/recompensas/etiquetas. FinishMenu tiene exitGame y reboot restaurados; resto online incompleto.

Datos establecidos: claves `Character` (Sora inicial), `CharacterCPU` (Amon inicial), `Stage`, `Timer`, `GameSpeed`. Alias visibles Sora→Siro y Amon→Blaze; no asumir que versiones de los videos y del APK tienen el mismo catálogo. Libres: Sora,Vinea,Diana,Amon; campos de propiedad para los restantes. Precio por personaje: 5 trofeos o 200 cristales; trofeos se gastan. Etapas 0..7, 7 aleatoria. Opciones de tiempo [300,420,540,180]. Escena de batalla índice 5. Guardado nuevo `reconstruction-player.json` con `.bak`, **sin deserializar/modificar el legado BinaryFormatter `/player.save`**: decisión nueva documentada.

### Gráficos, especial y aura

GameManager (13 métodos) y GraphicsManager (13 métodos): preferencias, resoluciones, FPS, velocidad, callbacks y ajustes con direcciones nativas. Ver sus informes `Knowledge/*-manager-reconstruction.md`.

SpecialAtk y AuraController restaurados. Ver `Knowledge/special-attack-reconstruction.md`. Carga: umbral .9 e incremento val/numberOfSpecial; drenaje principal -.034/time por espera .04 s; no convertir arbitrariamente a deltaTime. Corutinas reales reemplazan stubs generados. Callback FixedUpdate era RET nativo y se retiró. Sus dependencias de animación/daño todavía necesitan integración runtime.

### Controlador central: últimos cambios del agente principal

GameController: Awake **offline**, selección aleatoria de CPU en Survival, singleton/PV; helpers de distancia/capa; MainMenu; creación de prefabs exactos `Resources/PhotonPrefabs/<nombre>` en (5,0,5) y (-5,0,-5); AddComponent de HostPlayer/GuestPlayer; asociación a BattleSceneSingleton; AI toggle; CompleteMainChar (Animator/MobileInputs/Description.moveset); inicio del contador; EnableInputs, ReadyToAttack; callbacks de ataque, especial, dirección, pausa/reinicio/salida.

BattleSceneSingleton: singleton y métodos de oponente, identificación LM y distancia. HostPlayer/GuestPlayer: singleton, tags `HostPlayer`/`GuestPlayer`, capas 9/8 y limpieza; Guest conserva referencia en Survival como el nativo.

**GameController.EnableSpecial sigue VACÍO** y varios métodos/corutinas online permanecen stubs. No declarar completo ese controlador. Awake requiere managers persistentes y BattleSceneSingleton previamente inicializados; el orden debe verificarse en runtime (no añadir búsquedas/fallbacks arbitrarios). No se implementó todavía un bootstrap para abrir directamente la arena. No hay informe individual del controlador: este apartado registra sus últimas ediciones.

### Controles y AI

Editados: InputSet, InputReader, MobileInputs, DesktopInputs, AnimatorStatesManager, ButtonsController, ButtonAttack, RadialUIFX, AnimationMovement, LookAt, ActivateAI, AI. No se escribió todavía `Knowledge/combat-reconstruction.md`; usar este checkpoint y los dumps.

AI: 54 métodos con recuperación, incluido Lv1 y acciones/movimiento/ruteo básico. **34 métodos pendientes**: Lv2, Lv3, FastAttack, FastAttackFloor, EvadeAttackDelay, Legend, Pro, Hard, Normal y 25 emparejamientos elementales. Conservan advertencias explícitas una vez por método; no sustituyen todas las dificultades por una IA genérica. CpuMode 0 / Lv1 es la única estrategia completa declarada por ese agente, aún sin prueba ejecutada.

Revisiones concretas pendientes:

1. **AI.HasSpecial** usa actualmente `Eliot` para Dante(Clone)/Agata(Clone). El slot nativo `0x2814960` se ha resuelto a **`Dante`**; evidencia nueva `Evidence/handoff-ai-special-key.json`. Corregir ese caso y verificar las ramas antes de confiar en él. Otros grupos observados: Rina/Amon→Rina, Eliot/Vinea→Eliot, Sora/Aura→Aura, Diana/Ren→Ren.
2. **DesktopInputs.Actions** omite una comprobación de preparación de ataque a través de `BattleSceneSingleton.i + 0x50`; interpretar el campo correctamente antes de corregir (no asumir readiness por nombre).
3. AI.Lv1 debe auditar el argumento de callback de DoInTime; constantes .2/.6 confirmadas, el agente señaló un posible detalle pendiente.
4. ButtonsController captura referencias de círculo/área y la letra pasada en cooldown, corrigiendo la propiedad mutable original. Está marcado como decisión estructural, requiere prueba.
5. Se añadieron guardas de null e inicialización Awake de algunos singletons para el orden de arranque; no son prueba de equivalencia original.

### Presentación y puntuación

CameraManager, Counter, AIToggle, StatsController, BattleUIManager, LerpManager, ExitMenu, RateGame implementados; ver `Knowledge/battle-presentation-reconstruction.md`. Counter inicial 3; al cero muestra GO!, quita blockInputs, habilita entradas e invoca onCounterFinish. Stats usa valores/prefabs originales.

La corutina de cámara tenía un avance temporal sospechoso en el nativo: se aplicó una **corrección explícita de reconstrucción** para acumular deltaTime en cada frame. No afirmar fallo runtime original sin prueba. Validar transición de .1 s: después de duración+frames alcanza pose exacta y elapsedTime vuelve a 0.

**Score** restaura temporizador y presentación offline; **Utilities.GetTimeAsString** es el único método restaurado de Utilities. Survival no usa temporizador; otros modos usan Timer, alertan a ≤30 y disparan timeout a ≤0. TimeUp espera 4 s y delega en ScoreController. Ver `Knowledge/score-reconstruction.md`.

**ScoreController y ResultsController siguen completamente en stubs y no fueron editados**: son los dueños reales de puntos, victoria, fin de combate y fórmulas de recompensas. Su implementación es crítica; no inventar valores desde el video. Existe `Evidence/native-disassembly-ScoreController.txt`. Los métodos de CrystalsNotification ya están disponibles para integrar recompensas exactas.

### Proyectiles y daño

Últimos cambios guardados, aún sin `Knowledge/projectile-reconstruction.md`:

- SummonAttack: Start,ChangeColor,Create,CreateAsChild,CreateAwakeAtk,RestoreJumpAtk,CreateSimpleAtk,CreateBranchAttack,CreateAttack,WaterShieldExists. **CreateKickAttack permanece vacío**. Métodos online avisan y no conectan.
- CreateAwakeAtk exige AnimatorStatesManager.awakening; elemento **Leaf**, no Water: espera 2 s Leaf / 3 s otros; Leaf requiere distancia ≥5. Prefabs/capas originales.
- AttacksManager (plural): inicialización/listas/GetOwnAttacks; capa 8 usa OpponentAttacks.
- AddToAttackManager: registro y eliminación; capa 9 usa MainCharAttacks.
- MoveAttack: cuatro métodos restaurados; movimiento nativo por FixedUpdate, sin introducir dt.
- AttackEvents.OnTriggerEnter: Description.Type == "Char".
- ChangeRendererMat: únicamente ChangeColor, material/estela y exclusión tag UnColor. Otros métodos vacíos.
- GameEvents: eventos C# e invocaciones restauradas; Start hace i=this, falta reproducir protección nativa contra duplicados.

**ConfigAttackLayer, DestroyOnCollision, SummonOnCollision y DamageOnCollision permanecen totalmente vacíos**. Ya hay dumps ARM64 corregidos. Sin ellos no se ha restaurado daño/colisión funcional. DamageOnCollision offset0x28 es **specialCharge**, no Damage; Description.Damage es otro contrato, no mezclarlos.

## Ruta recomendada para continuar

1. Leer este archivo y los informes existentes. Verificar estado de procesos antes de abrir Unity. No repetir AssetRipper/descargas/inspección de APK o cambiar REA setup: la evidencia ya está disponible.
2. Compilar el checkpoint actual. Corregir errores de firmas/paquetes/orden de inicialización en su productor; conservar referencias y respaldo de Evidence.
3. Completar los dueños críticos: DamageOnCollision, ConfigAttackLayer, colisiones y ScoreController/ResultsController; GameController.EnableSpecial; revisar los tres problemas de control/AI enumerados.
4. Ejecutar una prueba real **offline** desde DesarrolladoPor→StartMenu→HowToPlay→Character Selection→Bending Battle, con CPU Lv1. Comprobar inputs, contador, movimiento, ataques, defensa, impactos, puntos, victoria, recompensas, reinicio/retorno y guardado. Registrar logs/capturas. No sustituir esta prueba por la auditoría de importación.
5. Revisar visualmente shaders/UI/cámaras/material ausente. Probar la corrección de cámara con una aserción de pose/tiempo independiente.
6. Completar Survival, torneo offline, personajes/ataques restantes y AI Lv2/Lv3 y emparejamientos. Usar `Knowledge/game-coverage.json` (33 subsistemas), `game-spec.md` y las fuentes, actualizando cobertura real.
7. Mantener RAG fresco y guardar nuevos hallazgos con procedencia. Solo afirmar reconstrucción completa después de aceptación funcional y comparación con referencias. Los datos de escenas y nombres estáticos no establecen fidelidad global.

La solicitud del usuario sigue pendiente: no reducir silenciosamente «todo el juego» a una demo. El alcance inmediato elegido es offline; este es un **proyecto recuperado y parcialmente reimplementado**, no una entrega final jugable.


## Estado al cerrar esta actualización

Comprobado a 2026-10-15T21:57:55.1973687Z. IN_PROGRESS: Unity PID10736, inicio local2026-10-15 16:53:46; compilando shaders. NO hay resultado Windows confirmado ni player probe ejecutado.

RAG reconstruido después de terminar los tres trabajadores: **26 documentos,4388 fragmentos**. Ningún trabajador sigue escribiendo; no se programaron nuevos trabajos ni ejecución automática del player. El siguiente agente debe empezar por la sección LEER PRIMERO de este archivo.

## HUD and alternate controls ? 2026-10-16
- User mapping: WASD movement, left click normal attack, Space evade/shield/jump, right click super. Default arrows/Z/X/C unchanged. InputReader and desktop callback guard updated together.
- Battle camera eye now preserves source sprite aspect (219.85344 / 117.84775), with uniform scale. Runtime HUD inspection isolated three broken recovered SVGImage components (both Score/BG backdrops and Settings/Gear). Replaced the five battle SVGImage components (including two inactive Shield_Img images) with standard UGUI Image using the original sprite/color/rect fields. Earlier PlayerStats/EnemyStats hypothesis had no visual effect and was reverted. Scene backup: Evidence/hud-fix-backup/Bending Battle.unity.
- Previous control/flight acceptance reports belong to earlier assembly hashes; they do not validate the new mapping. Updated desktop probe source for future explicit control validation.

Final verification: Evidence/offline-smoke-probe-20261016-033453.json passed; inspected Evidence/offline-smoke-probe-20261016-033453.Bending-Battle.png confirms horizontal eye, restored gear and both rectangular score backgrounds with no oversized oval/vertical strip. Windows build succeeded0 errors (Evidence/windows-build-validation.json, generated08:34:45 UTC). Gameplay DLL6ca9bfe6325e59bcb7e3755f0725f62eb195d4dcbc4dd7c6cf6a4fe94efe5144; scene assets changed in this build, so the DLL hash alone does not identify visual asset revisions. Physical new control mapping was not separately exercised.

## Correcciones de la lista del usuario — 2026-10-18

Implementadas en UnityProject: reflejo de JumpR para Dante/Agata; tornados laterales Sora/Aura con trayectoria mundial fija tras .75s (se respeta el giro de CreateKickAttack); zoom adicional 25%; reparación de interpolación z en IncreaseSize mediante revisión del ARM64 retenido; ganador local a pantalla completa, sin cámara del perdedor/divisor/HUD de combate; SELECT CHARACTER con autoajuste en los botones que retornan a selección.

Ver `Knowledge/corrections-2026-10-18.md` para evidencia, archivos y límites. Roslyn editor/Windows development: 0 errores; auditoría de recursos aprobada (25 registros). El primer intento de Unity falló por licencia; al repetirlo, el player Windows se compiló correctamente y pasó el smoke test. No atribuir a este checkpoint pruebas anteriores a sus cambios. Probes y contratos actualizados; el escenario completo OctoberCorrections y la revisión visual siguen pendientes.

## Recompilación completada — 2026-10-18 21:57 UTC

La licencia se reconoció correctamente al repetir la compilación en la sesión del usuario. Player Windows actualizado: Succeeded, 0 errores, 43 advertencias (`Evidence/windows-build-validation.json`, log `Evidence/unity-windows-build-20261018-final.log`). El smoke antiguo esperaba la cámara anterior y botones táctiles activos; se adaptó al zoom out 25% y al callback de ataque de PC. Smoke final `Evidence/offline-smoke-probe-20261018-165734.json`: passed, sin errores; movimiento/cámara/ataque/CPU observados. Gameplay DLL SHA256 c15befd179d65f606030a0e4a061d0c281324a6f4ae84d4be7b62579a24823b7. Builds/Windows ya contiene los cambios del 18 de octubre. El escenario OctoberCorrections completo y la comparación visual permanecen pendientes.
