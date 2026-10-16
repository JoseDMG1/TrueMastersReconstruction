# Native shadow quality coordination and roster coverage

ShadowController was an active stub. It is restored using the REA skill workflow, retained original ARM64 input, corrected Capstone addresses, Cpp2IL ISIL API identities and exact metadata-slot decoding. These are local observations, not connected REA server Evidence. Native binary SHA256 remains d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c.

## Native contract

Start at 0x12BD11C subscribes ChangeShadow to GraphicsManager.graphicsChange, obtains its own DrawTargetObject, assigns targetDirection from GameObject.FindGameObjectWithTag("MainLight").transform and applies ChangeShadow immediately. MainLight is a tag, not an object name. Literal/type slots identify userQualityGraphics, GraphicsManager, MainLight and Component.GetComponent<DrawTargetObject>. OnDestroy at 0x12BD2D8 removes the callback. ChangeShadow at 0x12BD260 uses GameObject.SetActive(PlayerPrefs.GetInt("userQualityGraphics",0)==1).

The exact native equality enables these projectors only on MIN (preference1), not MED2/MAX3 or default0. GraphicsManager's existing quality handlers publish the change. The controller remains subscribed when its GameObject is inactive so that another change can reactivate it. It is not an OnEnable/OnDisable subscription. The implementation retains the publisher instance for symmetric cleanup and guards missing publisher/config/light instead of reproducing native null faults; it invents no replacement light or target.

With the direction assigned, DrawTargetObject.UpdateTransform 0x12E0AB0 looks toward own position plus targetDirection.forward, using the selected target axis as up. It does not use the light forward vector as up while preserving the old projection direction. Native Awake caches the local target offset; native OnEnable 0x12E0E48 does not recache that offset. Both points were corrected in the core reproducer. Runtime coverage includes a forward-vector check and controlled target displacement while inactive, followed by reactivation with the same relative offset.

## Runtime fixture

The ShadowRoster scenario uses fresh normal fighter scenes for all twelve roster entries, a disabled CPU and the main combat Animator of the selected fighter. It invokes the actual GraphicsManager.graphicsChange event with temporary preference values0/1/2/3/1, checks subscribed controller activation, tagged MainLight direction and texture release/recreation, then runs GPU silhouette/automatic animation/lifecycle checks. A synthetic controller creation/destruction fixture checks listener-count add/remove symmetry once.

Sora includes an authored inactive alternate controller that has not run Start. The fixture identifies live controllers from the publisher's actual invocation list and checks that dormant controllers remain inactive. The static inventory covers thirteen serialized controllers in twelve fighter prefabs; normal-form runtime acceptance does not cover that dormant alternate/awakened route.

The first attempt offline-shadowroster-probe-20261015-232922.json failed on MainLight binding: the initial implementation mistakenly searched by name, and the observer also included Sora's never-started alternate controller. ISIL establishes FindGameObjectWithTag; the source and observer were corrected before the next run. That failed report remains historical evidence, not accepted coverage.

The next report233218 passed its sixty activation/resource cases and twelve pose checks, but subsequent image review exposed an earlier core orientation error once MainLight was bound. Its assertions did not check the forward vector or offset preservation. That report and Graphics233515 are historical for the newer direction/follow checkpoint. New acceptance requires those explicit assertions.

## Preservation and limits

CombatTrip captures save bytes and preference presence/values before temporary ownership changes. CaptureFixture now also includes userQualityGraphics and userFrameCap. Graphics' ordinary branch now calls CaptureFixture explicitly before its changes. Earlier ordinary Graphics runs had not wired that snapshot despite documentation claiming full restoration; their GPU observations remain valid, but those old restoration claims were unsupported. Do not use their generic completion text as proof of snapshots.

The quality matrix fixture dispatches the native preference/event route; it does not click MIN/MED/MAX or establish resolution-switching behavior. GPU checks use controlled BasicAttack poses with animation events disabled and restored, not complete movesets or original visual parity. Mask/receiver shaders remain approximations; advanced shadow profiles, awakened alternate shadows, performance and Android remain incomplete.

Tools/audit-shadow-quality.mjs records exact native listing/source/publisher hashes, metadata slots, prefab references and selected runtime/build identities in Evidence/shadow-quality-roster-contract.json. Reproduce with Tools/Run-OfflineProbe.ps1 -Scenario ShadowRoster after a fresh build.

## Current quality and roster acceptance (supersedes earlier checkpoint limits)

Final build Evidence/windows-build-validation-shadow-quality-20261016-044023.json succeeded with0errors/33warnings; assembly SHA256 11c5bf91564a87b8978c0afd036a6ef070aafb8ee2285cf898c9435f1952c618. Final roster report Evidence/offline-shadowroster-probe-20261015-234058.json passed/errors=[] for twelve normal fighters: sixty quality cases, twelve follow cases, twelve automatic GPU animation cases and twenty-five cookie checks. Forward-vector assertions passed; maximum relative-offset error6.848052E-07. Listener add/remove symmetry passed. Dormant Sora alternate remains outside accepted runtime scope.

Final ordinary Graphics Evidence/offline-graphics-probe-20261015-234404.json passed/errors=[]; Vinea changed52bones/1770pixels through automatic rendering. Both final fixtures recorded restoration after explicit snapshots. Earlier ordinary Graphics restoration claims were unsupported because CaptureFixture was not wired; historical GPU observations remain bounded evidence. Current quality matrix uses actual preference/event dispatch, not resolution buttons. Approximate shaders, advanced plugin features, natural combat, original visual parity, performance and Android remain unaccepted. ShadowController coordination and normal-form all-roster GPU coverage are now accepted within these stated bounds. Contracts: Evidence/shadow-quality-roster-contract.json and Evidence/dynamic-shadow-core-contract.json. See Knowledge/shadow-quality-reconstruction.md for corrected light direction, target up axis and offset caching lifecycle.
