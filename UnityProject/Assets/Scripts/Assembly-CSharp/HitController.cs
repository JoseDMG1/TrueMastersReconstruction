using UnityEngine;

public class HitController : MonoBehaviour
{
	private void Start()
	{
	}

	public NetAnimation GetHitAnimationName(GameObject atk, GameObject player)
	{
		return NetAnimation.Movement;
	}

	private bool IsShieldBroken(int atkDamage, float impact, float angle, bool unblockeable)
	{
		return false;
	}

	public float GetAngleDiference(GameObject obj1, GameObject obj2)
	{
		return 0f;
	}
}
