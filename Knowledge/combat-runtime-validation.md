# Combat continuation - 2026-10-15

The complete offline game remains unfinished. This checkpoint extends the
earlier Windows acceptance work with effect dispatch, all-roster defense and
special/branch-prison tests. The original Android runtime has not been executed.

## Collision producer and related consumers

Native Description.effect is field+0x40; ReleaseBy is+0x38. OnTriggerEnter now
binds effect once for reaction dispatch and damage-event exceptions. Three
related consumers were audited and corrected from the same native field:
DestroyIfSameObjIsNear, SummonAttack.CreateKickAttack, DestroyOnCollision.
The last also required its actual literal ShadowTramp rather than branchPrison.
See Evidence/description-field-consumer-audit.json and defense-reconstruction.md.
ReleaseBy remains a preserved metadata/serialized field; it was not renamed.

Historical overlap reports showing+.5 before this correction establish collider
entry, not correct reaction routing. The corrected frontal blocking tests below
exercise the real dispatch after the producing-boundary fix. Other source
corrections still require their own runtime coverage.

## Executed defense tests

Evidence/offline-defenses-probe-20261015-175750.json passed for Diana.
Evidence/offline-defenses-probe-20261015-175848.json then passed all12 fighters,
24 observed hits, errors=[]. Actual B EventTrigger PointerEnter/Exit changes
Jump; stationary movement enters the real Evade/Block controller path.
Recovered AirAtk overlaps are synthetic: effect=Front, strengths1 and3,
opposing layer. Physics and product callbacks run normally, without directly
calling HitAnimation or seeding Blocking.

Each low hit added.5 damage while preserving Blocking; each heavy hit added1.5
and cleared Blocking, matching native dispatch. Temporary ownership, save bytes
and captured preferences were restored. These tests do not establish aiming,
side/back/awakened defense branches or natural shield-meter depletion.
Native DamageShield has no direct callers in the bounded4146-method app scan;
do not invent automatic depletion from that negative search.

The tested gameplay assembly was
2271798de1dd8a8e3cbc502c6bb536fede936ee9c996a8a5c0ad78f6dcc92433,
build22:57:24Z,0errors/19warnings. This predates the later related-consumer and
Animator.Play cleanup. Fresh manifests delimit subsequent results.

## Special tests

Evidence/offline-specials-probe-20261015-175252.json passed Diana's actual
Special PointerClick route: accelerated charge through the native charge owner,
Awakening with discharge active, natural discharge completion to NumSpecials1,
recharge and phase1 Special. Six player attack entities were observed, no errors.
This first run predates the collision-effect correction and is not defense proof.

A full12-fighter Specials run, Evidence/offline-specials-probe-20261015-180546.json,
completed both phases for Sora,Vinea,Diana,Amon,Aura,Eliot,Ren,Rina,Dante,Agata.
Diana and Ren each produced five observed rocks, all moving beyond.5units
(maximum travel341.661/343.6592). It then failed during Raven's phase1 Special:
Particle System Shape Module could not read mesh Torus because Read/Write was
disabled. Raven Awakening/discharge had completed; its special did not complete,
and Maya was not reached. This failed overall run is not all-roster acceptance.
That failure is retained. Subsequent bounded reruns below supersede its pending cases.

Raven rerun Evidence/offline-specials-probe-20261015-182511.json subsequently
passed, errors=[], with Awakening/natural discharge and phase1 Special producing
five attack entities. Torus readability repair was the single mesh change;
vertices/materials/GUID were preserved. See particle-mesh-readability-reconstruction.md.
The other11 inventoried skinned-mesh shape sources were not modified.
Maya then passed Evidence/offline-specials-probe-20261015-182812.json: damage
seeded to5.5 returned to0, healing particles appeared, maximum wings magnitude
was1.732051 and no destroyed life crystals remained. Evidence/specials-roster-acceptance.json
joins twelve cases with per-source hashes/build identities. This is coverage
across multiple runs, not one clean twelve-fighter run.
CPU behavior is disabled; the charge fixture does not establish natural charge
generation or aiming. Save/preferences restoration runs on completion/failure.

## Branch prison and animation-call correction

Thirteen additional owners were restored from corrected ARM64 in the roster and
branch-prison units; see roster-moveset-reconstruction.md and
branch-prison-reconstruction.md for exact coverage, callbacks and constraints.

Evidence/offline-prison-probe-20261015-180344.json reached real Maya DownAttack0,
spawned BranchPrisonATK, observed its disabled collider enable, synthetically
positioned CPU in the active volume, and received real trap callbacks.
enemyTrapped/ref-to-CPU/disable-collider succeeded. Crush failed: no
PrisonExplosion state or crush object; prison eventually expired and owner cleared.
This is a retained failed test, not completed trap acceptance.

