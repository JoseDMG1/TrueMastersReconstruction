# Auditoría estática de escenas y menús Unity

Fuente: `Evidence/unity-menu-audit.json`. Esta inspección lee YAML exportado, sin abrir Unity ni ejecutar scripts.

Cobertura: 12 escenas, 6 prefabs examinados, 6 prefabs candidatos de UI con detalle retenido; 10177/10177 documentos parseados. 0 errores YAML y 0 errores meta. 0 referencias de script sin asociación única.

Se encontraron 570 textos UI serializados y 247 llamadas persistentes. La falta de estos campos no demuestra que el juego carezca de ellos.

## Assets/Scenes/Bending Battle.unity

GUID: `1fc6e50399903004fb4294df61e954cc`. 415 GameObjects, 546 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas/OpenChat` [ID 14]; `Canvas/SettingsOptions/QualitySlider` [ID 23]; `Canvas/PlayerStats` [ID 27]; `Canvas/Room` [ID 28]; `Canvas/SettingsOptions/Music` [ID 30]; `Canvas/SettingsOptions` [ID 31]; `Canvas/Room/ShareRoom` [ID 32]; `Canvas/SettingsOptions/DpadToggle` [ID 37]; `Canvas/FinishMenu/SendFriendRequest` [ID 61]; `Canvas/SettingsOptions/JoystickToggle` [ID 68]; `Canvas/SettingsOptions/Quality` [ID 94]; `Canvas/Score/LifePlayerPos` [ID 107]; `Canvas/Settings` [ID 132]; `Canvas` [ID 142]; `Canvas/SettingsOptions/ControllerToggle` [ID 148]; `Canvas/SettingsOptions/VoiceChat` [ID 150]; `Canvas/Results/Bonus/PlayAgain (2)` [ID 157]; `Canvas/Room/RoomName` [ID 166]; `Canvas/Results/PlayAgain (1)` [ID 195]; `Canvas/SettingsOptions/Options` [ID 210]; `Canvas/VoiceChat` [ID 212]; `Canvas/FinishMenu/PlayAgain_LM` [ID 215]; `Canvas/ExitMenu` [ID 226]; `Canvas/Score/LifePlayerPos/LifePlayer` [ID 244]; `Canvas/Resume/Canvas HTP/Content/Defences/Jump R/Button` [ID 251]; `Canvas/Resume/Canvas HTP/Content/Defences/Roll/Button` [ID 261]; `Canvas/Resume/Canvas HTP/Content/Defences/Jump L/Button b` [ID 286]; `Canvas/Resume/Canvas HTP/Content/Attacks/Firewall/Button` [ID 300]; `Canvas/ExitMenu/PlayAgain` [ID 306]; `Canvas/Resume/Canvas HTP/Content/Attacks/Fireball/Button` [ID 307]; `Canvas/Resume/Canvas HTP/Content/Attacks/Fireball R/Button` [ID 317]; `Canvas/Resume/Canvas HTP/Content/Controls/Attack/Button (1)` [ID 328]; `Canvas/Spectators/Scroll/Panel` [ID 334]; `Canvas/Resume/Canvas HTP/Content/Controls/Attack/Button` [ID 341]; `Canvas/Resume/Canvas HTP/Content/Controls/Defend/Button` [ID 343]; `Canvas/Resume/Canvas HTP/Content/Defences/Block/Button` [ID 344]; `Counter/Canvas` [ID 345]; `Canvas/Resume/Canvas HTP/Content/Defences/BackFlip/Button` [ID 357]; `Canvas/ExitMenu/MainMenu` [ID 364]; `Canvas/Resume/Canvas HTP/Content/Title/HowToPlay (1)` [ID 371]; `Canvas/Resume/Canvas HTP` [ID 378]; `Canvas/Resume/Canvas HTP/Content/Attacks/Fireball L/Button` [ID 380]; `Canvas/FinishMenu/PlayAgain` [ID 382]; `Canvas/FinishMenu/OpenChat` [ID 384]; `Canvas/FinishMenu` [ID 386]; `Canvas/Chat` [ID 393]; `Canvas/Chat/Content/ChatText` [ID 399]; `Canvas/Resume/Canvas HTP/Content/Attacks/Firewave/Button` [ID 412].

Textos serializados: "play again"; "ll"; "Winner"; "results"; "CPU"; "10"; "7"; "0"; "X"; "-"; "rank"; "player 2"; "1214-uscc"; "PLAYER 1"; "5:00"; " main menu"; "resume game"; "250"; "3"; "Play again\n"; "Disable Mic"; "Music"; "!"; "mute "; "Inv"; "rewarded!"; "bonus"; "controller"; "Send Friend request"; "Waiting for your opponent..."; "Fps"; "Waiting ..."; "mute Opponent"; "ping"; "6"; "lv 2"; "RES"; "x"; "Dpad"; "LOCK 30 fps"; "30"; "Rematch?"; "joystick"; "a"; "/"; "b"; "Attack"; "Right Atk"; "watch ad"; "B"; "cONTINUE"; "A"; "exit game"; "Up atk"; "move"; "Block"; "backflip"; "basic atk (LV1)     down atk (LV2)     up atk (LV3)"; "Left ATK"; "jump r"; "Play Again"; "jump l"; "MED"; "min"; "max"; "Down ATK"; "HOW TO PLAY"; "defense"; "Basic ATK"; "open chat"; "¡Thanks for playing!"; "Enter message..."; "SEND"; "Your opponent is typing..."; "CHAT"; "roll". Métodos UnityEvent: ToogleAI, CapFrameRate, OpenChat, changeQuality, pauseGame, ChangeSpectatorView, ShareCode, Music, ActivateDpad, DpadOn, ResumeGame, Special, SetSButton, SendFriendRequest, UpDown, Opacity100, UpDownDpad, Opacity0, Mute, JoystickOn, DisableMic, Attacking, RightLeft, LeftRightDpad, Invert, Rematch, OpenSettings, CloseSettings, ControllerOn, reboot, AttackingButton, SetBButton, Jumping, ToggleSpectatorScroll, OpenExitMenu, MuteOpponent, SwitchCamera, ToggleVoiceChat, ShowRewardedAd, MainMenu, ExitGame, OnButtonPress, SetAButton, PlayAgain, setOriginalRes, setMinRes, setMedRes, resumeGame, SendInvitation, CloseChat, SendText, Typing, CloseMenu.

## Assets/Scenes/Character Selection.unity

GUID: `9ecea07b3fd82484a95f9e6e67bbf5a6`. 1471 GameObjects, 290 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `SelectionController` [ID 2]; `Canvas/SelectStage` [ID 10]; `Canvas/LeftBar/SurvivalRecord` [ID 16]; `Canvas` [ID 25]; `Canvas/SelectStage/Select` [ID 36]; `Canvas/LeftBar/VoiceChat` [ID 37]; `Canvas/SelectStage/CloseButton (1)` [ID 43]; `Music` [ID 49]; `Canvas/ShopButton` [ID 59]; `Canvas/LeftBar/CPUController/CharacterName (1)` [ID 70]; `Canvas/ShopButton/ShopButton (1)` [ID 71]; `Canvas/LeftBar/StageSelector/StageButton` [ID 86]; `Canvas/LeftBar/Button` [ID 91]; `Canvas/LeftBar/StageSelector` [ID 94]; `Canvas/LeftBar/CharacterName` [ID 106]; `Canvas/IAPManager/BG/CloseButton` [ID 107]; `Canvas/IAPManager/BG/BuyOption100` [ID 273]; `Canvas/GoBack/QuitButton` [ID 326]; `Canvas/IAPManager/BG/BuyOption500` [ID 668]; `Canvas/IAPManager/BG/BuyOption250` [ID 669]; `Canvas/IAPManager/BG/BuyOption500/BuyButton` [ID 840]; `Canvas/IAPManager/BG/BuyOption250/BuyButton` [ID 841]; `Canvas/IAPManager/BG/BuyOption100/BuyButton` [ID 1446].

Textos serializados: "Off"; "0"; "Lv 5"; "Voice Chat"; "On"; "5"; "select stage"; "5:00"; "NORMAL"; "Choose your character"; "300"; "DANTE"; "\n\nComing soon\n"; "unlock stage"; "?"; "High score"; "Unlock\n"; "This character \nisn’t available \nfor this mode yet\n\n"; "250"; "RINA"; "200"; "Random"; "blaze\n"; "start"; "select"; "close"; "QUIT "; "quit"; "Are you sure you want to leave?"; "cancel"; "shop"; "▶"; "+"; "uninstalling it will cause you to lose your progress"; "➤"; "extra"; "1"; "Level up!"; "10"; "Maya"; "unlock"; "Important!  this game saves locally"; "coming soon"; "Crystals"; "Game speed"; "CPU"; "buy"; "100"; "$2.99"; "$1.99"; "$0.99"; "500"; "Level". Métodos UnityEvent: previousStage, OpenStageSelector, SelectStage, NextMode, nextStage, CloseStageSelector, previousCharacter, Toggle, BuyCrystals500, ShowOfferwall, UnlockCharacter, PreviousMode, ChangeSpeed, Test, start, nextCharacter, CloseShop, BuyCrystals250, Unlock, HideCrystalsNotification, BuyCrystals100, ChangeTime.

## Assets/Scenes/ConnectingServer.unity

GUID: `01494aa05ebc0a34bbe315c941adfb05`. 65 GameObjects, 103 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas` [ID 11]; `Canvas/ConnectingText` [ID 13]; `RoomManager` [ID 22]; `Canvas` [ID 37]; `Canvas/GoBack/QuitButton` [ID 46]; `Canvas/CodePrivate/JoinRoom` [ID 48]; `Canvas/QuickPlay` [ID 49]; `Canvas/ShareCode/ShareCodeButton` [ID 50]; `Canvas/CreateRoom` [ID 51]; `Canvas/Alert/PlayWithFriends` [ID 52]; `PhotonRoom` [ID 54]; `Music` [ID 59].

