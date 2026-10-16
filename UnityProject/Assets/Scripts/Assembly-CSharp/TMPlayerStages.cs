using System;

[Serializable]
public class TMPlayerStages
{
	public int stage1;

	public int stage2;

	public int stage3;

	public int stage4;

	public int stage5;

	public int stage6;

	public int stage7;

	public TMPlayerStages(int stage1, int stage2, int stage3, int stage4, int stage5, int stage6, int stage7)
	{
		this.stage1 = stage1; this.stage2 = stage2; this.stage3 = stage3; this.stage4 = stage4;
		this.stage5 = stage5; this.stage6 = stage6; this.stage7 = stage7;
	}

	public TMPlayerStages()
	{
	}
}
