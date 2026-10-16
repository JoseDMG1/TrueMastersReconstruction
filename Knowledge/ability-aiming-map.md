# Twelve-fighter ability direction map

Generated from Evidence/ability-dependencies.json using YAML references, not inferred from attack names. Each row is an animator state, not a separate guaranteed hit. Dependencies are conservative prefab references (including effects and nested dependencies); active execution and screenshot acceptance are separate. Native contracts and compatibility adaptations: ability-aiming-reconstruction.md.

## Sora

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | Follow (AirAtkTwister), MoveWater (AirAtkTwister), RotateSelf (AirAtkTwister) |
| BasicAttack | GoForward (AirAtk), RotateSelf (AirAtk) |
| UpAttack | GoForward (AirAtkStrong), RotateSelf (AirAtkStrong) |
| RightAttack | GoForward (AirAtkR), TurnAttack (AirAtkR), RotateSelf (AirAtkR) |
| LeftAttack | GoForward (AirAtkL), Rotate (AirAtkL), RotateSelf (AirAtkL) |
| BasicAttack2 | GoForward (AirAtk2), RotateSelf (AirAtk2) |
| DownAttack2 | RotateSelf (ChargeAir) |
| UpAttack2 | FollowOwner (AirUp2FX), FollowOwner (AirShieldAtk), FollowOwner (AirUp2FX2), GoForward (AirAtkStrong2), Follow (AirAtkStrong2), RotateSelf (AirAtkStrong2) |
| RightAttack2 | Follow (AirAtkR2), MoveAttack (AirAtkR2), RotateSelf (AirAtkR2) |
| LeftAttack2 | Follow (AirAtkL2), MoveAttack (AirAtkL2), RotateSelf (AirAtkL2) |
| Special | Follow (AirAtkTwisterSpecial), MoveWater (AirAtkTwisterSpecial), RotateSelf (AirAtkTwisterSpecial), GoForward (AirAtkStrongLow), RotateSelf (AirAtkStrongLow) |

## Vinea

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | GoForward (WaterWaveAtk), RotateSelf (WaterWaveAtk) |
| BasicAttack | Follow (FastWaterAtk), GoForward (FastWaterAtk), ChangeStartPosition (FastWaterAtk), RotateSelf (FastWaterAtk) |
| UpAttack2 | FollowOwner (WaterCircleUpAttack), RotateSelf (WaterCircleUpAttack), FollowOwner (WaterShieldAtk3), Follow (WaterAtkFront), GoForward (WaterAtkFront) |
| RightAttack | FollowOwner (WaterShieldRight), RotateSelf (WaterShieldRight) |
| LeftAttack | FollowOwner (WaterShieldLeft), RotateSelf (WaterShieldLeft) |
| BasicAttack2 | GoForward (WaterAtk2), RotateSelf (WaterAtk2), MoveWater (WaterAtk), RotateSelf (WaterAtk) |
| DownAttack2 | DropWhenOwnerWeak (WaterUpAtkFX), FollowOwner (WaterUpAtkFX), RotateSelf (WaterUpAtkFX), GoForward (FreezeUpAtk), Follow (FreezeUpAtk), PingPongMove (FreezeUpAtk) |
| UpAttack | Follow (IceAtk), GoForward (IceAtk), ChangeStartPosition (IceAtk), RotateSelf (IceAtk) |
| RightAttack2 | Rotate (WaterAtkR), MoveAttack (WaterAtkR), RotateSelf (WaterAtkR) |
| LeftAttack2 | Rotate (WaterAtkL), MoveAttack (WaterAtkL), RotateSelf (WaterAtkL) |
| Special | GoForward (FreezeAtk2), Follow (FreezeAtk2), RotateSelf (FreezeAtk2), Follow (IceAtkS1), MoveWater (IceAtkS1), RotateSelf (IceAtkS1), Follow (IceAtkS2), MoveWater (IceAtkS2), RotateSelf (IceAtkS2), Follow (IceAtkS3), MoveWater (IceAtkS3), RotateSelf (IceAtkS3), Follow (IceAtkS4), MoveWater (IceAtkS4), RotateSelf (IceAtkS4), Follow (IceAtkS5), MoveWater (IceAtkS5), RotateSelf (IceAtkS5) |

