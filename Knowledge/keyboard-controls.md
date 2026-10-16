# Desktop controls: current Windows acceptance

Default: arrow keys move, Z normal attack, X evade/shield, C super. Alternate: WASD move, left click attack, Space evade/shield/jump, right click super. Neither Shift key is bound. The WASD + RATÓN checkbox persists AlternateKeyboardControls independently of inversion.

This is a user-requested desktop feature. InputReader samples before MobileInputs, preserves screen input separately, ignores unpressed mouse hover and suppresses alternate mouse attack over actionable UI. GameController guards desktop touch-zone callbacks. Super fires on the combined rising edge. Countdown, pause, focus loss and disable clear controls; physical pause/focus acceptance is pending.

## Historical Windows acceptance (previous mappings)

Evidence/offline-desktopcontrols-probe-20261015-214806.json passed with no errors. Two fresh Sora scenes, disabled CPU, actual Windows OS key/mouse input in the focused player. Default Z/X/C and alternate left-click/left-Shift/Space entered BasicAttack, left directional evade JumpL, awakening and phase1 Special. Eight cardinal movement/release cases passed. Super charge was accelerated through ChargeSpecial. Holding super before readiness did not trigger until release/repress. Cursor hover did not attack. The layout checkbox persisted across scene reload without inversion changes. Both screenshots were inspected and the option fits the panel.

Evidence/offline-menu-probe-20261015-215010.json passed with no errors on the same assembly. Dpad/joystick movement/release, inversion independence, settings and purchase fixture regression passed. Original save bytes and preference presence/values were restored by the probes.

Current Windows build Evidence/windows-build-validation-controls-20261016-024758.json succeeded with 0 errors and 25 warnings. Gameplay assembly SHA256 ff47aa8fb7ee511de062e70e4f9324a6d6f25a6dfce7252a0cf39ef74db39779. Exact paths, source hashes and limitations are recorded in Evidence/desktop-controls-acceptance.json. Reproduce with Tools/Run-OfflineProbe.ps1 -Scenario DesktopControls after building.

## Limits and next checks

Neutral shield, other evade directions, remaining roster, natural super charging, physical focus/pause edge cases, physical OS checkbox click and gamepad are not accepted. Existing MUT/INV clipping and character overexposure remain. Full game reconstruction and Android remain incomplete. Earlier reports tested older bindings and do not establish current mapping acceptance.

The user clarified left Shift only after the recorded runtime acceptance. InputReader and GameController now omit the right Shift alias; that removal is a subsequent change.

Latest left-Shift-only Windows rebuild: Evidence/windows-build-validation-controls-leftshift-20261016-025334.json, succeeded with 0 errors and 25 warnings. Assembly a5522362d4371d0aa830647ea32ea323474e7343ccb148d1116308b910039490. Existing physical-input acceptance belongs to the preceding assembly; no repeat was required for the removal of the unused right-Shift alias.
