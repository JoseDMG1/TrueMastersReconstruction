using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
	public TMP_Text speedText;

	public List<string> speedList;

	private int actualSpeed;

	private void Start()
	{
		SetSpeed(PlayerPrefs.GetInt("GameSpeed", 1));
	}

	public void SetSpeed(int speed)
	{
		actualSpeed = speed;
		speedText.text = speedList[speed];
		PlayerPrefs.SetInt("GameSpeed", speed);
	}

	public void ChangeSpeed(int next)
	{
		actualSpeed += next;
		if (actualSpeed == -1) actualSpeed = speedList.Count - 1;
		if (actualSpeed > speedList.Count - 1) actualSpeed = 0;
		SetSpeed(actualSpeed);
	}
}
