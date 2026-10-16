using System.Collections;
using UnityEngine;

// Native 0x1228858..0x1228CA8, corrected ARM64 mapping.
public class DisableColliderAnim : MonoBehaviour
{
    private Animator anim;
    public float delay;
    private void Start() { StartCoroutine(DefineAnim(delay)); }
    private IEnumerator DefineAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (gameObject.layer == 1) yield return new WaitForSeconds(.02f);
        var player = gameObject.layer == 8 ? HostPlayer.hostPlayer.gameObject : GuestPlayer.guestPlayer.gameObject;
        anim = player.GetComponent<Animator>();
        InvokeRepeating(nameof(CheckAttack), 0f, .05f);
    }
    private void CheckAttack()
    {
        if (anim.gameObject.GetComponent<Description>().element != "Light") return;
        var state = anim.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName("UpAttack2")) return;
        float time = state.normalizedTime;
        GetComponent<BoxCollider>().enabled = !(time > .2f && time < .45f);
    }
}
