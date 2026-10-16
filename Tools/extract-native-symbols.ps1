Add-Type -Path 'C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data\MonoBleedingEdge\lib\mono\gac\Mono.Cecil\0.11.1.0__0738eb9f132ed756\Mono.Cecil.dll'
$a=[Mono.Cecil.AssemblyDefinition]::ReadAssembly('C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\Cpp2IL-ILRecovery\Assembly-CSharp.dll')
$rows=New-Object System.Collections.Generic.List[object]
$fields=New-Object System.Collections.Generic.List[object]
function AttrData($member){$out=@{};foreach($c in $member.CustomAttributes){foreach($f in $c.Fields){$out[$f.Name]=$f.Argument.Value}};return $out}
function VisitTypes($types){foreach($t in $types){foreach($m in $t.Methods){$attrs=AttrData $m;$rows.Add([pscustomobject]@{declaring_type=$t.FullName;method_full_name=$m.FullName;method_name=$m.Name;return_type=$m.ReturnType.FullName;parameters=@($m.Parameters | ForEach-Object {[pscustomobject]@{name=$_.Name;type=$_.ParameterType.FullName}});attributes=$attrs})};foreach($f in $t.Fields){$fields.Add([pscustomobject]@{declaring_type=$t.FullName;name=$f.Name;type=$f.FieldType.FullName;is_static=$f.IsStatic;attributes=(AttrData $f)})};VisitTypes $t.NestedTypes}}
VisitTypes $a.MainModule.Types
@{methods=$rows.ToArray();fields=$fields.ToArray()} | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath 'C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\cpp2il-native-symbols-raw.json' -Encoding UTF8
$a.Dispose()
