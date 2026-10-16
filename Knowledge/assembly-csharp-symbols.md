# Assembly-CSharp metadata (observed, not implementations)

Source APK 51e274bd08fd72ff2be562960d21f31871c0318a1e99d9a5c193e4f5d6024c4d; member global-metadata.dat, v39. Native type table absent.

## <Module>

Metadata offset 4893232.

Fields: .

Methods: .

## ActivateOnClick

Metadata offset 4893308.

Fields: element.

Methods: Start(); Activate(); .ctor().

## AddCustomPart

Metadata offset 4893384.

Fields: newPart, original, spawnedMesh.

Methods: AddPart(); RemovePart(); Update(); .ctor().

## AddSpeed

Metadata offset 4893460.

Fields: distance, delay, duration, easing.

Methods: Start(); .ctor().

## AddToAttackManager

Metadata offset 4893536.

Fields: .

Methods: Start(); OnDestroy(); AddAttackToList(); .ctor().

## <ChangeSizeCoroutine>d__6

Metadata offset 4893612.

Fields: <>1__state, <>2__current, <>4__this, duration, newSize, <initialSize>5__2, <elapsed>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <HitAnimationCoroutine>d__7

Metadata offset 4893688.

Fields: <>1__state, <>2__current, <>4__this, <fullSize>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## AirShieldManager

Metadata offset 4893764.

Fields: ShieldExplosionPrefab.

Methods: Start(); StartShield(); FinishShield(); OnHit(); ShieldBreak(); ChangeSizeCoroutine(newSize, duration); HitAnimationCoroutine(); .ctor().

## AIToggle

Metadata offset 4893840.

Fields: .

Methods: Start(); OnDestroy(); ChangeVisibility(visible); ToogleAI(active); StartAI(); .ctor().

## AltCharacter

Metadata offset 4893916.

Fields: anim.

Methods: Start(); .ctor().

## AnimationController_LM

Metadata offset 4893992.

Fields: tcpConnection, localData.

Methods: Init(); OnDestroy(); Reset(); PlayAndSendAnimation(animation, player, transitionSpeed); GetAndPlayAnimation(data); .ctor().

## <>c__DisplayClass11_0

Metadata offset 4894068.

Fields: <>4__this, initialSpeed.

Methods: .ctor(); <GoVertical>b__0(speed).

## <LerpToZeroInTimeCoroutine>d__9

Metadata offset 4894144.

Fields: <>1__state, <>2__current, totalTime, startSpeed, onValueChanged, <elapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## AnimatorEventsController

Metadata offset 4894220.

Fields: playerController, verticalCoroutine, horizontalCoroutine.

Methods: Start(); OnDestroy(); Update(); Restore(playerLayer); CreateFX(creation); CreatePlayerFX(creation); LerpToZeroInTimeCoroutine(startSpeed, totalTime, onValueChanged); GoHorizontal(initialSpeedAndTime); GoVertical(initialSpeedAndTime); reduceCollider(); recoverCollider(); StopJump(); StartAwakening(); StartSpecial(); CreateAttack(attackPrefab); NoDelay(); CreateSideAttack(attackPrefab); CreateLandAtk(); .ctor(); <GoHorizontal>b__10_0(speed).

## AtkEffect

Metadata offset 4894296.

Fields: value__, None, LeftAtk, RightAtk, Freeze, Hit3, EarthStun.

Methods: .

## AtkDescription

Metadata offset 4894372.

Fields: id, owner, damage, hp, effect, attacksController, fxController, indestructible, unblockable.

Methods: Start(); Update(); .ctor().

## AttackController

Metadata offset 4894448.

Fields: attack.

Methods: Start(); ShowExplotion(); .ctor().

## AttackEvents

Metadata offset 4894524.

Fields: AttackHit.

Methods: OnTriggerEnter(other); .ctor().

## AttacksController_LM

Metadata offset 4894600.

Fields: attackList, destroyAtkController, tcpConnection, localData, fxController, special_LM, currentAttackID.

Methods: Init(); OnDestroy(); Reset(); CreateAndSendAttack(attackPrefab, playerLayer, playerID, pos, rot); CreateAtk(attackPrefab, atkLayer, pos, rot, id, playerID); GetAtkLayer(parentLayer); GetParentLayer(atkLayer); SendCreateAttackMessage(atkName, atkID, pos, rot, atkLayer, playerID); OnAttackReceived(message); ChangeAtkLayer(atkLayer); .ctor().

## AttacksManager

Metadata offset 4894676.

Fields: MainCharAttacks, OpponentAttacks, i.

Methods: Start(); GetOwnAttacks(layer); .ctor().

## AudioRandomClip

Metadata offset 4894752.

Fields: audioSource, audioClips.

Methods: Awake(); Update(); AssignRandomClip(); ChangeClip(); .ctor().

## AuraController

Metadata offset 4894828.

Fields: auras, chargeFX.

Methods: Start(); ShowChargeAwake(); StartAura(); StartAuraInit(); StopAura(); StopAuraInit(); .ctor().

## IAura

Metadata offset 4894904.

Fields: .

Methods: PlayAuraFXs(); StopAuraFXs().

## AuraManager

Metadata offset 4894980.

Fields: auraPrefabs, chargeList, auraInstace, playerController.

Methods: InitAura(); FinishAura(); ShowChargeAwakening(); Update(); .ctor().

## CharacterEnum

Metadata offset 4895056.

Fields: value__, Sora, Aura, Vinea, Eliot, Diana, Ren.

Methods: .

## AvatarSwapper

Metadata offset 4895132.

Fields: animator, models, avatars, iddleAnimations, currentIndex, currentCharacter, currentInstance, playerController, onPlayerSwapped.

Methods: Awake(); Update(); ChangeCharacter(characterEnum); .ctor().

## BattleCheckpoint

Metadata offset 4895208.

Fields: player, cpointOpponent.

Methods: Start(); .ctor().

## BattleSceneSingleton

Metadata offset 4895284.

Fields: i, inputReader, MainCharacter, Opponent, PlayerControlledByHost, PlayerControlledByGuest, players, counter, cameraManager, aiToggle, statsController, SpecialAtk, specialButtonOutline, specialButtonFill, specialButtonText, mainCharScore, opponentScore, scoreManager, WinnerText, PlayAgain_LM, PlayAgain_LM_ExitMenu, finishText, timeOutSound, finishMenu.

Methods: Awake(); OnDestroy(); GetOpponent(playerID); IsMainCharacter(playerID); DistanceBetweenPlayers(); .ctor().

## IBattleTask

Metadata offset 4895360.

Fields: .

Methods: Init(); Reset().

## <StarGameCO>d__18

Metadata offset 4895436.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## BattleTasks_LM

Metadata offset 4895512.

Fields: battleTasks, localData, createCharacters_LM, guestInputs_LM, startCounter_LM, animationController_LM, movePlayersInHost_LM, movePlayersInGuest_LM, damageController, attacksController_LM, FxController, blockingController_LM, special_LM, scoreController_LM, menuController_LM.

Methods: Start(); OnDestroy(); StartGame(); StarGameCO(); ResetAll(); .ctor().

## <ShareCodeToFriend>d__38

Metadata offset 4895588.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## BattleUIManager

Metadata offset 4895664.

Fields: ratePopupPrefab, ratePopupGameObj, exitMenuScript, ratePopupScript, spectators, spectatorsNumber, roomName, spectatorView, scoreP1, scoreP2, nameP1, nameP2, AtkP1, AtkP2, lifeP1, lifeP2, p1Life1, p1Life3, p2Life1, p2Life3, scoreP1Pos, scoreP2Pos, nameP1Pos, nameP2Pos, AtkP1Pos, AtkP2Pos, lifeP1Pos, lifeP2Pos, SpectatorNamePrefab, SpectatorScroll, SpectatorPanel, spectatorListOpen.

Methods: Start(); Update(); OpenExitMenu(); OpenRatePopup(); SendFriendRequest(); ShareCode(); ShareCodeToFriend(); RefreshSpectators(); ChangeSpectatorView(); ChangeSpectatorCamera(); ChangeSpectatorElements(); ResetPosition(); ToggleSpectatorScroll(); .ctor().

## Blocking

Metadata offset 4895740.

Fields: .

Methods: .ctor().

## BlockingController

Metadata offset 4895816.

Fields: tcpConnection, localData.

Methods: Init(); OnDestroy(); Reset(); BlockingLogic(player, inputs); SendUnblock(playerId); GetUnblock(data); .ctor().

## BonePhysics

Metadata offset 4895892.

Fields: targetlastPosition, targetHorizontal, targetVertical, lastTargetHorizontal, iddleSpeed, Limdistance, distance, maxHorizontalMove, maxVerticalMove, minVerticalMove, twist, target, limitBoneAngle, boneAngle, horizontal, vertical, velocity, maxVelocity, horizontalAngle, minAngle, maxAngle, maxDistance, minDistance, running, iddle, runningSpeed.

Methods: FixedUpdate(); SetByLastPosition(); SmoothAnim(); .ctor().

## BonusText

Metadata offset 4895968.

Fields: .

Methods: Start(); .ctor().

## BranchDrill

Metadata offset 4896044.

Fields: rotateSelf, simpleMove, stopTime.

Methods: Start(); Stop(); Update(); .ctor().

## BranchPrison

Metadata offset 4896120.

Fields: enemyTrapped, explotion.

Methods: Start(); DestroyPrison(playerLayer); HitOnTrap(playerLayer); OnDestroy(); PrisonTrap(playerLayer); .ctor().

## <ReduceSpeed>d__15

Metadata offset 4896196.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <RetrieveAttack>d__16

Metadata offset 4896272.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## BranchPrisonAnim

Metadata offset 4896348.

Fields: move, rotate, branch1, branch2, originalMoveSpeed, originalRotateSpeed, originalBranch1Pos, originalBranch2Pos, time, elapsedtime, curve, inverseCurve, refCharacter, onAttackExplote.

Methods: Start(); ReduceSpeed(); RetrieveAttack(); PlayRetrieveAnim(); .ctor().

## BranchPrisonCharController

Metadata offset 4896424.

Fields: actualPrisonAtk, crushAttack.

Methods: CrushAttack(); StartBranchCrushAnim(); PlayCharCrushAnim(); .ctor().

## branchPrisonController

Metadata offset 4896500.

Fields: hitCharacter, branch1, branch2, branchAnim, explotionPrefab, destroyByTime, enemyTrapped, crushSound, owner.

Methods: Start(); PlayCrushAnimation(); TrapEnemy(); DestroyOnEnemyHit(); AddListernerOnEnemyDamage(); PlayHideAnimation(); DestroyInTime(); .ctor().

## <ShowTimeRecovery>d__13

Metadata offset 4896576.

Fields: <>1__state, <>2__current, <>4__this, time.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ButtonAttack

Metadata offset 4896652.

Fields: letter, attackType, recoveryTime, currentButtonCircle, currentButtonLetter, currentHitZone, currentRadialFX, reloading, currentTime.

Methods: Start(); OnButtonPress(); ReloadButton(); StopAttacking(); ShowTimeRecovery(time); RestoreButton(); ChangeToA(); ChangeToX(); .ctor().

## <ShowTimeRecovery>d__22

Metadata offset 4896728.

Fields: <>1__state, <>2__current, buttonLetter, time, <>4__this, <originalLetter>5__2, <currentTime>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ButtonsController

Metadata offset 4896804.

Fields: i, buttonACircle, buttonALetter, AHitZone, ARadialFX, buttonXCircle, buttonXLetter, XHitZone, XRadialFX, buttonBCircle, buttonBLetter, currentButtonCircle, currentButtonLetter, currentHitZone, currentRadialFX, buttonAttackA, buttonAttackX, blockInputs, lastAttackType.

Methods: Start(); ChangeColor(color, button); StartButtonRecovery(time, button); ShowTimeRecovery(buttonLetter, time); .ctor().

## <cShake>d__5

Metadata offset 4896880.

Fields: <>1__state, <>2__current, <>4__this, duration, <easyOut>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## CameraShake

Metadata offset 4896956.

Fields: i, originalPos, Shaking.

Methods: Awake(); Shake(duration); cShake(duration); .ctor().

## ChangeCamera

Metadata offset 4897032.

Fields: .

Methods: Start(); .ctor().

## ChangeCollider

Metadata offset 4897108.

Fields: newSize, delay.

Methods: Start(); ChangeSize(); .ctor().

## ChangeColliderCenter

Metadata offset 4897184.

Fields: col, startCenter, targetCenter, duration, delay, timeElapsed.

Methods: Start(); EnableDelay(); Update(); .ctor().

## ChangeColliderScale

Metadata offset 4897260.

Fields: col, startScale, targetScale, duration, delay, timeElapsed.

Methods: Start(); EnableDelay(); Update(); .ctor().

## ChangeColliderScale_LM

Metadata offset 4897336.

Fields: col, startScale, targetScale, duration, delay, timeElapsed, running.

Methods: Awake(); OnEnable(); StartScaling(); Update(); .ctor().

## ChangeColorSameElement

Metadata offset 4897412.

Fields: effect, PlayerColor, EnemyColor.

Methods: Start(); Update(); .ctor().

## ChangeMaterialVisibility

Metadata offset 4897488.

Fields: .

Methods: Start(); .ctor().

## ChangeMatSameChar

Metadata offset 4897564.

Fields: altMAT, mesh, skinnedMesh.

Methods: Start(); .ctor().

## <ChangeVisibility>d__5

Metadata offset 4897640.

Fields: <>1__state, <>2__current, <>4__this, opacity, duration, <timeElapsed>5__2, <color>5__3, <originalOpacity>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeMeshVisibility

Metadata offset 4897716.

Fields: mesh, duration, mat.

Methods: Start(); VisibilityChange(opacity); ChangeVisibility(opacity, duration); .ctor().

## ChangeMoveSpeed

Metadata offset 4897792.

Fields: newSpeed, delay.

Methods: Start(); ChangeSpeed(); Update(); .ctor().

## ChangeParticleColor

Metadata offset 4897868.

Fields: col.

Methods: Start(); Update(); .ctor().

## ChangeRendererMat

Metadata offset 4897944.

Fields: newMat.

Methods: Start(); ChangeColor(obj); ChangeColorOnline(); .ctor().

## ChangeScale

Metadata offset 4898020.

Fields: startScale, targetScale, duration, delay, easing.

Methods: Awake(); Start(); .ctor().

## ChangeScaleLoop

Metadata offset 4898096.

Fields: duration, minScale, maxScale, timer.

Methods: Update(); .ctor().

## <ChangeVisibility>d__6

Metadata offset 4898172.

Fields: <>1__state, <>2__current, <>4__this, <timeElapsed>5__2, <color>5__3, <originalOpacity>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeSimpleMeshVis

Metadata offset 4898248.

Fields: mesh, mat, visibility, delay, duration.

Methods: Start(); ChangeVisibility(); .ctor().

## <Change>d__6

Metadata offset 4898324.

Fields: <>1__state, <>2__current, <>4__this, size, <timeElapsed>5__2, <originalScale>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeSize

Metadata offset 4898400.

Fields: newSize, duration, destroyAtEnd, delay.

Methods: OnEnable(); ChangeSizeTo(size); Change(size); .ctor().

## <ForceOnlineStartPosition>d__5

Metadata offset 4898476.

Fields: <>1__state, <>2__current, <>4__this, right, forward, <timeElapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeStartPosition

Metadata offset 4898552.

Fields: forward, right, up, fixOnlinePosition.

Methods: Awake(); ForceOnlineStartPosition(right, forward); .ctor().

## ChangeStartRotation

Metadata offset 4898628.

Fields: addRotation.

Methods: Start(); .ctor().

## ChangeStartScale

Metadata offset 4898704.

Fields: scale.

Methods: Awake(); .ctor().

## ChangeText

Metadata offset 4898780.

Fields: newText, mode.

Methods: Start(); Update(); .ctor().

## CharacterMovementAnimation

Metadata offset 4898856.

Fields: animator, lastPosition, smoothingSpeed, smoothedHorizontal, smoothedVertical.

Methods: Start(); Update(); .ctor().

## CharactersManager

Metadata offset 4898932.

Fields: i, characters, playableCharacters, lastCharacter.

Methods: Start(); SetCharacters(); SetPlayableCharacters(); RemoveBlockCharacters(); RemoveRandom(character); IsCharUnlocked(charName); .ctor().

## CharactersTournament

Metadata offset 4899008.

Fields: i, characters, players.

Methods: Awake(); Start(); Init(); SetCharacters(); SetPlayer(); Suffle(); SetIcons(); SaveCharacters(); .ctor().

## CharacterSwap

Metadata offset 4899084.

Fields: .

Methods: Start(); Update(); .ctor().

## ChargeSpecial

Metadata offset 4899160.

Fields: lastAtk, newAtk.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## ComplexMove

Metadata offset 4899236.

Fields: totalDuration, forwardCurve, rightCurve, upCurve, maxForwardSpeed, maxRightSpeed, maxUpSpeed, initialForward, initialRight, initialUp, startTime.

Methods: Start(); Update(); .ctor().

## <>c

Metadata offset 4899312.

Fields: <>9, <>9__20_0.

Methods: .cctor(); .ctor(); <GetLocalIP>b__20_0(a).

## <StartGuestConnection>d__21

Metadata offset 4899388.

Fields: <>1__state, <>2__current, <>4__this, friendsIP, port, <ping>5__2, <timeout>5__3, <timer>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <StartHostConnectionCO>d__27

Metadata offset 4899464.

Fields: <>1__state, <>2__current, <>4__this, <ping>5__2, <timeout>5__3, <timer>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Connecting_LM

Metadata offset 4899540.

Fields: ping_text, tcp_text, udp_text, attemps, numberOfAttemps, ip_text, ClientType, udpConnection, tcpConnection, localData, syncTimers, friendIP, PingDone, TCPHostReceived, UDPGuestReceived, UDPHostReceived, TCPGuestReceived.

Methods: Start(); OnDestroy(); JoinGame(friendsIPLastPart, port); GetLocalIP(); StartGuestConnection(friendsIP, port); ConnectionResults(result); StartServerHost(port); StartServerGuest(port); StartHostConnection(guestIP); Update(); StartHostConnectionCO(); GetFullIPFromLastNumbers(lastPart); SendSyncTimers(); GetSyncTimers(data); GetLastNumberOfIP(ip); SendTCPMsg(host); GetTCPMsg(data); Close(); CleanUI(); SendUDPMsg(host); GetUDPMsg(data); ConnectionTimeOut(); .ctor().

## Connect_LM

Metadata offset 4899616.

Fields: inputIP, dropdown, connecting_LM.

Methods: Start(); Connect(); Create(); Update(); .ctor().

## CopyParentLayer

Metadata offset 4899692.

Fields: .

Methods: Start(); .ctor().

## CopyPatherLayer

Metadata offset 4899768.

Fields: .

Methods: Awake(); Update(); .ctor().

## CounterDone

Metadata offset 4899844.

Fields: .

Methods: .ctor(object, method); Invoke(); BeginInvoke(callback, object); EndInvoke(result).

## <ChargeShieldBar>d__10

Metadata offset 4899920.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## CounterUI

Metadata offset 4899996.

Fields: onCounterDone, radialImage, timeText, duration, elpasedTime.

Methods: add_onCounterDone(value); remove_onCounterDone(value); Start(); Update(); ChargeShieldBar(); .ctor().

## CpuMode

Metadata offset 4900072.

Fields: textMode, modes, actualMode, maxLv.

Methods: Start(); ChangeMode(); NextMode(); PreviousMode(); Update(); .ctor().

## CreateByTime

Metadata offset 4900148.

Fields: creation, delay, time.

Methods: Start(); Sumon(); Update(); .ctor().

## CreateCharacters_LM

Metadata offset 4900224.

Fields: TCPConnection, localData, damageController, attacksController, specialController, scoreController.

Methods: Init(); Reset(); SpawnCharacters(); .ctor().

## CreateObjAnim

Metadata offset 4900300.

Fields: .

Methods: CreateObj(obj); .ctor().

## CreateObject

Metadata offset 4900376.

Fields: obj, delay, addPosition.

Methods: Start(); Create(); .ctor().

## CreateOnClick

Metadata offset 4900452.

Fields: Creation.

Methods: Start(); Create(); .ctor().

## CreateOnCollition

Metadata offset 4900528.

Fields: prefab, createOnCollider, fxController.

Methods: Start(); OnTriggerEnter(other); .ctor().

## <ManualBroadCastCO>d__11

Metadata offset 4900604.

Fields: <>1__state, <>2__current, <>4__this, <subnet>5__2, <i>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## CreateRoomManager

Metadata offset 4900680.

Fields: roomNameInput, passwordInput, UIManager, udpManager, currentRoomName, currentPassword, broadCastRoom.

Methods: Start(); Create(); ValidateFields(); ManualBroadCast(); ManualBroadCastCO(); BroadcastRoomInfo(); Test(); CloseConnection(); .ctor().

## RoomData

Metadata offset 4900756.

Fields: e, roomName, password, ip.

Methods: .ctor(e, roomName, password, ip).

## CrystalsNotification

Metadata offset 4900832.

Fields: i, BonusAdGlobe, AdGlobe, numCrystals, numCrystalsBonus.

Methods: Start(); Update(); ShowBonusSmall(crystals); ShowBonus(crystals); Hide(); WinCrystals(crystals); SpendCrystals(crystals); WinTrophies(trophies); SpendTrophies(trophies); .ctor().

## CustomLog

Metadata offset 4900908.

Fields: content, i.

Methods: Start(); OnDestroy(); AddMessage(message); Clear(); Update(); .ctor().

## CustomShow

Metadata offset 4900984.

Fields: showInMode.

Methods: Start(); Update(); .ctor().

## DamageController

Metadata offset 4901060.

Fields: tcpConnection, localData, playersMovement, hitController, animationController, onPlayerHPChanged, special_LM.

Methods: Init(); OnDestroy(); Reset(); PlayAndSendHitAnimation(player, atk); SendPlayerHp(playerID, hp); ResetHP(playerController); UpdateHealthUI(playerID, hp); GetPlayerHp(data); .ctor().

## DamagePlayerController

Metadata offset 4901136.

Fields: .

Methods: Start(); Update(); .ctor().

## DarkModeTransform

Metadata offset 4901212.

Fields: parts.

Methods: Start(); DarkTransform(time); ChangeToBlack(); ChangeToNormal(); .ctor().

## DatabaseTextChange

