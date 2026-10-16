using System.Collections;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    public Vector3 newSize;
    public float duration;
    public bool destroyAtEnd;
    public float delay;

    private void OnEnable()
    {
        StartCoroutine(Change(newSize));
    }

    public void ChangeSizeTo(Vector3 size)
    {
        // Native 0x121DE98 does not cancel an earlier transition.
        StartCoroutine(Change(size));
    }

    public IEnumerator Change(Vector3 size)
    {
        // Native coroutine MoveNext 0x121DEEC: snapshot follows the delay.
        yield return new WaitForSeconds(delay);
        float timeElapsed = 0f;
        Vector3 originalScale = transform.localScale;
        while (timeElapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, size, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = size;
        if (destroyAtEnd) Destroy(gameObject);
    }
}
