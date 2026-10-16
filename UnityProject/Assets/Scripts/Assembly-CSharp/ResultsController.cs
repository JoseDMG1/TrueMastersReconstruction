using UnityEngine;
using UnityEngine.UI;

// Rewards reconstructed from ResultsController ARM64; ranks precede difficulty multipliers.
public class ResultsController : MonoBehaviour
{
    public static ResultsController resultsController;
    public Text rank;
    public Text crystalsView;
    public GameObject bonus;
    public Text crystalViewBattle;
    private float crystalsReward;
    public GameObject playAgainButton;
    private bool initialized;

    private void Awake() { resultsController = this; }
    private void OnDestroy() { if (resultsController == this) resultsController = null; }

    public void InitResults()
    {
        // Reconstruction guard: one reward transaction per result screen/scene.
        if (initialized) return;
        initialized = true;
        Debug.Log("RESULTS");
        GetComponent<Animator>().SetBool("In", true);
        CalculateCrystalReward();
        CrystalsNotification.i.WinCrystals(crystalsReward);
        DisplayBonus();
    }

    private void DisplayBonus()
    {
        // Original requires a loaded rewarded advertisement and reward >=3.
        // The offline reconstruction has no ad provider and cannot offer that bonus.
        bonus.SetActive(false);
        playAgainButton.SetActive(true);
    }

    private void CalculateCrystalReward()
    {
        var score = Score.score;
        var mode = SystemManager.i.mode;
        crystalsReward = mode == GameMode.Survival
            ? score.playerPoints * 2f : score.playerPoints - score.enemyPoints + 3f;
        if (GameController.i.practiceMode) crystalsReward = 0f;
        if (mode == GameMode.Tournament)
        {
            TournamentController.i.loseTournament = crystalsReward <= 2f;
            if (crystalsReward == 3f)
            {
                int comparison = StatsController.i.GetPlayerLifeCrystals(false).CompareTo(StatsController.i.GetPlayerLifeCrystals(true));
                if (comparison > 0) { TournamentController.i.loseTournament = false; return; }
                if (comparison < 0) { TournamentController.i.loseTournament = true; return; }
                GameController.i.draw = true;
            }
        }
        CalculateRank();
        if (mode == GameMode.SinglePlayer)
        {
            switch (PlayerPrefs.GetInt("CpuMode"))
            {
                case 1: crystalsReward = Mathf.Ceil(crystalsReward * 1.5f); break;
                case 2: crystalsReward = Mathf.Ceil(crystalsReward * 2f); break;
                case 3: crystalsReward = Mathf.Ceil(crystalsReward * 2.5f); break;
                case 4: crystalsReward = Mathf.Ceil(crystalsReward * 3f); break;
            }
        }
        crystalsView.text = crystalsReward.ToString();
    }

    private void CalculateRank()
    {
        if (SystemManager.i.mode == GameMode.Survival)
        {
            if (crystalsReward > 20f)
            {
                Notify("Congrats your score is over 10!   You got 20 extra Crystals", "success");
                CrystalsNotification.i.WinCrystals(20f);
            }
            rank.text = Score.score.playerPoints.ToString();
            return;
        }
        switch (crystalsReward)
        {
            case 0f: rank.text = "F"; break;
            case 1f: rank.text = "E"; break;
            case 2f: rank.text = "D"; break;
            case 3f: rank.text = "C"; break;
            case 4f: rank.text = "B"; break;
            case 5f: rank.text = "A"; break;
            case 6f: rank.text = "S"; break;
        }
        if (crystalsReward == 0f && GameController.i.practiceMode)
        {
            rank.text = "-";
            Notify("Crystals disabled - Cpu was disabled during the battle", "info");
        }
    }

    private void Notify(string message, string type)
    {
        if (Notifications.i != null) Notifications.i.Log(message, 10f, type);
        else Debug.Log(message);
    }

    public void ShowRewardedAd()
    {
        Debug.LogWarning("Rewarded advertisements are unavailable in the offline reconstruction; no bonus awarded.", this);
        DisplayBonus();
    }
}
