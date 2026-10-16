# Ability aiming and authored motion reconstruction

## Request and evidence scope

User asks that all twelve fighters' abilities be well designed and aim toward the opponent as in the supplied videos. Follow the REA skill with retained original ARM64/metadata and corrected Capstone mapping. No connected REA MCP tools are available; these are local static observations and reconstruction runtime checks, not REA server Evidence. Original libil2cpp SHA256 d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c. Cpp2IL printed disassembly has the known offset error; use corrected listings and ISIL only as an API identity aid.

Tools/audit-ability-dependencies.mjs traces twelve fighters,132 normal directional/Special states,164 referenced prefabs and109 source owners using actual Unity YAML and retained clip event references. It conservatively follows all external prefab references, including non-spawn references. Enabled components do not establish execution, and dynamic string-loaded resources remain outside the closure. Empty method flags are lexical candidates, not proof of missing native functionality. Native RET and four-byte tail branches must be distinguished.

## Direction is part of each move's design

Do not replace all motions with continuous LookAt. Native contracts include planar fighter facing, straight owner-forward launches, once-only launch aiming, horizontal lock after a delay, signed-dot homing, authored curved turns, local curve paths, fixed captured destinations, owner-bound effects and opponent-position summons. An area, shield, trap or self-heal need not fly toward the opponent.

- LookAt.Setup selects the other root by layer equality, not GameObject identity; same-layer helper objects resolve correctly. LateUpdate tracks the opponent in the horizontal plane except in Freeze. Its actual native method has one Freeze check; the unrelated multiple checks in printed Cpp2IL disassembly are invalid-address output.
- HomingAttack resolves a target, schedules activation, then turns using signed unnormalized cross/dot thresholds. Horizontal default40, authored speeds retained. Vertical comparison is asymmetric in the original and is preserved. It does not translate the projectile itself.
- LockToTarget obtains the rival at Start and schedules one planar Quaternion.LookRotation. Moving the opponent afterward does not turn it every frame.
- RotateToOpponent aims the two leaf attacks once at launch and includes height. Update is a native RET.
- TurnAttack uses authored delay/duration and speed multiplied50, yaw direction chosen by right. Target position is read but not used for steering. Keep this curved trajectory rather than converting it to homing.
- FollowOwner copies position AND rotation of the root selected by actual fighter layer. Rotate applies authored degrees per physics tick after delay, without deltaTime multiplication.
- MoveToTarget is a curve movement owner, despite its name: capture world origin at Start; increment elapsed before evaluating X/Y/Z curves; transform their force-scaled vector by CURRENT orientation. Three flower prefabs use it. No opponent lookup.
- MoveRelativeToRotation captures a world destination from initial orientation, waits, interpolates and snaps at completion. MoveObjectLocal2 instead captures local origin after delay and moves to origin+AddVector in parent space.
- CreateObject preserves delayed creation, weak-owner rejection, rotated local offset, root layer and optional material adaptation. CreateObjAnim spawns the supplied animation object at the owner pose; does not invent ownership propagation or parenting.
- EnableByDistance uses world-origin stage distance (strict outside), not distance to opponent. ShadowBatAtk has missing authored object references; guards prevent null faults but no target object was invented. DropAtk falls per physics tick only while ConfigAttackLayer.playerWeak, not unconditionally.
- ChangeStartRotation applies the authored local Euler vector at Start.

## Explicit compatibility adaptations

Current SummonAttack copies fighter layer9/8 to spawned roots, while recovered HomingAttack/LockToTarget compare the named PlayerAtk layer11. Recognize either the named attack layer OR the current MainCharacter.layer as player ownership, choosing the opponent; other ownership chooses MainCharacter. This adaptation prevents current offline player projectiles from targeting the player and is not a claim of literal native equivalence. Test both fighter and named attack layers, on both sides. Required-reference safeguards and omitted FollowOwner per-frame debug logging are documented deviations. No authored prefab values, GUIDs or immutable exports are changed.

## Video observations

Source directory C:/Users/Jose/Desktop/true masters videos gameplay y menú. The all-characters video is1003.2seconds and demonstrates an older five-element roster; it does not supply unambiguous examples of every current twelve-fighter ability or awakened variant. Existing Knowledge/video-analysis.md and video-findings.json preserve this boundary.

Additional 2x2 sheets extracted using the existing ffmpeg9.0.2 at nominal three-second intervals: Evidence/ability-video-air.png from6s, water from205s, earth from405s, fire from670s and lightning from855s (four frames each). Water/earth/fire/lightning sheets inspected directly, plus existing single-start-1-23.png for air. Visible effects and poses broadly support attacks launched toward a rival, ground formations and self-centered charge effects. Sparse frames do not establish steering equations, hit timing or all ability designs. Native code/authored data supply those finer contracts; full visual parity remains unaccepted.

## Runtime validation method and history