Textos serializados: "Enter code..."; " ENTER CODE..."; "​"; "QUIT "; "quit"; "Are you sure you want to leave?"; "Create private match"; "Play with friends"; "quick Play"; "share code"; "Join private match"; "cancel"; "Server -"; "Option A"; "LOADING.."; "\rThis is a new game and it's in development\nYou may have a hard time finding a match\nI recommend you to play with your friends"; "Join a private match". Métodos UnityEvent: JoinRoom, PlayWithFriends, Share, ConnectToRegion, FadeToLevel, CreatePrivate, ShareCode, QuickMatch.

## Assets/Scenes/DesarrolladoPor.unity

GUID: `f6594b49095f1fd43a358c0e7ce78758`. 70 GameObjects, 100 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `GameControllers/CharactersManager` [ID 8]; `LoadingScreen/Tutorial01` [ID 24]; `LoadSaveCanvas` [ID 28]; `GameControllers/TournamentController` [ID 34]; `GameControllers/SurvivalController` [ID 41]; `FriendInvitationPopup/BG/FriendName` [ID 43]; `Notifications/Popup/Buttons` [ID 56]; `Canvas` [ID 58]; `LevelChanger/Canvas` [ID 63]; `FriendInvitationPopup` [ID 65].

Textos serializados: "SOME ATTACKS CAN BE DODGED BY GOING UNDER THEM"; "Accept"; "Has challenged you to battle"; "CANCEL"; "YES"; "Friend"; "OK"; "Pop up text"; "Decline"; "Loading saved data "; "Game still in development"; "BONUS AD WATCHED +25 "; "Please Wait..."; "AD WATCHED +4"; "Vs"; "HOST"; "gUEST"; "Notification". Métodos UnityEvent: ClosePopup, DeclineInvitation, Yes, AcceptInvitation, SaveData, LoadData, Remove.

