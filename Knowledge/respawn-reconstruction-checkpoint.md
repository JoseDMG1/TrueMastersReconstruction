# Death, falling and respawn investigation checkpoint

Investigation interrupted for the requested handoff. No runtime scripts were changed during this second assignment; Respawn.cs, Death.cs, SurvivalController.cs and NormalMode.cs remain stubs. Do not mistake the generated evidence for completed logic.

## Evidence produced/read

Correct mapped ARM64 dumps freshly generated using Tools/disassemble-type.ps1:

- Evidence/native-disassembly-Respawn.txt (respawn VA0x12A76B8, CreateNewChar0x12A7828, Update)
- Evidence/native-disassembly-Death.txt (OnStateEnter0x12267C8)
- Evidence/native-disassembly-SurvivalController.txt
- Evidence/native-disassembly-NormalMode.txt

Read Respawn ISIL for named-call hints; its printed Disassembly remains invalid and is not authority. Generic lookup0x2814D10 resolves GameObject.GetComponent<AnimatorStatesManager>. Native Death.OnStateEnter writes that component's offset0x40, which must be matched to the native field map before implementation. The Death StateMachineBehaviour bool death chooses true/false. It is not itself a ScoreController.CheckPoints producer.

RAG query `Respawn Death NormalMode Falling CheckPoints` was attempted but correctly rejected because Knowledge changed after the last index build. Rebuild the index after all handoff documentation changes; do not claim that this rejected query returned findings. Earlier successful scoring RAG retrieval remains documented in scoring-reconstruction.md.

## Confirmed source-derived respawn behavior

Respawn.respawn0x12A76B8: HostPlayer tag sets world position(7,5,0). Other fighter in non-Survival sets(-7,5,0). Other fighter in Survival consults SurvivalController.disableRespawn; if true it returns; otherwise it destroys its gameObject, calls CreateNewChar, then RespawnOff. CreateNewChar is a substantial native routine, not implemented yet: it builds candidate character indices, removes currentChar, picks random remaining entry, updates currentChar, uses CharactersManager.RemoveRandom, loads Resources/PhotonPrefabs prefixed character path, instantiates at(-5,2,-5) with the old rotation, attaches a component, enables AI/on, rewires LookAt targets, plays a native animation state, updates GameController.OpponentCharacter and BattleSceneSingleton.Opponent. Resolve all generic/string slots and corrected branches before translating; do not use ISIL register clobber artifacts as source.

SurvivalController native Start0x12B6988 sets currentChar0 and singleton. SurvivalModeOn0x12B69E0 sets survivalMode true. RespawnOff0x12B69EC sets disableRespawn true and invokes RespawnOn after1s. RespawnOn0x12B6A44 clears flag. Update and OnDestroy are native RET; safe owner-aware singleton cleanup can be a documented reconstruction decision.

## Animation event evidence

Seven existing clips Assets/AnimationClip/FallingToLanding{Shadow,Light,Grass,Fire,Earth,Air,Water}.anim contain functionName:respawn at line116188. Exact event times, other events and state transitions remain to be extracted with YAML parser before modifying clips. Seven controllers AirAnimatorController, char1_Animation_controller, EarthAnimatorController, GrassAnimatorController, WaterAnimatorController, ShadowAnimatorController, LightingAnimatorController contain Falling and Death triggers/states and FallingToLanding states. They have not yet been audited for transition conditions, assigned StateMachineBehaviour instances or event routing. Preserve all existing animation/meta assets.

## Next work and validation limits

1. Restore Respawn and SurvivalController from mapped native evidence; restore Death state flag using field-map match.
2. Trace actual existing transitions Falling -> FallingToLanding -> respawn and Death -> DelayWin/result, plus state flags and damaged-fighter reset. Coordinate AnimatorEventsController/AnimatorStatesManager ownership with AI agent; DamageOnCollision ownership with damage agent. CheckPoints currently receives direct calls from ScoreController.OnTriggerEnter; further producers and reset side effects are not fully traced.
3. Audit NormalMode references before implementing: its naming alone does not establish it is used by the current battle composition.
4. Root must run real normal nonfinal-point respawn, three-point result, timeout and Survival transition probes. No Unity was launched by this worker. No new compile claims apply to this unfinished assignment; root's earlier menu-to-arena smoke passed before this investigation and cannot establish respawn functionality.

The prior scoring assignment IS saved: ScoreController.cs + ResultsController.cs implementation, Score singleton compile correction, Knowledge/scoring-reconstruction.md. The parent later reported menu-to-arena with gameStarted=true and zeroerrors; this is distinct from a tested score/respawn loop.

