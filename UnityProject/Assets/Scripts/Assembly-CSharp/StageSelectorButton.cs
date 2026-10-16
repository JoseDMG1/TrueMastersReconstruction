using UnityEngine;
using UnityEngine.UI;

public class StageSelectorButton : MonoBehaviour
{
	public static StageSelectorButton stageSelectorButton;

	public Animator StageSelectorAnim;

	public Image image;

	public int lastStage;

	private void Start()
	{
		stageSelectorButton = this;
		StagesManager.i.CheckInitialStage();
		ChangeImage();
	}

	private void OnDestroy()
	{
		if (stageSelectorButton == this) stageSelectorButton = null;
	}

	public void OpenStageSelector()
	{
		if (SelectionController.i.ready) return;
		StageSelectorAnim.SetTrigger("Slide");
		lastStage = StagesManager.i.actualStage;
	}

	public void CloseStageSelector()
	{
		StageSelectorAnim.SetTrigger("Slide");
		StagesManager.i.actualStage = lastStage;
		StageSelector.stageSelector.ChangeStage();
	}

	public void SelectStage()
	{
		if (!StagesManager.i.IsStageUnlocked(StagesManager.i.actualStage)) return;
		ChangeImage();
		StageSelectorAnim.SetTrigger("Slide");
	}

	public void ChangeImage()
	{
		image.sprite = Resources.Load<Sprite>("Images/Stages/" + StagesManager.i.actualStage);
	}
}
