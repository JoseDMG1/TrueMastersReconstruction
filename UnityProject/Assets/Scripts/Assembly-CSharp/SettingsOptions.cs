using UnityEngine;
// Corrected ARM64 0x12BD044..0x12BD110. Native Start/Update are RET.
public class SettingsOptions : MonoBehaviour
{
    public Animator anim;
    public GameObject closeBG;
    public void CloseSettings()
    {
        anim.Play("SettingsSlideOut");
        closeBG.SetActive(false);
    }
    public void OpenSettings()
    {
        anim.Play("SettingsSlideIn");
        closeBG.SetActive(true);
    }
}
