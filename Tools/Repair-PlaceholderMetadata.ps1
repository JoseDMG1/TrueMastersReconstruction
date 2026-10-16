param([string]$Project = (Join-Path $PSScriptRoot '..\UnityProject'))
$ErrorActionPreference = 'Stop'
$sdk = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data\DotNetSdk\dotnet.exe'
& $sdk run --project (Join-Path $PSScriptRoot 'PlaceholderMetadataRepair\PlaceholderMetadataRepair.csproj') -- ([IO.Path]::GetFullPath($Project))
if ($LASTEXITCODE -ne 0) { throw "Placeholder metadata repair failed ($LASTEXITCODE)." }
