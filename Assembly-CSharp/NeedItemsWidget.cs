using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
public class NeedItemsWidget : LazyWidget<NeedItemsWidgetData>
{
	// Token: 0x06003A0B RID: 14859 RVA: 0x00116042 File Offset: 0x00114242
	public override void Redraw()
	{
		base.Redraw();
		this.DrawNeeds();
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x00116050 File Offset: 0x00114250
	private void DrawNeeds()
	{
		if (this.data.NeedItems == null)
		{
			return;
		}
		for (int i = 0; i < this.data.NeedItems.Count; i++)
		{
			NeedItemData needItemData = this.data.NeedItems[i];
			UIItemCell uiitemCell = this.itemCells[i];
			uiitemCell.Draw(new Item(needItemData.Id, needItemData.GetCount(this.data.WgoData)), true, this.data.MultiInventory.GetTotalCount(needItemData.Id), false, 1, !this.data.IsActive, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			if (this.data.IsActive)
			{
				uiitemCell.Background.sprite = this.activeFilledBackSprite;
			}
			else
			{
				uiitemCell.Background.sprite = this.inactiveCellBackSprite;
			}
			uiitemCell.gameObject.SetActive(true);
		}
		for (int j = this.data.NeedItems.Count; j < 3; j++)
		{
			this.itemCells[j].gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002DC6 RID: 11718
	[SerializeField]
	private List<UIItemCell> itemCells = new List<UIItemCell>();

	// Token: 0x04002DC7 RID: 11719
	[SerializeField]
	private Sprite activeFilledBackSprite;

	// Token: 0x04002DC8 RID: 11720
	[SerializeField]
	private Sprite activeEmptyCellBackSprite;

	// Token: 0x04002DC9 RID: 11721
	[SerializeField]
	private Sprite inactiveCellBackSprite;
}
