using UnityEngine;

// Corrected ARM64 0x1283580..0x1283BF0. Animation-event FX ownership.
public class CreateInBone : MonoBehaviour
{
    public Animator animator;
    public GameObject creationPrefab;
    public HumanBodyBones bonesSelected;

    public void CreateFxInHands()
    {
        CreateAtHand(creationPrefab, HumanBodyBones.LeftHand);
        CreateAtHand(creationPrefab, HumanBodyBones.RightHand);
    }
    public void CreateFxInLeftHand(GameObject fx) { CreateAtHand(fx, HumanBodyBones.LeftHand); }
    public void CreateFxInRightHand(GameObject fx) { CreateAtHand(fx, HumanBodyBones.RightHand); }
    private void CreateAtHand(GameObject fx, HumanBodyBones hand)
    {
        var instance = Instantiate(fx);
        instance.transform.parent = animator.GetBoneTransform(hand);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;
    }
}
