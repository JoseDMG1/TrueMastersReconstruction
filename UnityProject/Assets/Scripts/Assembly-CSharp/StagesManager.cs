using System.Collections.Generic;
using UnityEngine;

public class StagesManager : MonoBehaviour
{
	public static StagesManager i;

	public List<int> stages;

	public List<int> playableStages;

	public int actualStage;

	public int lastStage;

	// This reconstruction build exposes every map without changing saved ownership.
	public bool unlockAllMaps = true;

	private void Start()
	{
		if (i == null) i = this;
		else if (i != this) { Destroy(this); return; }
		stages = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
		SetPlayableStages();
		actualStage = 0;
	}

	private void AddFreeStages()
	{
	}

	public void SetPlayableStages()
	{
		playableStages = new List<int>(stages);
		if (!unlockAllMaps) RemoveBlockStages();
	}

	private void RemoveBlockStages()
	{
		var player = SystemManager.i.tmPlayer;
		if (player.tmPremium == 2 || player.tmPremium == 3) return;
		var s = player.stages;
		int[] ownership = { s.stage1, s.stage2, s.stage3, s.stage4, s.stage5, s.stage6 };
		for (int index = 0; index < ownership.Length; index++)
			if (ownership[index] != 1) playableStages.Remove(index + 1);
	}

	public void CheckInitialStage()
	{
		if (!IsStageUnlocked(actualStage)) actualStage = 0;
	}

	public bool HasStage()
	{
		return IsStageUnlocked(actualStage);
	}

	public int RemoveRandom(int stage)
	{
		if (stage == 7)
		{
			var candidates = new List<int>(playableStages);
			candidates.Remove(7);
			if (candidates.Count >= 2) candidates.Remove(lastStage);
			stage = candidates[Random.Range(0, candidates.Count)];
		}
		lastStage = stage;
		return stage;
	}

	public bool IsStageUnlocked(int stageName)
	{
		// Offline branch of 0x12C2850. Original auth+premium branch is online-only.
		return playableStages.Contains(stageName);
	}
}