Metadata offset 4901288.

Fields: textConnected, textDisconnected, text.

Methods: Start(); .ctor().

## Death

Metadata offset 4901364.

Fields: death.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## DecreaseOnClick

Metadata offset 4901440.

Fields: elements.

Methods: Start(); Decrease(); .ctor().

## DecreaseSizeOnCollition

Metadata offset 4901516.

Fields: size.

Methods: Start(); OnTriggerEnter(collider); .ctor().

## <>c__DisplayClass6_0

Metadata offset 4901592.

Fields: atkID.

Methods: .ctor(); <OnDestroyAtkReceived>b__0(a).

## DestroyAtkController

Metadata offset 4901668.

Fields: atkController.

Methods: Start(); OnDestroy(); DestroyAndSendDestroyMsg(attack, byTime); DestroyAttack(attack); SendDestroyAttackMessage(attackID); OnDestroyAtkReceived(message); .ctor().

## DestroyByTime_Basic

Metadata offset 4901744.

Fields: delay.

Methods: Start(); Update(); .ctor().

## <DestroyCO>d__2

Metadata offset 4901820.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## DestroyByTime_LM

Metadata offset 4901896.

Fields: time.

Methods: Start(); DestroyCO(); .ctor().

## DestroyIcePrison

Metadata offset 4901972.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## DestroyIfPlayerWeak

Metadata offset 4902048.

Fields: .

Methods: Update(); .ctor().

## DestroyIfSameObjIsNear

Metadata offset 4902124.

Fields: limitDistance.

Methods: Start(); DestroyNearAttack(attackList); .ctor().

## DestroyOnClick

Metadata offset 4902200.

Fields: elements, time.

Methods: Start(); Destroy(); .ctor().

## <DestroyCO>d__5

Metadata offset 4902276.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## DestroyOnCollision_LM

Metadata offset 4902352.

Fields: destroyDelay, attacksController, atkDescription.

Methods: Start(); OnTriggerEnter(collider); DestroyCO(); .ctor().

## DestroyOnEdge

Metadata offset 4902428.

Fields: .

Methods: Start(); Update(); .ctor().

## DestroyOnOwnerDamage

Metadata offset 4902504.

Fields: owner.

Methods: Start(); DestroyByHit(); OnDestroy(); .ctor().

## DestroyOnSceneChange

Metadata offset 4902580.

Fields: targetSceneIndex.

Methods: Start(); OnSceneChanged(current, next); OnDestroy(); .ctor().

## DestroyOthers

Metadata offset 4902656.

Fields: others.

Methods: Start(); Update(); OnDestroy(); .ctor().

## DestroyWhenCreated

Metadata offset 4902732.

Fields: target.

Methods: Start(); .ctor().

## DetachFromParent

Metadata offset 4902808.

Fields: delay.

Methods: Start(); Detach(); .ctor().

## <DefineAnim>d__3

Metadata offset 4902884.

Fields: <>1__state, <>2__current, delay, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## DisableColliderAnim

Metadata offset 4902960.

Fields: anim, delay.

Methods: Start(); DefineAnim(delay); CheckAttack(); .ctor().

## DisableColliderOnCollition

Metadata offset 4903036.

Fields: col, onlyByPlayers.

Methods: Start(); OnTriggerEnter(collider); .ctor().

## DisableColliderOnCollition_LM

Metadata offset 4903112.

Fields: .

Methods: Start(); OnTriggerEnter(collider); .ctor().

## DisableColliderOnline

Metadata offset 4903188.

Fields: .

Methods: Start(); .ctor().

## DisableComponent

Metadata offset 4903264.

Fields: comp, timer.

Methods: Start(); Update(); DisableComponentByTime(); .ctor().

## <>c

Metadata offset 4903340.

Fields: <>9, <>9__1_0.

Methods: .cctor(); .ctor(); <Start>b__1_0(comp).

## DisableComponentsOnline

Metadata offset 4903416.

Fields: comps.

Methods: Start(); .ctor().

## EarthPrisonController

Metadata offset 4903492.

Fields: playerController, currentEarthPrison, earthPrisonPrefab.

Methods: Start(); Init(); OnDestroy(); CreateEarthPrisonAttack(attackPrefab); CreateEarthPrison2(); SendCreatePrison(); GetCreatePrison(data); CloseEarthPrison(); CloseEarthPrison2(); SendCloseEarthPrison(); GetCloseEarthPrison(data); BreakEarthPrison(); SendBreakPrison(); GetBreakPrison(data); .ctor().

## EarthPrison_LM

Metadata offset 4903568.

Fields: prisonParts, moveUpList, moveDownList, captured, EarthPrisonExplosion.

Methods: Start(); ClosePrison(); OnTriggerEnter(other); StartCapture(); ReleasePrison(); DestroyPrison(); .ctor().

## EarthShieldManager

Metadata offset 4903644.

Fields: ShieldExplosionPrefab, originalScale, shieldAnim.

Methods: Start(); StartShield(); FinishShield(); OnHit(); ShieldBreak(); .ctor().

## EnableByDistance

Metadata offset 4903720.

Fields: objRef, objToChange, distance.

Methods: Update(); .ctor().

## EnableComponent

Metadata offset 4903796.

Fields: components, limitDistance, enemy.

Methods: Start(); Update(); .ctor().

## EnableComponentsOnCollition

Metadata offset 4903872.

Fields: components.

Methods: OnTriggerEnter(other); .ctor().

## EnableScriptByScene

Metadata offset 4903948.

Fields: script, disableOnSceneExit, sceneIndex.

Methods: Start(); OnSceneLoaded(scene, mode); OnDestroy(); .ctor().

## EnableScriptByTime

Metadata offset 4904024.

Fields: scripts, time.

Methods: Start(); InitScript(); .ctor().

## EnableScriptOnCollition

Metadata offset 4904100.

Fields: components.

Methods: OnTriggerEnter(collider); .ctor().

## EventSystemKeepSelected

Metadata offset 4904176.

Fields: eventSystem, lastSelected.

Methods: Start(); Update(); .ctor().

## <FadeIn>d__8

Metadata offset 4904252.

Fields: <>1__state, <>2__current, <>4__this, source, duration, targetVolume.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <FadeOut>d__9

Metadata offset 4904328.

Fields: <>1__state, <>2__current, <>4__this, source, duration, <startVolume>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FadeAudio

Metadata offset 4904404.

Fields: audioSource, fadeDuration, clipTimer, songTime, targetVolume, isFading.

Methods: Start(); Update(); FadeIn(source, duration, targetVolume); FadeOut(source, duration); .ctor().

## <FadeToLevelDelayCorutine>d__7

Metadata offset 4904480.

Fields: <>1__state, <>2__current, time, <>4__this, level.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LevelChanger

Metadata offset 4904556.

Fields: animator, i, levelToLoad, loadingScreen.

Methods: Start(); FadeToNextLevel(); FadetoLevelDelay(level, time); FadeToLevelDelayCorutine(level, time); FadeToLevel(levelIndex); OnFadeComplete(); ExitGame(); MainMenu(); Disconnect(); HideCrystalsNotification(); .ctor().

## FindPrefabWithComponent

Metadata offset 4904632.

Fields: .

Methods: Start(); Update(); .ctor().

## <>c__DisplayClass17_0

Metadata offset 4904708.

Fields: DBTask, DBTask2.

Methods: .ctor(); <CleanInvitations>b__0(); <CleanInvitations>b__1().

## <>c__DisplayClass18_0

Metadata offset 4904784.

Fields: LoginTask.

Methods: .ctor(); <AutoLogin>b__0().

## <>c__DisplayClass18_1

Metadata offset 4904860.

Fields: logged.

Methods: .ctor(); <AutoLogin>b__1(status).

## <>c__DisplayClass22_0

Metadata offset 4904936.

Fields: DBTask.

Methods: .ctor(); <CheckIfPremium>b__0().

## <>c__DisplayClass23_0

Metadata offset 4905012.

Fields: DBTask.

Methods: .ctor(); <GetLastVersion>b__0().

## <>c__DisplayClass24_0

Metadata offset 4905088.

Fields: DBTask.

Methods: .ctor(); <CheckIfLogged>b__0().

## <>c__DisplayClass25_0

Metadata offset 4905164.

Fields: DBTask.

Methods: .ctor(); <CheckStatus>b__0().

## <>c__DisplayClass26_0

Metadata offset 4905240.

Fields: DBTask.

Methods: .ctor(); <CheckMode>b__0().

## <>c__DisplayClass26_1

Metadata offset 4905316.

Fields: DBTask2.

Methods: .ctor(); <CheckMode>b__1().

## <>c__DisplayClass27_0

Metadata offset 4905392.

Fields: DBTask2.

Methods: .ctor(); <CreateData>b__0().

## <>c__DisplayClass28_0

Metadata offset 4905468.

Fields: DBTask.

Methods: .ctor(); <LoadUserData>b__0().

## <>c__DisplayClass30_0

Metadata offset 4905544.

Fields: DBTask.

Methods: .ctor(); <UpdatePlayingMode>b__0().

## <>c__DisplayClass31_0

Metadata offset 4905620.

Fields: DBTask.

Methods: .ctor(); <UpdateLastSessionDate>b__0().

## <>c__DisplayClass32_0

Metadata offset 4905696.

Fields: DBTask.

Methods: .ctor(); <UpdatePlayedTime>b__0().

## <>c__DisplayClass33_0

Metadata offset 4905772.

Fields: DBTask.

Methods: .ctor(); <UpdateLastDevice>b__0().

## <>c__DisplayClass34_0

Metadata offset 4905848.

Fields: DBTask.

Methods: .ctor(); <CheckSaveData>b__0().

## <>c__DisplayClass35_0

Metadata offset 4905924.

Fields: DBTask.

Methods: .ctor(); <SaveCrystals>b__0().

## <>c__DisplayClass36_0

Metadata offset 4906000.

Fields: DBTask.

Methods: .ctor(); <SaveTrophies>b__0().

## <>c__DisplayClass37_0

Metadata offset 4906076.

Fields: DBTask.

Methods: .ctor(); <SaveSurvivalRecord>b__0().

## <>c__DisplayClass38_0

Metadata offset 4906152.

Fields: DBTask2.

Methods: .ctor(); <SaveCharacters>b__0().

## <>c__DisplayClass39_0

Metadata offset 4906228.

Fields: DBTask2.

Methods: .ctor(); <SaveStages>b__0().

## <>c__DisplayClass40_0

Metadata offset 4906304.

Fields: DBTask.

Methods: .ctor(); <SaveFriend>b__0().

## <>c__DisplayClass40_1

Metadata offset 4906380.

Fields: DBTask2.

Methods: .ctor(); <SaveFriend>b__1().

## <>c__DisplayClass41_0

Metadata offset 4906456.

Fields: DBTask2.

Methods: .ctor(); <SendFriendRequest>b__0().

## <>c__DisplayClass42_0

Metadata offset 4906532.

Fields: DBTask2.

Methods: .ctor(); <AcceptFriendRequest>b__0().

## <>c__DisplayClass43_0

Metadata offset 4906608.

Fields: DBTask.

Methods: .ctor(); <DeleteFriendCo>b__0().

## <>c__DisplayClass43_1

Metadata offset 4906684.

Fields: DBTask2.

Methods: .ctor(); <DeleteFriendCo>b__1().

## <AcceptFriendRequest>d__42

Metadata offset 4906760.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <AutoLogin>d__18

Metadata offset 4906836.

Fields: <>1__state, <>2__current, <>4__this, _email, _password, <>8__1, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckIfLogged>d__24

Metadata offset 4906912.

Fields: <>1__state, <>2__current, <>4__this, <>8__1, callback.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckIfPremium>d__22

Metadata offset 4906988.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckInternet>d__19

Metadata offset 4907064.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckMode>d__26

Metadata offset 4907140.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1, friendTemplate, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckSaveData>d__34

Metadata offset 4907216.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckStatus>d__25

Metadata offset 4907292.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1, friendTemplate.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CleanInvitations>d__17

Metadata offset 4907368.

Fields: <>1__state, <>2__current, invitation, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CreateData>d__27

Metadata offset 4907444.

Fields: <>1__state, <>2__current, <>4__this, <>8__1, <user>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <DeleteFriendCo>d__43

Metadata offset 4907520.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <GetLastVersion>d__23

Metadata offset 4907596.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <LoadUserData>d__28

Metadata offset 4907672.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveCharacters>d__38

Metadata offset 4907748.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveCrystals>d__35

Metadata offset 4907824.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveFriend>d__40

Metadata offset 4907900.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveStages>d__39

Metadata offset 4907976.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveSurvivalRecord>d__37

Metadata offset 4908052.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SaveTrophies>d__36

Metadata offset 4908128.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SendFriendRequest>d__41

Metadata offset 4908204.

Fields: <>1__state, <>2__current, <>4__this, friendID, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdateLastDevice>d__33

Metadata offset 4908280.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdateLastSessionDate>d__31

Metadata offset 4908356.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdatePlayedTime>d__32

Metadata offset 4908432.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdatePlayingMode>d__30

Metadata offset 4908508.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FirebaseController

Metadata offset 4908584.

Fields: i, dependencyStatus, auth, DBreference, tmPlayer, canSave, guestInvitation, currentInvitation, user_reference, noInternet.

Methods: Start(); InitializeFirebase(); IsConnected(); AutoLog(); StartUpdateCorroutines(); HandleInvitationChanged(sender, args); CheckInvitationStatus(invitation); CleanInvitations(invitation); AutoLogin(_email, _password); CheckInternet(); CheckIfOnline(lastSesion); CheckLastInvitation(lastInvitationTime); CheckIfPremium(); GetLastVersion(); CheckIfLogged(callback); CheckStatus(friendID, friendTemplate); CheckMode(friendID, friendTemplate); CreateData(); LoadUserData(); LoadDataInGame(playerDatabase); UpdatePlayingMode(); UpdateLastSessionDate(); UpdatePlayedTime(); UpdateLastDevice(); CheckSaveData(); SaveCrystals(); SaveTrophies(); SaveSurvivalRecord(); SaveCharacters(); SaveStages(); SaveFriend(friendID); SendFriendRequest(friendID); AcceptFriendRequest(friendID); DeleteFriendCo(friendID); .ctor().

## FollowOwner_LM

Metadata offset 4908660.

Fields: .

Methods: Start(); Update(); .ctor().

## FollowPath

Metadata offset 4908736.

Fields: pathManager, pathPoint.

Methods: Start(); Update(); .ctor().

## ForceShieldOff

Metadata offset 4908812.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## <>c__DisplayClass10_0

Metadata offset 4908888.

Fields: DBTask, DBTask2.

Methods: .ctor(); <GuestDeclineInvitation>b__0(); <GuestDeclineInvitation>b__1().

## <>c__DisplayClass9_0

Metadata offset 4908964.

Fields: DBTask, DBTask2.

Methods: .ctor(); <UpdateGuestReady>b__0(); <UpdateGuestReady>b__1().

## <GuestDeclineInvitation>d__10

Metadata offset 4909040.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdateGuestReady>d__9

Metadata offset 4909116.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FriendInvitationPopup

Metadata offset 4909192.

Fields: i, friendName, hostID, boxPopup.

Methods: Start(); OpenInvitation(username, userID); AcceptInvitation(); DeclineInvitation(); CloseInvitation(); UpdateGuestReady(); GuestDeclineInvitation(); .ctor().

## FriendReqTemplate

Metadata offset 4909268.

Fields: friendName, friendID.

Methods: Start(); Update(); SelectFriend(); .ctor().

## <>c__DisplayClass13_0

Metadata offset 4909344.

Fields: DBTask, DBTask2.

Methods: .ctor(); <SendFriendInvitationPopup>b__0(); <SendFriendInvitationPopup>b__1().

## <>c__DisplayClass26_0

Metadata offset 4909420.

Fields: DBTask2.

Methods: .ctor(); <SendCodeToGuest>b__0().

## <SendCodeToGuest>d__26

Metadata offset 4909496.

Fields: <>1__state, <>2__current, code, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SendFriendInvitationPopup>d__13

Metadata offset 4909572.

Fields: <>1__state, <>2__current, <>8__1, <>4__this, <myInvitation>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FriendsConnection

Metadata offset 4909648.

Fields: i, regions, alert, serverText, dropdown, regionMenu, loading, counter, hostCode, hostRegion.

Methods: Awake(); CounterDone(); OnDestroy(); SendFriendInvitationPopup(); Connect(); CheckConnection(); OnConnectedToMaster(); DelayJoinRoom(); OnRegionListReceived(regionHandler); OnCreateRoomFailed(returnCode, message); OnJoinedRoom(); ConnectToRegion(regionIndex); CreateCode(); GuestReady(); GuestHasDeclined(); ConnectToHostRoom(code); SendCodeToGuest(code); .ctor().

## FriendTemplate

Metadata offset 4909724.

Fields: friendName, friendStatus, friendID.

Methods: Start(); Update(); SelectFriend(); .ctor().

## FxAuraManager

Metadata offset 4909800.

Fields: auraFxs.

Methods: PlayAuraFXs(); StopAuraFXs(); .ctor().

## FxController

Metadata offset 4909876.

Fields: tcpConnection, localData.

Methods: Init(); OnDestroy(); Reset(); CreateAndSendFX(fxPrefab, pos, rot); CreateFX(fxPrefab, pos, rot); SendFxToGuest(fxName, pos, rot); OnFXReceived(message); .ctor().

## GameEvents

Metadata offset 4909952.

Fields: i, onAttackLayerSetUp, OnPlayerDamaged, OnPlayerPrisonHit, OnPlayerPrisonTrap, onSummonedInOpp.

Methods: Start(); add_onAttackLayerSetUp(value); remove_onAttackLayerSetUp(value); AttackLayerSetUp(atkID); add_OnPlayerDamaged(value); remove_OnPlayerDamaged(value); PlayerDamaged(layer); add_OnPlayerPrisonHit(value); remove_OnPlayerPrisonHit(value); PlayerPrisonHit(layer); add_OnPlayerPrisonTrap(value); remove_OnPlayerPrisonTrap(value); PlayerPrisonTrap(layer); add_onSummonedInOpp(value); remove_onSummonedInOpp(value); SummonedInOpp(atkID); .ctor().

## GameModeVisibility

Metadata offset 4910028.

Fields: visibleInModes.

Methods: Awake(); .ctor().

## GameSpeed

Metadata offset 4910104.

Fields: speedText, speedList, actualSpeed.

Methods: Start(); SetSpeed(speed); ChangeSpeed(next); .ctor().

## GameVersion

Metadata offset 4910180.

Fields: .

Methods: Start(); Update(); .ctor().

## GPGSIds

Metadata offset 4910256.

Fields: achievement_perfect, leaderboard_score, achievement_250_crystals.

Methods: .

## GPGSManager

Metadata offset 4910332.

Fields: .

Methods: Start(); .ctor().

## <ChangeWings>d__5

Metadata offset 4910408.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## GrassSpecialController

Metadata offset 4910484.

Fields: wings, growDelay, shirnkDelay.

Methods: Start(); StartSpecialAtk(); ChangeWings(); .ctor().

## GuestInputs_LM

Metadata offset 4910560.

Fields: udpConnection, localData, OnGuestInputs, sendTimer, sendInterval.

Methods: Init(); OnDestroy(); Reset(); Update(); SendGuestInputs(inputs); GetGuestInputs(inputsBytes); .ctor().

## GuestMessageHandler

Metadata offset 4910636.

Fields: iPConnection, multiplayerData.

Methods: SendGuestMessage(netMessage); .ctor().

## GuestMovement_LM

Metadata offset 4910712.

Fields: currentGuestInputs, localData, guest, inputConnection_LM, udpConnection, disableInputs, targetPosition, inputAnimationController, sendTimer, sendInterval.

Methods: Start(); Update(); MoveMainCharacterInGuest(); MoveGuestCharacter(); SendGuestPositionToGuest(); GetGuestPositionInGuest(netPositionBytes); SetGuestInputs(inputSet); OnDestroy(); .ctor().

## hid

Metadata offset 4910788.

Fields: .

Methods: Start(); Update(); .ctor().

## HideGameObject

Metadata offset 4910864.

Fields: gameobjectToHide.

Methods: Start(); ToggleHide(); Update(); .ctor().

## HideOnLocalGuest

Metadata offset 4910940.

Fields: .

Methods: Start(); .ctor().

## HideOnTourney

Metadata offset 4911016.

Fields: .

Methods: Start(); Update(); .ctor().

## HitController

Metadata offset 4911092.

Fields: .

Methods: Start(); GetHitAnimationName(atk, player); IsShieldBroken(atkDamage, impact, angle, unblockeable); GetAngleDiference(obj1, obj2); .ctor().

## HomingAttack

Metadata offset 4911168.

Fields: target, delay, homing, horizontalSpeed, verticalSpeed.

Methods: Start(); Update(); StartHoming(); AngleDir(fwd, targetDir, up); .ctor().

## HostAnimations_LM

Metadata offset 4911244.

Fields: .

Methods: Start(); Update(); .ctor().

## HostMessage

Metadata offset 4911320.

Fields: type, ipSender, data.

Methods: .ctor().

## HostMessageHandler

Metadata offset 4911396.

Fields: iPConnection, multiplayerData.

Methods: SendHostMessage(netMessage); SerializeMessage(type, payload); DeserializeMessage(json); .ctor().

## HostMovement_LM

Metadata offset 4911472.

Fields: currentInputs, localData, host, udpConnection, inputAnimationController, sendTimer, sendInterval, targetPosition, disableInputs.

Methods: Start(); Update(); MoveHostCharacter(); SetHostInputs(hostInputs); SendHostPositionToGuest(); GetHostPositionInGuest(netPositionBytes); OnDestroy(); .ctor().

## HowToPlayManager

Metadata offset 4911548.

Fields: .

Methods: Continue(); .ctor().

## IcePrisonController

Metadata offset 4911624.

Fields: i.

Methods: Start(); .ctor().

## IcePrisonController_LM

Metadata offset 4911700.

Fields: .

Methods: Start(); Update(); .ctor().

## IncreaseCapsuleCollider

Metadata offset 4911776.

Fields: capCollider, originalSize, speed.

Methods: Start(); FixedUpdate(); .ctor().

## InputAnimationController

Metadata offset 4911852.

Fields: animationController.

Methods: StartAnimation(currentInputs, player); GetAnimationName(playerElement, animationType); HandleHorizontalInput(currentInputs, playerElement, movesetIndex, player); HandleVerticalInput(currentInputs, playerElement, movesetIndex, player); .ctor().

