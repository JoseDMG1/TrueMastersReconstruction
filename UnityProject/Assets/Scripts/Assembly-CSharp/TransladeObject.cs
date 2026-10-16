using System.Collections;
using UnityEngine;

// Corrected ARM64 0x12CE6F8..0x12CEAA4. Persistent motion service.
public class TransladeObject : MonoBehaviour
{
    public static TransladeObject i;

    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) { Destroy(this); return; }
    }
    private void OnDestroy() { if (i == this) i = null; }

    public IEnumerator MoveToPosition(Transform startpos, Vector3 endpos, float duration)
    {
        float t = 0f;
        do
        {
            t += Time.deltaTime / duration;
            // Native interpolates from the current position each iteration and
            // SmoothStep goes from 0 to .5, rather than a conventional fixed start.
            startpos.position = Vector3.Lerp(startpos.position, endpos, Mathf.SmoothStep(0f, .5f, t));
            yield return new WaitForSeconds(.02f);
        } while (t <= 1f);
    }
}
