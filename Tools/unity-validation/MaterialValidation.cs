using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class MaterialValidation
{
    [Serializable] public class Item { public string path, shader, mainTexture, bumpTexture; public Color color; public float metallic; }
    [Serializable] public class Report { public string generatedUtc; public Item[] materials; }
    public static void Run() {
        var paths = new[] { "Assets/Material/Dirt.mat", "Assets/Material/Dirt 1.mat" };
        var report = new Report { generatedUtc = DateTime.UtcNow.ToString("O"), materials = new Item[paths.Length] };
        for (int i=0; i<paths.Length; i++) {
            var m = AssetDatabase.LoadAssetAtPath<Material>(paths[i]);
            if (m == null) throw new InvalidOperationException(paths[i]);
            var bump = m.HasProperty("_BumpMap") ? m.GetTexture("_BumpMap") : null;
            report.materials[i] = new Item { path=paths[i], shader=m.shader.name, color=m.color,
                mainTexture=m.mainTexture ? AssetDatabase.GetAssetPath(m.mainTexture) : null,
                bumpTexture=bump ? AssetDatabase.GetAssetPath(bump) : null,
                metallic=m.HasProperty("_Metallic") ? m.GetFloat("_Metallic") : float.NaN };
        }
        File.WriteAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../Evidence/material-loaded-validation.json")), JsonUtility.ToJson(report,true));
        Debug.Log("MATERIAL_LOADED_PROBE_COMPLETE");
        EditorApplication.Exit(0);
    }
}
