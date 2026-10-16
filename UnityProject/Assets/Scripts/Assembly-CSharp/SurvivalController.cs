using UnityEngine;

public class SurvivalController : MonoBehaviour
{
    public static SurvivalController survivalController;
    public bool survivalMode;
    public int currentChar;
    public bool disableRespawn;

    private void Start()
    {
        currentChar = 0;
        survivalController = this;
    }
    private void OnDestroy()
    {
        // Reconstruction cleanup keeps ownership when other scenes replace this singleton.
        if (survivalController == this) survivalController = null;
    }
    public void SurvivalModeOn() { survivalMode = true; }
    public void RespawnOff()
    {
        disableRespawn = true;
        Invoke(nameof(RespawnOn), 1f);
    }
    public void RespawnOn() { disableRespawn = false; }
}
