using System.Collections.Generic;
using UnityEngine;

public class MoveLocalBetweenPoints : MonoBehaviour
{
	public List<Vector3> localPositions;

	public float moveSpeed;

	private int currentIndex;

	private Vector3 currentTargetLocal;

	public float intervalTime;

	private float timer;

	private void Update()
	{
	}

	private void SetNextTarget()
	{
	}
}
