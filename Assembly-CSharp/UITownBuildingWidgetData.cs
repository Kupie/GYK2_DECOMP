using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A6B RID: 2667
public class UITownBuildingWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06004857 RID: 18519 RVA: 0x00156F6A File Offset: 0x0015516A
	// (set) Token: 0x06004858 RID: 18520 RVA: 0x00156F72 File Offset: 0x00155172
	public TownBuildingDef TownBuildingDef { get; private set; }

	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06004859 RID: 18521 RVA: 0x00156F7B File Offset: 0x0015517B
	// (set) Token: 0x0600485A RID: 18522 RVA: 0x00156F83 File Offset: 0x00155183
	public Action<List<NeedItemData>> OnPress { get; private set; }

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x0600485B RID: 18523 RVA: 0x00156F8C File Offset: 0x0015518C
	// (set) Token: 0x0600485C RID: 18524 RVA: 0x00156F94 File Offset: 0x00155194
	public Action OnOver { get; private set; }

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x0600485D RID: 18525 RVA: 0x00156F9D File Offset: 0x0015519D
	// (set) Token: 0x0600485E RID: 18526 RVA: 0x00156FA5 File Offset: 0x001551A5
	public Action OnOut { get; private set; }

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x0600485F RID: 18527 RVA: 0x00156FAE File Offset: 0x001551AE
	// (set) Token: 0x06004860 RID: 18528 RVA: 0x00156FB6 File Offset: 0x001551B6
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x06004861 RID: 18529 RVA: 0x00156FBF File Offset: 0x001551BF
	// (set) Token: 0x06004862 RID: 18530 RVA: 0x00156FC7 File Offset: 0x001551C7
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x06004863 RID: 18531 RVA: 0x00156FD0 File Offset: 0x001551D0
	// (set) Token: 0x06004864 RID: 18532 RVA: 0x00156FD8 File Offset: 0x001551D8
	public string Name { get; private set; }

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x06004865 RID: 18533 RVA: 0x00156FE1 File Offset: 0x001551E1
	// (set) Token: 0x06004866 RID: 18534 RVA: 0x00156FE9 File Offset: 0x001551E9
	public string Description { get; private set; }

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x06004867 RID: 18535 RVA: 0x00156FF2 File Offset: 0x001551F2
	// (set) Token: 0x06004868 RID: 18536 RVA: 0x00156FFA File Offset: 0x001551FA
	public WorldZoneData WorldZoneData { get; private set; }

	// Token: 0x06004869 RID: 18537 RVA: 0x00157004 File Offset: 0x00155204
	public UITownBuildingWidgetData(TownBuildingDef townBuildingDef, MultiInventory multiInventory, Action<TownBuildingDef, List<NeedItemData>> onPress, Action onOver, Action onOut, WorldZoneData worldZoneData)
	{
		UITownBuildingWidgetData.<>c__DisplayClass36_0 CS$<>8__locals1 = new UITownBuildingWidgetData.<>c__DisplayClass36_0();
		CS$<>8__locals1.onPress = onPress;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		this.TownBuildingDef = townBuildingDef;
		this.MultiInventory = multiInventory;
		this.WorldZoneData = worldZoneData;
		this.OnPress = new Action<List<NeedItemData>>(CS$<>8__locals1.<.ctor>g__OnPressAction|0);
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.FillCraftItemCellsData();
		this.Name = LLBase.L(this.TownBuildingDef.id);
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x00157080 File Offset: 0x00155280
	public List<NeedItemData> GetCurrentNeedItems()
	{
		List<NeedItemData> list = new List<NeedItemData>();
		for (int i = 0; i < this.CraftItemCellsData.Count; i++)
		{
			list.Add(this.CraftItemCellsData[i].currentItem);
		}
		return list;
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x001570C4 File Offset: 0x001552C4
	private void FillCraftItemCellsData()
	{
		if (this.CraftItemCellsData != null)
		{
			this.CraftItemCellsData.Clear();
		}
		else
		{
			this.CraftItemCellsData = new List<UICraftItemCellData>();
		}
		foreach (NeedItemData needItemData in this.TownBuildingDef.needItems)
		{
			this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, this.MultiInventory, null, 0f, "", null));
		}
	}
}
