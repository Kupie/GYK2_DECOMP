using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000996 RID: 2454
public class UICraftsTabWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06004168 RID: 16744 RVA: 0x001379D4 File Offset: 0x00135BD4
	// (set) Token: 0x06004169 RID: 16745 RVA: 0x001379DC File Offset: 0x00135BDC
	public string TabId { get; private set; }

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x0600416A RID: 16746 RVA: 0x001379E5 File Offset: 0x00135BE5
	// (set) Token: 0x0600416B RID: 16747 RVA: 0x001379ED File Offset: 0x00135BED
	public List<CraftDef> Crafts { get; private set; }

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x0600416C RID: 16748 RVA: 0x001379F6 File Offset: 0x00135BF6
	// (set) Token: 0x0600416D RID: 16749 RVA: 0x001379FE File Offset: 0x00135BFE
	public WgoData WgoData { get; private set; }

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x0600416E RID: 16750 RVA: 0x00137A07 File Offset: 0x00135C07
	// (set) Token: 0x0600416F RID: 16751 RVA: 0x00137A0F File Offset: 0x00135C0F
	public Action<CraftDef, List<NeedItemData>, CraftParamsData, int> OnCraftToQueueAdded { get; private set; }

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x06004170 RID: 16752 RVA: 0x00137A18 File Offset: 0x00135C18
	// (set) Token: 0x06004171 RID: 16753 RVA: 0x00137A20 File Offset: 0x00135C20
	public Action<CraftDef, List<NeedItemData>, CraftParamsData, int> OnCraftStarted { get; private set; }

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06004172 RID: 16754 RVA: 0x00137A29 File Offset: 0x00135C29
	// (set) Token: 0x06004173 RID: 16755 RVA: 0x00137A31 File Offset: 0x00135C31
	public bool IsGravePartRemove { get; private set; }

	// Token: 0x06004174 RID: 16756 RVA: 0x00137A3A File Offset: 0x00135C3A
	public UICraftsTabWidgetData(WgoData wgoData, List<CraftDef> crafts, string tabId, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onCraftToQueueAdded, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onCraftStarted, bool isGravePartRemove)
	{
		this.WgoData = wgoData;
		this.OnCraftToQueueAdded = onCraftToQueueAdded;
		this.OnCraftStarted = onCraftStarted;
		this.TabId = tabId;
		this.Crafts = crafts;
		this.IsGravePartRemove = isGravePartRemove;
	}
}
