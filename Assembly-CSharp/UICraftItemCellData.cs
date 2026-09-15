using System;
using System.Collections.Generic;

// Token: 0x0200098B RID: 2443
public class UICraftItemCellData
{
	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x060040CB RID: 16587 RVA: 0x00135D7D File Offset: 0x00133F7D
	// (set) Token: 0x060040CC RID: 16588 RVA: 0x00135D85 File Offset: 0x00133F85
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x060040CD RID: 16589 RVA: 0x00135D8E File Offset: 0x00133F8E
	// (set) Token: 0x060040CE RID: 16590 RVA: 0x00135D96 File Offset: 0x00133F96
	public WgoData WgoData { get; private set; }

	// Token: 0x060040CF RID: 16591 RVA: 0x00135DA0 File Offset: 0x00133FA0
	public UICraftItemCellData(NeedItemData item, MultiInventory multiInventory, Action onItemChanged, float needDurability = 0f, string runesStr = "", WgoData wgoData = null)
	{
		this.runesStr = runesStr;
		this.needDurability = needDurability;
		this.MultiInventory = multiInventory;
		this.WgoData = wgoData;
		this.FillItemVariants(item);
		if (this.itemVariants.Count == 0)
		{
			this.itemVariants.Add(item);
		}
		this.currentItem = this.itemVariants[0];
		this.OnItemChanged = onItemChanged;
		this.initialItem = item;
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x00135E1D File Offset: 0x0013401D
	public UICraftItemCellData(NeedItemData item, MultiInventory multiInventory, Action onItemChanged, float needDurability, bool drawRunes, WgoData wgoData = null)
		: this(item, multiInventory, onItemChanged, needDurability, "", wgoData)
	{
		if (drawRunes)
		{
			this.runesStr = this.GetRunesFromCurrentItem();
		}
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x00135E44 File Offset: 0x00134044
	public void NextItem()
	{
		if (this.currentItemIndex == this.itemVariants.Count - 1)
		{
			this.currentItemIndex = 0;
		}
		else
		{
			this.currentItemIndex++;
		}
		this.currentItem = this.itemVariants[this.currentItemIndex];
		Action onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged();
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x00135EA4 File Offset: 0x001340A4
	public void PrevItem()
	{
		if (this.currentItemIndex == 0)
		{
			this.currentItemIndex = this.itemVariants.Count - 1;
		}
		else
		{
			this.currentItemIndex--;
		}
		this.currentItem = this.itemVariants[this.currentItemIndex];
		Action onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged();
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x00135F03 File Offset: 0x00134103
	private string GetRunesFromCurrentItem()
	{
		if (this.currentItem == null || this.currentItem.IsGroup || this.currentItem.ItemDef == null)
		{
			return string.Empty;
		}
		return this.currentItem.ItemDef.GetRunesAsString();
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x00135F40 File Offset: 0x00134140
	private void FillItemVariants(NeedItemData item)
	{
		if (item.IsGroup)
		{
			List<ItemDef> list;
			if (item.TryGetGroupItemDefs(out list) && list != null)
			{
				this.FormCraftItemCellDataForGroupItem(item, list);
			}
			return;
		}
		this.itemVariants.Add(item);
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x00135F78 File Offset: 0x00134178
	private void FormCraftItemCellDataForGroupItem(NeedItemData groupItem, List<ItemDef> groupItemDefs)
	{
		List<NeedItemData> list = new List<NeedItemData>();
		List<NeedItemData> list2 = new List<NeedItemData>();
		foreach (ItemDef itemDef in groupItemDefs)
		{
			NeedItemData needItemData = new NeedItemData(itemDef.id, groupItem.count);
			if (this.HasVariantInInventory(needItemData))
			{
				list.Add(needItemData);
			}
			else
			{
				list2.Add(needItemData);
			}
		}
		this.itemVariants.AddRange(list);
		this.itemVariants.AddRange(list2);
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x0013600C File Offset: 0x0013420C
	private bool HasVariantInInventory(NeedItemData variant)
	{
		return this.MultiInventory != null && this.MultiInventory.GetTotalCount(variant.Id) > 0;
	}

	// Token: 0x040032CC RID: 13004
	public NeedItemData currentItem;

	// Token: 0x040032CD RID: 13005
	public List<NeedItemData> itemVariants = new List<NeedItemData>();

	// Token: 0x040032CE RID: 13006
	public float needDurability;

	// Token: 0x040032CF RID: 13007
	public int currentItemIndex;

	// Token: 0x040032D0 RID: 13008
	public string runesStr;

	// Token: 0x040032D1 RID: 13009
	public NeedItemData initialItem;

	// Token: 0x040032D4 RID: 13012
	private Action OnItemChanged;
}
