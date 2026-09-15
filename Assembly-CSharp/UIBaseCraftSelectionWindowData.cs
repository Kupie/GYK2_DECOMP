using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000977 RID: 2423
public class UIBaseCraftSelectionWindowData : LazyWidgetDataBase
{
	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06003FE9 RID: 16361 RVA: 0x001323EB File Offset: 0x001305EB
	// (set) Token: 0x06003FEA RID: 16362 RVA: 0x001323F3 File Offset: 0x001305F3
	public Action OnCraftToQueueAdded { get; private set; }

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06003FEB RID: 16363 RVA: 0x001323FC File Offset: 0x001305FC
	// (set) Token: 0x06003FEC RID: 16364 RVA: 0x00132404 File Offset: 0x00130604
	public Action OnCraftStarted { get; private set; }

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06003FED RID: 16365 RVA: 0x0013240D File Offset: 0x0013060D
	// (set) Token: 0x06003FEE RID: 16366 RVA: 0x00132415 File Offset: 0x00130615
	public Action OnPressPlusQueue { get; private set; }

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06003FEF RID: 16367 RVA: 0x0013241E File Offset: 0x0013061E
	// (set) Token: 0x06003FF0 RID: 16368 RVA: 0x00132426 File Offset: 0x00130626
	public Action OnPressMinusQueue { get; private set; }

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x0013242F File Offset: 0x0013062F
	// (set) Token: 0x06003FF2 RID: 16370 RVA: 0x00132437 File Offset: 0x00130637
	public CraftComponent CraftComponent { get; private set; }

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x00132440 File Offset: 0x00130640
	// (set) Token: 0x06003FF4 RID: 16372 RVA: 0x00132448 File Offset: 0x00130648
	public CraftDef CraftDefinition { get; private set; }

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x00132451 File Offset: 0x00130651
	// (set) Token: 0x06003FF6 RID: 16374 RVA: 0x00132459 File Offset: 0x00130659
	public int CraftsCount { get; protected set; }

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x00132462 File Offset: 0x00130662
	// (set) Token: 0x06003FF8 RID: 16376 RVA: 0x0013246A File Offset: 0x0013066A
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x00132473 File Offset: 0x00130673
	// (set) Token: 0x06003FFA RID: 16378 RVA: 0x0013247B File Offset: 0x0013067B
	public List<UICraftRequirementWidgetData> CraftRequirementWidgetData { get; private set; }

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06003FFB RID: 16379 RVA: 0x00132484 File Offset: 0x00130684
	// (set) Token: 0x06003FFC RID: 16380 RVA: 0x0013248C File Offset: 0x0013068C
	public CraftParamsData ParamsData { get; private set; }

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06003FFD RID: 16381 RVA: 0x00132495 File Offset: 0x00130695
	// (set) Token: 0x06003FFE RID: 16382 RVA: 0x0013249D File Offset: 0x0013069D
	public WgoData WgoData { get; private set; }

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06003FFF RID: 16383 RVA: 0x001324A6 File Offset: 0x001306A6
	// (set) Token: 0x06004000 RID: 16384 RVA: 0x001324AE File Offset: 0x001306AE
	public IWorker DisplayableWorker { get; private set; }

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06004001 RID: 16385 RVA: 0x001324B7 File Offset: 0x001306B7
	// (set) Token: 0x06004002 RID: 16386 RVA: 0x001324BF File Offset: 0x001306BF
	public bool CanStartCraft { get; private set; }

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06004003 RID: 16387 RVA: 0x001324C8 File Offset: 0x001306C8
	// (set) Token: 0x06004004 RID: 16388 RVA: 0x001324D0 File Offset: 0x001306D0
	public List<NeedItemData> CurrentNeedItems { get; private set; }

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06004005 RID: 16389 RVA: 0x001324D9 File Offset: 0x001306D9
	// (set) Token: 0x06004006 RID: 16390 RVA: 0x001324E1 File Offset: 0x001306E1
	public List<CraftElementBase> CraftQueue { get; private set; }

