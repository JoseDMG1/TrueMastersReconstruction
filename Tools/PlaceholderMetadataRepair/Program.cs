using System.Security.Cryptography;
using System.Text.Json;
using Mono.Cecil;

if (args.Length != 1) throw new ArgumentException("Usage: PlaceholderMetadataRepair <UnityProject>");
var project = Path.GetFullPath(args[0]);
var plugins = Path.Combine(project, "Assets", "Plugins");
if (!Directory.Exists(plugins)) throw new DirectoryNotFoundException(plugins);
var evidence = Path.GetFullPath(Path.Combine(project, "..", "Evidence", "import-repairs"));
Directory.CreateDirectory(evidence);
var changes = new List<object>();
foreach (var file in Directory.EnumerateFiles(plugins, "*.dll", SearchOption.AllDirectories)) {
    using var assembly = AssemblyDefinition.ReadAssembly(file);
    var removed = new List<string>();
    Visit(assembly.MainModule.Types, removed);
    if (removed.Count == 0) continue;
    var before = Hash(file);
    var temporary = file + ".reconstruction.tmp";
    assembly.Write(temporary);
    assembly.Dispose();
    var backup = Path.Combine(evidence, Path.GetFileName(file) + "." + before + ".original");
    File.Replace(temporary, file, backup);
    changes.Add(new { file, before_sha256 = before, after_sha256 = Hash(file),
        reason = "Removed null-namespace MovedFrom attributes from generated dummy assembly; method bodies remain placeholders.",
        types = removed, backup });
}
if (changes.Count > 0) File.WriteAllText(Path.Combine(evidence, "placeholder-metadata-repairs.json"), JsonSerializer.Serialize(changes, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine(JsonSerializer.Serialize(new { changed_assemblies = changes.Count, evidence }));

static string Hash(string file) { using var s = File.OpenRead(file); return Convert.ToHexString(SHA256.HashData(s)).ToLowerInvariant(); }
static void Visit(IEnumerable<TypeDefinition> types, List<string> removed) {
    foreach (var type in types) {
        for (int i = type.CustomAttributes.Count - 1; i >= 0; i--) {
            var attr = type.CustomAttributes[i];
            if (attr.AttributeType.FullName == "UnityEngine.Scripting.APIUpdating.MovedFromAttribute" &&
                attr.ConstructorArguments.Count == 1 && attr.ConstructorArguments[0].Value == null) {
                removed.Add(type.FullName);
                type.CustomAttributes.RemoveAt(i);
            }
        }
        Visit(type.NestedTypes, removed);
    }
}
