using System.Collections;
using UnityEngine;

// Native VA 0x1297378, coroutine bodies 0x12977A4/0x12975E0.
public class IncreaseSize : MonoBehaviour
{
    private Vector3 originalSize;
    public float speed;
    public float decreaseAfterTime;
    public bool disableAutoPlay;

    private void OnEnable()
    {
        originalSize = transform.localScale;
        transform.localScale = originalSize * .01f;
        if (disableAutoPlay) return;
        Increase();
        if (decreaseAfterTime != 0f) Decrease(decreaseAfterTime);
    }
    public void Increase(float delay = 0f)
    {
        // The native method does not read delay.
        StopAllCoroutines();
        StartCoroutine(IncreaseSmooth());
    }
    public void Decrease(float delay = 0f) { Invoke(nameof(DelayDecrease), delay); }
    public IEnumerator IncreaseSmooth()
    {
        while (transform.localScale.y < originalSize.y * .99f)
        {
            var current = transform.localScale;
            // Native z interpolation starts at .99 rather than current.z (0x1297860).
            // Preserve the observed arithmetic; this is not a generic scale tween.
            current.z = .99f;
            transform.localScale = Vector3.Lerp(current, originalSize, speed);
            yield return new WaitForSeconds(.02f);
        }
    }
    public void DelayDecrease()
    {
        StopAllCoroutines();
        StartCoroutine(DecreaseSmooth());
    }
    public IEnumerator DecreaseSmooth()
    {
        while (transform.localScale.y > .01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, speed * 1.5f);
            yield return new WaitForSeconds(.02f);
        }
        transform.localScale = originalSize;
        gameObject.SetActive(false);
    }
}
