using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008F9 RID: 2297
public class UIGroupsItemCell : MonoBehaviour
{
	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x06003C12 RID: 15378 RVA: 0x0011EFA6 File Offset: 0x0011D1A6
	public List<string> ItemGroups
	{
		get
		{
			return this.itemGroups;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06003C13 RID: 15379 RVA: 0x0011EFAE File Offset: 0x0011D1AE
	public UIItemCell UIItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x0011EFB8 File Offset: 0x0011D1B8
	public void Draw(Item item, ItemRelatedWidgetState customState = ItemRelatedWidgetState.NotSet)
	{
		this.itemCell.Draw(item, false, -1, false, 1, false, 0, true, false, false, customState, false);
		this.emptySlotImage.gameObject.SetActive(false);
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0011EFED File Offset: 0x0011D1ED
	public void DrawEmpty()
	{
		this.itemCell.DrawEmpty(false, true, false);
		this.emptySlotImage.gameObject.SetActive(true);
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0011F00E File Offset: 0x0011D20E
	public void DrawEmptyInteractable()
	{
		this.itemCell.DrawEmptyInteractable(false, false);
		this.emptySlotImage.gameObject.SetActive(true);
	}

	// Token: 0x04002F48 RID: 12104
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002F49 RID: 12105
	[SerializeField]
	private List<string> itemGroups;

	// Token: 0x04002F4A RID: 12106
	[SerializeField]
	private Image emptySlotImage;
}
