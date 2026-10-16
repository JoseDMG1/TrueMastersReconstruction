using UnityEngine;
using UnityEngine.Events;

// Corrected ARM64 0x12795B4..0x12796DC.
public class RefHitCharacter : MonoBehaviour
{
    public GameObject hitCharacter;
    public UnityEvent<int> onHitCharacter;

    private void OnTriggerEnter(Collider other)
    {
        var description = other.GetComponent<Description>();
        if (description == null || description.Type != "Char") return;
        hitCharacter = other.gameObject;
        onHitCharacter.Invoke(hitCharacter.layer);
    }
}
