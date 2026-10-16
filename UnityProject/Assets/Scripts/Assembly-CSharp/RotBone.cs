using UnityEngine;

public class RotBone : MonoBehaviour
{
	public Transform child;

	private Vector3 lastTipPos;

	private Vector3 dirPosToChild;

	private Vector3 dirPosToLastChild;

	private Quaternion rot;

	public bool debug;

	public float radius;

	private Quaternion originalLocalRot;

	public float speed;

	private Vector3 originalDir;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Move()
	{
	}

	public Quaternion GetRotationToChild()
	{
		return default;
	}

	private void OnDrawGizmos()
	{
	}
}
