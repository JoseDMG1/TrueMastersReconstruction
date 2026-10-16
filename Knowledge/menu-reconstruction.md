# Offline menu reconstruction

Status: static reconstruction in progress. No PlayMode run, network connection,
purchase, entitlement request or gameplay reproduction is established here.
The complete native evidence remains in `Evidence/native-disassembly-*.txt`.
These dumps use `cpp2il-native-address-map.json` and the correctly mapped ARM64
library, unlike Cpp2IL's INVALID textual disassembly.

## Recovered facts

Main-menu SinglePlayerMode and SurvivalMode load build index 3 (tutorial), set
GameMode 0 and 1 respectively. LevelChanger skips tutorial index 3 to selection
index 4 after howToPlayShowed is set. HowToPlayManager.Continue loads 4
(0x123C508). LevelChanger.FadeToLevel (0x122B964) drives FadeOut, waits one second,
then LoadingScreen loads asynchronously and drives FadeIn. Its delayed overload
defaults to 1.7 seconds. LoadingScreen preserves progress smoothing and its
offline coroutine; advertising is omitted explicitly.

SystemManager.Awake (0x12B6CAC) restores a player, boot-loads the next scene after
three seconds, selects StartMenu, disables vsync and targets 60 FPS. TMPlayer
constructor (0x12B73E4) initializes empty TMPlayerCharacters and TMPlayerStages:
it assigns no currency, ownership, premium or username defaults. Parameterized
ownership constructors are direct field assignments (0x12B80D4, 0x12B813C).

CharactersManager.SetCharacters (0x121ECBC) orders Sora, Vinea, Diana, Amon,
Aura, Eliot, Ren, Rina, Dante, Agata, Raven, Maya, ?. Filtering (0x121F300)
removes each of the eight paid characters unless its ownership integer is 1;
premium 2 or 3 bypasses filtering. The first four are free. '?' chooses from
playable characters excluding '?' and the last random result (0x121F648).
IsCharUnlocked's implemented branch is the offline list-membership branch;
the original additional authenticated premium branch is not connected.

SelectionController.SetDefaultCharacters (0x12ABFE8) uses PlayerPrefs Character
= Sora and CharacterCPU = Amon only when Character has no key. CPUController
(0x1283198..0x1283578) uses the full character catalog, clamps next/previous at
the ends, stores CharacterCPU, and displays Amon as Blaze and Sora as Siro.
SelectionController stores the original identifiers while applying the same
display aliases. This resolves the video's Blaze naming discrepancy statically.
The camera view order in Show (0x12802D0) is Rina, Amon, Diana, Ren, Vinea,
Eliot, Aura, Sora, Dante, Agata, Raven, Maya, ?. Random toggles randomBanner.
LateUpdate interpolates position and Euler angles using deltaTime * speed
(default speed 2, constructor 0x1280B98).

TimeOption defaults to 0, cycles 0..3, and sets Timer to [300, 420, 540, 180]
seconds. These are the actual Int32 words at native VA 0x652BC0 referenced by
SetTime (0x12ABF7C), independent of the serialized display strings. GameSpeed
(0x1236BA4..0x1236E64) restores PlayerPrefs GameSpeed default 1, selects the
serialized label and wraps changes at the speed-list boundaries.

StagesManager.Start (0x12C21AC) creates indices 0..7. Filtering (0x12C2638)
removes 1..6 according to stage1..stage6 ownership unless premium 2/3; 0 is
free and 7 is random. AddFreeStages is an actual native no-op (0x12C2634).
RemoveRandom (0x12C2930) excludes 7 and the last stage when multiple choices
exist. StageSelector uses Images/Stages/{index} Resources sprites, stops at
carousel ends, and shows lock UI for unavailable stages. StageSelectorButton
uses Slide animation and restores the previous index on cancel.

Offline SelectionController.start (0x12AD7C4) requires an unlocked selection,
invokes onStart, removes arrows, triggers Cheer on the selected preview,
stores resolved Stage, and calls GoToBattle (0x12AD43C) to load build 5 after
1.7 seconds. ready and inBattle prevent repeated entry. Offline selection
does not implement tournament or local multiplayer entry in this path.

