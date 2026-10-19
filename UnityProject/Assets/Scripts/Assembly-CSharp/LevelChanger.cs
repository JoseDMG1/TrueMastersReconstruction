using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    public static LevelChanger i;
    private int levelToLoad;
    public bool loadingScreen;

    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(this);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadetoLevelDelay(int level, float time = 1.7f)
    {
        StartCoroutine(FadeToLevelDelayCorutine(level, time));
    }

    private IEnumerator FadeToLevelDelayCorutine(int level, float time)
    {
        yield return new WaitForSeconds(time);
        FadeToLevel(level);
    }

    public void FadeToLevel(int levelIndex)
    {
        if (LoadingScreen.i.loading) return;
        LoadingScreen.i.loading = true;
        HideCrystalsNotification();

        // ARM64 0x122BA10..0x122BA34: skip tutorial only after first display.
        levelToLoad = SystemManager.i.howToPlayShowed && levelIndex == 3 ? 4 : levelIndex;
        if (levelToLoad == 3) SystemManager.i.howToPlayShowed = true;

        animator.SetTrigger("FadeOut");
        Invoke(nameof(OnFadeComplete), 1f);
    }

    public void OnFadeComplete()
    {
        LoadingScreen.i.StartLoading(levelToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        // Confirmed Quit returns to the menu from offline, LAN, and Photon screens.
        if (SystemManager.i != null) SystemManager.i.mode = GameMode.StartMenu;
        TCPConnection tcp = FindObjectOfType<TCPConnection>();
        if (tcp != null) tcp.CloseConnection();
        UDPConnectionManager udp = FindObjectOfType<UDPConnectionManager>();
        if (udp != null) udp.CloseConnection();
        if (Photon.Pun.PhotonNetwork.IsConnected) Photon.Pun.PhotonNetwork.Disconnect();
        FadeToLevel(1);
    }

    public void Disconnect()
    {
        Debug.LogError("Photon session shutdown has not been reconstructed.", this);
    }

    public void HideCrystalsNotification()
    {
        if (CrystalsNotification.i != null)
            CrystalsNotification.i.Hide();
    }
}