Root cause was a wrong translation of native engine call0x2364318:
Animator.Play(string), not SetTrigger. BranchPrisonCharController now plays
PrisonExplosion directly; no such trigger parameter exists in the Maya
controller. SettingsOptions and WhipController had the same error and were
corrected. The bounded engine-call audit examined16 direct Play(string) sites;
LeafShieldController and DesktopInputs also required corrections, while five
sites remain in explicitly unreconstructed LM components. Six exact API
contracts are centralized with proof in animator-api-contract-audit.md.
ForceShieldOff was also restored from native (six elements, no Leaf dispatch).
Weak gates
and physical trap callbacks were preserved, without forcing animator state in
the probe. The subsequent build and acceptance are recorded below.

The earlier menu probe only accepted the close-background toggle; it did not
prove panel animation. The source probe now also checks SettingsSlideIn/Out
states and captures open/closed images. Report182904 passed these checks but its
close capture caught a partly displaced panel. A follow-up waits for the exact
authored clip endpoints before capture; do not infer full UI layout fidelity.

The first fresh prison repetition (182058) did play PrisonExplosion, but the
frame-poll observer did not see the short-lived crush object. It retained a
probe_failed result; spawning is not yet accepted. BranchCrushAtk has an actual
DestroyOnCollision component and the overlapped Amon's Description.Damage18
exceeds its Defense2, so a first physics step can remove it before a frame poll.
The probe now observes GameEvents.onAttackLayerSetUp synchronously, matching the
GameObject EntityId and verifying the cage target before destruction. Product
attack lifetime/collision behavior was preserved. CreateByTime, a directly attached
FX producer, was also restored from native InvokeRepeating(delay,time), preserving
the observed0/0 values rather than inventing an interval.

## Successful post-correction suite

Evidence/offline-combat-suite-20261015-182744.json completed all five cases with
zero reported runtime errors. All share gameplay assembly SHA256
07bb4dcdf94e5eb6e70ad9bccb37a9e913aa9860eebf2615eae5361108bc8b9d.

- Prison182744 observed actual PrisonExplosion, synchronous BranchCrushAtk
  creation on layer9 with the correct cage target, cage destruction and cleared
  attacker ownership. CPU placement was synthetic; miss/weak/interruption cases
  remain untested. It does not independently establish visual FX parity.
- Maya Specials182812 passed healing/wings/two-phase checks described above.
- Menu182904 passed economy/persistence and animation-state/background checks;
  settled position acceptance is a separate follow-up.
- ImpactAndRounds182930 observed recovered AirAtk overlap delta1 and two damage
  events after effect routing was corrected, then two respawns,3-0/rankS/reward6
  and idempotent reward. Impact placement and score-producing falls were synthetic.
  A short natural observation counted player1/CPU2 attacks and four hit events;
  this is not natural match-win or aiming acceptance.
- Maya Defenses183010 repeated frontal low/heavy checks on the fresh native
  Animator/ForceShieldOff build. All-roster coverage above belongs to its earlier
  identified assembly and must not be silently attributed to this build.

The latest subsequent build (23:32:45Z) changed only the menu validation observer,
not the reconstructed gameplay. It succeeded with0errors/19warnings in17.4919s,
543329388bytes. Warnings include missing legacy particle shader fallbacks; zero
reported runtime errors does not mean warning-free or complete graphics parity.

## Settled settings follow-up

Evidence/offline-menu-probe-20261015-183440.json passed with errors=[], gameplay
assembly5bde451c86106882673d0cb1710d2611f47ceeeab15a7050246b3ed9e00170fc.
The real settings RectTransform reached open x=-160.4 and closed x=-722.7173
(authored closed key=-723; tolerance1), using the original UI callbacks and no
forced transform. Both screenshots were inspected: the settings text panel is
absent after closing. Open text labels MUT/INV remain visibly clipped and the
character appears overexposed; those visual concerns remain pending comparison
against original video evidence. This acceptance establishes the animation
endpoints, not complete menu/graphics fidelity. Exact save bytes were restored.

## Ability aiming checkpoint (2026-10-16)

See ability-aiming-reconstruction.md, ability-aiming-map.md and Evidence/ability-aiming-acceptance.json. Final AbilityAim002256 passed all120 normal routes/96root bearings/120live orientation checks and actual Follow target assertions; final Specials003331 passed12awakening/decharge/super cycles. Same final052221 build/25d345 assembly, errors empty and fixtures restored. Particle black-quad repair is approximate, with final qualitative captures. This is not all awakened normal attacks, natural moving-target hit outcomes or full original visual fidelity.
