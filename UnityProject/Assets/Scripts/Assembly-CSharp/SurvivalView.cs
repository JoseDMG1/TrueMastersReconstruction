using UnityEngine;

public class SurvivalView : MonoBehaviour
{
	public bool off;

	public string changeText;

	private void Start()
	{
		if (!string.IsNullOrEmpty(changeText)) GetComponent<TMPro.TMP_Text>().text = changeText;
		else if ((SystemManager.i.mode == GameMode.Survival && off) ||
			(SystemManager.i.mode != GameMode.Survival && !off)) Destroy(gameObject);
	}

	private void Update()
	{
	}
}
