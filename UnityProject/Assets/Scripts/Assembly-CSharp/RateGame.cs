using UnityEngine;

public class RateGame : MonoBehaviour
{
    private void Start() { OpenRatePopup(); }
    public void OpenRatePopup() { LerpManager.i.LerpScale(gameObject, Vector3.zero, Vector3.one, 0.5f); }
    public void CloseRatePopup() { LerpManager.i.LerpScale(gameObject, Vector3.one, Vector3.zero, 0.5f); }
    public void RateItNow()
    {
        Debug.LogWarning("Store rating is unavailable in this offline reconstruction.", this);
        CloseRatePopup();
    }
    public void NoRate() { PlayerPrefs.SetInt("RateGame", 100); CloseRatePopup(); }
    public void RemindLater() { PlayerPrefs.SetInt("RateGame", 0); CloseRatePopup(); }
}
