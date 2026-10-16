# Directional offline combat validation

The full game remains unfinished. The new Moves scenario attempts ten routes
(neutral/up/down/right/left, each ordinary and second variant) per selected
fighter through actual pointer input. Optional -Fighter selects one roster
member; omit for all twelve. Root evidence and build identities delimit actual
executed coverage, not merely the intended scenario.

## Input ownership and first observer correction

Evidence/offline-moves-probe-20261015-184231.json failed after Sora BasicAttack:
the probe assumed an active ButtonAttack for each attackType0/1. In the shipped
Bending Battle scene, X GameObject225 is inactive; its ButtonAttack1671 exists
but is not the visible input route. This was a probe failure, not evidence of
a broken product attack.

The active AHitZone GameObject303 has EventTrigger1584 with PointerEnter:
InputReader.SetAButton(true), GameController.Attacking(true), and PointerExit:
the same false callbacks. Unlike ButtonAttack.OnButtonPress, this route does
not call AttackingButton or overwrite the animator's variant integers. Native
ChangeAtk callbacks naturally alternate those integers. Directly invoking the
hidden X or forcing animator integers would conceal this distinction.

The corrected scenario requires one active SetAButton PointerEnter binding,
dispatches real PointerEnter/Exit events, checks the existing variant integer
before each action, and observes the expected state by shortNameHash. It never
forces Animator.Play, Stunned, variant values or projectile lifetime. Direction
axes use InputReader dPad mode. Fighter transforms and rigidbody velocities are
reset between moves as an explicitly synthetic spatial fixture; CPU behavior
is disabled after countdown. Save/preferences are restored on completion/error.

Entry waits are bounded, as are recovery waits. Movement of enabled MoveRock,
MoveLight and MoveWater components is sampled separately from spawning. The
probe records both synchronous creation events and frame-polled Description
objects: SumonByTime has no native creation event, and a short-lived collider
may vanish before polling. Neither observation mechanism alone establishes
complete child production. Two seconds after state exit allow delayed effects.

No reported runtime errors and valid state entry/exit do not prove damage,
aiming, collision variants, visual fidelity, awakening routes or all remaining
stub dependencies. Motion observations are evidence only for the identified
components/objects and sampled interval. Coverage and remaining failures follow
in subsequent retained reports.

## Controller-specific state expectation correction

The corrected pointer probe184523 passed all ten Sora routes (errors=[]).
Its final limitation string was accidentally replaced by the generic Specials
string at CombatTrip exit; it did not execute Specials. The retained observations
delimit the actual ten-route scope. Subsequent code preserves the Moves scope.

Full-roster attempt184752 then completed Sora and Vinea's two basic variants,
but failed its UpAttack expectation: the actual clip was UpAttack2. Vinea/Eliot
controllers intentionally map upward variant0 to UpAttack2, variant1 to UpAttack.
This is another observer assumption failure, not proof of product route failure.

Tools/build-move-input-contracts.cjs now derives all120 expected routes from the
existing format-aware controller audit, preserving ordered entry transitions,
conditions/default-state identity, audit SHA256 and controller hashes. It fails
if a controller's current bytes differ from retained evidence or a condition
is unknown. Build-OfflinePlayer copies the generated development-only TextAsset
into probe Resources. The probe consumes that parsed contract; it no longer
guesses state names by suffix or special-cases fighter names. It also samples
the signed authored StartRotation result as a quaternion, relative to the
creation-time pose, after its scheduled delay. Runtime errors and failed
expectations remain distinct from completed route acceptance.

## Executed complete bounded roster run

Evidence/offline-moves-probe-20261015-185330.json passed all120 routes in one
run, errors=[]. Every shipped fighter completed ten actual pointer-input
routes, using its retained controller-specific state expectation and natural
variant alternation. The last observation was at603.95real seconds. Gameplay
assembly8e23df4e037e2daafa8b079e934870f9ee9b1694a913d0b9f4f6315760a22bf8;
build23:52:54Z,0errors/24warnings,76.5934s,543395341bytes.

Evidence/directional-moves-acceptance.json retains the report SHA256, launch
identity, per-fighter counts,42 sampled motion observations and12 yaw-bearing
projectile instances. All twelve authored yaw checks met their.5degree
tolerance (the source report records errors0). Some motion entries are zero:
WaterAtk's actual MoveWater serializes speed0 and no source change invented a
nonzero value. Other enabled MoveWater objects, including AirAtkTwister, moved.

This build includes the Earth prison, DragonWood, Shadow and fire collider
source restorations. State/callback success alone does not accept the complete
effect, timing or collision result of those units; dedicated fixtures follow.
It predates the JoystickSettings/Joystick/VariableJoystick restoration and the
InputReader/MobileInputs boundary corrections; do not silently attribute this
all-roster run to the subsequent input build. CPU was disabled and transforms
reset between moves. No natural aiming, attack-caused match victory, all
awakened variants, exact graphics/audio or complete game claim is made.