## Diana

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | MoveRock (RocksJump), Follow (RocksJump) |
| BasicAttack | MoveRock (RockH), Rotate (RockH) |
| UpAttack | MoveRock (BigRock) |
| RightAttack | MoveRock (RockR), Rotate (RockR) |
| LeftAttack | MoveRock (RockL), Rotate (RockL) |
| BasicAttack2 | MoveRock (RockH2), Rotate (RockH2) |
| RightAttack2 | MoveAttack (RockL2), RotateSelf (RockL2) |
| DownAttack2 | SumonInOpponent (EarthPrison), MoveObject (EarthPrison) |
| UpAttack2 | GoForward (RockPillar), Follow (RockPillar), MoveObject (RockPillarFX) |
| LeftAttack2 | MoveAttack (RockR2), RotateSelf (RockR2) |
| Special | DropAtk (EarthSpecial), MoveObject (EarthSpecial), MoveSpecialRock (EarthSpecial) |

## Amon

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | GoForward (NetworkFireWall), ChangeStartPosition (NetworkFireWall), GoForward (FirewallExplotion) |
| BasicAttack | GoForward (NetworkFireball) |
| UpAttack | GoForward (NetworkFirePillar), GoForward (FirepillarExplotion) |
| RightAttack | GoForward (FireKickHR), RotateSelf (FireKickHR) |
| LeftAttack | GoForward (FireKickHL), RotateSelf (FireKickHL) |
| BasicAttack2 | GoForward (FireFloor) |
| DownAttack2 | MoveAttack (FireDownAtk2), Follow (FireDownAtk2), ChangeStartPosition (FireDownAtk2) |
| UpAttack2 | RotateSelf (ChargeFire), GoForward (NetworkFirePillar2), Follow (NetworkFirePillar2), RotateSelf (NetworkFirePillar2), GoForward (FireSpecialExplotion) |
| RightAttack2 | GoForward (NetworkFireball R), Rotate (NetworkFireball R), Follow (NetworkFireball R) |
| LeftAttack2 | GoForward (NetworkFireball L), Follow (NetworkFireball L), Rotate (NetworkFireball L) |
| Special | GoForward (NetworkFireball SL), GoForward (NetworkFireball SR), GoForward (NetworkFirePillar), GoForward (NetworkFireWall), ChangeStartPosition (NetworkFireWall), GoForward (FireSpecial), Follow (FireSpecial), GoForward (FirepillarExplotion), GoForward (FirewallExplotion), GoForward (FireSpecialExplotion) |

## Aura

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | Follow (AirAtkTwister), MoveWater (AirAtkTwister), RotateSelf (AirAtkTwister) |
| BasicAttack | GoForward (AirAtk), RotateSelf (AirAtk) |
| UpAttack | GoForward (AirAtkStrong), RotateSelf (AirAtkStrong) |
| RightAttack | GoForward (AirAtkR), TurnAttack (AirAtkR), RotateSelf (AirAtkR) |
| LeftAttack | GoForward (AirAtkL), Rotate (AirAtkL), RotateSelf (AirAtkL) |
| BasicAttack2 | GoForward (AirAtk2), RotateSelf (AirAtk2) |
| DownAttack2 | RotateSelf (ChargeAir) |
| UpAttack2 | FollowOwner (AirUp2FX), FollowOwner (AirShieldAtk), FollowOwner (AirUp2FX2), GoForward (AirAtkStrong2), Follow (AirAtkStrong2), RotateSelf (AirAtkStrong2) |
| RightAttack2 | Follow (AirAtkR2), MoveAttack (AirAtkR2), RotateSelf (AirAtkR2) |
| LeftAttack2 | Follow (AirAtkL2), MoveAttack (AirAtkL2), RotateSelf (AirAtkL2) |
| Special | Follow (AirAtkTwisterSpecial), MoveWater (AirAtkTwisterSpecial), RotateSelf (AirAtkTwisterSpecial), GoForward (AirAtkStrongLow), RotateSelf (AirAtkStrongLow) |

