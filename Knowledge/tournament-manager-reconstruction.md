# Tournament bracket and presentation reconstruction

Checkpoint: TournamentManager, PlayerCheckpointsProgress, BattleCheckpoint, CharactersTournament, ZoomCanvas, TourneyLevel and HideOnTourney now contain reconstructed implementations. Their original .meta files were retained. TournamentController was restored earlier. This worker did not launch Unity; compilation and runtime acceptance remain the principal agent's responsibility.

## Sources and verified boundaries

Source native library: Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so SHA-256 d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c. Native reasoning uses corrected Capstone ARM64, not the invalid Cpp2IL Disassembly section. Address and field mapping: Evidence/cpp2il-native-address-map.json.

- Evidence/native-disassembly-TournamentManager.txt: Start0x12CBDD0; persistence/progression0x12CC094..0x12CC8C8; transition0x12CCE8C..0x12CD2C0.
- Evidence/native-disassembly-TournamentManager__NextTurn_d__28.txt: actual iterator0x12CD3F4..0x12CD66C.
- Evidence/native-disassembly-PlayerCheckpointsProgress.txt: positioning/motion/progression/loser/opponent0x1250F9C..0x1251494.
- Evidence/native-disassembly-CharactersTournament.txt: initialization and icon ownership0x121F85C..0x1220418.
- Evidence/native-disassembly-ZoomCanvas.txt, TourneyLevel.txt, HideOnTourney.txt, BattleCheckpoint.txt: exact corresponding producer bodies.
- Evidence/tournament-native-generic-resolution.json resolves RectTransform, BattleCheckpoint, Image, IncreaseSize and RotateSelf component ownership.
- Evidence/tournament-native-literal-resolution.json resolves roster order, callback names, CharacterCPU and Level prefix.

## Established static behavior

Tournament scene contains eight players in ID order1..8; persistent checkpoints/losers lists have nine slots with slot0 unused. TournamentManager initializes icons, music, checkpoints, positions, opponents, then losers. Brand-new round0 advances round1. Re-entry restores its previous round, displays VS, schedules Continue2s and removal1.5s; Continue advances once immediately and again1.5s later. Only rounds1/3/5 activate VS and schedule StartBattle. Round6 celebrates; round7 returns without advancing any checkpoint.

SetLosers colors previously lost players, then resolves each non-null opponent. Player1 loses only when loseTournament is true; other pairs use Random.Range(0,2). Importantly, scene opponent links are one-sided. A naive symmetric bracket would produce incorrect double elimination; retain recovered links. Player1's actual win/loss is owned by ResultsController. No outcomes are invented by this manager.

NextTurn skips losers, then yields waits0,0,1,0,1,... before advancing each surviving player. It persists each checkpoint immediately, refreshes opponents after enumeration, and invokes ActivateVS after1s. MoveSpeed is a per-FixedUpdate Lerp parameter, default.01 but serialized scene.05. Losing sets the first child Image to RGB100/255 alpha1 and writes its persistent loser flag. BattleCheckpoint's native Start is RET; occupancy is assigned by PlayerCheckpointsProgress.

CharactersTournament appends a twelve-name roster, Fisher-Yates shuffles, resolves the selected character through CharactersManager.RemoveRandom, removes that name and inserts it at index0. It persists a new copy and restores that exact order on return. Only eight icons are parented to eight slots. Image is enabled and RectTransform.SetParent uses worldPositionStays=false, matching the native bool argument. Native fresh scene serializes characters=[]; no extra clearing or random roster policy introduced.

StartBattle reads player0's linked opponent name into CharacterCPU, zooms1.5s later and loads arena3s later. The stage sentinel7 resolves through StagesManager.RemoveRandom once and persists for subsequent matches. The battle counter increments after requesting arena scene5. Zoom selects its serialized view by battle counter and disables player motion components. Final sun rotation writes RotateSelf.z (field0x28), not x; values.2 then.5. RotateSun cancels pending invokes, zooms in1s and shows stars1.5s. Music owner switches at stored rounds3/5. PlayAgain spends the exact native fee50 via TournamentController.PayTournament. TourneyLevel exposes levels1..3; HideOnTourney destroys its own GameObject when Tournament mode is active.

## Static validation executed

`node Tools/validate-tournament-bracket.mjs` parsed the actual scene with the existing failsafe Unity YAML parser. Evidence/tournament-bracket-static-validation.json retains scene hash, all checkpoint/opponent paths and eight modeled coin combinations. Every modeled winning path eliminates7 players and retains player1; first pairs1-2/3-4/5-6/7-8, then two semifinal pairs, then final. This is a model over source-derived state; it is not a Unity playthrough and does not establish menu animation timing or battle execution.

