# Offline scoring and results reconstruction

This checkpoint replaces the ScoreController and ResultsController recovery stubs with readable C# in the working UnityProject. The original export remains immutable. Original working files are backed up in Evidence/WorkingBackups/scoring-2026-10-16. Existing serialized fields, public signatures and .meta GUIDs are preserved. Source-derived reconstruction is not original source recovery or proof of runtime parity.

## Evidence and retrieval

RAG context query `ScoreController ResultsController rewards rank survival finish --limit 4` retrieved score-reconstruction.md, game-spec.md, ResultsController symbols and video-findings.json/cpu-rewards. The video observation (3-2 rank B and eight crystals) is corroboration, not the reward formula authority. The formula comes from corrected ARM64.

Native binary SHA256: d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c. Address/field authority: Evidence/cpp2il-native-address-map.json. Correct mapped disassemblies: Evidence/native-disassembly-ScoreController.txt, native-disassembly-ResultsController.txt, native-disassembly-ScoreController__DropAtks_d__21.txt and native-disassembly-ScoreController__UpdateScore_d__7.txt. Cpp2IL ISIL is used only for call hints; its printed Disassembly block is invalid for this ELF. Two nested state machines were disassembled with Tools/disassemble-type.ps1 using their exact mapped names. Tools/resolve-native-usage.cjs and Tools/resolve-native-generic.cjs resolved strings and concrete GetComponent/GetComponents types. Numeric table at VA0x652CA0 was read via ELF segment mapping: float32 [1.5,2,2.5,3]. No game runtime or backend was launched by this worker.

## Scoring and lifecycle reconstructed from native

- Start 0x12A402C binds Score.score, Animator and PhotonView.
- OnTriggerEnter 0x12A40CC: FinalZone when battle unfinished places this fighter at world (7,5,0). DeathZone excludes the exact object name DistanceCollider and in offline calls AddPoints then CheckPoints. Native has no battleFinish gate on DeathZone; reconstruction retains that condition structure.
- AddPoints 0x12A4384 belongs to the fighter who fell: tag GuestPlayer increments playerPoints, otherwise enemyPoints; updates Score display. Native Photon property transport is excluded from offline scope.
- CheckPoints 0x12A4540 normal: 3-3 draws, player score3 triggers Death on GuestPlayer, enemy score3 triggers Death on HostPlayer, otherwise Falling; both >=4 returns. Survival: enemyPoints>=1 ends run presentation, sets newRecord and updates survivalRecord if playerPoints exceeds saved value, destroys Guest, invokes DanceSurvival/finishMenu after1s, then Falling. Native saves record through Firebase; reconstructed offline save uses existing SystemManager.SaveData("survivalRecord"). This is an explicit local replacement and never contacts Firebase. CheckPoints itself does not write battleFinish for the survival branch, matching native; callers and animation lifecycle must establish its timing.
- Falling 0x12A5A54 sets Animator trigger Falling. Death 0x12A5B8C marks battleFinish then FinalDeath (0x12A6284), which triggers Death and invokes DelayWin after1s. DelayWin 0x12A6300 disables BoxCollider and MobileInputs then FoeWin. BoxCollider is resolved from generic slot0x2814430; it is not CapsuleCollider.
- FoeWin 0x12A5CDC disables the single-player AI toggle, disables losing rigidbody gravity, picks the other fighter by HostPlayer tag, disables winner movement, shows Winner!, changes camera/music, hides special, dances and opens results after2.5s (immediately if draw).
- TimeOver 0x12A5544: compare points first, then StatsController.GetPlayerLifeCrystals(false/true) on tie, then Draw on a complete tie. EndBattle 0x12A5704 resolves winner/loser, disables both movement, winner presentation and results after1s.
- Draw 0x12A4AA4 sets GameController.draw and battleFinish, uses far scene camera, disables both movement, dances both, changes music, shows DRAW and invokes finishMenu after1s.
- finishMenu 0x12A5F04 starts Score.TimerBattleFinish; increments RateGame unless exactly100; hides SpecialButton if present; offline calls ResultsController.InitResults. Online finish/rematch transport remains unavailable.
- disableMovement 0x12A4C98 rebinds Animator and sets Stunned/Weak, disables AnimationMovement/DesktopInputs/MobileInputs/LookAt, stops enabled AI and clears its on flag, schedules DropAtks after1s. DropAtks MoveNext0x12A63A0 waits first, then calls DamageOnCollision.DropAllAtks. UpdateScore MoveNext0x12A64BC also waits first, then refreshes display (not immediately).
- changeMusic 0x12A53D8 uses StageController.i.music.GetComponents<AudioSource>(): disable[0], enable[1] unless draw. This reconstruction permits absent stage presentation while StageController is restored separately; it does not invent replacement music.
- Native Dance 0x12A4EEC switches exact clone names: Rina/Agata -> dance3; Dante/Aura/Maya -> dance1; Ren -> dance6; Amon -> dance8; Eliot -> dance4; Diana -> dance9; Sora -> dance2; Vinea -> dance7; Raven -> Sitting. Unknown names cause no dance, as in native. String states include literal spaces, e.g. "dance 3".
- showWinnerText 0x12A587C displays Winner! for normal mode, New Record! only when survival newRecord is true. Its character argument is not consulted by native.

