using TMPro;
using UnityEngine;

public class SurvivalRecord : MonoBehaviour
{
	public TMP_Text recordValue;

	private void Start()
	{
		float value = SystemManager.i.tmPlayer.survivalRecord;
		if (value == 0f) Destroy(gameObject);
		else recordValue.text = value.ToString();
	}

	private void Update()
	{
	}
}
