using System.Collections;
using TMPro;
using UnityEngine;

// Native UI references and public button method names retained; online providers deferred.
public class BattleUIManager : MonoBehaviour
{
public GameObject ratePopupPrefab;

	private GameObject ratePopupGameObj;

	public ExitMenu exitMenuScript;

	private RateGame ratePopupScript;

	public TMP_Text spectators;

	public int spectatorsNumber;

	public TMP_Text roomName;

	public int spectatorView = 1;

	public GameObject scoreP1;

	public GameObject scoreP2;

	public GameObject nameP1;

	public GameObject nameP2;

	public GameObject AtkP1;

	public GameObject AtkP2;

	public GameObject lifeP1;

	public GameObject lifeP2;

	public GameObject p1Life1;

	public GameObject p1Life3;

	public GameObject p2Life1;

	public GameObject p2Life3;

	public GameObject scoreP1Pos;

	public GameObject scoreP2Pos;

	public GameObject nameP1Pos;

	public GameObject nameP2Pos;

	public GameObject AtkP1Pos;

	public GameObject AtkP2Pos;

	public GameObject lifeP1Pos;

	public GameObject lifeP2Pos;

	public GameObject SpectatorNamePrefab;

	public GameObject SpectatorScroll;

	public GameObject SpectatorPanel;

	private bool spectatorListOpen;

	    private void Start()
    {
        if (GameController.i != null && GameController.i.online) OnlineUnavailable(nameof(Start));
    }
    private void Update() { }
    public void OpenExitMenu()
    {
        if (exitMenuScript.open) exitMenuScript.CloseMenu();
        else exitMenuScript.OpenMenu();
    }
    public void OpenRatePopup()
    {
        if (ratePopupScript != null) { ratePopupScript.OpenRatePopup(); return; }
        ratePopupGameObj = Instantiate(ratePopupPrefab);
        ratePopupScript = ratePopupGameObj.GetComponent<RateGame>();
        ratePopupGameObj.transform.parent = transform.parent;
        var rect = ratePopupGameObj.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0f, 100f, 0f);
        rect.localScale = Vector3.zero;
    }
    public void SendFriendRequest() { OnlineUnavailable(nameof(SendFriendRequest)); }
    public void ShareCode() { StartCoroutine(ShareCodeToFriend()); }
    private IEnumerator ShareCodeToFriend()
    {
        OnlineUnavailable(nameof(ShareCodeToFriend));
        yield break;
    }
    public void RefreshSpectators() { OnlineUnavailable(nameof(RefreshSpectators)); }
    public void ChangeSpectatorView()
    {
        spectatorView = spectatorView + 1 >= 4 ? 1 : spectatorView + 1;
        ChangeSpectatorCamera();
        ChangeSpectatorElements();
    }
    public void ChangeSpectatorCamera()
    {
        if (spectatorView == 3) { CameraManager.cameraM.ChangeToSceneCamera(); return; }
        if (spectatorView != 1 && spectatorView != 2) return;
        CameraManager.cameraM.ChangeCameraParent(spectatorView == 1 ? GameController.i.MainCharacter : GameController.i.OpponentCharacter);
        if (PlayerPrefs.GetInt("Cam", 0) != 0) CameraManager.cameraM.ChangeCameraToFar();
        else CameraManager.cameraM.ChangeCameraToClose();
    }
    public void ChangeSpectatorElements()
    {
        bool swap = spectatorView == 2;
        scoreP1.transform.SetParent((swap ? scoreP2Pos : scoreP1Pos).transform);
        scoreP2.transform.SetParent((swap ? scoreP1Pos : scoreP2Pos).transform);
        nameP1.transform.SetParent((swap ? nameP2Pos : nameP1Pos).transform);
        nameP2.transform.SetParent((swap ? nameP1Pos : nameP2Pos).transform);
        AtkP1.transform.SetParent((swap ? AtkP2Pos : AtkP1Pos).transform);
        AtkP2.transform.SetParent((swap ? AtkP1Pos : AtkP2Pos).transform);
        lifeP1.transform.SetParent((swap ? lifeP2Pos : lifeP1Pos).transform);
        lifeP2.transform.SetParent((swap ? lifeP1Pos : lifeP2Pos).transform);
        ResetPosition();
    }
    private void ResetPosition()
    {
        foreach (var element in new[] { scoreP1, scoreP2, nameP1, nameP2, AtkP1, AtkP2, lifeP1, lifeP2 })
            element.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    public void ToggleSpectatorScroll()
    {
        LerpManager.i.LerpScale(SpectatorScroll, spectatorListOpen ? Vector3.one : Vector3.zero,
            spectatorListOpen ? Vector3.zero : Vector3.one, 0.25f);
        spectatorListOpen = !spectatorListOpen;
    }
    private void OnlineUnavailable(string action)
    {
        Debug.LogWarning("BattleUIManager." + action + " requires the unreconstructed online provider.", this);
    }
}