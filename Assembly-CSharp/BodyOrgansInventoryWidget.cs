using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public class BodyOrgansInventoryWidget : InventoryWidgetBase<BodyOrgansInventoryWidgetData>
{
	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x06003BBF RID: 15295 RVA: 0x0011D596 File Offset: 0x0011B796
	private BodyOrgansInventoryWidgetData Data
	{
		get
		{
			return this.data as BodyOrgansInventoryWidgetData;
		}
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x0011D5A4 File Offset: 0x0011B7A4
	public override void Init()
	{
		base.Init();
		this.mainOrgansFixedTypeItemCells = base.GetComponentsInChildren<UIFixedTypeItemCell>(true).ToList<UIFixedTypeItemCell>();
		foreach (UIFixedTypeItemCell uifixedTypeItemCell in this.mainOrgansFixedTypeItemCells)
		{
			if (!this.mainOrgansItemCellsByType.TryAdd(uifixedTypeItemCell.ItemType, uifixedTypeItemCell) && uifixedTypeItemCell.ItemType != ItemType.None)
			{
				Debug.LogError(string.Format("AutopsyInventoryWidget: itemType {0} was already added", uifixedTypeItemCell.ItemType));
			}
		}
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0011D640 File Offset: 0x0011B840
	protected override void ClearCallbacks()
	{
		this.UnsubscribeFromInventoryEvents();
		base.ClearCallbacks();
		foreach (UIFixedTypeItemCell uifixedTypeItemCell in this.mainOrgansFixedTypeItemCells)
		{
			uifixedTypeItemCell.UIItemCell.ClearCallbacks();
		}
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0011D6A4 File Offset: 0x0011B8A4
	public override void UpdateItemRelatedWidgetStateForCells()
	{
		for (int i = 0; i < this.mainOrgansFixedTypeItemCells.Count; i++)
		{
			UIFixedTypeItemCell uifixedTypeItemCell = this.mainOrgansFixedTypeItemCells[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(uifixedTypeItemCell.UIItemCell.DisplayingItem);
			uifixedTypeItemCell.UIItemCell.SetWidgetState(flag ? this.data.ItemRelatedWidgetState : ItemRelatedWidgetState.Disabled);
		}
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0011D71C File Offset: 0x0011B91C
	public override void Redraw()
	{
		base.Redraw();
		this.SubscribeToInventoryEvents();
		if (this.Data.IsActive)
		{
			for (int i = 0; i < LazyConsts.MAIN_ORGANS_TYPES.Count; i++)
			{
				ItemType itemType = LazyConsts.MAIN_ORGANS_TYPES[i];
				UIFixedTypeItemCell uifixedTypeItemCell;
				if (this.mainOrgansItemCellsByType.TryGetValue(itemType, out uifixedTypeItemCell))
				{
					if (this.data.Inventory.Data.HasItemsByItemType(itemType))
					{
						Item itemByType = this.data.Inventory.Data.GetItemByType(itemType);
						uifixedTypeItemCell.Draw(itemByType, this.data.ItemRelatedWidgetState, true);
						uifixedTypeItemCell.UpdateWidgetBackgroundSprite(this.activeFilledBackSprite);
					}
					else
					{
						uifixedTypeItemCell.DrawEmpty();
						uifixedTypeItemCell.UpdateWidgetBackgroundSprite(this.activeEmptyCellBackSprite);
					}
					uifixedTypeItemCell.UIItemCell.OnItemCellOver = this.onItemCellOver;
					uifixedTypeItemCell.UIItemCell.OnItemCellOut = this.onItemCellOut;
					uifixedTypeItemCell.UIItemCell.OnItemCellPress = this.onItemCellPress;
					uifixedTypeItemCell.UIItemCell.OnItemCellPress2 = this.onItemCellPress2;
				}
			}
		}
		else
		{
			foreach (UIFixedTypeItemCell uifixedTypeItemCell2 in this.mainOrgansFixedTypeItemCells)
			{
				uifixedTypeItemCell2.DrawEmpty();
				uifixedTypeItemCell2.UpdateWidgetBackgroundSprite(this.inactiveCellBackSprite);
			}
		}
		if (this.Data.IsActive)
		{
			this.activeStyle.ApplyStyle(this.headerLabel, false, null, null, null);
			return;
		}
		this.inactiveStyle.ApplyStyle(this.headerLabel, false, null, null, null);
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0011D8E8 File Offset: 0x0011BAE8
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new BodyOrgansInventoryWidgetData());
	}

	// Token: 0x04002F14 RID: 12052
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x04002F15 RID: 12053
	[SerializeField]
	private TextStyle activeStyle;

	// Token: 0x04002F16 RID: 12054
	[SerializeField]
	private TextStyle inactiveStyle;

	// Token: 0x04002F17 RID: 12055
	[SerializeField]
	private Sprite activeFilledBackSprite;

	// Token: 0x04002F18 RID: 12056
	[SerializeField]
	private Sprite activeEmptyCellBackSprite;

	// Token: 0x04002F19 RID: 12057
	[SerializeField]
	private Sprite inactiveCellBackSprite;

	// Token: 0x04002F1A RID: 12058
	private List<UIFixedTypeItemCell> mainOrgansFixedTypeItemCells;

	// Token: 0x04002F1B RID: 12059
	private Dictionary<ItemType, UIFixedTypeItemCell> mainOrgansItemCellsByType = new Dictionary<ItemType, UIFixedTypeItemCell>();
}
