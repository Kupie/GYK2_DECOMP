using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A3B RID: 2619
public class UIFuelCraftWindow : UIBaseCraftSelectionWindow
{
	// Token: 0x06004690 RID: 18064 RVA: 0x0014E00C File Offset: 0x0014C20C
	public override void Init()
	{
		base.Init();
		this.startCraftButton.onClick.AddListener(new UnityAction(this.OnStartCraftPressed));
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x0014E031 File Offset: 0x0014C231
	public override void DeInit()
	{
		base.DeInit();
		this.startCraftButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x0014E049 File Offset: 0x0014C249
	protected override void SetData(UIBaseCraftSelectionWindowData data)
	{
		base.SetData(data);
		this.SubscribeToDataChanges();
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x0014E058 File Offset: 0x0014C258
	public override void Redraw()
	{
		base.Redraw();
		base.DrawBaseElements();
		this.outputItem.UIItemCell.ShowMouseSelectionFrame = false;
		this.UpdateDrawState(null);
		base.UpdateTalent();
		if (this.data.WgoData.id == "alchemy_flask_1_shed")
		{
			this.headerLabel.text = LLBase.L("ui_place");
			this.descriptionLabel.text = LLBase.L("ui_alchemy_flask_1_shed");
		}
		else
		{
			this.descriptionLabel.text = LLBase.L("ui_fuel_helper");
		}
		UIInfoWidgetData uiinfoWidgetData = new UIInfoWidgetData(this.data.WgoData, this.data.CraftDefinition, true);
		this.infoWidget.Draw(uiinfoWidgetData);
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004694 RID: 18068 RVA: 0x0014E13C File Offset: 0x0014C33C
	protected override void ChangeCraftCount(int delta)
	{
		if (delta == 0 || this.data == null)
		{
			return;
		}
		if (this.data.CraftComponent.CraftElementsQueue.Count == 0)
		{
			base.ChangeCraftCount(delta);
			return;
		}
		if (delta > 0)
		{
			this.ChangeQueueElementCount(delta);
		}
		else
		{
			this.RemoveQueueElementCount(-delta);
		}
		this.UpdateButtonsInteractableState();
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x0014E18F File Offset: 0x0014C38F
	protected override void UpdateButtonsText()
	{
		this.startCraftButtonText.text = LLBase.L("ui_place");
		base.AddTalentLockText(this.startCraftButtonText);
	}

	// Token: 0x06004696 RID: 18070 RVA: 0x0014E1B4 File Offset: 0x0014C3B4
	protected override void UpdateCraftCountElementsInteractableStatus()
	{
		if (this.data.CraftComponent.CraftElementsQueue.Count == 0)
		{
			base.UpdateCraftCountElementsInteractableStatus();
			return;
		}
		this.minusCraftButton.interactable = this.data.CraftQueue.Count > 1 || this.data.CraftQueue[0].Count != 1 || !this.data.CraftQueue[0].IsStarted;
	}

	// Token: 0x06004697 RID: 18071 RVA: 0x001330F4 File Offset: 0x001312F4
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x06004698 RID: 18072 RVA: 0x0014E232 File Offset: 0x0014C432
	protected override string GetStartCraftGamepadTipKey()
	{
		return "ui_place";
	}

	// Token: 0x06004699 RID: 18073 RVA: 0x0014E23C File Offset: 0x0014C43C
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

	// Token: 0x0600469A RID: 18074 RVA: 0x00133356 File Offset: 0x00131556
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x0600469B RID: 18075 RVA: 0x0014E2E0 File Offset: 0x0014C4E0
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (gamepadNavigationItem != null && gamepadNavigationItem == this.outputItem.UIItemCell.GamepadNavigationItem)
		{
			base.AddCraftCountGamepadTips(list);
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x0014E348 File Offset: 0x0014C548
	protected override void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChanges)
		{
			this.data.SubscribeToDataChanges();
			this.data.CraftComponent.OnCraftAddedToQueue += this.UpdateDrawState;
			this.data.CraftComponent.OnCraftRemovedFromQueue += this.UpdateDrawState;
			this.subscribedToDataChanges = true;
		}
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x0014E3A8 File Offset: 0x0014C5A8
	protected override void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChanges)
		{
			this.data.UnsubscribeFromDataChanges();
			this.data.CraftComponent.OnCraftAddedToQueue -= this.UpdateDrawState;
			this.data.CraftComponent.OnCraftRemovedFromQueue -= this.UpdateDrawState;
			this.subscribedToDataChanges = false;
		}
	}

	// Token: 0x0600469E RID: 18078 RVA: 0x0014E407 File Offset: 0x0014C607
	public override void Close()
	{
		base.Close();
		this.UnsubscribeFromDataChanges();
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x0014E418 File Offset: 0x0014C618
	private void UpdateDrawState(CraftElementBase element = null)
	{
		this.outputItem.UIItemCell.OnMultiplierChange(1, false);
		this.data.SetCraftCount(1);
		if (this.data.CraftComponent.CraftElementsQueue.Count == 2)
		{
			this.outputItem.UpdateQueueCount(this.data.CraftComponent.CraftElementsQueue[0].Count + this.data.CraftComponent.CraftElementsQueue[1].Count);
		}
		else if (this.data.CraftComponent.CraftElementsQueue.Count == 1)
		{
			this.outputItem.UpdateQueueCount(this.data.CraftComponent.CraftElementsQueue[0].Count);
		}
		else
		{
			this.outputItem.UpdateQueueCount(0);
		}
		this.UpdateCraftCountElementsInteractableStatus();
	}

	// Token: 0x060046A0 RID: 18080 RVA: 0x0014E4F4 File Offset: 0x0014C6F4
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

	// Token: 0x060046A1 RID: 18081 RVA: 0x0014E53C File Offset: 0x0014C73C
	private void ChangeQueueElementCount(int delta)
	{
		if (this.data.CraftQueue.Count > 1)
		{
			this.data.CraftQueue[1].Count = Math.Min(this.data.CraftQueue[1].Count + delta, 999);
		}
		else
		{
			this.data.CraftQueue[0].Count = Math.Min(this.data.CraftQueue[0].Count + delta, 999);
		}
		this.UpdateDrawState(null);
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x0014E5D8 File Offset: 0x0014C7D8
	private void RemoveQueueElementCount(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (this.data.CraftQueue.Count == 0)
			{
				return;
			}
			this.OnMinusQueueElement();
		}
	}

	// Token: 0x060046A3 RID: 18083 RVA: 0x0014E60C File Offset: 0x0014C80C
	private void OnMinusQueueElement()
	{
		if (this.data.CraftQueue.Count > 1)
		{
			CraftElementBase craftElementBase = this.data.CraftQueue[1];
			int num = craftElementBase.Count;
			craftElementBase.Count = num - 1;
			if (this.data.CraftQueue[1].Count == 0)
			{
				this.RemoveFromQueue(this.data.CraftQueue[1]);
			}
		}
		else if (this.data.CraftQueue[0].Count != 1 || !this.data.CraftQueue[0].IsStarted)
		{
			CraftElementBase craftElementBase2 = this.data.CraftQueue[0];
			int num = craftElementBase2.Count;
			craftElementBase2.Count = num - 1;
			if (this.data.CraftQueue[0].Count == 0)
			{
				this.RemoveFromQueue(this.data.CraftQueue[0]);
			}
		}
		this.UpdateDrawState(null);
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x0014E70C File Offset: 0x0014C90C
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("test_firewood_shed_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		CraftInteractionHandler craftInteractionHandler = new CraftInteractionHandler();
		craftInteractionHandler.Init(wgo);
		craftInteractionHandler.HasInteraction(MainGame.PlayerController);
		craftInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x04003734 RID: 14132
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x04003735 RID: 14133
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;
}