## Assets/Scenes/FriendsConnection.unity

GUID: `c08ca7b0e2b77f144b90a0e248805cc3`. 41 GameObjects, 57 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `FriendsConnectionController` [ID 6]; `Canvas/BG/RegionMenu` [ID 20]; `Canvas` [ID 22]; `Canvas/GoBack/QuitButton` [ID 32].

Textos serializados: "60"; "Loading..."; "SERVER"; "Option A"; "START GAME"; "QUIT "; "quit"; "Are you sure you want to leave?"; "cancel". Métodos UnityEvent: CreateCode, ConnectToRegion.

## Assets/Scenes/HowToPlay.unity

GUID: `fd0534a6850b67b4888d73047ceada2e`. 117 GameObjects, 155 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `HowToPlayManager` [ID 1]; `Canvas` [ID 3]; `Canvas/Canvas HTP/Content/Defences/Jump R/Button` [ID 17]; `Canvas/Canvas HTP/Content/Defences/Roll/Button` [ID 22]; `Canvas/Canvas HTP/Content/Defences/Jump L/Button b` [ID 43]; `Canvas/Canvas HTP/Content/Attacks/Firewall/Button` [ID 49]; `Canvas/Canvas HTP/Content/Attacks/Fireball/Button` [ID 51]; `Canvas/Canvas HTP/Content/Attacks/Fireball R/Button` [ID 60]; `Canvas/Canvas HTP/Content/Controls/Attack/Button (1)` [ID 68]; `Canvas/Canvas HTP/Content/Controls/Attack/Button` [ID 74]; `Canvas/Canvas HTP/Content/Controls/Defend/Button` [ID 76]; `Canvas/Canvas HTP/Content/Defences/Block/Button` [ID 77]; `Canvas/Canvas HTP/Content/Defences/BackFlip/Button` [ID 84]; `Canvas/Canvas HTP` [ID 95]; `Canvas/Canvas HTP/Content/Title/HowToPlay (1)` [ID 102]; `Canvas/Canvas HTP/Content/Attacks/Fireball L/Button` [ID 106]; `Canvas/Canvas HTP/Content/Attacks/Firewave/Button` [ID 115].