# Resumed implementation checkpoint

The earlier interrupted status above is historical. Respawn.cs, Death.cs and SurvivalController.cs are NOW implemented in the editable project; NormalMode remains untouched pending proof it participates in current composition. Backups: Evidence/WorkingBackups/respawn-2026-10-16. No .meta changed.

Death.OnStateEnter generic slot0x2814D10 is GameObject.GetComponent<AnimatorStatesManager>; native field map offset0x40 is precisely AnimatorStatesManager.death. The behavior copies its serialized bool to that field when the component exists. SurvivalController restores currentChar0 singleton Start, survivalMode setter, disableRespawn1s cooldown and owner-safe singleton cleanup (new reconstruction cleanup).

Respawn restores host/non-survival guest positions and Survival replacement. Correct native loop0x12A79FC compares index against CharactersManager.characters.Count (includes '?' entry); removes currentChar, picks remaining random index, stores selected index and passes its character through RemoveRandom. Correct generic slot0x2816D48 is AddComponent<GuestPlayer>. Spawn is(-5,2,-5) with old rotation. AI enabled/on true; generic LookAt references are reassigned, Animator.Play literal FallingToLanding resolved by slot0x2816828. GameController.OpponentCharacter and BattleSceneSingleton.Opponent are assigned.

Explicit ownership correction: the reconstructed replacement is published immediately to GuestPlayer.guestPlayer, GameController.guestPlayer and battle/controller opponent references, with tagGuestPlayer/layer8, before LookAt targets and animation are assigned. Native relied on GuestPlayer.Start for that identity publication while reading the old singleton earlier in the same call. Immediate publication establishes a single replacement owner and avoids consumers resolving the soon-destroyed opponent. Host and main-character LookAt point at the new fighter; replacement LookAt points at host. This is an intentional lifecycle correction, not a claim that native execution was observed broken. Native Respawn.Update only logged host and LookAt names per frame and is omitted. CreateNewChar stays synchronous; no network calls/providers used.

## Structured asset audit

Tools/audit-respawn.mjs uses existing parseUnity YAML parser (failsafe schema preserves object IDs) for38controllers and9Falling*.anim clips. Exact file SHA256, documentIDs, states, transitions, behavior script refs and event payloads retained in Evidence/respawn-animation-audit.json. Clip event map is parsed independently at its top-level indentation boundary, preserving all event data while avoiding reprocessing large curve arrays.

All7 elemental fighter controllers have Falling -> FallingToLanding, Death terminal (no transitions), Falling2 0 0 terminal, FallingToLanding -> state-machine exit (m_DstState0 appears null in the report). All7 elemental FallingToLanding clips emit respawn at0.1, preceded by weak0.05(float2), Stunned0.06666667 and stopGo0.083333336. Fire example later reducesCollider0.13333334 and recoversCollider1.8333334; complete event lists for every clip remain in report. FallingToDeath emits recoverCollider0.033333335, weak0.05(float2), Stunned0.06666667 and stopGo0.083333336; no respawn event.

Only AirAnimatorController directly embeds the Death.cs GUID (10ed01e7033004e73eabfa9e30c99d24) in this bounded seven-controller search: Falling uses serialized death1, and another behavior elsewhere uses death0. Other controllers have different/shared state behaviors; restoring this class alone does not establish their flags are equivalent. No state or clip assets were edited.

A bounded ISIL named-call search found ScoreController.CheckPoints calls in ScoreController.OnTriggerEnter and Score.CheckSpectatorScore only. Animation clips in this audit do not directly call CheckPoints. This is evidence of the static search boundary, not a full native xref proof. Nonfinal round producer is DeathZone collision -> ScoreController.AddPoints/CheckPoints -> Falling transition -> FallingToLanding animation event -> Respawn.respawn. Final round producer uses native ScoreController.Death/FinalDeath -> Death animation and Invoke DelayWin1s -> FoeWin -> results.

Potential adjacent issue sent to parent: weak animation event carries float2 while current DamageOnCollision.weak has no parameters; verify Unity dispatch behavior/signature with its owner before changing it. reduceCollider/recoverCollider also have handlers in both ColliderController (restored) and AnimatorEventsController (currently stub). These are separate ownership/integration checks, not modifications made by this worker.

Validation: YAML audit executed successfully38controllers/9clips/7respawn event clips. No Unity compilation or runtime was launched by this worker. Root must compile and exercise nonfinal respawn, final3points/results and Survival replacement identity/cooldown; native source tracing does not establish these runtime outcomes. No RAG rebuild was attempted concurrently with other agents' corpus writes; parent rebuilds at checkpoint.
