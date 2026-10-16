# Controlled moving-target flight and natural impact

The `MovingTarget` scenario exercises Sora's real UpAttack followed by UpAttack2 through UGUI and the authored Animator alternation. The latter naturally creates `AirAtkStrong2(Clone)` with the original `Follow` and `GoForward` components. Gameplay sources and authored prefab values were preserved.

Current checkpoint: `Evidence/offline-movingtarget-probe-20261016-030909.json` passed on assembly5f27b00b4a836b051af33902ee8adb6e255876858fa950c075296ff39ecbd0ad, build `Evidence/windows-build-validation-flight-final-20261016-025525.json` (0errors/37warnings).145live samples,2m target travel, zero drift/rotation error, contact and damage event at frame1279, Damage0->1. The refreshed `Evidence/moving-target-flight-acceptance.json` binds this run and is linked from `Evidence/flight-matrix-acceptance.json`. Read `Knowledge/flight-matrix-reconstruction.md` for the matching368-case/five-awake-route extension. The ten-route Sora regression and measurements below are the initial checkpoint on their separately recorded older assembly.

## Initial checkpoint measurements (historical)

`Evidence/offline-movingtarget-probe-20261016-021459.json` passed with no errors. Its structured `.flight.json` sidecar records153 live frame samples and10 actual corrective turns. Nine turns occurred while the target was moving, before impact. Every sample selected the opponent root. The CPU moved from `(0,0,17)` to `(2,0,17)` over0.3seconds, with maximum commanded-to-actual position drift0. The projectile launched from the real fighter animation; the fixture initially places the controlled fighter at `(0,0,-8)` to provide a measurable flight window within the stage.

At frame1274, approximately0.429seconds after creation, the projectile's own `OnTriggerEnter` observer registered contact with the CPU's real damage owner. In the same frame, the CPU's original `onDamageRecived` event fired with `Damage=1`, up from0. No collision callback, damage method, hit state or projectile overlap was manually invoked. This establishes one controlled natural hit, not every attack or an AI-versus-player match.

The early Update observer supplies the target path and snapshots the projectile before its normal movement owners run. LateUpdate records its actual rotation and position. `Follow.speed` is the authored1.5 multiplied by52.75, or79.125degrees/second. Native thresholds are raw signed cross-dot greater than0.1 or less than-0.1. Maximum expected-versus-actual rotation error was0 across the samples. The external audit independently recomputes the planar cross-dot from recorded positions/forward vectors and checks speed, target binding, physical contact and damage correlation.

Build `Evidence/windows-build-validation-moving-target-20261016-021430.json` succeeded with0 errors/35 warnings. Gameplay assembly SHA256 `6b67e19907e4fc2ffdf4894eeddf86a2f26cda6962b9467fdb8ba7520c2776ab`. The trace, launch, archived build, assembly, validation/gameplay sources, prefab and native listings are bound by `Evidence/moving-target-flight-acceptance.json`.

Ordinary Sora regression `Evidence/offline-abilityaim-probe-20261016-021618.json` passed all10 normal input routes on that same assembly, with fixture restoration. This is selected-fighter regression; the older all-roster results retain their separately recorded binary identity.

## Fixture boundaries

The CPU's AI was already disabled by the combat harness. The new fixture also disables its locomotion owners, suppresses Animator root motion and makes its Rigidbody kinematic, synchronizing both Transform and Rigidbody positions. Its real collider, damage component and Animator reactions remain active. These controlled movement settings, listener subscription and saved root pose/rigidbody settings are restored at completion or observer destruction. The main report separately confirms restoration of the original reconstruction save bytes. Natural knockback movement and CPU AI decisions are outside this fixture.

Post-impact samples include the projectile continuing beyond the target. Native raw cross-dot steering is not equivalent to a nearest-angle turn or automatic180-degree pursuit when a target lies directly behind it. The test checks the recovered contract per frame rather than requiring angular error to decrease throughout the entire projectile lifetime.

## Reproduction and previous diagnostic

Build with `Tools/Build-OfflinePlayer.ps1`, archive its build report, run `Tools/Run-OfflineProbe.ps1 -Scenario MovingTarget`, then run `node Tools/audit-moving-target.mjs <main-runtime.json> <archived-build.json> [ordinary-Sora-AbilityAim-runtime.json]`. Use sequential player instances so shared fixtures finish restoring. Ordinary `Moves`/`AbilityAim` no longer includes the previous unconditional moving-target assertion.

Reports015601,015826,020111 and020414 are historical failed observer experiments. They compared angular-error reduction over a fixed interval with insufficient position/physics records. The earlier claim that the final opponent position was inconsistent with the intended displacement was itself unproven: the original/commanded positions were not retained. The new controlled flight demonstrates working Follow steering and contact for this route, without assigning a definitive cause to those old failures.

Local REA work used the retained original ARM64 binary/metadata and corrected Capstone listings: Follow.Start0x128C0E4, Follow.Update0x128C2C4, AngleDir0x128C528 and GoForward.Update0x1295098. Cpp2IL's known-offset printed disassembly is not used as the instruction oracle. No REA MCP tools or installed rea CLI were available; these remain local observations, not REA server Evidence.

The earlier all-roster acceptance in `Evidence/ability-aiming-acceptance.json` remains bound to its own older assembly and successful stationary-target reports. This continuation validates one representative moving-target projectile family. Other projectiles, heights, edges, interrupted owners, awakened jump attacks and attack-caused match completion remain pending.
