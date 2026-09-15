using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200084E RID: 2126
public class UITooltipCraftItemCell : MonoBehaviour
{
	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x0600367C RID: 13948 RVA: 0x00108480 File Offset: 0x00106680
	public LazyButton NextItemButton
	{
		get
		{
			return this.nextItemButton;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x0600367D RID: 13949 RVA: 0x00108488 File Offset: 0x00106688
	public LazyButton PrevItemButton
	{
		get
		{
			return this.prevItemButton;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x0600367E RID: 13950 RVA: 0x00108490 File Offset: 0x00106690
	public UIItemCell ItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x00108498 File Offset: 0x00106698
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			this.TryInitGamepadNavigationItem();
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x001084A6 File Offset: 0x001066A6
	private void Awake()
	{
		this.nextItemButton.onClick.AddListener(new UnityAction(this.OnNextItem));
		this.prevItemButton.onClick.AddListener(new UnityAction(this.OnPrevItem));
		this.TryInitGamepadNavigationItem();
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x001084E8 File Offset: 0x001066E8
	public void Draw(UICraftItemCellData craftItemCellData, Action onVariableItemChanged, bool isTooltipView = false, bool drawAsNeedItem = true)
	{
		this.data = craftItemCellData;
		this.drawAsNeedItem = drawAsNeedItem;
		this.onVariableItemChanged = onVariableItemChanged;
		this.SubscribeToInventoryChanges();
		if (craftItemCellData.itemVariants.Count > 1 && !isTooltipView)
		{
			this.nextItemButton.gameObject.SetActive(true);
			this.prevItemButton.gameObject.SetActive(true);
		}
		else
		{
			this.nextItemButton.gameObject.SetActive(false);
			this.prevItemButton.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
		this.runesLabel.text = this.data.runesStr;
		this.runesLabel.gameObject.SetActive(!string.IsNullOrEmpty(this.data.runesStr));
		this.RedrawItem(null);
		this.isBig = this.data.currentItem.ItemDef != null && this.data.currentItem.ItemDef.itemSize == ItemSize.Big;
		this.UpdateSize(this.isBig);
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x001085F4 File Offset: 0x001067F4
	public void Flush()
	{
		this.currentMultiplier = 1;
		this.drawAsNeedItem = true;
		this.onVariableItemChanged = null;
		this.nextItemButton.gameObject.SetActive(false);
		this.prevItemButton.gameObject.SetActive(false);
		this.UnsubscribeFromInventoryChanges();
		if (this.isBig)
		{
			this.UpdateSize(false);
		}
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x00108650 File Offset: 0x00106850
	private void UpdateSize(bool big)
	{
		if (big)
		{
			this.layoutElement.minWidth = this.bigLayoutSize.x;
			this.layoutElement.minHeight = this.bigLayoutSize.y;
			return;
		}
		this.layoutElement.minWidth = this.defaultLayoutSize.x;
		this.layoutElement.minHeight = this.defaultLayoutSize.y;
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x001086BC File Offset: 0x001068BC
	private void RedrawItem(List<Item> items = null)
	{
		if (!this.drawAsNeedItem)
		{
			this.itemCell.Draw(new Item(this.data.currentItem.Id, this.data.currentItem.GetCount(this.data.WgoData)), false, -1, false, this.currentMultiplier, false, 0, true, false, true, ItemRelatedWidgetState.NotSet, false);
			this.itemCell.UpdateCountLabelAsRegularItem(this.currentMultiplier);
		}
		else if (this.data.needDurability > 0f)
		{
			this.itemCell.Draw(new Item(this.data.currentItem.Id, this.data.currentItem.GetCount(this.data.WgoData)), true, this.data.MultiInventory.HasItemWithEnoughDurability(this.data.currentItem.Id, this.data.needDurability) ? 0 : 1, false, this.currentMultiplier, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		}
		else
		{
			this.itemCell.Draw(new Item(this.data.currentItem.Id, this.data.currentItem.GetCount(this.data.WgoData)), true, this.data.MultiInventory.GetTotalCount(this.data.currentItem.Id), false, this.currentMultiplier, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		}
		if (this.data.itemVariants.Count > 1)
		{
			this.itemCell.StarIcon.gameObject.SetActive(false);
		}
		this.itemCell.SetNativeSizeForIcon();
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x0010885F File Offset: 0x00106A5F
	private void OnNextItem()
	{
		this.data.NextItem();
		this.RedrawItem(null);
		Action action = this.onVariableItemChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x00108883 File Offset: 0x00106A83
	private void OnPrevItem()
	{
		this.data.PrevItem();
		this.RedrawItem(null);
		Action action = this.onVariableItemChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003687 RID: 13959 RVA: 0x001088A8 File Offset: 0x00106AA8
	private void SubscribeToInventoryChanges()
	{
		if (!this.subscribedToInventoryChanges)
		{
			MainGame.PlayerData.inventory.OnItemsAdd += this.RedrawItem;
			MainGame.PlayerData.inventory.OnItemsRemove += this.RedrawItem;
			this.subscribedToInventoryChanges = true;
		}
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x001088FC File Offset: 0x00106AFC
	private void UnsubscribeFromInventoryChanges()
	{
		if (this.subscribedToInventoryChanges)
		{
			MainGame.PlayerData.inventory.OnItemsAdd -= this.RedrawItem;
			MainGame.PlayerData.inventory.OnItemsRemove -= this.RedrawItem;
			this.subscribedToInventoryChanges = false;
		}
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x00108950 File Offset: 0x00106B50
	private void TryInitGamepadNavigationItem()
	{
		if (this.gamepadNavigationItem == null)
		{
			this.gamepadNavigationItem = base.GetComponent<GamepadNavigationItem>();
			if (this.gamepadNavigationItem != null)
			{
				this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.itemCell.OnGamepadOver), new UnityAction(this.itemCell.OnGamepadOut), new UnityAction(this.itemCell.OnGamepadPress));
			}
		}
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x001089C3 File Offset: 0x00106BC3
	public void SetMultiplierValue(int multiplier)
	{
		this.currentMultiplier = multiplier;
		this.itemCell.OnMultiplierChange(this.currentMultiplier, false);
	}

	// Token: 0x04002B7D RID: 11133
	private Action onVariableItemChanged;

	// Token: 0x04002B7E RID: 11134
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002B7F RID: 11135
	[SerializeField]
	private LazyButton nextItemButton;

	// Token: 0x04002B80 RID: 11136
	[SerializeField]
	private LazyButton prevItemButton;

	// Token: 0x04002B81 RID: 11137
	[SerializeField]
	private TextMeshProUGUI runesLabel;

	// Token: 0x04002B82 RID: 11138
	[SerializeField]
	private Vector2 defaultLayoutSize;

	// Token: 0x04002B83 RID: 11139
	[SerializeField]
	private Vector2 bigLayoutSize;

	// Token: 0x04002B84 RID: 11140
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x04002B85 RID: 11141
	private bool subscribedToInventoryChanges;

	// Token: 0x04002B86 RID: 11142
	private int currentMultiplier = 1;

	// Token: 0x04002B87 RID: 11143
	private UICraftItemCellData data;

	// Token: 0x04002B88 RID: 11144
	private bool isBig;

	// Token: 0x04002B89 RID: 11145
	private bool drawAsNeedItem = true;

	// Token: 0x04002B8A RID: 11146
	private GamepadNavigationItem gamepadNavigationItem;
}
