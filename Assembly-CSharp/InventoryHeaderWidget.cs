using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008A4 RID: 2212
public class InventoryHeaderWidget : LazyWidget<InventoryHeaderWidgetData>
{
	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x00112C2F File Offset: 0x00110E2F
	public LazyButton MoveAllSimilarItemsToBagBtn
	{
		get
		{
			return this.moveAllSimilarItemsToBagBtn;
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06003919 RID: 14617 RVA: 0x00112C37 File Offset: 0x00110E37
	public InventoryHeaderWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x00112C3F File Offset: 0x00110E3F
	public bool IsMoveAllSimilarBtnInteractable
	{
		get
		{
			return this.moveAllSimilarItemsToBagBtn != null && this.moveAllSimilarBtnShouldBeShown && this.moveAllSimilarItemsToBagBtn.interactable;
		}
	}

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x0600391B RID: 14619 RVA: 0x00112C64 File Offset: 0x00110E64
	// (remove) Token: 0x0600391C RID: 14620 RVA: 0x00112C9C File Offset: 0x00110E9C
	public event Action OnMoveAllSimilarBtnInteractableChanged;

	// Token: 0x0600391D RID: 14621 RVA: 0x00112CD4 File Offset: 0x00110ED4
	public override void Init()
	{
		base.Init();
		if (this.moveAllSimilarItemsToBagBtn == null)
		{
			return;
		}
		this.moveAllSimilarItemsToBagBtn.onClick.RemoveAllListeners();
		this.moveAllSimilarItemsToBagBtn.onClick.AddListener(new UnityAction(this.OnMoveAllSimilarItemFromPlayerToChestToBagBtnPressed));
		this.moveAllSimilarItemsToBagBtn.onEnterSound = string.Empty;
		this.moveAllSimilarItemsToBagBtn.onNotInteractableEnterSound = string.Empty;
		this.moveAllSimilarItemsToBagBtn.onEnter.RemoveAllListeners();
		this.moveAllSimilarItemsToBagBtn.onNotInteractableEnter.RemoveAllListeners();
		this.moveAllSimilarItemsToBagBtn.onExit.RemoveAllListeners();
		this.moveAllSimilarItemsToBagBtn.onNotInteractableExit.RemoveAllListeners();
		this.moveAllSimilarItemsToBagBtn.onEnter.AddListener(new UnityAction(this.OnMoveAllSimilarBtnOver));
		this.moveAllSimilarItemsToBagBtn.onNotInteractableEnter.AddListener(new UnityAction(this.OnMoveAllSimilarBtnOver));
		this.moveAllSimilarItemsToBagBtn.onExit.AddListener(new UnityAction(this.OnMoveAllSimilarBtnOut));
		this.moveAllSimilarItemsToBagBtn.onNotInteractableExit.AddListener(new UnityAction(this.OnMoveAllSimilarBtnOut));
		this.moveAllSimilarItemsToBagBtn.SetCallbacksIntoGamepadNavigationItem();
		this.moveAllSimilarItemsToBagBtn.gameObject.SetActive(false);
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x00112E0E File Offset: 0x0011100E
	public override void Redraw()
	{
		base.Redraw();
		this.UpdateHeader();
		if (this.moveAllSimilarBtnShouldBeShown)
		{
			this.RefreshMoveAllSimilarBtnInteractable();
		}
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x00112E2A File Offset: 0x0011102A
	public override void Hide()
	{
		this.SetMoveAllSimilarBtnState(false, null, null);
		base.Hide();
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x00112E3B File Offset: 0x0011103B
	public void ChangeActiveViewState(bool isActive)
	{
		this.data.IsActiveViewState = isActive;
		this.UpdateViewState();
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x00112E50 File Offset: 0x00111050
	public void SetMoveAllSimilarBtnState(bool isActive, Action onPress = null, Func<Inventory> getTargetInventory = null)
	{
		if (this.moveAllSimilarItemsToBagBtn == null)
		{
			return;
		}
		bool isMoveAllSimilarBtnInteractable = this.IsMoveAllSimilarBtnInteractable;
		this.moveAllSimilarBtnShouldBeShown = isActive;
		this.ApplyMoveAllSimilarBtnVisibility();
		this.moveAllSimilarItemsToBagBtn.onClick.RemoveAllListeners();
		if (onPress != null)
		{
			this.moveAllSimilarItemsToBagBtn.onClick.AddListener(delegate
			{
				onPress();
			});
		}
		this.getMoveAllSimilarTargetInventory = (isActive ? getTargetInventory : null);
		if (isActive)
		{
			this.RefreshMoveAllSimilarBtnInteractable();
			return;
		}
		this.HideMoveAllSimilarTooltip(true);
		this.UnsubscribeFromMoveAllSimilarInventories();
		this.NotifyMoveAllSimilarBtnInteractableChanged(isMoveAllSimilarBtnInteractable);
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00112EEC File Offset: 0x001110EC
	public void RefreshMoveAllSimilarBtnInteractable()
	{
		if (!this.moveAllSimilarBtnShouldBeShown)
		{
			return;
		}
		this.UpdateMoveAllSimilarInventorySubscriptions();
		this.UpdateMoveAllSimilarBtnInteractable();
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x00112F04 File Offset: 0x00111104
	private void UpdateHeader()
	{
		string headerIconId = this.data.HeaderIconId;
		if (!string.IsNullOrEmpty(headerIconId))
		{
			this.headerIcon.gameObject.SetActive(true);
			this.headerIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(headerIconId, null);
			this.decorWhenNoHeaderIcon.gameObject.SetActive(false);
			this.headerIcon.SetNativeSize();
		}
		else
		{
			this.headerIcon.gameObject.SetActive(false);
			this.decorWhenNoHeaderIcon.gameObject.SetActive(true);
		}
		string text;
		if (!string.IsNullOrEmpty(this.data.CustomHeaderId))
		{
			text = this.data.CustomHeaderId;
		}
		else
		{
			Inventory inventory = this.data.Inventory;
			text = ((inventory != null) ? inventory.ViewId : null);
		}
		string text2 = text;
		if (this.data.DrawHeader && !string.IsNullOrEmpty(text2))
		{
			this.header.text = LLBase.L(text2);
			this.header.transform.parent.gameObject.SetActive(true);
		}
		else
		{
			this.header.transform.parent.gameObject.SetActive(false);
		}
		this.UpdateViewState();
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x00113028 File Offset: 0x00111228
	private void UpdateViewState()
	{
		if (this.data.IsActiveViewState)
		{
			this.headerBackground.sprite = this.headerBackgroundActiveSprite;
			this.headerActiveStyle.ApplyStyle(this.header, false, null, null, null);
			return;
		}
		this.headerBackground.sprite = this.headerBackgroundInactiveSprite;
		this.headerInactiveStyle.ApplyStyle(this.header, false, null, null, null);
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x001130BF File Offset: 0x001112BF
	private void OnMoveAllSimilarItemFromPlayerToChestToBagBtnPressed()
	{
		Action onMoveAllSimilarItemFromPlayerToChest = this.data.OnMoveAllSimilarItemFromPlayerToChest;
		if (onMoveAllSimilarItemFromPlayerToChest == null)
		{
			return;
		}
		onMoveAllSimilarItemFromPlayerToChest();
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x001130D8 File Offset: 0x001112D8
	private void OnMoveAllSimilarBtnOver()
	{
		UITooltip.ShowSimpleInfo(this.moveAllSimilarItemsToBagBtn.transform, LLBase.L("btn_hint_move_all_identical_items"), default(Vector2), null);
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnMoveAllSimilarBtnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x00113109 File Offset: 0x00111309
	private void HideMoveAllSimilarTooltip(bool immediately)
	{
		if (this.moveAllSimilarItemsToBagBtn == null)
		{
			return;
		}
		if (UITooltip.IsTooltipShowingAtTarget(this.moveAllSimilarItemsToBagBtn.transform as RectTransform))
		{
			if (immediately)
			{
				UITooltip.HideImmediately();
				return;
			}
			UITooltip.Hide();
		}
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x00113140 File Offset: 0x00111340
	private void UpdateMoveAllSimilarInventorySubscriptions()
	{
		InventoryHeaderWidgetData data = this.data;
		Inventory inventory = ((data != null) ? data.Inventory : null);
		Func<Inventory> func = this.getMoveAllSimilarTargetInventory;
		Inventory inventory2 = ((func != null) ? func() : null);
		if (this.subscribedSourceInventory != inventory)
		{
			this.UnsubscribeInventory(ref this.subscribedSourceInventory);
			this.SubscribeInventory(inventory, ref this.subscribedSourceInventory);
		}
		if (this.subscribedTargetInventory != inventory2)
		{
			this.UnsubscribeInventory(ref this.subscribedTargetInventory);
			this.SubscribeInventory(inventory2, ref this.subscribedTargetInventory);
		}
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x001131B7 File Offset: 0x001113B7
	private void SubscribeInventory(Inventory inventory, ref Inventory subscribedInventory)
	{
		if (inventory == null)
		{
			return;
		}
		inventory.OnItemsAdd += this.OnMoveAllSimilarInventoriesChanged;
		inventory.OnItemsRemove += this.OnMoveAllSimilarInventoriesChanged;
		subscribedInventory = inventory;
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x001131E4 File Offset: 0x001113E4
	private void UnsubscribeInventory(ref Inventory subscribedInventory)
	{
		if (subscribedInventory == null)
		{
			return;
		}
		subscribedInventory.OnItemsAdd -= this.OnMoveAllSimilarInventoriesChanged;
		subscribedInventory.OnItemsRemove -= this.OnMoveAllSimilarInventoriesChanged;
		subscribedInventory = null;
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x00113214 File Offset: 0x00111414
	private void UnsubscribeFromMoveAllSimilarInventories()
	{
		this.UnsubscribeInventory(ref this.subscribedSourceInventory);
		this.UnsubscribeInventory(ref this.subscribedTargetInventory);
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x0011322E File Offset: 0x0011142E
	private void OnMoveAllSimilarInventoriesChanged(List<Item> items)
	{
		this.UpdateMoveAllSimilarBtnInteractable();
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x00113238 File Offset: 0x00111438
	private void UpdateMoveAllSimilarBtnInteractable()
	{
		if (!this.moveAllSimilarBtnShouldBeShown || this.moveAllSimilarItemsToBagBtn == null)
		{
			return;
		}
		bool isMoveAllSimilarBtnInteractable = this.IsMoveAllSimilarBtnInteractable;
		InventoryHeaderWidgetData data = this.data;
		Inventory inventory = ((data != null) ? data.Inventory : null);
		Func<Inventory> func = this.getMoveAllSimilarTargetInventory;
		Inventory inventory2 = ((func != null) ? func() : null);
		this.moveAllSimilarItemsToBagBtn.interactable = inventory != null && inventory2 != null && inventory2.CanTakeAnyItemsExistingInMeFromOtherInventory(inventory, true, true);
		this.NotifyMoveAllSimilarBtnInteractableChanged(isMoveAllSimilarBtnInteractable);
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x001132AD File Offset: 0x001114AD
	private void NotifyMoveAllSimilarBtnInteractableChanged(bool wasInteractable)
	{
		if (wasInteractable != this.IsMoveAllSimilarBtnInteractable)
		{
			Action onMoveAllSimilarBtnInteractableChanged = this.OnMoveAllSimilarBtnInteractableChanged;
			if (onMoveAllSimilarBtnInteractableChanged == null)
			{
				return;
			}
			onMoveAllSimilarBtnInteractableChanged();
		}
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x001132C8 File Offset: 0x001114C8
	private void ApplyMoveAllSimilarBtnVisibility()
	{
		if (this.moveAllSimilarItemsToBagBtn == null)
		{
			return;
		}
		bool flag = this.moveAllSimilarBtnShouldBeShown && !LazyInput.IsGamepadActive;
		if (this.moveAllSimilarItemsToBagBtn.gameObject.activeSelf == flag)
		{
			return;
		}
		this.moveAllSimilarItemsToBagBtn.gameObject.SetActive(flag);
		if (!flag)
		{
			this.HideMoveAllSimilarTooltip(true);
		}
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x00113327 File Offset: 0x00111527
	private void OnInputChanged()
	{
		this.ApplyMoveAllSimilarBtnVisibility();
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x0011332F File Offset: 0x0011152F
	private void TrySubscribeToInputEvents()
	{
		if (this.subscribedToInputEvents)
		{
			return;
		}
		LazyInput.OnInputChanged += this.OnInputChanged;
		LazyInput.OnActiveGamepadChangedEvent += this.OnInputChanged;
		this.subscribedToInputEvents = true;
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x00113363 File Offset: 0x00111563
	private void TryUnsubscribeFromInputEvents()
	{
		if (!this.subscribedToInputEvents)
		{
			return;
		}
		LazyInput.OnInputChanged -= this.OnInputChanged;
		LazyInput.OnActiveGamepadChangedEvent -= this.OnInputChanged;
		this.subscribedToInputEvents = false;
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x00113397 File Offset: 0x00111597
	private void OnEnable()
	{
		this.TrySubscribeToInputEvents();
		this.ApplyMoveAllSimilarBtnVisibility();
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x001133A5 File Offset: 0x001115A5
	private void OnDisable()
	{
		this.TryUnsubscribeFromInputEvents();
		this.HideMoveAllSimilarTooltip(true);
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002D5D RID: 11613
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x04002D5E RID: 11614
	[SerializeField]
	private Image headerBackground;

	// Token: 0x04002D5F RID: 11615
	[SerializeField]
	private Image headerIcon;

	// Token: 0x04002D60 RID: 11616
	[SerializeField]
	private Image decorWhenNoHeaderIcon;

	// Token: 0x04002D61 RID: 11617
	[SerializeField]
	private LazyButton moveAllSimilarItemsToBagBtn;

	// Token: 0x04002D62 RID: 11618
	[SerializeField]
	private Sprite headerBackgroundActiveSprite;

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	private Sprite headerBackgroundInactiveSprite;

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	private TextStyle headerActiveStyle;

	// Token: 0x04002D65 RID: 11621
	[SerializeField]
	private TextStyle headerInactiveStyle;

	// Token: 0x04002D67 RID: 11623
	private Func<Inventory> getMoveAllSimilarTargetInventory;

	// Token: 0x04002D68 RID: 11624
	private Inventory subscribedSourceInventory;

	// Token: 0x04002D69 RID: 11625
	private Inventory subscribedTargetInventory;

	// Token: 0x04002D6A RID: 11626
	private bool moveAllSimilarBtnShouldBeShown;

	// Token: 0x04002D6B RID: 11627
	private bool subscribedToInputEvents;
}
