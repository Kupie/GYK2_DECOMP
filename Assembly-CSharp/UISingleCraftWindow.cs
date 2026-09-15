using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A3C RID: 2620
public class UISingleCraftWindow : UIBaseCraftSelectionWindow
{
	// Token: 0x060046A6 RID: 18086 RVA: 0x0014E00C File Offset: 0x0014C20C
	public override void Init()
	{
		base.Init();
		this.startCraftButton.onClick.AddListener(new UnityAction(this.OnStartCraftPressed));
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x0014E031 File Offset: 0x0014C231
	public override void DeInit()
	{
		base.DeInit();
		this.startCraftButton.onClick.RemoveAllListeners();
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x0014E774 File Offset: 0x0014C974
	protected override void SetData(UIBaseCraftSelectionWindowData data)
	{
		base.SetData(data);
		this.SetupCraftsCountCache();
		this.SubscribeToDataChanges();
	}

	// Token: 0x060046A9 RID: 18089 RVA: 0x0014E78C File Offset: 0x0014C98C
	public override void Redraw()
	{
		base.Redraw();
		base.DrawBaseElements();
		base.UpdateTalent();
		UIInfoWidgetData uiinfoWidgetData = new UIInfoWidgetData(this.data.WgoData, this.data.CraftDefinition, true);
		this.infoWidget.Draw(uiinfoWidgetData);
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x0014E7F9 File Offset: 0x0014C9F9
	protected override void OnStartCraftPressed()
	{
		if (this.data.CraftQueue.Count == 0)
		{
			Action onStartCraftPressed = this.onStartCraftPressed;
			if (onStartCraftPressed != null)
			{
				onStartCraftPressed();
			}
		}
		else
		{
			this.UpdateCraftsCountValue();
		}
		this.Close();
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x0014E82C File Offset: 0x0014CA2C
	protected override void UpdateButtonsInteractableState()
	{
		this.startCraftButton.interactable = (this.data.CraftQueue.Count == 0 && this.data.CanStartCraft) || this.data.CraftQueue.Count > 0;
		this.UpdateCraftGamepadTips();
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x0014E87F File Offset: 0x0014CA7F
	protected override string GetStartCraftGamepadTipKey()
	{
		if (this.data != null && this.data.CraftQueue != null && this.data.CraftQueue.Count > 0)
		{
			return "ui_update";
		}
		return "ui_create";
	}

	// Token: 0x060046AD RID: 18093 RVA: 0x0014E8B4 File Offset: 0x0014CAB4
	protected override void UpdateButtonsText()
	{
		if (this.data.CraftQueue.Count == 0)
		{
			this.startCraftButtonText.text = LLBase.L("ui_create");
		}
		else
		{
			this.startCraftButtonText.text = LLBase.L("ui_update");
		}
		base.AddTalentLockText(this.startCraftButtonText);
	}

	// Token: 0x060046AE RID: 18094 RVA: 0x0014E90C File Offset: 0x0014CB0C
	protected override void UpdateCraftCountElementsInteractableStatus()
	{
		if (this.data.CraftComponent.CraftElementsQueue.Count == 0)
		{
			base.UpdateCraftCountElementsInteractableStatus();
			return;
		}
		this.minusCraftButton.interactable = (this.data.CraftQueue[0].IsStarted ? (this.data.CraftsCount > 1) : (this.data.CraftsCount > 0));
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x0014E978 File Offset: 0x0014CB78
	protected override void Update()
	{
		base.Update();
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x0014E988 File Offset: 0x0014CB88
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.StartCraft, new Func<bool>(base.OnStartCraft));
		gameKeyDelegates.Add(GameKey.DpadUp, new Func<bool>(this.OnDpadUpPressed));
		gameKeyDelegates.Add(GameKey.DpadDown, new Func<bool>(this.OnDpadDownPressed));
		gameKeyDelegates.Add(GameKey.Up, new Func<bool>(this.OnDpadUpPressed));
		gameKeyDelegates.Add(GameKey.Down, new Func<bool>(this.OnDpadDownPressed));
		gameKeyDelegates.Add(GameKey.RightClick, new Func<bool>(this.OnPressedBack));
		return gameKeyDelegates;
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x0014EA2C File Offset: 0x0014CC2C
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

	// Token: 0x060046B2 RID: 18098 RVA: 0x0014EAA8 File Offset: 0x0014CCA8
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

	// Token: 0x060046B3 RID: 18099 RVA: 0x00133356 File Offset: 0x00131556
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x0014EB24 File Offset: 0x0014CD24
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

	// Token: 0x060046B5 RID: 18101 RVA: 0x0014EC04 File Offset: 0x0014CE04
	protected override void UpdateCounters()
	{
		this.outputItem.UIItemCell.OnMultiplierChange(this.data.CraftsCount, this.data.CraftsCount != 1);
		int num = ((this.data.CraftQueue.Count > 0 && this.data.CraftQueue[0].IsStarted) ? (this.data.CraftsCount - 1) : this.data.CraftsCount);
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.SetMultiplierValue(num);
		}
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x0014E407 File Offset: 0x0014C607
	public override void Close()
	{
		base.Close();
		this.UnsubscribeFromDataChanges();
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x0014ECC8 File Offset: 0x0014CEC8
	private void RemoveFromQueue(CraftElementBase craftElement)
	{
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		if (currentCraftElement != null && currentCraftElement == craftElement)
		{
			this.data.CraftComponent.RemoveCurNotStartedCraft();
			return;
		}
		this.data.CraftComponent.RemoveFromQueue(craftElement, false);
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x0014ED10 File Offset: 0x0014CF10
	private void SetupCraftsCountCache()
	{
		this.initialCraftsCount = 1;
		if (this.data.CraftQueue.Count == 1)
		{
			this.initialCraftsCount = this.data.CraftQueue[0].Count;
		}
		else if (this.data.CraftQueue.Count == 2)
		{
			this.initialCraftsCount = this.data.CraftQueue[0].Count + this.data.CraftQueue[1].Count;
		}
		this.data.SetCraftCount(this.initialCraftsCount);
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x0014EDAC File Offset: 0x0014CFAC
	private void UpdateCraftsCountValue()
	{
		int num = this.data.CraftsCount - this.initialCraftsCount;
		int num2 = Mathf.Abs(num);
		if (num <= 0)
		{
			if (num < 0)
			{
				if (this.data.CraftQueue.Count == 1)
				{
					this.RemoveCraftsCount(this.data.CraftQueue[0], num2);
					return;
				}
				if (this.data.CraftQueue.Count == 2)
				{
					if (this.data.CraftQueue[1].Count > num2)
					{
						this.data.CraftQueue[1].Count -= num2;
						return;
					}
					num2 -= this.data.CraftQueue[1].Count;
					this.RemoveFromQueue(this.data.CraftQueue[1]);
					this.RemoveCraftsCount(this.data.CraftQueue[0], num2);
				}
			}
			return;
		}
		if (this.data.CraftQueue.Count == 1)
		{
			this.data.CraftQueue[0].Count += num;
			return;
		}
		this.data.CraftQueue[1].Count += num;
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x0014EEF1 File Offset: 0x0014D0F1
	private void RemoveCraftsCount(CraftElementBase craftElement, int removableCount)
	{
		if (removableCount == 0)
		{
			return;
		}
		if (craftElement.Count <= removableCount)
		{
			this.RemoveFromQueue(craftElement);
			return;
		}
		craftElement.Count -= removableCount;
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x0014EF18 File Offset: 0x0014D118
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("descent_ladder_forest_broken", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		CraftInteractionHandler craftInteractionHandler = new CraftInteractionHandler();
		craftInteractionHandler.Init(wgo);
		craftInteractionHandler.HasInteraction(MainGame.PlayerController);
		craftInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x04003736 RID: 14134
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x04003737 RID: 14135
	private int initialCraftsCount;
}
