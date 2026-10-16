using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Editor-only audit. Does not enter Play Mode or assert gameplay equivalence.
public static class RecoveryValidation
{
    [Serializable] public class SceneAudit
    {
        public string path;
        public int gameObjects, missingScriptSlots, renderers, missingMaterialSlots;
        public string[] missingScriptObjects;
    }
    [Serializable] public class Report
    {
        public string unityVersion, generatedUtc, coverage;
        public bool compilationSucceeded;
        public SceneAudit[] scenes;
        public string[] limitations;
    }

    public static void Run()
    {
        var report = new Report {
            unityVersion = Application.unityVersion,
            generatedUtc = DateTime.UtcNow.ToString("O"),
            compilationSucceeded = true,
            coverage = "Imported serialized scenes; gameplay and online services not executed.",
            limitations = new [] {
                "Compilation and scene deserialization do not establish recovered method correctness.",
                "Missing renderer slots do not include dynamically assigned materials.",
                "Scene audit opens scenes in the editor; it does not enter Play Mode."
            }
        };
        var audits = new List<SceneAudit>();
        try {
            foreach (var guid in AssetDatabase.FindAssets("t:Scene", new [] {"Assets"})) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                var missing = new List<string>();
                int objectCount = 0, missingSlots = 0, rendererCount = 0, materialSlots = 0;
                foreach (var root in scene.GetRootGameObjects()) {
                    foreach (var transform in root.GetComponentsInChildren<Transform>(true)) {
                        objectCount++;
                        int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject);
                        if (count > 0) {
                            missingSlots += count;
                            missing.Add(HierarchyPath(transform) + " (" + count + ")");
                        }
                    }
                    foreach (var renderer in root.GetComponentsInChildren<Renderer>(true)) {
                        rendererCount++;
                        materialSlots += renderer.sharedMaterials.Count(m => m == null);
                    }
                }
                audits.Add(new SceneAudit {
                    path = path, gameObjects = objectCount, missingScriptSlots = missingSlots,
                    renderers = rendererCount, missingMaterialSlots = materialSlots,
                    missingScriptObjects = missing.ToArray()
                });
            }
            report.scenes = audits.ToArray();
            var directory = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "Evidence"));
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "unity-import-validation.json"), JsonUtility.ToJson(report, true));
            Debug.Log("REA_RECONSTRUCTION_IMPORT_AUDIT_COMPLETE " + audits.Count + " scenes");
            EditorApplication.Exit(0);
        } catch (Exception error) {
            Debug.LogException(error);
            EditorApplication.Exit(1);
        }
    }

    static string HierarchyPath(Transform transform)
    {
        var names = new List<string>();
        while (transform != null) { names.Add(transform.name); transform = transform.parent; }
        names.Reverse();
        return string.Join("/", names);
    }
}
