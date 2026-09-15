using System;
using LazyBearTechnology;

// Token: 0x020008A6 RID: 2214
public class InventoryHeaderWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x0600393A RID: 14650 RVA: 0x001133C9 File Offset: 0x001115C9
	// (set) Token: 0x0600393B RID: 14651 RVA: 0x001133D1 File Offset: 0x001115D1
	public bool IsActiveViewState { get; set; }

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x0600393C RID: 14652 RVA: 0x001133DA File Offset: 0x001115DA
	// (set) Token: 0x0600393D RID: 14653 RVA: 0x001133E2 File Offset: 0x001115E2
	public bool DrawHeader { get; set; } = true;

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x0600393E RID: 14654 RVA: 0x001133EB File Offset: 0x001115EB
	// (set) Token: 0x0600393F RID: 14655 RVA: 0x001133F3 File Offset: 0x001115F3
	public Inventory Inventory { get; set; }

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06003940 RID: 14656 RVA: 0x001133FC File Offset: 0x001115FC
	// (set) Token: 0x06003941 RID: 14657 RVA: 0x00113404 File Offset: 0x00111604
	public string HeaderIconId { get; set; }

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06003942 RID: 14658 RVA: 0x0011340D File Offset: 0x0011160D
	// (set) Token: 0x06003943 RID: 14659 RVA: 0x00113415 File Offset: 0x00111615
	public string CustomHeaderId { get; set; }

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06003944 RID: 14660 RVA: 0x0011341E File Offset: 0x0011161E
	// (set) Token: 0x06003945 RID: 14661 RVA: 0x00113426 File Offset: 0x00111626
	public Action OnMoveAllSimilarItemFromPlayerToChest { get; set; }

	// Token: 0x06003946 RID: 14662 RVA: 0x0011342F File Offset: 0x0011162F
	public InventoryHeaderWidgetData()
	{
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x0011343E File Offset: 0x0011163E
	public InventoryHeaderWidgetData(Inventory inventory, string headerIconId = "comm-header_2-type_icon-main_inventory", string customHeaderId = "", bool isActiveViewState = true, Action onMoveAllSimilarItemFromPlayerToChest = null)
	{
		this.IsActiveViewState = isActiveViewState;
		this.Inventory = inventory;
		this.HeaderIconId = headerIconId;
		this.CustomHeaderId = customHeaderId;
		this.OnMoveAllSimilarItemFromPlayerToChest = onMoveAllSimilarItemFromPlayerToChest;
	}
}
