using UnityEngine;

public class TagInChildren : MonoBehaviour
{
	public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
	{
		// Native scans immediate children only, including inactive children.
		for (int index = 0; index < parent.transform.childCount; index++)
		{
			GameObject child = parent.transform.GetChild(index).gameObject;
			if (child.tag == tag) return child;
		}
		return null;
	}
}
