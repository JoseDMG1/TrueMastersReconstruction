using System.Collections.Generic;
using UnityEngine;

public class SColliderGroup : MonoBehaviour
{
	public List<SBoneGroup> boneGroups;

	public SpringCollider[] colliders;

	private void Awake()
	{
	}
}
