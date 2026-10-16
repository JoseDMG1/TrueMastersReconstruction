# Offline defense reconstruction

Current correction: the collision producer used the wrong Description string.
DamageOnCollision.OnTriggerEnter now binds Description.effect once and passes
that same value to reaction dispatch and onDamageRecived exception lookup.
ARM640x1284058 loads Description+0x40 intoX21;0x12840EC passesX21 to
HitAnimation and0x1284124 passes it to List.Contains. Metadata establishes
Description.ReleaseBy at+0x38 and Description.effect at+0x40. The earlier
ReleaseBy interpretation in Knowledge/projectile-reconstruction.md is corrected.
All native references use ELF SHA256
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c` with the
corrected VA/raw mapping. Cpp2IL printed Disassembly is not used.

This fix is source-derived and awaits a fresh build/runtime defense probe.
Earlier overlap probes that observed+.5 damage did not validate reaction routing:
AirAtk.ReleaseBy=Player while effect is empty, so the wrong field took an unknown
named-effect branch. Their startup/round observations remain bounded historical
results; do not claim they exercised the corrected reaction or shield behavior.

## Entry, hold and release

Tools/audit-defense-controllers.cjs uses the existing format-aware YAML parser;
Evidence/defense-controller-static-audit.json retains controller input hashes,
Block/BlockHit/hitBlocking state behaviours, clip callbacks, exits and enclosing
state-machine entry conditions for all seven element controllers.

Real battle UI B is GameObject280/EventTrigger1435. PointerEnter(eventID0) calls
InputReader.SetBButton(true) and GameController.Jumping(true); PointerExit(1)
calls both with false. Stationary Horizontal/Vertical=0 plus Jump=true enters the
Evade submachine, whose default state is Block. Horizontal/Vertical outside
[-.1,.1], or Jump=false, exits the Block submachine. These conditions establish
a real input path; directly playing Block bypasses it.

AnimatorStatesManager.Block native0x127EFA8 only sets the Block trigger, matching
the existing implementation. No m_ConditionEvent Block was found in the seven
examined controllers, so that trigger alone is not established as a viable state
entry for these assets. Record its result separately if tested; use the serialized
B input route for an acceptance probe instead of forcing Animator.Play(Block).
AI.Block independently sets Jump through SetAction(0,0,Jump), prevents duplicate
requests for1.5s, and chooses Jump instead when shieldDamage>=5.

Block state has Stunned, StopBlocking, SendAnimatorValues and Recover behaviours.
Recover.keepBlocking=true on all seven controllers; StopBlocking.OnStateEnter
sets Blocking=true and invokes the correct elemental shield owner. Water/Fire/
Earth/Light/Shadow/Leaf clips also call blocking, at0 or.033333335s; Air Block clip
has no such event and relies on the state behaviour. DamageOnCollision.blocking
native0x12871E8 and StopBlocking0x1287240 only set the Animator Blocking flag.

StopBlocking StateMachineBehaviour exits without switching off the shield when
current state is BlockHit; otherwise it clears Blocking and dispatches shieldOff.
Air BlockHit has Stunned behaviour and transitions back to Block at exitTime
.67391306, duration.25. Other controller details are retained in the audit rather
than assumed identical. Elemental shield owners were restored earlier; their
GPU/audio fidelity and every block-hit route still need runtime verification.

## Collision reaction and shield break

DamageOnCollision.HitAnimation native0x1284798 uses Description.effect, not the
attack creator identity ReleaseBy. Blank/left/right effects are resolved by the
native angular rule; Pos uses relative position; Front normalizes to empty and
is useful as an explicit test fixture to remove angle ambiguity.

For a normal non-awakened, non-Weak frontal hit, it first calls GetDamage(.5),
then computes effective strength:

    attackDamage - (Blocking ? 1 : 0)
      + (Damage > 5 ? 1 : 0)
      + (Damage > 10 ? 1 : 0)
      + (Damage > 30 ? 1 : 0)

Damage thresholds use the value AFTER that initial+.5. Strength0, or strength1
while Blocking, pushes backward and plays BlockHit. If strength>1 while Blocking,
it emits ShieldExplotion, clears Blocking and adds another.5; the subsequent
not-Blocking branch adds another.5. Thus a clean low frontal block can still
add.5 Damage, and a clean heavy break can add1.5 total per accepted collision.
No assumption of zero HP/damage on a block is valid for this implementation.

Strength1/2 plays hit1, strength3 hit2, otherwise Fall3 (apart from strength0
BlockHit). Descriptions Damage1 and3 at low accumulated damage give controlled
strength0 and2 while blocking. Side/back handling and awakening differ; Air has
extra side/back shield handling. They require separate tests rather than applying
this frontal expectation to every collision. ShieldExplotion/DestroyShield is an
explosion-effect spawn; its name does not establish shield GameObject destruction.

## Shield bar and legacy recovery scope

DamageShield(int) native0x1287298, SetShieldbarDamage0x128734C, RestoreShield
MoveNext0x12884E0 and the existing source agree:

- Ignore DamageShield while restoringShield. Otherwise add the supplied integer
  to shieldDamage, then update the selected StatsController bar.
- At>=5, start a30s scaled wait, set restoringShield and ChangeButton(1,B).
- After30s, reset shieldDamage/restoringShield, update bar, ChangeButton(0,B).
- RestoreDamage0x1287CA8 resets Damage and shieldDamage together, updates the bar
  and clears restoringShield. ResetDamage stops this owner's coroutines before
  restoring and resetting B presentation, which is important across respawns.
- SetShieldBar sets X=5-min(5,shieldDamage), Y2/Z1. Stats ChargeShieldBar, started
  by ChangeColorIcon(1,B), visually refills X by.0105 every.02s, so visual full can
  precede the30s owner recovery. The visual coroutine does not clear shieldDamage.
- Ordinary low/high blocked hits do not call DamageShield in the native
  HitAnimation body, and no such call was added to the reconstruction.

Tools/find-native-directcalls.cjs scans BL/B targets in all mapped application
method bodies. Evidence/defense-native-directcalls.json reports zero direct calls
to DamageShield0x1287298, one call to RestoreShield from DamageShield, and bar
updates from DamageShield, RestoreDamage and RestoreShield.MoveNext. A bounded
serialized clip/UnityEvent name search also found no DamageShield callback.
This does NOT prove unreachable code: virtual/delegate/reflection and unexamined
engine invocations are outside this scan. Natural shield-meter depletion remains
unestablished. A reflection/direct DamageShield fixture can validate the owner in
isolation, but must not be reported as a naturally reached combat feature. Do not
invent a5-hit cooldown mechanic or add DamageShield to blocked hits.

## Concrete parent runtime plan

1. Start a normal offline low-damage fighter, disable CPU actions as an explicit
   probe condition, retain player/CPU layer identities. Enter B via real
   PointerEnter, stationary; wait for current/next Block and Blocking=true plus
   actual shield owner activation. Release PointerExit and observe exit/recovery.
2. Re-enter block. Instantiate a recovered AirAtk and retain its actual collider,
   ConfigAttackLayer/Rigidbody lifetime; assign opposing owner layer. Explicitly
   fixture Description.effect=Front to remove angle ambiguity; keep Damage1.
   Cause one physics overlap and record collider entry/event count, Damage delta,
   Animator BlockHit/current-next, Blocking, shield-owner state and bar state.
   Do not call HitAnimation directly. One accepted frontal collision should yield
   the low-strength branch; repeated collider entries must be counted separately.
3. After stable block recovery, repeat with the same recovered projectile carrier,
   explicit effectFront and fixture Description.Damage3. At low accumulated damage
   expect strength2, break/Blocking=false and hit1 with the native damage sequence.
   Check real animation recovery clears Stunned and allows B re-entry; do not
   clear Animator flags just to make the test pass.
4. If testing the shield meter, isolate/document its direct owner fixture, await
   natural30s scaled recovery, and distinguish early visual refill from owner
   shieldDamage/restoringShield. Restore save/preferences/temporary mutations.
5. Repeat representative elemental shields and Air side/back routes after the
   frontal contract passes. This does not replace every move/defense pairing.

No Unity/build was launched by this worker. Source changed only at the confirmed
collision-field producer; no shield damage, input route or timing was invented.

## Description field boundary: related consumer cleanup completed

After OnTriggerEnter's wrong string field was demonstrated, the remaining three
implemented `.ReleaseBy` consumers were audited at their producing Description
loads rather than patched by downstream reaction exceptions. All three also read
native offset+0x40, and now use effect:

| Owner | Corrected native load sites | Behavior |
|---|---|---|
| DestroyIfSameObjIsNear.DestroyNearAttack |0x1227944,0x1227964| Compare candidate/own effect before cleaning prior nearby duplicates |
| DestroyOnCollision.OnTriggerEnter |0x128A254| byTheSameObject checks effect ShadowTramp; exact literal slot0x28168C8 |
| SummonAttack.CreateKickAttack |0x12B5CB8,0x12B5E08| Dispatch LeftAttack/RightAttack effect; slots0x28157E8/0x28157D8 |

DestroyOnCollision had a related wrong literal as well as the wrong field:
ShadowTramp is the resolved native value, not branchPrison. The branchPrison
exception in DamageOnCollision.Awake is a separate legitimate effect and remains
unchanged. ReleaseBy itself remains a public serialized field with its .meta and
asset values preserved; the audit does not claim it has no original purpose.

Tools/audit-description-consumers.cjs retains field offsets, exact resolved
literals, all four owner source/native/.meta SHA identities and bounded source
search results in Evidence/description-field-consumer-audit.json. The completed
search found zero remaining `.ReleaseBy` reads in existing Assembly-CSharp C#.
Unimplemented native/online/LM consumers are outside that source search.
Knowledge/projectile-reconstruction.md has been corrected at every related
statement so retrieval cannot propagate the earlier field/literal interpretation.

Fresh runtime validation is required after these producer corrections, including
kick spawns, nearby effect deduplication, and ShadowTramp self-collision cleanup.
Earlier basic-projectile creation alone does not establish those corrected paths.
No additional speculative rule, material or scene change was introduced.

## ForceShieldOff restored: prison/freeze interruption owner

ForceShieldOff.OnStateEnter was an exported empty stub, not a native no-op.
Corrected ARM64 entry0x123287C is0x4D4 bytes. It first sets Animator Blocking=false
via SetBool0x2362FF4 (call site0x12329B4), reads Description.element at+0x28 and
invokes the actual element shield owner's off method:

- Air AirShieldController.shieldOff (call0x1232A24)
- Water WaterShieldController.shieldOff (0x1232A70)
- Earth ShieldController.shieldOff (0x1232ABC)
- Fire ShieldMannager.shieldOff (0x1232B08)
- Light LightShieldManager.shieldOff (0x1232B54)
- Shadow ShadowHandController.ShieldOff (0x1232BA0)

There is no native Leaf branch. The reconstruction preserves this absence instead
of broadening the switch to mimic StopBlocking. After element dispatch, it finds
the direct child ShadowHandGrab(Clone) and, if present, calls its
ShadowHandGrabController.ForceDisapear (0x1232C48). This is an unconditional state
interruption owner; StopBlocking's special BlockHit exit exception does not apply.
The final native branch sends Photon StopNetBlock when online and locally owned;
that service path remains unreconstructed, while the local effects are restored.

Tools/audit-force-shield-off.cjs retains literals, resolved generic component
owners, source/native/.meta SHA identities and controller bindings in
Evidence/force-shield-off-native-bindings.json and
Evidence/force-shield-off-state-bindings.json. Literal slots include Blocking
0x28154D0, ShadowHandGrab(Clone)0x28154A0, StopNetBlock0x28154C8. Generic slots
0x2815470/98/88/90/78/80 resolve the six off owners;0x2815468 resolves the grab
controller;0x2814830 resolves Description.

Bounded inspection of direct PrisonFree/Freeze behaviours on the seven fighter
controllers found ForceShieldOff, Stunned, StopGo and SendAnimatorValues, plus
WhipOff on Grass. Stunned/StopGo are already restored. WhipOff.OnStateExit is a
true native RET at0x12DD7D4 and remains empty. SendAnimatorValues.OnStateEnter
is not RET, but native0x12BC834/838 checks GameController.online and returns
when false; its current empty implementation therefore does not omit an offline
local effect in these states. Its network path remains unimplemented. The audit
also records the separate PlayerAnimator controller's different bindings without
claiming that controller is used by the twelve normal fighter prefabs.

No other demonstrated active offline stub was expanded in this bounded pass.
ForceShieldOff has no serialized fields; its .meta identity is unchanged.
No Unity/build was launched. A fresh prison/freeze probe should start with a
visible shield, enter PrisonFree/Freeze through a real attack and observe
Blockingfalse, element owner shutdown and the actual trapped-child disappearance
continuation. This source restoration alone does not establish that runtime path.
