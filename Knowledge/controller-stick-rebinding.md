# Controller rebinding and left stick correction

The controls panel treats gamepad input as a signal to inspect or assign. While open, it suspends EventSystem navigation events and the custom controller navigation; A no longer submits a focused UI button and B no longer closes the panel. Mouse interaction selects the field; Escape or CERRAR closes the panel. Navigation state is restored on close or destruction.

Previously, any directional button binding replaced its entire analog axis, including returning zero while the assigned buttons were idle. LocalControls now combines the stick with additional digital bindings: digital directions take precedence when pressed, and otherwise the stick remains available. Both players use this shared path. Keyboard controls are unchanged.

Directional capture accepts the corresponding left-stick deflection above 0.65 as well as a button. Capture first waits for held buttons/stick input to return to neutral. USAR STICK IZQUIERDO clears the four extra directional button bindings in the displayed player/preset. Direction labels show the stick and any additional button explicitly. Adjacent mapping buttons now fit their row spacing without overlapping hit rectangles.

The opt-in runtime fixture `-rea-controls-regression <absolute-report.json>` checks analog/digital composition, four directional capture predicates, A/B assignment callbacks and persistence across both players and both presets, and EventSystem isolation/restoration. It restores saved preferences and runs no simulated physical input. This fixture does not prove a new physical-controller gameplay session. Earlier physical sampling is retained separately in `Evidence/xbox-user-button-all-slots.json` and its summary.

Runtime results and build acceptance are recorded separately after running the current build.

Final controls acceptance: Evidence/controls-loading-final.json passed all 62 checks with no errors. The controls canvas now hides during loading transitions, cancels active capture and restores navigation. The latest Windows build completed with zero errors.
