# Offline control settings and touch joystick

REA skill applied to retained native ARM64 evidence and named ISIL. Corrected ELF mapping is `Evidence/cpp2il-native-address-map.json`; Cpp2IL printed Disassembly is offset incorrectly by0x4000 and was not used as byte authority. Reproducible bounded audit: `node Tools/audit-joystick-contracts.cjs`; full fields, native source identities, scene components, callback serialization, InputManager axis configuration and literal/generic resolutions are in `Evidence/joystick-native-contracts.json`. This is **source-derived restoration, not runtime acceptance**. No Unity build, process or probe was launched by this worker.

## Preferences and UI owner

JoystickSettings methods restored from Start0x123EA6C, JoystickOn0x123EC10, DpadOn0x123ECEC, ControllerOn0x123EDE4, Invert0x123EEB4 and Update0x123EF10. Native defaults contain no explicit field assignments. Start skips connected spectators, reads PlayerPrefs string **Joystick** with default **Joystick**, then dispatches exact case-sensitive values Joystick/Dpad/Controller. Unknown stored strings are retained without selecting another mode. ControllerInverted is an integer default0; exactly1 turns on invertedToggle, whose dynamic callback sets inverted and writes1. Invert(false) writes0. Native does not explicitly Save PlayerPrefs, force inversion false when the saved value is0, or change the joystick mode while changing inversion.

Mode methods write the same preference and set the three toggle values in native order. Dpad enables dpad, hides joystick and enables buttons A/B. Joystick hides dpad, enables buttons A/B, then enables joystick. Controller hides dpad and joystick; **it leaves buttons A/B as they were**. No new layout/state assumptions were introduced.

Active Bending Battle scene component1328 owns these references: dpadToggle1271, joystickToggle1307, controllerToggle1396, invertedToggle1372, dpad184, joystick297, buttonA224 and buttonB280. Its original script GUID is unchanged. Mode choices are **EventTrigger eventID4 (PointerClick)**, void calls with m_Mode1, from components1272/1308/1397. The Dpad Toggle.onValueChanged calls InputReader.ActivateDpad dynamically (m_Mode0) on reader1370; inversion Toggle.onValueChanged calls Invert dynamically. This indirect toggle callback is the native link to InputReader.dPad. Do not replace it with silent toggle updates or add duplicate direct ActivateDpad calls.

## Joystick input geometry

Joystick and VariableJoystick retain all original fields, serialized names and GUIDs. Base constructor handleRange1, deadZone0, axisOptionsBoth, snap flagsfalse and inputzero. Variable constructor moveThreshold1, Fixed mode and fixedPositionzero. HandleRange/DeadZone/MoveThreshold setters take absolute value; AxisOptions setter stores its argument. The native AxisOptions getter body at0x123DF34 is RET with no identifiable return-value load. Its editable C# field getter is an **inferred API contract**; no active workflow uses that getter, and it is not claimed as proven native behavior.

Joystick.Start0x123DF60 captures its RectTransform and parent Canvas, takes abs of handle/deadzone, sets background pivot and handle anchors/pivot to(.5,.5), resets handle position, and logs the original error if no Canvas. Variable.Start0x123E880 calls base, captures the background's authored anchored position and applies its mode. Active scene component1575 is VariableJoystick with handleRange1/deadZone0/Both/no snapping/threshold1/Fixed, background1078 and handle1077.

OnDrag0x123E118 selects canvas.worldCamera only for ScreenSpaceCamera, obtains background's screen position, divides pointer offset by `(background.sizeDelta/2)*canvas.scaleFactor`, then zeroes the excluded axis. Magnitude <=deadZone becomeszero; magnitude>1 becomesnormalized. The handle's anchoredPosition equals processed input times background radius times handleRange. PointerDown runs OnDrag immediately; PointerUp resets both input and handle tozero.

Horizontal/Vertical expose raw processed input unless snapping is enabled. For Both axes, angle against Vector2.up determines snap dead sectors: horizontal iszero at angle<22.5 or>157.5; vertical iszero for67.5<angle<112.5. Otherwise use signed1, retaining zero exactly. A single-axis configuration signs the active value. These thresholds/branches come from corrected SnapFloat0x123DD54, not assumed square quantization.

Variable Fixed mode preserves authored background anchoredPosition and keeps it visible. Floating/Dynamic modes hide it until PointerDown, then place it at the converted pointer position. PointerUp hides non-Fixed backgrounds. Dynamic HandleInput0x123E96C moves the background by `normalised*(magnitude-moveThreshold)*radius` when magnitude>threshold, then applies base deadzone/clamping. ScreenPointToAnchoredPosition0x123E428 uses RectTransformUtility.ScreenPointToLocalPointInRectangle, the baseRect pivot/size and background.anchorMax; failed conversion yieldszero. Only this active Variable family was restored; unrelated joystick subclasses remain outside this bounded task.

## Direction ownership and legacy controller limitation

InputReader.Update0x123B8BC calls SetJoystick whenever dPad=false **before** testing blockInputs. SetJoystick0x123BC0C reads this actual joystick's Horizontal and Vertical into InputSet.x/y. When unblocked and keyboard=true, native uses **Input.GetAxis**, not GetAxisRaw, for Horizontal/Vertical; held J/K/L set A/B/S. Current implementation's only required change here was GetAxisRaw -> GetAxis. Native per-frame debug-string allocation/logging was already omitted and remains omitted. Dpad callbacks continue to write InputSet directly when dPad=true. No polling flag was invented.

