using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>Applies the Android app name and True Masters launcher icons during Android builds.</summary>
public sealed class AndroidProductNameBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string AndroidProductName = "TM Rebuilding";
    private static string previousProductName;
    private static bool restoreProductName;
    private static Dictionary<PlatformIconKind, Texture2D[][]> previousAndroidIcons;
    private static readonly PlatformIconKind[] AndroidIconKinds =
    {
        AndroidPlatformIconKind.Adaptive
    };

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android) return;

        var iconForeground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Texture2D/TrueMastersAndroidIconForeground.png");
        var iconBackground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Texture2D/TrueMastersAndroidIconBackground.png");
        if (!iconForeground || !iconBackground)
            throw new BuildFailedException("True Masters Android adaptive icon layers were not found in Assets/Texture2D.");

        previousProductName = PlayerSettings.productName;
        restoreProductName = true;
        PlayerSettings.productName = AndroidProductName;
        ConfigureAndroidIcons(iconForeground, iconBackground);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android) return;

        RestoreAfterBuild();
    }

    public static void RestoreAfterBuild()
    {
        if (restoreProductName)
        {
            PlayerSettings.productName = previousProductName;
            previousProductName = null;
            restoreProductName = false;
        }
        RestoreAndroidIcons();
    }

    private static void ConfigureAndroidIcons(Texture2D foreground, Texture2D background)
    {
        previousAndroidIcons = new Dictionary<PlatformIconKind, Texture2D[][]>();
        foreach (var kind in AndroidIconKinds)
        {
            var icons = PlayerSettings.GetPlatformIcons(NamedBuildTarget.Android, kind);
            previousAndroidIcons[kind] = icons.Select(icon => icon.GetTextures()).ToArray();
            foreach (var icon in icons)
            {
                var layers = new Texture2D[Mathf.Max(2, icon.maxLayerCount)];
                layers[0] = background;
                layers[1] = foreground;
                for (int i = 2; i < layers.Length; i++) layers[i] = background;
                icon.SetTextures(layers);
            }
            PlayerSettings.SetPlatformIcons(NamedBuildTarget.Android, kind, icons);
        }
    }

    private static void RestoreAndroidIcons()
    {
        if (previousAndroidIcons == null) return;

        foreach (var pair in previousAndroidIcons)
        {
            var icons = PlayerSettings.GetPlatformIcons(NamedBuildTarget.Android, pair.Key);
            for (int i = 0; i < icons.Length && i < pair.Value.Length; i++)
            {
                var layers = pair.Value[i];
                if (layers == null || layers.Length == 0)
                    layers = new Texture2D[Mathf.Max(1, icons[i].maxLayerCount)];
                icons[i].SetTextures(layers);
            }
            PlayerSettings.SetPlatformIcons(NamedBuildTarget.Android, pair.Key, icons);
        }
        previousAndroidIcons = null;
    }
}
