using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008BA RID: 2234
public class NeedItemsWidgetData : LazyWidgetDataBase
{
	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06003A0F RID: 14863 RVA: 0x0011617C File Offset: 0x0011437C
	// (set) Token: 0x06003A10 RID: 14864 RVA: 0x00116184 File Offset: 0x00114384
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06003A11 RID: 14865 RVA: 0x0011618D File Offset: 0x0011438D
	// (set) Token: 0x06003A12 RID: 14866 RVA: 0x00116195 File Offset: 0x00114395
	public List<NeedItemData> NeedItems { get; private set; }

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06003A13 RID: 14867 RVA: 0x0011619E File Offset: 0x0011439E
	// (set) Token: 0x06003A14 RID: 14868 RVA: 0x001161A6 File Offset: 0x001143A6
	public bool IsActive { get; private set; }

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06003A15 RID: 14869 RVA: 0x001161AF File Offset: 0x001143AF
	// (set) Token: 0x06003A16 RID: 14870 RVA: 0x001161B7 File Offset: 0x001143B7
	public WgoData WgoData { get; private set; }

	// Token: 0x06003A17 RID: 14871 RVA: 0x001161C0 File Offset: 0x001143C0
	public NeedItemsWidgetData(List<NeedItemData> needItems, MultiInventory multiInventory, bool isActive, WgoData wgoData = null)
	{
		this.NeedItems = needItems;
		this.MultiInventory = multiInventory;
		this.IsActive = isActive;
		this.WgoData = wgoData;
	}
}
