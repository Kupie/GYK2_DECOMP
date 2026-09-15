using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000A6D RID: 2669
public class UITownBuildingWindow : LazyWindow<UITownBuildingWindowData>
{
	// Token: 0x0600486E RID: 18542 RVA: 0x00157176 File Offset: 0x00155376
	public override void Init()
	{
		base.Init();
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, this.buildingElement, 70f);
		this.buildingElement.gameObject.SetActive(false);
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x001571B4 File Offset: 0x001553B4
	public override void Redraw()
	{
		this.onBuildPressed = this.data.OnBuildPressed;
		MultiInventory multiInventory = new MultiInventory(this.data.PlayerData, true);
		foreach (TownBuildingDef townBuildingDef in this.data.BuildsToDisplay)
		{
			UITownBuildingWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UITownBuildingWidget>(this.buildingsListContent.transform);
			elementFromPool.Fold();
			this.displayedBuildItemGUIs.Add(elementFromPool);
			UITownBuildingWidgetData uitownBuildingWidgetData = new UITownBuildingWidgetData(townBuildingDef, multiInventory, new Action<TownBuildingDef, List<NeedItemData>>(this.OnBuildPressed), null, null, this.data.AssignedWgo.Data.WorldZoneData);
			elementFromPool.Init();
			elementFromPool.Draw(uitownBuildingWidgetData);
		}
		this.UpdateNoBuildingsObject();
		string text = (string.IsNullOrEmpty(this.data.AssignedWgo.Data.Definition.craftIconId) ? "i_b_city" : this.data.AssignedWgo.Data.Definition.craftIconId);
		UIInfoWidgetData uiinfoWidgetData = new UIInfoWidgetData(this.data.AssignedWgo.Data, text, false);
		uiinfoWidgetData.CraftComponent = null;
		this.uiInfoWidget.Draw(uiinfoWidgetData);
		base.Redraw();
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x00157308 File Offset: 0x00155508
	public override void Open(UITownBuildingWindowData data)
	{
		this.autoScroll.SkipNextAutoscroll = true;
		base.Open(data);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.scrollRect.DOKill(false);
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x0015735C File Offset: 0x0015555C
	public override void Hide()
	{
		foreach (UITownBuildingWidget uitownBuildingWidget in this.displayedBuildItemGUIs)
		{
			uitownBuildingWidget.DeInit();
			uitownBuildingWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UITownBuildingWidget>(uitownBuildingWidget);
		}
		this.displayedBuildItemGUIs.Clear();
		base.Hide();
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x001573D0 File Offset: 0x001555D0
	private void OnBuildPressed(TownBuildingDef buildData, List<NeedItemData> needItems)
	{
		Action<TownBuildingDef, List<NeedItemData>> action = this.onBuildPressed;
		if (action == null)
		{
			return;
		}
		action(buildData, needItems);
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x001573E4 File Offset: 0x001555E4
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Fold, new Func<bool>(this.FoldPress));
		return gameKeyDelegates;
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x00157403 File Offset: 0x00155603
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (!LazyInput.IsGamepadActive && this.foldedElement != null)
		{
			this.foldedElement.Fold();
			this.foldedElement = null;
		}
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x00157434 File Offset: 0x00155634
	private bool FoldPress()
	{
		UITownBuildingWidget uitownBuildingWidget;
		if (base.GamepadNavigationController.FocusedItem.TryGetComponent<UITownBuildingWidget>(out uitownBuildingWidget))
		{
			uitownBuildingWidget.Unfold();
			this.foldedElement = uitownBuildingWidget;
			if (uitownBuildingWidget.DisplayedIngredients.Count > 0)
			{
				base.GamepadNavigationController.SetFocusedItem(uitownBuildingWidget.DisplayedIngredients[0].GamepadNavigationItem);
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

	// Token: 0x06004876 RID: 18550 RVA: 0x001574D0 File Offset: 0x001556D0
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Select(true, true, true));
		UITownBuildingWidget uitownBuildingWidget;
		if (gamepadNavigationItem.TryGetComponent<UITownBuildingWidget>(out uitownBuildingWidget) && uitownBuildingWidget.DisplayedIngredients.Count > 0)
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_unfold", true, true, true));
		}
		UICraftItemCell uicraftItemCell;
		if (gamepadNavigationItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_fold", true, true, true));
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x00157570 File Offset: 0x00155770
	private void UpdateNoBuildingsObject()
	{
		bool flag = this.displayedBuildItemGUIs.Count > 0;
		this.noBuildingsObj.SetActive(!flag);
		if (!flag)
		{
			this.noBuildingsText.text = LLBase.L("ui_builddesk_town_is_empty");
		}
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003880 RID: 14464
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x04003881 RID: 14465
	[SerializeField]
	private UITownBuildingWidget buildingElement;

	// Token: 0x04003882 RID: 14466
	[SerializeField]
	[Space]
	private GameObject buildingsListContent;

	// Token: 0x04003883 RID: 14467
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04003884 RID: 14468
	[SerializeField]
	private AutoScroll autoScroll;

	// Token: 0x04003885 RID: 14469
	[SerializeField]
	private GameObject noBuildingsObj;

	// Token: 0x04003886 RID: 14470
	[SerializeField]
	private TextMeshProUGUI noBuildingsText;

	// Token: 0x04003887 RID: 14471
	private List<UITownBuildingWidget> displayedBuildItemGUIs = new List<UITownBuildingWidget>();

	// Token: 0x04003888 RID: 14472
	private Action<TownBuildingDef, List<NeedItemData>> onBuildPressed;

	// Token: 0x04003889 RID: 14473
	private UITownBuildingWidget foldedElement;
}
