using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

/// <summary>Uses a mobile-specific display name while building Android players.</summary>
public sealed class AndroidProductNameBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string AndroidProductName = "TM Rebuilding";
    private static string previousProductName;
    private static bool restoreProductName;

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android) return;

        previousProductName = PlayerSettings.productName;
        restoreProductName = true;
        PlayerSettings.productName = AndroidProductName;
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android || !restoreProductName) return;

        PlayerSettings.productName = previousProductName;
        previousProductName = null;
        restoreProductName = false;
    }
}
