using System;
using System.Collections;
using UnityEngine;

// Native tween owner restored; easing comes from the recovered serialized curves.
public class LerpManager : MonoBehaviour
{
    public static LerpManager i;
    public AnimationCurve[] animCurves;
    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) Destroy(this);
    }
    public void LerpPosition(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing = LerpEasing.EasyInOut, float delay = 0f)
    {
        StartCoroutine(LerpPositionCoroutine(obj, start, target, duration, easing, delay));
    }
    private IEnumerator LerpPositionCoroutine(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing, float delay)
    {
        return Tween(obj, start, target, duration, easing, delay, (tr, value) => tr.position = value);
    }
    public void LerpLocalPosition(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing = LerpEasing.EasyInOut, float delay = 0f)
    {
        StartCoroutine(LerpLocalPositionCoroutine(obj, start, target, duration, easing, delay));
    }
    private IEnumerator LerpLocalPositionCoroutine(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing, float delay)
    {
        return Tween(obj, start, target, duration, easing, delay, (tr, value) => tr.localPosition = value);
    }
    public void LerpScale(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing = LerpEasing.EasyInOut, float delay = 0f)
    {
        StartCoroutine(LerpScaleCoroutine(obj, start, target, duration, easing, delay));
    }
    private IEnumerator LerpScaleCoroutine(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing, float delay)
    {
        return Tween(obj, start, target, duration, easing, delay, (tr, value) => tr.localScale = value);
    }
    private IEnumerator Tween(GameObject obj, Vector3 start, Vector3 target, float duration, LerpEasing easing, float delay, Action<Transform, Vector3> setValue)
    {
        yield return new WaitForSeconds(delay);
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            if (obj == null) yield break;
            setValue(obj.transform, Vector3.Lerp(start, target, AddEasing(timeElapsed, duration, easing)));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (obj != null) setValue(obj.transform, target);
    }
    public float AddEasing(float timeElapsed, float duration, LerpEasing easing = LerpEasing.EasyInOut)
    {
        return animCurves[(int)easing].Evaluate(timeElapsed / duration);
    }
}
