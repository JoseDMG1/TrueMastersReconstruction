using UnityEngine;

// Corrected ARM64 0x1223700..0x1223848. Serialized timing is caller-owned.
public class CreateByTime : MonoBehaviour
{
    public GameObject creation;
    public float delay;
    public float time;

    private void Start()
    {
        InvokeRepeating(nameof(Sumon), delay, time);
    }
    private void Sumon()
    {
        Instantiate(creation, transform.position, transform.rotation);
    }
    // Native Update is RET; no per-frame replacement timer is introduced.
}
