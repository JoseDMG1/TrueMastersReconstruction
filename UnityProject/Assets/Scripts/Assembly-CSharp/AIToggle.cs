using UnityEngine;
using UnityEngine.UI;

// Native AIToggle: local AI lifetime follows the countdown UnityEvent.
public class AIToggle : MonoBehaviour
{
    private Counter subscribedCounter;

    private void Start()
    {
        if (SystemManager.i.mode == GameMode.LocalMultiplayer || GameController.i.online) return;
        subscribedCounter = BattleSceneSingleton.i.counter;
        subscribedCounter.onCounterFinish.AddListener(StartAI);
        BattleSceneSingleton.i.Opponent.GetComponent<AI>().enabled = true;
    }

    private void OnDestroy()
    {
        if (subscribedCounter != null) subscribedCounter.onCounterFinish.RemoveListener(StartAI);
    }

    public void ChangeVisibility(bool visible)
    {
        transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public void ToogleAI(bool active)
    {
        BattleSceneSingleton.i.Opponent.GetComponent<AI>().ManageAI(active);
    }

    public void StartAI()
    {
        ToogleAI(GetComponent<Toggle>().isOn);
    }
}
