# Special charge, awakening and stats audit

Current scope: SpecialAtk and StatsController were already reconstructed. This
pass compared their offline paths with correctly mapped ARM64 and found no
remaining stub or demonstrated producing bug requiring a source edit. No game
source/.meta was changed. This is a static audit, not new runtime acceptance.
Native source SHA-256:
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.

## Evidence and retained tools

Corrected sources: Evidence/native-disassembly-SpecialAtk.txt and
Evidence/native-disassembly-StatsController.txt; nested coroutine dumps with
prefix SpecialAtk__DechargeSpecial/DechargeSpecialLM and StatsController__Hit,
__GrassHealing, __RestoreHeal2, __ChargeShieldBar. Named ISIL calls are hints
only; Cpp2IL's printed Disassembly is not used. Native VA/raw mapping remains
Evidence/cpp2il-native-address-map.json. Existing
Evidence/special-atk-native-constants.json records resolved literals.

Tools/audit-special-stats.cjs uses the format-aware Unity YAML parser and produces
Evidence/special-stats-static-audit.json with scene/controller/clip hashes,
SpecialAtk fields, special button callbacks and ancestors, controller transitions,
and Awakening/Special_0 clip events. Rebuild this audit if these inputs change.

## Entry path and entitlement boundary

Battle scene GameController.SpecialButton points to GameObject279 named S.
Its m_IsActive=1, ancestor Canvas142 is also active. Besides RectTransform it
has EventTrigger1290 and RemoveAsSpectator1556(delay0). Actual callback is
EventTrigger PointerClick(eventID4) -> GameController.Special, not Button.onClick;
PointerEnter/Exit update InputReader.SetSButton(true/false).

The offline spawning path calls CompleteMainChar then starts the counter.
No invocation of EnableSpecial was found in the bounded working source and
serialized UnityEvent/animation searches. EnableSpecial's name is not evidence
that startup requires calling it. Its implemented offline HasKey alias lookup
is Rina for Amon/Rina; Dante for Dante/Agata; Eliot for Eliot/Vinea; Aura for
Sora/Aura; Ren for Diana/Ren. Raven/Maya use its default return, which does not
disable their already-active button. No missing ownership/entitlement should be
invented to force visibility. Record actual runtime activeSelf/activeInHierarchy
and callback receiver before the special probe.

GameController.Special native0x1293668 requires active button, charged,
MainCharacter, not decharging, and Animator Stunned=false, then triggers Special
and sets Stunned=true. All seven shipped element AnimatorControllers default
NumSpecials=0. Their transitions require Special trigger plus NumSpecials=0 for
Awakening, NumSpecials=1 for the actual Special state. Thus a first successful
button click is awakening; it is not proof of the element's special attack.
Raven's prefab references ShadowAnimatorController; Maya references
GrassAnimatorController. Their shared awakening entry follows the same transition
contract; their actual second special needs separate validation.

## Charge and discharge semantics checked

SpecialAtk.ChargeSpecial native0x12B0F9C checks readiness BEFORE adding this
call's increment. While fill<.9 and charged=false, it adds val/numberOfSpecial.
A subsequent invocation at fill>=.9 marks charged, fills to1, colors letter and
circle black, plays its AudioSource once. Calls during decharging return. Scene
numberOfSpecial=1; no current source assigns another count.

A bounded probe may use ChargeSpecial(1) twice as an explicit charge fixture,
then invoke the actual PointerClick callback; one call alone does not set charged.
This validates charge/UI integration, not naturally earned attack charging.
Natural sources remain ChargeSpecial.OnStateEnter(.03) and main-character
DamageOnCollision.GetDamage(.05), which need their own runtime observations.
ResetCharge native0x12B10D0 clears fill/charged/sound, uses serialized sBlack,
sets NumSpecials=0 and text X. It does not clear decharging; do not add that
behavior without evidence.

DechargeSpecial coroutine0x12B13A4 sets decharging/fill1, subtracts .034/time
per .04s wait, then calls AuraController.StopAura and clears readiness flags.
Unless Survival AND Character=Maya, it sets text S and NumSpecials=1. This Maya
Survival exception is native, not an inferred restriction. The decrement formula
does not produce exactly the supplied number of elapsed seconds; preserve it.
AnimatorStatesManager.AwakeningStart native0x127EB84 confirms the existing20f
argument, starts aura, shakes camera(4,3,0,2) and starts main-player discharge.
Awakening clip calls this at .8333333s. The second special requires waiting until
decharging=false, then earning/refilling charge again.

DechargeSpecialLM native0x12B161C only reduces fill; its absent aura/readiness
cleanup is native. LM is outside this offline single-player audit.

## Stats and Maya GrassHealing checked

StatsController.Start0x12C3E0C establishes singleton, names and shieldbarMax5.
SetPlayersName0x12C3F40 offline unauthenticated branch uses Player/CPU; authenticated
profile/network identities are not claimed reconstructed by this audit.
SetHealth0x12C4418 uses destroyed-crystal damage thresholds1/5/10, marking tag
Destroyed and color(0,0,0,.4). Hit0x12C5834 (MoveNext) flashes red and lerps color
back to white or the destroyed color; GetPlayerLifeCrystals0x12C509C counts images
whose tag is not Destroyed. RestoreHealth0x12C4AF4/RestoreHeal2 0x12C5AD8 start
with dark images and restore white/Untagged after .75s. Addresses of nested
methods are retained in the corresponding dumps if entry lengths differ.

GrassHealing MoveNext0x12C5448 matches existing implementation: choose main/opponent
life images, wait1s, play heal FX, repeat five sequential passes over the selected
images; each image becomes green(0,.7,0,1), loses Destroyed tag, lerps toward white
by.06 per .02s wait while t advances2*deltaTime through .75, then becomes white.
The source array is GameObject.GetComponentsInChildren<Image>, confirmed by generic
usage0x2817BE0; GetComponent<Image> confirmed0x2815E50 and literal Untagged
0x2817C10. This is not a missing health-counter decrement: GrassSpecialController
already calls DamageOnCollision.RestoreDamage immediately, separately from the
stats animation coroutine. Maya Special_0 clip calls StartSpecialAtk at.05s,
CreateAttack at.11666667s, ResetSpecialAtk at.85s. Wings growDelay0/shirnkDelay6.

ChargeShieldBar MoveNext0x12C5248 matches existing per-tick +.0105 on X, .02s wait,
red(.6,0,0,.7) then green(.3,.5,.3,.8). SetShieldBar0x12C500C clamps damage at5 and
sets scale(5-min(5,damage),2,1). No new numerical or timing change was introduced.

## Runtime work remaining

- Observe default active button for every fighter; invoke actual PointerClick.
- Separate first awakening and second elemental Special, including normal
  charge sources, NumSpecials, Stunned recovery and decharging gate.
- For Maya, retain actual damaged starting state, observe damage reset and
  selected crystals' tags/colors plus healing particles and wing scales over
  time; distinguish seeded presentation fixtures from real combat damage.
- Check Maya Survival exception without rewriting it to match other modes.
- Verify special projectile lifecycle/collision effects, and actual attack-caused
  wins; successful charge/UI transitions alone do not establish those behaviors.

No Unity/build/probe was launched. No speculative source correction was made.