Textos serializados: "CONTINUE"; "/"; "b"; "Attack"; "a"; "Right Atk"; "Up atk"; "move"; "Block"; "backflip"; "basic atk (LV1)     down atk (LV2)     up atk (LV3)"; "Left ATK"; "jump r"; "jump l"; "x"; "Down ATK"; "defense"; "Basic ATK"; "HOW TO PLAY"; "roll". Métodos UnityEvent: Continue.

## Assets/Scenes/LocalMultiplayer.unity

GUID: `ba8ccf10c9c857046bb164c8b5587533`. 357 GameObjects, 473 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `TestCanvas` [ID 3]; `Canvas/Create_UI/Fields/Buttons` [ID 8]; `Canvas (1)/JoinGame_UI/Fields/Buttons` [ID 14]; `Canvas/RoomList_UI/RoomMask` [ID 22]; `Canvas/Create_UI/Fields/RoomName` [ID 26]; `Canvas/EnterPassword_UI/Fields/Buttons` [ID 37]; `Canvas (1)/RoomList_UI/RoomMask/RoomContainer` [ID 41]; `Canvas/Connecting_UI` [ID 50]; `Canvas/RoomList_UI/RoomMask/RoomContainer` [ID 58]; `Canvas (1)/JoinGame_UI/Fields/FriendsIP` [ID 71]; `LM_Connection/Controllers/UDPConnection` [ID 79]; `Canvas/RoomList_UI` [ID 83]; `Canvas (1)` [ID 92]; `LM_Connection/Controllers/TCPConnection` [ID 94]; `LM_Connection/Controllers/UpdatePlayersScene_LM` [ID 101]; `Canvas (1)/SearchRoom_UI` [ID 102]; `Canvas (1)/Connecting_UI/Button` [ID 129]; `LM_Connection/BattleTasks_LM/CreateCharacters` [ID 140]; `Canvas/RoomsNotFound_UI` [ID 146]; `TestCanvas/Test/TestArea/UDP/UDPConnect/UDPConnect` [ID 150]; `Canvas (1)/Connecting_UI` [ID 152]; `Canvas/SearchRoom_UI` [ID 160]; `CreateRoomManager` [ID 167]; `LM_Connection/BattleTasks_LM/MovePlayersInGuest_LM` [ID 176]; `Canvas/RoomList_UI/Buttons` [ID 193]; `Canvas (1)/RoomList_UI` [ID 207]; `JoinRoomManager` [ID 210]; `LM_Connection/CharacterSelection_LM` [ID 214]; `TestCanvas/Test/TestArea/UDP/UDPConnect` [ID 215]; `TestCanvas/Test/TestArea/TCP/TCPConnect` [ID 220]; `LM_Connection/BattleTasks_LM/MenuController_LM` [ID 223]; `Canvas (1)/RoomList_UI/RoomMask` [ID 228]; `LM_Connection/BattleTasks_LM/MovePlayersInHost_LM` [ID 229]; `Canvas (1)/JoinGame_UI/FriendsIP` [ID 231]; `TestCanvas/Test/TestArea/UDP/UDPRestart/UDPConnect` [ID 234]; `LM_Connection` [ID 240]; `TestCanvas/Test/TestArea/UDP/UDPBroad/UDPConnect` [ID 249]; `LM_Connection/Controllers/RoomMessageHandler` [ID 252]; `Canvas (1)/RoomList_UI/Buttons` [ID 266]; `Canvas (1)/EnterPassword_UI/Fields/Buttons` [ID 269]; `Canvas` [ID 271]; `Canvas/GoBack/QuitButton` [ID 304]; `Canvas (1)/GoBack/QuitButton` [ID 305]; `Canvas/Start_UI/Join Room` [ID 311]; `Canvas (1)/Start_UI/Join Room` [ID 313]; `Canvas (1)/Start_UI/CreateRoom` [ID 314]; `Canvas/Start_UI/CreateRoom` [ID 318]; `Canvas (1)/JoinGame_UI/Fields/Buttons/Connect` [ID 330]; `Music` [ID 345].

