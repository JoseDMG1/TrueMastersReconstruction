param([string]$Unity = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Unity.exe')
$ErrorActionPreference = 'Stop'
$taskRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$taskProject = Join-Path $taskRoot 'UnityProject'
$taskEditor = Join-Path $taskProject 'Assets\Editor\ReconstructionValidation'
$taskRuntime = Join-Path $taskProject 'Assets\ReconstructionValidation'
New-Item -ItemType Directory -Path $taskEditor, $taskRuntime -Force | Out-Null
node (Join-Path $PSScriptRoot 'audit-ability-aiming.mjs') | Out-Host
if ($LASTEXITCODE -ne 0) { throw 'Ability aiming inventory failed.' }
node (Join-Path $PSScriptRoot 'build-move-input-contracts.cjs') | Out-Host
if ($LASTEXITCODE -ne 0) { throw 'Move input contract generation failed.' }
node (Join-Path $PSScriptRoot 'build-flight-matrix.mjs') | Out-Host
if ($LASTEXITCODE -ne 0) { throw 'Flight profile inventory failed.' }
$taskProbeResources = Join-Path $taskRuntime 'Resources'
New-Item -ItemType Directory -Path $taskProbeResources -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\AbilityAimProfiles.json') -Destination $taskProbeResources -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineAbilityAimProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineMovingTargetProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineFlightMatrixProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineAwakeJumpProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\FlightMatrixProfiles.json') -Destination $taskProbeResources -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\MoveInputContracts.json') -Destination $taskProbeResources -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\BuildOfflinePlayer.cs') -Destination $taskEditor -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflinePlayerProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineEarthPrisonProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineDragonwoodProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineMayaProductionProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineAnimationColliderProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineColliderGeometryProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineDynamicShadowProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineShadowQualityProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineDesktopControlsProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineDianaRockProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\DesktopHudProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineOctoberCorrectionsProbe.cs') -Destination $taskRuntime -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineThunderLightProbe.cs') -Destination $taskRuntime -Force
$taskLog = Join-Path $taskRoot ('Evidence\unity-windows-build-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '.log')
$taskArgs = @('-batchmode', '-nographics', '-projectPath', ('"' + $taskProject + '"'), '-executeMethod', 'BuildOfflinePlayer.Run', '-logFile', ('"' + $taskLog + '"'))
$taskProcess = Start-Process -FilePath $Unity -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
[pscustomobject]@{ProcessId=$taskProcess.Id; Log=$taskLog; StartedUtc=[DateTime]::UtcNow.ToString('O')} | ConvertTo-Json
