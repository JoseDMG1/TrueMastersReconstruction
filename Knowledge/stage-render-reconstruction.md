# Stage rendering investigation

Current checkpoint (2026-10-15T22:43Z): the white-floor cause is confirmed by a
real Windows GPU A/B, not just inferred. Disabling the dummy shadow Projectors
in the same frozen scene restores the floor's visible surface detail. The parent
has applied the approximate dynamic projector shader replacement with a backup;
its new GPU/cookie validation passed (final checkpoint below). Dynamic shadow texture generation
is still unimplemented. Earlier unresolved/hypothesis sections below preserve
the investigation history and are superseded by the final checkpoint.

Static audit: `Tools/audit-stage-render.cjs` uses the existing format-aware Unity
YAML parser and retains object IDs, serialized fields and input SHA-256 in
`Evidence/stage-render-static-audit.json`. This audit examines Stage0, Bending
Battle, StartMenu and Character Selection; it does not establish runtime values.

## Confirmed producer omission: stage skybox

Stage0 root has SetSkyBox (script GUID `8f9b6337993741f80880863c44e9c4d6`),
whose serialized skybox is WispySkyboxMat (GUID
`e22c726595d75a547be2f834038a0aa2`). The battle scene's default skybox is
6SidedMegaSun (GUID `f7c9848eba8bf2546852fc1f8f6f3268`). The working SetSkyBox
Awake was empty. Corrected ARM64 `0x12AE268` loads field +0x20 and tail-calls
`0x23809F8`; mapped ISIL identifies that callee as RenderSettings.set_skybox.
Restored the exact assignment `RenderSettings.skybox = skybox`, without editing
sky materials, texture mappings, or lights. Evidence:
`Evidence/native-disassembly-SetSkyBox.txt`,
`Evidence/Cpp2IL-ISIL/IsilDump/Assembly-CSharp/SetSkyBox.txt` (ISIL only; its printed
Disassembly is invalid). Native SHA-256:
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.
Runtime confirmation of the selected skybox and its appearance remains pending.

## White floor remains unresolved

Stage0 Cylinder's renderer references Dirt (`14d57ea2d3494be4ab44adc7d748598d`)
and CylinderBorder references Dirt 1 (`e3eedd6802df56c4b897e1a3891a6ed5`).
The prefab contains two active directional lights with serialized intensities
2.5 and 1.5. No stage material-color initialization script is attached to these
objects. Stage0 scripts are SetSkyBox and FadeAudio only. Existing separate real
Unity asset probe `Evidence/material-loaded-validation.json` confirms Standard
shader and brown material colors with normal textures, but that asset-only probe
does not confirm the instantiated renderers' assigned materials, material
instances, loaded shader properties, mesh normals or camera state. Those runtime
facts must be captured before changing their producer. No arbitrary light,
material color or renderer override was introduced in this investigation.

## Black menu screenshot timing

The persistent LevelChanger controller's default state is Fade_In. Its recovered
clip lasts 1 second and animates Canvas/BlackFade Image alpha from 1 to 0, plus
Image enabled from 1 at t=0 to 0 at t=1. LoadingScreen sets FadeIn only after
async activation has completed and its coroutine resumes. Therefore a sceneReady
signal plus .4 seconds is insufficient evidence of a finished fade. A screenshot
probe should wait for LoadingScreen.loading == false and BlackFade's Image to
be disabled (or Fade_In normalizedTime >= 1), before capturing and invoking the
next navigation action. This is a hypothesis for early black captures, not a
confirmed runtime diagnosis; no fade animation or menu code was changed.

No Unity process or new runtime check was launched by this worker.

## Runtime checkpoint: renderer assignments confirmed

The parent-run real Unity report
`Evidence/offline-tournament-probe-20261015-172112.renderers.json` confirms the
instantiated Stage0 Cylinder uses Standard/Dirt with color
(.48113209, .42656705, .34223923, 1), and CylinderBorder uses Standard/Dirt 1
with color (.32075471, .26972184, .19487362, 1). Cylinder world bounds are about
39.5 x 100 x 39.5 centered at y=-50; Border bounds are 54 x 40 x 54 centered
at y=-20.01. Both directional lights are enabled with intensities 1.5 and 2.5.
These facts rule out missing ground renderers, an unspawned stage, an incorrect
visible material name/shader, and a runtime color reset as explanations for the
white floor in the examined snapshot. They do not establish fragment output.

Additional bounded static checks: both battle cameras serialize HDR enabled,
no target render texture and default rendering path. Dirt_NRM imports as normal
map (textureType 1), sRGB false. Stage lights serialize no color-temperature
conversion and zero bounce intensity. None of these checks identifies a proven
producing bug. No further floor fix was made. Remaining investigation requires
runtime camera/color-space/HDR and actual material property/keyword/mesh-normal
state, or a controlled rendering comparison with retained evidence. The selected
stage skybox assignment fix is independent of the unresolved floor output.

## Stronger bounded hypothesis: dummy projector repainting