Textos serializados: "share this code with your friend "; "Test------"; "Port"; "loading..."; "Result"; "TCP conection ..."; "Option A"; "​"; "Enter your friend's code\n"; "Serching for rooms"; "Close"; "Time sync"; "SEND UDP"; "UDPSend"; "my ip"; "Ip..."; "Welcome to Local Multiplayer!\nTo play, make sure both players are connected\n to the same Wi-Fi network\n\nYou might run into some glitches\nas this mode is currently under development\n"; "Select a room\n"; "Enter Room's Name"; "UDP CONECTION ...\n"; "this room is locked"; "wating for opponent"; "TCPSend\n"; "Conenction fail"; "1"; "Enter Password (Optional)"; "UDPConnect"; "PING CONECTION ..."; "Enter Password"; "0.0.0.0"; "Clear"; "No rooms found. Please make sure you’re connected to the same Wi-Fi network as the room creator."; "Enter text..."; "create room"; "Ping"; "7777"; "UDPStart"; "Please make sure all the players are connected to the same Wi-Fi network."; "enter your friend's CODE"; "Port..."; "UDPRestart\n"; "TCP Start"; "TCP Connect\n"; "UDPBroad"; "QUIT "; "quit"; "Are you sure you want to leave?"; "CANCEL"; "Search Again"; "try Again"; "Connect\n"; "join game\n"; "Create room\n"; "search"; "Create Game\n"; "CREATE"; "Enter"; "join room\n"; "cancel"; "5555"; "3333"; "1 player (VS CPU)". Métodos UnityEvent: TCPSend, UDPSend, ConnectionResults, ChangeCurrentActiveObject, StartHostConnection, UDP_BroadCast, ClosePasswordPopup, SendUDPMsg, UDPRestart, Ping, ManualBroadCast, ToggleHide, UDPConnect, CloseConnections, TCPConnect, TCPStart, SendTimerToGuest, Clear, GoToMainMenu, InitSearchRooms, CloseRoomServer, Connect, CloseConnection, ChangePlayerType, ShowStartUI, Close, ShowCreateUI, Create, CheckPassword.

## Assets/Scenes/Login.unity

GUID: `b9b798c9b8aaa664db37bd6d5d4dcbd0`. 165 GameObjects, 214 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas/UserData_UI/Elements/Buttons` [ID 13]; `Canvas/Final_UI/Elements/Buttons/Friends` [ID 17]; `Canvas/Register_UI/Elements/Buttons` [ID 19]; `Canvas/Scoreboard_UI/Elements/Buttons` [ID 36]; `Canvas` [ID 43]; `Canvas/Final_UI/Elements/Buttons/MainMenu` [ID 45]; `Canvas/Login_UI` [ID 69]; `Canvas/MainMenu_Btn` [ID 84]; `Canvas/Login_UI/Elements/Buttons` [ID 92]; `Canvas/Login_UI/Elements/Buttons/Login_Btn` [ID 94]; `Canvas/ResetPass_UI/Elements/Buttons` [ID 123]; `Canvas/Final_UI/Elements/Buttons` [ID 131]; `Canvas/GoBack/QuitButton` [ID 154].

Textos serializados: "USER DATA"; "Kills"; "Your username can not be changed once created"; "Back"; "Save"; "​"; "Sign Out"; "Enter Email..."; "Friends"; "Your saved data is now safe. Soon you'll be able to join leaderboards, add friends, and more!"; "Xp"; "REGISTER USER"; "Main Menu"; "Reset"; "Patreon"; "Enter Password..."; "Scoreboard"; "Deaths"; "Register"; "● Reset your password"; "Confirm Password..."; "True Masters "; "Welcome"; "RESET PASSWORD"; "Username"; "Login"; "Enter a username"; "Load progress from Google Account"; "x"; "Now you can become a Patron and have awesome benefits like AD FREE gameplay and UNLOCK ALL characters, modes and stages in the game."; "Enter Username..."; "LOGIN USER"; "Congratulations you are now logged in. "; "QUIT "; "quit"; "0"; "Are you sure you want to leave?"; "250"; "cancel". Métodos UnityEvent: GoToFriendsScene, MainMenu, QuestionLoad, LoginScreen, ResetPasswordScreen, SignOutButton, RegisterScreen, SaveDataButton, FormatString, LoginButton, GoToPatreon, UserDataScreen, ScoreboardButton, RegisterButton, ResetPasswordButton.

## Assets/Scenes/OnlineFriends.unity

GUID: `efcda80ddcf0dbd418e73ec53b5f706a`. 62 GameObjects, 92 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas/FriendsList` [ID 10]; `FriendsController` [ID 20]; `Canvas/AddFriend` [ID 23]; `Canvas/FriendsList/Scroll/Panel` [ID 25]; `Canvas` [ID 30]; `Canvas/FriendRequest/Scroll/Panel` [ID 33]; `Canvas/FriendRequest` [ID 34]; `Canvas/Tabs/FriendsList_Tab` [ID 41]; `Canvas/Tabs/Friend Request_Tab` [ID 49]; `Canvas/Tabs/AddFriend_Tab` [ID 51]; `Canvas/GoBack/QuitButton` [ID 53].

