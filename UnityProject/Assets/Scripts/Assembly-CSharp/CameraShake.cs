using System.Collections;
using UnityEngine;

// Native VA 0x121C084, Shake 0x121C1B4, coroutine MoveNext 0x121C300.
public class CameraShake : MonoBehaviour
{
    public static CameraShake i;
    private Vector3 originalPos;
    private bool Shaking;
    private void Awake()
    {
        if (!i) i = this;
        else if (i != this) Destroy(this);
    }
    public void Shake(float duration = .5f)
    {
        if (Shaking) { transform.localPosition = originalPos; Shaking = false; return; }
        originalPos = transform.localPosition;
        StopAllCoroutines();
        StartCoroutine(cShake(duration));
    }
    public IEnumerator cShake(float duration)
    {
        float easyOut = .02f;
        Shaking = true;
        while (duration > 0f)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * duration * .5f;
            duration -= .04f;
            easyOut += .004f;
            yield return new WaitForSeconds(easyOut);
        }
        transform.localPosition = originalPos;
        Shaking = false;
    }
}