AbilityAim uses actual normal attack UGUI PointerEnter/Exit and recovered animator entry contracts, temporary ownership and disabled CPU. Opponent placement/reset is synthetic, varied by bearing. Root orientation is checked for both fighters across four bearings, with same-layer helper target selection. Component fixtures copy all15 authored HomingAttack,6 LockToTarget and3 curve profiles and test both fighter layers and named attack layers. Reflection invokes controlled component methods/iterators to isolate exact math; these are not natural damage outcome tests. Supplemental fixtures cover leaf launch direction, event/deferred producer semantics, weak rejection, curve frame math, owner following, per-tick rotation, curved turns, weak descent and strict stage-boundary activation.

Initial report offline-abilityaim-probe-20261016-000113.json passed its component checks but failed first Sora attack entry: dispatching UGUI from end-of-frame did not enter the state. Resuming the normal coroutine phase before dispatch corrected the observer. Provisional offline-abilityaim-probe-20261016-000519.json passed Sora's ten routes, eight facing cases, sixty homing-profile cases,twenty-four lock cases,twelve curve cases and four grouped supplemental/motion cases, errors empty. This does not establish all-roster final acceptance. Later source adds full per-frame post-LateUpdate facing and live homing target checks plus per-move screenshots; a fresh build and all-roster run are required.

## Remaining acceptance boundaries

Do not claim every skill hits, exact original VFX/shader/art parity, all awakened forms, all target movement/height/edge/interrupt interactions, online targeting or Android from these fixtures. Video comparison is sampled qualitative context, not registered frame parity. Current runtime result/build identities must be appended after final validation and retained in HANDOFF and a hash-bound acceptance contract.

## Additional native motion owners restored

PingPongMove.Start is a four-byte tail branch to FirstMove, not RET. It alternates existing manager-owned local-position tweens start/finish and reverse, using authored duration/EasyOut. destroyTime is unused in native and remains unused. Public LerpLocalPosition starts the same manager coroutine instead of directly calling its private iterator, an explicit implementation adaptation.

DropWhenOwnerWeak queries the owner's Weak animator bool every0.25seconds, latches drop and disables FollowOwner; descent is up*speed*.35 per render frame, not per second. RotateByAngle waits, captures LOCAL rotation and interpolates from start to start*Euler(targetRotation), increments elapsed before Clamp01/Slerp, then snaps exactly. Default target(0,90,0),duration1,delay1. These bring this checkpoint to eighteen restored/corrected source owners. Native evidence per class and three scoped reconstruction notes remain in Evidence.

### Inventory versus reachable normal routes

The full Resources inventory contains15 HomingAttack and6 LockToTarget profiles, but the conservative132-state normal Attack/Special prefab closure currently contains ZERO enabled instances of those two owners. Their96 combined profile/curve cases therefore establish restored serialized component contracts, not natural homing acceptance in the current normal moveset. Live homing monitoring remains installed and must not be interpreted as positive coverage if its entity count stays0. Current natural routes chiefly use fighter facing plus straight launches, curved turns, owner-following effects and opponent-position summons. RotateToOpponent appears on Maya's two leaf routes; TurnAttack on Sora/Aura RightAttack; PingPongMove on Vinea/Eliot DownAttack2. Do not sell legacy/alternate profile restoration as a newly homing normal moveset.

### Particle render defect discovered during this request

First all-roster AbilityAim captures predate the shader correction. Sora BasicAttack showed black rectangular particle backgrounds. Exact YAML bindings trace five AirAtk particle renderers to Air_MAT/Air_MAT2 and Custom/AdditiveTintable2. The dust texture has a black opaque background; smoke uses transparency. Both custom additive shader exports were opaque DummyShaderTextExporter passes, ignored vertex/tint color and lacked blending/depth-write settings.

Tools/restore-ability-particle-shaders.cjs preserves original shader bytes under Evidence/ability-particle-shader-backup and replaces only these two working shaders with an approximate Built-In transparent SrcAlpha/One additive pass, vertex color/tint, no depth writes, two-sided quads and optional soft-particle/fog behavior. This is a reasoned rendering repair, NOT recovered original GPU source. Exact hashes/classification in Evidence/ability-particle-shader-reconstruction.json. All-roster motion observations before this render repair stay bounded code/aim evidence; inspect a post-repair rerun before claiming the black-quad defect resolved visually.

### Actual normal-projectile steering owner

Follow has55 enabled state-component references across45/132 states and33 unique prefabs. GoForward has85 enabled occurrences across62states and45prefabs. These are static reference counts, not execution counts. Their retained offline sources match native: Follow selects opposite root by MainCharacter.layer equality, multiplies speed52.75, turns by signed cross/dot each Update and destroys only itself after positive stopAfter; GoForward keeps moving after steering removal and honors delay/Stop/weak motion. This checkpoint does not change those already-restored gameplay sources.

