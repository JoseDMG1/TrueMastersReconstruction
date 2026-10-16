# Maya DragonWood path reconstruction

REA skill applied to retained native evidence. This is a source-derived reconstruction, with runtime acceptance still pending. Native identity is SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`. Correct ELF VA/raw mapping comes from `Evidence/cpp2il-native-address-map.json`; Cpp2IL printed Disassembly has the known 0x4000 mapping error. Corrected listings are `Evidence/native-disassembly-{MoveForward,MovePath,PathManager,FollowPath,TransformData,StopMovePath,ChangeMoveSpeed}.txt`. Named ISIL identifies Unity API calls; constants, fields and branches were checked against corrected ARM64.

`Evidence/dragonwood-path-native-contract.json`, produced by `node Tools/audit-dragonwood-contract.mjs`, retains the exact prefab components/fields, controller entry conditions, clip events and SHA256 of current implementation and listings. No prefab, script GUID or immutable export was changed.

## Actual input route and composition

Maya **DownAttack2** is selected by the Attacks state-machine entry transition `1109279359913592680`: `Vertical < 0` and integer `DownAttack == 1`. This was read from the controller, not inferred from the name suffix. Clip `DownAttack2_4.anim` has offline CreateAttack at clip time .28333333 with DragonWoodATK GUID `098f8d00dd15f004693f2d2e79b03193`; its earlier CreateNetworkAttack at .016666668 is the separate online path. Production should be tested through InputReader/cardinal input and ButtonAttack.

DragonWoodATK root has MoveForward.speed=0, ChangeMoveSpeed newSpeed=13/delay=1.5, existing Follow for opponent yaw and StopMovePath. Child **Head** starts local(-8,-2,2), with MovePath xSpeed10/ySpeed8/zSpeed0, curve values 0/.5/0 at normalized times 0/.5/1, JumpTime1.5, leftJump=true, maxJumps6 and dropSpeed=-.1. Head owns PathManager with 60 history points and spacing .2. Twelve existing following parts use indices **5,5,10,15,20,25,30,35,40,45,50,55**. The parts are authored children; this unit does not instantiate more children.

## Restored contracts

- **MoveForward.Update 0x1246FA0**: normal movement is local `Translate(0,0,speed*deltaTime)`. Weak ConfigAttackLayer instead uses world position displacement `(-up*.35 + forward*.25)*speed*deltaTime`. Start is native RET and remains empty. Constant -.35 was checked both from immediate 0xBEB33333 and ELF float at VA0x651A30. This owner differs from MoveRock/MoveLight, which move per physics tick.
- **ChangeMoveSpeed.Start 0x121D370 / ChangeSpeed 0x121D3C0**: Invoke ChangeSpeed after delay, then write sibling MoveForward.speed=newSpeed. Update is native RET. Constructor defaults10/3 were read from VA0x651E10/14; prefab overrides13/1.5 remain. Metadata generic slot0x2814890 resolves Component.GetComponent<MoveForward>; slot0x2814888 resolves ChangeSpeed literal.
- **MovePath.Start 0x1247178**: capture local position; schedule Disable only if disableJumpTime !=0. Update0x1247204 accumulates elapseTime while below JumpTime; on a later frame it resets0, flips leftJump and increments numberOfJumps. Before disabled, y=curve.Evaluate(elapseTime/JumpTime)*ySpeed. When jumps>=maxJumps, disable further curve updates and add dropSpeed to y **each Update, without deltaTime**. z increments zSpeed*deltaTime; x increments +xSpeed*deltaTime when leftJump and negative otherwise. Apply initialPos+(x,y,z). Disable0x1247330 only sets disableJump=true: by itself it freezes the current vertical offset, while the max-jumps branch causes falling. Constructor defaults maxJumps3/dropSpeed-.1 restored; serialized maxJumps6 retained. Literal slot0x2815C88 resolves Disable.
- **PathManager.Start 0x1247B88**: capture world position and append missing history until limitPositions. Entries begin at current world position then descend world Y by spacing; rotation is the native `Quaternion.LookRotation(Vector3.up,Vector3.up)`. Existing serialized entries are retained. LateUpdate0x1247E68 acts only when distance from lastPosition is strictly >spacing, computes normalized full3D direction and LookRotation(direction,up), then inserts samples at spacing,2*spacing,... strictly below distance. It removes the oldest entry per insertion and keeps constant count; afterward lastPosition=current position. This drops any subspacing remainder, matching native. MoveLastToFirst0x1247D50 discards a fetched old value and replaces the last entry with the new front entry. Constructor spacing=.2 restored.
- **FollowPath.Update 0x1232798** copies selected history world position and rotation directly. Start is native RET. Indices were preserved; there is no interpolation or new tracking target.
- **TransformData ctor0x1247B74** writes supplied position and rotation. The exported stub incorrectly replaced both with default zero; preserving that stub would collapse every recorded history sample.
- **StopMovePath.Start0x12C5E48** resolves GameController.GetAttackOwner(root layer), captures DamageOnCollision and registers StopMove on onDamageRecived. StopMove0x12C5F44 sets movePath.numberOfJumps=movePath.maxJumps; Update is RET. OnDestroy0x12C5F60 unregisters the same method. Reconstruction retains the exact registered publisher instead of re-reading a possibly reassigned public owner during cleanup.

PathManager additionally rejects nonpositive/nonfinite distanceBetweenPoints and disables itself with an error. That is an explicit malformed-input guard against the native unbounded loop; the authored .2 path is unaffected. Other invalid serialized input handling was not broadened.

## Bounded runtime acceptance to perform

Trigger actual Maya downward variant1, verify observed animator state DownAttack2 and DragonWoodATK creation from its real event. Observe both root and Head separately: root speed initially0 then13 after1.5 scaled seconds; Head local x changes before root advances, with roughly +10 units/sec for its first jump, y curve peak near4, then x sign changes after JumpTime. Existing Follow can turn the root, so world endpoint distance alone is weaker than accumulated root travel/local Head samples.

After all Start callbacks, history count should be60 and all indices0..55 valid. Across later samples, follower world poses must match their indexed history poses, allowing normal Update/LateUpdate frame ordering. History front must evolve as Head moves, and separated indices should form a trail. Observe orientation as well as position; root existence alone does not establish this chain.

For interruption, deliver a real hit to Maya through DamageOnCollision, observe numberOfJumps becomes maxJumps, then disableJump=true and falling y changes by -.1 per Update. This event should not destroy the dragon immediately. A separate cleanup fixture should destroy DragonWood and then hit the previous owner, proving listener removal without MissingReferenceException. Native lifecycle defaults and full ten-second lifetime need observation; no runtime or build was performed by this worker.

sumonOnCollider (LeafFrontHit/LeafAtkL/R/FlowerBeam) and ZigzagController (FloorBeam) do not occur in the DragonWood prefab and remain separate unfinished Maya units. A passing DragonWood fixture would not establish their production or motion behavior. All online behavior remains deferred.
# Subsequent executed acceptance

Evidence/offline-dragonwood-probe-20261015-190932.json passed, errors=[].
Actual Maya down-input alternation created DragonWood, with no attack transform
or animator-state mutation. Over5.201756scaledseconds/243samples, root speed
changed0->13 at1.501787s; accumulated root travel48.37587 and head travel81.85461.
Head local X range15.41948, local Y range3.999998, history60points and three
observed jumps. All12 authored followers moved and each had171 matching
position/rotation samples while the root was moving (minimum position error
0..5.34e-7 and rotation error0).

Synthetic owner GetDamage(.5) produced exact delta.5, one real GameEvents damage
notification and ConfigAttackLayer.playerWeak=true. That API does not emit the
onDamageRecived UnityEvent used by StopMovePath; actual collision interruption
is not accepted by this fixture. No CPU relocation was needed in the executed
helper; an optional unexecuted relocation draft was removed and Tools retains
the exact tested helper source. Cleanup deliberately destroys the test attack;
natural lifetime and full aiming/damage/visual fidelity remain unaccepted.

Gameplay assembly317849cd908478f430f65e7a2c80e63b9025d1b4451812844a213641d6880acb;
fixture/source: Tools/unity-validation/OfflineDragonwoodProbe.cs.

## Later real-collision interruption acceptance

Evidence/offline-dragonwood-probe-20261015-192028.json passed on assembly
7d99e897ea66370c8268deb123965ed1c1f8cc40162c85d05345022256818c25.
It repeated the motion/chain checks, then instantiated a recovered AirAtk inside
Maya's real collider on opposing layer8, using normal AttackLayerSetUp. It did
not invoke HitAnimation or onDamageRecived manually. The natural damage callback
fired once; StopMovePath changed jumps3->6 (max6), disableJump became true and
local head Y changed-.2999997 across three Updates (native expectation-.3).
This supersedes the earlier unaccepted collision-interruption statement for
this bounded synthetic-overlap case. Natural aiming, other collision branches,
ten-second lifetime, post-destruction listener behavior and visual parity remain
unverified. The overlap projectile and exact listener owner were cleaned up.