## InputSet

Metadata offset 4911928.

Fields: x, y, a, b, s.

Methods: Equals(other); Clone(); .ctor().

## InputReader

Metadata offset 4912004.

Fields: inputSet, onInputSet, joystick, blockInputs, dPad, keyboard.

Methods: Start(); Update(); SetJoystick(); ActivateDpad(isActive); UpDownDpad(value); LeftRightDpad(value); SetAButton(active); SetBButton(active); SetSButton(active); SetXpos(x); SetYpos(y); .ctor().

## <>c__DisplayClass11_0

Metadata offset 4912080.

Fields: client, <>4__this.

Methods: .ctor(); <ListenForClients>b__0().

## IPConnection

Metadata offset 4912156.

Fields: tcpListener, tcpClient, stream, port, messageQueue, onMessageReceived, listenerThread, clientThread, isRunning.

Methods: Start(); StartServer(); ListenForClients(); HandleClient(client); ConnectToServer(ip); ListenForMessages(); SendNetMessage(message); Update(); OnApplicationQuit(); OnDestroy(); CloseConnection(); SendNetEventMessage(netMessage); GetLocalIPAddress(); .ctor().

## <>c__DisplayClass7_0

Metadata offset 4912232.

Fields: roomData.

Methods: .ctor(); <HandleIncomingMessage>b__0(r).

## JoinRoomManager

Metadata offset 4912308.

Fields: udpManager, UI_Manager, roomDataList, roomItemPrefab, roomContainer, passwordManager.

Methods: InitSearchRooms(); HandleIncomingMessage(message); SearchRooms(); CheckRooms(); RoomsNotFound(); CloseRoomServer(); InitRoomList(); Test(); .ctor().

## JoystickPlayerExample

Metadata offset 4912384.

Fields: speed, variableJoystick, rb.

Methods: FixedUpdate(); .ctor().

## JoystickSetterExample

Metadata offset 4912460.

Fields: variableJoystick, valueText, background, axisSprites.

Methods: ModeChanged(index); AxisChanged(index); SnapX(value); SnapY(value); Update(); .ctor().

## Joystick

Metadata offset 4912536.

Fields: handleRange, deadZone, axisOptions, snapX, snapY, background, handle, baseRect, canvas, cam, input.

Methods: get_Horizontal(); get_Vertical(); get_Direction(); get_HandleRange(); set_HandleRange(value); get_DeadZone(); set_DeadZone(value); get_AxisOptions(); set_AxisOptions(value); get_SnapX(); set_SnapX(value); get_SnapY(); set_SnapY(value); Start(); OnPointerDown(eventData); OnDrag(eventData); HandleInput(magnitude, normalised, radius, cam); FormatInput(); SnapFloat(value, snapAxis); OnPointerUp(eventData); ScreenPointToAnchoredPosition(screenPosition); .ctor().

## AxisOptions

Metadata offset 4912612.

Fields: value__, Both, Horizontal, Vertical.

Methods: .

## DynamicJoystick

Metadata offset 4912688.

Fields: moveThreshold.

Methods: get_MoveThreshold(); set_MoveThreshold(value); Start(); OnPointerDown(eventData); OnPointerUp(eventData); HandleInput(magnitude, normalised, radius, cam); .ctor().

## FixedJoystick

Metadata offset 4912764.

Fields: .

Methods: .ctor().

## FloatingJoystick

Metadata offset 4912840.

Fields: .

Methods: Start(); OnPointerDown(eventData); OnPointerUp(eventData); .ctor().

## VariableJoystick

Metadata offset 4912916.

Fields: moveThreshold, joystickType, fixedPosition.

Methods: get_MoveThreshold(); set_MoveThreshold(value); SetMode(joystickType); Start(); OnPointerDown(eventData); OnPointerUp(eventData); HandleInput(magnitude, normalised, radius, cam); .ctor().

## JoystickType

Metadata offset 4912992.

Fields: value__, Fixed, Floating, Dynamic.

Methods: .

## JoystickSettings

Metadata offset 4913068.

Fields: dpadToggle, joystickToggle, controllerToggle, invertedToggle, dpad, joystick, buttonA, buttonB, inverted.

Methods: Start(); Invert(inv); DpadOn(); JoystickOn(); ControllerOn(); Update(); .ctor().

## jumpSimulator

Metadata offset 4913144.

Fields: animCurve, timeElapsed, duration, force, delay.

Methods: Start(); Update(); .ctor().

## <LagPulseGuestRoutine>d__22

Metadata offset 4913220.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <LagPulseHostRoutine>d__16

Metadata offset 4913296.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LagController

Metadata offset 4913372.

Fields: currentLag, avgLag, guestTimeWhenFirstPulse, hostTimeWhenFirstPulse, hostTimeWhenSecondPulse, receivedFirstPulse, lagSamples, lagTimer, secondLagPulse, TCPConnection, localData, activeLagCoroutine, onLagDataUpdated, lagSamplesCount.

Methods: Start(); Test(); LagPulseHostRoutine(); SendLagPulseToGuest(); SendSecondLagPulseToGuest(); OnLagDataReceived(message); OnLagPulseReceived(message); SendLagDataToHost(); LagPulseGuestRoutine(); CalculateAverageLag(); .ctor().

## LandController

Metadata offset 4913448.

Fields: playerController, LandAttacks.

Methods: Start(); Update(); .ctor().

## LeafShieldController

Metadata offset 4913524.

Fields: leafShield, leafAnimatorList, leafSize, leafShieldExplotion.

Methods: Start(); ShieldOn(); ShieldOff(); ShieldHit(); Update(); .ctor().

## LerpEasing

Metadata offset 4913600.

Fields: value__, Linear, EasyOut, EasyInOut.

Methods: .

## <LerpLocalPositionCoroutine>d__6

Metadata offset 4913676.

Fields: <>1__state, <>2__current, delay, obj, start, target, <>4__this, duration, easing, <timeElapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <LerpPositionCoroutine>d__4

Metadata offset 4913752.

Fields: <>1__state, <>2__current, delay, obj, start, target, <>4__this, duration, easing, <timeElapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <LerpScaleCoroutine>d__8

Metadata offset 4913828.

Fields: <>1__state, <>2__current, delay, obj, start, target, <>4__this, duration, easing, <timeElapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LerpManager

Metadata offset 4913904.

Fields: i, animCurves.

Methods: Start(); LerpPosition(obj, start, target, duration, easing, delay); LerpPositionCoroutine(obj, start, target, duration, easing, delay); LerpLocalPosition(obj, start, target, duration, easing, delay); LerpLocalPositionCoroutine(obj, start, target, duration, easing, delay); LerpScale(obj, start, target, duration, easing, delay); LerpScaleCoroutine(obj, start, target, duration, easing, delay); AddEasing(timeElapsed, duration, easing); .ctor().

## LevelChangeOnClick

Metadata offset 4913980.

Fields: level.

Methods: Start(); ChangeLevel(); .ctor().

## LevelManager

Metadata offset 4914056.

Fields: i, voiceChatManager.

Methods: Start(); .ctor().

## <Off>d__6

Metadata offset 4914132.

Fields: <>1__state, <>2__current, <>4__this, <t>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <On>d__5

Metadata offset 4914208.

Fields: <>1__state, <>2__current, <>4__this, <t>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LightOn

Metadata offset 4914284.

Fields: maxLight, duration, off, delayOff.

Methods: Start(); On(); Off(); .ctor().

## LightSword

Metadata offset 4914360.

Fields: shield, sword.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## <Dark>d__11

Metadata offset 4914436.

Fields: <>1__state, <>2__current, mat, <duration>5__2, <t>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <Light>d__10

Metadata offset 4914512.

Fields: <>1__state, <>2__current, <>4__this, mat, recover, <duration>5__2, <t>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LightWeapon

Metadata offset 4914588.

Fields: sword, shield, swordMesh, shieldMesh, yellow.

Methods: Start(); LightSword(); LightSwordAndShield(); LightShield(); DarkShield(); Light(mat, recover); Dark(mat); Update(); .ctor().

## <LoadScene>d__15

Metadata offset 4914664.

Fields: <>1__state, <>2__current, scene, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## LoadingScreen

Metadata offset 4914740.

Fields: i, bg, logo, progress, vsText, hostText, guestText, asyncOperation, num, speed, loading, tutorial01.

Methods: Start(); FixedUpdate(); StartLoading(scene); LoadScene(scene); ShowAd(); .ctor().

## CharacterName

Metadata offset 4914816.

Fields: value__, Sora, Aura, Vinea, Eliot, Diana, Ren.

Methods: .

## LocalMultiplayerData

Metadata offset 4914892.

Fields: HostCharacter, GuestCharacter, stage, gameSpeed, isHost, timer, guestScene, hostScene, battleStarted, onPlayerTypeChanged, onPlayersInSameScene, onPlayersReachBattleScene.

Methods: PlayersInSameScene(); SetHostScene(scene); SetGuestScene(scene); CheckAndTriggerSceneEvents(); ChangePlayerType(hostType); .ctor().

## LocalMultiplayerManager

Metadata offset 4914968.

Fields: start_UI, create_UI, Waiting_UI, roomsNotFound, searchRoomUI, roomListUI, onResetUI.

Methods: Start(); ShowStartUI(); ShowCreateUI(); ShowWaitingUI(); ShowSearchRoomUI(); ShowRoomsNotFound(); ShowRoomsListUI(); ToggleUI(uiToShow); GoToMainMenu(); .ctor().

## LockToTarget

Metadata offset 4915044.

Fields: target, delay.

Methods: Start(); Lock(); .ctor().

## MenuController_LM

Metadata offset 4915120.

Fields: tcpConnection, battleTasks_LM, playAgainClicked.

Methods: Init(); OnDestroy(); Reset(); OnPlayAgainClicked(); SendPlayAgainMsg(); GetPlayAgainMsg(data); .ctor().

## MeshSwapper

Metadata offset 4915196.

Fields: animator, modelA, modelB, usingA.

Methods: Start(); Update(); ShowModelA(); ShowModelB(); BindBones(smr); .ctor().

## Move

Metadata offset 4915272.

Fields: addVector, endPosition, duration, target.

Methods: Start(); .ctor().

## moveGameObj

Metadata offset 4915348.

Fields: addPosition, duration, delay, easing.

Methods: Start(); LerpPosition(); .ctor().

## MoveLocalBetweenPoints

Metadata offset 4915424.

Fields: localPositions, moveSpeed, currentIndex, currentTargetLocal, intervalTime, timer.

Methods: Update(); SetNextTarget(); .ctor().

## MoveLocalList

Metadata offset 4915500.

Fields: .

Methods: .ctor().

## MovementStateEvents

Metadata offset 4915576.

Fields: onMovementStateEnter, onMovementStateExit.

Methods: OnStateEnter(animator, stateInfo, layerIndex); OnStateExit(animator, stateInfo, layerIndex); .ctor().

## MovePlayersInGuest_LM

Metadata offset 4915652.

Fields: localData, udpConnection, host, guest, hostTargetPosition, guestTargetPosition.

Methods: Init(); OnDestroy(); Reset(); Update(); GetPlayersPosition(data); MovePlayer(player, targetPosition); .ctor().

## MovePlayersInHost_LM

Metadata offset 4915728.

Fields: hostInputs, guestInputs, localData, guestInputsReader, inputAnimationController, host, guest, disableHostInputs, disableGuestInputs, udpConnection, blockingController, sendTimer, sendInterval, playerSpeed.

Methods: Init(); OnDestroy(); Reset(); Update(); HandlePlayerInput(player, disableInputs, inputs); SetHostInputs(inputs); SetGuestInputs(inputs); MovePlayerInHost(player, currentInputs); SendPlayersPositionToGuest(); RoundVector3(vector, decimals); WriteVector3(writer, vector); .ctor().

## <MoveToPosition>d__6

Metadata offset 4915804.

Fields: <>1__state, <>2__current, <>4__this, <elapsedTime>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## MoveRelativeToRotation

Metadata offset 4915880.

Fields: targetOffset, moveDuration, delay, startPosition, finalPosition.

Methods: Start(); MoveToPosition(); .ctor().

## MoveToPoint

Metadata offset 4915956.

Fields: points, actualPoint, lastPoint.

Methods: Start(); ChangePoint(point); Update(); .ctor().

## Msg

Metadata offset 4916032.

Fields: .

Methods: Start(); Update(); .ctor().

## MusicController

Metadata offset 4916108.

Fields: lastVolume.

Methods: Start(); Music(on); .ctor().

## MuteController

Metadata offset 4916184.

Fields: .

Methods: Start(); Mute(mute); .ctor().

## BonePath

Metadata offset 4916260.

Fields: children, path.

Methods: Start(); UpdateBonesRot(); GetPathPos(dis); .ctor().

## BranchPart

Metadata offset 4916336.

Fields: size, speed, jumpDistance, path, bones, move, branch.

Methods: Awake(); Update(); .ctor().

## MoveForward

Metadata offset 4916412.

Fields: speed.

Methods: Start(); Update(); .ctor().

## MovePath

Metadata offset 4916488.

Fields: y, x, z, xSpeed, ySpeed, zSpeed, curve, elapseTime, initialPos, JumpTime, leftJump, disableJumpTime, disableJump, numberOfJumps, maxJumps, dropSpeed.

Methods: Start(); Update(); Disable(); .ctor().

## PathController

Metadata offset 4916564.

Fields: pathPositions, points, limitPositions, ready, limitDistance, point, startPoint, elpasedDistance, div, lastPosition, showPoints, bones.

Methods: Start(); Stop(); LateUpdate(); .ctor().

## TransformData

Metadata offset 4916640.

Fields: position, rotation.

Methods: .ctor(pos, rot).

## PathManager

Metadata offset 4916716.

Fields: points, limitPositions, lastPosition, distanceBetweenPoints.

Methods: Start(); MoveLastToFirst(pos, rot); LateUpdate(); .ctor().

## TracePath

Metadata offset 4916792.

Fields: children, path.

Methods: Start(); UpdateBonesRot(); GetPathPos(dis); .ctor().

## NetworkAtk

Metadata offset 4916868.

Fields: atk, side.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## ObjectSpawner

Metadata offset 4916944.

Fields: obj, interval, startDelay, target.

Methods: Start(); Update(); Create(); .ctor().

## OnlineCharacter

Metadata offset 4917020.

Fields: position, animation.

Methods: .ctor(position, animation).

## OnlineCharacterDatabase

Metadata offset 4917096.

Fields: hostCharacter.

Methods: Start(); Update(); UpdatePosition(); .ctor().

## OnlineChat

Metadata offset 4917172.

Fields: chatText, input, iconMsg, iconAlertMsg, chatInit, TypingAlert, typingAlertText, yourUsername.

Methods: Start(); SendText(); CloseChat(); Typing(); ShowTypingAlert(CharUsername); HideTypingAlert(); ReciveMSG(msg, opUsername); OpenChat(); Update(); .ctor().

## <>c__DisplayClass30_0

Metadata offset 4917248.

Fields: DBTask.

Methods: .ctor(); <CheckIfHaveFriend>b__0().

## <>c__DisplayClass31_0

Metadata offset 4917324.

Fields: DBTask.

Methods: .ctor(); <CheckIfAlreadyFriends>b__0().

## <>c__DisplayClass32_0

Metadata offset 4917400.

Fields: DBTask.

Methods: .ctor(); <SetOnlineCode>b__0().

## <CheckIfAlreadyFriends>d__31

Metadata offset 4917476.

Fields: <>1__state, <>2__current, friendID, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckIfHaveFriend>d__30

Metadata offset 4917552.

Fields: <>1__state, <>2__current, friendID, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SetOnlineCode>d__32

Metadata offset 4917628.

Fields: <>1__state, <>2__current, code, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## OnlineManager

Metadata offset 4917704.

Fields: i, hostReady, guestReady, regions, OpUsername, OpUserID, alreadyFriends, spectatorCode, rooms.

Methods: Start(); OnRoomListUpdate(roomList); SetGuestReady(stage); OnRegionListReceived(regionHandler); OnDisconnected(cause); SetPlayerScene(scene); ArePlayersSameScene(scene); OnPlayerLeftRoom(otherPlayer); SetRoomPropAsString(propertie, value); SetRoomPropAsInt(propertie, value); SetRoomPropAsFloat(propertie, value); GetRoomPropAsString(propertie); GetRoomPropAsInt(propertie); GetRoomPropAsFloat(propertie); GetPlayerPropAsString(player, propertie); GetPlayerPropAsInt(player, propertie); GetPlayerPropAsFloat(player, propertie); SetPlayerPropAsString(player, propertie, value); SetPlayerPropAsInt(player, propertie, value); SetPlayerPropAsFloat(player, propertie, value); CleanConnection(); CheckIfHaveFriend(friendID); CheckIfAlreadyFriends(friendID); SetOnlineCode(code); IsPlayer1(); IsPlayer2(); IsSpectator(); GetOnlineCode(); GetPlayerNumber(player); .ctor().

## OnlineMovingAnim

Metadata offset 4917780.

Fields: anim, lastPosition, targetHorizontal, targetVertical, smoothSpeed, distance, Limdistance.

Methods: Start(); Update(); SetByLastPosition(); SetAnimSmooth(); SendOnlinePosition(); UpdateOnlinePosition(newPosition); SetOnlinePosition(onlinePos); .ctor().

## OnlineObjectPosition

Metadata offset 4917856.

Fields: .

Methods: Start(); UpdatePosition(sender, args); .ctor().

## OpacityByTouch

Metadata offset 4917932.

Fields: .

Methods: Start(); Update(); Opacity100(); Opacity0(); .ctor().

## PasswordManager_LM

Metadata offset 4918008.

Fields: passwordPopup, currentRoomItem, passwordInputField, roomMessageHandler.

Methods: TryToConnect(roomItem); CheckPassword(); ClosePasswordPopup(); .ctor().

## AppSettingsExtensions

Metadata offset 4918084.

Fields: .

Methods: GetChatSettings(appSettings).

## ChannelSelector

Metadata offset 4918160.

Fields: Channel.

Methods: SetChannel(channel); OnPointerClick(eventData); .ctor().

## ChatAppIdCheckerUI

Metadata offset 4918236.

Fields: Description.

Methods: Update(); .ctor().

## ChatGui

Metadata offset 4918312.

Fields: ChannelsToJoinOnConnect, FriendsList, HistoryLengthToFetch, <UserName>k__BackingField, selectedChannelName, chatClient, chatAppSettings, missingAppIdErrorPanel, ConnectingLabel, ChatPanel, UserIdFormPanel, InputFieldChat, CurrentChannelText, ChannelToggleToInstantiate, FriendListUiItemtoInstantiate, channelToggles, friendListItemLUT, ShowState, Title, StateText, UserIdText, HelpText, TestLength, testBytes.

Methods: get_UserName(); set_UserName(value); Start(); Connect(); OnDestroy(); OnApplicationQuit(); Update(); OnEnterSend(); OnClickSend(); SendChatMessage(inputLine); PostHelpToCurrentChannel(); DebugReturn(level, message); OnConnected(); OnDisconnected(); OnChatStateChange(state); OnSubscribed(channels, results); OnSubscribed(channel, users, properties); InstantiateChannelButton(channelName); InstantiateFriendButton(friendId); OnUnsubscribed(channels); OnGetMessages(channelName, senders, messages); OnPrivateMessage(sender, message, channelName); OnStatusUpdate(user, status, gotMessage, message); OnUserSubscribed(channel, user); OnUserUnsubscribed(channel, user); OnChannelPropertiesChanged(channel, userId, properties); OnUserPropertiesChanged(channel, targetUserId, senderUserId, properties); OnErrorInfo(channel, error, data); AddMessageToSelectedChannel(msg); ShowChannel(channelName); OpenDashboard(); .ctor(); .cctor().

## FriendItem

Metadata offset 4918388.

Fields: NameLabel, StatusLabel, Health.

Methods: set_FriendId(value); get_FriendId(); Awake(); OnFriendStatusUpdate(status, gotMessage, message); .ctor().

## IgnoreUiRaycastWhenInactive

Metadata offset 4918464.

Fields: .

Methods: IsRaycastLocationValid(screenPoint, eventCamera); .ctor().

## NamePickGui

Metadata offset 4918540.

Fields: UserNamePlayerPref, chatNewComponent, idInput.

Methods: Start(); EndEditOnEnter(); StartChat(); .ctor().

## PhotonDestroy

Metadata offset 4918616.

Fields: .

Methods: OnTriggerEnter(other); SendDestroy(); .ctor().

## <PingIP>d__19

Metadata offset 4918692.

Fields: <>1__state, <>2__current, <>4__this, ip, <ping>5__2, <timeout>5__3, <timer>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## PingController

Metadata offset 4918768.

Fields: localIPText, inputIP, inputPort, resultText, tcpConnection, udpConnection.

Methods: Start(); Ping(); TCPConnect(); TCPStart(); TCPSend(); GetTCPMessage(data); GetUDPMessage(data); UDPRestart(); UDPSend(); UDP_BroadCast(); UDPConnect(); CloseConnections(); GetLocalIP(); PingIP(ip); .ctor().

## PingPongMove

Metadata offset 4918844.

Fields: start, finish, duration, destroyTime.

Methods: Start(); FirstMove(); SecondMove(); Update(); .ctor().

## PlaceInBone

Metadata offset 4918920.

Fields: animator, bone, head.

Methods: Start(); .ctor().

## PlayerCheckpointsProgress

Metadata offset 4918996.

Fields: checkpoints, nextCheckpoint, moveSpeed, checkpoint, loser, player, playerName, currentCheckpoint, opponent.

Methods: Start(); SetPosition(); FixedUpdate(); NextCheckpoint(); Lose(); SetOpponent(); .ctor().

## PlayerController

Metadata offset 4919072.

Fields: .

Methods: .ctor().

## <DropAllAttacksCo>d__37

Metadata offset 4919148.

Fields: <>1__state, <>2__current, atk, <dropSpeed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## PlayerController_LM

Metadata offset 4919224.

Fields: playerID, TCPConnection, localData, damageController, attacksController, specialController, scoreController, inmune, playerElement, moveSetIndex, specialIndex, controlledBy, landAtk, isBlocking, hp, maxHp, stuned, weak, awake, playerSpecialCharge, score, onDamage, playerFXList, landFXList, unfrozable.

