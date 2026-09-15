using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200097A RID: 2426
public class UICraftSelectionWindow : UIBaseCraftSelectionWindow, IUIWindowCustomOperable
{
	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x0600401C RID: 16412 RVA: 0x00132E59 File Offset: 0x00131059
	private UICraftSelectionWindowData Data
	{
		get
		{
			return this.data as UICraftSelectionWindowData;
		}
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x00132E68 File Offset: 0x00131068
	public override void Init()
	{
		base.Init();
		this.addToQueueButton.onClick.AddListener(new UnityAction(this.OnAddToQueuePressed));
		this.startCraftButton.onClick.AddListener(new UnityAction(this.OnStartCraftPressed));
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x00132EB4 File Offset: 0x001310B4
	public override void DeInit()
	{
		base.DeInit();
		this.addToQueueButton.onClick.RemoveAllListeners();
		this.startCraftButton.onClick.RemoveAllListeners();
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00132EDC File Offset: 0x001310DC
	public override void Redraw()
	{
		base.Redraw();
		base.DrawBaseElements();
		base.UpdateTalent();
		this.DrawBoostItemCell();
		bool flag = !(this.data.WgoData.Worker is ZombieWgoData) && !this.Data.IsGravePartRemove;
		this.startCraftButton.gameObject.SetActive(flag);
		this.gamepadTipStartCraft.gameObject.SetActive(flag);
		((RectTransform)base.transform).RefreshContentFitter();
		bool flag2 = !this.data.CraftDefinition.isAutopsyCraft && !this.data.CraftDefinition.isAddToQueueDisabled;
		this.addToQueueButton.gameObject.SetActive(flag2);
		this.gamepadTipQueue.gameObject.SetActive(flag2);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.UpdateCraftGamepadTips();
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x00132FC8 File Offset: 0x001311C8
	private void DrawBoostItemCell()
	{
		this.boostItemCell.gameObject.SetActive(false);
		CraftDef craftDefinition = this.data.CraftDefinition;
		AlchemyMixDef mixDef = craftDefinition as AlchemyMixDef;
		if (mixDef != null && mixDef.BoostCraft != null)
		{
			this.boostItemCell.gameObject.SetActive(true);
			this.boostItemCell.runesLabel.text = mixDef.BoostCraft.GetBoostRunesAsString();
			this.boostItemCell.uiItemCell.DrawCustom(mixDef.BoostCraft.GetCraftResultIcon(this.data.WgoData), 1, true, false);
			this.boostItemCell.uiItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
			{
				UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, mixDef.BoostCraft);
			};
		}
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x00133098 File Offset: 0x00131298
	protected override void UpdateButtonsText()
	{
		this.startCraftButtonText.text = LLBase.L("ui_craft");
		base.AddTalentLockText(this.startCraftButtonText);
		this.addToQueueButtonText.text = (this.Data.IsGravePartRemove ? LLBase.L("remove") : LLBase.L("ui_add_to_queue"));
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x001330F4 File Offset: 0x001312F4
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x06004023 RID: 16419 RVA: 0x001330FC File Offset: 0x001312FC
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.StartCraft, new Func<bool>(base.OnStartCraft));
		gameKeyDelegates.Add(GameKey.AddCraftToQueue, new Func<bool>(this.OnAddToQueue));
		gameKeyDelegates.Add(GameKey.DpadUp, new Func<bool>(this.OnDpadUpPressed));
		gameKeyDelegates.Add(GameKey.DpadDown, new Func<bool>(this.OnDpadDownPressed));
		gameKeyDelegates.Add(GameKey.Up, new Func<bool>(this.OnDpadUpPressed));
		gameKeyDelegates.Add(GameKey.Down, new Func<bool>(this.OnDpadDownPressed));
		gameKeyDelegates.Add(GameKey.RightClick, new Func<bool>(this.OnPressedBack));
		return gameKeyDelegates;
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x001331B8 File Offset: 0x001313B8
	protected override void UpdateCraftGamepadTips()
	{
		base.UpdateCraftGamepadTips();
		if (this.gamepadTipQueue == null || this.addToQueueButton == null)
		{
			return;
		}
		if (this.data != null)
		{
			this.gamepadTipQueue.text = new LazyGameKeyTip(GameKey.AddCraftToQueue, this.Data.IsGravePartRemove ? "remove" : "ui_add_to_queue", this.addToQueueButton.interactable, true, true).ToString();
			return;
		}
		this.gamepadTipQueue.text = new LazyGameKeyTip(GameKey.AddCraftToQueue, "ui_add_to_queue", this.addToQueueButton.interactable, true, true).ToString();
	}

	// Token: 0x06004025 RID: 16421 RVA: 0x00133260 File Offset: 0x00131460
	protected override bool OnDpadUpPressed()
	{
		if (!LazyInput.IsGamepadActive || base.GamepadNavigationController.FocusedItem == null)
		{
			return false;
		}
		UICraftItemCell uicraftItemCell;
		if (base.GamepadNavigationController.FocusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell) && uicraftItemCell.NextItemButton.gameObject.activeSelf && uicraftItemCell.NextItemButton.interactable)
		{
			Button.ButtonClickedEvent onClick = uicraftItemCell.NextItemButton.onClick;
			if (onClick != null)
			{
				onClick.Invoke();
			}
			return true;
		}
		return base.OnDpadUpPressed();
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x001332DC File Offset: 0x001314DC
	protected override bool OnDpadDownPressed()
	{
		if (!LazyInput.IsGamepadActive || base.GamepadNavigationController.FocusedItem == null)
		{
			return false;
		}
		UICraftItemCell uicraftItemCell;
		if (base.GamepadNavigationController.FocusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell) && uicraftItemCell.PrevItemButton.gameObject.activeSelf && uicraftItemCell.PrevItemButton.interactable)
		{
			Button.ButtonClickedEvent onClick = uicraftItemCell.PrevItemButton.onClick;
			if (onClick != null)
			{
				onClick.Invoke();
			}
			return true;
		}
		return base.OnDpadDownPressed();
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x00133356 File Offset: 0x00131556
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x0013336C File Offset: 0x0013156C
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (gamepadNavigationItem != null)
		{
			UICraftItemCell uicraftItemCell;
			if (gamepadNavigationItem == this.outputItem.UIItemCell.GamepadNavigationItem)
			{
				base.AddCraftCountGamepadTips(list);
			}
			else if (gamepadNavigationItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
			{
				if (uicraftItemCell.NextItemButton.gameObject.activeSelf)
				{
					list.Add(new LazyGameKeyTip(GameKey.DpadUp, "tip_next", uicraftItemCell.NextItemButton.interactable, true, true));
				}
				if (uicraftItemCell.PrevItemButton.gameObject.activeSelf)
				{
					list.Add(new LazyGameKeyTip(GameKey.DpadDown, "tip_prev", uicraftItemCell.PrevItemButton.interactable, true, true));
				}
			}
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x0013344C File Offset: 0x0013164C
	protected override void UpdateButtonsInteractableState()
	{
		WgoData wgoData = this.data.WgoData;
		bool flag = !(((wgoData != null) ? wgoData.Worker : null) is ZombieWgoData) && !this.Data.IsGravePartRemove;
		this.startCraftButton.interactable = this.data.CanStartCraft && flag;
		this.addToQueueButton.interactable = !this.data.CraftDefinition.isFuelCraft && !this.data.CraftDefinition.isAutopsyCraft && this.IsCraftAvailableByExtensionState() && !this.data.CraftDefinition.isAddToQueueDisabled;
		this.UpdateCraftGamepadTips();
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x001334F8 File Offset: 0x001316F8
	private bool IsCraftAvailableByExtensionState()
	{
		UIBaseCraftSelectionWindowData data = this.data;
		return ((data != null) ? data.WgoData : null) != null && this.data.CraftDefinition != null && this.data.WgoData.CraftComponent.IsCraftAllowedByAttachedExtensions(this.data.CraftDefinition);
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x00133548 File Offset: 0x00131748
	private void OnAddToQueuePressed()
	{
		Action onAddToCraftQueuePressed = this.onAddToCraftQueuePressed;
		if (onAddToCraftQueuePressed != null)
		{
			onAddToCraftQueuePressed();
		}
		this.Close();
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x00133561 File Offset: 0x00131761
	private bool OnAddToQueue()
	{
		if (this.addToQueueButton.interactable)
		{
			this.OnAddToQueuePressed();
			return true;
		}
		return false;
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x0013357C File Offset: 0x0013177C
	[LazyUITest]
	protected override void TestDraw()
	{
		WgoData wgoData = new WgoData("woodworking_workbench_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(wgoData, wgoData.CraftComponent.CraftsIn[0] as CraftDef, null, null);
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x001335D0 File Offset: 0x001317D0
	[LazyUITest]
	protected void TestDrawStar()
	{
		WgoData wgoData = new WgoData("test_workbench", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(wgoData, wgoData.CraftComponent.CraftsIn[0] as CraftDef, null, null);
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x0013362C File Offset: 0x0013182C
	bool IUIWindowCustomOperable.get_IsShown()
	{
		return base.IsShown;
	}

	// Token: 0x04003266 RID: 12902
	[SerializeField]
	private LazyButton addToQueueButton;

	// Token: 0x04003267 RID: 12903
	[SerializeField]
	private TextMeshProUGUI gamepadTipQueue;

	// Token: 0x04003268 RID: 12904
	[SerializeField]
	private UIMixItemCell boostItemCell;

	// Token: 0x04003269 RID: 12905
	[SerializeField]
	private TextMeshProUGUI addToQueueButtonText;
}