MobileInputs.DetectInputs0x129A8E8 loads BattleSceneSingleton.inputReader.inputSet.x/y directly at0x129A9CC..0x129A9E0, quantizes with thresholds +.3/-.3, then writes Animator SHorizontal/SVertical with smoothing. **Those quantized values are native locals**, not writes to public legacy fields MobileInputs.x/z. The C# producer was corrected to use locals and preserve those field values. Existing locomotion routing, fighter movement and InputReader callbacks remain intact. Its offline Update gate already matched the substantive branch; online spectator/native authority is outside the offline acceptance.

JoystickSettings.Update first requires GameController/MainCharacter and a finished Counter (counter absent or value<=0). Start axis==1 selects Controller. A saved Dpad value returns without legacy-axis writes. In Controller mode, P2Horizontal/P2Vertical seed values, nonzero P3 components override independently, then nonzero P4 components override independently. Inversion negates **vertical only**. Fire1==1 calls GameController.Attacking, Fire2==1 Jumping, Fire3==1 Special each frame. In Joystick mode it reads the restored VariableJoystick getters. It then quantizes the legacy path: abs(vertical)>.8 wins and zeroes horizontal; otherwise abs(horizontal)>.9 wins and zeroes vertical; weaker axes independently use strict +/- .2 gates. Boundary equality does not take the stronger branch.

Crucially, those GameController.UpDown0x1293850/RightLeft0x12938D0 calls write **MobileInputs.z/x legacy fields**, whereas the native MobileInputs locomotion path reads InputReader.inputSet. This native pipeline does **not establish a physical controller -> InputReader locomotion bridge**. Restoring a new bridge solely from presumed intent would invent semantics. The settings UI/preferences, real touch joystick and native legacy dispatch are restored; physical gamepad locomotion remains an explicit unresolved native gap. No control-mode flag or hidden merge of these two owners was added. Current InputManager contains all ten referenced controller/action axes; availability of a physical controller has not been measured.

## Proposed acceptance

- Back up presence and exact values of Joystick string and ControllerInverted int before launching. Exercise actual PointerClick callbacks and dynamic inversion Toggle callbacks; assert exact preference values, three toggle states, actual object active flags and Dpad reader flag. Restore both key presence and value after each probe. Controller mode must not assert buttons A/B hidden.
- Save each recognized mode, reload scene, confirm Start restores it. Save unknown mode and confirm it is not silently normalized. Inversion uses exactly1 as saved-on;0/default remain the scene's authored state unless the user toggles it.
- For active Fixed joystick, obtain the actual screen background center and radius including canvas.scaleFactor. PointerDown atcenter =>Directionzero; drag half-radius right =>Horizontal~.5/Vertical~0, handle displacement half-radius*handleRange; drag beyondradius =>magnitude1; PointerUp=>Direction/handlezero. Verify InputReader receives those values while dPad=false and the actual fighter can move after countdown.
- Switch Dpad, confirm joystick inactive and touch drag no longer overwrites InputSet. Use original directional UI events to change InputSet and release tozero. Test the real character route with blockInputs respected.
- Fixture subclasses or reflection may measure deadzone, snapping, axes and Floating/Dynamic separately; label such setup explicitly and restore authored mode. Do not mistake those fixtures for original scene settings.
- Physical controller tests must report separate outputs: native axis reads, legacy MobileInputs field writes and actual InputReader/fighter response. A preference switch alone is not accepted physical gameplay.

All proposed runtime checks remain unexecuted here. Source audit covers the active Bending Battle scene and the five touched owners; it does not prove every menu, joystick subclass, controller device or Android input path.

## Subsequent executed Windows acceptance

Evidence/offline-menu-probe-20261015-190827.json passed, errors=[], on gameplay
assembly317849cd908478f430f65e7a2c80e63b9025d1b4451812844a213641d6880acb.
Original PointerClick callbacks selected Dpad, Controller and Joystick modes;
preference values, toggle/active flags and reader Dpad mode matched contracts.
Actual Right Dpad PointerEnter/Exit produced input1->0 and fighter travel1.708534.
Dynamic inversion Toggle set true and native ControllerInverted=1. Controller
mode retained A/B buttons; no physical controller gameplay was asserted.

Actual Fixed joystick PointerDown at center was zero; half-radius drag produced
(.5,0), InputReader forwarded(.5,0), beyond-radius drag clamped to(1,0), fighter
travel1.790758, PointerUp reset both joystick and reader tozero. No direct input
values were forced to simulate that drag. CPU was disabled for this isolated
control fixture. Original key presence/values for Joystick and ControllerInverted,
other captured prefs and exact reconstruction-save bytes were restored.

The same report repeated129 translations,299/300 stage purchase and JSON readback,
settings Play states/settled poses. It does not establish mode reload, unknown
saved strings, snapping/deadzone, Floating/Dynamic modes, every option, Android
touch delivery or physical gamepad locomotion. Those remain distinct checks.
