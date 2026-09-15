using System;

// Token: 0x0200030B RID: 779
[Serializable]
public class CPA_SetSpawnerActive : CapturePointAction
{
	// Token: 0x060014C3 RID: 5315 RVA: 0x000659E0 File Offset: 0x00063BE0
	public override void Execute(LazyConsts.Fighting.TeamType teamType, FightingLevel level)
	{
		if (this.teamType != teamType)
		{
			return;
		}
		level.SetSpawnerActive(this.lineId, this.spawnZoneName, this.isActive);
	}

	// Token: 0x0400157F RID: 5503
	public bool isActive;

	// Token: 0x04001580 RID: 5504
	public int lineId;

	// Token: 0x04001581 RID: 5505
	public string spawnZoneName;
}
