# Diana lateral evade rock correction

Date: 2026-10-17 UTC. Scope: player-reported lingering rock after lateral evade.

## Evidence and limits

The available REA connector was not callable; root's `npx -y rea-agents@6.5.0 doctor --client codex --json` failed loading `@modelcontextprotocol/client/dist/index.mjs`. No client configuration was modified. This analysis uses retained local native disassembly and recovered asset YAML, and does not claim a new REA engine session or observed original-game runtime.

Retained target: `Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so`, SHA256 D5669FA920C5034EC1ED5B7988149825430F65EA557BA7882567C0790DA2DD8C.

`Evidence/native-disassembly-MoveObject.txt`, complete StartMoving 0x129C0CC..0x129C2D4, captures current world position into endpoint at 0x129C13C/148 and adds AddVector only when nonzero (0x129C158..1A4). It does not restore the original spawn position for a zero vector. FixedUpdate 0x129C010 uses per-physics-tick clamped Lerp(speed), without deltaTime. Finish 0x129C2D8 just stops movement. DestroyByTime Start 0x1289CE0 schedules original lifetime.

Both recovered and working RockJumpFX / RockJumpFX2 / RockJumpFX_LM / RockJumpFX_LM2 originally contained a +3Y first movement (delay0, speed.1, finish.5s), then zero-vector second movement (delay.5, speed.05, finish1s), and DestroyByTime=3s. The root components are exactly those three scripts plus AudioSource; the sole mesh child has MeshFilter and MeshRenderer only. There is no Animator, scale controller or separate visibility component that could hide the stationary elevated rock.

Normal JumpL_3 / JumpR_2 animation events Create at .33333334 refer to RockJumpFX / RockJumpFX2. EarthJumpL / EarthJumpR CreateFX at .16666667 refer to the LM pair.

## Adjustment

Only the second movement vector changes from (0,0,0) to (0,-3,0) in all four working prefabs. It begins retreat at .5s and sinks during the following second. Original first rise, movement speeds/delays, audio and 3s cleanup remain. This is a bounded gameplay presentation correction requested by the user; the new vector is not presented as a recovered original value.

The mesh Rock_03_LOD0 bounds have centerY=-.04789573, extentY=1.3259215; mesh child localY=-2. Therefore its visible top lies rootY-.72197423, so returning root to approximately zero puts the mesh fully below the floor. At .02s fixed ticks a .05 Lerp for1s traverses about92.3% of the3-unit retreat (~2.769). The originally elevated root reaches ~2.78, so predicted final root is ~.01 and mesh top ~-.71. Exact invoke/fixed-tick ordering must be observed in Unity.

## Runtime acceptance prepared

`-rea-offline-probe <absolute-report.json> -rea-diana-evade-probe` selects Diana via existing harness; CPU disabled and existing save/preferences fixture restored. `OfflineDianaRockProbe` uses actual event prefab references from the loaded controller, instantiates isolated clones at world origin, and records scaled age/root Y/renderer top at .46/.62/1/1.56/3.16 seconds. It requires rise>2, retreat>2.5, top<0 at1.56s, destruction by3.16s. Clones are destroyed in finally. Sidecar: `<report>.rocks.json`. This covers actual prefab lifecycle and geometry, not natural evade-input acceptance or original video parity.

No Unity build was run by the Diana worker. Parent owns build and runtime result.