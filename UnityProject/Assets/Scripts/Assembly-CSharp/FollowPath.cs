using UnityEngine;

public class FollowPath : MonoBehaviour
{
	public PathManager pathManager;

	public int pathPoint;

	private void Start()
	{
	}

	private void Update()
	{
		TransformData point = pathManager.points[pathPoint];
		transform.position = point.position;
		transform.rotation = point.rotation;
	}
}
