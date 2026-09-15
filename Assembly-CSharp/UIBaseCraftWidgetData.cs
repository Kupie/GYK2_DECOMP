using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000981 RID: 2433
public class UIBaseCraftWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06004059 RID: 16473 RVA: 0x001344BD File Offset: 0x001326BD
	// (set) Token: 0x0600405A RID: 16474 RVA: 0x001344C5 File Offset: 0x001326C5
	public Action OnPress { get; private set; }

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x0600405B RID: 16475 RVA: 0x001344CE File Offset: 0x001326CE
	// (set) Token: 0x0600405C RID: 16476 RVA: 0x001344D6 File Offset: 0x001326D6
	public Action OnOver { get; private set; }

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x0600405D RID: 16477 RVA: 0x001344DF File Offset: 0x001326DF
	// (set) Token: 0x0600405E RID: 16478 RVA: 0x001344E7 File Offset: 0x001326E7
	public Action OnOut { get; private set; }

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x0600405F RID: 16479 RVA: 0x001344F0 File Offset: 0x001326F0
	// (set) Token: 0x06004060 RID: 16480 RVA: 0x001344F8 File Offset: 0x001326F8
	public Action OnPressPlusQueue { get; private set; }

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06004061 RID: 16481 RVA: 0x00134501 File Offset: 0x00132701
	// (set) Token: 0x06004062 RID: 16482 RVA: 0x00134509 File Offset: 0x00132709
	public Action OnPressMinusQueue { get; private set; }

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06004063 RID: 16483 RVA: 0x00134512 File Offset: 0x00132712
	// (set) Token: 0x06004064 RID: 16484 RVA: 0x0013451A File Offset: 0x0013271A
	public CraftComponent CraftComponent { get; private set; }

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06004065 RID: 16485 RVA: 0x00134523 File Offset: 0x00132723
	// (set) Token: 0x06004066 RID: 16486 RVA: 0x0013452B File Offset: 0x0013272B
	public CraftDef CraftDefinition { get; private set; }

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06004067 RID: 16487 RVA: 0x00134534 File Offset: 0x00132734
	// (set) Token: 0x06004068 RID: 16488 RVA: 0x0013453C File Offset: 0x0013273C
	public int CraftsCount { get; private set; }

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06004069 RID: 16489 RVA: 0x00134545 File Offset: 0x00132745
	// (set) Token: 0x0600406A RID: 16490 RVA: 0x0013454D File Offset: 0x0013274D
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x0600406B RID: 16491 RVA: 0x00134556 File Offset: 0x00132756
	// (set) Token: 0x0600406C RID: 16492 RVA: 0x0013455E File Offset: 0x0013275E
	public List<UICraftRequirementWidgetData> CraftRequirementWidgetData { get; private set; }

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x0600406D RID: 16493 RVA: 0x00134567 File Offset: 0x00132767
	// (set) Token: 0x0600406E RID: 16494 RVA: 0x0013456F File Offset: 0x0013276F
	public UICraftRequirementWidgetData FuelRequirementWidgetData { get; private set; }

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x0600406F RID: 16495 RVA: 0x00134578 File Offset: 0x00132778
	// (set) Token: 0x06004070 RID: 16496 RVA: 0x00134580 File Offset: 0x00132780
	public CraftParamsData ParamsData { get; private set; }

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06004071 RID: 16497 RVA: 0x00134589 File Offset: 0x00132789
	// (set) Token: 0x06004072 RID: 16498 RVA: 0x00134591 File Offset: 0x00132791
	public WgoData WgoData { get; set; }

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06004073 RID: 16499 RVA: 0x0013459A File Offset: 0x0013279A
	// (set) Token: 0x06004074 RID: 16500 RVA: 0x001345A2 File Offset: 0x001327A2
	public IWorker DisplayableWorker { get; set; }

	// Token: 0x06004075 RID: 16501 RVA: 0x001345AC File Offset: 0x001327AC
	public UIBaseCraftWidgetData(WgoData wgoData, CraftDef craftDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress, Action onOver, Action onOut)
	{
		UIBaseCraftWidgetData <>4__this = this;
		this.WgoData = wgoData;
		this.DisplayableWorker = wgoData.Worker ?? MainGame.PlayerController;
		this.CraftComponent = wgoData.CraftComponent;
		this.CraftsCount = 1;
		this.CraftDefinition = craftDef;
		this.OnPress = delegate
		{
			<>4__this.OnPressAction(onPress);
		};
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.OnPressPlusQueue = new Action(this.OnPlus);
		this.OnPressMinusQueue = new Action(this.OnMinus);
		this.UpdateRequirements();
		this.FillCraftItemCellsData(false);
		this.UpdateCraftParamsData();
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x00134664 File Offset: 0x00132864
	public UIBaseCraftWidgetData(WgoData wgoData, AlchemyMixDef mixDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress, Action onOver, Action onOut)
	{
		UIBaseCraftWidgetData <>4__this = this;
		this.WgoData = wgoData;
		this.DisplayableWorker = wgoData.Worker ?? MainGame.PlayerController;
		this.CraftComponent = wgoData.CraftComponent;
		this.CraftsCount = 1;
		this.CraftDefinition = mixDef;
		this.OnPress = delegate
		{
			<>4__this.OnPressAction(onPress);
		};
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.OnPressPlusQueue = new Action(this.OnPlus);
		this.OnPressMinusQueue = new Action(this.OnMinus);
		this.UpdateRequirements();
		this.FillCraftItemCellsData(true);
		this.UpdateCraftParamsData();
	}

	// Token: 0x06004077 RID: 16503 RVA: 0x0013471A File Offset: 0x0013291A
	public void UpdateCraftParamsData()
	{
		if (this.ParamsData == null)
		{
			this.ParamsData = new CraftParamsData(this.CraftDefinition.id, this.WgoData, CraftParamsData.CraftParamsType.Common, -1);
		}
		this.ParamsData.RecalculateParams(this.GetCurrentNeedItems(), this.DisplayableWorker);
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x0013475C File Offset: 0x0013295C
	public List<NeedItemData> GetCurrentNeedItems()
	{
		List<NeedItemData> list = new List<NeedItemData>();
		foreach (UICraftItemCellData uicraftItemCellData in this.CraftItemCellsData)
		{
			list.Add(uicraftItemCellData.currentItem);
		}
		return list;
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x001347BC File Offset: 0x001329BC
	public void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChages)
		{
			this.subscribedToDataChages = true;
		}
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x001347CD File Offset: 0x001329CD
	public void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChages)
		{
			this.subscribedToDataChages = false;
		}
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x001347E0 File Offset: 0x001329E0
	private void FillCraftItemCellsData(bool drawRunes = false)
	{
		if (this.CraftItemCellsData != null)
		{
			this.CraftItemCellsData.Clear();
		}
		else
		{
			this.CraftItemCellsData = new List<UICraftItemCellData>();
		}
		MultiInventory craftableMultiInventory = this.CraftComponent.CraftableObject.GetCraftableMultiInventory(false);
		foreach (NeedItemData needItemData in this.CraftDefinition.needItems)
		{
			this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, craftableMultiInventory, new Action(this.UpdateCraftParamsData), 0f, drawRunes, this.WgoData));
		}
		CraftDef craftDefinition = this.CraftDefinition;
		if (craftDefinition != null && craftDefinition.hasDurabilityUseItem)
		{
			this.CraftItemCellsData.Add(new UICraftItemCellData(craftDefinition.durabilityUseItem, craftableMultiInventory, new Action(this.UpdateCraftParamsData), craftDefinition.needItemsDurabilityUse, false, this.WgoData));
		}
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x001348D0 File Offset: 0x00132AD0
	private void OnPressAction(Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress)
	{
		if (onPress != null)
		{
			onPress(this.CraftDefinition, this.GetCurrentNeedItems(), this.ParamsData, this.CraftsCount);
		}
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x001348F3 File Offset: 0x00132AF3
	private void OnPlus()
	{
		this.AddCraftsCount(1);
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x001348FC File Offset: 0x00132AFC
	private void OnMinus()
	{
		this.AddCraftsCount(-1);
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x00134908 File Offset: 0x00132B08
	public void AddCraftsCount(int delta)
	{
		if (delta == 0)
		{
			return;
		}
		int num = Math.Clamp(this.CraftsCount + delta, 1, 999);
		if (num == this.CraftsCount)
		{
			return;
		}
		this.CraftsCount = num;
		this.UpdateRequirements();
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x00134944 File Offset: 0x00132B44
	private void UpdateRequirements()
	{
		if (this.CraftRequirementWidgetData == null)
		{
			this.CraftRequirementWidgetData = new List<UICraftRequirementWidgetData>();
		}
		else
		{
			this.CraftRequirementWidgetData.Clear();
		}
		List<PerkData> linkedActivePerks = this.GetLinkedActivePerks();
		Item toolForWorkOnCraft = this.DisplayableWorker.GetToolForWorkOnCraft(this.WgoData, this.CraftDefinition);
		if (this.CraftDefinition.energyPerTick.HasExpression)
		{
			float num = this.CraftDefinition.energyPerTick.EvaluateFloat();
			foreach (PerkData perkData in linkedActivePerks)
			{
				if (!perkData.Definition.energyAdd.EqualsTo(0f, 1E-05f))
				{
					num -= perkData.Definition.energyAdd;
				}
			}
			if (!toolForWorkOnCraft.IsEmpty && !toolForWorkOnCraft.Definition.GetGameResOnUse("energy").EqualsTo(0f, 1E-05f))
			{
				num -= toolForWorkOnCraft.Definition.GetGameResOnUse("energy");
			}
			string text = "energy_1";
			if (num >= ConstDef.Get("energy_craft_border_2").FloatValue)
			{
				text = "energy_3";
			}
			else if (num >= ConstDef.Get("energy_craft_border_1").FloatValue)
			{
				text = "energy_2";
			}
			UICraftRequirementWidgetData uicraftRequirementWidgetData = new UICraftRequirementWidgetData("energy", text, num, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, true, PlayerEnergyGameResSystem.GetSystem().IsEnoughValue(num));
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData);
		}
		if (this.CraftDefinition.insanityPerTick.HasExpression)
		{
			float num = this.CraftDefinition.insanityPerTick.EvaluateFloat();
			foreach (PerkData perkData2 in linkedActivePerks)
			{
				if (!perkData2.Definition.insanityAdd.EqualsTo(0f, 1E-05f))
				{
					num -= perkData2.Definition.insanityAdd;
				}
			}
			if (!toolForWorkOnCraft.IsEmpty && !toolForWorkOnCraft.Definition.GetGameResOnUse("insanity").EqualsTo(0f, 1E-05f))
			{
				num -= toolForWorkOnCraft.Definition.GetGameResOnUse("insanity");
			}
			string text2 = "insanity_1";
			if (num >= ConstDef.Get("insanity_craft_border_2").FloatValue)
			{
				text2 = "insanity_3";
			}
			else if (num >= ConstDef.Get("insanity_craft_border_1").FloatValue)
			{
				text2 = "insanity_2";
			}
			UICraftRequirementWidgetData uicraftRequirementWidgetData2 = new UICraftRequirementWidgetData("insanity", text2, num, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, true, PlayerInsanityGameResSystem.GetSystem().CanChangeInsanity(num));
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData2);
		}
		if (this.CraftDefinition.insanityLock.HasExpression)
		{
			float num = this.CraftDefinition.insanityLock.EvaluateFloat();
			UICraftRequirementWidgetData uicraftRequirementWidgetData3 = new UICraftRequirementWidgetData("insanity_lock", "icon_sanity_lock", num, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, true, PlayerInsanityGameResSystem.GetSystem().IsEnoughValue(num));
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData3);
		}
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00134C48 File Offset: 0x00132E48
	private List<PerkData> GetLinkedActivePerks()
	{
		List<PerkData> list = new List<PerkData>();
		PerkSystemData perkSystemData = MainGame.Instance.GameSave.perkSystemData;
		using (List<string>.Enumerator enumerator = this.CraftDefinition.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				int num = perkSystemData.activePerks.FindIndex((PerkData x) => x.id == linkedPerk);
				if (num != -1)
				{
					list.Add(perkSystemData.activePerks[num]);
				}
			}
		}
		return list;
	}

	// Token: 0x04003299 RID: 12953
	private bool subscribedToDataChages;
}
