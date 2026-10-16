using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Recovered offline path; connected loading and advertising remain unavailable.
public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen i;
    public Image bg;
    public Image logo;
    public TMP_Text progress;
    public GameObject vsText;
    public Text hostText;
    public Text guestText;
    private AsyncOperation asyncOperation;
    private float num;
    private float speed;
    public bool loading;
    public SpriteUIAnimation tutorial01;

    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(this);
    }

    private void FixedUpdate()
    {
        if (asyncOperation == null) return;
        num = Mathf.Lerp(num, 100f, speed);
        progress.text = ((int)num).ToString() + "%";
    }

    public void StartLoading(int scene)
    {
        num = 0f;
        speed = 0.0005f;
        bg.gameObject.SetActive(true);
        logo.gameObject.SetActive(true);
        progress.gameObject.SetActive(true);
        vsText.SetActive(true);
        StartCoroutine(LoadScene(scene));
    }

    public IEnumerator LoadScene(int scene)
    {
        if (SystemManager.i.mode == GameMode.Online || SystemManager.i.mode == GameMode.Spectator)
        {
            Debug.LogError("Connected scene loading has not been reconstructed.", this);
            loading = false;
            yield break;
        }

        // Original mask 0x450 schedules interstitials for scenes 4, 6 and 10.
        // No ad request is made in this offline reconstruction.
        if (scene == 4 || scene == 6 || scene == 10)
            Invoke(nameof(ShowAd), 1f);

        vsText.SetActive(false);
        asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) speed = 0.1f;
            else if (asyncOperation.progress > 0.5f) speed = 0.005f;
            else if (asyncOperation.progress > 0.3f) speed = 0.001f;

            if (num > 99f) asyncOperation.allowSceneActivation = true;
            yield return new WaitForSeconds(0.1f);
        }

        // Preserve the old loading view until activation, then restore FadeIn.
        if (scene == 5) tutorial01.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        logo.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        vsText.SetActive(false);
        asyncOperation = null;
        progress.text = string.Empty;
        LevelChanger.i.animator.SetTrigger("FadeIn");
        loading = false;
    }

    private void ShowAd()
    {
        Debug.LogWarning("Interstitial advertising has not been reconstructed; no advertisement or reward action was performed.", this);
    }
}

