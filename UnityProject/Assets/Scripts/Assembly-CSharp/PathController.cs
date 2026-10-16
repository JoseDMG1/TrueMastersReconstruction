using System.Collections.Generic;
using UnityEngine;

// Corrected ARM64 0x1247354..0x1247B68. Fixed-capacity moving path history.
public class PathController : MonoBehaviour
{
    public List<Vector3> pathPositions;
    public List<GameObject> points;
    public int limitPositions;
    public bool ready;
    public float limitDistance;
    public GameObject point;
    public Vector3 startPoint;
    public float elpasedDistance;
    public float div;
    public Vector3 lastPosition;
    public bool showPoints;
    public BonePath bones;

    private void Start()
    {
        startPoint = transform.position + new Vector3(0f, -20f, 0f);
        lastPosition = startPoint;
        float distance = Vector3.Distance(startPoint, transform.position);
        elpasedDistance = 0f;
        div = distance / limitPositions;
        while (pathPositions.Count < limitPositions)
        {
            // Native initial positions are (0,distance-progress,0), not startPoint.
            pathPositions.Add(new Vector3(0f, distance - elpasedDistance, 0f));
            elpasedDistance += div;
            if (showPoints) points.Add(Instantiate(point, pathPositions[pathPositions.Count - 1], Quaternion.identity));
        }
        ready = true;
    }
    public void Stop() { enabled = false; }
    private void LateUpdate()
    {
        float distance = Vector3.Distance(lastPosition, transform.position);
        if (distance < limitDistance) return;
        Vector3 direction = (transform.position - lastPosition).normalized;
        elpasedDistance = 0f;
        div = (int)(distance / limitDistance);
        for (int index = 0; index < div; index++)
        {
            Vector3 next = lastPosition + direction * elpasedDistance;
            pathPositions.Add(next);
            pathPositions.RemoveAt(0);
            elpasedDistance += limitDistance;
            if (showPoints)
            {
                points.Add(Instantiate(point, next, Quaternion.identity));
                Destroy(points[0]);
                points.RemoveAt(0);
            }
        }
        lastPosition = transform.position;
        bones.UpdateBonesRot();
    }
}
