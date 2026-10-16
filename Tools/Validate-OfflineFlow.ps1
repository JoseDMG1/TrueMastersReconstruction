param(
    [string]$Unity = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Unity.exe',
    [string]$Project = (Join-Path $PSScriptRoot '..\UnityProject')
)
$ErrorActionPreference = 'Stop'
$Project = [IO.Path]::GetFullPath($Project)
if (-not (Test-Path -LiteralPath $Unity)) { throw "Unity editor not found: $Unity" }
if (-not (Test-Path -LiteralPath (Join-Path $Project 'Assets'))) { throw "Unity Assets directory not found: $Project" }
$evidence = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\Evidence'))
$editor = Join-Path $Project 'Assets\Editor\ReconstructionValidation'
New-Item -ItemType Directory -Path $editor -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\OfflineFlowValidation.cs') -Destination $editor -Force
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$log = Join-Path $evidence ("unity-offline-flow-$stamp.log")
$report = Join-Path $evidence 'offline-flow-validation.json'
if (Test-Path -LiteralPath $report) {
    Copy-Item -LiteralPath $report -Destination (Join-Path $evidence ("offline-flow-validation-before-$stamp.json"))
}
$arguments = @('-batchmode', '-nographics', '-projectPath', ('"' + $Project + '"'), '-executeMethod', 'OfflineFlowValidation.Run', '-logFile', ('"' + $log + '"'))
$process = Start-Process -FilePath $Unity -ArgumentList $arguments -PassThru -WindowStyle Hidden
[pscustomobject]@{ProcessId=$process.Id; Project=$Project; Log=$log; Report=$report; StartedUtc=[DateTime]::UtcNow.ToString('O')} | ConvertTo-Json
