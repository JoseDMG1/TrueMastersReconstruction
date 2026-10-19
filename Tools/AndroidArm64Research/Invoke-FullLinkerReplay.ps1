param(
    [string]$ValidationRoot = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\WorkingBackups\AndroidArm64Validation-20261019',
    [string]$EditorData = 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data',
    [string]$EvidenceRoot = 'C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\AndroidArm64Research-20261019'
)
$ErrorActionPreference = 'Stop'
$project = Join-Path $ValidationRoot 'UnityProject'
$plugins = Join-Path $project 'Assets\Plugins'
$experiment = Join-Path $EvidenceRoot ('full-replay-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$corrected = Join-Path $experiment 'corrected-plugins'
New-Item -ItemType Directory -Path $corrected -Force | Out-Null
$changes = @()
foreach ($file in Get-ChildItem -LiteralPath $plugins -Filter '*.dll') {
    $bytes = [IO.File]::ReadAllBytes($file.FullName)
    $pe = [BitConverter]::ToInt32($bytes, 0x3c)
    if ([BitConverter]::ToUInt32($bytes, $pe) -ne 0x4550) { throw "Not a PE image: $file" }
    $offset = $pe + 22
    $flags = [BitConverter]::ToUInt16($bytes, $offset)
    $originalHash = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
    $newFlags = [uint16]($flags -bor 0x2000)
    $replacement = [BitConverter]::GetBytes($newFlags)
    $bytes[$offset] = $replacement[0]
    $bytes[$offset + 1] = $replacement[1]
    $target = Join-Path $corrected $file.Name
    [IO.File]::WriteAllBytes($target, $bytes)
    $changes += [pscustomobject]@{
        source = $file.FullName; copy = $target; originalHash = $originalHash
        correctedHash = (Get-FileHash -LiteralPath $target -Algorithm SHA256).Hash
        oldFlags = ('{0:X4}' -f $flags); newFlags = ('{0:X4}' -f $newFlags)
    }
}
$changes | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $experiment 'header-copies.json') -Encoding UTF8

