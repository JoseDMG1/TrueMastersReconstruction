using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	public List<TransformData> points;

	public int limitPositions;

	public Vector3 lastPosition;

	public float distanceBetweenPoints = 0.2f;

	private void Start()
	{
		lastPosition = transform.position;
		// Native 0x1247B88 seeds a vertical history using LookRotation(up, up).
		Quaternion rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
		float offset = 0f;
		while (points.Count < limitPositions)
		{
			points.Add(new TransformData(transform.position + new Vector3(0f, offset, 0f), rotation));
			offset -= distanceBetweenPoints;
		}
	}

	private void MoveLastToFirst(Vector3 pos, Quaternion rot)
	{
		points.RemoveAt(points.Count - 1);
		points.Insert(0, new TransformData(pos, rot));
	}

	private void LateUpdate()
	{
		// A malformed nonpositive spacing would make the native loop unbounded.
		if (!(distanceBetweenPoints > 0f) || float.IsInfinity(distanceBetweenPoints))
		{
			Debug.LogError("PathManager requires finite positive distanceBetweenPoints.", this);
			enabled = false;
			return;
		}
		Vector3 delta = transform.position - lastPosition;
		float distance = delta.magnitude;
		if (distance <= distanceBetweenPoints) return;
		Vector3 direction = delta.normalized;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		for (float offset = distanceBetweenPoints; offset < distance; offset += distanceBetweenPoints)
			MoveLastToFirst(lastPosition + direction * offset, rotation);
		lastPosition = transform.position;
	}
}
