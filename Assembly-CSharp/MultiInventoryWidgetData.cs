using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008B4 RID: 2228
public class MultiInventoryWidgetData : LazyWidgetDataBase
{
	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x060039EA RID: 14826 RVA: 0x001159EB File Offset: 0x00113BEB
	// (set) Token: 0x060039EB RID: 14827 RVA: 0x001159F3 File Offset: 0x00113BF3
	public Action<int> OnInventoryRemoved { get; set; }

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x060039EC RID: 14828 RVA: 0x001159FC File Offset: 0x00113BFC
	// (set) Token: 0x060039ED RID: 14829 RVA: 0x00115A04 File Offset: 0x00113C04
	public Action<InventoryWidgetDataBase> OnInventoryAdded { get; set; }

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x060039EE RID: 14830 RVA: 0x00115A0D File Offset: 0x00113C0D
	// (set) Token: 0x060039EF RID: 14831 RVA: 0x00115A15 File Offset: 0x00113C15
	public Action OnMoveAllSimilarPressed { get; set; }

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x060039F0 RID: 14832 RVA: 0x00115A1E File Offset: 0x00113C1E
	// (set) Token: 0x060039F1 RID: 14833 RVA: 0x00115A26 File Offset: 0x00113C26
	public Func<Inventory> GetMoveAllSimilarTargetInventory { get; set; }

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x060039F2 RID: 14834 RVA: 0x00115A2F File Offset: 0x00113C2F
	// (set) Token: 0x060039F3 RID: 14835 RVA: 0x00115A37 File Offset: 0x00113C37
	public Action OnMoveAllSimilarTargetChanged { get; set; }

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x060039F4 RID: 14836 RVA: 0x00115A40 File Offset: 0x00113C40
	// (set) Token: 0x060039F5 RID: 14837 RVA: 0x00115A48 File Offset: 0x00113C48
	public Action OnSelectedWidgetChanged { get; set; }

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x060039F6 RID: 14838 RVA: 0x00115A51 File Offset: 0x00113C51
	// (set) Token: 0x060039F7 RID: 14839 RVA: 0x00115A59 File Offset: 0x00113C59
	public string HeaderLocaleId { get; set; } = "ui_multiinventory";

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x060039F8 RID: 14840 RVA: 0x00115A62 File Offset: 0x00113C62
	// (set) Token: 0x060039F9 RID: 14841 RVA: 0x00115A6A File Offset: 0x00113C6A
	public MultiInventoryWidgetMode WidgetSelectionMode { get; set; }

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x060039FA RID: 14842 RVA: 0x00115A73 File Offset: 0x00113C73
	// (set) Token: 0x060039FB RID: 14843 RVA: 0x00115AA0 File Offset: 0x00113CA0
	public InventoryWidgetDataBase SelectedWidgetData
	{
		get
		{
			if (this.selectedWidgetData != null)
			{
				return this.selectedWidgetData;
			}
			if (this.inventoriesData.Count <= 0)
			{
				return null;
			}
			return this.inventoriesData[0];
		}
		set
		{
			if (this.selectedWidgetData == value)
			{
				return;
			}
			this.selectedWidgetData = value;
			Action onSelectedWidgetChanged = this.OnSelectedWidgetChanged;
			if (onSelectedWidgetChanged == null)
			{
				return;
			}
			onSelectedWidgetChanged();
		}
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x00115AC3 File Offset: 0x00113CC3
	public MultiInventoryWidgetData(MultiInventoryWidgetMode widgetSelectionMode = MultiInventoryWidgetMode.Default)
	{
		this.WidgetSelectionMode = widgetSelectionMode;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00115AE8 File Offset: 0x00113CE8
	public void Add(InventoryWidgetDataBase widgetData)
	{
		this.inventoriesData.Add(widgetData);
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00115AF6 File Offset: 0x00113CF6
	public void AddRange(List<InventoryWidgetDataBase> widgetsData)
	{
		this.inventoriesData.AddRange(widgetsData);
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x00115B04 File Offset: 0x00113D04
	public Inventory FindBagInventory(Item item)
	{
		for (int i = 0; i < this.inventoriesData.Count; i++)
		{
			BagInventoryWidgetData bagInventoryWidgetData = this.inventoriesData[i] as BagInventoryWidgetData;
			if (bagInventoryWidgetData != null && bagInventoryWidgetData.Inventory.Data == item)
			{
				return bagInventoryWidgetData.Inventory;
			}
		}
		return null;
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x00115B54 File Offset: 0x00113D54
	public void OnBagRemoved(Item bag)
	{
		int num = this.inventoriesData.FindIndex((InventoryWidgetDataBase b) => b.Inventory.Data.UniqueId == bag.UniqueId);
		if (num != -1)
		{
			Action<int> onInventoryRemoved = this.OnInventoryRemoved;
			if (onInventoryRemoved != null)
			{
				onInventoryRemoved(num);
			}
			this.inventoriesData.RemoveAt(num);
		}
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x00115BA8 File Offset: 0x00113DA8
	public void OnBagAdded(BagInventoryWidgetData bagInventoryWidgetData)
	{
		this.inventoriesData.Add(bagInventoryWidgetData);
		Action<InventoryWidgetDataBase> onInventoryAdded = this.OnInventoryAdded;
		if (onInventoryAdded == null)
		{
			return;
		}
		onInventoryAdded(bagInventoryWidgetData);
	}

	// Token: 0x04002DB0 RID: 11696
	public List<InventoryWidgetDataBase> inventoriesData = new List<InventoryWidgetDataBase>();

	// Token: 0x04002DB3 RID: 11699
	public Action onWidgetHide;

	// Token: 0x04002DBA RID: 11706
	private InventoryWidgetDataBase selectedWidgetData;
}
