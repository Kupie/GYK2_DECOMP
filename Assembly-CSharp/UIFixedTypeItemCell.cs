using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008BC RID: 2236
public class UIFixedTypeItemCell : MonoBehaviour
{
	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06003A1C RID: 14876 RVA: 0x0011623C File Offset: 0x0011443C
	public UIItemCell UIItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00116244 File Offset: 0x00114444
	public ItemType ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06003A1E RID: 14878 RVA: 0x0011624C File Offset: 0x0011444C
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			return this.itemCell.GamepadNavigationItem;
		}
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0011625C File Offset: 0x0011445C
	public void Draw(Item item, ItemRelatedWidgetState customState = ItemRelatedWidgetState.NotSet, bool drawAsInteractable = true)
	{
		this.itemCell.Draw(item, false, -1, false, 1, !drawAsInteractable, 0, true, false, false, customState, false);
		this.isEmpty = false;
		this.emptySlotImage.gameObject.SetActive(false);
		this.TryUpdateUnknownOrgan();
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x001162A3 File Offset: 0x001144A3
	public void DrawEmpty()
	{
		this.itemCell.DrawEmpty(false, true, false);
		this.isEmpty = true;
		this.emptySlotImage.gameObject.SetActive(true);
		this.TryUpdateUnknownOrgan();
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x001162D1 File Offset: 0x001144D1
	public void DrawEmptyInteractable()
	{
		this.itemCell.DrawEmptyInteractable(false, false);
		this.isEmpty = true;
		this.emptySlotImage.gameObject.SetActive(true);
		this.TryUpdateUnknownOrgan();
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x00116300 File Offset: 0x00114500
	public void TryUpdateUnknownOrgan()
	{
		if (!LazyConsts.MAIN_ORGANS_TYPES.Contains(this.ItemType))
		{
			this.unknownImage.gameObject.SetActive(false);
			this.itemCell.LazyButton.interactable = true;
			return;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.unlockedOrgans.Contains(this.ItemType))
		{
			this.unknownImage.gameObject.SetActive(false);
			this.itemCell.LazyButton.interactable = true;
			return;
		}
		this.unknownImage.gameObject.SetActive(true);
		this.unknownImage.color = (this.isEmpty ? new Color(1f, 1f, 1f, 0.5f) : Color.white);
		this.itemCell.LazyButton.interactable = false;
		this.emptySlotImage.gameObject.SetActive(false);
		this.itemCell.DrawEmpty(true, true, false);
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x001163FD File Offset: 0x001145FD
	public void UpdateWidgetBackgroundSprite(Sprite sprite)
	{
		this.background.sprite = sprite;
	}

	// Token: 0x04002DD0 RID: 11728
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002DD1 RID: 11729
	[SerializeField]
	private ItemType itemType;

	// Token: 0x04002DD2 RID: 11730
	[SerializeField]
	private Image emptySlotImage;

	// Token: 0x04002DD3 RID: 11731
	[SerializeField]
	private Image unknownImage;

	// Token: 0x04002DD4 RID: 11732
	[SerializeField]
	private Image background;

	// Token: 0x04002DD5 RID: 11733
	private bool isEmpty;
}
