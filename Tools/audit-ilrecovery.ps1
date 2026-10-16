Add-Type -Path 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data\MonoBleedingEdge\lib\mono\gac\Mono.Cecil\0.11.1.0__0738eb9f132ed756\Mono.Cecil.dll'
$p='C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\Cpp2IL-ILRecovery\Assembly-CSharp.dll'
$a=[Mono.Cecil.AssemblyDefinition]::ReadAssembly($p)
$rows=New-Object System.Collections.Generic.List[object]
function VisitTypes($types){foreach($t in $types){foreach($m in $t.Methods){$op=@($m.Body.Instructions | ForEach-Object {$_.OpCode.Name});$kind=if(-not $m.HasBody){'no-body'}elseif($op.Count -eq 2 -and $op[0] -eq 'ldnull' -and $op[1] -eq 'throw'){'throw-null-placeholder'}elseif($op.Count -eq 0){'empty'}else{'candidate-recovered'};$rows.Add([pscustomobject]@{type=$t.FullName;method=$m.FullName;kind=$kind;instructions=@($m.Body.Instructions | Where-Object { $null -ne $_ } | ForEach-Object {$_.ToString()})})};VisitTypes $t.NestedTypes}}
VisitTypes $a.MainModule.Types
$summary=@($rows | Group-Object kind | ForEach-Object {[pscustomobject]@{kind=$_.Name;count=$_.Count}})
@{source=$p;sha256=(Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLowerInvariant();summary=$summary;methods=$rows.ToArray()} | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath 'C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\cpp2il-ilrecovery-audit.json' -Encoding UTF8
$summary | ConvertTo-Json
$a.Dispose()

