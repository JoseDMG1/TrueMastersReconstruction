using UnityEngine;

// Corrected ARM64 0x12C344C..0x12C36E4. Final trophy transaction owner.
public class StarsFX : MonoBehaviour
{
    public float DelayMove = 3f;
    public Transform endPosition;
    public float duration = 5f;
    public GameObject menu;

    private void Start()
    {
        // Native uses constant 3, despite the serialized DelayMove field.
        Invoke(nameof(StartMoving), 3f);
    }

    public void StartMoving()
    {
        GetComponent<IncreaseSize>().Decrease();
        StartCoroutine(TransladeObject.i.MoveToPosition(gameObject.transform, endPosition.position, duration));
        Invoke(nameof(WinTrophy), 2f);
    }

    public void WinTrophy()
    {
        SystemManager.i.tmPlayer.trophies += TournamentController.i.tourneyLevel;
        TrophiesManager.i.UpdateTrophies();
        SystemManager.i.SaveData("trophies");
        PlayerPrefs.SetInt("TourneyTimes", 0);
        Menu();
    }

    public void Menu()
    {
        menu.SetActive(true);
    }
}
