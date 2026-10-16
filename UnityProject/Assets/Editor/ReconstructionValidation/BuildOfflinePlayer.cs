using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildOfflinePlayer
{
    [Serializable] public class Report {
        public string generatedUtc, unityVersion, result, output;
        public double seconds;
        public ulong bytes;
        public int errors, warnings;
        public string limitation = "Development Windows build; unsupported reconstructed features remain incomplete. Build success alone does not establish playability.";
    }
    public static void Run() {
        var root = Path.GetFullPath(Path.Combine(Application.dataPath, "../.."));
        var output = Path.Combine(root, "Builds/Windows/TrueMastersOffline.exe");
        Directory.CreateDirectory(Path.GetDirectoryName(output));
        var build = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = output,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.Development
        });
        var summary = build.summary;
        var report = new Report { generatedUtc = DateTime.UtcNow.ToString("O"), unityVersion = Application.unityVersion,
            result = summary.result.ToString(), output = output, seconds = summary.totalTime.TotalSeconds,
            bytes = summary.totalSize, errors = summary.totalErrors, warnings = summary.totalWarnings };
        File.WriteAllText(Path.Combine(root, "Evidence/windows-build-validation.json"), JsonUtility.ToJson(report, true));
        Debug.Log("OFFLINE_WINDOWS_BUILD_COMPLETE " + report.result);
        EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
    }
}
