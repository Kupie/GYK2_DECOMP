using System;

// Token: 0x0200035F RID: 863
public interface IWorker
{
	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060016E7 RID: 5863
	SGuid Id { get; }

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060016E8 RID: 5864
	IWorkActivity WorkerActivity { get; }

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060016E9 RID: 5865
	MultiInventory WorkerMultiInventory { get; }

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x060016EA RID: 5866
	Inventory WorkerInventory { get; }

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x060016EB RID: 5867
	Inventory WorkerToolInventory { get; }

	// Token: 0x060016EC RID: 5868
	int GetMasteryLevelForTalentBranch(string talentId, CraftDefBase craftDef = null);

	// Token: 0x060016ED RID: 5869
	bool HasToolForWork(WgoData wgoData, CraftDefBase craftDef);

	// Token: 0x060016EE RID: 5870
	bool HasToolForWork(WgoData wgoData, out ItemType itemType);

	// Token: 0x060016EF RID: 5871
	int GetPerksCraftMasteryBonusValue(CraftDefBase craftDef);

	// Token: 0x060016F0 RID: 5872
	int GetPerksCraftStartTicksBonusValue(CraftDefBase craftDef);

	// Token: 0x060016F1 RID: 5873
	int GetPerksCraftAddTotalProgressTicksValue(CraftDefBase craftDef);

	// Token: 0x060016F2 RID: 5874
	float GetPerksEnergyBonusValue(CraftDefBase craftDef);

	// Token: 0x060016F3 RID: 5875
	float GetPerksInsanityBonusValue(CraftDefBase craftDef);

	// Token: 0x060016F4 RID: 5876
	Item GetToolForWorkOnCraft(WgoData wgoData, CraftDefBase craftDef);

	// Token: 0x060016F5 RID: 5877
	CraftStatus CheckWorkerDependentValues(CraftElement craftElement, float deltaTime = 1f, bool skipEnergyCheck = false, bool skipInsanityCheck = false);

	// Token: 0x060016F6 RID: 5878 RVA: 0x0006D65B File Offset: 0x0006B85B
	public static IWorker FromId(SGuid id)
	{
		if (id == null || id.IsEmpty)
		{
			return null;
		}
		if (id == MainGame.PlayerData.Guid)
		{
			return MainGame.PlayerController;
		}
		return MainGame.ZombieSystemData.GetZombie(id);
	}

	// Token: 0x060016F7 RID: 5879
	void AddRes(string type, float value);

	// Token: 0x060016F8 RID: 5880
	void MultiplyRes(string type, float value);

	// Token: 0x060016F9 RID: 5881
	void SetRes(string type, float value);

	// Token: 0x060016FA RID: 5882
	float GetRes(string type, float defaultValue = 0f);
}