Textos serializados: "Requests"; "Send Friend Request"; "You can add a friend by searching for them with their username"; "Watch Battle"; "​"; "Accept Friend Request"; "Enter Username"; "Invite to battle"; "Status"; "Friends List"; "Friends"; "Add Friend"; "Delete"; "Name"; "QUIT "; "quit"; "Are you sure you want to leave?"; "cancel". Métodos UnityEvent: SendInvitation, AcceptFriendRequest, SearchFriend, ShowfriendRequestsUI, WatchBattle, OrderFriendList, QuestionDeleteFriend, ShowFriendListUI, ShowAddFriendUI.

## Assets/Scenes/Spectator.unity

GUID: `c8c587d883ea93542b7a0a887d905046`. 23 GameObjects, 36 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas` [ID 6]; `Music` [ID 7]; `Canvas/GoBack/QuitButton` [ID 13].

Textos serializados: "Waiting for players to choose their characters"; "QUIT "; "quit"; "Are you sure you want to leave?"; "250"; "cancel". Métodos UnityEvent: ninguno recuperado.

## Assets/Scenes/StartMenu.unity

GUID: `0606db6b8e65f1641b1442ed9792aff7`. 93 GameObjects, 136 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Canvas/HelpMe` [ID 1]; `Music` [ID 3]; `Canvas/MultiPlayer_UI` [ID 9]; `Canvas/UpdateController/UpdateButton` [ID 17]; `StarMenuManager` [ID 18]; `Canvas` [ID 19]; `Canvas/BendingBattleLogo/Canvas` [ID 23]; `Canvas/BlockButtonsBG` [ID 26]; `Canvas` [ID 27]; `Canvas/Base_UI/1Player` [ID 57]; `Canvas/Help` [ID 66]; `Canvas/Base_UI/2Players` [ID 68]; `Canvas/PlayTeddies` [ID 72]; `Canvas/Base_UI/Tournament` [ID 74]; `Canvas/Base_UI/Survival` [ID 78].

Textos serializados: "share game"; "50"; "LEVEL 3"; "entry fee"; "update now"; "new version available"; "Log in"; "0"; "1 player "; "250"; "cancel"; "Online\n"; "SURVIVAL"; "Back"; "tournament"; "enter"; "Play again\n"; "Local Network\n"; "2 players"; "support developers by sharing the game"; "TRUE         MASTERS"; "Update now"; "game still in development BY FACU RODQUIN"; "Version 1.00"; "PLAY TEDDIES"; "save game"; "2 players (Online)"; "1 player (VS CPU)". Métodos UnityEvent: GoToDiscord, GoToPatreon, BackLevel, GoToInstagram, NextLevel, GoToYouTube, UpdateNow, ShowVersion, SurvivalMode, ShowMultiplayerUI, CloseTourneyAlert, Online2Players, TourneyAlert, Share, PlayTeddies, EnterTournament, SaveGame, GoToLocalMultiplayer, GoToScene, SinglePlayerMode.