`Tools/audit-shadow-projectors.cjs` and
`Evidence/shadow-projector-static-audit.json` audit all twelve shipped fighter
prefabs with input hashes. All twelve have enabled Projectors referencing
BlobShadowProjector (Sora and Rina contain two projectors). The material GUID
9701fd2d7471f374bae787c1a0f7733b selects the shader GUID
ce0f3e7827cc956459e41f028a8e267c:
DynamicShadowProjector/Projector/Dynamic/Shadow With Linear Falloff.
Its exported shader SHA-256 is
f3949c9f041451b43ad8bc0c547816eb81f8eca58704d6932ec1463b96ac1b16.
The code explicitly carries DummyShaderTextExporter and writes constant opaque
white, without Blend, ZWriteOff, unity_Projector or projected bounds handling.
This is a concrete producing shader defect independent of Dirt's correct shader
and brown colors: a projector pass can repaint receiver meshes after their own
material pass. This plausibly explains both full white ground and white Dante
body portions blending into it. Actual causation remains inferred until the
parent's controlled GPU projector A/B result. No product materials/shaders/scene
were changed for this diagnosis.

Primary upstream lookup found Nyahoon's official shader documentation describing
dynamic/static and LinearFalloff variants, but no exact licensed base-plugin
source or package version in its official public repositories. The full plugin
is linked as an Asset Store asset on its official product page. The public
DynamicShadowProjectorExtensionForLWRP repository contains extension scripts,
not base shaders; the examined tree has no declared LICENSE. The separate
ProjectorForLWRP repository is MIT and contains a recoverable reference shader
and Built-In projector matrix path. Exact pinned source and license are retained
under Evidence/vendor-references/nyahoon-projector with provenance hashes.

A concrete candidate approximation, MIT notice, sources, known differences and
GPU test plan are staged outside Assets at
Tools/shader-candidates/dynamic-shadow-projector/README.md. The dynamic variant
was subsequently applied and validated as recorded below; the static variant
remains unapplied. It needs actual dynamic shadow texture generation from the
currently stubbed ShadowTextureRenderer/DrawTargetObject owner to claim animated
shadows; a neutral missing-texture fallback alone does not restore them.

## Confirmed A/B result and applied approximation

Real Windows Direct3D11 run
`Evidence/offline-graphics-probe-20261015-173810.json` reports passed, errors=[],
and records the three active projectors, their shader names and a controlled
comparison in the same frozen arena. The parent inspected both captures:
`offline-graphics-probe-20261015-173810.projectors-original.png` has white ground;
`offline-graphics-probe-20261015-173810.projectors-disabled.png` visibly recovers
the textured ground and ordinary character/UI shading. Original enable flags
were restored. This establishes projector rendering as the white-floor producer
for the examined runtime scene; material color/shader remapping is not needed.
The shader's constant-white fragment output explains that runtime result.
The specific Dante screenshot was not this A/B's fighter configuration, so its
individual body rendering still requires rechecking after the shader change.

Parent applied the dynamic approximation only with
`Tools/restore-projector-shader.cjs --apply`.
`Evidence/projector-shader-restoration.json` records generatedUtc
2026-10-15T22:39:25.430Z, original dummy shader hash
f3949c9f041451b43ad8bc0c547816eb81f8eca58704d6932ec1463b96ac1b16,
replacement hash27468b329b752fe012f9616ea245ff2ec6f7f0335797c3ababd8ba8f39bf6f5d,
common include and MIT license hashes, preserved shader .meta hash and backup
Evidence/WorkingBackups/projector-shader-2026-10-15T22-39-25-429Z.
No product stage material, directional light or scene was edited by this repair.
The non-dynamic candidate was not applied.

The repaired-build run `Evidence/offline-graphics-probe-20261015-174126.json`
passed without errors. Original and black-cookie captures were opened: the
ordinary scene shows textured ground, and a black synthetic cookie darkens a
bounded projected area. Tools/Compare-ProjectorCaptures.ps1 measured the full
1920x1080 frozen frames; its retained .pixel-comparison.json reports:
original versus disabled =0 changed pixels; disabled versus white cookie =0;
white versus black cookie =251499 changed/darkened pixels,0 brightened above
RGB tolerance2, changed bounding box x578..1376/y542..1079. This establishes
neutral fallback/white-cookie and bounded darkening for the observed orthographic
scene, not exact original falloff, perspective, every stage or transparent
receivers. Temporary materials/textures, enable flags and timeScale were restored
in finally. ShadowTextureRenderer/DrawTargetObject remain stubs, so exact animated
dynamic shadows are still not reconstructed.
Existing directional-light shadows visible in the disabled capture do not prove
that the dynamic texture shadow owner works. No worker Unity/build was launched.

## Newer basic dynamic texture acceptance

The earlier texture-owner stub status is superseded for the authored zero-blur/zero-mip Built-In profile. See Knowledge/dynamic-shadow-core.md and Evidence/dynamic-shadow-core-contract.json. Final Graphics232058 passed actual GPU silhouette/binding, automatic pose response (52bones/1741pixels) and RT disable/recreation in Diana/Vinea scene. Mask and receiver shaders remain approximations; advanced plugin helpers/settings and original visual parity are incomplete. Static inventory covers12fighters/13profiles, runtime coverage is bounded to the selected scene.
