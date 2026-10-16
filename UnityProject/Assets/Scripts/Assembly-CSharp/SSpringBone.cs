using UnityEngine;

public class SSpringBone : MonoBehaviour
{
	public float stiff;

	public float drag;

	private Vector3 boneAxis;

	private float radius;

	public SpringCollider[] colliders;

	private bool debug;

	private Quaternion localRotation;

	private Vector3 currTipPos;

	private Vector3 prevTipPos;

	public Vector3 aimVector;

	public float springLength;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void Update()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
