using UnityEngine;
// Corrected ARM64 0x123B244..0x123B370.
public class IcePrisonController : MonoBehaviour
{
    public static IcePrisonController i;
    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) { Destroy(this); return; }
    }
    private void OnDestroy() { if (i == this) i = null; }
}
