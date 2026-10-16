# Element-specific offline owners

This pass translates corrected ARM64 from native binary SHA-256
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.
Correctly mapped disassemblies are retained as
`Evidence/native-disassembly-LightWeapon.txt`,
`Evidence/native-disassembly-LightWeapon__Light_d__10.txt`,
`Evidence/native-disassembly-LightWeapon__Dark_d__11.txt`,
`Evidence/native-disassembly-GrassSpecialController.txt`,
`Evidence/native-disassembly-GrassSpecialController__ChangeWings_d__5.txt` and
`Evidence/native-disassembly-MoveSpecialRock.txt`. Exported C# stubs and Cpp2IL's
incorrect printed Disassembly are not claimed as original method bodies.

## LightWeapon

Start (`0x1242568`) adds each serialized swordMesh/shieldMesh renderer's material
instance to the respective existing list. Native calls Renderer.get_material,
not sharedMaterial or all material slots. LightSword (`0x124241C`) and
LightSwordAndShield (`0x12428AC`) start recoverable light coroutines. LightShield
(`0x12422D8`) keeps the emission on; DarkShield (`0x1242ABC`) starts darkening.

The Light coroutine (`0x1242E58`) uses duration .2s, increments t by deltaTime/.2
once per .02s wait, runs while t<=1, and sets _EmissionColor. Yellow uses the
material base color plus (.1,.1,.05,1); blue uses (0,.5t,t,1). Recover launches
Dark, which waits .2s, then sets emission (1-t,1-t,1-t,1) using the same loop
conditions/timing (`0x1242CB8`). No clamp/final black assignment was added:
native can overshoot t and does not perform such finalization. Constants come
from native floats VA0x652310=.2, VA0x651BA8=.1, VA0x651B48=.05,
VA0x651BC8=.02, VA0x651B2C=.2. Slot0x2815B90 resolves _EmissionColor.

Concrete consumers: Dante prefab has three swordMesh and three shieldMesh
references, yellow=false. Agata has one of each, yellow=true. Animation clips
DownAttack2_3 and Taunt call LightSwordAndShield; Taunt2 calls LightSword.
No shader/keyword repair was inferred from this translation.

## GrassSpecialController

StartSpecialAtk (`0x1236E8C`) starts its wings coroutine, then starts
StatsController.GrassHealing with opponent determined by comparing this fighter's
layer with GameController.MainCharacter.layer, then calls
DamageOnCollision.RestoreDamage. Usage0x28156C0 resolves GetComponent<DamageOnCollision>,
and0x28156C8 identifies StatsController. Existing health and damage owners remain
responsible for those effects; this pass does not duplicate their rules.
ChangeWings (`0x123702C`) waits serialized growDelay, calls ChangeSizeTo(one),
waits shirnkDelay and calls ChangeSizeTo(zero). Maya prefab owns this component
with growDelay0/shirnkDelay6 and a serialized wings reference; Special_0 clip
emits StartSpecialAtk. The previously restored ChangeSize is reused.

## MoveSpecialRock

Start (`0x129D2CC`) inherits layer from parent.parent, schedules StartMoving after
delay, and sets Description Damage/Defense to20. Before movement, FixedUpdate
refreshes the 3D normalized direction to GuestPlayer for layer9, otherwise
HostPlayer; it discards Y after normalization. Once ready, displacement is
serialized speed per physics tick (native contains no deltaTime multiplication),
child0 rotates(4,4,0), sound plays once, and Damage/Defense become2/1; delay1.5
uses3/2 instead. StartMoving (`0x129D744`) also enables SummonOnCollision.
Generic usage slots0x28144A0/0x2817100/0x2816A58 resolve Description/AudioSource/
SummonOnCollision. Guest/Host type slots0x2814908/0x2814910 are explicitly resolved.
EarthSpecial prefab uses five rocks with delays .6,1,1.5,2,2.5 and speed1;
RockSpecial uses delay1/speed.8.

## Coverage and remaining checks

Source methods are restored with original serialized fields and .meta identities.
No Unity process, compilation, gameplay, GPU appearance or full roster validation
was performed by this worker. Parent owns centralized checks. In particular,
EarthSpecial/RockSpecial's spawn parenting must be validated in their actual
runtime invocation before claiming their movement works. Other roster-specific
components still contain stubs; these three owners do not establish that all
12 fighters or every special are complete.

## Runtime correction: material-cache ownership (roster probe)

Real Windows report `Evidence/offline-roster-probe-20261015-172833.json` and
matching log observed NullReferenceException at LightWeapon.Light mat.color,
called immediately by LightSwordAndShield in Character Selection after selecting
Agata. A format-aware audit (`Tools/audit-lightweapon-materials.cjs`, output
`Evidence/lightweapon-material-boundary-audit.json`) checks both working hosts
and the immutable WithNative scene export with SHA-256 identities.

Observed cause: scene component3450 on Agata serializes sword=[fileID0], while
its swordMesh=[2887] points to a real SkinnedMeshRenderer with material GUID
c38f8d9e193775f48a8748d65018e17f. Its shield lists are empty. The immutable
WithNative scene has the same null entry, establishing it predates the recent
implementation. Combat Agata and Dante prefabs and scene Dante have empty
material lists plus valid renderer references. The native Start appends renderer
material instances to the material lists; a literal append retains the preexisting
null entry and the animation callback then consumes it. This evidence identifies
a serialized list-entry problem, rather than requiring a destruction-timing
hypothesis. It does not prove why the shipped/exported scene contains that entry
or that the original Android runtime reproduces the failure.

Reconstruction correction: Start now replaces sword and shield with fresh runtime
lists sized to their source mesh lists before adding each renderer.material.
This makes the renderer bindings the single producer of the per-instance material
cache. All callbacks and coroutines consume that cache unchanged; no null-skipping
or fallback color was added. Public field names and .meta identity are preserved,
and neither scene YAML nor immutable evidence is changed. This intentionally
differs from native append behavior for nonempty pre-serialized material lists;
all four working LightWeapon hosts were audited, and only the Agata scene's null
entry was nonempty. Start list initialization was reviewed statically; no Unity
or build was launched by that worker. The parent's real repeat
`Evidence/offline-roster-probe-20261015-173352.json` passed: all twelve shipped
fighters completed selection, arena spawn and one basic-attack sample without
exceptions, including Agata. Original reconstruction save bytes and captured
preferences were restored. This validates the cache correction and bounded
roster path, not every special, defense or shader appearance.
