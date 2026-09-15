using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000581 RID: 1409
[Serializable]
public class CraftParamsData
{
	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06002412 RID: 9234 RVA: 0x000A9CCC File Offset: 0x000A7ECC
	[CanBeNull]
	private WgoData WgoData
	{
		get
		{
			if (!(this.wgoUniqueId == null))
			{
				return MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoUniqueId);
			}
			return null;
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06002413 RID: 9235 RVA: 0x000A9CF8 File Offset: 0x000A7EF8
	public CraftDefBase CraftDef
	{
		get
		{
			return GameBalance.GetCraftDefBase(this.craftId);
		}
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06002414 RID: 9236 RVA: 0x000A9D08 File Offset: 0x000A7F08
	public TalentDef TalentDef
	{
		get
		{
			if (this.WgoData == null)
			{
				return null;
			}
			if (this.assignedTalentDef == null)
			{
				return this.assignedTalentDef = GameBalance.Me.GetDataOrNull<TalentDef>(this.WgoData.Definition.talent);
			}
			return this.assignedTalentDef;
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06002415 RID: 9237 RVA: 0x000A9D51 File Offset: 0x000A7F51
	public int MasteryLock
	{
		get
		{
			if (this.customMasteryLock > -1)
			{
				return this.customMasteryLock;
			}
			if (this.CraftDef == null)
			{
				return 1;
			}
			return this.CraftDef.talentLock;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06002416 RID: 9238 RVA: 0x000A9D78 File Offset: 0x000A7F78
	public int MasteryValue
	{
		get
		{
			bool flag = this.WgoData != null && this.WgoData.Worker != null && !(this.WgoData.Worker is global::UnityEngine.Object);
			int num2;
			if (this.WgoData != null && !flag && this.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing)
			{
				int num;
				if (this.gardenType == CraftParamsData.GardenType.Vineyard)
				{
					num = MainGame.PlayerData.GetResInt("g_vineyard_farming_base");
				}
				else
				{
					num = MainGame.PlayerData.GetResInt("g_garden_farming_base");
				}
				num2 = num;
			}
			else if (this.TalentDef == null)
			{
				num2 = 1;
			}
			else if (!flag)
			{
				num2 = MainGame.PlayerController.GetMasteryLevelForTalentBranch(this.TalentDef.id, this.CraftDef);
			}
			else
			{
				num2 = this.WgoData.Worker.GetMasteryLevelForTalentBranch(this.TalentDef.id, this.CraftDef);
			}
			return num2 + this.GetWgoPerksCraftMasteryBonusValue();
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06002417 RID: 9239 RVA: 0x000A9E53 File Offset: 0x000A8053
	public ItemType RequiredToolType
	{
		get
		{
			if (this.requiredToolType == ItemType.None)
			{
				this.requiredToolType = this.GetRequiredToolType();
			}
			return this.requiredToolType;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06002418 RID: 9240 RVA: 0x000A9E70 File Offset: 0x000A8070
	public bool HasRequiredTool
	{
		get
		{
			if (this.WgoData == null)
			{
				return true;
			}
			if (this.WgoData.Worker == null)
			{
				return MainGame.PlayerController.HasToolForWork(this.WgoData, this.CraftDef);
			}
			return this.WgoData.Worker.HasToolForWork(this.WgoData, this.CraftDef);
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06002419 RID: 9241 RVA: 0x000A9EC8 File Offset: 0x000A80C8
	public int PerksCraftStartTicksBonusValue
	{
		get
		{
			int num;
			if (this.WgoData == null)
			{
				num = 0;
			}
			else if (this.WgoData.Worker == null)
			{
				num = MainGame.PlayerController.GetPerksCraftStartTicksBonusValue(this.CraftDef);
			}
			else
			{
				num = this.WgoData.Worker.GetPerksCraftStartTicksBonusValue(this.CraftDef);
			}
			return num + this.GetWgoPerksCraftStartTicksBonusValue();
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600241A RID: 9242 RVA: 0x000A9F20 File Offset: 0x000A8120
	public int PerksCraftAddTotalProgressTicksValue
	{
		get
		{
			int num;
			if (this.WgoData == null)
			{
				num = 0;
			}
			else if (this.WgoData.Worker == null)
			{
				num = MainGame.PlayerController.GetPerksCraftAddTotalProgressTicksValue(this.CraftDef);
			}
			else
			{
				num = this.WgoData.Worker.GetPerksCraftAddTotalProgressTicksValue(this.CraftDef);
			}
			return num + this.GetWgoPerksCraftAddTotalProgressTicks();
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600241B RID: 9243 RVA: 0x000A9F78 File Offset: 0x000A8178
	public int PerksCraftMasteryBonusValue
	{
		get
		{
			if (this.WgoData != null)
			{
				return this.GetWgoPerksCraftMasteryBonusValue();
			}
			return 0;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x0600241C RID: 9244 RVA: 0x000A9F8A File Offset: 0x000A818A
	public int CraftStartTicks
	{
		get
		{
			return ((this.craftParamsType == CraftParamsData.CraftParamsType.GardenPlanting) ? 0 : (this.itemsCraftStartTicksBonusValue + this.PerksCraftStartTicksBonusValue)) + ((this.WgoData != null && this.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing) ? this.WgoData.GetGameResInt("succeded_cells") : 0);
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x0600241D RID: 9245 RVA: 0x000A9FCA File Offset: 0x000A81CA
	public int FailedStartTicks
	{
		get
		{
			if (this.WgoData == null || this.craftParamsType != CraftParamsData.CraftParamsType.GardenGrowing)
			{
				return 0;
			}
			return this.WgoData.GetGameResInt("failed_cells");
		}
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000A9FF0 File Offset: 0x000A81F0
	public CraftParamsData(string craftId, WgoData wgoData, CraftParamsData.CraftParamsType craftParamsType = CraftParamsData.CraftParamsType.Common, int customMasteryLock = -1)
	{
		this.craftId = craftId;
		this.wgoUniqueId = wgoData.UniqueId;
		this.customMasteryLock = customMasteryLock;
		CraftDef craftDef;
		if (craftParamsType == CraftParamsData.CraftParamsType.Common && GameBalance.Me.gardenGrowingCrafts.TryGetValue(craftId, out craftDef))
		{
			this.craftParamsType = CraftParamsData.CraftParamsType.GardenGrowing;
			this.gardenType = GameBalance.Me.gardenGrowingCraftTypes[craftId];
			return;
		}
		this.craftParamsType = craftParamsType;
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000AA077 File Offset: 0x000A8277
	public CraftParamsData(string craftId, GameRes customRes)
	{
		this.craftId = craftId;
		this.customRes = customRes;
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000AA0AC File Offset: 0x000A82AC
	public CraftParamsData(CraftParamsData other)
	{
		this.craftId = other.craftId;
		this.wgoUniqueId = other.wgoUniqueId;
		this.itemsCraftStartTicksBonusValue = other.itemsCraftStartTicksBonusValue;
		this.totalPerks = other.totalPerks;
		this.totalTalentsAurasAndPerks = other.totalTalentsAurasAndPerks;
		this.totalItemsQuality = other.totalItemsQuality;
		this.totalTalentsAurasPerksAndItemQuality = other.totalTalentsAurasPerksAndItemQuality;
		this.total = other.total;
		this.customRes = other.customRes;
		this.customMasteryLock = other.customMasteryLock;
		this.craftParamsType = other.craftParamsType;
		this.gardenType = other.gardenType;
		this.craftDef = other.craftDef;
		this.assignedTalentDef = other.assignedTalentDef;
		this.requiredToolType = other.requiredToolType;
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000AA190 File Offset: 0x000A8390
	public void RecalculateParams(List<NeedItemData> needItems, IWorker worker)
	{
		if (this.craftParamsType != CraftParamsData.CraftParamsType.GardenGrowing)
		{
			this.RecalculateItemsDependentParams(needItems);
		}
		this.RecalculateItemsNotDependentParams(worker);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000AA1A9 File Offset: 0x000A83A9
	public void RecalculateParams(List<Item> needItems, IWorker worker)
	{
		if (this.craftParamsType != CraftParamsData.CraftParamsType.GardenGrowing)
		{
			this.RecalculateItemsDependentParams(needItems);
		}
		this.RecalculateItemsNotDependentParams(worker);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000AA1C4 File Offset: 0x000A83C4
	public bool Equals(CraftParamsData other)
	{
		return this.craftId == other.craftId && this.totalItemsQuality.Equals(other.totalItemsQuality) && this.totalPerks.Equals(other.totalPerks) && this.totalTalentsAurasAndPerks.Equals(other.totalTalentsAurasAndPerks) && this.totalTalentsAurasPerksAndItemQuality.Equals(other.totalTalentsAurasPerksAndItemQuality) && this.total.Equals(other.total) && this.itemsCraftStartTicksBonusValue.Equals(other.itemsCraftStartTicksBonusValue) && this.PerksCraftStartTicksBonusValue.Equals(other.PerksCraftStartTicksBonusValue);
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000AA26F File Offset: 0x000A846F
	private void RecalculateItemsDependentParams(List<NeedItemData> needItems)
	{
		this.totalItemsQuality = NeedItemData.GetQualitySum(needItems);
		this.itemsCraftStartTicksBonusValue = NeedItemData.GetCraftStartTicksBonusValue(needItems);
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000AA289 File Offset: 0x000A8489
	private void RecalculateItemsDependentParams(List<Item> needItems)
	{
		this.totalItemsQuality = Item.GetQualityAverage(needItems);
		this.itemsCraftStartTicksBonusValue = Item.GetCraftStartTicksBonusValue(needItems);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000AA2A4 File Offset: 0x000A84A4
	private void RecalculateItemsNotDependentParams(IWorker worker)
	{
		this.totalTalentsAurasAndPerks = 0f;
		this.totalPerks = 0f;
		this.totalTalentsAurasAndPerks = this.totalPerks;
		this.totalTalentsAurasPerksAndItemQuality = this.totalTalentsAurasAndPerks + this.totalItemsQuality;
		this.total = this.totalTalentsAurasPerksAndItemQuality;
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000AA2F4 File Offset: 0x000A84F4
	private ItemType GetRequiredToolType()
	{
		ItemType itemType = ItemType.None;
		CraftDef craftDef = this.CraftDef as CraftDef;
		if (craftDef != null && craftDef.customItemTypeAction != ItemType.None && !this.CraftDef.isAuto)
		{
			itemType = craftDef.customItemTypeAction;
		}
		if (itemType == ItemType.None && this.WgoData != null)
		{
			itemType = this.WgoData.Definition.toolAction.actionableTool;
		}
		return itemType;
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000AA350 File Offset: 0x000A8550
	private int GetWgoPerksCraftAddTotalProgressTicks()
	{
		int num = 0;
		if (this.WgoData != null)
		{
			foreach (string text in this.CraftDef.linkedPerks)
			{
				if (this.WgoData.HasPerk(text))
				{
					num += GameBalance.Me.GetData<PerkDef>(text).craftTotalProgressTicksBonus;
				}
			}
		}
		return num;
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x000AA3D0 File Offset: 0x000A85D0
	private int GetWgoPerksCraftStartTicksBonusValue()
	{
		int num = 0;
		if (this.WgoData != null)
		{
			foreach (string text in this.CraftDef.linkedPerks)
			{
				if (this.WgoData.HasPerk(text))
				{
					num += GameBalance.Me.GetData<PerkDef>(text).craftStartTicks;
				}
			}
		}
		return num;
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000AA450 File Offset: 0x000A8650
	private int GetWgoPerksCraftMasteryBonusValue()
	{
		int num = 0;
		if (this.WgoData != null)
		{
			foreach (string text in this.CraftDef.linkedPerks)
			{
				if (this.WgoData.HasPerk(text))
				{
					num += GameBalance.Me.GetData<PerkDef>(text).craftMasteryBonus;
				}
			}
		}
		return num;
	}

	// Token: 0x04002008 RID: 8200
	[SerializeField]
	private string craftId;

	// Token: 0x04002009 RID: 8201
	[SerializeField]
	private SGuid wgoUniqueId;

	// Token: 0x0400200A RID: 8202
	[SerializeField]
	private int itemsCraftStartTicksBonusValue;

	// Token: 0x0400200B RID: 8203
	public float totalPerks;

	// Token: 0x0400200C RID: 8204
	public float totalTalentsAurasAndPerks;

	// Token: 0x0400200D RID: 8205
	public float totalItemsQuality = 1f;

	// Token: 0x0400200E RID: 8206
	public float totalTalentsAurasPerksAndItemQuality;

	// Token: 0x0400200F RID: 8207
	public float total;

	// Token: 0x04002010 RID: 8208
	public GameRes customRes = new GameRes();

	// Token: 0x04002011 RID: 8209
	public int customMasteryLock = -1;

	// Token: 0x04002012 RID: 8210
	public CraftParamsData.CraftParamsType craftParamsType;

	// Token: 0x04002013 RID: 8211
	public CraftParamsData.GardenType gardenType;

	// Token: 0x04002014 RID: 8212
	public ItemType selectedOrganTypeForChange;

	// Token: 0x04002015 RID: 8213
	[NonSerialized]
	private CraftDefBase craftDef;

	// Token: 0x04002016 RID: 8214
	[NonSerialized]
	private TalentDef assignedTalentDef;

	// Token: 0x04002017 RID: 8215
	[NonSerialized]
	private ItemType requiredToolType;

	// Token: 0x02000582 RID: 1410
	public enum CraftParamsType
	{
		// Token: 0x04002019 RID: 8217
		Common,
		// Token: 0x0400201A RID: 8218
		GardenGrowing,
		// Token: 0x0400201B RID: 8219
		GardenPlanting
	}

	// Token: 0x02000583 RID: 1411
	public enum GardenType
	{
		// Token: 0x0400201D RID: 8221
		None,
		// Token: 0x0400201E RID: 8222
		Vineyard
	}
}
