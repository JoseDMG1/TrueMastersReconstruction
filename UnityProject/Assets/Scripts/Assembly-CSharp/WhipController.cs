using Photon.Pun;
using UnityEngine;

// Corrected ARM64 0x12DD348..0x12DD7C8. Offline leaf-whip presentation.
public class WhipController : MonoBehaviour
{
    public Animator anim;
    public GameObject handFX;
    public AudioSource whipSound;
    public float leftWhipSoundDelay;
    public float rightWhipSoundDelay;
    private DamageOnCollision damageOwner;

    private void Start()
    {
        damageOwner = GetComponent<DamageOnCollision>();
        damageOwner.onDamageRecived.AddListener(IddleWhip);
    }
    public void PlayWhipSound() { whipSound.Play(); }
    public void RightWhipAtk()
    {
        anim.Play("RightAtk");
        Invoke(nameof(PlayWhipSound), rightWhipSoundDelay);
    }
    public void LeftWhipAtk()
    {
        anim.Play("LeftAtk");
        Invoke(nameof(PlayWhipSound), leftWhipSoundDelay);
    }
    [PunRPC] public void IddleWhip() { anim.Play("Iddle"); }
    public void NetIddleWhip()
    {
        Debug.LogWarning("Online whip synchronization is not reconstructed.", this);
    }
    public void ShowHandLeafFX()
    {
        foreach (var particle in handFX.GetComponentsInChildren<ParticleSystem>()) particle.Play();
    }
    private void OnDestroy()
    {
        if (damageOwner != null) damageOwner.onDamageRecived.RemoveListener(IddleWhip);
    }
}
