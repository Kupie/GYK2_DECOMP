using System;

// Token: 0x02000326 RID: 806
[Serializable]
public class FightingStageData
{
	// Token: 0x0600159F RID: 5535 RVA: 0x00069634 File Offset: 0x00067834
	public FightingStageData(int id)
	{
		this.id = id;
	}

	// Token: 0x04001619 RID: 5657
	public int id;

	// Token: 0x0400161A RID: 5658
	public bool enabled;
}
