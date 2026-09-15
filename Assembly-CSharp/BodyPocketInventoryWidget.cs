using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
public class BodyPocketInventoryWidget : InventoryWidgetBase<BodyPocketInventoryWidgetData>
{
	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x0011DA4E File Offset: 0x0011BC4E
	private BodyPocketInventoryWidgetData Data
	{
		get
		{
			return this.data as BodyPocketInventoryWidgetData;
		}
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x0011DA5B File Offset: 0x0011BC5B
	public override void Init()
	{
		base.Init();
		this.cells = base.GetComponentsInChildren<UIItemCell>(true).ToList<UIItemCell>();
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x0011DA78 File Offset: 0x0011BC78
	public override void Redraw()
	{
		base.Redraw();
		this.SubscribeToInventoryEvents();
		foreach (UIItemCell uiitemCell in this.cells)
		{
			uiitemCell.OnItemCellOver = this.onItemCellOver;
			uiitemCell.OnItemCellOut = this.onItemCellOut;
			if (!this.Data.FirstEmptyIsInteractable)
			{
				uiitemCell.OnItemCellPress = this.onItemCellPress;
				uiitemCell.OnItemCellPress2 = this.onItemCellPress2;
			}
		}
		int num = 0;
		if (this.Data.IsActive)
		{
			foreach (Item item in this.data.Inventory.Data.Inventory)
			{
				if (!BodyPocketInventoryWidget.ShouldSkipItem(item, this.Data.ZombieWgoData))
				{
					int num2 = Mathf.Min(item.Count, this.cells.Count - num);
					for (int i = 0; i < num2; i++)
					{
						Item item2 = ((item.Count == 1) ? item : Item.Copy(item));
						item2.Count = 1;
						this.cells[num].Draw(item2, false, -1, false, 1, false, 0, true, false, false, this.data.ItemRelatedWidgetState, false);
						this.cells[num].Background.sprite = this.activeFilledBackSprite;
						num++;
					}
					if (num >= this.cells.Count)
					{
						break;
					}
				}
			}
			for (int j = num; j < this.cells.Count; j++)
			{
				UIItemCell uiitemCell2 = this.cells[j];
				if (j == num && this.Data.FirstEmptyIsInteractable)
				{
					uiitemCell2.DrawCustom("i_slot-plus", 1, true, false);
					uiitemCell2.Background.sprite = this.activeFilledBackSprite;
					uiitemCell2.OnItemCellPress = this.onItemCellPress;
					uiitemCell2.OnItemCellPress2 = this.onItemCellPress2;
				}
				else
				{
					uiitemCell2.DrawEmpty(false, true, false);
					uiitemCell2.Background.sprite = this.activeEmptyCellBackSprite;
				}
			}
		}
		else
		{
			foreach (UIItemCell uiitemCell3 in this.cells)
			{
				uiitemCell3.DrawEmpty(false, true, false);
				uiitemCell3.Background.sprite = this.inactiveCellBackSprite;
			}
		}
		if (this.Data.IsActive)
		{
			this.activeStyle.ApplyStyle(this.headerLabel, false, null, null, null);
			return;
		}
		this.inactiveStyle.ApplyStyle(this.headerLabel, false, null, null, null);
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x0011DD88 File Offset: 0x0011BF88
	public static int GetOccupiedSlotCount(Item bodyItem, ZombieWgoData zombieWgoData)
	{
		if (bodyItem == null)
		{
			return 0;
		}
		int num = 0;
		foreach (Item item in bodyItem.Inventory)
		{
			if (!BodyPocketInventoryWidget.ShouldSkipItem(item, zombieWgoData))
			{
				num += item.Count;
			}
		}
		return num;
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x0011DDF0 File Offset: 0x0011BFF0
	private static bool ShouldSkipItem(Item item, ZombieWgoData zombieWgoData)
	{
		return item.IsEmpty || item.Definition.isMainOrgan || item.Definition.itemGroupIds.Contains("burial_reward") || (zombieWgoData != null && (zombieWgoData.equippedArmor == item.UniqueId || zombieWgoData.equippedHand == item.UniqueId || zombieWgoData.equippedCollar == item.UniqueId));
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x0011DE74 File Offset: 0x0011C074
	public override void UpdateItemRelatedWidgetStateForCells()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			UIItemCell uiitemCell = this.cells[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(uiitemCell.DisplayingItem);
			uiitemCell.SetWidgetState(flag ? this.data.ItemRelatedWidgetState : ItemRelatedWidgetState.Disabled);
		}
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0011DEE4 File Offset: 0x0011C0E4
	protected override void ClearCallbacks()
	{
		this.UnsubscribeFromInventoryEvents();
		base.ClearCallbacks();
		foreach (UIItemCell uiitemCell in this.cells)
		{
			uiitemCell.Flush(true);
		}
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002F1F RID: 12063
	public const int SlotCount = 6;

	// Token: 0x04002F20 RID: 12064
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x04002F21 RID: 12065
	[SerializeField]
	private TextStyle activeStyle;

	// Token: 0x04002F22 RID: 12066
	[SerializeField]
	private TextStyle inactiveStyle;

	// Token: 0x04002F23 RID: 12067
	[SerializeField]
	private Sprite activeFilledBackSprite;

	// Token: 0x04002F24 RID: 12068
	[SerializeField]
	private Sprite activeEmptyCellBackSprite;

	// Token: 0x04002F25 RID: 12069
	[SerializeField]
	private Sprite inactiveCellBackSprite;

	// Token: 0x04002F26 RID: 12070
	private List<UIItemCell> cells = new List<UIItemCell>();
}
