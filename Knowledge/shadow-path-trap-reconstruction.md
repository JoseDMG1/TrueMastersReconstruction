# Raven local motion and trap power-up

REA skill applied to retained native evidence. This checkpoint is **native-derived source restoration; Unity build and runtime acceptance remain pending with the parent agent**. Native input SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`. The corrected ELF VA mapping, not Cpp2IL printed Disassembly, owns branch/constant interpretation. Named ISIL identifies Unity calls and is checked against corrected ARM64. Reproducible inventory: `node Tools/audit-shadow-path-contracts.cjs`; complete fields, source digests, native listings, literal/generic resolutions and 25 serialized prefab components are retained in `Evidence/shadow-path-native-contracts.json`.

## Local presentation motion

`MoveObjectLocal.Start` VA `0x129C308` schedules StartMoving after delay. Until then moveFinished=true. StartMoving (`0x129C45C`) records the current **localPosition**, adds AddVector when its squared magnitude is at least Unity's `9.99999944e-11` zero comparison threshold, multiplies speed by **52.75**, and schedules Finish after finishMove. Update (`0x129C358`) uses `Vector3.Lerp(currentLocalPosition, positionEnd, speed*Time.deltaTime)`; Finish sets moveFinished=true without snapping to the endpoint. Defaults recovered from constructor: speed=.5, finishMove=.55, moveFinished=true. No root forward motion is provided by this component.

For a constant frame delta, after N updates remaining local displacement is `(1-clamp01(52.75*serializedSpeed*deltaTime))^N` times the initial offset. Finite finishMove can stop before exact convergence. Two components on the same transform both run their own schedules; do not treat their differing directions as contradictory serialized data.

- ShadowBird child first rises +3.5 localY after .3s, speed .075, finish1s. A second child motion drops2 after20.5s at speed.015, finish1s. The latter may outlive the attack's lifetime and requires a fixture that retains the component naturally or explicitly states a prolonged observation setup.
- ShadowSpider and ShadowWolf children drop2 after10.5s, speed.05, finish1s. A short directional move observation cannot accept these delayed paths.
- Other reached consumers: JumpBranchL/R each have two motions (+1 after.1/speed.1/finish.55 and -1.1 after.5/speed.07/finish1), RocksPrison/DownEarthAtk2 +.5 after.2/speed.2/finish.55, and two ShadowHandChair components with delay0/speed.5/finish.55. These are consequences of restoring the shared original owner; no prefabs were altered.

## Trap detection, aim and one-time power-up

`TrapPowerUp.Start` (`0x12CEBC0`) sets directionChanged=true and invokes RestoreDirectionChanged after startDelay. Update (`0x12CEC18`) checks only after that flag becomes false. Offline CheckIfTrapNear (`0x12CECBC`) scans the **same owner's attacks**: layer9 uses MainCharAttacks (+0x20), other layers OpponentAttacks (+0x28). It requires strict 3D distance `< limitDistance` via existing Utilities.AreObjetsNear and **Description.effect == "ShadowTramp"** (+0x40), not ReleaseBy. It instantiates TrapFX at the projectile pose, calls ChangeDirection, then PowerUpAttack. The trap is not consumed. It does not break the current enumeration after a match, so multiple nearby traps can create multiple FX in that call. Later detection is blocked until reboundDelay expires.

ChangeDirection (`0x12CF12C`) looks toward GuestPlayer for layer9, HostPlayer otherwise; converts the resulting quaternion to positive Euler degrees, then sets yaw-only `Quaternion.Euler(0,yaw,0)`, sets directionChanged=true and schedules its reset. The directly required Utilities.QuaternionToVector3 (`0x12DB80C`) was restored alone using Quaternion.eulerAngles, matching Internal_ToEulerRad -> Rad2Deg57.29578018 -> Internal_MakePositive. No unrelated Utilities stubs were changed.

PowerUpAttack (`0x12CF2C8`) is once per component instance: Damage+=1, Defense+=1, then **Damage capped at3**, with no defense cap. Optional ScaleObject.localScale *=1.25. powerUpFX activates. If Jump=true it enables the same GameObject's jumpSimulator and sends the real Animator.SetTrigger("Jump") call (`0x2363508`). A repeated trap encounter redirects and shows TrapFX but does not stack damage/defense/scale. Native constructor defaults limitDistance4, startDelay.5, reboundDelay.5. Serialized values take precedence.

All eight consumers use limitDistance4. Their start/rebound delays: Arrow and ArrowDown0/.3, Bat.5/.4, Bird1.2/.5, Shark0/.7, Snake.9/1, Spider0/1, Wolf1/.5. Only Spider sets Jump=true. Raw object references, child IDs and flags are in the inventory.

## Direct jump dependency

`jumpSimulator.Start` (`0x123F588`) leaves the offline component intact; the original online nonowner removes its component, not the GameObject. Update (`0x123F64C`) sets absolute **worldY** from `animCurve.Evaluate(timeElapsed/duration)*force`, retaining current worldX/Z, then advances timeElapsed by deltaTime. When timeElapsed>duration it writes Evaluate(1)*force and schedules component destruction; native execution still falls through the subsequent gate and sample on that final frame. This order was preserved.

The native delay gate advances no clock while timeElapsed<delay. Positive delay therefore leaves this clock stationary; do not silently implement a conventional delay countdown. All five retained prefab consumers have delay0. Spider is initially disabled and configured duration1/force1.5 with a two-key curve; trap enables it. Its final Y is around-.0036 from the authored near-zero final curve value, not its initial Y. Shark has duration2.1/force20 and an initially disabled jump component (its TrapPowerUp.Jump=false); LeafAtkChild has duration1.5/force15 and is enabled. FlowerAtkL and FlowerSumonerR carry enabled jump components with empty curves/duration0/force0. Their first sample therefore passes timeElapsed/duration=0/0 to AnimationCurve.Evaluate exactly as the native method; Unity's handling and resulting position/errors require runtime verification. No invented fallback constants or asset edits were introduced.

## Acceptance fixtures still required

1. Capture each local component's creation localPosition, then check no motion before delay, the clamped-Lerp step after delay, and stable localPosition after Finish. Compare to local rather than world coordinates; root motion can mask the distinction. Short-lived attacks need samples before destruction.
2. Use an actual recovered Raven projectile and same-owner ShadowTrapFar/ShadowTrap with effect ShadowTramp. Keep unrelated attacks out of the near region. Confirm no change before startDelay; after entering `<4`, expect FX, yaw toward the opponent with zero pitch/roll, Damage=min(original+1,3), Defense=original+1, scale ratio1.25 and powerUpFX active. At exactly4, no power-up. An opposing-owner trap alone must not power up this projectile.
3. Keep the trap near, alter opponent heading and observe another redirect only after reboundDelay. Damage/defense/scale must remain at the one-time result; trap must remain present unless some other owner destroys it.
4. Spider branch: disabled jumpSimulator becomes enabled; Animator Jump trigger uses the controller's real transition. Sample root Y against the authored curve for approximately1s while confirming current X/Z are retained by the jump component. Confirm jump component disappears after duration, projectile GameObject remains subject to its other lifetime owners.

These are proposed checks, not runtime observations. Source restoration preserves serialized names, fields, script GUIDs, prefabs and native lifecycle; no Unity processes were launched by this worker. Offline Update and detection deliberately return when GameController.online is true because Photon authority and RPC transport remain deferred. ShowOnlineFX implements its local receiver body (PowerUpAttack and TrapFX creation), which does not establish functioning networking.

## Subsequent executed bounded Raven acceptance

The full120-route run185330 included all ten Raven inputs without reported
runtime errors. Fresh Raven run192535 subsequently passed ten routes on assembly
14657c9e903a432a7dfd0deb73d40e227be51693f6af71771c86bfd7157e45b4.
Its probe observed the owner's private powered flag read-only, rather than
inferring completion solely from an effect GameObject's active state.

Four actual routes produced observed power-ups through normal trap detection:
DownAttack2 ShadowSnake damage2->3/defense1->2; RightAttack2 ShadowWolf1->2/0->1;
LeftAttack ShadowBat1->2/0->1; LeftAttack2 ShadowBird1->2/0->1. All four had the
power-up FX active. CPU behavior was disabled and the generic move fixture
reset actor positions between moves; this is not general natural aiming proof.

The prior192136 observer compared current scale to creationScale*1.25, producing
false comparison results. Creation pose is not the operation-entry pose, so
that comparison cannot establish an incorrect native multiplication. The new
report retains creation/current scales as context, explicitly leaves exact
scale multiplication unaccepted, and independently verifies damage/defense.
No gameplay change was made to accommodate the observer.

General motion records for child components in these reports are WORLD-space
displacement. Root movement can contribute; those records do not separately
accept MoveObjectLocal's local curve, its long delays or its finish time. Exact
local curves, one-time/no-stack behavior, strict distance boundary, opposite
ownership, cooldown redirection, Spider jump and graphical/audio parity remain
pending dedicated fixtures. No-error Maya routes exercised exported zero-duration
jump consumers without reported errors in that bounded run; no engine edge-case
or global finite-value proof follows from it.
