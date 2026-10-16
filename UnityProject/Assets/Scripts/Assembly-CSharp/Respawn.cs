using System.Collections.Generic;
using UnityEngine;

// Mapped native 0x12A76B8/0x12A7828; see Knowledge/respawn-reconstruction-checkpoint.md.
public class Respawn : MonoBehaviour
{
    public void respawn()
    {
        if (CompareTag("HostPlayer"))
        {
            transform.position = new Vector3(7f, 5f, 0f);
            return;
        }
        if (SystemManager.i.mode != GameMode.Survival)
        {
            transform.position = new Vector3(-7f, 5f, 0f);
            return;
        }
        if (SurvivalController.survivalController.disableRespawn) return;
        Destroy(gameObject);
        CreateNewChar();
        SurvivalController.survivalController.RespawnOff();
    }

    private void CreateNewChar()
    {
        var survival = SurvivalController.survivalController;
        var characters = CharactersManager.i;
        var candidates = new List<int>();
        // Native enumerates the entire characters list, including its random selection entry.
        for (int index = 0; index < characters.characters.Count; index++) candidates.Add(index);
        candidates.Remove(survival.currentChar);
        int selected = candidates[Random.Range(0, candidates.Count)];
        survival.currentChar = selected;
        string character = characters.RemoveRandom(characters.characters[selected]);
        var prefab = Resources.Load<GameObject>("PhotonPrefabs/" + character);
        var replacement = Instantiate(prefab, new Vector3(-5f, 2f, -5f), transform.rotation);
        replacement.AddComponent<GuestPlayer>();
        var ai = replacement.GetComponent<AI>();
        ai.enabled = true;
        ai.on = true;
        // Publish the replacement at the resource-owning boundary before consumers resolve targets.
        // Native relied on Start of GuestPlayer; same-frame animation events can precede that callback.
        GameController.i.OpponentCharacter = replacement;
        GameController.i.guestPlayer = replacement;
        BattleSceneSingleton.i.Opponent = replacement;
        GuestPlayer.guestPlayer = replacement.GetComponent<GuestPlayer>();
        replacement.tag = "GuestPlayer";
        replacement.layer = 8;
        var host = HostPlayer.hostPlayer.gameObject;
        replacement.GetComponent<LookAt>().target = host.transform;
        host.GetComponent<LookAt>().target = replacement.transform;
        BattleSceneSingleton.i.MainCharacter.GetComponent<LookAt>().target = replacement.transform;
        replacement.GetComponent<Animator>().Play("FallingToLanding");
    }

    // Native Update only prints host/LookAt names every frame; omit that diagnostic spam.
}
