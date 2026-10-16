using System.Collections;
using UnityEngine;

public class ChangeStartPosition : MonoBehaviour
{
    public float forward;
    public float right;
    public float up;
    public bool fixOnlinePosition;
    private void Awake()
    {
        // Native VA 0x121E118 applies this offset in Awake, including offline.
        transform.position += transform.right * right + transform.forward * forward + transform.up * up;
        if (GameController.i && GameController.i.online && fixOnlinePosition)
            StartCoroutine(ForceOnlineStartPosition(right, forward));
    }
    private IEnumerator ForceOnlineStartPosition(float right, float forward)
    {
        Debug.LogWarning("Online projectile position correction is unavailable.", this);
        yield break;
    }
}
