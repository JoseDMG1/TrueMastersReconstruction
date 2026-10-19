param(
    [string]$Project = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\WorkingBackups\AndroidArm64Validation-20261019\UnityProject',
    [string]$Output = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\AndroidArm64Research-20261019\plugin-audit.json'
)
$ErrorActionPreference = 'Stop'
[void][Reflection.Assembly]::LoadFrom('C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data\Managed\Unity.Cecil.dll')
$assets = Join-Path $Project 'Assets'
$game = [Mono.Cecil.AssemblyDefinition]::ReadAssembly((Join-Path $Project 'Library\Bee\PlayerScriptAssemblies\Assembly-CSharp.dll'))
try { $gameReferences = @($game.MainModule.AssemblyReferences | ForEach-Object Name) }
finally { $game.Dispose() }
$inventory = foreach ($file in Get-ChildItem -LiteralPath (Join-Path $assets 'Plugins') -Filter '*.dll') {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName)
    try {
        $pinvokes = @()
        $methodCount = 0
        foreach ($type in $assembly.MainModule.GetTypes()) {
            foreach ($method in $type.Methods) {
                $methodCount++
                if ($method.IsPInvokeImpl) {
                    # HasPInvokeInfo tests the PInvokeImpl flag too; inspect the object itself.
                    $info = $method.PInvokeInfo
                    $pinvokes += [pscustomobject]@{
                        method = $method.FullName
                        missingInfo = ($null -eq $info)
                        missingModule = ($null -eq $info.Module)
                        module = $info.Module.Name; entryPoint = $info.EntryPoint
                    }
                }
            }
        }
        $guid = $null
        $references = @()
        if (Test-Path -LiteralPath ($file.FullName + '.meta')) {
            $line = Get-Content -LiteralPath ($file.FullName + '.meta') | Where-Object { $_ -match '^guid:' }
            $guid = $line -replace '^guid:\s*', ''
            if ($guid) {
                $references = @(& rg -l --fixed-strings --glob '*.prefab' --glob '*.unity' --glob '*.asset' $guid $assets)
                if ($LASTEXITCODE -gt 1) { throw "Asset reference scan failed for $file" }
            }
        }
        [pscustomobject]@{
            name = $file.Name; path = $file.FullName
            version = $assembly.Name.Version.ToString(); kind = $assembly.MainModule.Kind.ToString()
            entryPoint = if ($assembly.EntryPoint) { $assembly.EntryPoint.FullName } else { $null }
            hasPublicKey = $assembly.Name.HasPublicKey
            sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
            directGameReference = ($gameReferences -contains $assembly.Name.Name)
            assemblyReferences = @($assembly.MainModule.AssemblyReferences | ForEach-Object Name)
            methodCount = $methodCount; pinvokes = $pinvokes
            guid = $guid; serializedAssetReferences = $references
        }
    } finally { $assembly.Dispose() }
}
$report = [pscustomobject]@{
    generatedUtc = [DateTime]::UtcNow.ToString('O')
    project = $Project
    scope = 'Read-only managed metadata and text-serialized asset reference audit. No asset references found does not exclude dynamic or binary references.'
    plugins = @($inventory)
}
$report | ConvertTo-Json -Depth 9 | Set-Content -LiteralPath $Output -Encoding UTF8
$inventory | Select-Object name, kind, directGameReference, methodCount, @{n='BadPInvoke';e={@($_.pinvokes | Where-Object missingInfo).Count}}, @{n='AssetRefs';e={$_.serializedAssetReferences.Count}} | Format-Table -AutoSize
