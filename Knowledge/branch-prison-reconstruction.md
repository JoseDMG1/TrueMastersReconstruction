# Maya branch prison: complete producing chain checkpoint

Implemented together: branchPrisonController, BranchPrisonAnim (two actual C# coroutines), PathController, BonePath, RefHitCharacter, and the additional required EnableCollider producer. Existing BranchPrisonCharController was restored immediately before this unit. All .meta GUIDs retained. No Unity/build/player run by this worker. Parent must compile and observe actual physics/animation integration before claiming working trap gameplay.

## Sources

Corrected mapped ARM64 for native library SHA d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c:

- native-disassembly-branchPrisonController.txt0x121ABAC..0x121B2AC.
- native-disassembly-BranchPrisonAnim.txt and native-disassembly-BranchPrisonAnim__ReduceSpeed_d__15.txt / __RetrieveAttack_d__16.txt0x121A4A8..0x121A990.
- native-disassembly-PathController.txt0x1247354..0x1247B68.
- native-disassembly-BonePath.txt0x124697C..0x1246E88.
- native-disassembly-RefHitCharacter.txt0x12795B4..0x12796DC.
- native-disassembly-EnableCollider.txt0x128AF64..0x128B044.
- Evidence/branch-prison-native-literals.json / branch-prison-native-generics.json retain selected exact string/type resolutions.
- Evidence/branch-prison-prefab-components.jsonl retains parsed fields/callback ordering; producer Tools/inspect-branch-prison-prefab.mjs.
- Evidence/roster-moveset-audit.json retains original state/clip event timings, parameters and source hashes.

## Native behavior translated

Maya DownAttack variant0 creates BranchPrisonATK using SummonAttack.CreateAttack at clip1.15, Animator state speed1.2; approximate launch.958scaledseconds after state entry. Its earlier CreateNetworkAttack.8333333 is offline no-op. DownAttack also plays handFX; UpAttack/UpAttack2 are different attacks and should not be used for this trap fixture.

BranchPrisonATK.prefab GUID e1d7790768ed35b4fa63f39a84e48830 is the root collider/event owner. Description.Type Attack, element Fire, Damage1, Defense20, ReleaseBy Player, effect branchPrison. The element value is an observed asset fact despite Maya being Leaf; it was not renamed. The BoxCollider is trigger size1.5x10x1.5 centerzero and initially DISABLED. EnableCollider invokes enableCollider after.9s and enables it only when ConfigAttackLayer.playerWeak isfalse. DisableCollider invokes Disable at1.2s and calls branchPrisonController.PlayHideAnimation. Consequently the original hit window is.9..1.2scaledseconds after creation; merely spawning this prefab without its EnableCollider implementation could never yield a collision.

RefHitCharacter.OnTriggerEnter accepts a collider carrying Description.Type Char, records its GameObject and invokes onHitCharacter with its layer. Serialized persistent callbacks are ordered: TrapEnemy -> DisableCollider.Disable -> AddListernerOnEnemyDamage. No callback is fired manually by the reconstruction. TrapEnemy marks enemyTrapped, then checks owner Animator.Weak; iffalse, owner BranchPrisonCharController.PlayCharCrushAnim explicitly calls Animator.Play("PrisonExplosion"). Native does not clear enemyTrapped when the owner is Weak.

Prison owner Start schedules DestroyInTime at serialized5.4s, finds attacking character by rootlayer through GameController.GetAttackOwner and assigns its actualPrisonAtk. PlayCrushAnimation starts the branch animation RetrieveAttack on its owning component and plays crushSound. PlayHideAnimation returns if trapped; otherwise disables both PathControllers and starts RetrieveAttack. AddListernerOnEnemyDamage attaches DestroyOnEnemyHit to the opposite character's DamageOnCollision.onDamageRecived. That callback compares the enemy's NEXT Animator state with PrisonFree; if it isnot PrisonFree it instantiates BranchExplotion at prison pose and destroys the prison. The timeout instantiates the same effect only when a character was recorded, then destroys the root.

Maya PrisonExplosion state speed2. Clip StartBranchCrushAnim2.9833333 occurs approximately1.492scaledseconds after entry; CrushAttack5.983333 occurs approximately2.992scaledseconds after entry. The latter creates the character's serialized crushAttack GUID da6878eacf2233942b07c0fd98a1f1db at prison pose, assigns character layer, points DestroyWhenCreated.target to the prison root and emits GameEvents.AttackLayerSetUp with Unity EntityId. Runtime state speed/transitions/timeScale can change observed wall timing; these source ratios are fixture guidance, not measurements.

BranchPrisonAnim records SimpleMove.speed, RotateSelf.x and both branch localPosition.x values. ReduceSpeed uses curve.Evaluate(elapsedtime/time) for movement/rotation and branch x while retaining y/z. RetrieveAttack resets elapsedtime, negates original movement/rotation, then updates only movement/rotation using the SAME curve offset0x58. The serialized inverseCurve is empty and is not read by the native body. The onAttackExplote UnityEvent is declared but not invoked in either examined iterator; no new invocation was invented. Both loops advance Time.deltaTime and yieldnull, sharing elapsedtime exactly as the original fields do.

PathController initializes a100-position history (serializedlimitPositions100), startPoint=worldposition+(0,-20,0), lastPosition=startPoint and stepdistance20/count. Initial entries are native literal coordinates(0,distance-progress,0), not translated relative startPoint. LateUpdate returns below limitDistance.1, computes positive truncated movement/limitDistance, appends equally spaced points from lastPosition and removes oldest one each time; optional debug marker objects follow the same list ownership. It updates lastPosition and calls BonePath.UpdateBonesRot. Stop disables only the PathController component.

BonePath appends its own Transform and then GetComponentsInChildren<Transform>, including the repeated root. It places its root at path[0], then rotates each child toward history indexed by banker's rounding of (distance*100)/(limitDistance*100); distance comes from children1/2 spacing multiplied by current index. It uses Quaternion.LookRotation(direction,Vector3.up).

## Explicit reconstruction corrections and lifecycle ownership

BonePath's native index guard uses index>Count, allowing index==Count into an invalid list read. The reconstruction clamps the index at the producing boundary to0..Count-1. This is a deliberate correction, not a claim that the shipped game was observed crashing. The actual prefab supplies100 history points; empty/malformed path configuration remains unsupported.

branchPrisonController retains the exact DamageOnCollision listener owner and removes its callback on destruction; it clears the attacker's actualPrisonAtk only if still referencing this component. This cleanup was absent in the examined native body and prevents stale event listeners and stale ownership after expiry. No general search for replacement owners is added.

WhipController and IcePrison cleanup recovered in the adjacent roster checkpoint are separate contracts. Native BranchPrison.cs had no references in the bounded working prefab search and was not confused with branchPrisonController; it remains outside this concrete prefab chain.

## Concrete real-collider probe fixture for parent

1. Select Maya as host through existing reconstruction selection/boot fixture; ordinary offline arena; disable CPU behavior for deterministic observation and wait countdown/gameStarted, both character Animator states Movement, no Weak/Stunned/Freeze. Snapshot player save/prefs using the existing restoration mechanism. Do not manually invoke TrapEnemy or PrisonExplosion callbacks.
2. Use actual movement input y=-1,x=0 and real A path with DownAttack integer0, then release. Assert actual Maya Animator state DownAttack; wait a newly instantiated branchPrisonController/BranchPrisonATK root from the clip event. Find it via owner BranchPrisonCharController.actualPrisonAtk after Start. Assert attacklayer==hostlayer, config.playerWeakfalse, root BoxCollider disabled initially, no enemy trapped.
3. Observe the collider become enabled around.9scaledseconds after spawn. Its automatic disable is1.2s; the probe must be polling everyframe/physicsstep, not waiting an arbitrary second. After enable, place the opposing character inside the box bounds.center (or target position so its character collider intersects), clear its rigidbody linearVelocity and synchronize transforms, then allow actual Physics FixedUpdate(s). This is a synthetic positioning fixture with a REAL collider/trigger and serialized callbacks; do not describe it as natural aiming/combat. Layer8/9 collision matrix must remain the actual arena configuration.
4. Assert RefHitCharacter.hitCharacter is the opponent, enemyTrappedtrue, root Collider disabled immediately by serialized callback, and owner Animator enters PrisonExplosion through its native Play call. Observe the victim's PrisonFree reaction from DamageOnCollision.effect branchPrison separately; Unity trigger dispatch ordering between objects is not guaranteed by static YAML.
5. Assert branchAnim has real motion/history updates without coroutine-null/receiver errors, and at the PrisonExplosion crush event the new crush attack uses hostlayer and destroys the referenced prison via DestroyWhenCreated. Capture event/order and cleanup; do not require the prison still exist after crush. The source window fits before timeout5.4s when clip speed2 applies.
6. Separate miss fixture: create the same attack through real DownAttack, keep opponent outside collider, observe enable.9/disable1.2, PathControllers stop and retrieve coroutine runs, then timeout destroys root without hit-target explosion.
7. Separate interruption fixture: after actual trap, cause a real nonexception projectile impact against victim and inspect DestroyOnEnemyHit + NEXT-state PrisonFree check, retained listener cleanup, and no call into destroyed prison on subsequent damage. Weak attacker fixture should set actual game state via a real hit if practical; state seeding must be labeled synthetic.

The full12-character general probe cannot substitute for these three trap cases. No result is asserted yet. ConfigAttackLayer.playerWeak production itself was not modified here; its broader ownership semantics remain another audited component boundary.

## Runtime failure and native binding correction

Parent's Evidence/offline-prison-probe-20261015-180344.json reports actual DownAttack creation, EnableCollider activation and successful real Physics overlap: enemyTrappedtrue, correct RefHitCharacter and disabled collider. It then reports no PrisonExplosion state/no crush and timed expiry. This establishes a partial working chain and a failed animation continuation, not successful prison acceptance.

Root cause in this reconstruction: PlayCharCrushAnim had incorrectly translated call target0x2364318 as Animator.SetTrigger. Corrected ARM640x121AC6C branches to0x2364318, and retained ISIL explicitly identifies that target as Animator.Play(string). Maya's controller contains PrisonExplosion state but no corresponding trigger parameter or incoming condition transitions. The state is intentionally entered through the explicit Play method. The actualPrisonAtk.enemyTrapped condition and owner.Weak guard remain unchanged. The fix is in the real owner; the probe must not force Animator.Play to bypass it or arbitrarily clear Weak.

The same incorrectly assumed binding affected this worker's SettingsOptions and WhipController. Their retained ISIL likewise identifies Animator.Play, and their corrected ARM64 calls exactly0x2364318. All three owners now call Play with their exact state strings: PrisonExplosion; SettingsSlideIn/SettingsSlideOut; LeftAtk/RightAtk/Iddle. Tools/inspect-ai-flow.cjs now annotates0x2364318 as Animator.Play(string), preventing the same ambiguous unlabelled call from being reinterpreted. Parent is independently auditing other callers across the game. The earlier successful settings test asserted closeBG only and did not establish panel animation playback.

These fixes are saved but have not been compiled or retested by this worker. Future runtime evidence must be attributed to a subsequent build and separately assert actual Animator state/clip, not just visibility or trap flags. No native shipped-game defect is claimed.

## Timed crush visual producer recovered

CreateByTime was an additional stub on GameObject/BranchCrushAtk.prefab (its GUIDda6878eacf2233942b07c0fd98a1f1db is Maya's crushAttack reference). Corrected native Evidence/native-disassembly-CreateByTime.txt establishes Start0x1223700..0x122374C invokes MonoBehaviour.InvokeRepeating with exact serialized delay/time; ISIL independently names InvokeRepeating. Callback slot0x2814BD8 resolves literal Sumon. Sumon0x1223750..0x1223848 instantiates serialized creation at its OWN current world position/rotation. It does not assign owner layer, parent, material, event or modify the repeat interval. Native Update is RET and omitted.

The editable CreateByTime.cs now implements those exact two methods, retaining its .meta identity. The BranchCrushAtk instance serializes creation GUID822608a9531ffa34cbc4f9ccf09dd258 (BranchExplotion), delay0,time0. Those values remain untouched and are passed to Unity's native InvokeRepeating contract, without inventing a minimum interval or replacing it with a custom timer. Other actual callers such as UpEarthAtk2 serialize positive intervals (.1); shared timing behavior is preserved.

This is a missing visual/effect producer, separate from whether the short-lived crush attack was observed. Parent's fresh prison run already observed the real PrisonExplosion clip after the native Play correction; a per-frame search can miss a spawned projectile destroyed during the same physics cycle. Parent is moving its observer to synchronous GameEvents.onAttackLayerSetUp EntityId notification, without extending projectile lifetime or changing its collision behavior. That observer change belongs to the probe, not this product owner.

The CreateByTime source change was not compiled or executed by this worker. Subsequent root validation is recorded below; actual visual producer occurrence and FX parity remain separate from attack creation.

## Subsequent executed acceptance

Evidence/offline-prison-probe-20261015-182744.json passed after compilation:
real Maya down attack, timed collider activation, synthetic CPU placement,
actual trap callbacks, PrisonExplosion state, synchronous BranchCrushAtk event
with layer9 and correct cage target, cage destruction and cleared attacker
ownership. Errors=[]. Build identity and successful five-case suite are recorded
in combat-runtime-validation.md. The previous failures remain retained evidence;
miss/weak/interruption cases and exact visual FX parity are still unaccepted.

## Subsequent positioning producer and observer correction

SumonInOpponent is now restored, so BranchPrisonATK appears at the opponent's
actual position. Report191027 failed a frame-poll collider-enabled assertion
even though its synchronous observer saw correctly targeted BranchCrushAtk.
The trigger can trap the CPU and disable its collider in the same physics tick
that enables it; that polling failure does not establish absent activation.

The probe now accepts either a directly sampled enabled collider or the real
enemyTrapped flag plus exact RefHitCharacter target. It no longer repositions
an already trapped CPU. Actual input uses the active SetAButton pointer zone,
preserving native variant state. Shared zone discovery has one owner in the
main validation probe. An initial observer compilation failed with a variable
scope error (unity-windows-build-20261015-191346.log); it was corrected before
the successful fresh build, without modifying gameplay.

Evidence/offline-prison-probe-20261015-191606.json passed, errors=[]:
native opponent placement, no synthetic CPU relocation in this run, real trap,
PrisonExplosion, synchronous BranchCrushAtk layer9/correct cage target,
cage destroyed and attacker reference cleared. The collider's brief enabled
window was not sampled directly; timing is not newly accepted by that flag.
Gameplay assemblya8286d2228001b41dbf5896de8c5e6ab2e92ae47abc89c9961762e5ba9df5f05,
build00:15:34Z,0errors/22warnings. General aiming, miss/weak/interruption and
visual parity remain separate scope.
