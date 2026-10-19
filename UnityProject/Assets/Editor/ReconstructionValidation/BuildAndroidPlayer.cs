using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAndroidPlayer
{
    [Serializable]
    private sealed class Report
    {
        public string generatedUtc;
        public string unityVersion;
        public string result;
        public string output;
        public double seconds;
        public ulong bytes;
        public int errors;
        public int warnings;
    }

    public static void Run()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, "../.."));
        var output = Path.Combine(projectRoot, "Builds/Android/TM_Rebuilding_PortraitPrototype.apk");
        Directory.CreateDirectory(Path.GetDirectoryName(output));

        BuildReport build;
        var previousAndroidStrippingLevel = PlayerSettings.GetManagedStrippingLevel(NamedBuildTarget.Android);
        var previousAndroidScriptingBackend = PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android);
        var previousAndroidArchitectures = PlayerSettings.Android.targetArchitectures;
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Disabled);
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        try
        {
            build = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                locationPathName = output,
                target = BuildTarget.Android,
                options = BuildOptions.None
            });
        }
        finally
        {
            AndroidProductNameBuildProcessor.RestoreAfterBuild();
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, previousAndroidStrippingLevel);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, previousAndroidScriptingBackend);
            PlayerSettings.Android.targetArchitectures = previousAndroidArchitectures;
        }

        var summary = build.summary;
        var report = new Report
        {
            generatedUtc = DateTime.UtcNow.ToString("O"),
            unityVersion = Application.unityVersion,
            result = summary.result.ToString(),
            output = output,
            seconds = summary.totalTime.TotalSeconds,
            bytes = summary.totalSize,
            errors = summary.totalErrors,
            warnings = summary.totalWarnings
        };
        File.WriteAllText(Path.Combine(projectRoot, "Evidence/android-build-validation.json"), JsonUtility.ToJson(report, true));
        Debug.Log("ANDROID_APK_BUILD_COMPLETE " + report.result + " " + output);
        EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
    }
}
