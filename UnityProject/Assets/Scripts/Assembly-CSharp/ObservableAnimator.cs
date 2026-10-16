using UnityEngine;

public class ObservableAnimator : MonoBehaviour
{
	public Animator anim;

	public float newHorizontal;

	public float newVertical;

	public float sHorizontal;

	public float sVertical;

	public Vector3 lastPosition;

	public Vector3 newPosition;

	public GameObject target;

	public float dir;

	public float distance;

	public float newDistance;

	public float lastDistance;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void GetTarget()
	{
	}

	public void GetAnimation()
	{
	}

	private void Smooth()
	{
	}

	public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		return 0f;
	}
}
