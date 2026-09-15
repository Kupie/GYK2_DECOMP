using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000851 RID: 2129
public class UITooltipNeedsItemWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06003692 RID: 13970 RVA: 0x00108B17 File Offset: 0x00106D17
	// (set) Token: 0x06003693 RID: 13971 RVA: 0x00108B1F File Offset: 0x00106D1F
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06003694 RID: 13972 RVA: 0x00108B28 File Offset: 0x00106D28
	// (set) Token: 0x06003695 RID: 13973 RVA: 0x00108B30 File Offset: 0x00106D30
	public CraftDef CraftDefinition { get; private set; }

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06003696 RID: 13974 RVA: 0x00108B39 File Offset: 0x00106D39
	// (set) Token: 0x06003697 RID: 13975 RVA: 0x00108B41 File Offset: 0x00106D41
	public CraftComponent CraftComponent { get; private set; }

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06003698 RID: 13976 RVA: 0x00108B4A File Offset: 0x00106D4A
	// (set) Token: 0x06003699 RID: 13977 RVA: 0x00108B52 File Offset: 0x00106D52
	public bool DrawAsNeedItem { get; private set; } = true;

	// Token: 0x0600369A RID: 13978 RVA: 0x00108B5C File Offset: 0x00106D5C
	public UITooltipNeedsItemWidgetData(CraftDef craftDef, CraftComponent craftComponent, List<NeedItemData> customItems = null)
	{
		this.CraftDefinition = craftDef;
		this.CraftComponent = craftComponent;
		this.customItems = customItems;
		this.wgoData = craftComponent.CraftableObject as WgoData;
		this.FillCraftItemCellsData(craftComponent.CraftableObject.GetCraftableMultiInventory(false), false);
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x00108BAF File Offset: 0x00106DAF
	public UITooltipNeedsItemWidgetData(CraftDef craftDef, MultiInventory multiInventory, List<NeedItemData> customItems = null)
	{
		this.CraftDefinition = craftDef;
		this.customItems = customItems;
		this.FillCraftItemCellsData(multiInventory, false);
	}

	// Token: 0x0600369C RID: 13980 RVA: 0x00108BD4 File Offset: 0x00106DD4
	public UITooltipNeedsItemWidgetData(List<NeedItemData> needItems, MultiInventory multiInventory, WgoData wgoData = null)
	{
		this.DrawAsNeedItem = false;
		this.customItems = needItems;
		this.wgoData = wgoData;
		this.FillCraftItemCellsData(multiInventory, false);
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x00108C00 File Offset: 0x00106E00
	private void FillCraftItemCellsData(MultiInventory multiInventory, bool drawRunes = false)
	{
		if (this.CraftItemCellsData != null)
		{
			this.CraftItemCellsData.Clear();
		}
		else
		{
			this.CraftItemCellsData = new List<UICraftItemCellData>();
		}
		if (this.customItems == null)
		{
			foreach (NeedItemData needItemData in this.CraftDefinition.needItems)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, multiInventory, null, 0f, drawRunes, this.wgoData));
			}
			CraftDef craftDefinition = this.CraftDefinition;
			if (craftDefinition != null && craftDefinition.hasDurabilityUseItem)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(craftDefinition.durabilityUseItem, multiInventory, null, craftDefinition.needItemsDurabilityUse, false, this.wgoData));
				return;
			}
		}
		else
		{
			foreach (NeedItemData needItemData2 in this.customItems)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData2, multiInventory, null, 0f, drawRunes, this.wgoData));
			}
		}
	}

	// Token: 0x04002B93 RID: 11155
	private List<NeedItemData> customItems;

	// Token: 0x04002B94 RID: 11156
	private WgoData wgoData;
}