## Eliot

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | GoForward (WaterWaveAtk), RotateSelf (WaterWaveAtk) |
| BasicAttack | Follow (FastWaterAtk), GoForward (FastWaterAtk), ChangeStartPosition (FastWaterAtk), RotateSelf (FastWaterAtk) |
| UpAttack2 | FollowOwner (WaterCircleUpAttack), RotateSelf (WaterCircleUpAttack), FollowOwner (WaterShieldAtk3), Follow (WaterAtkFront), GoForward (WaterAtkFront) |
| RightAttack | FollowOwner (WaterShieldRight), RotateSelf (WaterShieldRight) |
| LeftAttack | FollowOwner (WaterShieldLeft), RotateSelf (WaterShieldLeft) |
| BasicAttack2 | GoForward (WaterAtk2), RotateSelf (WaterAtk2), MoveWater (WaterAtk), RotateSelf (WaterAtk) |
| DownAttack2 | DropWhenOwnerWeak (WaterUpAtkFX), FollowOwner (WaterUpAtkFX), RotateSelf (WaterUpAtkFX), GoForward (FreezeUpAtk), Follow (FreezeUpAtk), PingPongMove (FreezeUpAtk) |
| UpAttack | Follow (IceAtk), GoForward (IceAtk), ChangeStartPosition (IceAtk), RotateSelf (IceAtk) |
| RightAttack2 | Rotate (WaterAtkR), MoveAttack (WaterAtkR), RotateSelf (WaterAtkR) |
| LeftAttack2 | Rotate (WaterAtkL), MoveAttack (WaterAtkL), RotateSelf (WaterAtkL) |
| Special | GoForward (FreezeAtk2), Follow (FreezeAtk2), RotateSelf (FreezeAtk2), Follow (IceAtkS1), MoveWater (IceAtkS1), RotateSelf (IceAtkS1), Follow (IceAtkS2), MoveWater (IceAtkS2), RotateSelf (IceAtkS2), Follow (IceAtkS3), MoveWater (IceAtkS3), RotateSelf (IceAtkS3), Follow (IceAtkS4), MoveWater (IceAtkS4), RotateSelf (IceAtkS4), Follow (IceAtkS5), MoveWater (IceAtkS5), RotateSelf (IceAtkS5) |

## Ren

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | MoveRock (RocksJump), Follow (RocksJump) |
| BasicAttack | MoveRock (RockH), Rotate (RockH) |
| UpAttack | MoveRock (BigRock) |
| RightAttack | MoveRock (RockR), Rotate (RockR) |
| LeftAttack | MoveRock (RockL), Rotate (RockL) |
| BasicAttack2 | MoveRock (RockH2), Rotate (RockH2) |
| RightAttack2 | MoveAttack (RockL2), RotateSelf (RockL2) |
| DownAttack2 | SumonInOpponent (EarthPrison), MoveObject (EarthPrison) |
| UpAttack2 | GoForward (RockPillar), Follow (RockPillar), MoveObject (RockPillarFX) |
| LeftAttack2 | MoveAttack (RockR2), RotateSelf (RockR2) |
| Special | DropAtk (EarthSpecial), MoveObject (EarthSpecial), MoveSpecialRock (EarthSpecial) |

