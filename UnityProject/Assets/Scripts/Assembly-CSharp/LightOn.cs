using System.Collections;
using UnityEngine;

// Restored from retained ARM64 Start/On/Off and their coroutine bodies.
// Native constants: +0.15/-0.15 intensity per 0.02 scaled-second wait.
public class LightOn : MonoBehaviour
{
    private float maxLight;
    private Light effectLight;
    public float duration = .5f;
    public bool off;
    public float delayOff = 2f;

    private void Start()
    {
        effectLight = GetComponent<Light>();
        if (!effectLight) return;
        maxLight = effectLight.intensity;
        effectLight.intensity = 0f;
        StartCoroutine(On());
        if (off) StartCoroutine(Off());
    }

    private IEnumerator On()
    {
        if (duration <= 0f) { effectLight.intensity = maxLight; yield break; }
        for (float t = 0f; t <= 1f;)
        {
            t += Time.deltaTime / duration;
            if (effectLight.intensity <= maxLight)
            {
                effectLight.intensity += .15f;
                yield return new WaitForSeconds(.02f);
            }
        }
        effectLight.intensity = maxLight;
    }

    private IEnumerator Off()
    {
        yield return new WaitForSeconds(delayOff);
        if (duration <= 0f) { effectLight.intensity = 0f; yield break; }
        for (float t = 0f; t <= 1f;)
        {
            t += Time.deltaTime / duration;
            if (effectLight.intensity >= 0f)
            {
                effectLight.intensity -= .15f;
                yield return new WaitForSeconds(.02f);
            }
        }
        effectLight.intensity = 0f;
    }
}
