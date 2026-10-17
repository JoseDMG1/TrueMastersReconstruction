using System.Collections;
using Photon.Pun;
using UnityEngine;

// Offline reconstruction from correctly mapped ARM64; see Knowledge/scoring-reconstruction.md.
public class ScoreController : MonoBehaviour
{
    public Score score;
    public Animator anim;
    public PhotonView PV;
    private bool newRecord;
    private bool finishPresented;

    private void Start()
    {
        score = Score.score;
        anim = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalZone") && !score.battleFinish)
        {
            transform.position = new Vector3(7f, 5f, 0f);
            return;
        }
        if (!other.CompareTag("DeathZone") || name == "DistanceCollider") return;
        if (GameController.i.online)
        {
            Debug.LogWarning("Online scoring synchronization is unavailable in the offline reconstruction.", this);
            return;
        }
        AddPoints();
        CheckPoints();
    }

    [PunRPC]
    public void AddPoints()
    {
        // This component belongs to the fighter who fell: the opponent receives the point.
        if (CompareTag("GuestPlayer")) score.playerPoints++;
        else score.enemyPoints++;
        score.UpdateScore();
    }

    private IEnumerator UpdateScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        score.UpdateScore();
    }

    public void Draw()
    {
        GameController.i.draw = true;
        score.battleFinish = true;
        CameraManager.cameraM.ChangeToSceneCameraFar();
        disableMovement(HostPlayer.hostPlayer.gameObject);
        disableMovement(GuestPlayer.guestPlayer.gameObject);
        Dance(HostPlayer.hostPlayer.gameObject);
        Dance(GuestPlayer.guestPlayer.gameObject);
        changeMusic();
        score.finishText.enabled = true;
        score.finishText.text = "DRAW";
        Invoke(nameof(finishMenu), 1f);
    }

    public void DanceSurvival() { Dance(HostPlayer.hostPlayer.gameObject); }

    public void Dance(GameObject winner)
    {
        // Native switch uses exact instantiated prefab names and Animator.Play state names.
        string state;
        switch (winner.name)
        {
            case "Rina(Clone)": state = "dance 3"; break;
            case "Dante(Clone)":
            case "Aura(Clone)":
            case "Maya(Clone)": state = "dance 1"; break;
            case "Ren(Clone)": state = "dance 6"; break;
            case "Amon(Clone)": state = "dance 8"; break;
            case "Eliot(Clone)": state = "dance 4"; break;
            case "Diana(Clone)": state = "dance 9"; break;
            case "Sora(Clone)": state = "dance 2"; break;
            case "Vinea(Clone)": state = "dance 7"; break;
            case "Agata(Clone)": state = "dance 3"; break;
            case "Raven(Clone)": state = "Sitting"; break;
            default: return;
        }
        winner.GetComponent<Animator>().Play(state);
    }

    [PunRPC]
    public void TimeOver()
    {
        if (SystemManager.i.mode == GameMode.SinglePlayer)
            BattleSceneSingleton.i.aiToggle.ChangeVisibility(false);
        int comparison = score.playerPoints.CompareTo(score.enemyPoints);
        if (comparison == 0)
            comparison = StatsController.i.GetPlayerLifeCrystals(false).CompareTo(StatsController.i.GetPlayerLifeCrystals(true));
        if (comparison == 0) Draw();
        else EndBattle(comparison > 0 ? HostPlayer.hostPlayer.gameObject : GuestPlayer.guestPlayer.gameObject);
    }

    public void EndBattle(GameObject winner)
    {
        GameObject loser = winner == HostPlayer.hostPlayer.gameObject ? GuestPlayer.guestPlayer.gameObject : HostPlayer.hostPlayer.gameObject;
        disableMovement(winner);
        showWinnerText(winner);
        changeCamera(winner);
        changeMusic();
        Dance(winner);
        Invoke(nameof(finishMenu), 1f);
        disableMovement(loser);
    }

    [PunRPC]
    public void CheckPoints()
    {
        if (SystemManager.i.mode == GameMode.Survival)
        {
            if (score.enemyPoints >= 1)
            {
                if (SystemManager.i.tmPlayer.survivalRecord < score.playerPoints)
                {
                    newRecord = true;
                    SystemManager.i.tmPlayer.survivalRecord = score.playerPoints;
                    // Offline persistence replaces the original Firebase SaveSurvivalRecord coroutine.
                    SystemManager.i.SaveData("survivalRecord");
                }
                var winner = HostPlayer.hostPlayer.gameObject;
                disableMovement(winner);
                showWinnerText(winner);
                changeCamera(winner);
                changeMusic();
                Destroy(GuestPlayer.guestPlayer.gameObject);
                Invoke(nameof(DanceSurvival), 1f);
                Invoke(nameof(finishMenu), 1f);
            }
            Falling();
            return;
        }
        if (score.enemyPoints == 3 && score.playerPoints == 3) { Draw(); return; }
        if (score.enemyPoints == 3 && CompareTag("HostPlayer")) { Death(); return; }
        if (score.playerPoints == 3 && CompareTag("GuestPlayer")) { Death(); return; }
        if (score.enemyPoints >= 4 && score.playerPoints >= 4) return;
        Falling();
    }

    public void FoeWin()
    {
        if (SystemManager.i.mode == GameMode.SinglePlayer)
            BattleSceneSingleton.i.aiToggle.ChangeVisibility(false);
        GetComponent<Rigidbody>().useGravity = false;
        GameObject winner = CompareTag("HostPlayer") ? GuestPlayer.guestPlayer.gameObject : HostPlayer.hostPlayer.gameObject;
        disableMovement(winner);
        showWinnerText(winner);
        changeCamera(winner);
        changeMusic();
        DisableSpecialButton();
        Dance(winner);
        if (GameController.i.draw) finishMenu();
        else Invoke(nameof(finishMenu), 2.5f);
    }

    private void finishMenu()
    {
        // Reconstruction guard: repeated collision/animation callbacks must not award twice.
        if (finishPresented) return;
        finishPresented = true;
        StartCoroutine(score.TimerBattleFinish());
        int played = PlayerPrefs.GetInt("RateGame");
        if (played != 100) PlayerPrefs.SetInt("RateGame", played + 1);
        if (GameController.i.SpecialButton != null) GameController.i.SpecialButton.SetActive(false);
        if (GameController.i.online)
        {
            Debug.LogWarning("Online finish menu is unavailable in the offline reconstruction.", this);
            return;
        }
        ResultsController.resultsController.InitResults();
    }

    private void DisableSpecialButton() { GameController.i.SpecialButton.SetActive(false); }
    private void showWinnerText(GameObject character)
    {
        if (SystemManager.i.mode == GameMode.Survival && !newRecord) return;
        score.finishText.enabled = true;
        score.finishText.text = SystemManager.i.mode == GameMode.Survival ? "New Record!" : "Winner!";
    }
    private void changeCamera(GameObject winner)
    {
        if (!GameController.i.draw) CameraManager.cameraM.ChangeCameraToWinner(winner);
    }
    private void changeMusic()
    {
        // Stage initialization is reconstructed separately; absent presentation must not prevent results.
        if (StageController.i == null || StageController.i.music == null) return;
        var music = StageController.i.music.GetComponents<AudioSource>();
        music[0].enabled = false;
        if (!GameController.i.draw) music[1].enabled = true;
    }
    public void disableMovement(GameObject player)
    {
        var animator = player.GetComponent<Animator>();
        animator.Rebind();
        animator.SetBool("Stunned", true);
        animator.SetBool("Weak", true);
        player.GetComponent<AnimationMovement>().enabled = false;
        player.GetComponent<DesktopInputs>().enabled = false;
        player.GetComponent<MobileInputs>().enabled = false;
        var localInput = player.GetComponent<LocalOfflinePlayerInput>();
        if (localInput) localInput.enabled = false;
        StartCoroutine(DropAtks(1f, player));
        player.GetComponent<LookAt>().enabled = false;
        // Reconstruction correction: preserve yaw using a valid quaternion (native zeroes x/z/w).
        player.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        var ai = player.GetComponent<AI>();
        if (ai.enabled) { ai.on = false; ai.enabled = false; }
    }
    public IEnumerator DropAtks(float time, GameObject winner)
    {
        yield return new WaitForSeconds(time);
        if (winner != null) winner.GetComponent<DamageOnCollision>().DropAllAtks();
    }
    private void Falling()
    {
        if (GameController.i.online) { Debug.LogWarning("Online fall synchronization is unavailable.", this); return; }
        anim.SetTrigger("Falling");
    }
    private void Death()
    {
        score.battleFinish = true;
        if (GameController.i.online) { Debug.LogWarning("Online death synchronization is unavailable.", this); return; }
        FinalDeath();
    }
    [PunRPC]
    public void FinalDeath()
    {
        Invoke(nameof(DelayWin), 1f);
        anim.SetTrigger("Death");
    }
    public void DelayWin()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MobileInputs>().enabled = false;
        FoeWin();
    }
}
