using UnityEngine;

public class RemoveGravity : MonoBehaviour
{
	private void Start()
	{
		Remove();
	}

	private void Update()
	{
	}

	private void Remove()
	{
		// Native offline branch returns unchanged at VA 0x12A759C.
		// Only a remote online PhotonView disables gravity and sets isKinematic.
		if (GameController.i && GameController.i.online)
			Debug.LogWarning("Remote rigidbody ownership is unavailable offline.", this);
	}
}
