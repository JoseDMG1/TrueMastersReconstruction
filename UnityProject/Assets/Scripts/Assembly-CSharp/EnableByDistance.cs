using UnityEngine;

public class EnableByDistance : MonoBehaviour
{
	public GameObject objRef;

	public GameObject objToChange;

	public float distance;

	private void Update()
	{
		// Native 0x122A78C uses stage bounds, not distance to the rival.
		if (objRef && objToChange && GameController.i)
			objToChange.SetActive(GameController.i.IsOutsideStage(objRef.transform.position, distance));
	}
}
