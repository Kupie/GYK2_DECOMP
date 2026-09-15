using System;

// Token: 0x0200033A RID: 826
[Serializable]
public abstract class FightingPhaseData
{
	// Token: 0x06001601 RID: 5633 RVA: 0x0006A71A File Offset: 0x0006891A
	public virtual void UpdatePhase(float progress, FightingLevelPresetProcessor processor, FightingLevelPreset.FightingLineData line, out int sentToSpawnThisTime)
	{
		sentToSpawnThisTime = 0;
	}

	// Token: 0x04001660 RID: 5728
	public int duration;
}