Methods: Start(); OnDestroy(); MakeItUnfrozable(); RestoreFrozenState(); GetCleanFXList(data); CleanFXList(); SendCleanFX(); Update(); ChangeMoveSet(); ChangeSpecial(); OnTriggerEnter(collider); DropAllAttacks(); DropAllAttacksCo(atk); Reset(); Falling(); RestoreInmune(); .ctor().

## PlayerEvents

Metadata offset 4919300.

Fields: .

Methods: Start(); Update(); .ctor().

## PlayerIcon

Metadata offset 4919376.

Fields: icon.

Methods: Start(); Update(); .ctor().

## PlayerVoice

Metadata offset 4919452.

Fields: .

Methods: Start(); .ctor().

## PlayParticlesOnEnable

Metadata offset 4919528.

Fields: fx.

Methods: OnEnable(); .ctor().

## PlaySoundOnEnable

Metadata offset 4919604.

Fields: sound.

Methods: OnEnable(); .ctor().

## Counter

Metadata offset 4919680.

Fields: value, text, onCounterFinish.

Methods: Awake(); subtractValue(); ChangeCamera(); .ctor().

## <ChangeSize>d__4

Metadata offset 4919756.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeParticleSize

Metadata offset 4919832.

Fields: delay, psMain, actualSize.

Methods: Start(); ChangeSize(); .ctor().

## PrisonController

Metadata offset 4919908.

Fields: moveFinished, trap, playerTrap, walls, delayRevert.

Methods: Awake(); OnDestroy(); DestroyPrison(playerLayer); DestroyPrisonOnline(); DestroyPrisonHit(playerLayer); DestroyAllPrison(); FinishMove(); PrisonTrap(playerLayer); StartMoves(); StartMovesOnline(); RevertWalls(); RevertWallsOnline(); OnlineDestroy(); .ctor().

## <ChargeShieldBar>d__4

Metadata offset 4919984.

Fields: <>1__state, <>2__current, <>4__this, time.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## RadialUIFX

Metadata offset 4920060.

Fields: radialImage.

Methods: Start(); Update(); ShowRadial(time); ChargeShieldBar(time); .ctor().

## RandomRotation

Metadata offset 4920136.

Fields: .

Methods: Start(); Update(); .ctor().

## RCPManager

Metadata offset 4920212.

Fields: i, pv, player1Ready, player2Ready.

Methods: Start(); SetHostReady(OpUsername, OpUserID); SetGuestReady(OpUsername, OpUserID); Player2Ready(); TimeOverRPC(); .ctor().

## re

Metadata offset 4920288.

Fields: .

Methods: .ctor().

## RecoverReduceCollider

Metadata offset 4920364.

Fields: reduce.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## Recover_LM

Metadata offset 4920440.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## ReduceDefense

Metadata offset 4920516.

Fields: .

Methods: OnTriggerEnter(other); .ctor().

## ReduceSpeed

Metadata offset 4920592.

Fields: time, script.

Methods: Start(); Update(); .ctor().

## RefHitCharacter

Metadata offset 4920668.

Fields: hitCharacter, onHitCharacter.

Methods: Start(); OnTriggerEnter(other); .ctor().

## RemoveAsSpectator

Metadata offset 4920744.

Fields: delay.

Methods: Start(); DisableObject(); .ctor().

## RenderInTime

Metadata offset 4920820.

Fields: time, skinnedMeshRenderer.

Methods: Start(); Render(); .ctor().

## ResetOwnerAnim

Metadata offset 4920896.

Fields: limitTime, currentTime.

Methods: Update(); OnDestroy(); .ctor().

## RevertLayer

Metadata offset 4920972.

Fields: delay.

Methods: Start(); Update(); .ctor().

## RoomItem

Metadata offset 4921048.

Fields: roomName_UI, roomName, ip, password, isLocked, lockIcon, passwordManager.

Methods: SetRoomName(); ConnectToHost(); .ctor().

## <ShareCodeToFriend>d__17

Metadata offset 4921124.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## RoomManager

Metadata offset 4921200.

Fields: quickPlayBtn, createPrivateBtn, share, join, code, inputCode, privateCreation, connectionText, photonRoom.

Methods: Start(); ResetUI(); PlayWithFriends(); QuickMatch(); ActivateQuickAndCreatePriv(); JoinRoom(); CreatePrivate(); ShareCode(); ShareCodeToFriend(); FinishedSharing(); .ctor(); <ShareCodeToFriend>b__17_0(result, shareTarget).

## RoomMessageHandler

Metadata offset 4921276.

Fields: UI_Manager, TCPConnection, udpConnection, syncTimers, localMultiplayerData, maxPlayers, currentPlayers, createRoomManager.

Methods: Start(); OnDestroy(); Handler(message); SendRoomMessage(eventCode); ConnectToHost(hostIP); ConnectToGuest(guestIP); GoToCharacterSelection(offsetTimer); Test(); .ctor().

## <RotateAfterDelay>d__4

Metadata offset 4921352.

Fields: <>1__state, <>2__current, <>4__this, <timeElapsed>5__2, <startRotation>5__3, <endRotation>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## RotateByAngle

Metadata offset 4921428.

Fields: targetRotation, duration, delay.

Methods: Start(); RotateAfterDelay(); .ctor().

## <Rotate>d__4

Metadata offset 4921504.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Rotate_LM

Metadata offset 4921580.

Fields: delay, force, increacedFactor.

Methods: Start(); Rotate(); .ctor().

## SBoneGroup

Metadata offset 4921656.

Fields: stiff, drag, bones, colliders.

Methods: Start(); Update(); SetWeight(); .ctor().

## Character

Metadata offset 4921732.

Fields: moveSpeed, rotationSpeed, anim, x, y.

Methods: Start(); FixedUpdate(); .ctor().

## SColliderGroup

Metadata offset 4921808.

Fields: boneGroups, colliders.

Methods: Awake(); .ctor().

## <EndBattleCO>d__12

Metadata offset 4921884.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <PlayTimeOut>d__19

Metadata offset 4921960.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ScoreController_LM

Metadata offset 4922036.

Fields: tcpConnection, localData, animationController, battleFinish.

Methods: Init(); OnDestroy(); Reset(); UpdateScore(); SendScore(player0Score, player1Score); GetScore(data); UpdateScoreUI(player0Score, player1Score); EndBattle(); EndBattleCO(); ShowFinishMenu(); GetWinner(); isDraw(); GetVictoryAnimation(playerID); PlayAndSendTimeOut(); GetTimeOut(data); PlayTimeOut(); .ctor().

## ScoreManager

Metadata offset 4922112.

Fields: .

Methods: Start(); Update(); .ctor().

## PowerLevel

Metadata offset 4922188.

Fields: value__, Lv1, Lv2, Lv3, Indestructible.

Methods: .

## ElementType

Metadata offset 4922264.

Fields: value__, Wind, Water, Earth, Fire, Electricity, Shadow.

Methods: .

## AttackData

Metadata offset 4922340.

Fields: elementType, powerLevel, jumpable, explotion.

Methods: GetDamage(); GetDefense(); .ctor().

## Attack_SO

Metadata offset 4922416.

Fields: originalSpeed, speed.

Methods: .ctor().

## ButtonStyle

Metadata offset 4922492.

Fields: bgColor, bgheight, bgwidth, fontSize, textColor.

Methods: .ctor().

## ActivateAI

Metadata offset 4922568.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## AddToAtkManager_LM

Metadata offset 4922644.

Fields: parentAtkDescription, atkDescription.

Methods: Start(); Update(); .ctor().

## AttacksAI

Metadata offset 4922720.

Fields: value__, none, basic, up, down, left, right, special.

Methods: .

## TestDelegate

Metadata offset 4922796.

Fields: .

Methods: .ctor(object, method); Invoke(); BeginInvoke(callback, object); EndInvoke(result).

## AI

Metadata offset 4922872.

Fields: anim, animPlayer, player, direction, centerPos, Score, easyMove, on, cpuElement, playerElement, HardMode, SpecialDone, Special2Done, modeCpu, moving, callback, onlyJump, blockActive, atkType, Active.

Methods: Start(); NewAI(); Lv1(); Lv2(); Lv3(); RandomAtk(); FastAttack(); FastAttackFloor(); EvadeAttack(); EvadeAttackDelay(); DoInTime(action, start, end); CheckAttacks(); IsOnFront(pos); Move(); Update(); CPUSelection(); SinglePlay(); NormalTourney(); EasyTourney(); ProTourney(); ActivateAI(); DeativateAI(); ManageAI(state); ChangeEasyMove(); ChangeDirection(); IsNearEdge(); CenterSide(); DistanceFromPlayer(); DistanceFromCenter(); IsCenterOnFront(); Legend(); FireVsFire(); FireVsEarth(); FireVsAir(); FireVsWater(); FireVsLight(); EarthVsEarth(); EarthVsFire(); EarthVsAir(); EarthVsWater(); EarthVsLight(); AirVsEarth(); AirVsAir(); AirVsFire(); AirVsWater(); AirVsLight(); WaterVsFire(); WaterVsEarth(); WaterVsAir(); WaterVsWater(); WaterVsLight(); LightVsWater(); LightVsFire(); LightVsEarth(); LightVsAir(); LightVsLight(); Pro(); Hard(); Normal(); Noob(); Easy(); restore(); RandomAtk2(); RandomSideOrDownAtk(); RandomSideOrUpAtk(); RandomSideOrBasic(); RandomAtkButDownAtk(); RandomAtkFront(); RandomAtkStrong(); RandomAtkSide(); RandomBasicAtk(); BasicAtk(); SideAtkL(); SideAtkR(); Jump(); Block(); DeactiveBlock(); BackFlip(); Roll(); DownAtk(); UpAtk(); CPUAwakening(); FinishAwakening(); Special(); HasSpecial(); Special2(); MoveToSide(side, back); MoveFront(); .ctor().

## AirShieldController

Metadata offset 4922948.

Fields: shield, IncreaseSize, DecreaseSize, speed, dspeed, anim.

Methods: Start(); FixedUpdate(); shieldOn(); shieldOff(); .ctor().

## Amon

Metadata offset 4923024.

Fields: amon.

Methods: Start(); OnDestroy(); .ctor().

## AnimationMovement

Metadata offset 4923100.

Fields: moving, horizontal, vertical, PV, netPosition.

Methods: Start(); FixedUpdate(); Move(newPosition); SendBlock(); NetBlock(); StopNetBlock(); goHorizontal(speed); goDiagonal(speed); goVertical(speed); goHorizontal2(speed); goVertical2(speed); stopGo(); DistanceFromPlayer(); .ctor().

## AnimatorStatesManager

Metadata offset 4923176.

Fields: anim, enterTransition, PV, LandAtk, death, awakening.

Methods: Awake(); ChangeAtk(atk); OnlineChangeColorIcon(color, button); Stunned(); AwakeningStart(); Hit(); Falling(); Block(); SetAnimatorParameters(atk, jump, hor, ver); PlayAnimation(animation); SendAnimation(); SumonLandAttack(); NoDelay(); .ctor().

## <>c__DisplayClass16_0

Metadata offset 4923252.

Fields: LoginTask.

Methods: .ctor(); <Login>b__0().

## <>c__DisplayClass17_0

Metadata offset 4923328.

Fields: RegisterTask.

Methods: .ctor(); <Register>b__0().

## <>c__DisplayClass17_1

Metadata offset 4923404.

Fields: ProfileTask.

Methods: .ctor(); <Register>b__1().

## <Login>d__16

Metadata offset 4923480.

Fields: <>1__state, <>2__current, <>4__this, _email, _password, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <Register>d__17

Metadata offset 4923556.

Fields: <>1__state, <>2__current, _username, <>4__this, _email, _password, <>8__1, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## AuthManager

Metadata offset 4923632.

Fields: dependencyStatus, auth, User, emailLoginField, passwordLoginField, warningLoginText, confirmLoginText, usernameRegisterField, emailRegisterField, passwordRegisterField, passwordRegisterVerifyField, warningRegisterText.

Methods: Awake(); InitializeFirebase(); LoginButton(); RegisterButton(); Login(_email, _password); Register(_email, _password, _username); .ctor(); <Awake>b__12_0(task).

## CameraController

Metadata offset 4923708.

Fields: cameraController, transitionSpeed, currentView, views, randomBanner.

Methods: Awake(); Show(character); LateUpdate(); .ctor().

## <ChangeCameraSmooth>d__21

Metadata offset 4923784.

Fields: <>1__state, <>2__current, <>4__this, targetPosition, targetRotation.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## CameraManager

Metadata offset 4923860.

Fields: cameraM, transitionSpeed, transitionDuration, elapsedTime, ViewCameraClose, ViewCameraFar, ViewCamaraRot, ViewWinCamera, ViewWinCamaraRot, SceneCameraPos, SceneCameraRot, SceneCameraPosFar, SceneCameraRotFar.

Methods: Start(); ChangeCameraParent(newParent); ChangeCameraToClose(); ChangeCameraToFar(); ChangeToSceneCamera(); ChangeToSceneCameraClose(); ChangeToSceneCameraFar(); ChangeCameraToWinner(winner); ChangeCameraSmooth(targetPosition, targetRotation); SwitchCamera(); .ctor().

## <ChangeSpeed>d__5

Metadata offset 4923936.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ChangeAtkSpeed

Metadata offset 4924012.

Fields: attack, time, elapsedtime, curve.

Methods: Start(); ChangeSpeed(); .ctor().

## ChangeInSurvival

Metadata offset 4924088.

Fields: changeText.

Methods: Start(); .ctor().

## ChangeTextOnlineMode

Metadata offset 4924164.

Fields: onlineText.

Methods: Start(); Update(); Change(); .ctor().

## CharacterSelection_LM

Metadata offset 4924240.

Fields: TCPConnection, localData, hostReady, guestReady.

Methods: Start(); Init(); OnDestroy(); HandlerMessagesFromGuest(message); HandleMessagesFromHost(payload); OnSceneLoaded(scene, mode); SetUpStartButtonEvent(); TryToStartGame(fromOpponentMessage); SendCharacterSelection(); SetCharacterSelection(); .ctor().

## ChildrenManager

Metadata offset 4924316.

Fields: .

Methods: .ctor().

## CloudVariables

Metadata offset 4924392.

Fields: <Crystals>k__BackingField, <Aura>k__BackingField, <Eliot>k__BackingField, <Ren>k__BackingField, <Rina>k__BackingField.

Methods: get_Crystals(); set_Crystals(value); get_Aura(); set_Aura(value); get_Eliot(); set_Eliot(value); get_Ren(); set_Ren(value); get_Rina(); set_Rina(value); .ctor().

## ColliderController

Metadata offset 4924468.

Fields: boxCollider, smallCollider.

Methods: Start(); reduceCollider(); recoverCollider(); .ctor().

## CopyLocalRotation

Metadata offset 4924544.

Fields: target, fixAngle.

Methods: Start(); Update(); .ctor().

## CPUController

Metadata offset 4924620.

Fields: characters, actualCharacter, nameText.

Methods: Start(); nextCharacter(); previousCharacter(); ChangeCharacter(character); .ctor().

## CreateInBone

Metadata offset 4924696.

Fields: animator, creationPrefab, bonesSelected.

Methods: CreateFxInHands(); CreateFxInLeftHand(fx); CreateFxInRightHand(fx); .ctor().

## CrystalManager

Metadata offset 4924772.

Fields: i, number.

Methods: Start(); OnDestroy(); UpdateCrystals(); .ctor().

## <RecoverStats>d__52

Metadata offset 4924848.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <RestoreShield>d__40

Metadata offset 4924924.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <TryToFreeze>d__53

Metadata offset 4925000.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## DamageOnCollision

Metadata offset 4925076.

Fields: Damage, shieldDamage, specialCharge, anim, PV, _shieldExplotion, unfrozable, unTrap, restoringShield, frontBackRatio, frontRatio, onDamageRecived, onDamageEventExeptions.

Methods: Awake(); OnTriggerEnter(collider); InvokeOnDamageOnline(); Validations(colliderTag); HitAnimation(attackDamage, effect, name, distance, collider); EffectAnimation(effect, name, distance, attackDamage); WeakAnimation(attackDamage); FreezeAnimation(colliderName); RightAtkAnim(attackDamage); LeftAtkAnim(attackDamage); BackAtkAnim(attackDamage); ShakeCamera(magnitude, roughness, fadeInTime, fadeOutTime); RecoverUnTrap(); ShieldBlockAnim(); ShakeCameraOnline(magnitude, roughness, fadeInTime, fadeOutTime); MakeUnfrozable(); MakeFrozable(); DropAllAtks(); PlayHitAnimation(animation); ShieldExplotion(); DestroyShield(); DestroyShadowHandGrab(); blocking(); StopBlocking(); DamageShield(damage); SetShieldbarDamage(); ChangeButton(color, button); RestoreShield(); SetShieldBarOnline(shieldDamage); GetDamage(amount, effect); SetDamageOnline(damage); ResetSpecialAtk(); ResetSpecialFreeze(); RestoreDamage(); ResetDamage(); ResetDamageOnline(player); weak(); ShieldOff(); Recover(); RecoverStats(); TryToFreeze(); .ctor().

## DatabaseManager

Metadata offset 4925152.

Fields: i, _name, _email, databaseName, userID, dbReference, app.

Methods: Awake(); Start(); CreateNewUser(); HandleValueChanged(sender, args); .ctor().

## Description

Metadata offset 4925228.

Fields: Type, element, Damage, Defense, ReleaseBy, effect, evade, evadeNear, evadeClose, speed, category, moveset.

Methods: .ctor().

## DesktopInputs

Metadata offset 4925304.

Fields: speed, anim, PV, horizontal, vertical, fire, jump, Special.

Methods: Start(); Update(); DetectInputs(); Actions(x, z, a, j, s); StopAttacking(); StopJumping(); .ctor().

## DestroyAtkSimple

Metadata offset 4925380.

Fields: explosion.

Methods: OnTriggerEnter(other); DestroyAtk(); .ctor().

## DestroyByDeathZone

Metadata offset 4925456.

Fields: .

Methods: OnTriggerEnter(other); .ctor().

## DestroyByTime

Metadata offset 4925532.

Fields: time, _finalExplotion.

Methods: Start(); FinalAnimation(); ForceDestroy(); PhotonDestroy(); .ctor().

## DestroyOnCollision

Metadata offset 4925608.

Fields: delay, onlySummonExplotion, stopAtkFirst, exceptPlayer, byTheSameObject, explotion.

Methods: Start(); OnTriggerEnter(other); DestroyObject(ignoreSumonOnCollider); DisableShield(); DeleteObj(); PhotonDestroy(); StopAttack(); .ctor().

## DisableCollider

Metadata offset 4925684.

Fields: time, onColliderDisable.

Methods: Start(); Disable(); .ctor().

## DisableParticleCollition

Metadata offset 4925760.

Fields: time.

Methods: Start(); Disable(); Update(); .ctor().

## DistanceCollider

Metadata offset 4925836.

Fields: .

Methods: OnTriggerEnter(other); .ctor().

## DontDestroyOnLoad

Metadata offset 4925912.

Fields: .

Methods: Start(); Update(); .ctor().

## DropAtk

Metadata offset 4925988.

Fields: speed.

Methods: Start(); FixedUpdate(); .ctor().

## DropWhenOwnerWeak

Metadata offset 4926064.

Fields: drop, speed.

Methods: Start(); Update(); CheckOwner(); .ctor().

## EnableCollider

Metadata offset 4926140.

Fields: time.

Methods: Start(); enableCollider(); .ctor().

## Enemy

Metadata offset 4926216.

Fields: enemy.

Methods: Start(); Update(); .ctor().

## ExitMenu

Metadata offset 4926292.

Fields: open.

Methods: OpenMenu(); CloseMenu(); PlayAgain(); MainMenu(); .ctor().

## Mode

Metadata offset 4926368.

Fields: value__, forward, up.

Methods: .

## <AnimateSpeed>d__8

Metadata offset 4926444.

Fields: <>1__state, <>2__current, <>4__this, <elapsedTime>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FallMovement

Metadata offset 4926520.

Fields: speedCurve, maxSpeed, duration, speed, mode.

Methods: Start(); Update(); AnimateSpeed(); .ctor().

## FinalZone

Metadata offset 4926596.

Fields: .

Methods: Start(); Update(); .ctor().

## FinishMenu

Metadata offset 4926672.

Fields: playAgain_btn, wait_btn, rematch_btn, invitationSent, iconChat, chat, SpectatorReady.

Methods: Start(); Update(); exitGame(); OpponentReadyRematch(); SpectatorLoading(); ArePlayersInSelectionScene(); SendInvitation(); Rematch(); reboot(); OpenChat(); .ctor().

## FirebasePushNotifications

Metadata offset 4926748.

Fields: .

Methods: Start(); OnTokenReceived(sender, token); OnMessageReceived(sender, e); .ctor().

## Follow

Metadata offset 4926824.

Fields: speed, originalSpeed, startSpeed, startFaster, startTime, stopAfter, target.

Methods: Start(); RecoverSpeed(); Update(); AngleDir(fwd, targetDir, up); .ctor().

## FollowOpponent

Metadata offset 4926900.

Fields: .

Methods: Start(); Update(); .ctor().

## FollowOwner

Metadata offset 4926976.

Fields: .

Methods: Start(); Update(); .ctor().

## JSONArray`1

Metadata offset 4927052.

Fields: Items.

Methods: .ctor().

## <>c__DisplayClass32_0

Metadata offset 4927128.

Fields: DBTask.

Methods: .ctor(); <SearchFriendDatabase>b__0().

## <>c__DisplayClass33_0

Metadata offset 4927204.

Fields: DBTask.

Methods: .ctor(); <CheckIfHaveFriend>b__0().

## <>c__DisplayClass48_0

Metadata offset 4927280.

Fields: DBTask.

Methods: .ctor(); <SearchFriendsDatabase>b__0().

## <>c__DisplayClass52_0

Metadata offset 4927356.

Fields: DBTask.

Methods: .ctor(); <CheckIfFriendHasOtherInvitation>b__0().

## <>c__DisplayClass55_0

Metadata offset 4927432.

Fields: DBTask.

Methods: .ctor(); <GetOnlineCode>b__0().

## <CheckIfFriendHasOtherInvitation>d__52

Metadata offset 4927508.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CheckIfHaveFriend>d__33

Metadata offset 4927584.

Fields: <>1__state, <>2__current, friendID, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <FillRequestList>d__41

