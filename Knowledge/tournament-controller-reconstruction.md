# Tournament state owner reconstruction

TournamentController.cs restored from Evidence/native-disassembly-TournamentController.txt, native field map and exact call/literal resolution. No Unity runtime/compilation performed by this worker. TournamentManager and bracket progression remain stubs; this checkpoint does NOT establish playable tournaments. Parent has been notified that payment currently routes into that incomplete flow.

Native Start0x12B81B0 establishes singleton, logs singleton presence and destroys duplicate component, then calls Reset. Reconstruction omits diagnostic print, returns after destroying duplicate to avoid resetting its state, and OnDestroy clears only the owned singleton (native OnDestroy RET). Reset0x12B82D8 clears/reuses checkpoints and losers, inserts nine0/ninefalse, round0,battle1,stage7,loseTournamentfalse,characters.Clear. times and tourneyLevel remain untouched. Null-list initialization is a working-export robustness choice, not native behavior.

PayTournament0x12B84A4 compares tmPlayer.crystals with suppliedfee; insufficient displays "You need more crystals" for5s type"alert" and returns. Success spends exactfee via CrystalsNotification.SpendCrystals, Reset, GoogleAdsManager.skipAd=true, FadeToLevel4, SystemManager.modeTournament/RefreshPlayingMode, Notifications.Remove, increments PlayerPrefsTourneyTimes. Strings slots0x28177A8/77B0/4C68 resolved. No ad/backend request is issued; absentGoogleAdsManager is tolerated in offline scope. No fee inferred from name/video; caller supplies fee. No reward amount or tournament progression invented.

NormalMode GUID had no nonmeta/noncs references in bounded working Assets search, so it was not edited. Current battle composition owns controls in GameController.

Anticipated round issue sent to parent: Falling.anim weak events at0.016666668float2 and0.15float0; DamageOnCollision weak currently parameterlesssetsWeaktrue. Verify dispatch/clearing with owner. Falling clip also resetsdamage0.1,ShieldOff0.11666667,DropAllAtks0.13333334. These are source-derived asset facts, not a reproduced failure.
