# Directional attack dependencies: movement and delayed creation

REA skill applied to retained native evidence. This is source-derived reconstruction, not a claim that every move has passed runtime acceptance. Native input: `Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so`, SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`. Correct ELF mapping is retained in `Evidence/cpp2il-native-address-map.json`; Cpp2IL's printed Disassembly is offset incorrectly by 0x4000. Named ISIL was used only with corrected ARM64 branch/field verification.

## Restored owner contracts

Three previously empty components were initially implemented in the working Unity project; StartRotation was restored in a follow-up below. Serialized fields, class names and script GUIDs remain unchanged. Consumer inventory and exact prefab/component fields: `Evidence/directional-move-producer-contracts.json`. Source routes: `Evidence/roster-moveset-audit.json`.

**MoveRock**: FixedUpdate `0x129C85C`, ChangeDirection `0x129CB1C`, IsPlayerIsOk `0x129CCC8`, constructor `0x129D2AC`; corrected body listing `Evidence/native-disassembly-MoveRock.txt`.

- If ConfigAttackLayer.playerWeak, advance `(-up*.35 + forward*.25)*speed` every physics tick. This reads the configuration flag, not the separate legacy MoveRock.playerWeak field.
- Before readiness and below world Y=4, rise by `up*speed*sumonSpeed` each tick.
- At Y>=4 or after readiness, capture opponent direction once, normalize the full 3D delta, then clear Y. Move by this fixed direction times speed per tick and rotate child0 by `(rotateX,rotateY,rotateZ)` each tick. Do not normalize again after removing Y or insert deltaTime.
- Layer9 targets GuestPlayer; other layers target HostPlayer. IsPlayerIsOk uses the inverse owner lookup and flags early hit/fall states; it does not run from FixedUpdate, and no new invocation was invented.
- Native constructor defaults are sumonSpeed=.18, rotateY=4, rotateZ=1. Serialized consumers override these.

**MoveLight**: Start `0x129B9B4`, FixedUpdate `0x129BA90`, ChangeDirection `0x129BC78`, StartMov `0x129BFA4`; corrected listing `Evidence/native-disassembly-MoveLight.txt`.

- Start offsets world position by forward*position and schedules StartMov after timeToStart.
- Weak configuration advances `(-up + forward*.1)*speed` every physics tick, even before moving becomes true.
- Otherwise waits for moving, then copies the owner's rotation once when GoToOpponent=true and advances forward*speed per tick. The native calculates a normalized opponent direction and clears its Y, but does not use that direction for travel. Preserve this distinction: replacing it with LookAt would change behavior.
- Layer9 owner is HostPlayer; other layers use GuestPlayer. GoToOpponent constructor default=true; side prefabs explicitly false preserve authored yaw.

**SumonByTime**: Start `0x12C6360`, Sumon `0x12C63B4`; Update `0x12C63B0` is a native RET and remains empty. Corrected listing `Evidence/native-disassembly-SumonByTime.txt`.

- Start invokes Sumon once after time (constructor default1).
- Cancel creation when ignoreIfWeak and ConfigAttackLayer.playerWeak; or ignoreIfCollides and BoxCollider.enabled=false; or GameController.IsOutsideStage(position,27).
- Instantiate create at current position, copying rotation if configured. The false branch constructs the native zero Quaternion `(0,0,0,0)`, not Quaternion.identity. This may be normalized by Unity; runtime should establish any emitted warning.
- If attack=true copy the root layer. Native has no AttackLayerSetUp event here; observers must watch the instantiated child identity as well as events.
- Destroy this GameObject only after successful creation when destroyAfterSumon=true. Gate cancellation leaves it alive.
- Native online branch uses PhotonNetwork ownership/instantiate and is outside the offline reconstruction. The implementation reports unavailable and returns if PhotonNetwork.IsConnected. Original online mutation is not claimed.
- Constructor default copyParentRotation=true. Update remains a native no-op.

Generic metadata slot `0x2814C38` resolves Component.GetComponent<ConfigAttackLayer>; `0x2814430` resolves BoxCollider. Host/guest slots `0x2814910` / `0x2814908` were resolved from relocation-backed metadata. Constant reads on corrected ELF: `0x651B7C=.35`, `0x651BA8=.1`, `0x651C2C=.00001`, constructor `0x651CA0=(4,1)`.

## Bounded runtime fixtures to accept these contracts

Use actual cardinal input and both attack variants, then assert motion or secondary production as appropriate. Instantiation by itself is insufficient.

- Diana/Ren: BasicAttack RockH speed=.9, initial rise=.18/tick until Y4, then forward travel; DownAttack RocksJump speed=.8/rise=.152; UpAttack BigRock .7/rise=.175; left/right RockL/R .9/rise=.18; BasicAttack2 RockH2 .85/rise=.255. Observe a projectile early before impact/destruction; cumulative path or multiple physics samples is stronger than end-to-end displacement.
- Dante/Agata BasicAttack emits ShockAtk2 immediate .8/tick and ShockAtk delayed .7s with speed1/initial offset4. UpAttack includes multiple shots. Side variants have GoToOpponent=false and must preserve their yaw. Confirm travel after readiness and nonmovement during delay, excluding the initial offset.
- Maya BasicAttack LeafAtk immediate .5/tick; UpAttack2 LeafAtkBig waits1.5s then advances .5/tick. Other dependencies of Maya directional attacks remain incomplete.
- Amon/Rina right/left FireKickHR/HL spawn Bonfire after.7s unless weak; UpAttack2 NetworkFirePillar2 produces FireSpecialExplotion at.5s and Bonfire at.6s, with the latter gated by collider enabled. FireSpecialExplotion can itself produce Bonfire after.7s. Verify child layer, exact child name and delayed occurrence rather than only parent spawn.
- Raven UpAttack2 ShadowArrowTrap produces ShadowTrapFar after.65s, then destroys itself; false rotation-copy branch is exercised here.
- Maya side variant2 FlowerSumonerL/R produces flowerBonfire after.75s. Existing movement/collision stubs mean presence does not establish trajectory or hit fidelity.

## Prioritized remaining native gaps

Read-only scout traversed event object references and recursive prefab references. `Evidence/directional-prefab-empty-scout.json` retains fighter/state/prefab/script candidates. Its empty-body detection is lexical, and external prefab animation controller/clip callbacks are not recursively inspected. It is candidate coverage, not completeness.

1. **Water projectile motion — restored by parent after this scout**: MoveWater Start `0x129DA44` length0x19C, Update `0x129DBE0` length0x26C, ChangeDirection `0x129DE4C` length0x324, StartMov `0x129E170` length0xC. Originally empty, active Vinea/Eliot directional dependencies; the parent subsequently restored this owner and owns its current runtime acceptance. Fixture: both basic variants and up/down variants must travel after their serialized delay. See `Evidence/native-disassembly-MoveWater.txt`.
2. **Light side aim — restored in follow-up**: StartRotation.Start `0x12B3448` schedules Rotate `0x12B3498` (0x30 bytes, Transform.Rotate with stored Y angle). FixedUpdate `0x12B34C8` is RET and stays empty. Constructor `0x12B34CC` calls MonoBehaviour with no field defaults. Native `rot` is field+0x30 and `delay` field+0x34; the retained `initialRotation` quaternion is not read by these methods. Slot `0x28176C8` resolves literal Rotate. Start calls Invoke(nameof(Rotate),delay), and Rotate calls transform.Rotate(0f,rot,0f), a relative local yaw, once. Reached six enabled root components, all active with delay0: ShockAtk3L/R -10/+10 degrees from Dante/Agata UpAttack; ShockAtkL/R -20/+20 from Left/RightAttack; ShockAtkL2/R2 -25/+25 from Left/RightAttack2. These prefabs set MoveLight.GoToOpponent=false, so MoveLight does not overwrite their authored turn. Acceptance should capture rotation at creation, then compare Quaternion.Angle against Abs(rot) after the scheduled invocation; signed local yaw should also match, and later samples should show no repeated turn. Immediate creation-time polling alone may precede Invoke(0). All six prefab hashes, GUID ownership, fields and 12 fighter/state routes are in `Evidence/start-rotation-native-contract.json`. No runtime/build was performed by this worker for the follow-up.
3. **Earth prison — restored in follow-up**: PrisonController, SumonInOpponent and their directly required wall owner MoveObject now implement the native offline unit. Details and runtime fixture expectations are in `Knowledge/earth-prison-reconstruction.md`. Correcting the earlier scout classification: DestroyPrisonHit `0x12780A8` is a four-byte **B 0x1277F0C tail branch to DestroyAllPrison**, not RET. Method length4 does not classify a method as empty; inspect the instruction. See `Evidence/earth-prison-native-contract.json` and corrected native disassembly.
4. **Shadow trajectories/trap interaction**: MoveObjectLocal.Start `0x129C308`, Update `0x129C358` and StartMoving `0x129C45C` are substantive; empty on Spider/Wolf/Bird. TrapPowerUp.Start `0x12CEBC0`, Update `0x12CEC18`, CheckIfTrapNear `0x12CECBC` and PowerUpAttack `0x12CF2C8` are substantive and empty on most Raven projectiles. Fixture compare projectile movement and trap overlap, power-up effect and direction. See their native-disassembly files.
5. **Maya composite production and travel**: sumonOnCollider.OnTriggerEnter `0x12C70C8` (0x138), Sumon `0x12C7340` (0xF4) empty on LeafFrontHit/LeafAtkR/L/FlowerBeam. DragonWoodATK reaches MoveForward.Update `0x1246FA0` (0x1D0), MovePath.Update `0x1247204` (0x12C), PathManager.LateUpdate `0x1247E68` (0x290), FollowPath.Update `0x1232798` (0xDC), all empty. MoveForward.Start and FollowPath.Start are RET. FloorBeam ZigzagController.Update `0x12DD8C8` (0xEC) also empty. Separate fixtures need collision-generated child, path travel and follow chain, not just root existence.
6. **Fire collider evolution**: IncreaseCollider.Start `0x129727C` / IncreaseSize `0x12972CC`, ChangeCollider.Start `0x121C4CC` / ChangeSize `0x121C51C` are substantive empty methods; NetworkFireWall/NetworkFirePillar2. Fixture sample actual collider size before/after native delay and resulting overlap.

Do not spend restoration effort on the legacy AnimatorEventsController merely because it has empty callback names: none of the 12 current fighter prefab component inventories bind it; the active callbacks are owned by the reconstructed original-route components. WaterShieldOff.Start/Update/OnDestroy are genuine RET; its ActivateShield/DeactivateShield bodies are substantive and still candidates for the shield-specific audit.

This worker did not launch/build Unity or claim runtime acceptance. The parent owns the current directional probe and final build evidence.