Metadata offset 4927660.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <FillTmFriendsList>d__42

Metadata offset 4927736.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <GetOnlineCode>d__55

Metadata offset 4927812.

Fields: <>1__state, <>2__current, userID, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <InitFriendScene>d__47

Metadata offset 4927888.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <OrderFriendListCo>d__45

Metadata offset 4927964.

Fields: <>1__state, <>2__current, <>4__this, <friendList>5__2, <>7__wrap2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); <>m__Finally1(); <>m__Finally2(); <>m__Finally3(); <>m__Finally4(); <>m__Finally5(); <>m__Finally6(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SearchFriendDatabase>d__32

Metadata offset 4928040.

Fields: <>1__state, <>2__current, <>4__this, friendUsername, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <SearchFriendsDatabase>d__48

Metadata offset 4928116.

Fields: <>1__state, <>2__current, <>4__this, <>8__1.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FriendsController

Metadata offset 4928192.

Fields: i, inputField, alert, friendList_UI, addFriend_UI, friendRequests_UI, loading_UI, friendList_Btn, addFriend_Btn, friendRequests_Btn, invite_Btn, watchBattle_Btn, scroll, friendReqsPanel, friendReqPrefab, tmFriendPanel, tmFriendPrefab, completeFriendsList, requestFriendsList, tmFriendsList, friendSelected, friendReqSelected.

Methods: Awake(); Start(); LoginScreen(); RefreshFriendsList(); SearchFriend(); FriendRequestSent(); QuestionDeleteFriend(); DeleteFriend(); FriendDeleted(); FailToDeleteFriend(); SearchFriendDatabase(friendUsername); CheckIfHaveFriend(friendID); CleanUI(); ShowAddFriendUI(); ShowFriendListUI(); ShowfriendRequestsUI(); ShowLoadingUI(); HideLoadingUI(); AcceptFriendRequest(); FillRequestList(); FillTmFriendsList(); OnFriendSelected(); OrderFriendList(); OrderFriendListCo(); SortFriendListPanel(a, b); InitFriendScene(); SearchFriendsDatabase(); SearchFriendsInJson(json); SendInvitation(); CheckIfFriendHasOtherInvitation(); GoToFriendConnection(); WatchBattle(); GetOnlineCode(userID); .ctor().

## <ConfigSpectator>d__37

Metadata offset 4928268.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <CounterOnline>d__46

Metadata offset 4928344.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <StartCounterWhenTimeServerIs>d__47

Metadata offset 4928420.

Fields: <>1__state, <>2__current, timeStart, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## GameController

Metadata offset 4928496.

Fields: i, resumeButton, counter, playerPrefab, enemyPrefab, hostPlayer, guestPlayer, MainCharacter, OpponentCharacter, multiplayer, mobile, online, counterStarted, draw, SpecialButton, practiceMode, WaitingMsg, WaitingMsgText, battleStarted, pause, opponentReadyToAttack, PV, animMainChar, mobInputsMainChar, notMove, lastAttackButton, uiManager, gameStarted.

Methods: Awake(); IsOutsideStage(position, maxDistance); GetAttackOwner(attackLayer); GetEnemyByOwnerLayer(attackLayer); GetDistancesBetweenPlayers(); MainMenu(); LoadAds(); CreateCharacters_LM(); SetupCharacters(isHost); ConfigSpectator(); OnlineRestart(); Restart(); PrepareBattle(); StartBattle(); CreatePlayer(position); EnableInputs(); ReadyToAttack(); StartCounter(); CounterOnline(); StartCounterWhenTimeServerIs(timeStart); StartCounterNow(timeStart); CompleteMainChar(); IsPlayerWeak(); EnableSpecial(); DisableSpecial(); IsMainChar(layer); EnableMobileInputs(); EnableDesktopInputs(); Attacking(attack); AttackingButton(attackbutton); Special(); Jumping(jump); UpDown(vertical); RightLeft(horizontal); pauseGame(); ResumeGame(); quitGame(); .ctor().

## GameManager

Metadata offset 4928572.

Fields: resumeButton, fps, deltaTime, originalHeight, originalWidth, medResH, medResW, minResH, minResW, qualitySlider, max, med, min, frameCapToogle, fpsText, resText, qualityText, userQualityGraphics, userFrameCap.

Methods: Start(); Update(); SetInitialRes(); setMinRes(); setMedRes(); setOriginalRes(); CapFrameRate(cap); changeQuality(value); toogleSettings(); setInitialFrameCap(); setInitialQualityGraphics(); setResolutions(); restartButtonsColor(); .ctor().

## GoForward

Metadata offset 4928648.

Fields: speed, PV, delay, playerWeak, stopAfterTime, stop.

Methods: Start(); Stop(); RestoreStop(); Update(); IsPlayerIsOk(); .ctor().

## <>c

Metadata offset 4928724.

Fields: <>9, <>9__4_0, <>9__7_0, <>9__7_1, <>9__7_2, <>9__7_3, <>9__7_4, <>9__7_5.

Methods: .cctor(); .ctor(); <Start>b__4_0(initStatus); <RegisterEventHandlers>b__7_0(adValue); <RegisterEventHandlers>b__7_1(); <RegisterEventHandlers>b__7_2(); <RegisterEventHandlers>b__7_3(); <RegisterEventHandlers>b__7_4(); <RegisterEventHandlers>b__7_5(error).

## GoogleAdsManager

Metadata offset 4928800.

Fields: i, skipAd, interstitialAd, rewardedAd.

Methods: Start(); LoadInterstitialAd(); ShowAd(); RegisterEventHandlers(ad); LoadRewardedAd(); ShowRewardedAd(); .ctor(); <LoadInterstitialAd>b__5_0(ad, error); <LoadRewardedAd>b__8_0(ad, error); <ShowRewardedAd>b__9_0(reward).

## GraphicsManager

Metadata offset 4928876.

Fields: i, fps, deltaTime, originalHeight, originalWidth, medResH, medResW, minResH, minResW, max, med, min, frameCapToogle, fpsText, resText, qualityText, userQualityGraphics, userFrameCap, debug, graphicsChange.

Methods: Start(); OnDestroy(); Update(); SetInitialRes(); setMinRes(); setMedRes(); setOriginalRes(); CapFrameRate(cap); toogleSettings(); setInitialFrameCap(); setInitialQualityGraphics(); setResolutions(); restartButtonsColor(); .ctor().

## GuestPlayer

Metadata offset 4928952.

Fields: guestPlayer.

Methods: Start(); OnDestroy(); .ctor().

## HostPlayer

Metadata offset 4929028.

Fields: hostPlayer.

Methods: Start(); OnDestroy(); .ctor().

## IncreaseCollider

Metadata offset 4929104.

Fields: time.

Methods: Start(); IncreaseSize(); .ctor().

## <DecreaseSmooth>d__9

Metadata offset 4929180.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <IncreaseSmooth>d__7

Metadata offset 4929256.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## IncreaseSize

Metadata offset 4929332.

Fields: originalSize, speed, decreaseAfterTime, disableAutoPlay.

Methods: OnEnable(); Increase(delay); Decrease(delay); IncreaseSmooth(); DelayDecrease(); DecreaseSmooth(); .ctor().

## InitialConfig

Metadata offset 4929408.

Fields: .

Methods: Start(); .ctor().

## LightShieldManager

Metadata offset 4929484.

Fields: anim, shield, sound, On, weapon.

Methods: Start(); Update(); shieldOn(); shieldOff(); .ctor().

## LocalNetworkClasses

Metadata offset 4929560.

Fields: .

Methods: Start(); Update(); .ctor().

## GuestMessageType

Metadata offset 4929636.

Fields: value__, CharacterSelected, SceneChange, GuestInputs.

Methods: .

## HostMessageType

Metadata offset 4929712.

Fields: value__, StartGame, SceneChange, StartCounter, CreateCharacters, HostPosition, GuestPosition.

Methods: .

## PlayersPositions

Metadata offset 4929788.

Fields: hostPlayerPos, guestPlayerPos.

Methods: .ctor().

## NetMessage

Metadata offset 4929864.

Fields: netEvent, netEventType, ipSender, data.

Methods: .

## RoomMessageType

Metadata offset 4929940.

Fields: value__, JoinRoom, RoomJoined, RoomFull.

Methods: .

## NetEvent

Metadata offset 4930016.

Fields: value__, Room, Host, Guest.

Methods: .

## NetFastEvent

Metadata offset 4930092.

Fields: value__, HostPosition, RoomData, GuestInputs, GuestPosition, StartCounter, Timer, JoinRoom, RoomJoined, RoomFull, SceneChange, GuestCharacterSelected, HostCharacterSelected, LagPulse, SecondLagPulse, LagData, Animation, PlayersPositions, T1, T2, T3, T4, offsetTimer, Attack, DestroyAtk, CreateFX, Unblock, PlayerHp, AskForRoom, SpecialCharge, Score, DrainSpecial, PlayAgain, TimeOut, ShieldBehavior, TCPTest, UDPTest, TCPHostTest, TCPGuestTest, UDPHostTest, UDPGuestTest, GoToCharacterSelection, SyncTimers, CleanPlayerFXs, CloseConnection, CloseEarthPrison, CreateEarthPrison, BreakEarthPrison.

Methods: .

## NetAnimation

Metadata offset 4930168.

Fields: value__, Movement, WaterMovement, AirMovement, EarthMovement, FireMovement, Roll, BackFlip, AirJumpL, AirJumpR, AirBlock, AirBlockHit, AirBasicAttack1, AirBasicAttack2, AirRightAttack1, AirRightAttack2, AirLeftAttack1, AirLeftAttack2, AirDownAttack1, AirDownAttack2, AirUpAttack1, AirUpAttack2, WaterJumpL, WaterJumpR, WaterBlock, WaterBlockHit, WaterBasicAttack1, WaterBasicAttack2, WaterRightAttack1, WaterRightAttack2, WaterLeftAttack1, WaterLeftAttack2, WaterDownAttack1, WaterDownAttack2, WaterUpAttack1, WaterUpAttack2, WaterSpecial, EarthJumpL, EarthJumpR, EarthBlock, EarthBlockHit, EarthBasicAttack1, EarthBasicAttack2, EarthRightAttack1, EarthRightAttack2, EarthLeftAttack1, EarthLeftAttack2, EarthDownAttack1, EarthDownAttack2, EarthUpAttack1, EarthUpAttack2, EarthSpecial, Hit0, Hit1, Hit2, Hit3, BackHit, RightHit, LeftHit, RightHit2, LeftHit2, Fall, Land, Awakening, AirSpecial, SoraVictory, AuraVictory, VineaVictory, EliotVictory, DianaVictory, RenVictory, HitFlip, Freeze, EarthPrisonFree.

Methods: .

## JumpType

Metadata offset 4930244.

Fields: value__, Left, Right, Forward, Backward.

Methods: .

## NetPosition

Metadata offset 4930320.

Fields: e, pos.

Methods: .

## PlayerElement

Metadata offset 4930396.

Fields: value__, Fire, Water, Earth, Air.

Methods: .

## AtkName

Metadata offset 4930472.

Fields: value__, BasicAirAtk, BasicAirAtk2, UpAirAtk, UpAirAtk2, DownAirAtk, DownAirAtk2, RightAirAtk, RightAirAtk2, LeftAirAtk, LeftAirAtk2, BasicWaterAtk, BasicWaterAtk2, UpWaterAtk, UpWaterAtk2a, UpWaterAtk2b, DownWaterAtk, DownWaterAtk2, RightWaterAtk, RightWaterAtk2, LeftWaterAtk, LeftWaterAtk2, LandAirAtk, LandWaterAtk, LandEarthAtk, BasicEarthAtk, BasicEarthAtk2, UpEarthAtk, UpEarthAtk2, DownEarthAtk, DownEarthAtk2, LeftEarthAtk, LeftEarthAtk2, RightEarthAtk, RightEarthAtk2, EarthSpecial_LM, SpecialAirAtkLow, SpecialAirTwisterAtk, SpecialWaterAtk1, SpecialWaterAtk2, SpecialWaterAtk3, SpecialWaterAtk4, SpecialWaterAtk5.

Methods: .

## FxName

Metadata offset 4930548.

Fields: value__, AirExplosion, AirExplosionLong, AirExplosionBig, IceExplotionSmall, WaterExplotionSmall, WaterExplotionShieldRight, WaterExplotionShieldLeft, BigWaterExplotion, WaterExplotion, IceExplotion, IceShieldExplotion, BigRockExplotion_LM, RockExplotion_LM.

Methods: .

## <>c__DisplayClass27_0

Metadata offset 4930624.

Fields: LoginTask.

Methods: .ctor(); <Login>b__0().

## <>c__DisplayClass27_1

Metadata offset 4930700.

Fields: logged.

Methods: .ctor(); <Login>b__1(status).

## <>c__DisplayClass28_0

Metadata offset 4930776.

Fields: RegisterTask.

Methods: .ctor(); <Register>b__0().

## <>c__DisplayClass28_1

Metadata offset 4930852.

Fields: EmailTask.

Methods: .ctor(); <Register>b__1().

## <>c__DisplayClass29_0

Metadata offset 4930928.

Fields: LoginTask.

Methods: .ctor(); <ResetPassword>b__0().

## <>c__DisplayClass30_0

Metadata offset 4931004.

Fields: DBTask.

Methods: .ctor(); <CheckUsernameDatabase>b__0().

## <>c__DisplayClass31_0

Metadata offset 4931080.

Fields: DBTaskUsernames.

Methods: .ctor(); <UpdateUsernameDatabase>b__0().

## <>c__DisplayClass31_1

Metadata offset 4931156.

Fields: DBTask.

Methods: .ctor(); <UpdateUsernameDatabase>b__1().

## <CheckUsernameDatabase>d__30

Metadata offset 4931232.

Fields: <>1__state, <>2__current, _username, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <Login>d__27

Metadata offset 4931308.

Fields: <>1__state, <>2__current, _email, _password, <>8__1, <>4__this, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <Register>d__28

Metadata offset 4931384.

Fields: <>1__state, <>2__current, <>4__this, _email, _password, <>8__1, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <ResetPassword>d__29

Metadata offset 4931460.

Fields: <>1__state, <>2__current, _email, <>8__1, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdateUsernameDatabase>d__31

Metadata offset 4931536.

Fields: <>1__state, <>2__current, _username, <>8__1, <>4__this, <>8__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## FirebaseManager

Metadata offset 4931612.

Fields: i, emailLoginField, passwordLoginField, warningLoginText, confirmLoginText, usernameRegisterField, emailRegisterField, passwordRegisterField, passwordRegisterVerifyField, warningRegisterText, emailResetPassField, warningResetPassText, usernameField, xpField, killsField, deathsField, scoreElement, scoreboardContent.

Methods: Awake(); ClearLoginFeilds(); ClearRegisterFeilds(); LoginButton(); GoToFriendsScene(); RegisterButton(); ResetPasswordButton(); SignOutButton(); SaveDataButton(); Login(_email, _password); Register(_email, _password); ResetPassword(_email); CheckUsernameDatabase(_username); UpdateUsernameDatabase(_username); FormatString(); GoToPatreon(); QuestionLoad(); .ctor().

## ScoreElement

Metadata offset 4931688.

Fields: usernameText, killsText, deathsText, xpText.

Methods: NewScoreElement(_username, _kills, _deaths, _xp); .ctor().

## UIManager

Metadata offset 4931764.

Fields: instance, loginUI, registerUI, resetPasswordUI, userDataUI, scoreboardUI, finalScreenUI, LoadingScreenUI, welcomeText, patreonText.

Methods: Awake(); Start(); ClearScreen(); LoadingScreen(); LoginScreen(); RegisterScreen(); UserDataScreen(); ScoreboardScreen(); ResetPasswordScreen(); FinalScreen(); MainMenu(); .ctor().

## LookAt

Metadata offset 4931840.

Fields: target.

Methods: Start(); Setup(); LateUpdate(); .ctor().

## MenuController

Metadata offset 4931916.

Fields: .

Methods: Start(); Update(); .ctor().

## MicTest

Metadata offset 4931992.

Fields: .

Methods: Start(); .ctor().

## MobileInputs

Metadata offset 4932068.

Fields: joystick, anim, speed, PV, x, z.

Methods: Start(); Update(); DetectInputs(); .ctor().

## Intatiate

Metadata offset 4932144.

Fields: buttonStyle, button, text.

Methods: Start(); .ctor().

## MoveAttack

Metadata offset 4932220.

Fields: speed, speedFistMove, ready, playerWeak, dir, timeToStart, moving, StartPosition, EndPosition, EndPosition2, changeDirectionAtStart, startRotation.

Methods: Start(); FixedUpdate(); StartMov(); ChangeDirection(); .ctor().

## MoveBigRock

Metadata offset 4932296.

Fields: speed, ready.

Methods: FixedUpdate(); .ctor().

## MoveGroupRocks

Metadata offset 4932372.

Fields: .

Methods: Start(); Update(); .ctor().

## MoveLight

Metadata offset 4932448.

Fields: speed, ready, playerWeak, dir, timeToStart, moving, position, GoToOpponent.

Methods: Start(); FixedUpdate(); StartMov(); ChangeDirection(); .ctor().

## MoveObject

Metadata offset 4932524.

Fields: AddVector, speed, positionEnd, moveFinished, delay, finishMove, moveForward, moveRight, moveUp.

Methods: Start(); FixedUpdate(); StartMoving(); Finish(); .ctor().

## MoveObjectLocal

Metadata offset 4932600.

Fields: AddVector, speed, positionEnd, moveFinished, delay, finishMove.

Methods: Start(); Update(); StartMoving(); Finish(); .ctor().

## <MoveOverTime>d__7

Metadata offset 4932676.

Fields: <>1__state, <>2__current, <>4__this, <timeElapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## MoveObjectLocal2

Metadata offset 4932752.

Fields: AddVector, duration, startPosition, positionEnd, delay.

Methods: Start(); StartMoving(); MoveOverTime(); .ctor().

## MoveRock

Metadata offset 4932828.

Fields: speed, sumonSpeed, ready, rotateY, rotateZ, rotateX, playerWeak, dir.

Methods: FixedUpdate(); ChangeDirection(); IsPlayerIsOk(); .ctor().

## MoveSpecialRock

Metadata offset 4932904.

Fields: delay, speed, ready, soundPlayed, dir.

Methods: Start(); FixedUpdate(); ChangeDirection(); StartMoving(); .ctor().

## <MoveObjectToTarget>d__9

Metadata offset 4932980.

Fields: <>1__state, <>2__current, <>4__this, <elapsedTime>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## MoveToTarget

Metadata offset 4933056.

Fields: moveDuration, xCurve, yCurve, zCurve, YForce, XForce, ZForce, startPosition.

Methods: Start(); MoveObjectToTarget(); .ctor().

## MoveWater

Metadata offset 4933132.

Fields: speed, ready, playerWeak, dir, timeToStart, moving, position, positionV, positionZ, changeDirectionAtStart.

Methods: Start(); Update(); StartMov(); ChangeDirection(); IsPlayerIsOk(); .ctor().

## MoveWithKeys

Metadata offset 4933208.

Fields: horizontal, vertical, fire, jump, Special, speed.

Methods: FixedUpdate(); .ctor().

## NAudioPlayer

Metadata offset 4933284.

Fields: .

Methods: FromMp3Data(data); AudioMemStream(waveStream).

## WAV

Metadata offset 4933360.

Fields: <LeftChannel>k__BackingField, <RightChannel>k__BackingField, <StereoChannel>k__BackingField, <ChannelCount>k__BackingField, <SampleCount>k__BackingField, <Frequency>k__BackingField.

Methods: bytesToFloat(firstByte, secondByte); bytesToInt(bytes, offset); get_LeftChannel(); set_LeftChannel(value); get_RightChannel(); set_RightChannel(value); get_StereoChannel(); set_StereoChannel(value); get_ChannelCount(); set_ChannelCount(value); get_SampleCount(); set_SampleCount(value); get_Frequency(); set_Frequency(value); .ctor(wav); ToString().

## NextScene

Metadata offset 4933436.

Fields: sceneName, delay, time.

Methods: Update(); .ctor().

## NormalMode

Metadata offset 4933512.

Fields: resumeButton, counter, playerPrefab, enemyPrefab, player, enemy, multiplayer, mobile, online.

Methods: Start(); FixedUpdate(); Attacking(attack); Jumping(jump); pauseGame(); ResumeGame(); quitGame(); .ctor().

## YesCallback

Metadata offset 4933588.

Fields: .

Methods: .ctor(object, method); Invoke(); BeginInvoke(callback, object); EndInvoke(result).

## Notifications

Metadata offset 4933664.

Fields: i, yesCallback, barAnim, barText, barBG, popup, popupText, okBtn, yesBtn, noBtn.

Methods: Awake(); Log(notification, time, type); OpenQuestionPopup(text, callback); Yes(); OpenPopup(text); ClosePopup(); Remove(); SetColor(type); .ctor().

## OnlineCustomProperties

Metadata offset 4933740.

Fields: .

Methods: CharacterSelected(); AreCharactersSelected(); PlayerReadyToBattle(); ArePlayersReady(); CleanCustomProperties(); SetCounter(); SetStage(stage); SetPlayerScene(scene); ArePlayersSameScene(scene); .ctor().

## OnPlayerDisconnect

Metadata offset 4933816.

Fields: .

Methods: Start(); Update(); OnDisconnected(cause); OnPlayerDisconnected(player); PlayerDisconnected(); .ctor().

## AnimatorNetwork

Metadata offset 4933892.

Fields: .

Methods: Start(); .ctor().

## AttackNetworkConfig

Metadata offset 4933968.

Fields: PV.

Methods: Start(); OnPhotonSerializeView(stream, info); .ctor().

## ChatManager

Metadata offset 4934044.

Fields: chatclient, userID.

Methods: DebugReturn(level, message); OnChatStateChange(state); OnConnected(); OnDisconnected(); OnGetMessages(channelName, senders, messages); OnPrivateMessage(sender, message, channelName); OnStatusUpdate(user, status, gotMessage, message); OnSubscribed(channels, results); OnUnsubscribed(channels); OnUserSubscribed(channel, user); OnUserUnsubscribed(channel, user); Start(); Update(); .ctor().

## ConfigAttackLayer

Metadata offset 4934120.

Fields: playerWeak.

Methods: Awake(); DestroyAttack(); Update(); .ctor().

## ConfigGuest

Metadata offset 4934196.

Fields: PV, start.

Methods: Start(); FixedUpdate(); CreatePlayer(); .ctor().

## ConfigHost

Metadata offset 4934272.

Fields: PV, start.

Methods: Start(); CreatePlayer(); .ctor().

## <DestroyNetObject>d__4

Metadata offset 4934348.

Fields: <>1__state, <>2__current, time, <>4__this, gameObj.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## DestroyNetObjByTime

Metadata offset 4934424.

Fields: time, _finalExplotion.

Methods: Start(); FinalAnimation(); DestroyNetObject(gameObj, time); .ctor().

## Disconnet

Metadata offset 4934500.

Fields: .

Methods: Start(); Update(); DisconnectOtherPlayer(); .ctor().

## Guest

Metadata offset 4934576.

Fields: guest, guestReady.

Methods: Start(); OnDestroy(); SetGuestReady(); .ctor().

## Host

Metadata offset 4934652.

Fields: host, hostReady.

Methods: Start(); OnDestroy(); .ctor().

## MasterInputs

Metadata offset 4934728.

Fields: horizontal, vertical, fire, jump, PV.

Methods: Start(); FixedUpdate(); Attack(); .ctor().

## NetworkEnemy

Metadata offset 4934804.

Fields: PV.

Methods: Start(); Update(); OnSceneFinishLoading(scene, mode); Create(); .ctor().

## NetworkPlayer

Metadata offset 4934880.

Fields: PV.

Methods: Start(); CreatePlayers(); .ctor().

## ObservableAnimator

Metadata offset 4934956.

Fields: anim, newHorizontal, newVertical, sHorizontal, sVertical, lastPosition, newPosition, target, dir, distance, newDistance, lastDistance.

Methods: Start(); Update(); GetTarget(); GetAnimation(); Smooth(); AngleDir(fwd, targetDir, up); .ctor().

## PhotonRoom

Metadata offset 4935032.

Fields: customProperties, roomManager, regionText, connectionText, regionChanged.

Methods: Awake(); OnEnable(); OnDisable(); OnConnectedToMaster(); DelayJoinRoom(); OnJoinRandomFailed(returnCode, message); CreateRoom(); OnCreateRoomFailed(returnCode, message); OnJoinedRoom(); StartAsSpectator(); OnJoinRoomFailed(returnCode, message); OnPlayerEnteredRoom(newPlayer); OnCreatedRoom(); .ctor().

## PingPhoton

Metadata offset 4935108.

Fields: ping.

Methods: Start(); Update(); CheckPing(); SlowGame(time); .ctor().

## RegionsDropdown

Metadata offset 4935184.

Fields: dropdown, regions, roomManager, connectingText.

Methods: Start(); ConnectToRegion(regionIndex); .ctor().

## <DropAtks>d__21

Metadata offset 4935260.

Fields: <>1__state, <>2__current, time, winner.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <UpdateScore>d__7

Metadata offset 4935336.

Fields: <>1__state, <>2__current, delay, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ScoreController

Metadata offset 4935412.

Fields: score, anim, PV, newRecord.

Methods: Start(); OnTriggerEnter(other); AddPoints(); UpdateScore(delay); Draw(); DanceSurvival(); Dance(winner); TimeOver(); EndBattle(winner); CheckPoints(); FoeWin(); finishMenu(); DisableSpecialButton(); showWinnerText(character); changeCamera(winner); changeMusic(); disableMovement(player); DropAtks(time, winner); Falling(); Death(); FinalDeath(); DelayWin(); .ctor().

## SendInputsToMaster

Metadata offset 4935488.

Fields: horizontal, vertical, fire, jump, x, z, a, j, xr, zr, ar, jr, PV.

Methods: Start(); FixedUpdate(); OnPhotonSerializeView(stream, info); .ctor().

## SetNetworkPosition

Metadata offset 4935564.

Fields: PV, netPosition.

Methods: Start(); Update(); CorrectPosition(newPosition); .ctor().

## StartCounter

Metadata offset 4935640.

Fields: .

Methods: .ctor().

## <Play>d__7

Metadata offset 4935716.

Fields: <>1__state, <>2__current, <>4__this, audio.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## PlayAudios

Metadata offset 4935792.

Fields: audios, startTimes, listNumber, actualAudio.

Methods: Start(); Update(); PlayNext(); Play(audio); .ctor().

## PlayerStats

Metadata offset 4935868.

Fields: i, lifes, defense, lifesText, defenseText.

Methods: Start(); RestoreStats(); RefreshText(); LoseLife(lifesToLose); LoseDefense(defenseToLose); .ctor().

## RateGame

Metadata offset 4935944.

Fields: .

Methods: Start(); OpenRatePopup(); CloseRatePopup(); RateItNow(); NoRate(); RemindLater(); .ctor().

## Recover

Metadata offset 4936020.

Fields: keepStunned, keepWeak, keepBlocking, keepColliderSmall.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## RemoveGravity

Metadata offset 4936096.

Fields: .

Methods: Start(); Update(); Remove(); .ctor().

## RemoveInSurvival

Metadata offset 4936172.

Fields: .

Methods: Start(); Update(); .ctor().

## Respawn

Metadata offset 4936248.

Fields: .

Methods: respawn(); CreateNewChar(); Update(); .ctor().

## RestorePurchases

Metadata offset 4936324.

Fields: .

Methods: .ctor().

## ResultsController

Metadata offset 4936400.

Fields: resultsController, rank, crystalsView, bonus, crystalViewBattle, crystalsReward, playAgainButton.

Methods: Awake(); OnDestroy(); InitResults(); DisplayBonus(); CalculateCrystalReward(); CalculateRank(); ShowRewardedAd(); .ctor().

## Rina

Metadata offset 4936476.

Fields: rina.

Methods: Start(); OnDestroy(); Update(); .ctor().

## Rotate

Metadata offset 4936552.

Fields: delay, start, speed.

Methods: Start(); Init(); FixedUpdate(); .ctor().

## RotateAxis

Metadata offset 4936628.

Fields: axis, angle.

Methods: Start(); Update(); .ctor().

## RotateByMove

Metadata offset 4936704.

Fields: targetMovement, limitAngle, x_fix, z_fix, minSpeed, maxSpeed, transitionSpeed, invertLimit, bodyLimit, bodyAngleLimit, targetRot, lastPosition, dir, x, y, z, mag, targetMag.

Methods: Update(); GetMovement(); SetLimit(); .ctor().

## RotateByMovement

Metadata offset 4936780.

Fields: OGangleLimit, OGSpeed, angleLimit, speed, iddle, lastPosition, dir, targetRot, mag, min, max, originalLocalRot, speedTransition, SpineBone, spineLimit, bodyLimit, bodyLimitAngle, bodyCollitionAngle, spineAngle, x, targetX, z.

Methods: Start(); Update(); GetMovement(); CheckBodyCollition(); Rotate(); WrapAngle(angle); .ctor().

## RotateSelf

Metadata offset 4936856.

Fields: y, x, z, rotate, delay.

Methods: Start(); FixedUpdate(); StartRotation(); .ctor().

## RotateToOpponent

Metadata offset 4936932.

Fields: target.

Methods: Start(); Update(); .ctor().

## RotBone

Metadata offset 4937008.

Fields: child, lastTipPos, dirPosToChild, dirPosToLastChild, rot, debug, radius, originalLocalRot, speed, originalDir.

Methods: Start(); Update(); Move(); GetRotationToChild(); OnDrawGizmos(); .ctor().

## RotPart

Metadata offset 4937084.

Fields: target, speed, limitAngle, setValueByParent, limitXAxis, angle, velocity, targetLastPosition, targetAngleH, targetAngleV, targetRotation, originalRotation, limitMag.

Methods: Awake(); OnEnable(); Update(); SetSpeed(); SetParentValues(); GetTargetMovement(); AngleDir(fwd, targetDir, up); .ctor().

## SaveManager

Metadata offset 4937160.

Fields: .

Methods: .

## <OnlineTimer>d__19

Metadata offset 4937236.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <ReduceTime>d__16

Metadata offset 4937312.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <TimeUp>d__17

Metadata offset 4937388.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <TimerBattleFinish>d__18

Metadata offset 4937464.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Score

Metadata offset 4937540.

Fields: score, finishText, battleFinish, finishMenu, playerPoints, enemyPoints, p1Score, p2Score, dash, timeText, time, timeBG, alertColor, chat, onTimeOut.

Methods: Awake(); ReduceTime(); TimeUp(); TimerBattleFinish(); OnlineTimer(); UpdateScore(); UpdateSpectatorScore(); CheckSpectatorScore(); .ctor().

## SelectionController

Metadata offset 4937616.

Fields: i, actualCharacterIndex, actualCharacterName, nameText, buttonText, hostReady, guestReady, inBattle, unlockName, UnlockScreen, beforeArrow, afterArrow, crystalsView, trophiesView, beforeArrowStage, afterArrowStage, beforeArrowSpeed, afterArrowSpeed, ready, Price, PriceTrophy, Characters, cpuController, comingSoonScreen, trophyRequirement, crystalsPrice, timeText, times, actualTime, onStart, charNotAvialable.

Methods: Awake(); SetOnlineCode(); ChangeTime(); SetTime(); Start(); OnDestroy(); ResetPlayerProperties(); RefreshCharsAndStages(); SetDefaultCharacters(); nextCharacter(); previousCharacter(); ChangeCharacter(character); RemoveButtons(); OnlineMode(); start(); GoToBattle(); CheckIfLocked(); UnlockCharacter(); UnlockTrophyCharacter(); Unlock(); UnlockAnim(); .ctor().

## SetSkyBox

Metadata offset 4937692.

Fields: skybox.

Methods: Awake(); Update(); .ctor().

## SetTypePlayer

Metadata offset 4937768.

Fields: PV.

Methods: Start(); SetType(); .ctor().

## ShakeCamera

Metadata offset 4937844.

Fields: magnitude, roughness, fadeInTime, fadeOutTime.

Methods: Start(); Update(); .ctor().

## <ShareCodeToFriend>d__5

Metadata offset 4937920.

Fields: <>1__state, <>2__current, code, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <TakeScreenshotAndShare>d__4

Metadata offset 4937996.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## SharingManager

Metadata offset 4938072.

Fields: msg.

Methods: Start(); Share(); ShareCode(code); TakeScreenshotAndShare(); ShareCodeToFriend(code); FinishedSharing(); .ctor(); <TakeScreenshotAndShare>b__4_0(result, shareTarget); <ShareCodeToFriend>b__5_0(result, shareTarget).

## ShieldController

Metadata offset 4938148.

Fields: anim.

Methods: Start(); Update(); shieldOn(); shieldOff(); .ctor().

## ShieldMannager

Metadata offset 4938224.

Fields: anim, shield, sound, On.

Methods: Start(); Update(); shieldOn(); shieldOff(); .ctor().

## ShowInFirstPerson

Metadata offset 4938300.

Fields: originalSize.

Methods: Start(); CheckEnemy(); Update(); .ctor().

## ShowInOfflineMode

Metadata offset 4938376.

Fields: onlyOnTourney.

Methods: Start(); Update(); Config(); .ctor().

## ShowInOnlineMode

Metadata offset 4938452.

Fields: .

Methods: Start(); .ctor().

## ShowInSurvival

Metadata offset 4938528.

Fields: .

Methods: Start(); Update(); .ctor().

## ShowOnlyOnEditor

Metadata offset 4938604.

Fields: .

Methods: Start(); Update(); .ctor().

## SideCameraPlayer

Metadata offset 4938680.

Fields: originalPosition, currentView, speed, goToOriginal.

Methods: Start(); Update(); LateUpdate(); StopGoCamera(); StopGo(); GoToLeft(); GoToRight(); .ctor().

## SimpleMove

Metadata offset 4938756.

Fields: speed, dir, delay, moving.

Methods: Start(); StartMoving(); Update(); .ctor().

## SimpleSpringBone

Metadata offset 4938832.

Fields: child, gravity, stiff, boneAxis, radius, stiffnessForce, dragForce, colliders, debug, springLength, localRotation, currTipPos, prevTipPos.

Methods: Awake(); Start(); Update(); OnDrawGizmos(); .ctor().

## SlowMo

Metadata offset 4938908.

Fields: anim.

Methods: Start(); FixedUpdate(); OnTriggerEnter(other); slowMoOff(); .ctor().

## SocialMedia

Metadata offset 4938984.

Fields: .

Methods: GoToPatreon(); GoToYouTube(); GoToDiscord(); GoToInstagram(); .ctor().

## <DechargeSpecial>d__20

Metadata offset 4939060.

Fields: <>1__state, <>2__current, <>4__this, time.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <DechargeSpecialLM>d__19

Metadata offset 4939136.

Fields: <>1__state, <>2__current, <>4__this, time.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## SpecialAtk

Metadata offset 4939212.

Fields: specialAtk, charge, newCharge, lastCharge, speed, letter, circle, chargeImg, sYellow, sBlack, charged, soundPlayed, numberOfSpecial, decharging.

Methods: Start(); OnDestroy(); FixedUpdate(); ChargeSpecial(val); ResetCharge(); DechargeSpecialLM(time); DechargeSpecial(time); FinishEnemyAwakening(); .ctor().

## SpringBone

Metadata offset 4939288.

Fields: child, boneAxis, radius, isUseEachBoneForceSettings, stiffnessForce, dragForce, springForce, colliders, debug, threshold, springLength, localRotation, trs, currTipPos, prevTipPos, org, managerRef, spd.

Methods: Awake(); GetParentSpringManager(t); Start(); UpdateSpring(); OnDrawGizmos(); .ctor().

## SpringCollider

Metadata offset 4939364.

Fields: radius.

Methods: OnDrawGizmosSelected(); .ctor().

## SpringManager

Metadata offset 4939440.

Fields: dynamicRatio, stiffnessForce, stiffnessCurve, dragForce, dragCurve, springBones.

Methods: Start(); Update(); LateUpdate(); UpdateParameters(); UpdateParameter(fieldName, baseValue, curve); .ctor().

## SSpringBone

Metadata offset 4939516.

Fields: stiff, drag, boneAxis, radius, colliders, debug, localRotation, currTipPos, prevTipPos, aimVector, springLength.

Methods: Awake(); Start(); Update(); OnDrawGizmos(); .ctor().

## StageController

Metadata offset 4939592.

Fields: i, stages, stage, music.

Methods: Awake(); .ctor().

## StageSelector

Metadata offset 4939668.

Fields: stageSelector, stageImage, unlockButton.

Methods: Awake(); OnDestroy(); nextStage(); previousStage(); ChangeStage(); CheckStage(); .ctor().

## StartRotation

Metadata offset 4939744.

Fields: initialRotation, rot, delay.

Methods: Start(); Rotate(); FixedUpdate(); .ctor().

## StopBlocking

Metadata offset 4939820.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); OnStateExit(animator, stateInfo, layerIndex); .ctor().

