# Particle mesh CPU readability recovery

## Observed runtime blocker and source chain

Parent's Evidence/offline-specials-probe-20261015-180546.json/log ran the real two-phase awakening/special probe through Sora,Vinea,Diana,Amon,Aura,Eliot,Ren,Rina,Dante,Agata, then entered Raven. Raven awakening completed; the next real Special callback instantiated an animation-driven child attack and Unity6000.6.5f1 reported: Read/Write is disabled on mesh Torus used by Particle System Shape Module. The recorded stack reaches SummonAttack.CreateSimpleAtk. This is a reproduced reconstruction-runtime failure, not a claim about the original Android player's behavior.

Static source chain: Raven Special clip creates BigDemonAtk.prefab via CreateAttack at clip.76666665 (GUID051bee62fe2cb764787c5fa637265392). The BigDemonAtk animation invokes CreateSimpleAtk at1.7666667,2.6666667,4.116667 with TripleClawFXL/R/Front GUIDs592d33c765cbcff4a913e085601bbbfe /97e20fdfcdaf18c4ab83078955eee6f9 /4031554a72300b3448bdb85267d1bffd. Their enabled ParticleSystem ShapeModules reference MeshRenderers, whose same-GameObject MeshFilters reference Torus.asset GUID9db67e53203331b489b496757831acea, fileID4300000. Therefore scanning only ShapeModule.m_Mesh would miss the actual producing boundary.

## Applied narrow correction

Only UnityProject/Assets/Mesh/Torus.asset changed: serialized m_IsReadable0 ->1. All geometry, vertex/index buffers, materials, .meta identity, consumers, controllers and immutable exports remained untouched. Torus is referenced by15 enabled ShapeModule MeshRenderer consumers, including nine across the three claw FX and six shock effects. The type13 module values and renderer/MeshFilter PPtrs are retained, rather than inferred from prefab names.

Tool: Tools/unity-audit/restore-particle-mesh-readability.mjs. It uses YAML parsers for .meta identity and selected full ShapeModule fields, follows direct mesh/MeshRenderer/MeshFilter/SkinnedMeshRenderer PPtrs, and only accepts referenced owned serialized Mesh objects. It resolves source-span of Mesh.m_IsReadable, checks it is0/1, changes only that scalar, reparses and asserts complete Mesh semantic equality except the intended flag. It guards every write with the planned SHA256, makes exact byte backups first and rolls back already-written files on handled mutation failures when current identity still matches the owned written bytes.

Actual command executed:

`node Tools/unity-audit/restore-particle-mesh-readability.mjs --apply --mesh-guid 9db67e53203331b489b496757831acea`

Result:678working .prefab/.unity/.asset files examined,34 configured shape mesh references,12 referenced Mesh assets,0unknown references,1 changed asset. Evidence/mesh-shape-readability-applied.json retains consumer source hashes/PPtrs, mesh GUID/fileID, before/after SHA256, original immutable-export SHA256 and difference semantics. All12 examined working meshes compared semantically equal to native export after normalizing only readability; only Torus was selected/applied.

Backup: Evidence/WorkingBackups/mesh-shape-readability-2026-10-15T23-17-10.321Z, including manifest and original Torus.asset bytes. Original export source: Evidence/RecoveredUnityProject-WithNative/ExportedProject/Assets/Mesh/Torus.asset, unchanged. No provider installed/configured and no Unity/player/build launched by this worker.

## Idempotence and pending scope

The same exact --apply/--mesh-guid command ran a second time and reported changes0, unknowns0. Evidence/mesh-shape-readability-verification.json records it; the first applied manifest and backup pointer remain preserved. Tools default command is a read-only plan. An explicit selected GUID narrows mutations but keeps the complete mesh-consumer inventory available.

Eleven body meshes referenced through configured SkinnedMeshRenderer shape properties remain UNCHANGED: Maria_J_J_Ong_LOD2, Lower_Armor_LOD2, Body_LOD0, Head_Hands_LOD2, obj3D_Object_1_p_28_LOD0, Paladin_J_Nordstrom_LOD2, Lola_LOD2, D0208068_LOD2, Ch40__mod_FIN__V01_01_Ch40_LOD2, Kachujin_LOD2 and arissa_Body_Geo. Their references include shape types14 and some retained other-type properties; configured references alone do not establish that every one currently requires CPU readability at runtime. The manifest marks them inventory-only/unselected. No broad all-character geometry/import flag sweep was performed.

Parent must build the subsequent checkpoint and repeat Raven's actual special child sequence, Maya specials and pending trap/menu/impact cases. A successful static flag change or idempotent migration is not a runtime acceptance result. Unity CPU readability can retain additional CPU mesh storage; no runtime memory cost of the added Torus copy was measured. No global graphical fidelity claim is made.

Tool application additionally requires an explicit --mesh-guid and revalidates every retained consumer source hash before mutation. A bare --apply cannot silently enable all eleven inventory-only body meshes. These guards do not change the already-applied Torus asset or the zero-change idempotence result.

## Subsequent runtime result

The root built the corrected project and executed Raven again:
Evidence/offline-specials-probe-20261015-182511.json passed awakening, natural
discharge and phase1 Special, observing five attack entities with errors=[].
Maya and the prison/menu/impact follow-ups also passed in the separately retained
Evidence/offline-combat-suite-20261015-182744.json. See combat-runtime-validation.md
for fixture limitations and assembly hashes. No global mesh flag sweep or visual
fidelity acceptance follows from these bounded results.