## Rina

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | GoForward (NetworkFireWall), ChangeStartPosition (NetworkFireWall), GoForward (FirewallExplotion) |
| BasicAttack | GoForward (NetworkFireball) |
| UpAttack | GoForward (NetworkFirePillar), GoForward (FirepillarExplotion) |
| RightAttack | GoForward (FireKickHR), RotateSelf (FireKickHR) |
| LeftAttack | GoForward (FireKickHL), RotateSelf (FireKickHL) |
| BasicAttack2 | GoForward (FireFloor) |
| DownAttack2 | MoveAttack (FireDownAtk2), Follow (FireDownAtk2), ChangeStartPosition (FireDownAtk2) |
| UpAttack2 | RotateSelf (ChargeFire), GoForward (NetworkFirePillar2), Follow (NetworkFirePillar2), RotateSelf (NetworkFirePillar2), GoForward (FireSpecialExplotion) |
| RightAttack2 | GoForward (NetworkFireball R), Rotate (NetworkFireball R), Follow (NetworkFireball R) |
| LeftAttack2 | GoForward (NetworkFireball L), Follow (NetworkFireball L), Rotate (NetworkFireball L) |
| Special | GoForward (NetworkFireball SL), GoForward (NetworkFireball SR), GoForward (NetworkFirePillar), GoForward (NetworkFireWall), ChangeStartPosition (NetworkFireWall), GoForward (FireSpecial), Follow (FireSpecial), GoForward (FirepillarExplotion), GoForward (FirewallExplotion), GoForward (FireSpecialExplotion) |

## Dante

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | FollowOwner (ElectricityFXDown), FollowOwner (ElectricityFX), MoveLight (ShockAtk 2) |
| BasicAttack | MoveLight (ShockAtk), MoveLight (ShockAtk 1) |
| UpAttack | MoveLight (ShockAtk 3L), StartRotation (ShockAtk 3L), MoveLight (ShockAtk 3R), StartRotation (ShockAtk 3R), MoveLight (ShockAtk 3), FollowOwner (ElectricityFX) |
| RightAttack | MoveLight (ShockAtk R), StartRotation (ShockAtk R) |
| LeftAttack | MoveLight (ShockAtk L), StartRotation (ShockAtk L) |
| BasicAttack2 | MoveLight (ShockAtk S3), MoveLight (ShockAtk S2) |
| DownAttack2 | MoveObject (SumonThunderAtk), SumonInOpponent (SumonThunderAtk2), CreateObject (SumonThunderAtk2) |
| UpAttack2 | MoveLight (ShockAtk 3) |
| RightAttack2 | MoveLight (ShockAtk R2), StartRotation (ShockAtk R2) |
| LeftAttack2 | MoveLight (ShockAtk L2), StartRotation (ShockAtk L2) |
| Special | MoveLight (ShockAtk S2), MoveLight (ShockAtk S3), MoveLight (ShockAtk S1), MoveLight (ShockAtk 2), MoveLight (ShockAtk 3) |

## Agata

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | FollowOwner (ElectricityFXDown), FollowOwner (ElectricityFX), MoveLight (ShockAtk 2) |
| BasicAttack | MoveLight (ShockAtk), MoveLight (ShockAtk 1) |
| UpAttack | MoveLight (ShockAtk 3L), StartRotation (ShockAtk 3L), MoveLight (ShockAtk 3R), StartRotation (ShockAtk 3R), MoveLight (ShockAtk 3), FollowOwner (ElectricityFX) |
| RightAttack | MoveLight (ShockAtk R), StartRotation (ShockAtk R) |
| LeftAttack | MoveLight (ShockAtk L), StartRotation (ShockAtk L) |
| BasicAttack2 | MoveLight (ShockAtk S3), MoveLight (ShockAtk S2) |
| DownAttack2 | MoveObject (SumonThunderAtk), SumonInOpponent (SumonThunderAtk2), CreateObject (SumonThunderAtk2) |
| UpAttack2 | MoveLight (ShockAtk 3) |
| RightAttack2 | MoveLight (ShockAtk R2), StartRotation (ShockAtk R2) |
| LeftAttack2 | MoveLight (ShockAtk L2), StartRotation (ShockAtk L2) |
| Special | MoveLight (ShockAtk S2), MoveLight (ShockAtk S3), MoveLight (ShockAtk S1), MoveLight (ShockAtk 2), MoveLight (ShockAtk 3) |