## StopOnCollision

Metadata offset 4939896.

Fields: Stop, Tag.

Methods: OnTriggerEnter(other); .ctor().

## StopParticles

Metadata offset 4939972.

Fields: time.

Methods: Start(); Update(); Stop(); .ctor().

## Stunned

Metadata offset 4940048.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## SummonAttack

Metadata offset 4940124.

Fields: follow, PV, newMat, jumpAtk.

Methods: Start(); ChangeColor(obj); Create(creation); CreateAsChild(creation); CreateAwakeAtk(creation); CreateAwakeAtkNet(creation); RestoreJumpAtk(); CreateNetworkAttack(creation); CreateSimpleAtk(creation); CreateBranchAttack(creation); CreateAttack(creation); WaterShieldExists(creation); CreateNetworkKickAttack(creation); CreateKickAttack(creation); .ctor().

## SummonOnCollision

Metadata offset 4940200.

Fields: Create, SummonOnCollider, ForceNetworkSumon, PV.

Methods: Start(); OnTriggerEnter(collider); ChangeColor(obj, newMat); Summon(collider); IsFireball(); NetworkSummon(ignoreSumonOnCollider); .ctor().

## SurvivalController

Metadata offset 4940276.

Fields: survivalController, survivalMode, currentChar, disableRespawn.

Methods: Start(); OnDestroy(); Update(); SurvivalModeOn(); RespawnOff(); RespawnOn(); .ctor().

## SurvivalRecord

Metadata offset 4940352.

Fields: recordValue.

Methods: Start(); Update(); .ctor().

## SurvivalView

Metadata offset 4940428.

Fields: off, changeText.

Methods: Start(); Update(); .ctor().

## PlayerType

Metadata offset 4940504.

Fields: value__, Host, Guest.

Methods: .

## GameMode

Metadata offset 4940580.

Fields: value__, SinglePlayer, Survival, Tournament, LocalMultiplayer, Online, StartMenu, Lobby, FriendsMenu, AccountMenu, Spectator.

Methods: .

## <NextScene>d__14

Metadata offset 4940656.

Fields: <>1__state, <>2__current, delay.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## SystemManager

Metadata offset 4940732.

Fields: i, timesPlayed, mode, async, unlock, newUpdate, bonus, tmPlayer, playedTime, howToPlayShowed, playerType.

Methods: Awake(); ChangePlayingMode(playingMode); RefreshPlayingMode(); NextScene(delay); SaveData(data); SaveDataInDevice(); LoadDataFromDevice(); SavePlayerData(player); LoadPlayerData(); Update(); CheckForUpdate(); CheckBonus(); OnApplicationQuit(); .ctor().

## TagInChildren

Metadata offset 4940808.

Fields: .

Methods: FindGameObjectInChildWithTag(parent, tag); .ctor().

## <>c__DisplayClass11_0

Metadata offset 4940884.

Fields: client, <>4__this.

Methods: .ctor(); <ListenForClients>b__0().

## TCPConnectionManager

Metadata offset 4940960.

Fields: tcpListener, tcpClient, stream, listenerThread, clientThread, messageQueue, onMessageReceived, port.

Methods: Start(); Update(); StartServer(); ListenForClients(); HandleClient(client); ConnectToServer(serverIp); ReceiveMessages(); SendNetMessage(message); OnDestroy(); .ctor().

## TestFunction

Metadata offset 4941036.

Fields: functions, functions2.

Methods: Callfunctions(); Callfunctions2(); .ctor().

## TMPlayer

Metadata offset 4941112.

Fields: userID, lastSessionDate, lastDevice, playedTime, username, crystals, trophies, survivalRecord, characters, stages, playingMode, tmPremium.

Methods: .ctor().

## TMPlayerCharacters

Metadata offset 4941188.

Fields: aura, eliot, ren, rina, dante, agata, raven, maya.

Methods: .ctor(); .ctor(aura, eliot, ren, rina, dante, agata, raven, maya).

## TMPlayerStages

Metadata offset 4941264.

Fields: stage1, stage2, stage3, stage4, stage5, stage6, stage7.

Methods: .ctor(stage1, stage2, stage3, stage4, stage5, stage6, stage7); .ctor().

## TMFriend

Metadata offset 4941340.

Fields: username, friendID, accepted.

Methods: .ctor().

## TMInvitation

Metadata offset 4941416.

Fields: invitationTime, receiverType, hostID, guestID, hostUsername, guestUsername, hostReady, guestReady, onlineCode, decline, inBattle, stageSelected.

Methods: .ctor().

## TournamentController

Metadata offset 4941492.

Fields: i, checkpoints, round, loseTournament, losers, characters, battle, times, stage, tourneyLevel.

Methods: Start(); OnDestroy(); Reset(); Update(); PayTournament(fee); .ctor().

## UDPConnectionManager

Metadata offset 4941568.

Fields: udpClient, remoteEndPoint, port, byteQueue, onRawDataReceived, targetIP, lastClientIP.

Methods: Start(); EnableMulticast(); BroadCast(customPort); StartServer(customPort); Update(); SendRawData(data); ListenForMessages(); OnMessageReceived(result); StructToBytes(netMessage); GetLocalIPAddress(); GetBroadcastAddress(); OnApplicationQuit(); OnDestroy(); ChangeRemoteEndPoint(ip, customPort); CloseConnection(); .ctor().

## UnlockManager

Metadata offset 4941644.

Fields: .

Methods: Start(); Update(); .ctor().

## UnlockStage

Metadata offset 4941720.

Fields: crystalsView.

Methods: Start(); Update(); Unlock(); .ctor().

## UpdateController

Metadata offset 4941796.

Fields: .

Methods: Start(); Update(); UpdateNow(); .ctor().

## <AnimateVector3>d__8

Metadata offset 4941872.

Fields: <>1__state, <>2__current, <>4__this, <elapsedTime>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Vector3Animator

Metadata offset 4941948.

Fields: curveX, curveY, curveZ, startValue, endValue, duration, animatedVector.

Methods: Start(); AnimateVector3(); .ctor().

## <InitVoiceChat>d__9

Metadata offset 4942024.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## VoiceChatManager

Metadata offset 4942100.

Fields: alert, micOn, micOff, loading, playerVoice, playerVoiceCreated, voices.

Methods: Start(); ShowIcon(icon); InitVoiceChat(); CreatePlayerVoice(); StartRecording(); StopRecording(); ShowAlert(); RemoveAlert(); ToggleVoiceChat(); VoiceChatOn(); VoiceChatOff(); PermissionCallbacks_PermissionGranted(permissionName); AddToVoices(voice); MuteAll(); UnMuteAll(); .ctor().

## WaterShieldController

Metadata offset 4942176.

Fields: anim, RightShield, LeftShield, FrontShield, dust, RightShieldOn, LeftShieldOn.

Methods: Start(); CheckShields(); Update(); WLeftShieldOn(); WLeftShieldOff(); shieldOn(); shieldOff(); ActiveRightShield(); ActiveLeftShield(); DeactiveRightShield(); DeactiveLeftShield(); .ctor().

## WaterShieldOff

Metadata offset 4942252.

Fields: .

Methods: OnDestroy(); Start(); DeactivateShield(); ActivateShield(); Update(); .ctor().

## XMove

Metadata offset 4942328.

Fields: x, curve, elapseTime, time, force, initialPos.

Methods: Start(); Update(); .ctor().

## SearchRoomController

Metadata offset 4942404.

Fields: loading, RoomNotFound.

Methods: .ctor().

## SendAnimatorValues

Metadata offset 4942480.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## SendHostMovement

Metadata offset 4942556.

Fields: .

Methods: Start(); Update(); .ctor().

## SendPositionsToGuest_LM

Metadata offset 4942632.

