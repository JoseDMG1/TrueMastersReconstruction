param([string]$ProjectRoot='C:\Users\Jose\Desktop\TrueMastersReconstruction')
$ErrorActionPreference='Stop'
$editor='C:\Program Files\Unity\Hub\Editor\6000.6.5f1\Editor\Data'
Add-Type -Path "$editor\MonoBleedingEdge\lib\mono\gac\Mono.Cecil\0.11.1.0__0738eb9f132ed756\Mono.Cecil.dll"
$official="$editor\Resources\PackageManager\ProjectTemplates\libcache\com.unity.template.urp-blank-17.2.1\ScriptAssemblies"
$rows=New-Object System.Collections.Generic.List[object]
foreach($origin in @('placeholder','official')){
  $folder=if($origin -eq 'official'){$official}else{"$ProjectRoot\UnityProject\Assets\Plugins"}
  $resolver=New-Object Mono.Cecil.DefaultAssemblyResolver
  foreach($search in @($folder,"$editor\Managed","$editor\Managed\UnityEngine")){$resolver.AddSearchDirectory($search)}
  $parameters=New-Object Mono.Cecil.ReaderParameters
  $parameters.AssemblyResolver=$resolver
  $allTypes=@{}
  $assemblies=@{}
  foreach($preload in @('UnityEngine.UI','Unity.TextMeshPro')){
    $loaded=[Mono.Cecil.AssemblyDefinition]::ReadAssembly("$folder\$preload.dll",$parameters)
    $assemblies[$preload]=$loaded
    foreach($loadedType in $loaded.MainModule.Types){$allTypes[$loadedType.FullName]=$loadedType}
  }
  foreach($name in @('UnityEngine.UI','Unity.TextMeshPro')){
    $assembly=$assemblies[$name]
    foreach($type in $assembly.MainModule.Types){
      if($type.Name -eq '<Module>'){continue}
      $fields=New-Object System.Collections.Generic.List[object]
      $seen=New-Object 'System.Collections.Generic.HashSet[string]'
      $current=$type
      while($null -ne $current -and $seen.Add($current.FullName)){
        foreach($field in $current.Fields){
          if($field.IsStatic -or $field.IsNotSerialized){continue}
          $serialized=$field.IsPublic -or @($field.CustomAttributes | Where-Object {$_.AttributeType.FullName -in @('UnityEngine.SerializeField','UnityEngine.SerializeReference')}).Count -gt 0
          if(-not $serialized){continue}
          $aliases=@($field.CustomAttributes | Where-Object {$_.AttributeType.FullName -eq 'UnityEngine.Serialization.FormerlySerializedAsAttribute'} | ForEach-Object {$_.ConstructorArguments[0].Value})
          $fields.Add([PSCustomObject]@{name=$field.Name;type=$field.FieldType.FullName;declaring_type=$current.FullName;aliases=$aliases})
        }
        if($null -eq $current.BaseType -or $current.BaseType.FullName.StartsWith('System.') -or $current.BaseType.FullName -in @('UnityEngine.MonoBehaviour','UnityEngine.Behaviour','UnityEngine.Component','UnityEngine.Object','UnityEngine.ScriptableObject')){break}
        $baseName=$current.BaseType.FullName
        if(-not $allTypes.ContainsKey($baseName)){break}
        $current=$allTypes[$baseName]
      }
      $rows.Add([PSCustomObject]@{origin=$origin;assembly=$name;assembly_file="$folder\$name.dll";full_name=$type.FullName;namespace=$type.Namespace;name=$type.Name;base_type=$type.BaseType.FullName;serialized_fields=@($fields.ToArray())})
    }
  }
  foreach($loaded in $assemblies.Values){$loaded.Dispose()}
}
$target="$ProjectRoot\Evidence\ui-type-schema.json"
[IO.File]::WriteAllText($target,(ConvertTo-Json -InputObject @($rows.ToArray()) -Depth 12))
$target
