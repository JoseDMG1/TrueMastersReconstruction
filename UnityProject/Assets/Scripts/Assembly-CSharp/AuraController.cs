using Photon.Pun;
using UnityEngine;

// Reconstructed from Evidence/native-disassembly-AuraController.txt and native metadata.
public class AuraController : MonoBehaviour
{
    public ParticleSystem[] auras;
    public GameObject chargeFX;

    // Original Start VA0x1215FB8 is a single RET; removed as an empty Unity callback.

    // Native VA0x1215FBC. Only the instantiated object's root layer is changed.
    public void ShowChargeAwake()
    {
        GameObject effect = Instantiate(chargeFX, transform.position, transform.rotation);
        effect.layer = gameObject.layer;
    }

    // Native VA0x12160F0. Offline path performs only the local initialization.
    public void StartAura()
    {
        StartAuraInit();
        if (GameController.i.online && GetComponent<PhotonView>().IsMine)
            GetComponent<PhotonView>().RPC("StartAuraInit", RpcTarget.Others);
    }

    // Native VA0x1216234.
    [PunRPC]
    public void StartAuraInit()
    {
        GetComponent<AnimatorStatesManager>().awakening = true;
        for (int index = 0; index < auras.Length; index++)
            auras[index].Play();
    }

    // Native VA0x12162D8. Original network dispatch is guarded by online and IsMine.
    public void StopAura()
    {
        StopAuraInit();
        if (GameController.i.online && GetComponent<PhotonView>().IsMine)
            GetComponent<PhotonView>().RPC("StopAuraInit", RpcTarget.Others);
    }

    // Native VA0x121641C. Uses ParticleSystem.Stop(), without added Clear or destroy.
    [PunRPC]
    public void StopAuraInit()
    {
        GetComponent<AnimatorStatesManager>().awakening = false;
        for (int index = 0; index < auras.Length; index++)
            auras[index].Stop();
    }
}
