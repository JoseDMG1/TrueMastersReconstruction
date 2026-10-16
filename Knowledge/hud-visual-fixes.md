# Battle HUD visual corrections ? 2026-10-16

Runtime screenshot comparison confirmed a vertically stretched camera eye, an oversized dark oval at top left, a vertical dark strip at top right and an absent settings gear.

The eye3 sprite rect is 219.85344 ? 117.84775; its UI rect was45 ?65 with nonuniform x scale1.5. The camera eye now uses uniform scale, rect67.5 ?50, and Preserve Aspect.

The screen-space diagnostic Evidence/offline-smoke-probe-20261016-033113.hud.txt identifies both Score/BG backgrounds and Settings/Gear as recovered SVGImage components. The five battle SVGImage components, including two inactive Shield_Img images, are replaced by standard UGUI Image with existing sprite/color/RectTransform preserved. Null background sprites draw the intended rectangles; gear uses its recovered raster sprite. No global SVG DLL replacement.

A prior PlayerStats/EnemyStats sprite hypothesis was disproved by screenshot and reverted. HUD diagnostics now include all Graphic subclasses and screen-space bounds in opt-in player probe captures.

Alternate controls requested this turn: WASD, left click attack, Space evade/shield/jump, right click super. Default arrows/Z/X/C remains. Older physical-input acceptance reports belong to the previous mappings.

Final verification: Evidence/offline-smoke-probe-20261016-033453.json passed; inspected Evidence/offline-smoke-probe-20261016-033453.Bending-Battle.png confirms horizontal eye, restored gear and both rectangular score backgrounds with no oversized oval/vertical strip. Windows build succeeded0 errors (Evidence/windows-build-validation.json, generated08:34:45 UTC). Gameplay DLL6ca9bfe6325e59bcb7e3755f0725f62eb195d4dcbc4dd7c6cf6a4fe94efe5144; scene assets changed in this build, so the DLL hash alone does not identify visual asset revisions. Physical new control mapping was not separately exercised.
