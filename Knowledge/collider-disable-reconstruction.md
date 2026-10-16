# Collision-driven collider disabling

Restored DisableColliderOnCollition and DisableColliderOnCollition_LM using the REA skill workflow and retained original ARM64 input. Analysis used local Capstone and metadata tools; no REA MCP tools were connected and these outputs are not claimed as REA server Evidence. The native SHA256 is d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c. This is reimplementation, not recovered C# source.

## Observed native contract

DisableColliderOnCollition.OnTriggerEnter at 0x1228CF8 accepts incoming layer 8 or 9 only, different from the component's own GameObject layer. With onlyByPlayers false it disables the serialized col. With onlyByPlayers true it reads its OWN Description; a missing component or Type equal to Char prevents disabling. The incoming collider's Description is never inspected. Native field reads are col at +0x20, onlyByPlayers at +0x28 and Description.Type at +0x20. The name of the flag does not justify reversing these branches.

DisableColliderOnCollition_LM.OnTriggerEnter at 0x1228E74 ignores the incoming collider. It obtains its own BoxCollider and disables it only if that box's GameObject has tag Attack or Player. Both Start methods are genuine RET and remain empty.

Corrected native listings are Evidence/native-disassembly-DisableColliderOnCollition.txt and Evidence/native-disassembly-DisableColliderOnCollition_LM.txt. Slot 0x28144A0 resolves Component.GetComponent<Description>, 0x2814430 GetComponent<BoxCollider>, and literal slots 0x28144A8/0x2814DC0/0x2814668 resolve Char/Attack/Player. Retained Cpp2IL ISIL establishes API identities; printed disassembly is not used because its VA/raw mapping is wrong. Use Tools/disassemble-type.ps1 with the corrected address map.

## Serialized resources and deliberate adaptation

Tools/audit-collider-disable.mjs parses Unity YAML and records twelve prefab component references, including BranchDrillAtk, FloorBeam, FlowerBeam, shadow attacks, water attacks and TripleClaw effects. All authored onlyByPlayers values in this inventory are false; true-flag branches are fixture coverage, not proof of their natural use. No prefabs or original exports were edited.

LeftWaterAtk2 retains col fileID 0 in both the editable project and immutable original export. The implementation deliberately skips a missing reference instead of reproducing the native null exception. It does not substitute its own collider. This is a robustness adaptation with a separate missing-reference test, not native parity or recovery of an intended asset reference.

## Acceptance and limits

The MayaProduction scenario now includes fourteen synthetic real-physics cases: layer rejection, same-layer rejection, opposite layers in both directions, unrestricted source layer, own-description filtering, an absent incoming Description, the separate referenced-box target, missing col protection and the LM own-tag cases. It temporarily enables and restores each needed physics layer pair, destroys fixture objects and retains the existing save/preferences restoration. It does not call OnTriggerEnter directly.

Exact runtime/build reports, source/listing hashes, decoded metadata slots, prefab references and limitations are in Evidence/collider-disable-native-contract.json. Reproduce with Tools/Run-OfflineProbe.ps1 -Scenario MayaProduction after building, then Tools/audit-collider-disable.mjs with explicit runtime and archived build report paths.

Natural FlowerBeam activation, aiming, attack-caused victories, Android and original runtime/visual parity remain unresolved. The existing producer fixture still explicitly enables FlowerBeam's root trigger; restoring a disable component does not establish an enable owner.

Final runtime Evidence/offline-mayaproduction-probe-20261015-220052.json passed/errors=[]: all fourteen branch cases, four existing Maya producer cases and two floor-beam cases. Build Evidence/windows-build-validation-collider-disable-20261016-030029.json succeeded with 0 errors / 25 warnings. Gameplay assembly SHA2562429660093c1c48b88c13e59460c032ece40c3622ff5832e6a3270437cd06a54. Earlier thirteen-case report215824 predates the null-reference safeguard and is historical.
