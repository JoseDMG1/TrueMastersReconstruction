using UnityEngine;

// Corrected ARM64 0x1226430..0x12266C0. Raven evasion/attack appearance.
public class DarkModeTransform : MonoBehaviour
{
    public SkinnedMeshRenderer[] parts;
    public void DarkTransform(float time)
    {
        ChangeToBlack();
        Invoke(nameof(ChangeToNormal), time);
    }
    private void ChangeToBlack()
    {
        foreach (var part in parts)
        {
            var material = part.material;
            material.color = Color.black;
            material.SetFloat("_Glossiness", .2f);
            material.SetFloat("_Metallic", .5f);
        }
    }
    private void ChangeToNormal()
    {
        foreach (var part in parts)
        {
            var material = part.material;
            material.color = Color.white;
            material.SetFloat("_Glossiness", 0f);
            material.SetFloat("_Metallic", 0f);
        }
    }
}
