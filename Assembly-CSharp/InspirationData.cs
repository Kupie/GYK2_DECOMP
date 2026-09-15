using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
[Serializable]
public class InspirationData
{
	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06002553 RID: 9555 RVA: 0x000AEF80 File Offset: 0x000AD180
	public bool IsCompleted
	{
		get
		{
			return this.curProgressValue >= this.completionGoalValue;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06002554 RID: 9556 RVA: 0x000AEF93 File Offset: 0x000AD193
	public float Progress01
	{
		get
		{
			return Mathf.Clamp01((float)this.curProgressValue / (float)this.completionGoalValue);
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06002555 RID: 9557 RVA: 0x000AEFA9 File Offset: 0x000AD1A9
	public int MaxLevel
	{
		get
		{
			return GameBalance.Me.inspirationLevelsCache[this.id].levels.Count;
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06002556 RID: 9558 RVA: 0x000AEFCA File Offset: 0x000AD1CA
	public List<InspirationDef> PurchasedInspirations
	{
		get
		{
			return this.purchasedInspirations;
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06002557 RID: 9559 RVA: 0x000AEFD2 File Offset: 0x000AD1D2
	public List<InspirationDef> CompletedInspirations
	{
		get
		{
			return this.completedInspirations;
		}
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06002558 RID: 9560 RVA: 0x000AEFDC File Offset: 0x000AD1DC
	public bool IsAvailableToBuy
	{
		get
		{
			return this.IsCompleted && !this.isAllLevelsBought && MainGame.PlayerData.inventory.Data.HasItemQuantityInInventory("faith", InspirationDef.GetDataForLevel(this.id, this.curLevel).completionPrice);
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06002559 RID: 9561 RVA: 0x000AF02A File Offset: 0x000AD22A
	public bool IsHidden
	{
		get
		{
			return MainGame.Instance.GameSave.knowledgeSystem.IsInspirationHidden(this.id);
		}
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000AF048 File Offset: 0x000AD248
	public void DoLevelUp()
	{
		this.purchasedInspirations.Insert(0, InspirationDef.GetDataForLevel(this.id, this.curLevel));
		InspirationLevelData inspirationLevelData = GameBalance.Me.inspirationLevelsCache[this.id];
		this.isAllLevelsBought = this.curLevel == inspirationLevelData.levels.Count;
		int num = Mathf.Clamp(this.curLevel + 1, 1, inspirationLevelData.levels.Count);
		InspirationDef dataForLevel = inspirationLevelData.GetDataForLevel(num);
		this.curLevel = num;
		this.completionGoalValue = dataForLevel.completionGoalValue;
		MainGame.Instance.GameSave.knowledgeSystem.TryRevealInspirations();
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x000AF0EC File Offset: 0x000AD2EC
	public void FillBoughtInspirations()
	{
		this.purchasedInspirations.Clear();
		for (int i = 1; i < this.curLevel; i++)
		{
			this.purchasedInspirations.Add(InspirationDef.GetDataForLevel(this.id, i));
		}
		if (this.isAllLevelsBought)
		{
			this.purchasedInspirations.Add(InspirationDef.GetDataForLevel(this.id, this.curLevel));
		}
		this.purchasedInspirations.Reverse();
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x000AF15C File Offset: 0x000AD35C
	public void FillCompletedInspirations()
	{
		if (this.IsHidden)
		{
			return;
		}
		if (!this.isAllLevelsBought && this.IsCompleted)
		{
			for (int i = this.curLevel; i <= this.MaxLevel; i++)
			{
				InspirationDef dataForLevel = InspirationDef.GetDataForLevel(this.id, i);
				if (!this.completedInspirations.Contains(dataForLevel) && this.curProgressValue >= dataForLevel.completionGoalValue)
				{
					this.completedInspirations.Add(dataForLevel);
				}
			}
		}
	}

	// Token: 0x040020BA RID: 8378
	public string id;

	// Token: 0x040020BB RID: 8379
	public int curLevel;

	// Token: 0x040020BC RID: 8380
	public int curProgressValue;

	// Token: 0x040020BD RID: 8381
	public int completionGoalValue;

	// Token: 0x040020BE RID: 8382
	public bool isAllLevelsBought;

	// Token: 0x040020BF RID: 8383
	[NonSerialized]
	private List<InspirationDef> purchasedInspirations = new List<InspirationDef>();

	// Token: 0x040020C0 RID: 8384
	[NonSerialized]
	private List<InspirationDef> completedInspirations = new List<InspirationDef>();
}
