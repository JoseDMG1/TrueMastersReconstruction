using UnityEngine;

public class SpringBone : MonoBehaviour
{
	public Transform child;

	public Vector3 boneAxis;

	public float radius;

	public bool isUseEachBoneForceSettings;

	public float stiffnessForce;

	public float dragForce;

	public Vector3 springForce;

	public SpringCollider[] colliders;

	public bool debug;

	public float threshold;

	private float springLength;

	private Quaternion localRotation;

	private Transform trs;

	private Vector3 currTipPos;

	private Vector3 prevTipPos;

	private Transform org;

	private SpringManager managerRef;

	public float spd;

	private void Awake()
	{
	}

	private SpringManager GetParentSpringManager(Transform t)
	{
		return null;
	}

	private void Start()
	{
	}

	public void UpdateSpring()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
