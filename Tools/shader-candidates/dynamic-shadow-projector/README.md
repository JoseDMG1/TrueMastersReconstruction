# Proposed projector shader reconstruction (not applied)

Status update, 2026-10-15T22:39Z: the parent confirmed the dummy-projector cause
with a real frozen-scene GPU A/B, then applied ONLY the dynamic shader, common
include and MIT license to Assets using Tools/restore-projector-shader.cjs.
Evidence/projector-shader-restoration.json records backups and before/after
hashes. The static candidate remains unapplied. The new build/GPU cookie probe
has not yet been reported successful. The original proposal and limitations
below are retained as history; they do not describe current deployment status.

These files are candidates outside Assets. No Unity import, compilation, GPU test,
material edit or scene edit has been performed for them. They are an explicitly
approximate reconstruction, not recovered Asset Store shader source.

Primary references:

- Nyahoon official shader documentation: https://nyahoon.com/products/dynamic-shadow-projector/projector-shaders-for-dsp
  distinguishes dynamic/non-dynamic and Linear Falloff variants, and states the
  dynamic version hides parameters controlled by the component. Direct origin
  fetch returned HTTP500; the search provider's cached primary page was readable.
- Unity6 Projector reference: https://docs.unity3d.com/6000.0/Documentation/Manual/class-Projector.html
- Nyahoon's MIT ProjectorForLWRP repository, pinned commit
  97d226b297aeaaef7d1ad611a25575e36085ba03:
  https://github.com/nyahoon-games/ProjectorForLWRP/tree/97d226b297aeaaef7d1ad611a25575e36085ba03
  Its shader and cginc contain a Built-In Render Pipeline path using
  unity_Projector/unity_ProjectorClip, despite the repository's LWRP name.
  Local exact references and hashes: Evidence/vendor-references/nyahoon-projector.
  The original MIT notice is retained as LICENSE here. The candidate adapts its
  projection/multiply mechanism; no Asset Store source was copied.

Proposed behavior:

1. Multiply receiver RGB with the shadow cookie (Blend DstColor Zero); preserve
   depth and alpha (ZWriteOff, ColorMaskRGB); preserve serialized culling/offset.
2. Explicitly discard fragments outside projected XY and normalized depth0..1.
   The current dummy re-renders whole intersecting receiver meshes opaque white.
3. Interpret _ClipScale as near-plane fade sharpness, _Alpha as strength,
   _Ambient as minimum attenuation, with linear weakening toward far depth.
   This is a reconstruction choice; exact Nyahoon math is not established.
4. Retain _DSPMainTex/_DSPBaseAlpha property names for material compatibility,
   but transparent receiver texture/alpha integration is NOT reconstructed.

DynamicShadowLinearFalloff.shader keeps the exact exported dynamic shader name.
Its _ShadowTex is hidden, with white as neutral fallback until a runtime texture
owner supplies a real cookie. StaticShadowLinearFalloff.shader has the exported
non-dynamic name and exposes a normal cookie property with the exported gray
fallback. The original dynamic export omitted the runtime texture property from
its shown Properties block; adding a hidden property here is an explicit design
choice to keep its missing texture neutral, not evidence of original defaults.

Critical dependency: existing ShadowTextureRenderer/DrawTargetObject components
are method stubs and do not currently produce the dynamic shadow render texture.
A shader fix alone can eliminate repainting artifacts and can render an assigned
cookie, but cannot claim restored animated character shadows. Keeping actual
dynamic shadows requires restoring the texture owner (native evidence or exact
licensed upstream package), target capture, and cleanup/lifecycle. No procedural
blob, guessed skin color, material-color override or new scene lighting is used.
The shipped plugin version and full plugin license remain unknown; the MIT license
above applies only to the fetched ProjectorForLWRP references/candidate derivation.

Parent validation plan, after existing Projector A/B confirms causation:

- Freeze scene/camera; capture with projectors at original states, disabled, and
  restored. Compare Stage0 floor and Dante body. Restore states in finally.
- In an isolated test only, supply a synthetic black-on-white cookie to the
  candidate and capture receiver intersections. Outside the volume must remain
  unchanged; white cookie must leave RGB unchanged; black cookie must darken,
  never brighten. Restore/release all temporary materials/textures/projectors.
- Validate normalized depth convention and near/far XY clipping on orthographic
  projector1.5/range10 (actual prefabs). Perspective is not verified.
- Only then import a backed-up concrete shader replacement preserving .meta GUID,
  identify it as approximate, and record missing dynamic texture generation.
