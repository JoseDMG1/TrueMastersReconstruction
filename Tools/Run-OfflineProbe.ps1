param(
    [ValidateSet('Smoke','Rounds','ImpactAndRounds','Survival','Tournament','Roster','Menu','Graphics','Specials','Defenses','Prison','Moves','EarthPrison','Dragonwood','MayaProduction','DesktopControls','ShadowRoster','AbilityAim','ReportedFailures','MovingTarget','FlightMatrix','AwakeJumps','OctoberCorrections','LocalSelection','LocalSelectionUI','ThunderLight')][string]$Scenario = 'Smoke',
    [ValidateSet('Sora','Vinea','Diana','Amon','Aura','Eliot','Ren','Rina','Dante','Agata','Raven','Maya')][string]$Fighter,
    [string]$FlightProfile
)
$ErrorActionPreference = 'Stop'
$taskRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$taskExe = Join-Path $taskRoot 'Builds\Windows\TrueMastersOffline.exe'
if (-not (Test-Path -LiteralPath $taskExe)) { throw 'Build the Windows development player first.' }
$taskExisting = Get-Process -Name TrueMastersOffline -ErrorAction SilentlyContinue | Where-Object { $_.Path -eq $taskExe }
if ($taskExisting) { throw 'An owned player is already running. Finish its probe before another scenario writes shared reconstruction preferences/save data.' }
$taskStamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$taskPrefix = 'offline-' + $Scenario.ToLowerInvariant() + '-probe-' + $taskStamp
$taskReport = Join-Path $taskRoot ('Evidence\' + $taskPrefix + '.json')
$taskLog = Join-Path $taskRoot ('Evidence\' + $taskPrefix + '.log')
$taskArgs = @('-screen-fullscreen','0','-screen-width','960','-screen-height','540','-logFile',('"'+$taskLog+'"'),'-rea-offline-probe',('"'+$taskReport+'"'))
if ($Scenario -eq 'Rounds' -or $Scenario -eq 'ImpactAndRounds') { $taskArgs += '-rea-round-probe' }
if ($Scenario -eq 'ImpactAndRounds') { $taskArgs += '-rea-impact-probe' }
if ($Scenario -eq 'Survival') { $taskArgs += '-rea-survival-probe' }
if ($Scenario -eq 'Tournament') { $taskArgs += '-rea-tournament-probe' }
if ($Scenario -eq 'Roster') { $taskArgs += '-rea-roster-probe' }
if ($Scenario -eq 'OctoberCorrections') { $taskArgs += '-rea-october-corrections-probe' }
if ($Scenario -eq 'Menu') { $taskArgs += '-rea-menu-probe' }
if ($Scenario -eq 'LocalSelection') { $taskArgs += '-rea-local-selection-probe' }
if ($Scenario -eq 'LocalSelectionUI') { $taskArgs += @('-rea-local-selection-probe','-rea-ui-selection-probe') }
if ($Scenario -eq 'ThunderLight') { $taskArgs += '-rea-thunder-light-probe' }
if ($Scenario -eq 'AbilityAim') { $taskArgs += '-rea-ability-aim-probe' }
if ($Scenario -eq 'ReportedFailures') { $taskArgs += @('-rea-ability-aim-probe','-rea-reported-failures-probe') }
if ($Scenario -eq 'MovingTarget') { $taskArgs += @('-rea-ability-aim-probe','-rea-moving-target-probe'); if(-not $Fighter) {$Fighter='Sora'}; if($Fighter -ne 'Sora') {throw 'MovingTarget currently covers Sora UpAttack2 only.'} }
if ($Scenario -eq 'FlightMatrix') { $taskArgs += @('-rea-ability-aim-probe','-rea-flight-matrix-probe') }
if ($Scenario -eq 'AwakeJumps') { $taskArgs += @('-rea-ability-aim-probe','-rea-awake-jumps-probe') }
if ($FlightProfile) {
    $taskProfiles = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\FlightMatrixProfiles.json') -Raw | ConvertFrom-Json
    if($Scenario -ne 'FlightMatrix' -or @($FlightProfile.Split('|') | Where-Object {$_ -notin $taskProfiles.profiles.name}).Count -gt 0) {throw 'FlightProfile requires known FlightMatrix profile names separated by |.'}
    $taskArgs += @('-rea-flight-profile',('"'+$FlightProfile+'"'))
}
if ($Scenario -eq 'ShadowRoster') { $taskArgs += '-rea-shadow-roster-probe' }
if ($Scenario -eq 'Graphics') { $taskArgs += '-rea-graphics-probe' }
if ($Scenario -eq 'Specials') { $taskArgs += '-rea-specials-probe' }
if ($Scenario -eq 'Defenses') { $taskArgs += '-rea-defense-probe' }
if ($Scenario -eq 'Prison') { $taskArgs += '-rea-prison-probe' }
if ($Scenario -eq 'Moves') { $taskArgs += '-rea-moves-probe' }
if ($Scenario -eq 'EarthPrison') { $taskArgs += '-rea-earth-prison-probe' }
if ($Scenario -eq 'Dragonwood') { $taskArgs += '-rea-dragonwood-probe' }
if ($Scenario -eq 'DesktopControls') { $taskArgs += '-rea-desktop-controls-probe' }
if ($Scenario -eq 'MayaProduction') { $taskArgs += '-rea-maya-production-probe' }
if ($Fighter) {
    if ($Scenario -notin @('Specials','Defenses','Moves','AbilityAim','ReportedFailures','MovingTarget')) { throw '-Fighter selects the Specials, Defenses, Moves, AbilityAim, ReportedFailures or MovingTarget scenario.' }
    $taskArgs += @('-rea-fighter', $Fighter)
}
$taskProcess = Start-Process -FilePath $taskExe -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
$taskAssembly = Join-Path $taskRoot 'Builds\Windows\TrueMastersOffline_Data\Managed\Assembly-CSharp.dll'
$taskState = [pscustomobject]@{ ProcessId=$taskProcess.Id; Scenario=$Scenario; Fighter=$Fighter; StartedUtc=[DateTime]::UtcNow.ToString('O'); Exe=$taskExe; ExeSha256=(Get-FileHash -LiteralPath $taskExe -Algorithm SHA256).Hash.ToLower(); GameplayAssembly=$taskAssembly; GameplayAssemblySha256=(Get-FileHash -LiteralPath $taskAssembly -Algorithm SHA256).Hash.ToLower(); Report=$taskReport; Log=$taskLog; WaitedForCompletion=$false }
$taskState | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $taskRoot ('Evidence\' + $taskPrefix + '-launch.json')) -Encoding UTF8
$taskState | ConvertTo-Json

