using System;
using JetBrains.Annotations;

// Token: 0x02000275 RID: 629
public interface ICraftable
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06001051 RID: 4177
	string CraftableObjectId { get; }

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06001052 RID: 4178
	Inventory CraftableObjectCraftInventory { get; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06001053 RID: 4179
	Inventory CraftableObjectInventory { get; }

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06001054 RID: 4180
	[CanBeNull]
	IWorker CraftableAttachedWorker { get; }

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06001055 RID: 4181
	float AutoCraftTickDuration { get; }

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06001056 RID: 4182
	CraftableType CraftableType { get; }

	// Token: 0x06001057 RID: 4183 RVA: 0x00002318 File Offset: 0x00000518
	void OnAddToQueue(CraftElementBase craftElement)
	{
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x00002318 File Offset: 0x00000518
	void OnCraftStart(CraftElementBase craftElement)
	{
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x00002318 File Offset: 0x00000518
	void OnCraftEnd(CraftElementBase craftElement)
	{
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x00002318 File Offset: 0x00000518
	void OnCraftCancel(CraftElementBase craftElement)
	{
	}

	// Token: 0x0600105B RID: 4187
	MultiInventory GetCraftableMultiInventory(bool excludeWorkerInventory = false);

	// Token: 0x0600105C RID: 4188 RVA: 0x00002318 File Offset: 0x00000518
	void OnSuccessfulTicksChange(int startTick, int endTick, CraftElementBase craftElement)
	{
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x00002318 File Offset: 0x00000518
	void ProcessReadyToFinishCraft()
	{
	}

	// Token: 0x0600105E RID: 4190
	void MakeDrop(Item item);
}
