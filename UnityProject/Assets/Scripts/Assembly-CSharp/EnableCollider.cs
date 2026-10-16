using UnityEngine;
// Corrected ARM64 0x128AF64..0x128B044.
public class EnableCollider : MonoBehaviour
{
    public float time;
    private void Start() { Invoke(nameof(enableCollider), time); }
    private void enableCollider()
    {
        if (GetComponent<ConfigAttackLayer>().playerWeak) return;
        GetComponent<Collider>().enabled = true;
    }
}
