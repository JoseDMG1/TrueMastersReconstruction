# Desktop action HUD visibility and size

The previous build's captured battle (`Evidence/hud-before.png`) showed just one small action indicator. GameController creates DesktopCombatHud from Awake before ButtonsController.i is necessarily registered, leaving attack and shield source references null. The old cloning code also depended on mobile hierarchies and used invalid normalized anchors for the super percentage.

Configure now defers graphics creation by one frame. The desktop HUD copies the original circle sprites and letter fonts into three independent fixed rectangles, with no Selectable, EventTrigger or mobile button scripts. Every graphic has raycastTarget=false. The row remains at the lower left of each player's viewport. Diameters are 112 reference pixels in single player and 88 in local split screen, previously 76/54. The super value uses a normal pixel offset below the third indicator.

The local camera distance multiplier changes from 1.1 to 1.2 (about 9.1% farther than the previous setting), for both close and far modes and both players. Single-player camera placement is unchanged.

Current acceptance: `Evidence/desktop-hud-final.json` result=passed, errors=[]. Sidecars `.hud-single.json` and `.hud-local.json` establish three and six visible noninteractive indicators, nonoverlapping geometry within their respective viewports, original attack/shield sprites and super fill/value at a controlled 50%. Both local camera horizontal distances measured approximately 4.8. Captures `.hud-single.half.png` and `.hud-local.half.png` were visually inspected. The half-charge fixture changes meter state directly and restores it; no new physical-controller acceptance is claimed. The existing harness restores save/preferences.

Build `Evidence/unity-action-hud-initialization-fix.log` succeeded with zero errors. Earlier `desktop-hud-visibility.json` failed because attack/shield were missing their source sprites before the deferred initialization fix; that result is superseded by `desktop-hud-final.json`.

Reproduce with the Windows player arguments `-rea-offline-probe <absolute-report.json> -rea-desktop-hud-probe`.

Later requested camera tuning increases the local-only multiplier to 1.6. The latest Windows build compiled successfully; the runtime captures above document the earlier 1.2 setting. Single-player zoom remains unchanged.
