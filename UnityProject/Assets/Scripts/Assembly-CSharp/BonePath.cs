using System.Collections.Generic;
using UnityEngine;

// Corrected ARM64 0x124697C..0x1246E88.
public class BonePath : MonoBehaviour
{
    public List<Transform> children;
    public PathController path;

    private void Start()
    {
        children.Add(transform);
        // Includes this transform again, as the native method does.
        children.AddRange(transform.GetComponentsInChildren<Transform>());
    }
    public void UpdateBonesRot()
    {
        transform.position = path.pathPositions[0];
        for (int index = 1; index < children.Count; index++)
        {
            float distance = Vector3.Distance(children[1].position, children[2].position) * index;
            int position = GetPathPos(distance);
            // Reconstruction correction at the producing index boundary: native
            // checks >Count, allowing ==Count to reach an out-of-range list read.
            position = Mathf.Clamp(position, 0, path.pathPositions.Count - 1);
            Vector3 direction = path.pathPositions[position] - children[index].position;
            children[index].rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
    private int GetPathPos(float dis)
    {
        // Native preserves float arithmetic before banker's rounding.
        return Mathf.RoundToInt((dis * 100f) / (path.limitDistance * 100f));
    }
}
