# Earth prison reconstruction

REA skill applied to the retained native ARM64 library, SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`. Correct VA/raw mapping: `Evidence/cpp2il-native-address-map.json`. Source-derived contracts and observed prefab fields, component identities, method mappings and prefab hash are in `Evidence/earth-prison-native-contract.json`. This worker did not launch Unity/build/player; the parent owns subsequent runtime acceptance.

Restored scripts: PrisonController, SumonInOpponent and directly required MoveObject. Script GUIDs and existing serialized fields remain unchanged. SumonInOpponent's event parameter follows the reconstruction's existing EntityId contract; the original shipped Unity used int instance IDs. A scan of working prefabs/scenes/C# found no serialized UnityEvent or other caller for SumonInOpPos beyond its event owner.

## Positioning event owner

SumonInOpponent.Awake `0x12C6978` subscribes and SumonInOpPos `0x12C6A24` handles onAttackLayerSetUp; corrected body evidence is `Evidence/native-disassembly-SumonInOpponent.txt`. It ignores IDs belonging to another GameObject. Online non-owned actors are ignored. For a matching owned/offline root, layer9 moves to GuestPlayer.position, other layers to HostPlayer.position; rotation remains unchanged. It dispatches SummonedInOpp for the same identity and unregisters the setup listener. OnDestroy `0x12C6C04` also unregisters.

The implementation retains the exact GameEvents owner used for subscription; teardown does not reinterpret a later singleton as the original owner. The shared component also affects SumonThunderAtk2 and BranchPrisonATK; accepted Maya prison coverage predates this restored positioning producer and should be rechecked if a fixture relied on the earlier stub.

## Prison lifecycle and event contracts

PrisonController.Awake `0x1277810`: if either main/opponent Animator current layer0 state is PrisonFree, use DestroyOnCollision.DeleteObj and return. Otherwise offline or locally owned online prison subscribes to three events, schedules StartMoves after1s and FinishMove after delayRevert. Native constructor delayRevert=1; EarthPrison serializes1.5.

- OnPlayerPrisonHit => DestroyPrisonHit, **unconditionally DestroyAllPrison**. Native `0x12780A8` is a four-byte tail branch `B 0x1277F0C`, not RET.
- OnPlayerPrisonTrap => PrisonTrap `0x1278488`, sets trap=true and retains the received player layer; no ownership/layer filter exists in this native receiver.
- OnPlayerDamaged => DestroyPrison `0x1277EB8`, destroys only if received layer differs from prison layer AND trap=true. The trap's initial hit reaches damage before trap is set; later damage can release it.
- DestroyAllPrison invokes DestroyOnCollision.DeleteObj, including configured explosion and delay. Online RPC transport remains unavailable; no server behavior is claimed.
- OnDestroy cancels outstanding invokes and unregisters all three exact callbacks from the retained event owner. No callbacks are added if the initial PrisonFree guard rejects creation.

StartMoves `0x1278498` destroys the root immediately if ConfigAttackLayer.playerWeak. Otherwise it enables MoveObject component[0] on every wall. FinishMove `0x12780AC` invokes RevertWalls only when trap=false, and always disables the root BoxCollider. RevertWalls `0x1278118` logs Revert Walls, disables collider, disables wall movement[0], enables wall movement[1], and schedules root destruction after2.5s. Native local RPC recipients are implemented without claiming networking: StartMovesOnline enables movement[0]; RevertWallsOnline reverses and destroys after1s; DestroyPrisonOnline uses DeleteObj; OnlineDestroy destroys root.

No direct victim Animator mutation belongs to this component. Actual collision with Description.effect=prison reaches DamageOnCollision's native-derived effect route: contact distance>1.2 or prior unTrap>0 dispatches PlayerPrisonHit and hitBackFlip; centered first contact dispatches PrisonFree and PlayerPrisonTrap. This creates materially different caught/miss/escape paths to test.

## Wall movement owner

Corrected `Evidence/native-disassembly-MoveObject.txt`: Start `0x129BFC0`, FixedUpdate `0x129C010`, StartMoving `0x129C0CC`, Finish `0x129C2D8`, constructor `0x129C2E4`.

Constructor defaults: speed=.5, moveFinished=true, finishMove=.55. Start invokes StartMoving after delay. StartMoving clears moveFinished, captures current world position, adds AddVector only when squared magnitude>=1e-10, then adds transform.forward*moveForward, right*moveRight, up*moveUp. It invokes Finish after finishMove. FixedUpdate lerps current world position toward captured endpoint using clamped speed every physics tick; no deltaTime factor exists. Finish sets moveFinished=true. The initial true flag prevents movement toward default worldzero before StartMoving. No OnEnable reset or invented coroutine was introduced: the disabled reverse component runs its first Start when first enabled.

## Actual prefab route and measurable fixture

Diana/Ren cardinal DOWN with attack variant integer1 enters DownAttack2 and its CreateAttack callback instantiates Resources/photonprefabs/EarthPrison. Use actual input/attack buttons after readiness, not Animator.Play. Observe creation/setup event before Start to record positioning, then observe actual physics and animation outcomes.

EarthPrison prefab SHA256 `073c2e69824fc228310a32b07343fd473db058bf3a0ba6e1b9d1c90b9616de57`:

- Root starts collider disabled, size7x10x7; EnableCollider invokes enabling after1s unless ConfigAttackLayer.playerWeak.
- Root Description is authored element Fire, Damage1, Defense20, effect prison. Do not relabel element based on the character's name.
- Four wall objects start at local(+/-4,0,0) and(0,0,+/-4), each with two MoveObject components. Closure index0 enabled at serialization, delay1/speed.3, inward displacement2 and upward2.5. Most finishMove5, one wall .5.
- Reverse index1 starts disabled, first Start delay.8/speed.1, outward displacement2 and downward3. Most finishMove5, one wall .5. After enabling reverse, sample at least1.0scaled seconds later to prove movement. Merely asserting enabled flags is insufficient.
- FinishMove at1.5s after Awake disables the trigger. If untrapped, reverse starts then, expected visual movement from~2.3s and deletion~4s. If caught, do not reverse at1.5s; independent DestroyByTime expires root at5.75s and can create configured explosion.

Recommended fixtures: (1) centered stationary opponent: position matches original opponent pose after setup, closure displacement, real PrisonFree and trap event, no reversal at1.5, eventual expiry; (2) move opponent away before collider opens: no trap, reverse begins and root is gone~4s; (3) trap then actual separate damage: root deletes/explodes and listeners clean up; (4) owner weak before1s: root destroyed without starting walls. Distinguish synthetic repositioning/weak setup from naturally caused game events. At teardown fire no hidden damage into a new actor just to test cleanup; observe retained listener count or a subsequent independent prison if available.

Known coverage limits: online transport, natural CPU evasion/timing, audio fidelity and every unusual overlap are not established by this source reconstruction. Other EarthPrison presentation helpers may remain partial; no broader unrelated component restoration was performed in this unit.

## Executed Windows acceptance

Evidence/offline-earthprison-probe-20261015-190624.json passed both fresh-scene
Diana cases with errors=[]. Actual PointerEnter/Exit down inputs naturally
alternated0->1; no animator/event/destruction was forced. A centered stationary
CPU was trapped via real physics and entered PrisonFree. Collider opened at
age1.023066 and closed1.514721; each wall moved3.201units locally, no reverse
enabled and root expired at5.770964scaledseconds.

The miss fixture synthetically moved CPU sideways10 immediately after the real
SummonedInOpp event. No trap/PrisonFree occurred. Collider ages1.005589/1.518959,
all four closure displacements3.201, reverse displacements3.605/3.605/3.347/3.605,
and natural deletion at4.013809s. This proves actual wall travel, not merely
enabled flags. Save/preferences were restored; preparation and each prison
observation had separate bounded8s deadlines. Ren, later-damage release, weak
cancellation, natural evasion and graphics parity remain outside this report.

Gameplay assembly317849cd908478f430f65e7a2c80e63b9025d1b4451812844a213641d6880acb;
latest build00:05:44Z,0errors/22warnings. See directional-runtime-validation.md
for the distinct earlier120-route roster build and its limitations.
