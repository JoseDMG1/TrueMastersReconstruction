using UnityEngine;

public class MovePath : MonoBehaviour
{
	public float y;

	public float x;

	public float z;

	public float xSpeed;

	public float ySpeed;

	public float zSpeed;

	public AnimationCurve curve;

	public float elapseTime;

	public Vector3 initialPos;

	public float JumpTime;

	public bool leftJump;

	public float disableJumpTime;

	public bool disableJump;

	public int numberOfJumps;

	public int maxJumps = 3;

	public float dropSpeed = -0.1f;

	private void Start()
	{
		initialPos = transform.localPosition;
		if (disableJumpTime != 0f) Invoke(nameof(Disable), disableJumpTime);
	}

	private void Update()
	{
		// Native 0x1247204 resets on the frame after crossing JumpTime.
		if (elapseTime < JumpTime) elapseTime += Time.deltaTime;
		else
		{
			elapseTime = 0f;
			leftJump = !leftJump;
			numberOfJumps++;
		}
		if (!disableJump) y = curve.Evaluate(elapseTime / JumpTime) * ySpeed;
		if (numberOfJumps >= maxJumps)
		{
			disableJump = true;
			y += dropSpeed; // Native per-frame drop, deliberately not multiplied by deltaTime.
		}
		z += zSpeed * Time.deltaTime;
		x += (leftJump ? xSpeed : -xSpeed) * Time.deltaTime;
		transform.localPosition = initialPos + new Vector3(x, y, z);
	}

	public void Disable()
	{
		disableJump = true;
	}
}
