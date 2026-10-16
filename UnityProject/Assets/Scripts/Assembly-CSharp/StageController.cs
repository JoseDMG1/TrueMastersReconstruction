using UnityEngine;

public class StageController : MonoBehaviour
{
	public static StageController i;

	public GameObject[] stages;

	public GameObject stage;

	public GameObject music;

	private void Awake()
	{
		// Native 0x12B2DD0: instantiate the selected scene prefab, then bind
		// its immediate child tagged music. No inferred resource-name search.
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		int index = PlayerPrefs.GetInt("Stage", 0);
		if (index < 0 || index >= stages.Length)
			throw new System.InvalidOperationException("Selected recovered stage index out of range: " + index);
		stage = Instantiate(stages[index], transform.position, transform.rotation);
		music = TagInChildren.FindGameObjectInChildWithTag(stage, "music");
	}
}
