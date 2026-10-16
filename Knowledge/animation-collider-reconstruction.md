# Animation-dependent collider window and Maya flower route

DisableColliderAnim was restored from the retained original native binary following the REA skill. Local Capstone, the corrected VA/raw map and Cpp2IL ISIL supplied the observations; no connected REA MCP tools or original Android runtime were used. Source and listing hashes, resolved metadata slots, binary constants, prefab inventory and exact acceptance reports are recorded in Evidence/animation-collider-native-contract.json.

## Native window contract

Start 0x1228858 starts DefineAnim(delay). Coroutine MoveNext 0x1228AF4 first waits scaled delay, then repeats WaitForSeconds(.02) while its GameObject is on layer 1. Layer 8 selects HostPlayer.hostPlayer's Animator; every other resolved layer selects GuestPlayer.guestPlayer's Animator. It invokes CheckAttack immediately and every .05 seconds. The native static slots identify HostPlayer/GuestPlayer; selecting an animator from an intuitive ownership assumption would change this behavior.

CheckAttack 0x1228914 acts only when the selected animator's GameObject Description.element equals Light and its layer-0 state IsName UpAttack2. It sets this component's BoxCollider.enabled false strictly between normalizedTime .2 and .45, and true outside that open interval while the state/element match. With a different state or element it leaves the collider value unchanged. Literal slots identify Light, UpAttack2 and CheckAttack. Float data VAs 0x651B2C/0x651C0C/0x651BC8/0x651B48 decode to .2/.45/.02/.05. The original public delay and private anim fields remain; generated dummy coroutine scaffolding was replaced with an ordinary C# iterator.

Twenty-five prefab files contain twenty-six serialized instances. Controlled runtime tests use real delayed binding and InvokeRepeating with the actual Maya/Amon animators. Actor element and state are temporarily controlled, animation events disabled, then restored. Both layer-selection branches, initial delay, the layer-1 wait, six normalized-time window cases and nonmatching element/state preservation passed. These tests do not establish natural Dante/Agata attack acceptance or exact boundary sampling.

## FlowerBeam: evidence changes the pending question

GrassAnimatorController's UpAttack references UpAttack_1.anim. Its events create FlowerBeam with CreateAsChild at 3.3666666 seconds and FlowerSpecial with CreateAttack at 3.4833333 seconds. The original FlowerBeam root BoxCollider and Description are both disabled. SummonAttack.CreateAsChild copies pose/layer and parents the clone without enabling those components; that native routine is already reconstructed.

The authored Maya upward directional/button route was exercised through InputReader and the real UGUI attack PointerEnter callback, with CPU disabled and controlled initial actor poses. FlowerBeam appeared parented to Maya, its root collider and Description stayed disabled throughout 140 observed frames, and the object expired naturally. FlowerSpecial appeared separately with enabled damage Description and collider. No manual collider enable was used for this route. This supports the visual/damage split for the observed Grass UpAttack route, rather than a missing FlowerBeam enable owner. Other emitters and original runtime parity are not established.

The older isolated producer fixture still explicitly enables FlowerBeam to test sumonOnCollider; it must not be cited as proof of natural root activation. No asset or invented enable behavior was added.

## Reproduction

Build with Tools/Build-OfflinePlayer.ps1, then run Tools/Run-OfflineProbe.ps1 -Scenario MayaProduction. Tools/audit-animation-collider.mjs accepts explicit runtime and archived build JSON paths. Keep the exact report/launch assembly identity; earlier runs predate subsequent source checkpoints.

Final acceptance: Evidence/offline-mayaproduction-probe-20261015-221526.json passed/errors=[] on assembly a68322f582c151a460047b77a4cb3db9b67f2a4213a94a329c166499ed4630fd. Build Evidence/windows-build-validation-collider-geometry-20261016-031515.json succeeded, 0 errors / 27 warnings. Nine geometry cases had zero sampled/final error; six animation windows and all existing physical/producer/floor-beam regressions passed. Natural flower route again observed140 frames with disabled visual root and active separate FlowerSpecial.
