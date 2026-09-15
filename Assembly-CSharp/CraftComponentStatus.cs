using System;

// Token: 0x02000268 RID: 616
public enum CraftComponentStatus
{
	// Token: 0x0400126C RID: 4716
	None,
	// Token: 0x0400126D RID: 4717
	Started,
	// Token: 0x0400126E RID: 4718
	Finished,
	// Token: 0x0400126F RID: 4719
	Canceled,
	// Token: 0x04001270 RID: 4720
	QueueDelayed,
	// Token: 0x04001271 RID: 4721
	FinishDelayed,
	// Token: 0x04001272 RID: 4722
	ReadyToStartCraft,
	// Token: 0x04001273 RID: 4723
	ReadyToFinishAutoCraft,
	// Token: 0x04001274 RID: 4724
	WaitingForWorkerPickUp,
	// Token: 0x04001275 RID: 4725
	WaitingForOutputDrop
}
