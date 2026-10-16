using UnityEngine;

public class DisableColliderOnCollition : MonoBehaviour
{
	public BoxCollider col;

	public bool onlyByPlayers;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
		// Native 0x1228CF8: only opposite layer 8/9 contacts qualify.
		int incomingLayer = collider.gameObject.layer;
		if ((incomingLayer != 8 && incomingLayer != 9) || incomingLayer == gameObject.layer) return;
		if (onlyByPlayers)
		{
			// The shipped code reads this object's Description, not the incoming collider's.
			var description = GetComponent<Description>();
			if (!description || description.Type == "Char") return;
		}
		// LeftWaterAtk2 retains an empty serialized reference. Avoid the native null fault;
		// do not invent a replacement collider or change the recovered prefab.
		if (col) col.enabled = false;
	}
}
