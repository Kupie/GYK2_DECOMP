using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008BD RID: 2237
public class UIInsertItemCell : MonoBehaviour
{
	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06003A25 RID: 14885 RVA: 0x0011640B File Offset: 0x0011460B
	public UIItemCell UIItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x00116414 File Offset: 0x00114614
	public void Draw(Item item, bool hasRequiredTool, ItemType requiredTool)
	{
		this.itemCell.Draw(item, false, -1, false, 1, false, 0, true, false, false, hasRequiredTool ? ItemRelatedWidgetState.NotSet : ItemRelatedWidgetState.Disabled, false);
		this.emptySlotImage.gameObject.SetActive(false);
		this.requiredToolImage.gameObject.SetActive(false);
		this.backgroundImage.sprite = this.backgroundFilled;
		this.backgroundImage.color = Color.white;
		this.UpdateRequiredToolIcon(hasRequiredTool, requiredTool);
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x0011648C File Offset: 0x0011468C
	public void DrawEmpty(bool hasRequiredTool, ItemType requiredTool)
	{
		if (hasRequiredTool)
		{
			this.itemCell.DrawEmptyInteractable(false, false);
		}
		else
		{
			this.itemCell.DrawEmpty(true, true, false);
		}
		this.itemCell.SetWidgetState(hasRequiredTool ? ItemRelatedWidgetState.NotSet : ItemRelatedWidgetState.Disabled);
		this.emptySlotImage.gameObject.SetActive(true);
		this.backgroundImage.sprite = this.backgroundEmpty;
		this.UpdateRequiredToolIcon(hasRequiredTool, requiredTool);
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x001164F5 File Offset: 0x001146F5
	private void UpdateRequiredToolIcon(bool hasRequiredTool, ItemType requiredTool)
	{
		if (hasRequiredTool)
		{
			this.requiredToolImage.gameObject.SetActive(false);
			return;
		}
		this.requiredToolImage.gameObject.SetActive(true);
		this.requiredToolImage.sprite = CraftStatusIconHelper.GetCraftStatusIcon(CraftStatus.DoesntHaveRequiredTool, requiredTool, "");
	}

	// Token: 0x04002DD6 RID: 11734
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002DD7 RID: 11735
	[SerializeField]
	private Image emptySlotImage;

	// Token: 0x04002DD8 RID: 11736
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x04002DD9 RID: 11737
	[SerializeField]
	private Sprite backgroundEmpty;

	// Token: 0x04002DDA RID: 11738
	[SerializeField]
	private Sprite backgroundFilled;

	// Token: 0x04002DDB RID: 11739
	[SerializeField]
	private Image requiredToolImage;
}
