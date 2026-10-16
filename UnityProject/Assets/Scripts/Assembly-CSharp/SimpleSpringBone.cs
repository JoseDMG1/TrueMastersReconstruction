using UnityEngine;

public class SimpleSpringBone : MonoBehaviour
{
	private Transform child;

	public float gravity;

	private float stiff;

	private Vector3 boneAxis;

	private float radius;

	private float stiffnessForce;

	private float dragForce;

	public SpringCollider[] colliders;

	private bool debug;

	private float springLength;

	private Quaternion localRotation;

	private Vector3 currTipPos;

	private Vector3 prevTipPos;

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
