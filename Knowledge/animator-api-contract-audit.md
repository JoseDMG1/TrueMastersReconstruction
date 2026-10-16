# Animator native API contract audit

The exact native call target0x2364318 is Animator.Play(string), not SetTrigger.
The Maya branch-prison runtime exposed the earlier mistaken interpretation in
one owner; this pass audits every other direct application caller and fixes the
additional demonstrated misuse. Native ELF SHA256:
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.
The mapping is specific to this binary and is not a general Unity address table.

## Proof owner and reproducible evidence

Tools/animator-native-call-contracts.json now owns six verified engine mappings.
Tools/inspect-ai-flow.cjs reads those mappings instead of carrying its own partial
Animator write/Play interpretation. Each contract names an exact corrected ARM64
site plus a retained named ISIL call. Tools/audit-animator-call-contracts.cjs checks
those two independent representations and enumerates BL/B sites using
Tools/find-native-directcalls.cjs. Output:
Evidence/animator-call-contract-audit.json retains proof lines, paths/hashes,
all mapped direct sites and explicit source review findings. It does not infer
correctness from C# source string counts or animation state names.

| Native target | Confirmed API | Representative ARM64 proof |
|---|---|---|
|0x2364318|Animator.Play(string)|LeafShieldController.ShieldHit0x1240FA0|
|0x2364344|Animator.Play(string,int,float)|AnimatorStatesManager.PlayAnimation0x127F138|
|0x2363508|Animator.SetTrigger(string)|GameController.Special0x1293774|
|0x2362FF4|Animator.SetBool(string,bool)|DamageOnCollision.blocking0x1287238|
|0x2363354|Animator.SetInteger(string,int)|SpecialAtk.ResetCharge0x12B119C|
|0x2362AC4|Animator.SetFloat(string,float)|DamageOnCollision.RestoreDamage0x1287CF0|

For example AnimatorStatesManager.PlayAnimation passes layer=-1 and time=0 to
0x2364344, matching its existing explicit Play overload; ShadowHandController's
StopEnemy and ShadowBlockHit pass layer0/time0. GameController's Special and
counter calls to0x2363508 really are SetTrigger and were not changed.

## Every direct Play(string) application site reviewed

Sixteen call/tailcall sites were found across the4146 mapped application methods.
Every site's retained ISIL identifies Animator.Play, and the corrected native
call target is0x2364318. No engine/indirect/delegate/reflection callers are claimed
covered by this bounded scan.

| Owner/method | Site(s) | Source review/result |
|---|---|---|
|BranchPrisonCharController.PlayCharCrushAnim|0x121AC6C|Reserved worker corrected Play(PrisonExplosion); see branch-prison report|
|DamageController.PlayAndSendHitAnimation|0x1225C1C|Unreconstructed LocalMultiplayer owner; method remains stub|
|EarthShieldManager.StartShield/FinishShield/ShieldBreak|0x122A554,0x122A5EC,0x122A664|Unreconstructed IShield owner, methods remain stubs; attached in GameObject/EarthShield.prefab|
|LeafShieldController.ShieldHit|0x1240FA0|Corrected SetTrigger(Hit) to explicit Play(Hit)|
|AI.NewAI|0x125268C|Already Play(Special); preserved|
|DesktopInputs.Update|0x1288F08|Restored omitted key-up K -> Play(Taunt2), along with its contiguous native J -> GameController.Special shortcut|
|ScoreController.Dance|0x12A53C4|Already explicit Play with native winner-dependent state; preserved|
|Respawn.CreateNewChar|0x12A7C88|Already Play(FallingToLanding); preserved|
|SettingsOptions.Close/OpenSettings|0x12BD088,0x12BD0F0|Reserved worker corrected Play(SettingsSlideOut/In)|
|StartCounter_LM.StartCounter|0x12C3B24|Unreconstructed LocalMultiplayer scene owner; remains stub|
|WhipController.Right/Left/Iddle|0x12DD41C,0x12DD3A0,0x12DD58C|Reserved worker corrected Play(RightAtk/LeftAtk/Iddle)|

No unrelated LM implementation was introduced merely to fill the five stub sites.
The remaining incompleteness is explicit; enumerating their calls does not make
those owners functional. LocalMultiplayer scene references DamageController and
StartCounter_LM. Normal Earth defense currently uses ShieldController; this audit
does not establish all uses of the separate IShield prefab.

## Additional corrected owner details

LeafShieldController.ShieldHit still plays the explosion particle effect after
playing Hit on every serialized leaf animator. Its field names, .meta, scale
owners and state bindings remain unchanged. Knowledge/animation-callback-
reconstruction.md was corrected so retrieval no longer describes Hit as a trigger.

DesktopInputs.Update native0x1288EB8..0x1288F08 calls Input.GetKeyUp(106/J), then
GameController.Special; Input.GetKeyUp(107/K), then Animator.Play(Taunt2). The
literal is resolved by slot0x28169D8. These were missing from the reconstructed
Update, rather than incorrectly expressed as SetTrigger. They now run before
axis DetectInputs, preserving the existing offline ownership gate. Network and
spectator paths remain outside this audit's runtime claims. This does not add
new keys based on convenience; both shortcuts are native-derived.

## Validation and pending runtime scope

Executed read-only/static checks: six API proof contracts matched corrected ARM64
and named ISIL;16 direct Play sites enumerated and reviewed; the annotator reports
Animator.Play(string) at0x1240FA0 and Animator.SetTrigger(string) at0x127EFF4.
No Unity/build/probe was launched. The root's already-running twelve-special
probe predates these edits and cannot validate them.

Pending: run a fresh build; exercise Maya leaf shield hit and its Hit state;
exercise keyboard key-up K/Taunt2 and J/Special through the actual input owner;
verify corrected reserved-owner continuations (prison crush, whips, settings)
with runtime state checks. Existing successful basic attacks/settings panel
visibility do not establish these explicit Play animation continuations.
