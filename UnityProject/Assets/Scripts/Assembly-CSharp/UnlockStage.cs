using UnityEngine;
using UnityEngine.UI;

// Corrected ARM64 0x12BA544..0x12BA820. Price is300 crystals.
public class UnlockStage : MonoBehaviour
{
    public Text crystalsView;

    public void Unlock()
    {
        var player = SystemManager.i.tmPlayer;
        if (player.crystals < 300f)
        {
            Notifications.i.Log(TransladeController.i.translation.ChooseCharacter_YouNeedMoreCrystalsToUnlockStage, 7f, "info");
            return;
        }
        player.crystals -= 300f;
        var field = player.stages.GetType().GetField("stage" + StagesManager.i.actualStage);
        field.SetValue(player.stages, 1);
        StagesManager.i.SetPlayableStages();
        gameObject.SetActive(false);
        CrystalManager.i.UpdateCrystals();
        Notifications.i.Log(TransladeController.i.translation.ChooseCharacter_Text_NewStageUnlock, 5f, "success");
        SystemManager.i.SaveData("stages");
    }
    // Native Start hides this object only for connected non-master Photon clients.
    // Offline has no such clients; native Update is RET.
}
