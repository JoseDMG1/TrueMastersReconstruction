using System.Collections;
using UnityEngine;

// Native offline path VA 0x12A187C. The class name does not imply Photon is needed offline.
public class DestroyNetObjByTime : MonoBehaviour
{
    public float time;
    public GameObject _finalExplotion;
    private void Start()
    {
        if (GameController.i && GameController.i.online)
        {
            Debug.LogWarning("Network timed projectile deletion is unavailable.", this); return;
        }
        if (_finalExplotion) Invoke(nameof(FinalAnimation), time);
        Destroy(gameObject, time);
    }
    public void FinalAnimation()
    {
        if (_finalExplotion) Instantiate(_finalExplotion, transform.position, transform.rotation);
    }
    public IEnumerator DestroyNetObject(GameObject gameObj, float time)
    {
        if (GameController.i && GameController.i.online)
        {
            Debug.LogWarning("Network timed projectile deletion is unavailable.", this); yield break;
        }
        yield return new WaitForSeconds(time);
        if (gameObj) Destroy(gameObj);
    }
}
