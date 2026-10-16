param(
    [string]$Unity = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Unity.exe',
    [string]$Project = (Join-Path $PSScriptRoot '..\UnityProject')
)
$ErrorActionPreference = 'Stop'
$Project = [IO.Path]::GetFullPath($Project)
if (-not (Test-Path -LiteralPath $Unity)) { throw "Unity editor not found: $Unity" }
if (-not (Test-Path -LiteralPath (Join-Path $Project 'Assets'))) { throw "Unity Assets directory not found: $Project" }
$evidence = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\Evidence'))
New-Item -ItemType Directory -Path $evidence -Force | Out-Null
$editor = Join-Path $Project 'Assets\Editor\ReconstructionValidation'
New-Item -ItemType Directory -Path $editor -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-validation\RecoveryValidation.cs') -Destination $editor -Force
$log = Join-Path $evidence 'unity-import.log'
$arguments = @('-batchmode', '-nographics', '-projectPath', ('"' + $Project + '"'), '-executeMethod', 'RecoveryValidation.Run', '-logFile', ('"' + $log + '"'))
$process = Start-Process -FilePath $Unity -ArgumentList $arguments -PassThru -WindowStyle Hidden
[pscustomobject]@{ProcessId=$process.Id; Project=$Project; Log=$log} | ConvertTo-Json