Selection prices are serialized fields with constructor defaults trophyRequirement
= 5 and crystalsPrice = 200 (native words 0x652208). Dante, Agata, Raven and Maya
use trophy price UI and debit trophies; other paid characters debit crystals.
Debits are established by CrystalsNotification.SpendCrystals (0x12253A8) and
SpendTrophies (0x1225518). Unlock (0x12ADFE0) maps the lowercase original name to
the ownership field, sets 1, refreshes playable characters, animates and saves.
No initial balance or entitlement is fabricated.

## Source method coverage

Implemented offline bodies: DontDestroyOnLoad.Start; CPUController's four
methods; CharactersManager's six methods; StagesManager's eight methods
(including native-empty AddFreeStages); StageSelector's six methods;
StageSelectorButton's six methods; GameSpeed's three methods; SurvivalRecord.Start;
SurvivalView.Start; CameraController's three methods; SelectionController
Awake/ChangeTime/SetTime/Start/OnDestroy/RefreshCharsAndStages/SetDefaultCharacters/
nextCharacter/previousCharacter/ChangeCharacter/RemoveButtons/start/GoToBattle/
CheckIfLocked/UnlockCharacter/UnlockTrophyCharacter/Unlock/UnlockAnim.
TMPlayer's constructor and both ownership constructor overloads are implemented.
SystemManager's offline save/load and elapsed-time bodies replace prior stubs.

Previously recovered and retained: StarMenuManager.Start/ShowVersion/
ShowMultiplayerUI/GoToScene/SurvivalMode/SinglePlayerMode/GoToLocalMultiplayer;
LevelChanger offline navigation/fades; LoadingScreen offline coroutine;
HowToPlayManager.Continue; GameModeVisibility.Awake. Unity-empty Update methods
in DontDestroyOnLoad, SurvivalRecord and SurvivalView correspond to native ret.

## Deliberate adaptations and outstanding work

Original save I/O is BinaryFormatter at Application.persistentDataPath/player.save
(0x12B715C, 0x12B726C). Reconstruction uses a separate
reconstruction-player.json, writes a temporary file and retains .bak on replace.
It does not deserialize or modify the original binary save. Invalid JSON or
incomplete ownership records raise an error preserving the evidence. This is a
maintainable offline format adaptation, not original save compatibility.
SaveData preserves the local update and omits the original authenticated cloud
dispatch. Presence, account cleanup, updates and online readiness are not called.

Selection callbacks SetOnlineCode, ResetPlayerProperties and OnlineMode explicitly
report unavailable online behavior. Main-menu tournament/ad/premium/network
callbacks remain unavailable. Original localized notifications, currency UI refresh,
online stage-selector warnings and post-unlock offline warning are not recovered.
StarMenuManager.AllCharactersUnlocked still explicitly throws for unrecovered
premium policy. Local multiplayer, tournament, online session, server and advertisement
coverage remain open. Source control flow is not evidence of navigability in Unity:
compilation, retained scene references, animation state compatibility and PlayMode
behavior require separate verification.

## Currency notifications and finish callbacks

CrystalsNotification's singleton (0x1224F88), Hide (0x1225370), WinCrystals
(0x1225190), SpendCrystals (0x12253A8), WinTrophies (0x1225460) and SpendTrophies
(0x1225518) now implement exact add/subtract, local save and display refresh.
CrystalManager.UpdateCrystals (0x1283D24) and
TrophiesManager.UpdateTrophies (0x12CF6D8) display the stored float using ToString.
Their Start/OnDestroy singleton lifecycles are restored. Selection debits now
call this currency owner. ShowBonusSmall/ShowBonus activate their existing
notification globe, show the recovered literal label, credit the supplied
amount and hide after five seconds; these callbacks do not request ads.
FinishMenu.Start and Update are native no-ops. exitGame (0x128B644) quits.
reboot (0x128BD48) invokes GameController.Restart, adapting away the original
RateGame popup gate for the offline reconstruction. Online finish callbacks
remain outside this recovery. Currency display refresh mentioned as outstanding
above is superseded by these implementations.