## Assets/Scenes/Tournament.unity

GUID: `9c58637ac2a96f340b8aa5d4ffd694b2`. 129 GameObjects, 178 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `FinalMusic` [ID 3]; `Canvas/All/Checkpoints/Player-2` [ID 7]; `Canvas/Menu/BG/MainMenu` [ID 8]; `WinnerMusic` [ID 12]; `Canvas/All/Checkpoints/Player-1` [ID 25]; `Canvas/All/Checkpoints/Player-6` [ID 28]; `Music` [ID 38]; `Canvas` [ID 53]; `Canvas/All/Checkpoints/Player-4` [ID 57]; `Canvas/All/Checkpoints/Player-5` [ID 60]; `Canvas/Menu` [ID 63]; `Canvas/MenuPosition` [ID 64]; `TournamentManager` [ID 68]; `Canvas/All/Checkpoints/Player-7` [ID 73]; `Canvas/All/Characters` [ID 76]; `Canvas/All/Checkpoints/Player-8` [ID 77]; `Canvas/All/Checkpoints/Player-3` [ID 80]; `Canvas/GoBack/QuitButton` [ID 94]; `Canvas/Menu/BG/PlayAgain` [ID 99].

Textos serializados: "QUIT "; "quit"; "0"; "Are you sure you want to leave?"; "250"; "cancel"; "50"; "Exit game"; "main menu"; "VS"; "Play again ". Métodos UnityEvent: ExitGame, PlayAgain.

## Assets/GameObject/FriendReqTemplate.prefab

GUID: `71e5221185f57c64c9ff8c5031badb53`. 4 GameObjects, 5 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `FriendReqTemplate` [ID 1709254077376921]; `FriendReqTemplate/FriendName` [ID 1618682377417806].

Textos serializados: "FriendName". Métodos UnityEvent: SelectFriend.

## Assets/GameObject/FriendTemplate.prefab

GUID: `17b029b1bab2b724cb681a9ad89ada84`. 5 GameObjects, 6 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `FriendTemplate` [ID 1709254077376921]; `FriendTemplate/FriendName` [ID 1618682377417806]; `FriendTemplate/FriendStatus` [ID 1703437598541787].

Textos serializados: "FriendName"; "Loading...". Métodos UnityEvent: SelectFriend.

## Assets/GameObject/Player UI.prefab

GUID: `c381a46afe9efbf4bb6845cb561fdd8f`. 7 GameObjects, 7 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `Player UI` [ID 1709254077376921]; `Player UI/Player Name Text` [ID 1099118695303065].

Textos serializados: "Player Name". Métodos UnityEvent: ninguno recuperado.

## Assets/Resources/photonprefabs/chat/ChatOld.prefab

GUID: `da9074fb3728e784ca6817b312915e99`. 10 GameObjects, 13 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `ChatOld` [ID 1709254077376921].

Textos serializados: "Enter message..."; "send". Métodos UnityEvent: ninguno recuperado.

## Assets/Resources/ui/QuitAlert.prefab

GUID: `c3023864169359b44a2a5d935ecf311d`. 8 GameObjects, 17 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): ninguno.

Textos serializados: "Are you sure you want to leave?"; "quit"; "cancel". Métodos UnityEvent: ninguno recuperado.

## Assets/Resources/ui/RoomItem.prefab

GUID: `85894b48a69e0954799797e279a406bf`. 3 GameObjects, 5 MonoBehaviours.

Identificadores estáticos relevantes (no etiquetas de UI verificadas): `RoomItem` [ID 1709254077376921]; `RoomItem/RoomName` [ID 1730242686860875].

Textos serializados: "Room Name". Métodos UnityEvent: ConnectToHost.

## Límites

- Static exported serialization only; not runtime behavior or completeness of the original game.
- Missing labels, persistent calls or custom fields may reflect incomplete IL2CPP export. Absence here does not establish absence in the game.
- Mapped script GUIDs establish an exported asset association, not functional recovered implementation.
- Prefab instances are reported but overrides/inheritance are not expanded.
- Menu profile: scenes plus prefabs selected by explicit ui/chat/friend/player UI/button/toggle/slider/dropdown/inputfield/menu path identifiers. Other prefabs not parsed; no completeness claim for UI hidden in those prefabs.
- Names/control hints are static identifiers, not proof of interactability.
