using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008DA RID: 2266
public class UIAlchemyBoostsWindow : LazyWindow<UIAlchemyBoostsWindowData>
{
	// Token: 0x06003B07 RID: 15111 RVA: 0x00119F70 File Offset: 0x00118170
	public override void Init()
	{
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, this.boostElement, 70f);
		this.boostElement.gameObject.SetActive(false);
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x00119FAC File Offset: 0x001181AC
	public override void Redraw()
	{
		this.onBoostPressed = this.data.OnCraftPressed;
		this.canCraft = this.data.CanCraft;
		MultiInventory multiInventory = new MultiInventory(this.data.PlayerData, true);
		if (this.data.AdditionalInventories != null)
		{
			foreach (Inventory inventory in this.data.AdditionalInventories)
			{
				multiInventory.Add(inventory);
			}
		}
		foreach (CraftElement craftElement in this.data.CraftsToDisplay)
		{
			UIBoostElement elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIBoostElement>(this.listContent.transform);
			this.displayedBoostElements.Add(elementFromPool);
			UIBoostElementData uiboostElementData = new UIBoostElementData(craftElement, multiInventory, new Action<CraftElement, List<NeedItemData>>(this.OnBoostPressed), this.canCraft, null, null, this.data.AssignedWgo.Data.WorldZoneData, this.data.AssignedWgo.Data);
			elementFromPool.Init();
			elementFromPool.Draw(uiboostElementData);
		}
		base.Redraw();
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x0011A104 File Offset: 0x00118304
	public override void Open(UIAlchemyBoostsWindowData data)
	{
		base.Open(data);
		this.scrollRect.verticalNormalizedPosition = 1f;
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x0011A134 File Offset: 0x00118334
	public override void Hide()
	{
		foreach (UIBoostElement uiboostElement in this.displayedBoostElements)
		{
			uiboostElement.DeInit();
			uiboostElement.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIBoostElement>(uiboostElement);
		}
		this.displayedBoostElements.Clear();
		base.Hide();
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x0011A1A8 File Offset: 0x001183A8
	private void OnBoostPressed(CraftElement craftElement, List<NeedItemData> needItems)
	{
		Action<CraftElement, List<NeedItemData>> action = this.onBoostPressed;
		if (action == null)
		{
			return;
		}
		action(craftElement, needItems);
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x0011A1BC File Offset: 0x001183BC
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Fold, new Func<bool>(this.FoldPress));
		return gameKeyDelegates;
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x0011A1DB File Offset: 0x001183DB
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (!LazyInput.IsGamepadActive && this.foldedElement != null)
		{
			this.foldedElement.Fold();
			this.foldedElement = null;
		}
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x0011A20C File Offset: 0x0011840C
	protected override bool OnPressedBack()
	{
		UICraftItemCell uicraftItemCell;
		if (LazyInput.IsGamepadActive && base.GamepadNavigationController.FocusedItem != null && base.GamepadNavigationController.FocusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			this.foldedElement.Fold();
			base.GamepadNavigationController.SetFocusedItem(this.foldedElement.GetComponentInParent<GamepadNavigationItem>());
			this.foldedElement = null;
			return true;
		}
		return base.OnPressedBack();
	}

	// Token: 0x06003B0F RID: 15119 RVA: 0x0011A278 File Offset: 0x00118478
	private bool FoldPress()
	{
		UIBoostElement uiboostElement;
		if (base.GamepadNavigationController.FocusedItem.TryGetComponent<UIBoostElement>(out uiboostElement))
		{
			uiboostElement.Unfold();
			this.foldedElement = uiboostElement;
			if (uiboostElement.DisplayedIngredients.Count > 0)
			{
				base.GamepadNavigationController.SetFocusedItem(uiboostElement.DisplayedIngredients[0].GamepadNavigationItem);
				return true;
			}
		}
		UICraftItemCell uicraftItemCell;
		if (base.GamepadNavigationController.FocusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			this.foldedElement.Fold();
			base.GamepadNavigationController.SetFocusedItem(this.foldedElement.GetComponentInParent<GamepadNavigationItem>());
			this.foldedElement = null;
			return true;
		}
		return false;
	}

	// Token: 0x06003B10 RID: 15120 RVA: 0x0011A314 File Offset: 0x00118514
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Select(true, true, true));
		UIBoostElement uiboostElement;
		if (gamepadNavigationItem.TryGetComponent<UIBoostElement>(out uiboostElement) && uiboostElement.DisplayedIngredients.Count > 0)
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_unfold", true, true, true));
		}
		UICraftItemCell uicraftItemCell;
		if (gamepadNavigationItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_fold", true, true, true));
			list.Add(new LazyGameKeyTip(GameKey.Back, "tip_fold", true, true, true));
		}
		else if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003B11 RID: 15121 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002EA9 RID: 11945
	[SerializeField]
	private UIBoostElement boostElement;

	// Token: 0x04002EAA RID: 11946
	[SerializeField]
	[Space]
	private GameObject listContent;

	// Token: 0x04002EAB RID: 11947
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002EAC RID: 11948
	private List<UIBoostElement> displayedBoostElements = new List<UIBoostElement>();

	// Token: 0x04002EAD RID: 11949
	private Action<CraftElement, List<NeedItemData>> onBoostPressed;

	// Token: 0x04002EAE RID: 11950
	private Func<CraftElement, List<NeedItemData>, bool> canCraft;

	// Token: 0x04002EAF RID: 11951
	private UIBoostElement foldedElement;
}
