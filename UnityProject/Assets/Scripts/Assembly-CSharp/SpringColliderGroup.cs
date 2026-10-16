using System.Collections.Generic;
using UnityEngine;

public class SpringColliderGroup : MonoBehaviour
{
	public List<SpringBoneGroup> boneGroups;

	public SpringCollider[] colliders;

	private void Awake()
	{
	}
}
