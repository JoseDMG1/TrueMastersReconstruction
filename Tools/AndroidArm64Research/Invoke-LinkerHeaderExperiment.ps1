param(
    [string]$ValidationRoot = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\WorkingBackups\AndroidArm64Validation-20261019',
    [string]$EditorData = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data',
    [string]$EvidenceRoot = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\AndroidArm64Research-20261019'
)
$ErrorActionPreference = 'Stop'
$project = Join-Path $ValidationRoot 'UnityProject'
$plugins = Join-Path $project 'Assets\Plugins'
$experiment = Join-Path $EvidenceRoot ('header-ab-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
New-Item -ItemType Directory -Path $experiment -Force | Out-Null
[void][Reflection.Assembly]::LoadFrom((Join-Path $EditorData 'Managed\Unity.Cecil.dll'))

$inventory = foreach ($file in Get-ChildItem -LiteralPath $plugins -Filter '*.dll') {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName)
    try {
        [pscustomobject]@{
            name = $file.Name
            path = $file.FullName
            sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
            kind = $assembly.MainModule.Kind.ToString()
            entryPoint = if ($assembly.EntryPoint) { $assembly.EntryPoint.FullName } else { $null }
            hasPublicKey = $assembly.Name.HasPublicKey
        }
    } finally { $assembly.Dispose() }
}
$inventory | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $experiment 'plugin-inventory.json') -Encoding UTF8

$linkerAssemblyPath = Join-Path $EditorData 'il2cpp\build\deploy\monolinkerold.dll'
$linkerAssembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($linkerAssemblyPath)
try {
    $type = $linkerAssembly.MainModule.Types | Where-Object FullName -eq 'Mono.Linker.Old.Steps.ResolveFromAssemblyStep'
    $instructions = foreach ($method in $type.Methods | Where-Object Name -in @('Process', 'ProcessExecutable')) {
        $method.FullName
        foreach ($instruction in $method.Body.Instructions) { $instruction.ToString() }
    }
    $instructions | Set-Content -LiteralPath (Join-Path $experiment 'ResolveFromAssemblyStep.il.txt') -Encoding UTF8
} finally { $linkerAssembly.Dispose() }

# Change one PE header flag in an evidence copy. No source or installed SDK is modified.
$source = Join-Path $plugins 'Unity.VectorGraphics.dll'
$sourceBytes = [IO.File]::ReadAllBytes($source)
$peOffset = [BitConverter]::ToInt32($sourceBytes, 0x3c)
if ([BitConverter]::ToUInt32($sourceBytes, $peOffset) -ne 0x4550) { throw 'Not a PE image' }
$characteristicsOffset = $peOffset + 22
$flags = [BitConverter]::ToUInt16($sourceBytes, $characteristicsOffset)
if (($flags -band 0x2000) -ne 0) { throw 'Control is already marked as a DLL' }
$cases = @()
foreach ($case in @('control', 'dll-header')) {
    $inputDirectory = Join-Path $experiment ($case + '-input')
    New-Item -ItemType Directory -Path $inputDirectory | Out-Null
    $inputPath = Join-Path $inputDirectory 'Unity.VectorGraphics.dll'
    [byte[]]$bytes = $sourceBytes.Clone()
    if ($case -eq 'dll-header') {
        $replacement = [BitConverter]::GetBytes([uint16]($flags -bor 0x2000))
        $bytes[$characteristicsOffset] = $replacement[0]
        $bytes[$characteristicsOffset + 1] = $replacement[1]
    }
    [IO.File]::WriteAllBytes($inputPath, $bytes)
    $arguments = @(
        ('--include-assembly=' + $inputPath),
        ('--search-directory=' + $plugins),
        ('--search-directory=' + (Join-Path $project 'Library\Bee\PlayerScriptAssemblies')),
        ('--search-directory=' + (Join-Path $EditorData 'BCLExtensions\runtime\netstandard2.1')),
        ('--search-directory=' + (Join-Path $EditorData 'MonoBleedingEdge\lib\mono\unityaot-linux')),
        ('--search-directory=' + (Join-Path $EditorData 'MonoBleedingEdge\lib\mono\unityaot-linux\Facades')),
        ('--search-directory=' + (Join-Path $EditorData 'PlaybackEngines\AndroidPlayer\Variations\il2cpp\Managed')),
        '--dotnetprofile=unityaot-linux', '--dotnetruntime=Il2Cpp', '--platform=Android', '--architecture=Arm64',
        ('--out=' + (Join-Path $experiment ($case + '-output')))
    )
    $responsePath = Join-Path $experiment ($case + '.rsp')
    $responseText = ($arguments | ForEach-Object { '"' + $_ + '"' }) -join ' '
    [IO.File]::WriteAllText($responsePath, $responseText)
    $output = & (Join-Path $EditorData 'il2cpp\build\deploy\UnityLinker.exe') ('@' + $responsePath) 2>&1
    $exitCode = $LASTEXITCODE
    $output | Set-Content -LiteralPath (Join-Path $experiment ($case + '.log')) -Encoding UTF8
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($inputPath)
    try {
        $cases += [pscustomobject]@{
            case = $case; exitCode = $exitCode; kind = $assembly.MainModule.Kind.ToString()
            sha256 = (Get-FileHash -LiteralPath $inputPath -Algorithm SHA256).Hash
            originalException = [bool](($output -join "`n") -match 'ResolveFromAssemblyStep.ProcessExecutable')
            outputTail = @($output | Select-Object -Last 8 | ForEach-Object { $_.ToString() })
        }
    } finally { $assembly.Dispose() }
}
$report = [pscustomobject]@{
    generatedUtc = [DateTime]::UtcNow.ToString('O')
    experiment = $experiment; source = $source
    sourceUnchanged = ((Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash -eq ($inventory | Where-Object name -eq 'Unity.VectorGraphics.dll').sha256)
    changedOffset = $characteristicsOffset + 1
    change = 'IMAGE_FILE_DLL (0x2000) enabled in evidence copy only; method bodies unchanged'
    linkerSha256 = (Get-FileHash -LiteralPath $linkerAssemblyPath -Algorithm SHA256).Hash
    cases = $cases
}
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $experiment 'result.json') -Encoding UTF8
$report | ConvertTo-Json -Depth 8
