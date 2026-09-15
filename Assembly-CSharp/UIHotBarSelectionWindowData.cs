using System;
using LazyBearTechnology;

// Token: 0x020009E7 RID: 2535
public class UIHotBarSelectionWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06004419 RID: 17433 RVA: 0x00143B22 File Offset: 0x00141D22
	// (set) Token: 0x0600441A RID: 17434 RVA: 0x00143B2A File Offset: 0x00141D2A
	public Item Item { get; private set; }

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x0600441B RID: 17435 RVA: 0x00143B33 File Offset: 0x00141D33
	// (set) Token: 0x0600441C RID: 17436 RVA: 0x00143B3B File Offset: 0x00141D3B
	public UIHotBarWidgetData HotBarWidgetData { get; private set; }

	// Token: 0x0600441D RID: 17437 RVA: 0x00143B44 File Offset: 0x00141D44
	public UIHotBarSelectionWindowData(GameSave gameSave, Item item)
	{
		this.Item = item;
		this.HotBarWidgetData = new UIHotBarWidgetData(gameSave, false, item);
	}
}
