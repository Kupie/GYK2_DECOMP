using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008E3 RID: 2275
public class UIBoostElementData : LazyWidgetDataBase
{
	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x06003B67 RID: 15207 RVA: 0x0011C533 File Offset: 0x0011A733
	// (set) Token: 0x06003B68 RID: 15208 RVA: 0x0011C53B File Offset: 0x0011A73B
	public CraftElement CraftElement { get; private set; }

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x06003B69 RID: 15209 RVA: 0x0011C544 File Offset: 0x0011A744
	// (set) Token: 0x06003B6A RID: 15210 RVA: 0x0011C54C File Offset: 0x0011A74C
	public Action<List<NeedItemData>> OnPress { get; private set; }

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x06003B6B RID: 15211 RVA: 0x0011C555 File Offset: 0x0011A755
	// (set) Token: 0x06003B6C RID: 15212 RVA: 0x0011C55D File Offset: 0x0011A75D
	public Func<List<NeedItemData>, bool> CanCraft { get; private set; }

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x06003B6D RID: 15213 RVA: 0x0011C566 File Offset: 0x0011A766
	// (set) Token: 0x06003B6E RID: 15214 RVA: 0x0011C56E File Offset: 0x0011A76E
	public Action OnOver { get; private set; }

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x06003B6F RID: 15215 RVA: 0x0011C577 File Offset: 0x0011A777
	// (set) Token: 0x06003B70 RID: 15216 RVA: 0x0011C57F File Offset: 0x0011A77F
	public Action OnOut { get; private set; }

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x06003B71 RID: 15217 RVA: 0x0011C588 File Offset: 0x0011A788
	// (set) Token: 0x06003B72 RID: 15218 RVA: 0x0011C590 File Offset: 0x0011A790
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06003B73 RID: 15219 RVA: 0x0011C599 File Offset: 0x0011A799
	// (set) Token: 0x06003B74 RID: 15220 RVA: 0x0011C5A1 File Offset: 0x0011A7A1
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06003B75 RID: 15221 RVA: 0x0011C5AA File Offset: 0x0011A7AA
	// (set) Token: 0x06003B76 RID: 15222 RVA: 0x0011C5B2 File Offset: 0x0011A7B2
	public string Name { get; private set; }

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06003B77 RID: 15223 RVA: 0x0011C5BB File Offset: 0x0011A7BB
	// (set) Token: 0x06003B78 RID: 15224 RVA: 0x0011C5C3 File Offset: 0x0011A7C3
	public string Description { get; private set; }

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06003B79 RID: 15225 RVA: 0x0011C5CC File Offset: 0x0011A7CC
	// (set) Token: 0x06003B7A RID: 15226 RVA: 0x0011C5D4 File Offset: 0x0011A7D4
	public WorldZoneData WorldZoneData { get; private set; }

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06003B7B RID: 15227 RVA: 0x0011C5DD File Offset: 0x0011A7DD
	// (set) Token: 0x06003B7C RID: 15228 RVA: 0x0011C5E5 File Offset: 0x0011A7E5
	public WgoData WgoData { get; private set; }

	// Token: 0x06003B7D RID: 15229 RVA: 0x0011C5F0 File Offset: 0x0011A7F0
	public UIBoostElementData(CraftElement craftElement, MultiInventory multiInventory, Action<CraftElement, List<NeedItemData>> onPress, Func<CraftElement, List<NeedItemData>, bool> canCraft, Action onOver, Action onOut, WorldZoneData worldZoneData, WgoData wgoData = null)
	{
		UIBoostElementData.<>c__DisplayClass44_0 CS$<>8__locals1 = new UIBoostElementData.<>c__DisplayClass44_0();
		CS$<>8__locals1.onPress = onPress;
		CS$<>8__locals1.canCraft = canCraft;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		this.CraftElement = craftElement;
		this.MultiInventory = multiInventory;
		this.WorldZoneData = worldZoneData;
		this.WgoData = wgoData;
		this.OnPress = new Action<List<NeedItemData>>(CS$<>8__locals1.<.ctor>g__OnPressAction|0);
		this.CanCraft = new Func<List<NeedItemData>, bool>(CS$<>8__locals1.<.ctor>g__CanCraftFunc|1);
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.FillCraftItemCellsData();
		this.Name = craftElement.Def.id;
		this.Description = craftElement.Definition.GetBoostRunesAsString();
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0011C69C File Offset: 0x0011A89C
	public List<NeedItemData> GetCurrentNeedItems()
	{
		List<NeedItemData> list = new List<NeedItemData>();
		for (int i = 0; i < this.CraftItemCellsData.Count; i++)
		{
			list.Add(this.CraftItemCellsData[i].currentItem);
		}
		return list;
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x0011C6E0 File Offset: 0x0011A8E0
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
		if (this.CraftElement.Def.needItems != null)
		{
			foreach (NeedItemData needItemData in this.CraftElement.Def.needItems)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, this.MultiInventory, null, 0f, "", this.WgoData));
			}
		}
	}
}
