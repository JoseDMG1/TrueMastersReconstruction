using System.Collections;
using UnityEngine;

public class GrassSpecialController : MonoBehaviour
{
    public GameObject wings;
    public float growDelay;
    public float shirnkDelay;

    private void Start() { }

    public void StartSpecialAtk()
    {
        // Native 0x1236E8C: the coroutine runs on this component, while
        // StatsController owns the health interpretation.
        StartCoroutine(ChangeWings());
        bool opponent = GameController.i.MainCharacter.layer != gameObject.layer;
        StartCoroutine(StatsController.i.GrassHealing(opponent));
        GetComponent<DamageOnCollision>().RestoreDamage();
    }

    private IEnumerator ChangeWings()
    {
        yield return new WaitForSeconds(growDelay);
        wings.GetComponent<ChangeSize>().ChangeSizeTo(Vector3.one);
        yield return new WaitForSeconds(shirnkDelay);
        wings.GetComponent<ChangeSize>().ChangeSizeTo(Vector3.zero);
    }
}