Fields: localData, udpConnection, lastHostPosition, lastGuestPosition, <NetHostPlayerPos>k__BackingField, <NetGuestPlayerPos>k__BackingField.

Methods: get_NetHostPlayerPos(); set_NetHostPlayerPos(value); get_NetGuestPlayerPos(); set_NetGuestPlayerPos(value); Start(); Update(); SendUpdatedPositions(); SendPosition(playerType, position); ProcessPositionData(data); OnDestroy(); StructToBytes(playerPos); BytesToPlayerPosition(data); .ctor().

## PlayerPosition

Metadata offset 4942708.

Fields: playerType, position.

Methods: .ctor(type, pos); GetPosition().

## SetBones

Metadata offset 4942784.

Fields: newPart, original.

Methods: Start(); Update(); .ctor().

## SettingsOptions

Metadata offset 4942860.

Fields: anim, closeBG.

Methods: Start(); Update(); CloseSettings(); OpenSettings(); .ctor().

## ShadowController

Metadata offset 4942936.

Fields: shadowConfig.

Methods: Start(); OnDestroy(); ChangeShadow(); .ctor().

## ShadowHandController

Metadata offset 4943012.

Fields: shadowHandAnimator, anim, handFX, HandFXSound, floorDodgeFX, floorFXSound, ShadowHandFloorFX.

Methods: Start(); ShieldOn(); StopEnemy(); ShieldOff(); ShadowBlockHit(); ShowHandFX(); ShadowFloorFX(); .ctor().

## ShadowHandGrabController

Metadata offset 4943088.

Fields: anim, disapearTime, magicCircle, darkFire, explotion.

Methods: Start(); ForceDisapear(); Explote(); .ctor().

## ShieldBehaviour

Metadata offset 4943164.

Fields: .

Methods: .ctor().

## IShield

Metadata offset 4943240.

Fields: .

Methods: StartShield(); FinishShield(); OnHit(); ShieldBreak().

## ShieldBehavior

Metadata offset 4943316.

Fields: value__, Start, Finish, Hit, Break.

Methods: .

## ShieldManager

Metadata offset 4943392.

Fields: shieldList, ShieldPrefab, currentShieldLogic, localData, tcpConnection, playerController, isOn, avatarSwapper.

Methods: Awake(); Init(); OnDestroy(); StartShield(); FinishShield(); ShowShieldHitAnimation(); ShieldBreak(); SendShieldBehavior(shieldBehavior); GetShieldBehavior(data); .ctor().

## ShowHide

Metadata offset 4943468.

Fields: .

Methods: Start(); Update(); .ctor().

## <AlreadyFriends>d__3

Metadata offset 4943544.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## ShowIfConnectedDatabase

Metadata offset 4943620.

Fields: HideIfOpponentNotConnected, HideIfAlreadyFriends.

Methods: Start(); AlreadyFriends(); .ctor().

## ShowIfVoiceChat

Metadata offset 4943696.

Fields: .

Methods: Start(); .ctor().

## ShowInSpectator

Metadata offset 4943772.

Fields: .

Methods: Start(); Update(); .ctor().

## ShowQuitAlert

Metadata offset 4943848.

Fields: .

Methods: Start(); Update(); .ctor().

## ShowToGuest

Metadata offset 4943924.

Fields: .

Methods: Start(); Update(); .ctor().

## SingleActiveManager

Metadata offset 4944000.

Fields: currentGameObject.

Methods: Start(); ChangeCurrentActiveObject(newObject); .ctor().

## MoveCanvas

Metadata offset 4944076.

Fields: space, up, down, left, right.

Methods: Start(); Update(); .ctor().

## SlowSpeed

Metadata offset 4944152.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); OnStateExit(animator, stateInfo, layerIndex); .ctor().

## SpawOnOponnent

Metadata offset 4944228.

Fields: .

Methods: Start(); Spawn(); Update(); .ctor().

## <DrainSpecialUI>d__18

Metadata offset 4944304.

Fields: <>1__state, <>2__current, <>4__this, duration, <elapsed>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <FinishAwakening>d__10

Metadata offset 4944380.

Fields: <>1__state, <>2__current, time, playerController, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Special_LM

Metadata offset 4944456.

Fields: chargeImg, letter, outline, tcpConnection, localData, charged.

Methods: Init(); OnDestroy(); Reset(); InitAwakening(playerController, time); FinishAwakening(playerController, time); SendDrainSpecial(playerController, time); GetDrainSpecial(data); InitSpecial(playerController); ChargeSpecial(val, playerIndex); UpdateSpecialChargeUI(); DrainSpecial(playerController, time); ChangeSpecialButtons(); DrainSpecialUI(duration); SendSpecialCharges(); GetSpecialCharges(data); .ctor().

## SpectatorManager

Metadata offset 4944532.

Fields: room, ready, msg.

Methods: Awake(); CheckPlayersScene(); OnJoinedRoom(); OnJoinRoomFailed(returnCode, message); OnConnectedToMaster(); Connect(); EnterCode(); .ctor().

## SpringBoneGroup

Metadata offset 4944608.

Fields: gravity, bones, colliders.

Methods: Start(); Update(); SetGravity(); .ctor().

## SpringColliderGroup

Metadata offset 4944684.

Fields: boneGroups, colliders.

Methods: Awake(); .ctor().

## <Func_PlayAnimUI>d__11

Metadata offset 4944760.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## SpriteUIAnimation

Metadata offset 4944836.

Fields: m_Image, m_SpriteArray, m_Speed, m_IndexSprite, m_CorotineAnim, IsDone, playOnEnable.

Methods: OnEnable(); OnDisable(); Func_PlayUIAnim(); Func_StopUIAnim(); Func_PlayAnimUI(); .ctor().

## StageSelectorButton

Metadata offset 4944912.

Fields: stageSelectorButton, StageSelectorAnim, image, lastStage.

Methods: Start(); OnDestroy(); OpenStageSelector(); CloseStageSelector(); SelectStage(); ChangeImage(); .ctor().

## StagesManager

Metadata offset 4944988.

Fields: i, stages, playableStages, actualStage, lastStage.

Methods: Start(); AddFreeStages(); SetPlayableStages(); RemoveBlockStages(); CheckInitialStage(); HasStage(); RemoveRandom(stage); IsStageUnlocked(stageName); .ctor().

## StarMenuManager

Metadata offset 4945064.

Fields: tourneyAlert, points, missingCharacters, version, TournamentPrice, PriceText, blockButtons, isoLogo, base_UI, multiplayer_UI.

Methods: Start(); OnConnectedToMaster(); ShowVersion(); ShowMultiplayerUI(showMultiplayer); TourneyAlert(); CloseTourneyAlert(); EnterTournament(); PlayTeddies(); AllCharactersUnlocked(); GoToScene(scene); SurvivalMode(); SinglePlayerMode(); Online2Players(); GoToLocalMultiplayer(); Connect(); CheckConnection(); .ctor().

## StarsFX

Metadata offset 4945140.

Fields: DelayMove, endPosition, duration, menu.

Methods: Start(); StartMoving(); WinTrophy(); Menu(); .ctor().

## <PrepareCounter>d__6

Metadata offset 4945216.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <StartCounterRoutine>d__7

Metadata offset 4945292.

Fields: <>1__state, <>2__current, <>4__this, startCounterTime.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## StartCounter_LM

Metadata offset 4945368.

Fields: TCPConnection, syncTimers, localData.

Methods: Init(); OnDestroy(); Reset(); PrepareCounter(); StartCounterRoutine(startCounterTime); GetStartCounter(startCounterBytes); StartCounter(); SendStartCounterToGuest(startCounterTime); .ctor().

## <ChargeShieldBar>d__27

Metadata offset 4945444.

Fields: <>1__state, <>2__current, <>4__this, opponent, <bar>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <GrassHealing>d__24

Metadata offset 4945520.

Fields: <>1__state, <>2__current, <>4__this, opponent, <life>5__2, <children>5__3, <i>5__4, <>7__wrap4, <>7__wrap5, <crystalLife>5__7, <t>5__8.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <Hit>d__23

Metadata offset 4945596.

Fields: <>1__state, <>2__current, crystalLife, <originalColor>5__2, <t>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## <RestoreHeal2>d__22

Metadata offset 4945672.

Fields: <>1__state, <>2__current, <>4__this, opponent.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## StatsController

Metadata offset 4945748.

Fields: healthbarOp, healthbar, lifePlayer, lifeOp, Shieldbar, ShieldbarOp, shieldbarMax, iconA, iconB, iconAop, iconBop, player1Name, player2Name, healFxP1, healFxP2, i.

Methods: Start(); SetPlayersName(); LoseHealth(amount, opponent); SetHealth(damage, opponent); SetHealthLM(playerHP, opponent); RestoreHealth(opponent); RestoreHeal2(opponent); Hit(crystalLife); GrassHealing(opponent); ChangeColorIcon(color, button, opponent); SetShieldBar(shielDamage, opponent); ChargeShieldBar(opponent); Update(); GetPlayerLifeCrystals(guest); .ctor().

## StopGo

Metadata offset 4945824.

Fields: .

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## StopMovePath

Metadata offset 4945900.

Fields: owner, movePath.

Methods: Start(); StopMove(); OnDestroy(); Update(); .ctor().

## <StopParticlesAfterDelay>d__4

Metadata offset 4945976.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## StopParticlesOnCollision

Metadata offset 4946052.

Fields: particleSystems, delay.

Methods: OnCollisionEnter(collision); OnTriggerEnter(other); StopParticlesAfterDelay(); .ctor().

## SummonByTime

Metadata offset 4946128.

Fields: .

Methods: Start(); Update(); .ctor().

## SumonByTime

Metadata offset 4946204.

Fields: create, time, attack, destroyAfterSumon, copyParentRotation, ignoreIfCollides, ignoreIfWeak.

Methods: Start(); Update(); Sumon(); .ctor().

## SumonChildAtk

Metadata offset 4946280.

Fields: atkPrefab, offset, rotationOffset.

Methods: Start(); Update(); .ctor().

## SumonInOpponent

Metadata offset 4946356.

Fields: .

Methods: Awake(); SumonInOpPos(AtkID); OnDestroy(); .ctor().

## <>c

Metadata offset 4946432.

Fields: <>9, <>9__0_0, <>9__0_1.

Methods: .cctor(); .ctor(); <Sumon>b__0_0(x); <Sumon>b__0_1(x).

## SumonInOpponent_LM

Metadata offset 4946508.

Fields: .

Methods: Sumon(); .ctor().

## sumonOnCollider

Metadata offset 4946584.

Fields: sumon.

Methods: OnTriggerEnter(other); NetworkSumon(otherTransform); Sumon(otherTransform); .ctor().

## SumonOnDestroy

Metadata offset 4946660.

Fields: obj.

Methods: OnDestroy(); .ctor().

## <GetTimerOffsetRoutine>d__16

Metadata offset 4946736.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## SyncTimers

Metadata offset 4946812.

Fields: TCPConnection, localData, t1_guest, t2_host, t3_guest, t4_host, timeOffset, onTimeOffsetCalculated, timerOffsetAvg, offsetSamples, offsetSampleCount, currentSampleIndex, waitingSampleComplete.

Methods: Start(); OnDestroy(); GetAvgTimerOffset(); GetTimerOffsetRoutine(); GetTimerOffset(); SendTime(timeType); GetHostTime(); OnTimeReceived(message); CalculateTimeOffset(); SendOffsetTimerToHost(); .ctor(); <GetTimerOffsetRoutine>b__16_0().

## ParamType

Metadata offset 4946888.

Fields: value__, Int, Float, String, Bool, Double.

Methods: .

## <>c__DisplayClass30_0

Metadata offset 4946964.

Fields: <>4__this, success, error.

Methods: .ctor(); <ConnectCoroutine>b__0(ar).

## <ConnectCoroutine>d__30

Metadata offset 4947040.

Fields: <>1__state, <>2__current, <>4__this, serverIP, port, <>8__1, <timeout>5__2, <timer>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## TCPConnection

Metadata offset 4947116.

Fields: localData, tcpListener, tcpClient, networkStream, buffer, accumulator, isServer, isConnected, bytesQueue, onMessageReceived, onConnect, onClientConnected, port.

Methods: Start(); OnDestroy(); Init(); Update(); CreateMessage(netEvent, parameters); CreateAndSendMessage(netEvent, parameters); SendShortMessage(netEvent); ParseShortMessage(data, expectedEvent); ParseMessage(data, expectedEvent, expectedTypes); ProcessMessages(); GetLocalIPAddress(); SendNetMessage(message); SendByteMessage(payload); StartServer(customPort); ConnectToHost(serverIP, port); HandleMessage(data); SendMessage(stream, values); ConnectCoroutine(serverIP, port); OnApplicationQuit(); CloseConnection(); .ctor().

## <StartLagTimer>d__16

Metadata offset 4947192.

Fields: <>1__state, <>2__current, <>4__this.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## TCP_Timer

Metadata offset 4947268.

Fields: localData, TCPConnection, timer, lagTimer, lag, firstTimerCall, secondTimerCall, startTimer, onTimerUpdated.

Methods: Start(); initHostActions(); OnDestroy(); SendSecondTimer(); Update(); SendTimerToGuest(); ReceiveTimerFromHost(timerBytes); StartLagTimer(); .ctor().

## TestController

Metadata offset 4947344.

Fields: i, test.

Methods: Start(); .ctor().

## <LerpToZeroCoroutine>d__4

Metadata offset 4947420.

Fields: <>1__state, <>2__current, startValue, speed, onValueChanged, <value>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## TestingController

Metadata offset 4947496.

Fields: testingText.

Methods: Start(); Update(); Test(); LerpToZeroCoroutine(startValue, speed, onValueChanged); .ctor(); <Test>b__3_0(value).

## Test_UI

Metadata offset 4947572.

Fields: localData, timerText, UDPConnection.

Methods: Start(); Update(); .ctor().

## <NextTurn>d__28

Metadata offset 4947648.

