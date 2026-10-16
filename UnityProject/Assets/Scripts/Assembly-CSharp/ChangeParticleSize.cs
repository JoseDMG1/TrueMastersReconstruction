using System.Collections;
using UnityEngine;

// Native Start 0x1277554 and coroutine MoveNext 0x127769C.
public class ChangeParticleSize : MonoBehaviour
{
    public float delay;
    private ParticleSystem.MainModule psMain;
    private float actualSize;
    private void Start()
    {
        psMain = GetComponent<ParticleSystem>().main;
        actualSize = psMain.startSize.constant;
        StartCoroutine(ChangeSize());
    }
    private IEnumerator ChangeSize()
    {
        yield return new WaitForSeconds(delay);
        while (actualSize > 0f)
        {
            actualSize -= .08f;
            psMain.startSize = actualSize;
            yield return new WaitForSeconds(.02f);
        }
    }
}
