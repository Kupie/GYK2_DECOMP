using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B3 RID: 2227
public class MultiInventoryWidget : LazyWidget<MultiInventoryWidgetData>
{
	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x060039C7 RID: 14791 RVA: 0x00114FB8 File Offset: 0x001131B8
	// (set) Token: 0x060039C8 RID: 14792 RVA: 0x00114FC0 File Offset: 0x001131C0
	public Action OnInventoryRemove { get; set; }

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x060039C9 RID: 14793 RVA: 0x00114FC9 File Offset: 0x001131C9
	// (set) Token: 0x060039CA RID: 14794 RVA: 0x00114FD1 File Offset: 0x001131D1
	public Action OnInventoryAdd { get; set; }

	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x060039CB RID: 14795 RVA: 0x00114FDA File Offset: 0x001131DA
	// (set) Token: 0x060039CC RID: 14796 RVA: 0x00114FE2 File Offset: 0x001131E2
	public Action OnAnyInventoryRedraw { get; set; }

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x060039CD RID: 14797 RVA: 0x00114FEB File Offset: 0x001131EB
	// (set) Token: 0x060039CE RID: 14798 RVA: 0x00114FF3 File Offset: 0x001131F3
	public Action OnMoveAllSimilarBtnInteractableChanged { get; set; }

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x060039CF RID: 14799 RVA: 0x00114FFC File Offset: 0x001131FC
	public MultiInventoryWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x060039D0 RID: 14800 RVA: 0x00115004 File Offset: 0x00113204
	public bool IsBagSelected
	{
		get
		{
			return this.selectedBag != null;
		}
	}

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x00115010 File Offset: 0x00113210
	public bool IsMoveAllSimilarBtnInteractable
	{
		get
		{
			for (int i = 0; i < this.drawnInventories.Count; i++)
			{
				if (this.drawnInventories[i].InventoryHeaderWidget.IsMoveAllSimilarBtnInteractable)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x060039D2 RID: 14802 RVA: 0x0011504E File Offset: 0x0011324E
	public List<InventoryWidget> DrawnInventories
	{
		get
		{
			return this.drawnInventories;
		}
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x00115056 File Offset: 0x00113256
	public override void Init()
	{
		base.Init();
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, 42f);
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x00115070 File Offset: 0x00113270
	protected override void SetData(MultiInventoryWidgetData data)
	{
		base.SetData(data);
		this.scrollRect.verticalNormalizedPosition = 1f;
		data.OnInventoryAdded = new Action<InventoryWidgetDataBase>(this.OnInventoryAdded);
		data.OnInventoryRemoved = new Action<int>(this.OnInventoryRemoved);
		data.OnMoveAllSimilarTargetChanged = new Action(this.RefreshMoveAllSimilarBtnInteractable);
		foreach (InventoryWidgetDataBase inventoryWidgetDataBase in data.inventoriesData)
		{
			this.AddInventoryWidget(inventoryWidgetDataBase);
		}
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x00115110 File Offset: 0x00113310
	public override void Redraw()
	{
		base.Redraw();
		this.UpdateHeader();
		for (int i = 0; i < this.data.inventoriesData.Count; i++)
		{
			InventoryWidget inventoryWidget = this.drawnInventories[i];
			inventoryWidget.doNotTriggerOnRedraw = true;
			inventoryWidget.Draw(this.data.inventoriesData[i]);
			inventoryWidget.doNotTriggerOnRedraw = false;
		}
		this.OnInventoryRedraw();
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x0011517C File Offset: 0x0011337C
	private void UpdateHeader()
	{
		if (this.headerLabel == null)
		{
			this.headerLabel = base.GetComponentInChildren<LocalizedLabel>(true);
		}
		if (this.headerLabel == null)
		{
			return;
		}
		this.headerLabel.langToken = (string.IsNullOrEmpty(this.data.HeaderLocaleId) ? "ui_multiinventory" : this.data.HeaderLocaleId);
		this.headerLabel.Localize();
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x001151F0 File Offset: 0x001133F0
	public override void Hide()
	{
		base.Hide();
		if (this.data != null)
		{
			this.data.OnMoveAllSimilarTargetChanged = null;
		}
		for (int i = this.drawnInventories.Count - 1; i >= 0; i--)
		{
			this.ReleaseInventoryWidget(this.drawnInventories[i]);
		}
		this.drawnInventories.Clear();
		MultiInventoryWidgetData data = this.data;
		if (data == null)
		{
			return;
		}
		Action onWidgetHide = data.onWidgetHide;
		if (onWidgetHide == null)
		{
			return;
		}
		onWidgetHide();
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x00115268 File Offset: 0x00113468
	public void UpdatePrices(InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate priceDelegate, int countModificator)
	{
		if (priceDelegate == null)
		{
			return;
		}
		foreach (InventoryWidget inventoryWidget in this.drawnInventories)
		{
			inventoryWidget.UpdatePrices(priceDelegate, countModificator);
		}
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x001152C0 File Offset: 0x001134C0
	public void UpdateHappinessStatusIcons(Vendor vendor, Func<string, int> getExtraSoldCount = null, float extraUsedHappiness = 0f)
	{
		foreach (InventoryWidget inventoryWidget in this.drawnInventories)
		{
			inventoryWidget.UpdateHappinessStatusIcons(vendor, getExtraSoldCount, extraUsedHappiness);
		}
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x00115314 File Offset: 0x00113514
	private void OnInventoryRemoved(int index)
	{
		this.ReleaseInventoryWidget(this.drawnInventories[index]);
		this.drawnInventories.RemoveAt(index);
		Action onInventoryRemove = this.OnInventoryRemove;
		if (onInventoryRemove != null)
		{
			onInventoryRemove();
		}
		this.OnInventoryRedraw();
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x0011534B File Offset: 0x0011354B
	private void OnInventoryAdded(InventoryWidgetDataBase inventoryWidgetData)
	{
		this.AddInventoryWidget(inventoryWidgetData);
		List<InventoryWidget> list = this.drawnInventories;
		list[list.Count - 1].Draw(inventoryWidgetData);
		Action onInventoryAdd = this.OnInventoryAdd;
		if (onInventoryAdd != null)
		{
			onInventoryAdd();
		}
		this.OnInventoryRedraw();
	}

	// Token: 0x060039DC RID: 14812 RVA: 0x00115384 File Offset: 0x00113584
	private void OnInventoryRedraw()
	{
		((RectTransform)base.transform).RefreshContentFitter();
		Canvas.ForceUpdateCanvases();
		Action onAnyInventoryRedraw = this.OnAnyInventoryRedraw;
		if (onAnyInventoryRedraw == null)
		{
			return;
		}
		onAnyInventoryRedraw();
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x001153AC File Offset: 0x001135AC
	private void AddInventoryWidget(InventoryWidgetDataBase widgetDataBase)
	{
		InventoryWidget inventoryWidget;
		if (widgetDataBase is BagInventoryWidgetData)
		{
			inventoryWidget = UIPrefabsPooler.Instance.GetElementFromPool<BagInventoryWidget>(this.inventoryContainer);
		}
		else
		{
			InventoryWidgetDataBase inventoryWidgetDataBase = widgetDataBase as InventoryWidgetData;
			bool flag = false;
			WhiteListFilterSerializedItemProperty whiteListFilterSerializedItemProperty;
			if (inventoryWidgetDataBase.Inventory.Data.TryGetProperty<WhiteListFilterSerializedItemProperty>(out whiteListFilterSerializedItemProperty))
			{
				for (int i = 0; i < whiteListFilterSerializedItemProperty.WhiteList.itemsIds.Count; i++)
				{
					if (GameBalance.Me.GetData<ItemDef>(whiteListFilterSerializedItemProperty.WhiteList.itemsIds[i]).itemSize == ItemSize.Big)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				inventoryWidget = UIPrefabsPooler.Instance.GetElementFromPool<BigItemInventoryWidget>(this.inventoryContainer);
			}
			else
			{
				inventoryWidget = UIPrefabsPooler.Instance.GetElementFromPool<InventoryWidget>(this.inventoryContainer);
			}
		}
		InventoryWidgetData inventoryWidgetData = widgetDataBase as InventoryWidgetData;
		if (inventoryWidgetData != null)
		{
			inventoryWidgetData.OnWidgetPressed = new Action<InventoryWidget>(this.OnWidgetPressed);
		}
		this.drawnInventories.Add(inventoryWidget);
		inventoryWidget.Init();
		inventoryWidget.onRedraw += this.OnInventoryRedraw;
		inventoryWidget.InventoryHeaderWidget.OnMoveAllSimilarBtnInteractableChanged += this.OnHeaderMoveAllSimilarBtnInteractableChanged;
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x001154B8 File Offset: 0x001136B8
	private void ReleaseInventoryWidget(InventoryWidget inventoryWidget)
	{
		inventoryWidget.InventoryHeaderWidget.OnMoveAllSimilarBtnInteractableChanged -= this.OnHeaderMoveAllSimilarBtnInteractableChanged;
		inventoryWidget.onRedraw -= this.OnInventoryRedraw;
		inventoryWidget.Hide();
		inventoryWidget.Data.OnWidgetPressed = null;
		BagInventoryWidget bagInventoryWidget = inventoryWidget as BagInventoryWidget;
		if (bagInventoryWidget != null)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<BagInventoryWidget>(bagInventoryWidget);
			return;
		}
		BigItemInventoryWidget bigItemInventoryWidget = inventoryWidget as BigItemInventoryWidget;
		if (bigItemInventoryWidget != null)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<BigItemInventoryWidget>(bigItemInventoryWidget);
			return;
		}
		UIPrefabsPooler.Instance.ReleaseElementToPool<InventoryWidget>(inventoryWidget);
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x00115538 File Offset: 0x00113738
	public void SelectFirstWidget()
	{
		MultiInventoryWidgetMode widgetSelectionMode = this.data.WidgetSelectionMode;
		if ((widgetSelectionMode == MultiInventoryWidgetMode.SelectionWithMoveBtn || widgetSelectionMode == MultiInventoryWidgetMode.SelectionWithoutMoveBtn || widgetSelectionMode == MultiInventoryWidgetMode.BagMode) && this.drawnInventories.Count > 0)
		{
			this.SetDefaultStateForWidgetAndInactiveForOthers(this.drawnInventories[0]);
		}
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x00115580 File Offset: 0x00113780
	private void OnWidgetPressed(InventoryWidget inventoryWidget)
	{
		MultiInventoryWidgetMode widgetSelectionMode = this.data.WidgetSelectionMode;
		if (widgetSelectionMode == MultiInventoryWidgetMode.SelectionWithMoveBtn || widgetSelectionMode == MultiInventoryWidgetMode.SelectionWithoutMoveBtn || widgetSelectionMode == MultiInventoryWidgetMode.BagMode)
		{
			this.SetDefaultStateForWidgetAndInactiveForOthers(inventoryWidget);
		}
		if (this.data.WidgetSelectionMode == MultiInventoryWidgetMode.BagMode && inventoryWidget.Data.Inventory.Data != this.selectedBag)
		{
			this.SetDefaultStateForWidgetAndInactiveForOthers(inventoryWidget);
		}
	}

	// Token: 0x060039E1 RID: 14817 RVA: 0x001155DC File Offset: 0x001137DC
	public void EnableBagMode(Item bag, Action onBtnPressed)
	{
		this.selectedBag = bag;
		this.data.OnMoveAllSimilarPressed = onBtnPressed;
		this.data.WidgetSelectionMode = MultiInventoryWidgetMode.BagMode;
		InventoryWidget inventoryWidget = this.drawnInventories[0];
		this.data.SelectedWidgetData = inventoryWidget.Data;
		this.SetDefaultStateForWidgetAndInactiveForOthers(inventoryWidget);
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x0011562D File Offset: 0x0011382D
	public void DisableBagMode()
	{
		this.DisableBagMode(this.selectedBag);
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x0011563C File Offset: 0x0011383C
	public void DisableBagMode(Item bag)
	{
		this.data.OnMoveAllSimilarPressed = null;
		this.data.SelectedWidgetData = null;
		this.drawnInventories[0].ChangeMoveAllBtnState(false, null, null);
		for (int i = 0; i < this.drawnInventories.Count; i++)
		{
			if (this.data.WidgetSelectionMode == MultiInventoryWidgetMode.BagMode && this.IsWidgetForBag(this.drawnInventories[i], this.selectedBag))
			{
				this.drawnInventories[i].Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Default;
				this.drawnInventories[i].ChangeMoveAllBtnState(false, null, null);
				this.drawnInventories[i].Redraw();
			}
			else if (this.drawnInventories[i].Data.ItemRelatedWidgetState != ItemRelatedWidgetState.Disabled)
			{
				this.drawnInventories[i].Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Default;
				this.drawnInventories[i].ChangeMoveAllBtnState(false, null, null);
				this.drawnInventories[i].Redraw();
			}
		}
		this.data.WidgetSelectionMode = MultiInventoryWidgetMode.Default;
		this.selectedBag = null;
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x00115760 File Offset: 0x00113960
	private void RefreshMoveAllSimilarBtnInteractable()
	{
		for (int i = 0; i < this.drawnInventories.Count; i++)
		{
			this.drawnInventories[i].InventoryHeaderWidget.RefreshMoveAllSimilarBtnInteractable();
		}
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x00115799 File Offset: 0x00113999
	private void OnHeaderMoveAllSimilarBtnInteractableChanged()
	{
		Action onMoveAllSimilarBtnInteractableChanged = this.OnMoveAllSimilarBtnInteractableChanged;
		if (onMoveAllSimilarBtnInteractableChanged == null)
		{
			return;
		}
		onMoveAllSimilarBtnInteractableChanged();
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x001157AC File Offset: 0x001139AC
	private void SetDefaultStateForWidgetAndInactiveForOthers(InventoryWidget inventoryWidget)
	{
		this.data.SelectedWidgetData = inventoryWidget.Data;
		inventoryWidget.Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Default;
		inventoryWidget.UpdateItemRelatedWidgetStateForEveryThing();
		MultiInventoryWidgetMode widgetSelectionMode = this.data.WidgetSelectionMode;
		if (widgetSelectionMode == MultiInventoryWidgetMode.SelectionWithMoveBtn || widgetSelectionMode == MultiInventoryWidgetMode.BagMode)
		{
			inventoryWidget.ChangeMoveAllBtnState(true, this.data.OnMoveAllSimilarPressed, this.data.GetMoveAllSimilarTargetInventory);
		}
		for (int i = 0; i < this.drawnInventories.Count; i++)
		{
			if (!(this.drawnInventories[i] == inventoryWidget))
			{
				if (this.data.WidgetSelectionMode == MultiInventoryWidgetMode.BagMode && this.IsWidgetForBag(this.drawnInventories[i], this.selectedBag))
				{
					this.drawnInventories[i].Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Disabled;
					this.drawnInventories[i].UpdateItemRelatedWidgetStateForEveryThing();
					this.drawnInventories[i].ChangeMoveAllBtnState(false, null, null);
				}
				else
				{
					if (this.data.WidgetSelectionMode == MultiInventoryWidgetMode.BagMode)
					{
						BagInventoryWidgetData bagInventoryWidgetData = this.drawnInventories[i].Data as BagInventoryWidgetData;
						if (bagInventoryWidgetData != null && bagInventoryWidgetData.ParentInventory == inventoryWidget.Data.Inventory)
						{
							this.drawnInventories[i].Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Inactive;
							this.drawnInventories[i].UpdateItemRelatedWidgetStateForEveryThing();
							this.drawnInventories[i].ChangeMoveAllBtnState(false, null, null);
							goto IL_01A9;
						}
					}
					if (this.drawnInventories[i].Data.ItemRelatedWidgetState != ItemRelatedWidgetState.Disabled)
					{
						this.drawnInventories[i].Data.ItemRelatedWidgetState = ItemRelatedWidgetState.Inactive;
						this.drawnInventories[i].UpdateItemRelatedWidgetStateForEveryThing();
						this.drawnInventories[i].ChangeMoveAllBtnState(false, null, null);
					}
				}
			}
			IL_01A9:;
		}
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x00115978 File Offset: 0x00113B78
	private bool IsWidgetForBag(InventoryWidget widget, Item bag)
	{
		if (bag != null)
		{
			InventoryWidgetData data = widget.Data;
			bool flag;
			if (data == null)
			{
				flag = null != null;
			}
			else
			{
				Inventory inventory = data.Inventory;
				flag = ((inventory != null) ? inventory.Data : null) != null;
			}
			if (flag)
			{
				return widget.Data.Inventory.Data.UniqueId == bag.UniqueId;
			}
		}
		return false;
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x001159CA File Offset: 0x00113BCA
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new MultiInventoryWidgetData(MultiInventoryWidgetMode.Default));
	}

	// Token: 0x04002DA9 RID: 11689
	[SerializeField]
	private Transform inventoryContainer;

	// Token: 0x04002DAA RID: 11690
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002DAB RID: 11691
	[SerializeField]
	private LocalizedLabel headerLabel;

	// Token: 0x04002DAC RID: 11692
	private GamepadNavigationController gamepadNavigationController;

	// Token: 0x04002DAD RID: 11693
	private List<InventoryWidget> drawnInventories = new List<InventoryWidget>();

	// Token: 0x04002DAE RID: 11694
	private bool subscribedInventoryEvents;

	// Token: 0x04002DAF RID: 11695
	private Item selectedBag;
}