## Reconstruction choices and pending work

Singleton duplicate components return after destruction and OnDestroy clears only the owned singleton, as in the project's existing reconstruction pattern. Native logs are omitted. RET lifecycle methods and generated stub iterator wrappers are removed in favor of the actual C# coroutine. No network/ad calls were added.

TournamentManager.alert is serialized fileID0 in this scene. QuitAlert/CancelAlert still implement their native required-reference contracts, but direct invocation would fail without wiring. The scene includes named QuitAlert GameObjects; no automatic choice between them was made. Inspect actual UI ownership/callbacks before assigning any reference.

Required runtime acceptance: start from paid menu/character selection, bracket-to-arena, win then restore bracket, loss elimination/menu, all three battles, final zoom/stars/music, replay fee and return. Verify initial singleton ordering and scene indexes via the configured build settings. Parent must compile current checkpoint and run its probes; this document does not claim success. Final tournament trophy/prize presentation may be owned by serialized stars children/other scripts and has not been independently audited here.

## Runtime checkpoint added after principal verification

Principal's Development Windows player probe completed with result=passed, errors=[] in Evidence/offline-tournament-probe-20261015-172112.json (Unity6000.6.5f1, Direct3D11). Full log uses the same basename. This report was read after completion; an earlier partial log observation had briefly suggested no transition, but the final retained observations establish progress without a tournament-code change.

Observed sequence: original boot -> StartMenu -> temporary ownership/currency fixture -> actual TourneyAlert/EnterTournament callbacks -> Character Selection. Entry debited50 crystals and set Tournament mode. Actual start callback -> Tournament -> arena at approximately21.9s. Three matches were won by probe-controlled physics falls through actual DeathZone colliders, producing3-0 each; GameController.Restart returned to the bracket and next arena. Recorded CPUs Agata, Diana, Vinea; persistent battle counter2/3/4 after respective transitions. Final Tournament observed round6, seven loser flags, player1 loser=false at73.5s. Original reconstruction save bytes were restored at76.5s. Champion screenshot: Evidence/offline-tournament-probe-20261015-172112.champion.png. The worker did not execute this player; these are observations in the principal's retained evidence.

Coverage boundary: successful paid entry and full three-match winning bracket state progression with synthetic physics-fall wins. Natural AI combat, losing path, replay debit and visual/audio fidelity remain unverified. No game code was changed following this runtime report.

## Final trophy owner restored after the first winning-path probe

StarsFX and its direct producer TransladeObject had remained exported stubs. Both are now reconstructed, preserving their .meta files. Native corrected evidence: Evidence/native-disassembly-StarsFX.txt (0x12C344C..0x12C36E4), native-disassembly-TransladeObject.txt and native-disassembly-TransladeObject__MoveToPosition_d__2.txt (iterator0x12CE8E8..0x12CEA5C). Exact literal/generic resolutions retained in Evidence/starsfx-native-{literal,generic}-resolution.json.

StarsFX.Start schedules StartMoving after a constant3s. The serialized DelayMove field is ignored by the native body (Tournament scene stores5, native constructor default3). StartMoving calls its own IncreaseSize.Decrease(), starts TransladeObject.MoveToPosition on its own Transform toward endPosition.position using serialized duration (Tournament stores7, constructor default5), and schedules WinTrophy after2s. Thus the reward is due about5s after stars activation; a champion screenshot after3s is too early to establish a missing reward even with restored code.

WinTrophy adds exactly TournamentController.tourneyLevel to SystemManager.tmPlayer.trophies, refreshes TrophiesManager, persists with SaveData("trophies"), writes PlayerPrefs TourneyTimes=0 and activates the assigned menu. It does not call CrystalsNotification.WinTrophies and does not invent a fixed reward. Levels1/2/3 therefore produce1/2/3 trophies, respectively. The native method has no explicit once flag; scheduled Start occurs once per instantiated StarsFX lifetime. No speculative persistent transaction scheme added.

TransladeObject exists in the persistent boot scene. Its Start singleton is restored with the project's existing owned-singleton cleanup convention. The movement iterator starts t=0, adds Time.deltaTime/duration each iteration, interpolates from the CURRENT position toward the supplied endpoint with Mathf.SmoothStep(0,.5,t), yields WaitForSeconds(.02), then continues while t<=1. This unusual smoothstep range and current-position interpolation are direct ARM64 arithmetic, not a newly chosen tween. No final teleport or extra deltaTime normalization introduced.

This restoration has not yet been compiled or executed by this worker. The first successful tournament probe predates it; parent is extending the runtime probe to wait for and assert exact trophy delta, reset TourneyTimes and menu activation before restoring fixture save bytes. The prior probe remains evidence of bracket progression, not prize delivery.
