using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Native Score owns the timer and displays; ScoreController owns points and results.
public class Score : MonoBehaviour
{
    public static Score score;
    public TMP_Text finishText;
    public bool battleFinish;
    public GameObject finishMenu;
    public int playerPoints;
    public int enemyPoints;
    public TMP_Text p1Score;
    public TMP_Text p2Score;
    public TMP_Text dash;
    public TMP_Text timeText;
    public int time = 300;
    public Image timeBG;
    public Color alertColor;
    public OnlineChat chat;
    public UnityEvent onTimeOut = new UnityEvent();

    private void Awake()
    {
        score = this;
        if (SystemManager.i.mode == GameMode.Survival)
        {
            dash.text = "0";
            p1Score.text = "";
            p2Score.text = "";
            timeText.text = "";
            timeBG.color = Color.clear;
            return;
        }
        time = PlayerPrefs.GetInt("Timer");
        timeText.text = Utilities.i.GetTimeAsString(time);
        if (GameController.i != null && GameController.i.online)
        {
            Debug.LogWarning("Online timer synchronization has not been reconstructed.", this);
            return;
        }
        StartCoroutine(ReduceTime());
    }
    private IEnumerator ReduceTime()
    {
        while (BattleSceneSingleton.i.counter != null) yield return null;
        while (!battleFinish)
        {
            yield return new WaitForSeconds(Time.timeScale);
            if (battleFinish) yield break;
            time--;
            timeText.text = Utilities.i.GetTimeAsString(time);
            if (time <= 30) timeBG.color = alertColor;
            if (time <= 0)
            {
                onTimeOut?.Invoke();
                if (SystemManager.i.mode != GameMode.LocalMultiplayer) StartCoroutine(TimeUp());
                yield break;
            }
        }
    }
    public IEnumerator TimeUp()
    {
        battleFinish = true;
        finishText.text = "Time's Up!";
        finishText.enabled = true;
        GetComponent<AudioSource>().Play();
        CameraManager.cameraM.ChangeToSceneCameraClose();
        var controller = GameController.i.MainCharacter.GetComponent<ScoreController>();
        controller.disableMovement(HostPlayer.hostPlayer.gameObject);
        controller.disableMovement(GuestPlayer.guestPlayer.gameObject);
        if (SystemManager.i.mode == GameMode.LocalMultiplayer)
        {
            Debug.LogWarning("Local multiplayer timeout results require unreconstructed ScoreController_LM.", this);
            yield break;
        }
        yield return new WaitForSeconds(4f);
        controller.TimeOver();
    }
    public IEnumerator TimerBattleFinish()
    {
        if (GameController.i.online)
        {
            Debug.LogWarning("Online result restart synchronization has not been reconstructed.", this);
            yield break;
        }
        time = 20;
        timeBG.color = Color.black;
        while (time > 0)
        {
            yield return new WaitForSeconds(Time.timeScale);
            time--;
            timeText.text = Utilities.i.GetTimeAsString(time);
        }
        GameController.i.Restart();
    }
    public IEnumerator OnlineTimer()
    {
        Debug.LogWarning("Online timer synchronization has not been reconstructed.", this);
        yield break;
    }
    public void UpdateScore()
    {
        if (SystemManager.i.mode == GameMode.Survival)
        {
            dash.text = playerPoints.ToString();
            p1Score.text = "";
            p2Score.text = "";
        }
        else
        {
            p1Score.text = playerPoints.ToString();
            p2Score.text = enemyPoints.ToString();
        }
    }
    public void UpdateSpectatorScore()
    {
        p1Score.text = playerPoints.ToString();
        p2Score.text = enemyPoints.ToString();
    }
    public void CheckSpectatorScore()
    {
        if (playerPoints == 3 && enemyPoints == 3)
            GameController.i.MainCharacter.GetComponent<ScoreController>().CheckPoints();
        else if (enemyPoints == 3)
            GameController.i.MainCharacter.GetComponent<ScoreController>().FoeWin();
        else if (playerPoints == 3)
            GameController.i.OpponentCharacter.GetComponent<ScoreController>().FoeWin();
        else return;
        finishMenu.SetActive(true);
    }
}
