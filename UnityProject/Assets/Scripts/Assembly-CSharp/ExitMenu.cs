using UnityEngine;

// Native ExitMenu uses the shared LerpManager, rather than changing time scale itself.
public class ExitMenu : MonoBehaviour
{
    public bool open;
    public void OpenMenu()
    {
        LerpManager.i.LerpScale(gameObject, Vector3.zero, Vector3.one, 0.5f);
        open = true;
    }
    public void CloseMenu()
    {
        LerpManager.i.LerpScale(gameObject, Vector3.one, Vector3.zero, 0.5f);
        open = false;
    }
    public void PlayAgain()
    {
        if (SystemManager.i.mode == GameMode.LocalMultiplayer) return;
        // Offline reconstruction keeps restart free of the original store-rating gate.
        GameController.i.Restart();
    }
    public void MainMenu() { LevelChanger.i.FadeToLevel(1); }
}
