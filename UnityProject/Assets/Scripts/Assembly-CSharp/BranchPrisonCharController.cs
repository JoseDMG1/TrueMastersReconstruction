using UnityEngine;

// Corrected ARM64 0x121A9DC..0x121AC7C. Branch prison still has its own owner.
public class BranchPrisonCharController : MonoBehaviour
{
    public branchPrisonController actualPrisonAtk;
    public GameObject crushAttack;
    public void CrushAttack()
    {
        var attack = Instantiate(crushAttack, actualPrisonAtk.transform.position, actualPrisonAtk.transform.rotation);
        attack.layer = gameObject.layer;
        attack.GetComponent<DestroyWhenCreated>().target = actualPrisonAtk.gameObject;
        GameEvents.i.AttackLayerSetUp(attack.GetEntityId());
    }
    public void StartBranchCrushAnim() { actualPrisonAtk.PlayCrushAnimation(); }
    public void PlayCharCrushAnim()
    {
        if (actualPrisonAtk.enemyTrapped) GetComponent<Animator>().Play("PrisonExplosion");
    }
}