## Exact rewards and ranks

ResultsController.InitResults0x12A6110 logs RESULTS, sets its Animator bool In, calculates reward, awards via CrystalsNotification.WinCrystals and considers the ad offer. CalculateCrystalReward0x12A7F24:

- Survival base reward = playerPoints*2. Other modes = playerPoints-enemyPoints+3. practiceMode sets reward0.
- Rank is calculated from base reward BEFORE CPU difficulty multiplication. Base0..6 -> F,E,D,C,B,A,S. Practice base0 displays "-" and logs the native disabled-crystals message.
- SinglePlayer CpuMode1/2/3/4 multiplies by1.5/2/2.5/3 respectively and rounds UP using ceil. Other CpuMode values leave base unchanged. Example: 3-2 => base4 => B, CpuMode2 =>8. A corresponds base5 regardless of multiplier.
- Survival rank displays playerPoints. If BASE reward >20 (thus score>10), an additional20 crystals is awarded independently and a10s success notice uses literal "Congrats your score is over 10!   You got 20 extra Crystals". Score10 gets no extra; score11 gets22+20. Extra20 is not folded into displayed base reward.
- Tournament bookkeeping is translated from the native branch: <=2 sets loseTournament, >2 clears it, base3 compares life crystals; strict differences return before ordinary rank/display, tie sets draw. The surrounding tournament remains unreconstructed and is not claimed playable.

## Explicit design corrections and limits

Repeated result callbacks are guarded once per ScoreController and ResultsController scene instance so they cannot start duplicate reward transactions or restart timers. These guards are new reconstruction behavior. Native disableMovement zeroes quaternion x/z/w while retaining y; reconstruction instead assigns a valid Quaternion.Euler(0, controller yaw,0). No original-device reproduction establishes whether the native quaternion issue is visible. Null-aware winner attack-drop tolerates a fighter removed before its delayed coroutine; singleton cleanup only clears this ResultsController owner.

Offline DisplayBonus always hides bonus and enables Play Again. Native uses a loaded ad and reward>=3; no fabricated ad completion or bonus currency is provided. ShowRewardedAd emits an unavailable warning. Notifications.Log remains a separate stub in the overall project; this worker falls back to Debug.Log only when its singleton is absent. A loaded stub singleton still cannot render the notification until that subsystem is restored.

Score.cs compile bug corrected: HostPlayer.hostPlayer and GuestPlayer.guestPlayer are the real singleton fields; the previous nonexistent .i references were removed. No other Score timer semantics changed.

## Validation boundary

Static branch/constant/type tracing was performed against corrected ARM64 and metadata. No Unity process was launched by this worker. Root owns compilation and runtime validation; results are reported separately. Verify death/fall animation events, respawn on nonfinal point, full three-point victory, timeout score and crystal ties, survival gameover battleFinish and record save, reward persistence/reload, duplicate result callbacks, all dance states, camera transition, and offline menu restart. No claim of complete offline gameplay or tournament coverage follows from these scripts alone.