Final validation source now checks live Follow.target after LateUpdate throughout each move (in addition to root facing and optional legacy HomingAttack targets), and includes deterministic Follow/GoForward math fixtures on both fighter layers. Super validation also records root-facing/Follow targets across awakening and phase1 super. Earlier001030 lacks these new assertions. Current final build Evidence/windows-build-validation-ability-aim-20261016-052221.json succeeded0errors39warnings, assembly25d345dad47d9c306c1dc04558a19515665eb75d069aeedfedc77a09b40c9d1b. Final all-roster report offline-abilityaim-probe-20261016-002256.json passed/errors=[]. Sora basic repaired image inspected: additive dust no longer covers the arena/rival with opaque black quads. Sora/Vinea contact sheets inspected during final run; image poses are sampled, not complete move playback.

## Final normal-attack acceptance

Evidence/offline-abilityaim-probe-20261016-002256.json PASSED/errors=[] on the final052221 build/25d345 assembly in610.2831139seconds. All120 neutral/cardinal alternating input routes completed and recovered through actual UGUI/animator owners, with synthetic varied opponent placements and disabled CPU.96 root-facing cases cover both sides/four bearings/same-layer helpers.60 legacy homing-profile cases,24 planar lock cases,12 flower curve cases, two each of supplemental/motion/actual Follow/local groups and one live local ping-pong group passed.

All120 moves recorded post-LateUpdate root-facing samples; maximum0.02797646degrees. Live Follow.target assertions passed,47 accumulated per-move entity-samples (not globally unique projectiles; lingering objects may appear in more than one case). Legacy HomingAttack runtime counts stayed0, consistent with its absence from the132-state closure. Save/preferences fixture restoration is explicitly recorded.120 PNGs and twelve5x2 contact sheets generated; all twelve contact sheets reviewed. Some samples precede delayed producers or show effects after they travel beyond the camera, so a quiet image does not establish a missing move. These are qualitative samples rather than full visual parity.

The final super/awakening regression offline-specials-probe-20261016-003331.json PASSED/errors=[] on the SAME assembly25d345; see final super acceptance below.

## Final super acceptance and checkpoint

Evidence/offline-specials-probe-20261016-003331.json passed/errors=[] in458.3812785seconds: all twelve playable fighters entered awakening through actual Special UI, completed native decharge/phase transition, then executed one phase1 super. Charge was accelerated; discharge was allowed to complete. Twelve special-facing/live Follow target observations passed. Earth moving rocks and Maya healing/wings/life display passed. Both final scenarios explicitly restored fixtures. Final build052221 and assembly25d345dad47d9c306c1dc04558a19515665eb75d069aeedfedc77a09b40c9d1b match both runtime launch manifests and current executable data. No runtime errors.

This acceptance checkpoint was superseded by the awakened-normal follow-up below, which adds actual awakened normal routes, a separate super-facing window and current-build contact sheets.

## Awakened normal-attack follow-up

REA YAML/clip-event audit `Evidence/awakened-attack-event-audit.json` checks the retained 12-controller closure. Authored `CreateAwakeAtk` animation callbacks occur on Raven's `roll`, `JumpR` and `JumpL` clips, and Maya's `JumpR` and `JumpL` clips; the other ten controllers have none. This is static event evidence only and does not imply that the other fighters lack all awakening bonuses. Runtime execution of those five jump-triggered projectiles remains pending.

The final harness now performs one real normal-attack input per fighter while `AnimatorStatesManager.awakening` and `SpecialAtk.decharging` are both active. It selects the next native alternation for a varied cardinal/basic route, places the CPU at a synthetic bearing, waits for the authored Animator state and recovery, and samples facing in the post-LateUpdate window. `Evidence/offline-specials-probe-20261016-013210.json` PASSED/errors=[]: 12/12 awakened routes entered and recovered with awakening still active; maximum facing error0.02797646 degrees. The same run then allowed natural decharge and exercised one phase1 super per fighter. Super facing uses a separate observer window (361–374 frames per fighter); maximum error0.02797646 degrees. Charge remained fixture-accelerated, and enemy movement/impact was not accepted.

Matching all-roster normal regression `Evidence/offline-abilityaim-probe-20261016-012136.json` PASSED/errors=[] on the same gameplay assembly. It passed120 actual input routes, all previous root/legacy profile/motion checks, and restoration of the original save/preferences. The build is `Evidence/windows-build-validation-awakened-normals-20261016-012047.json`, Succeeded with0 errors/34 warnings; gameplay assembly SHA256 `93a66d4f0d01754eb4d65afabc54142e77e3b9fd236256a60abbe6e103761722`. The hash-bound result is in `Evidence/ability-aiming-acceptance.json`.

Generated and inspected all twelve final-build contact sheets `offline-abilityaim-probe-20261016-012136.contact-<fighter>.png`; 120 move captures,12 awakened captures and12 super captures are retained. The sheets show authored attacks/effects at sampled times, not a full playback or pixel-match against the older video. Remaining high-priority validation: moving target, height/edge/interrupt, actual impacts and match completion, plus dynamic activation/aim checks for Raven/Maya's authored awake-jump projectiles.