	// Token: 0x06004007 RID: 16391 RVA: 0x001324EC File Offset: 0x001306EC
	public UIBaseCraftSelectionWindowData(WgoData wgoData, CraftDef craftDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onAddToQueue, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onStartCraft)
	{
		UIBaseCraftSelectionWindowData <>4__this = this;
		this.WgoData = wgoData;
		this.DisplayableWorker = wgoData.Worker ?? MainGame.PlayerController;
		this.CraftComponent = wgoData.CraftComponent;
		this.CraftsCount = 1;
		this.CraftDefinition = craftDef;
		this.CraftQueue = wgoData.CraftComponent.CraftElementsQueue;
		this.OnCraftToQueueAdded = delegate
		{
			<>4__this.OnPressAction(onAddToQueue);
		};
		this.OnCraftStarted = delegate
		{
			<>4__this.OnPressAction(onStartCraft);
		};
		this.OnPressPlusQueue = new Action(this.OnPlus);
		this.OnPressMinusQueue = new Action(this.OnMinus);
		this.UpdateRequirements();
		this.FillCraftItemCellsData(false);
		this.UpdateCraftParamsDataAndCanStartStatus();
	}

	// Token: 0x06004008 RID: 16392 RVA: 0x001325BE File Offset: 0x001307BE
	public void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChanges)
		{
			this.subscribedToDataChanges = true;
			this.CraftComponent.OnCraftAddedToQueue += this.onCraftAddedToQueue;
			this.CraftComponent.OnCraftRemovedFromQueue += this.onCraftRemovedFromQueue;
		}
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x001325F1 File Offset: 0x001307F1
	public void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChanges)
		{
			this.subscribedToDataChanges = false;
			this.CraftComponent.OnCraftAddedToQueue -= this.onCraftAddedToQueue;
			this.CraftComponent.OnCraftRemovedFromQueue -= this.onCraftRemovedFromQueue;
		}
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x00132624 File Offset: 0x00130824
	public void SetCraftCount(int value)
	{
		this.CraftsCount = value;
	}

	// Token: 0x0600400B RID: 16395 RVA: 0x00132630 File Offset: 0x00130830
	public void AddCraftsCount(int delta)
	{
		if (delta == 0)
		{
			return;
		}
		int num = Math.Clamp(this.CraftsCount + delta, this.MinCraftsCount, this.MaxCraftsCount);
		if (num == this.CraftsCount)
		{
			return;
		}
		this.CraftsCount = num;
		this.UpdateCanStartStatus();
		this.UpdateRequirements();
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x0600400C RID: 16396 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	protected virtual int MinCraftsCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x0600400D RID: 16397 RVA: 0x00132678 File Offset: 0x00130878
	protected virtual int MaxCraftsCount
	{
		get
		{
			return 999;
		}
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x00132680 File Offset: 0x00130880
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
			if (!this.CraftDefinition.id.StartsWith("mix") || needItemData.IsGroup || needItemData.ItemDef == null || !needItemData.ItemDef.isFuel)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, craftableMultiInventory, new Action(this.UpdateCraftParamsDataAndCanStartStatus), 0f, drawRunes, this.WgoData));
			}
		}
		CraftDef craftDefinition = this.CraftDefinition;
		if (craftDefinition != null && craftDefinition.hasDurabilityUseItem)
		{
			this.CraftItemCellsData.Add(new UICraftItemCellData(craftDefinition.durabilityUseItem, craftableMultiInventory, new Action(this.UpdateCraftParamsDataAndCanStartStatus), craftDefinition.needItemsDurabilityUse, false, this.WgoData));
		}
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x001327A4 File Offset: 0x001309A4
	private void OnPressAction(Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress)
	{
		if (onPress != null)
		{
			onPress(this.CraftDefinition, this.CurrentNeedItems, this.ParamsData, this.CraftsCount);
		}
	}

	// Token: 0x06004010 RID: 16400 RVA: 0x001327C7 File Offset: 0x001309C7
	private void OnPlus()
	{
		this.AddCraftsCount(1);
	}

	// Token: 0x06004011 RID: 16401 RVA: 0x001327D0 File Offset: 0x001309D0
	protected virtual void OnMinus()
	{
		this.AddCraftsCount(-1);
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x001327DC File Offset: 0x001309DC
	protected void UpdateRequirements()
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
					num += perkData.Definition.energyAdd;
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
			UICraftRequirementWidgetData uicraftRequirementWidgetData = new UICraftRequirementWidgetData("energy", text, 0f, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, false, true);
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData);
		}
		if (this.CraftDefinition.insanityPerTick.HasExpression)
		{
			float num = this.CraftDefinition.insanityPerTick.EvaluateFloat();
			foreach (PerkData perkData2 in linkedActivePerks)
			{
				if (!perkData2.Definition.insanityAdd.EqualsTo(0f, 1E-05f))
				{
					num += perkData2.Definition.insanityAdd;
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
			UICraftRequirementWidgetData uicraftRequirementWidgetData2 = new UICraftRequirementWidgetData("insanity", text2, 0f, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, false, true);
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData2);
		}
		if (this.CraftDefinition.insanityLock.HasExpression)
		{
			float num = this.CraftDefinition.insanityLock.EvaluateFloat();
			UICraftRequirementWidgetData uicraftRequirementWidgetData3 = new UICraftRequirementWidgetData("insanity_lock", "icon_sanity_lock", num, linkedActivePerks, this.CraftDefinition, toolForWorkOnCraft, false, true);
			this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData3);
		}
		if (this.CraftDefinition.id.StartsWith("mix"))
		{
			MultiInventory craftableMultiInventory = this.WgoData.GetCraftableMultiInventory(true);
			foreach (NeedItemData needItemData in this.CraftDefinition.needItems)
			{
				if (!needItemData.IsGroup && needItemData.ItemDef != null && needItemData.ItemDef.isFuel)
				{
					UICraftRequirementWidgetData uicraftRequirementWidgetData4 = new UICraftRequirementWidgetData(needItemData.ItemDef.id, needItemData.ItemDef.id, (float)needItemData.GetCount(this.WgoData), new List<PerkData>(), this.CraftDefinition, toolForWorkOnCraft, true, craftableMultiInventory.GetTotalCount(needItemData.ItemDef.id) >= needItemData.GetCount(this.WgoData));
					this.CraftRequirementWidgetData.Add(uicraftRequirementWidgetData4);
				}
			}
		}
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x00132BCC File Offset: 0x00130DCC
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

	// Token: 0x06004014 RID: 16404 RVA: 0x00132C70 File Offset: 0x00130E70
	private void UpdateCraftParamsDataAndCanStartStatus()
	{
		this.CurrentNeedItems = this.GetCurrentNeedItems();
		if (this.ParamsData == null)
		{
			this.ParamsData = new CraftParamsData(this.CraftDefinition.id, this.WgoData, CraftParamsData.CraftParamsType.Common, -1);
		}
		this.ParamsData.RecalculateParams(this.CurrentNeedItems, this.DisplayableWorker);
		this.UpdateCanStartStatus();
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x00132CCC File Offset: 0x00130ECC
	protected void UpdateCanStartStatus()
	{
		if (this.CraftDefinition.isFuelCraft)
		{
			MultiInventory craftableMultiInventory = this.WgoData.GetCraftableMultiInventory(false);
			using (List<UICraftItemCellData>.Enumerator enumerator = this.CraftItemCellsData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICraftItemCellData uicraftItemCellData = enumerator.Current;
					if (craftableMultiInventory.GetTotalCount(uicraftItemCellData.currentItem.Id) < this.CraftsCount * uicraftItemCellData.currentItem.GetCount(this.WgoData))
					{
						this.CanStartCraft = false;
						break;
					}
					this.CanStartCraft = this.CraftDefinition.CanActuallyStartInstantCraft(this.CurrentNeedItems, this.WgoData);
				}
				return;
			}
		}
		CraftElement craftElement = new CraftElement(this.CraftDefinition.id, 1, this.CurrentNeedItems, this.ParamsData);
		this.CanStartCraft = this.WgoData.CraftComponent.GetStartCraftStatus(craftElement, null) == CraftStatus.OK;
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x00132DC0 File Offset: 0x00130FC0
	private List<NeedItemData> GetCurrentNeedItems()
	{
		List<NeedItemData> list = new List<NeedItemData>();
		foreach (UICraftItemCellData uicraftItemCellData in this.CraftItemCellsData)
		{
			list.Add(uicraftItemCellData.currentItem);
		}
		return list;
	}

	// Token: 0x0400325F RID: 12895
	public CraftComponent.DelCraftAddedToQueue onCraftAddedToQueue;

	// Token: 0x04003260 RID: 12896
	public CraftComponent.DelCraftRemovedFromQueue onCraftRemovedFromQueue;

	// Token: 0x04003261 RID: 12897
	private bool subscribedToDataChanges;
}
