using UnityEngine;

public class BranchPart : MonoBehaviour
{
	[SerializeField]
	private float size;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float jumpDistance;

	[SerializeField]
	private PathController path;

	[SerializeField]
	private BonePath bones;

	[SerializeField]
	private MovePath move;

	[SerializeField]
	private GameObject branch;

	private void Awake()
	{
	}

	private void Update()
	{
	}
}