Fields: <>1__state, <>2__current, <>4__this, <skip>5__2, <>7__wrap2, <p>5__4.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); <>m__Finally1(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## TournamentManager

Metadata offset 4947724.

Fields: i, players, winners, round, roundVS, sun, charactersTournament, zoomCanvas, winnerMusic, music, finalMusic, canvas, stars, menu, alert.

Methods: Start(); StartMusic(); SetLosers(); SetPositions(); SetCheckpoints(); Continue(); showMenu(); DelayStart(); ShowVS(); RotateSun(); ShowStars(); NextRound(); NextCheckpoint(); NextTurn(); SetOpponents(); ActivateVS(); RemoveVS(); StartBattle(); Zoom(); GoToBattle(); DisableCanvas(); EnableCanvas(); PlayAgain(); QuitAlert(); CancelAlert(); .ctor().

## TourneyLevel

Metadata offset 4947800.

Fields: level, actualLevel, maxLv.

Methods: Start(); Update(); NextLevel(); BackLevel(); SetLevel(); .ctor().

## Translation

Metadata offset 4947876.

Fields: StartMenu_1Player, StartMenu_Online, StartMenu_ShareGame, StartMenu_ShareGame_ThanksMessage, StartMenu_ExitGame, StartMenu_Tournament, StartMenu_Survival, StartMenu_NewVersion, StartMenu_UpdateNow, StartMenu_Text_PleaseSharYourFeedback, StartMenu_LoadingData, StartMenu_PleasWait, StartMenu_Text_Welcome, StartMenu_Text_Account, Online_Text_ErrorCheckYourInternet, Online_Text_ConnectingWithServe, Online_Text_ConnectionSuccessful, Online_Text_ConnectionSuccessfulServer, Online_Text_PlayerHasDisconected, Online_Text_ThereAreNoMatches, Online_QuickPlay, Online_QuickPlay_WaitingForOtherPlayer, Online_Text_WaitingForOtherPlayer, Online_CreatePrivate, Online_EnterCode, Online_JoinPrivate, Online_Text_ThisIsNewGame, Online_Server, Online_Server_Change, Online_Text_WaitingForYourOponent, Online_Text_APlayerHasEnterTheMatch, Online_Text_YouMostEnterACodeToJoinAPrivate, Online_CreatePrivate_ShareCode, Online_Text_FailedToConnect, Online_Text_WaitingForYourFriendToJoinTheGame, Online_Text_TheCodeYouEnteredMayNotBe, Online_Text_JoiningGame, Online_Text_ChangingServer, Online_Text_ConnectingTo, Online_Text_PleaseWait, Online_Text_Loading, Online_Text_TheBattleIsAboutToStart, HowToPlay_HowToPlay, HowToPlay_Move, HowToPlay_Attack, HowToPlay_Defense, HowToPlay_BasicAtk, HowToPlay_RightAtk, HowToPlay_LeftAtk, HowToPlay_DownAtk, HowToPlay_UpAtk, HowToPlay_Block, HowToPlay_JumpR, HowToPlay_JumpL, HowToPlay_BackFlip, HowToPlay_Roll, HowToPlay_LevelAttacks, HowToPlay_Continue, HowToPlay_Start, QuitBotton, QuitBotton_Text_AreYouSure, QuitBotton_Quit, QuitBotton_Cancel, ChooseCharacter_ChooseCharacter, ChooseCharacter_Random, ChooseCharacter_Unlock, ChooseCharacter_Text_GetReady, ChooseCharacter_Text_WaitingForOtherPlayer, ChooseCharacter_Text_YouNeedMoreCrystals, ChooseCharacter_Text_YouNeedMoreTrophies, ChooseCharacter_CPU, ChooseCharacter_LV1, ChooseCharacter_LV2, ChooseCharacter_LV3, ChooseCharacter_LV4, ChooseCharacter_LV5, ChooseCharacter_Start, ChooseCharacter_SelectStage, ChooseCharacter_SelectStage_Select, ChooseCharacter_SelectStage_Close, ChooseCharacter_SelectStage_UnlockStage, ChooseCharacter_Text_NewStageUnlock, Loading, BottonA, BottonB, Battle_Cpu, Battle_Quit_PlayAgain, Battle_Quit_MainMenu, Battle_Quit_ResumeGame, Battle_Settings, Battle_Settings_Lock30FPS, Battle_Settings_Min, Battle_Settings_Med, Battle_Settings_Max, Battle_Settings_Music, Battle_Settings_Mute, Battle_Settings_Joystick, Battle_Settings_Dpad, Battle_Settings_Controller, Battle_Settings_Inv, Winner_Winner, Winner_Results, Winner_Rank, Winner_PlayAgain, Winner_WatchAd, Winner_Bonus, Winner_Score, Winner_Text_CrystalsDisabled, Winner_Text_CongratsYouWon25ExtraCrystals, Winner_Rewarded, Winner_Continue, Tournament_Text_YouMostUnlock, Tournament_EntryFee, Tournament_Enter, Tournament_Cancel, Tournament_MainMenu, Tournament_PlayAgain, Tournament_ExitGame, SharingGame_HeyCheckThisGame, SharingGame_Video, SharingGame_InstallGame, RateGame_Description, RateGame_ThanksForPlaying, RateGame_RateNow, RateGame_NoThanks, RateGame_RemindMeLater, GameStillInDevelopment, ChooseCharacter_HighScore, ChooseCharacter_SelectStage_Price, ChooseCharacter_YouNeedMoreCrystalsToUnlockStage.

Methods: .ctor().

## TransladeController

Metadata offset 4947952.

Fields: i, translation.

Methods: Awake(); ChangeToEnglish(); .ctor().

## <MoveToPosition>d__2

Metadata offset 4948028.

Fields: <>1__state, <>2__current, duration, startpos, endpos, <t>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## TransladeObject

Metadata offset 4948104.

Fields: i.

Methods: Start(); MoveToPosition(startpos, endpos, duration); .ctor().

## TransladeText

Metadata offset 4948180.

Fields: textElement, VariableName.

Methods: Start(); Update(); .ctor().

## TrapPowerUp

Metadata offset 4948256.

Fields: limitDistance, powerUpFX, TrapFX, powered, directionChanged, startDelay, reboundDelay, ScaleObject, Jump, animator.

Methods: Start(); Update(); CheckIfTrapNear(); PowerUpAttack(); ChangeDirection(); RestoreDirectionChanged(); ShowOnlineFX(); .ctor().

## TrophiesManager

Metadata offset 4948332.

Fields: i, number.

Methods: Start(); OnDestroy(); UpdateTrophies(); .ctor().

## TurnAttack

Metadata offset 4948408.

Fields: target, delay, duration, turning, right, speed.

Methods: Start(); Update(); Init(); finish(); .ctor().

## UISelector

Metadata offset 4948484.

Fields: buttons.

Methods: Start(); MoveToButton(button); Update(); .ctor().

## MultiKeyDictionary`3

Metadata offset 4948560.

Fields: .

Methods: get_Item(key); ContainsKey(key1, key2); .ctor().

## Images

Metadata offset 4948636.

Fields: clearImage, collapseImage, clearOnNewSceneImage, showTimeImage, showSceneImage, userImage, showMemoryImage, softwareImage, dateImage, showFpsImage, infoImage, saveLogsImage, searchImage, copyImage, closeImage, buildFromImage, systemInfoImage, graphicsInfoImage, backImage, logImage, warningImage, errorImage, barImage, button_activeImage, even_logImage, odd_logImage, selectedImage, reporterScrollerSkin.

Methods: .ctor().

## _LogType

Metadata offset 4948712.

Fields: value__, Assert, Error, Exception, Log, Warning.

Methods: .

## Sample

Metadata offset 4948788.

Fields: time, loadedScene, memory, fps, fpsText.

Methods: MemSize(); GetSceneName(); .ctor().

## Log

Metadata offset 4948864.

Fields: count, logType, condition, stacktrace, sampleId.

Methods: CreateCopy(); GetMemoryUsage(); .ctor().

## ReportView

Metadata offset 4948940.

Fields: value__, None, Logs, Info, Snapshot.

Methods: .

## DetailView

Metadata offset 4949016.

Fields: value__, None, StackTrace, Graph.

Methods: .

## <readInfo>d__188

Metadata offset 4949092.

Fields: <>1__state, <>2__current, <>4__this, <www>5__2.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## Reporter

Metadata offset 4949168.

Fields: samples, logs, collapsedLogs, currentLog, logsDic, cachedString, show, collapse, clearOnNewSceneLoaded, showTime, showScene, showMemory, showFps, showGraph, showLog, showWarning, showError, numOfLogs, numOfLogsWarning, numOfLogsError, numOfCollapsedLogs, numOfCollapsedLogsWarning, numOfCollapsedLogsError, showClearOnNewSceneLoadedButton, showTimeButton, showSceneButton, showMemButton, showFpsButton, showSearchText, showCopyButton, showSaveButton, buildDate, logDate, logsMemUsage, graphMemUsage, gcTotalMemory, UserData, fps, fpsText, currentView, created, images, clearContent, collapseContent, clearOnNewSceneContent, showTimeContent, showSceneContent, userContent, showMemoryContent, softwareContent, dateContent, showFpsContent, infoContent, saveLogsContent, searchContent, copyContent, closeContent, buildFromContent, systemInfoContent, graphicsInfoContent, backContent, logContent, warningContent, errorContent, barStyle, buttonActiveStyle, nonStyle, lowerLeftFontStyle, backStyle, evenLogStyle, oddLogStyle, logButtonStyle, selectedLogStyle, selectedLogFontStyle, stackLabelStyle, scrollerStyle, searchStyle, sliderBackStyle, sliderThumbStyle, toolbarScrollerSkin, logScrollerSkin, graphScrollerSkin, size, maxSize, numOfCircleToShow, scenes, currentScene, filterText, deviceModel, deviceType, deviceName, graphicsMemorySize, maxTextureSize, systemMemorySize, Initialized, screenRect, toolBarRect, logsRect, stackRect, graphRect, graphMinRect, graphMaxRect, buttomRect, stackRectTopLeft, detailRect, scrollPosition, scrollPosition2, toolbarScrollPosition, selectedLog, toolbarOldDrag, oldDrag, oldDrag2, oldDrag3, startIndex, countRect, timeRect, timeLabelRect, sceneRect, sceneLabelRect, memoryRect, memoryLabelRect, fpsRect, fpsLabelRect, tempContent, infoScrollPosition, oldInfoDrag, tempRect, graphSize, startFrame, currentFrame, tempVector1, tempVector2, graphScrollerPos, maxFpsValue, minFpsValue, maxMemoryValue, minMemoryValue, gestureDetector, gestureSum, gestureLength, gestureCount, lastClickTime, startPos, downPos, mousePosition, frames, firstTime, lastUpdate, requiredFrames, updateInterval, threadedLogs.

Methods: get_TotalMemUsage(); Awake(); OnDestroy(); OnEnable(); OnDisable(); addSample(); Initialize(); initializeStyle(); Start(); clear(); calculateCurrentLog(); DrawInfo(); drawInfo_enableDisableToolBarButtons(); DrawReport(); drawToolBar(); DrawLogs(); drawGraph(); drawStack(); OnGUIDraw(); isGestureDone(); isDoubleClickDone(); getDownPos(); getDrag(); calculateStartIndex(); doShow(); Update(); CaptureLog(condition, stacktrace, type); AddLog(condition, stacktrace, type); CaptureLogThread(condition, stacktrace, type); _OnLevelWasLoaded(_null1, _null2); OnApplicationQuit(); readInfo(); SaveLogsToDevice(); .ctor().

## ReporterGUI

Metadata offset 4949244.

Fields: reporter.

Methods: Awake(); OnGUI(); .ctor().

## ReporterMessageReceiver

Metadata offset 4949320.

Fields: reporter.

Methods: Start(); OnPreStart(); OnHideReporter(); OnShowReporter(); OnLog(log); .ctor().

## UpdatePlayersScene

Metadata offset 4949396.

Fields: TCPConnection, localData.

Methods: Start(); OnDestroy(); CheckIfPlayersAreInSameScene(); OnSceneLoaded(scene, mode); NotifyOpponent(indexScene); MessageHandler(message); .ctor().

## User

Metadata offset 4949472.

Fields: username, email.

Methods: .ctor(username, email).

## Utilities

Metadata offset 4949548.

Fields: i, FindObjsWithComp, ComponentName, directory.

Methods: Start(); GetFieldValue(src, propName); SetFieldValue(src, propName, value); removeSimbols(region); GetTimeAsString(time); QuaternionToVector3(rot); Vector3ToQuaternion(V3); DistanceBtwTwoObjs(firstObj, secondObj); AreObjetsNear(firstObj, secondObj, limitDistance); LoadPrefabsContaining(path); GetAngleDiference(obj1, obj2); GetRelativePosition(referenceObject, targetObject); GetRelativePositionByAngle(referenceObject, targetObject, frontBackRatio, frontRatio); AngleDir(fwd, targetDir, up); GetDateTime(); GetAllChilds(Go); .ctor().

## VoiceChatButton

Metadata offset 4949624.

Fields: on, off.

Methods: Start(); Toggle(); TurnOn(); TurnOff(); PermissionCallbacks_PermissionDeniedAndDontAskAgain(permissionName); PermissionCallbacks_PermissionGranted(permissionName); PermissionCallbacks_PermissionDenied(permissionName); .ctor().

## VoiceChatController

Metadata offset 4949700.

Fields: i, playerVoice, othersVoices.

Methods: Start(); CreatePlayerVoice(); StartRecording(); StopRecording(); MuteAll(); UnMuteAll(); AddToOtherVoices(voice); .ctor().

## <ChangeSizeCoroutine>d__8

Metadata offset 4949776.

Fields: <>1__state, <>2__current, <>4__this, duration, newSize, <initialSize>5__2, <elapsed>5__3.

Methods: .ctor(<>1__state); System.IDisposable.Dispose(); MoveNext(); System.Collections.Generic.IEnumerator<System.Object>.get_Current(); System.Collections.IEnumerator.Reset(); System.Collections.IEnumerator.get_Current().

## WaterShieldManager

Metadata offset 4949852.

Fields: ShieldExplosionPrefab, iceShield, dustFX.

Methods: Start(); StartShield(); FinishShield(); OnHit(); ShieldBreak(); ChangeSizeCoroutine(newSize, duration); .ctor().

## WhipBehavior

Metadata offset 4949928.

Fields: left.

Methods: OnStateEnter(animator, stateInfo, layerIndex); .ctor().

## WhipController

Metadata offset 4950004.

Fields: anim, handFX, whipSound, leftWhipSoundDelay, rightWhipSoundDelay.

Methods: Start(); Update(); PlayWhipSound(); RightWhipAtk(); LeftWhipAtk(); IddleWhip(); NetIddleWhip(); ShowHandLeafFX(); OnDestroy(); .ctor().

## WhipOff

Metadata offset 4950080.

Fields: .

Methods: OnStateExit(animator, stateInfo, layerIndex); .ctor().

## YMove

Metadata offset 4950156.

Fields: y, curve, elapseTime, time, force, initialPos.

Methods: Start(); Update(); .ctor().

## ZigzagController

Metadata offset 4950232.

Fields: moveDistance, switchTime, direction, timer.

Methods: Start(); Update(); .ctor().

## ZoomCanvas

Metadata offset 4950308.

Fields: views, currentView.

Methods: Start(); FixedUpdate(); .ctor().

## MonoScriptData

Metadata offset 4950384.

Fields: FilePathsData, TypesData, TotalTypes, TotalFiles, IsEditorOnly.

Methods: .

## UnitySourceGeneratedAssemblyMonoScriptTypes_v1

Metadata offset 4950460.

Fields: .

Methods: Get(); .ctor().

## GPGSIds

Metadata offset 4950536.

Fields: achievement_perfect, leaderboard_score, achievement_250_crystals.

Methods: .

## ConnectAndJoinRandomLb

Metadata offset 4950612.

Fields: appSettings, lbc, ch, StateUiText.

Methods: Start(); Update(); OnConnected(); OnConnectedToMaster(); OnDisconnected(cause); OnCustomAuthenticationResponse(data); OnCustomAuthenticationFailed(debugMessage); OnRegionListReceived(regionHandler); OnRoomListUpdate(roomList); OnLobbyStatisticsUpdate(lobbyStatistics); OnJoinedLobby(); OnLeftLobby(); OnFriendListUpdate(friendList); OnCreatedRoom(); OnCreateRoomFailed(returnCode, message); OnJoinedRoom(); OnJoinRoomFailed(returnCode, message); OnJoinRandomFailed(returnCode, message); OnLeftRoom(); OnRegionPingCompleted(regionHandler); .ctor().

## EventSystemSpawner

Metadata offset 4950688.

Fields: .

Methods: Start(); .ctor().

## OnStartDelete

Metadata offset 4950764.

Fields: .

Methods: Start(); .ctor().

## TextButtonTransition

Metadata offset 4950840.

Fields: _text, Selectable, NormalColor, HoverColor.

Methods: Awake(); OnEnable(); OnDisable(); OnPointerEnter(eventData); OnPointerExit(eventData); .ctor().

## TextToggleIsOnTransition

Metadata offset 4950916.

Fields: toggle, _text, NormalOnColor, NormalOffColor, HoverOnColor, HoverOffColor, isHover.

Methods: OnEnable(); OnDisable(); OnValueChanged(isOn); OnPointerEnter(eventData); OnPointerExit(eventData); .ctor().

## Global

Metadata offset 4950992.

Fields: .

Methods: TestGlobal(); .ctor().

## CameraShakeState

Metadata offset 4951068.

Fields: value__, FadingIn, FadingOut, Sustained, Inactive.

Methods: .

## CameraShakeInstance

Metadata offset 4951144.

Fields: Magnitude, Roughness, PositionInfluence, RotationInfluence, DeleteOnInactive, roughMod, magnMod, fadeOutDuration, fadeInDuration, sustain, currentFadeTime, tick, amt.

Methods: .ctor(magnitude, roughness, fadeInTime, fadeOutTime); .ctor(magnitude, roughness); UpdateShake(); StartFadeOut(fadeOutTime); StartFadeIn(fadeInTime); get_ScaleRoughness(); set_ScaleRoughness(value); get_ScaleMagnitude(); set_ScaleMagnitude(value); get_NormalizedFadeTime(); get_IsShaking(); get_IsFadingOut(); get_IsFadingIn(); get_CurrentState().

## CameraShakePresets

Metadata offset 4951220.

Fields: .

Methods: get_Bump(); get_Explosion(); get_Earthquake(); get_BadTrip(); get_HandheldCamera(); get_Vibration(); get_RoughDriving().

## CameraShaker

Metadata offset 4951296.

Fields: Instance, instanceList, DefaultPosInfluence, DefaultRotInfluence, RestPositionOffset, RestRotationOffset, posAddShake, rotAddShake, cameraShakeInstances.

Methods: Awake(); Update(); GetInstance(name); Shake(shake); ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime); ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime, posInfluence, rotInfluence); StartShake(magnitude, roughness, fadeInTime); StartShake(magnitude, roughness, fadeInTime, posInfluence, rotInfluence); get_ShakeInstances(); OnDestroy(); .ctor(); .cctor().

## CameraUtilities

Metadata offset 4951372.

Fields: .

Methods: SmoothDampEuler(current, target, velocity, smoothTime); MultiplyVectors(v, w); .ctor().

## DrawSceneObject

Metadata offset 4951448.

Fields: m_replacementShader, m_cullingMask, m_shadowTextureRenderer.

Methods: get_replacementShader(); set_replacementShader(value); get_cullingMask(); set_cullingMask(value); get_shadowTextureRenderer(); OnValidate(); OnEnable(); OnDisable(); OnVisibilityChanged(isVisible); .ctor().

## ReplaceShader

Metadata offset 4951524.

Fields: renderType, shader.

Methods: .

## TextureAlignment

Metadata offset 4951600.

Fields: value__, None, TargetAxisX, TargetAxisY, TargetAxisZ.

Methods: .

## UpdateFunction

Metadata offset 4951676.

Fields: value__, OnPreCull, LateUpdate, UpdateTransform.

Methods: .

## CommandBufferHash

Metadata offset 4951752.

Fields: hash.

Methods: Init(); AddRenderer(renderer); Equals(rhs).

## DrawTargetObject

Metadata offset 4951828.

Fields: m_target, m_targetDirection, m_layerMask, m_textureAlignment, m_updateFunction, m_shadowShader, m_replacementShaders, m_renderChildren, m_autoDetectHierarchyChanges, m_followTarget, m_commandBufferHash, m_isCommandBufferDirty, m_commandBuffer, m_shadowRenderer, m_localTargetPosition, m_replacedMaterialCache.

Methods: get_target(); set_target(value); get_targetDirection(); set_targetDirection(value); get_renderChildren(); set_renderChildren(value); get_autoDetectHierarchyChanges(); set_autoDetectHierarchyChanges(value); get_layerMask(); set_layerMask(value); get_textureAlignment(); set_textureAlignment(value); get_updateFunction(); set_updateFunction(value); get_followTarget(); set_followTarget(value); get_shadowShader(); set_shadowShader(value); get_replacementShaders(); set_replacementShaders(value); SetCommandBufferDirty(); UpdateCommandBuffer(); UpdateMaterial(mat); UpdateTransform(); get_commandBuffer(); Awake(); OnValidate(); OnEnable(); OnDisable(); OnDestroy(); LateUpdate(); OnPreCull(); OnVisibilityChanged(isVisible); CreateCommandBuffer(); IsTargetObjectTreeStructureChanged(); AddDrawCommandForGameObject(obj, recursive, commandBufferHash, hashOnly); AddDrawCommand(renderer, renderTypeIndex); .ctor().

## TextureAlignment

Metadata offset 4951904.

Fields: value__, None, TargetAxisX, TargetAxisY, TargetAxisZ.

Methods: .

## UpdateFunction

Metadata offset 4951980.

Fields: value__, OnPreCull, LateUpdate, UpdateTransform.

Methods: .

## FollowTargetObject

Metadata offset 4952056.

Fields: m_target, m_targetDirection, m_textureAlignment, m_updateFunction, m_localTargetPosition.

Methods: get_target(); set_target(value); get_targetDirection(); set_targetDirection(value); get_textureAlignment(); set_textureAlignment(value); get_updateFunction(); set_updateFunction(value); UpdateTransform(); Awake(); LateUpdate(); OnPreCull(); .ctor().

## MipmappedShadowFallback

Metadata offset 4952132.

Fields: m_fallbackShaderOrMaterial, m_blurLevel, m_blurSize, m_modifyTextureSize, m_multiSampling, m_superSampling, m_textureWidth, m_textureHeight, m_tex2DlodCheckShader, m_glslCheckShader.

Methods: Awake(); ApplyFallback(projector); .ctor().

## Options

Metadata offset 4952208.

Fields: value__, CopyMainTexFromMaterial, CopyAlphaCutoffFromMaterial, CopyBaseColorFromMaterial, EnableAlphaCutoffByKeyword, EnableAlphaCutoff, CopyCullModeFromMaterial, CullFront, CullBack.

Methods: .

## CullMode

Metadata offset 4952284.

Fields: value__, None, Front, Back.

Methods: .

## PropertyBlockForTransparentReceiver

Metadata offset 4952360.

Fields: m_options, mainTexture, mainTextureST, baseAlpha, alphaCutoff, mainTextureName, baseColorName, alphaCutoffName, alphaCutoffKeyword, cullModeName, m_mainTextureSTName, m_renderer, m_propertyBlock, s_dspBaseAlphaId, s_dspCullModeId, s_dspMainTexId, s_dspMainTexStId.

Methods: GetOption(option); SetOption(option, value); get_copyMainTexFromMaterial(); set_copyMainTexFromMaterial(value); get_copyBaseColorFromMaterial(); set_copyBaseColorFromMaterial(value); get_copyAlphaCutoffFromMaterial(); set_copyAlphaCutoffFromMaterial(value); get_copyCullModeFromMaterial(); set_copyCullModeFromMaterial(value); get_cullMode(); set_cullMode(value); get_enableAlphaCutoffByKeyword(); set_enableAlphaCutoffByKeyword(value); get_enableAlphaCutoff(); set_enableAlphaCutoff(value); .cctor(); StaticInitialize(); GetMaterialMainTexture(); GetMaterialMainTextureST(); GetMaterialBaseColor(); GetMaterialAlphaCutoff(); GetMaterialCullMode(); UpdatePropertyBlock(); Initialize(); Awake(); Start(); .ctor().

## TextureMultiSample

Metadata offset 4952436.

Fields: value__, x1, x2, x4, x8.

Methods: .

## TextureSuperSample

Metadata offset 4952512.

Fields: value__, x1, x4, x16.

Methods: .

## MipmapFalloff

Metadata offset 4952588.

Fields: value__, None, Linear, Custom.

Methods: .

## BlurFilter

Metadata offset 4952664.

Fields: value__, Uniform, Gaussian.

Methods: .

## BlurParam

Metadata offset 4952740.

Fields: tap, offset, weight.

Methods: .

## ShadowTextureRenderer

Metadata offset 4952816.

Fields: m_multiSampling, m_superSampling, m_mipmapFalloff, m_blurFilter, m_testViewClip, m_textureWidth, m_textureHeight, m_mipLevel, m_blurLevel, m_blurSize, m_mipmapBlurSize, m_singlePassMipmapBlur, m_shadowColor, m_blurShader, m_downsampleShader, m_copyMipmapShader, m_eraseShadowShader, m_customMipmapFalloff, m_preferredTextureFormats, m_camerasForViewClipTest, <shadowTexture>k__BackingField, s_falloffParamID, s_blurOffsetHParamID, s_blurOffsetVParamID, s_blurWeightHParamID, s_blurWeightVParamID, s_shadowColorParamID, s_downSampleBlurOffset0ParamID, s_downSampleBlurOffset1ParamID, s_downSampleBlurOffset2ParamID, s_downSampleBlurOffset3ParamID, s_downSampleBlurWeightParamID, s_blitSrcTexParamID, s_blitSrcTexelSizeParamID, s_shadowTexParamID, s_mipLevelParamID, m_projector, m_projectorMaterial, m_commandBuffer, m_camera, m_postProcessCommands, m_isTexturePropertyChanged, m_isVisible, m_shadowTextureValid, s_sharedMaterials, CLONED_MATERIAL_HIDE_FLAGS, s_temporaryRenderTargetNameIds, s_temporaryRenderTargetIdentifiers, m_temporaryRenderTarget, m_propertyBlockPool, m_propetyBlockCount, s_borderMeshCache, s_borderMeshIndices, s_fullScreenQuad, MAX_BLUR_TAP_SIZE, s_blurWeights.

Methods: get_multiSampling(); set_multiSampling(value); get_superSampling(); set_superSampling(value); get_textureWidth(); set_textureWidth(value); get_textureHeight(); set_textureHeight(value); get_preferredTextureFormats(); set_preferredTextureFormats(value); get_mipLevel(); set_mipLevel(value); get_blurLevel(); set_blurLevel(value); get_blurSize(); set_blurSize(value); get_blurFilter(); set_blurFilter(value); get_mipmapBlurSize(); set_mipmapBlurSize(value); get_singlePassMipmapBlur(); set_singlePassMipmapBlur(value); get_mipmapFalloff(); set_mipmapFalloff(value); get_customMipmapFalloff(); set_customMipmapFalloff(value); get_shadowColor(); set_shadowColor(value); get_blurShader(); set_blurShader(value); get_downsampleShader(); set_downsampleShader(value); get_copyMipmapShader(); set_copyMipmapShader(value); get_eraseShadowShader(); set_eraseShadowShader(value); get_shadowTexture(); set_shadowTexture(value); get_testViewClip(); set_testViewClip(value); get_camerasForViewClipTest(); set_camerasForViewClipTest(value); get_projectorCamera(); get_cameraNearClipPlane(); set_cameraNearClipPlane(value); get_cameraCullingMask(); set_cameraCullingMask(value); SetReplacementShader(shader, replacementTag); get_isProjectorVisible(); get_isBuiltinRP(); SetTexturePropertyDirty(); CreateRenderTexture(); AddCommandBuffer(commandBuffer); RemoveCommandBuffer(commandBuffer); get_useIntermediateTexture(); Initialize(); IsInitialized(); Awake(); OnEnable(); OnDisable(); Start(); IsPrefabAsset(obj); IsPrefabAsset(); OnValidate(); OnApplicationPause(pause); OnApplicationFocus(focus); CloneProjectorMaterialIfShared(); OnDestroy(); IsReadyToExecute(); SetVisible(isVisible); get_postProcessCommandBuffer(); StaticInitialize(); .cctor(); CreateTemporaryRenderTarget(); ReleaseTemporaryRenderTarget(); ClearPropertyBlockPool(); AllocatePropertyBlock(); AddPostRenderPassCommands(cmd, srcId); AddEraseShadowOnBorderCommand(cmd, w, h); AddBlitToCurrentTargetCommand(cmd, srcId, mat, pass, properties); Update(); OnPreCull(); HasShadowColor(); PrepareRendering(); OnPreRender(); OnPostRender(); GetBlurParam(blurSize, filter); GetDownsampleBlurParam(blurSize, filter); SetBlurShaderProperties(dst, blurH, blurV); GetMipmapBlurParams(blurH, blurV); SetMipmapBlurShaderProperties(dst, blurH, blurV); SetMipmapDownsampleWithBlurProperties(dst, blurH, blurV, w, h); OnBeforeSerialize(); OnAfterDeserialize(); .ctor().

## FPSCheck

Metadata offset 4952892.

Fields: frameCount, startTime, fps, text.

Methods: Update(); .ctor().

## Rotate

Metadata offset 4952968.

Fields: m_rotateSpeed.

Methods: Update(); .ctor().

## Swing

Metadata offset 4953044.

Fields: m_minAngle, m_maxAngle, m_swingSpeed, m_initialRotation, m_swing.

Methods: Start(); Update(); .ctor().

## __StaticArrayInitTypeSize=24

Metadata offset 4953120.

Fields: .

Methods: .

## __StaticArrayInitTypeSize=32

Metadata offset 4953196.

Fields: .

Methods: .

## __StaticArrayInitTypeSize=11541

Metadata offset 4953272.

Fields: .

Methods: .

## __StaticArrayInitTypeSize=20274

Metadata offset 4953348.

Fields: .

Methods: .

## <PrivateImplementationDetails>

Metadata offset 4953424.

Fields: 18689A54C1FF754BE58500B2ED77A6C75B025BE96F6D01FEF89C42DA1C953F34, 4CDBAC10D79E4296645B43FEB5BFDAE6B2C7D60F293BDC4A9971B4B5D44055E8, 5AF7B50FA5DF79B63E9E93ED3DFDB74DBBFD4E39B34D0ECD6F160F8F5BF5D6E1, FF1F6EE5D67458CFAC950F62E93042E21FCB867E2234DCC8721801231064AD40.

Methods: ComputeStringHash(s).