# The live .rsp was overwritten by the later Mono build. Recover the actual
# IL2CPP response text from the preserved Bee graph's WriteFile payload.
$dagPath = Join-Path $project 'Library\Bee\Player23ca5f54.dag.json'
$dag = Get-Content -LiteralPath $dagPath -Raw | ConvertFrom-Json
$responseNode = $dag.Nodes | Where-Object Annotation -eq 'WriteResponseFile Library/Bee/artifacts/rsp/13019162185569853530.rsp'
if (@($responseNode).Count -ne 1) { throw 'Expected one response-file payload' }
$payloadPath = $dag.PayloadsFile
if (![IO.Path]::IsPathRooted($payloadPath)) { $payloadPath = Join-Path $project $payloadPath }
$payloadBytes = [IO.File]::ReadAllBytes($payloadPath)
$rsp = [Text.Encoding]::UTF8.GetString($payloadBytes, $responseNode.PayloadOffset, $responseNode.PayloadLength)
if (!$rsp.Contains('--dotnetruntime=Il2Cpp') -or !$rsp.Contains('--architecture=Arm64') -or $rsp.Contains('--rule-set=Copy')) {
    throw 'Recovered payload is not the expected IL2CPP/ARM64 linker invocation'
}
$rsp | Set-Content -LiteralPath (Join-Path $experiment 'original.rsp') -Encoding UTF8
# Unity removes Temp when it closes. Capture the descriptor in evidence and point only
# this replay to it, rather than recreating/modifying files under the project.
$firebaseDescriptor = Join-Path $experiment 'FirebaseAdditionalLink.xml'
Copy-Item -LiteralPath (Join-Path $ValidationRoot 'Evidence\FirebaseAdditionalLink-captured.xml') -Destination $firebaseDescriptor
$rsp = [regex]::Replace($rsp, '--include-link-xml="[^"]*/Temp/FirebaseAdditionalLink.xml"', ('--include-link-xml="' + $firebaseDescriptor.Replace('\','/') + '"'))
$cases = @()
Push-Location $project
try {
    foreach ($case in @('control', 'dll-headers', 'offline-lan-simulation')) {
        $text = $rsp
        if ($case -ne 'control') {
            $text = $text.Replace($plugins.Replace('\','/'), $corrected.Replace('\','/'))
        }
        if ($case -eq 'offline-lan-simulation') {
            # Evidence-only equivalent of compiling out an unused desktop MP3 helper
            # and excluding its plugin from Android. This does not edit project code.
            $offlinePlugins = Join-Path $experiment 'offline-plugins'
            $offlinePlayers = Join-Path $experiment 'offline-player-assemblies'
            New-Item -ItemType Directory -Path $offlinePlugins, $offlinePlayers | Out-Null
            Get-ChildItem -LiteralPath $corrected -Filter '*.dll' | Where-Object Name -ne 'NAudio.dll' | Copy-Item -Destination $offlinePlugins
            $playerDirectory = Join-Path $project 'Library\Bee\PlayerScriptAssemblies'
            Get-ChildItem -LiteralPath $playerDirectory -Filter '*.dll' | Copy-Item -Destination $offlinePlayers
            [void][Reflection.Assembly]::LoadFrom((Join-Path $EditorData 'Managed\Unity.Cecil.dll'))
            $gamePath = Join-Path $offlinePlayers 'Assembly-CSharp.dll'
            $resolver = New-Object Mono.Cecil.DefaultAssemblyResolver
            $resolver.AddSearchDirectory($offlinePlayers)
            $directories = [regex]::Matches($rsp, '--allowed-assembly="([^"]+)"') |
                ForEach-Object { [IO.Path]::GetDirectoryName($_.Groups[1].Value) } | Select-Object -Unique
            foreach ($directory in $directories) { $resolver.AddSearchDirectory($directory) }
            $reader = New-Object Mono.Cecil.ReaderParameters
            $reader.AssemblyResolver = $resolver
            $game = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($gamePath, $reader)
            try {
                $helper = $game.MainModule.Types | Where-Object FullName -eq 'NAudioPlayer'
                if (!$helper) { throw 'Expected NAudioPlayer helper was not found' }
                foreach ($type in $game.MainModule.GetTypes() | Where-Object FullName -ne 'NAudioPlayer') {
                    foreach ($method in $type.Methods | Where-Object HasBody) {
                        foreach ($instruction in $method.Body.Instructions) {
                            if (($instruction.Operand -is [Mono.Cecil.MethodReference] -and $instruction.Operand.DeclaringType.FullName -eq 'NAudioPlayer') -or
                                ($instruction.Operand -is [Mono.Cecil.TypeReference] -and $instruction.Operand.FullName -eq 'NAudioPlayer')) {
                                throw ('NAudioPlayer has a real caller: ' + $method.FullName)
                            }
                        }
                    }
                }
                [void]$game.MainModule.Types.Remove($helper)
                foreach ($reference in @($game.MainModule.AssemblyReferences | Where-Object Name -eq 'NAudio')) {
                    [void]$game.MainModule.AssemblyReferences.Remove($reference)
                }
                $game.Write($gamePath + '.tmp')
            } finally { $game.Dispose(); $resolver.Dispose() }
            Move-Item -LiteralPath ($gamePath + '.tmp') -Destination $gamePath -Force
            $text = $text.Replace($corrected.Replace('\','/'), $offlinePlugins.Replace('\','/'))
            $text = $text.Replace($playerDirectory.Replace('\','/'), $offlinePlayers.Replace('\','/'))
            $text = [regex]::Replace($text, '--allowed-assembly="[^"]*/NAudio.dll"\s*', '')
            $text = [regex]::Replace($text, '--include-unity-root-assembly="[^"]*/NAudio.dll"\s*', '')
        }
        $outDirectory = (Join-Path $experiment ($case + '-output')).Replace('\','/')
        $text = [regex]::Replace($text, '--out="[^"]+"', ('--out="' + $outDirectory + '"'))
        $trace = (Join-Path $experiment ($case + '.traceevents')).Replace('\','/')
        $text = [regex]::Replace($text, '--profiler-output-file="[^"]+"', ('--profiler-output-file="' + $trace + '"'))
        $caseRsp = Join-Path $experiment ($case + '.rsp')
        [IO.File]::WriteAllText($caseRsp, $text)
        $output = & (Join-Path $EditorData 'il2cpp\build\deploy\UnityLinker.exe') ('@' + $caseRsp) 2>&1
        $exitCode = $LASTEXITCODE
        $output | Set-Content -LiteralPath (Join-Path $experiment ($case + '.log')) -Encoding UTF8
        $cases += [pscustomobject]@{
            case = $case; exitCode = $exitCode
            originalException = [bool](($output -join "`n") -match 'ResolveFromAssemblyStep.ProcessExecutable')
            errors = @($output | Where-Object { $_ -match 'error IL|Exception:|Fatal error' } | ForEach-Object { $_.ToString() })
            outputTail = @($output | Select-Object -Last 10 | ForEach-Object { $_.ToString() })
        }
    }
} finally { Pop-Location }
$unchanged = @($changes | Where-Object { (Get-FileHash -LiteralPath $_.source -Algorithm SHA256).Hash -ne $_.originalHash }).Count -eq 0
$report = [pscustomobject]@{
    generatedUtc = [DateTime]::UtcNow.ToString('O'); experiment = $experiment
    scope = 'Recovers the IL2CPP/ARM64 response file from the old Bee graph payload and replays it using current cached player assemblies; this is not a clean build or APK validation.'
    responseSource = $payloadPath; payloadOffset = $responseNode.PayloadOffset; payloadLength = $responseNode.PayloadLength
    projectPluginFilesUnchanged = $unchanged; copiedPluginCount = $changes.Count; cases = $cases
}
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $experiment 'result.json') -Encoding UTF8
$report | ConvertTo-Json -Depth 8
