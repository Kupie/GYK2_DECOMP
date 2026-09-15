using System;

// Token: 0x020003FC RID: 1020
[Flags]
public enum ConditionalEventType
{
	// Token: 0x040019D0 RID: 6608
	None = 0,
	// Token: 0x040019D1 RID: 6609
	GameResChanged = 1,
	// Token: 0x040019D2 RID: 6610
	ItemsChanged = 2,
	// Token: 0x040019D3 RID: 6611
	CraftProgressChanged = 4,
	// Token: 0x040019D4 RID: 6612
	CraftStatusChanged = 8,
	// Token: 0x040019D5 RID: 6613
	WorkStateChanged = 16,
	// Token: 0x040019D6 RID: 6614
	ConveyorChanged = 32,
	// Token: 0x040019D7 RID: 6615
	FightingAgentChanged = 64,
	// Token: 0x040019D8 RID: 6616
	BuildingModeChanged = 128,
	// Token: 0x040019D9 RID: 6617
	CaretakerStateChanged = 256,
	// Token: 0x040019DA RID: 6618
	DockPointStatusChanged = 512,
	// Token: 0x040019DB RID: 6619
	TakenDockPointChanged = 1024,
	// Token: 0x040019DC RID: 6620
	HPChanged = 2048,
	// Token: 0x040019DD RID: 6621
	InteractableStateChanged = 4096,
	// Token: 0x040019DE RID: 6622
	ConveyorSystemChanged = 8192,
	// Token: 0x040019DF RID: 6623
	ParentWorkCondition = 16384,
	// Token: 0x040019E0 RID: 6624
	ZombieWorkerStateChanged = 32768,
	// Token: 0x040019E1 RID: 6625
	All = -1
}
