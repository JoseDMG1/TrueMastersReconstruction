using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Reconstructed from native ARM64; original component fields and animation-event names retained.
public class Counter : MonoBehaviour
{
    public float value = 3f;
    private TMP_Text text;
    public UnityEvent onCounterFinish = new UnityEvent();

    private void Awake()
    {
        text = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    public void subtractValue()
    {
        if (text.text == "GO!") Destroy(gameObject);
        text.text = value.ToString();
        if (value == 0f)
        {
            text.text = "GO!";
            BattleSceneSingleton.i.inputReader.blockInputs = false;
            GameController.i.EnableInputs();
            onCounterFinish?.Invoke();
        }
        value -= 1f;
        ChangeCamera();
    }

    public void ChangeCamera()
    {
        if (SystemManager.i && SystemManager.i.mode == GameMode.LocalOffline) return;
        var battle = BattleSceneSingleton.i;
        if (value == 0f)
        {
            battle.cameraManager.ChangeCameraParent(battle.MainCharacter);
            if (PlayerPrefs.GetInt("Cam", 0) != 0) battle.cameraManager.ChangeCameraToFar();
            else battle.cameraManager.ChangeCameraToClose();
        }
        else if (value == 1f) battle.cameraManager.ChangeCameraToWinner(battle.Opponent);
        else if (value == 2f) battle.cameraManager.ChangeCameraToWinner(battle.MainCharacter);
    }
}
