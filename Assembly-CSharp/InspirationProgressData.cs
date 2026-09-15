using System;

// Token: 0x020005A6 RID: 1446
[Serializable]
public class InspirationProgressData
{
	// Token: 0x0600255E RID: 9566 RVA: 0x00021B94 File Offset: 0x0001FD94
	public InspirationProgressData()
	{
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000AF1EB File Offset: 0x000AD3EB
	public InspirationProgressData(string id, int currentValue, int completionGoalValue = 0)
	{
		this.id = id;
		this.currentValue = currentValue;
		this.completionGoalValue = completionGoalValue;
	}

	// Token: 0x040020C1 RID: 8385
	public string id;

	// Token: 0x040020C2 RID: 8386
	public int currentValue;

	// Token: 0x040020C3 RID: 8387
	public int completionGoalValue;
}
