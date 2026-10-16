using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Corrected ARM64 0x121A4A8..0x121A990. Real iterators replace exported stubs.
public class BranchPrisonAnim : MonoBehaviour
{
    public SimpleMove move;
    public RotateSelf rotate;
    public GameObject branch1;
    public GameObject branch2;
    public float originalMoveSpeed;
    public float originalRotateSpeed;
    public float originalBranch1Pos;
    public float originalBranch2Pos;
    public float time;
    public float elapsedtime;
    public AnimationCurve curve;
    public AnimationCurve inverseCurve;
    public RefHitCharacter refCharacter;
    public UnityEvent onAttackExplote;

    private void Start()
    {
        originalMoveSpeed = move.speed;
        originalRotateSpeed = rotate.x;
        originalBranch1Pos = branch1.transform.localPosition.x;
        originalBranch2Pos = branch2.transform.localPosition.x;
        StartCoroutine(ReduceSpeed());
    }
    private IEnumerator ReduceSpeed()
    {
        while (elapsedtime < time)
        {
            elapsedtime += Time.deltaTime;
            float value = curve.Evaluate(elapsedtime / time);
            move.speed = originalMoveSpeed * value;
            rotate.x = originalRotateSpeed * value;
            Vector3 first = branch1.transform.localPosition;
            first.x = curve.Evaluate(elapsedtime / time) * originalBranch1Pos;
            branch1.transform.localPosition = first;
            Vector3 second = branch2.transform.localPosition;
            second.x = curve.Evaluate(elapsedtime / time) * originalBranch2Pos;
            branch2.transform.localPosition = second;
            yield return null;
        }
    }
    public IEnumerator RetrieveAttack()
    {
        elapsedtime = 0f;
        originalMoveSpeed = -originalMoveSpeed;
        originalRotateSpeed = -originalRotateSpeed;
        while (elapsedtime < time)
        {
            elapsedtime += Time.deltaTime;
            // Native reads curve at0x58 here too; inverseCurve remains unused.
            float value = curve.Evaluate(elapsedtime / time);
            move.speed = originalMoveSpeed * value;
            rotate.x = originalRotateSpeed * value;
            yield return null;
        }
    }
    public void PlayRetrieveAnim() { StartCoroutine(RetrieveAttack()); }
}