## Raven

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | RotateSelf (ShadowTrampCircle) |
| BasicAttack | GoForward (ShadowArrow) |
| UpAttack | GoForward (ShadowArrowDown) |
| RightAttack | Follow (ShadowSpiderAtk), GoForward (ShadowSpiderAtk), ChangeStartPosition (ShadowSpiderAtk), EnableByDistance (ShadowSpiderAtk), RotateSelf (ShadowSpiderAtk), MoveObjectLocal (ShadowSpiderAtk) |
| LeftAttack | Follow (ShadowBatAtk), GoForward (ShadowBatAtk), ChangeStartPosition (ShadowBatAtk), EnableByDistance (ShadowBatAtk), RotateSelf (ShadowBatAtk) |
| BasicAttack2 | Follow (ShadowSharkAtk), ChangeStartPosition (ShadowSharkAtk), GoForward (ShadowSharkAtk), RotateSelf (MagicCircleSumon) |
| DownAttack2 | Follow (ShadowSnakeAtk), GoForward (ShadowSnakeAtk), ChangeStartPosition (ShadowSnakeAtk), RotateSelf (ShadowSnakeAtk) |
| UpAttack2 | GoForward (ShadowArrowTrap), ChangeStartRotation (ShadowArrowTrap), RotateSelf (ShadowTrapFar) |
| RightAttack2 | Follow (ShadowWolfAtk), GoForward (ShadowWolfAtk), ChangeStartPosition (ShadowWolfAtk), RotateSelf (ShadowWolfAtk), MoveObjectLocal (ShadowWolfAtk) |
| LeftAttack2 | Follow (ShadowBirdAtk), GoForward (ShadowBirdAtk), ChangeStartPosition (ShadowBirdAtk), RotateSelf (ShadowBirdAtk), MoveObjectLocal (ShadowBirdAtk) |
| Special | RotateSelf (MagicCircleFloorFX), Follow (BigDemonAtk), ChangeStartPosition (BigDemonAtk), GoForward (BigDemonAtk), RotateSelf (BigDemonAtk), CreateObjAnim (BigDemonAtk) |

## Maya

| State | Referenced motion / direction owners |
|---|---|
| DownAttack | SumonInOpponent (BranchPrisonATK), RotateSelf (BranchPrisonATK) |
| BasicAttack | MoveLight (LeafAtk), Follow (LeafAtk), RotateSelf (LeafAtk) |
| UpAttack2 | MoveLight (LeafAtkBig), Follow (LeafAtkBig), RotateSelf (LeafAtkBig), RotateByAngle (LeafAtkBig) |
| RightAttack | RotateSelf (LeafFrontHit), ChangeStartPosition (LeafAtkR), RotateToOpponent (LeafAtkR) |
| LeftAttack | RotateSelf (LeafFrontHit), ChangeStartPosition (LeafAtkL), RotateToOpponent (LeafAtkL) |
| BasicAttack2 | GoForward (FloorBeam), RotateSelf (FloorBeam), GoForward (FloorBeamSumonFX), GoForward (FloorBeamExplotion) |
| DownAttack2 | MoveForward (DragonWoodATK), Follow (DragonWoodATK), MovePath (DragonWoodATK), FollowPath (DragonWoodATK) |
| UpAttack | RotateSelf (GrassSpecialFX), MoveObjectLocal2 (GrassSpecialFX), RotateSelf (GrassSpecialFX2), MoveObjectLocal2 (GrassSpecialFX2), RotateByAngle (GrassSpecialFX2), GoForward (FlowerSpecial), ChangeStartPosition (FlowerBeam), Follow (FlowerBeam), MoveWater (FlowerBeam), RotateSelf (FlowerBeam), GoForward (FlowerSpecialExplotion) |
| RightAttack2 | MoveToTarget (FlowerSumonerL), RotateSelf (flowerBonfire) |
| LeftAttack2 | MoveToTarget (FlowerSumonerR), RotateSelf (flowerBonfire) |
| Special | MoveObjectLocal2 (LightColumn) |
