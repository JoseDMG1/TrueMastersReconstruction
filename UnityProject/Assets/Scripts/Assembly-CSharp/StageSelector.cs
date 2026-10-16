using UnityEngine;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
	public static StageSelector stageSelector;

	public Image stageImage;

	public GameObject unlockButton;

	private void Awake()
	{
		stageSelector = this;
		StagesManager.i.CheckInitialStage();
		ChangeStage();
	}

	private void OnDestroy()
	{
		if (stageSelector == this) stageSelector = null;
	}

	public void nextStage()
	{
		if (StagesManager.i.actualStage == StagesManager.i.stages.Count - 1) return;
		StagesManager.i.actualStage++;
		ChangeStage();
	}

	public void previousStage()
	{
		if (StagesManager.i.actualStage == 0) return;
		StagesManager.i.actualStage--;
		ChangeStage();
	}

	public void ChangeStage()
	{
		stageImage.sprite = Resources.Load<Sprite>("Images/Stages/" + StagesManager.i.actualStage);
		CheckStage();
	}

	public void CheckStage()
	{
		unlockButton.SetActive(!StagesManager.i.IsStageUnlocked(StagesManager.i.actualStage));
	}
}
