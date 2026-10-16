using UnityEngine;

public class CreateObjAnim : MonoBehaviour
{
	public void CreateObj(GameObject obj)
	{
		// Native 0x12240F0: animation event spawn at this owner's world pose.
		if (obj) Instantiate(obj, transform.position, transform.rotation);
	}
}
