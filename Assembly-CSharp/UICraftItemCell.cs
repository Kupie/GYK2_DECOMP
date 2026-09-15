using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200098A RID: 2442
public class UICraftItemCell : MonoBehaviour
{
	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x060040BB RID: 16571 RVA: 0x00135883 File Offset: 0x00133A83
	public LazyButton NextItemButton
	{
		get
		{
			return this.nextItemButton;
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x060040BC RID: 16572 RVA: 0x0013588B File Offset: 0x00133A8B
	public LazyButton PrevItemButton
	{
		get
		{
			return this.prevItemButton;
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x060040BD RID: 16573 RVA: 0x00135893 File Offset: 0x00133A93
	public UIItemCell ItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x060040BE RID: 16574 RVA: 0x0013589B File Offset: 0x00133A9B
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			this.TryInitGamepadNavigationItem();
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x001358A9 File Offset: 0x00133AA9
	private void Awake()
	{
		this.nextItemButton.onClick.AddListener(new UnityAction(this.OnNextItem));
		this.prevItemButton.onClick.AddListener(new UnityAction(this.OnPrevItem));
		this.TryInitGamepadNavigationItem();
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x001358EC File Offset: 0x00133AEC
	public void Draw(UICraftItemCellData craftItemCellData, Action onVariableItemChanged, bool isTooltipView = false)
	{
		this.data = craftItemCellData;
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
		this.variabilityObject.SetActive(isTooltipView && craftItemCellData.initialItem.groupType > ItemGroup.None);
		base.gameObject.SetActive(true);
		this.runesLabel.text = this.data.runesStr;
		this.runesLabel.gameObject.SetActive(!string.IsNullOrEmpty(this.data.runesStr));
		this.RedrawItem(null);
		this.isBig = this.data.currentItem.ItemDef != null && this.data.currentItem.ItemDef.itemSize == ItemSize.Big;
		this.UpdateSize(this.isBig);
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x00135A10 File Offset: 0x00133C10
	public void Flush()
	{
		this.currentMultiplier = 1;
		this.onVariableItemChanged = null;
		this.nextItemButton.gameObject.SetActive(false);
		this.prevItemButton.gameObject.SetActive(false);
		this.UnsubscribeFromInventoryChanges();
		if (this.isBig)
		{
			this.UpdateSize(false);
		}
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00135A64 File Offset: 0x00133C64
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

	// Token: 0x060040C3 RID: 16579 RVA: 0x00135AD0 File Offset: 0x00133CD0
	private void RedrawItem(List<Item> items = null)
	{
		if (this.data.needDurability > 0f)
		{
			this.itemCell.Draw(new Item(this.data.currentItem.Id, this.data.currentItem.GetCount(this.data.WgoData)), true, this.data.MultiInventory.HasItemWithEnoughDurability(this.data.currentItem.Id, this.data.needDurability) ? 0 : 1, false, this.currentMultiplier, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		}
		else
		{
			this.itemCell.Draw(new Item(this.data.currentItem.Id, this.data.currentItem.GetCount(this.data.WgoData)), true, this.data.MultiInventory.GetTotalCount(this.data.currentItem.Id), false, this.currentMultiplier, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		}
		this.itemCell.ShowMouseSelectionFrame = false;
		this.itemCell.SetNativeSizeForIcon();
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00135BED File Offset: 0x00133DED
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

	// Token: 0x060040C5 RID: 16581 RVA: 0x00135C11 File Offset: 0x00133E11
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

	// Token: 0x060040C6 RID: 16582 RVA: 0x00135C38 File Offset: 0x00133E38
	private void SubscribeToInventoryChanges()
	{
		if (!this.subscribedToInventoryChanges)
		{
			MainGame.PlayerData.inventory.OnItemsAdd += this.RedrawItem;
			MainGame.PlayerData.inventory.OnItemsRemove += this.RedrawItem;
			this.subscribedToInventoryChanges = true;
		}
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x00135C8C File Offset: 0x00133E8C
	private void UnsubscribeFromInventoryChanges()
	{
		if (this.subscribedToInventoryChanges)
		{
			MainGame.PlayerData.inventory.OnItemsAdd -= this.RedrawItem;
			MainGame.PlayerData.inventory.OnItemsRemove -= this.RedrawItem;
			this.subscribedToInventoryChanges = false;
		}
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x00135CE0 File Offset: 0x00133EE0
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

	// Token: 0x060040C9 RID: 16585 RVA: 0x00135D53 File Offset: 0x00133F53
	public void SetMultiplierValue(int multiplier)
	{
		this.currentMultiplier = multiplier;
		this.itemCell.OnMultiplierChange(this.currentMultiplier, false);
	}

	// Token: 0x040032BE RID: 12990
	private Action onVariableItemChanged;

	// Token: 0x040032BF RID: 12991
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x040032C0 RID: 12992
	[SerializeField]
	private LazyButton nextItemButton;

	// Token: 0x040032C1 RID: 12993
	[SerializeField]
	private LazyButton prevItemButton;

	// Token: 0x040032C2 RID: 12994
	[SerializeField]
	private GameObject variabilityObject;

	// Token: 0x040032C3 RID: 12995
	[SerializeField]
	private TextMeshProUGUI runesLabel;

	// Token: 0x040032C4 RID: 12996
	[SerializeField]
	private Vector2 defaultLayoutSize;

	// Token: 0x040032C5 RID: 12997
	[SerializeField]
	private Vector2 bigLayoutSize;

	// Token: 0x040032C6 RID: 12998
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x040032C7 RID: 12999
	private bool subscribedToInventoryChanges;

	// Token: 0x040032C8 RID: 13000
	private int currentMultiplier = 1;

	// Token: 0x040032C9 RID: 13001
	private UICraftItemCellData data;

	// Token: 0x040032CA RID: 13002
	private bool isBig;

	// Token: 0x040032CB RID: 13003
	private GamepadNavigationItem gamepadNavigationItem;
}
