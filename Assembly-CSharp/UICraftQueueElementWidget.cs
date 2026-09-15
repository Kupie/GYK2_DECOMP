using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200098E RID: 2446
[RequireComponent(typeof(LazyButton))]
public class UICraftQueueElementWidget : LazyWidget<UICraftQueueElementWidgetData>
{
	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x060040FD RID: 16637 RVA: 0x0013669A File Offset: 0x0013489A
	public UIItemCell OutputItem
	{
		get
		{
			return this.outputItem;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x060040FE RID: 16638 RVA: 0x001366A2 File Offset: 0x001348A2
	public LazyButton QueueUpButton
	{
		get
		{
			return this.queueUpButton;
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x060040FF RID: 16639 RVA: 0x001366AA File Offset: 0x001348AA
	public LazyButton QueueDownButton
	{
		get
		{
			return this.queueDownButton;
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06004100 RID: 16640 RVA: 0x001366B2 File Offset: 0x001348B2
	public LazyButton MinusCraftButton
	{
		get
		{
			return this.minusCraftButton;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x06004101 RID: 16641 RVA: 0x001366BA File Offset: 0x001348BA
	public LazyButton PlusCraftButton
	{
		get
		{
			return this.plusCraftButton;
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06004102 RID: 16642 RVA: 0x001366C2 File Offset: 0x001348C2
	public UICraftQueueElementWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06004103 RID: 16643 RVA: 0x001366CC File Offset: 0x001348CC
	public override void Init()
	{
		base.Init();
		base.TryGetComponent<LazyButton>(out this.widgetButton);
		this.widgetButton.onClick.AddListener(new UnityAction(this.OnPress));
		this.widgetButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.widgetButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.infCraftButton.onClick.AddListener(new UnityAction(this.OnInfCraftButtonPressed));
		this.queueUpButton.onClick.AddListener(delegate
		{
			this.OnQueueUpButtonPressed();
		});
		this.queueDownButton.onClick.AddListener(delegate
		{
			this.OnQueueDownButtonPressed();
		});
		this.OutputItem.GamepadNavigationItem.SetCallbacks(new UnityAction(this.widgetButton.onEnter.Invoke), new UnityAction(this.widgetButton.onExit.Invoke), new UnityAction(this.widgetButton.onClick.Invoke));
	}

	// Token: 0x06004104 RID: 16644 RVA: 0x001367E8 File Offset: 0x001349E8
	public override void DeInit()
	{
		base.DeInit();
		this.widgetButton.onClick.RemoveAllListeners();
		this.widgetButton.onEnter.RemoveAllListeners();
		this.widgetButton.onExit.RemoveAllListeners();
		this.infCraftButton.onClick.RemoveAllListeners();
		this.queueUpButton.onClick.RemoveAllListeners();
		this.queueDownButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x0013685B File Offset: 0x00134A5B
	protected override void SetData(UICraftQueueElementWidgetData data)
	{
		base.SetData(data);
		this.SubscribeToCraftQueueElementEvents();
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x0013686C File Offset: 0x00134A6C
	public override void Redraw()
	{
		base.Redraw();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.onHide = this.data.OnHide;
		this.onPressPlusQueue = this.data.OnPressPlusQueue;
		this.onPressMinusQueue = this.data.OnPressMinusQueue;
		this.onInfCraftButtonPressed = this.data.OnInfCraftButtonPress;
		this.onQueueUpButtonPressed = this.data.OnPressQueueUp;
		this.onQueueDownButtonPressed = this.data.OnPressQueueDown;
		this.outputItem.gameObject.SetActive(true);
		this.outputItem.DrawCraftOutput(this.data.CraftQueueElement.Def.GetOutputPreview(this.data.WgoData), -1, this.data.CraftQueueElement.CraftStatus, this.data.CraftQueueElement.ParamsData.RequiredToolType, this.data.CraftQueueElement.Count, ItemRelatedWidgetState.NotSet, false);
		this.outputItem.CustomTooltipShowAction = delegate(UIItemCell cell)
		{
			UICraftStatusInfoWidgetData uicraftStatusInfoWidgetData = null;
			if (this.outputItem.StatusIcon != null && this.outputItem.StatusIcon.sprite != null && this.outputItem.StatusIcon.gameObject.activeSelf && this.outputItem.StatusIcon.gameObject.activeInHierarchy && this.data.CraftQueueElement.CraftStatus != CraftStatus.Other)
			{
				uicraftStatusInfoWidgetData = new UICraftStatusInfoWidgetData(this.DefineDescriptionForCraftStatus(), TextAlignmentOptions.Center, null, this.outputItem.StatusIcon.sprite);
			}
			LazyAudio.PlayAndForget("gui_hover_light");
			UITooltip.ShowCraftInfo(cell, this.data.WgoData, this.data.CraftQueueElement.Def as CraftDef, this.data.CraftQueueElement.Requirements, this.customTooltipTarget, uicraftStatusInfoWidgetData);
		};
		this.outputItem.GamepadNavigationItem.group = 1;
		this.craftProgress.transform.parent.gameObject.SetActive(this.data.CraftQueueElement.IsStarted);
		if (this.data.CraftQueueElement.IsStarted)
		{
			this.UpdateProgress();
		}
		this.HideSelection();
		this.UpdateTalentIcon();
		this.UpdateCount();
		this.UpdateStatusIcon(this.data.CraftQueueElement.CraftStatus);
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x00136A1E File Offset: 0x00134C1E
	public override void Hide()
	{
		this.outputItem.Flush(true);
		Action action = this.onHide;
		if (action != null)
		{
			action();
		}
		this.craftCountHold.Reset();
		this.UnsubscribeFromCraftQueueElementEvents();
		base.Hide();
	}

	// Token: 0x06004108 RID: 16648 RVA: 0x00136A54 File Offset: 0x00134C54
	public void HandleCountChange()
	{
		this.UpdateCount();
		if (this.data.CraftQueueElement.Count == 0)
		{
			Action<CraftElementBase> onRemoveFromQueuePressed = this.data.OnRemoveFromQueuePressed;
			if (onRemoveFromQueuePressed == null)
			{
				return;
			}
			onRemoveFromQueuePressed(this.data.CraftQueueElement);
		}
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x00136A8E File Offset: 0x00134C8E
	public void UpdateCount()
	{
		this.outputItem.OnMultiplierChange(this.data.CraftQueueElement.Count, false);
	}

	// Token: 0x0600410A RID: 16650 RVA: 0x00136AAC File Offset: 0x00134CAC
	public void UpdateStatusIcon(CraftStatus craftStartStatus)
	{
		if (craftStartStatus == CraftStatus.DoesntHaveRequiredTool)
		{
			this.outputItem.UpdateStatusIcon(craftStartStatus, this.data.CraftQueueElement.ParamsData.RequiredToolType);
			return;
		}
		this.outputItem.UpdateStatusIcon(craftStartStatus, ItemType.None);
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x00136AE4 File Offset: 0x00134CE4
	private string DefineDescriptionForCraftStatus()
	{
		CraftStatus craftStatus = this.data.CraftQueueElement.CraftStatus;
		if (craftStatus != CraftStatus.NotEnoughResources)
		{
			switch (craftStatus)
			{
			case CraftStatus.DoesntHaveRequiredTool:
				return LLBase.L("ui_craft_status_DoesntHaveRequiredTool");
			case CraftStatus.NotEnoughSpaceInWgo:
			case CraftStatus.NotEnoughSpaceInMultiInventory:
				return LLBase.L("ui_craft_status_NotEnoughSpaceInWgo");
			case CraftStatus.NotEnoughMastery:
				return LLBase.L("ui_craft_status_NotEnoughMastery");
			}
			return LLBase.L("ui_craft_status_NotEnoughResources");
		}
		return LLBase.L("ui_craft_status_NotEnoughResources");
	}

	// Token: 0x0600410C RID: 16652 RVA: 0x00136B60 File Offset: 0x00134D60
	public void UpdateTalentIcon()
	{
		if (this.data.CraftQueueElement.ParamsData.MasteryValue < this.data.CraftQueueElement.ParamsData.MasteryLock)
		{
			this.talentIcon.Draw(this.data.CraftQueueElement.ParamsData.TalentDef, this.data.CraftQueueElement.ParamsData.MasteryLock, false, this.data.CraftQueueElement.Def.isStarCraft || this.data.CraftQueueElement.Def.isAutopsyCraft || this.data.CraftQueueElement.Def.isPocketExtractCraft);
			return;
		}
		this.talentIcon.Hide();
	}

	// Token: 0x0600410D RID: 16653 RVA: 0x00136C28 File Offset: 0x00134E28
	public void UpdateQueueButtons()
	{
		int num = this.data.CraftQueue.IndexOf(this.data.CraftQueueElement);
		bool flag = num == 0;
		bool flag2 = num == this.data.CraftQueue.Count - 1;
		bool isStarted = this.data.CraftQueueElement.IsStarted;
		bool flag3 = num == 1 && !this.data.CraftQueue[num - 1].IsStarted;
		this.queueUpButton.gameObject.SetActive(true);
		this.queueUpButton.interactable = !flag && (num != 1 || flag3);
		this.queueDownButton.gameObject.SetActive(true);
		this.queueDownButton.interactable = !flag2 && (!flag || !isStarted);
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x00136CF8 File Offset: 0x00134EF8
	private void UnsubscribeFromCraftQueueElementEvents()
	{
		if (this.subscribedToCraftQueueElementEvents)
		{
			this.data.CraftQueueElement.OnStatusChanged -= this.UpdateStatusIcon;
			this.data.CraftQueueElement.OnCountChanged -= this.UpdateCount;
			this.data.CraftQueueElement.OnProgressChanged -= this.UpdateProgress;
			this.subscribedToCraftQueueElementEvents = false;
		}
	}

	// Token: 0x0600410F RID: 16655 RVA: 0x00136D68 File Offset: 0x00134F68
	private void SubscribeToCraftQueueElementEvents()
	{
		if (!this.subscribedToCraftQueueElementEvents)
		{
			this.data.CraftQueueElement.OnStatusChanged += this.UpdateStatusIcon;
			this.data.CraftQueueElement.OnCountChanged += this.UpdateCount;
			this.data.CraftQueueElement.OnProgressChanged += this.UpdateProgress;
			this.subscribedToCraftQueueElementEvents = true;
		}
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x00136DD8 File Offset: 0x00134FD8
	public void OnOver()
	{
		Action action = this.onOver;
		if (action != null)
		{
			action();
		}
		this.ShowSelection();
	}

	// Token: 0x06004111 RID: 16657 RVA: 0x00136DF1 File Offset: 0x00134FF1
	private void OnOut()
	{
		Action action = this.onOut;
		if (action != null)
		{
			action();
		}
		this.HideSelection();
	}

	// Token: 0x06004112 RID: 16658 RVA: 0x00136E0A File Offset: 0x0013500A
	private void OnPress()
	{
		Action action = this.onPress;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06004113 RID: 16659 RVA: 0x00136E1C File Offset: 0x0013501C
	private void ShowSelection()
	{
		this.plusCraftButton.interactable = !this.data.CraftQueueElement.IsStarted && !this.data.IsMulticraftDisabled;
		this.plusCraftButton.gameObject.SetActive(true);
		this.minusCraftButton.interactable = !this.data.CraftQueueElement.IsStarted;
		this.minusCraftButton.gameObject.SetActive(true);
		this.selectionFrame.gameObject.SetActive(true);
		this.UpdateQueueButtons();
	}

	// Token: 0x06004114 RID: 16660 RVA: 0x00136EB0 File Offset: 0x001350B0
	private void HideSelection()
	{
		this.infCraftButton.gameObject.SetActive(false);
		this.minusCraftButton.gameObject.SetActive(false);
		this.plusCraftButton.gameObject.SetActive(false);
		this.selectionFrame.gameObject.SetActive(false);
		this.queueUpButton.gameObject.SetActive(false);
		this.queueDownButton.gameObject.SetActive(false);
	}

	// Token: 0x06004115 RID: 16661 RVA: 0x00136F24 File Offset: 0x00135124
	public void ChangeCount(int delta)
	{
		if (delta == 0 || this.data == null)
		{
			return;
		}
		if (delta > 0)
		{
			if (this.plusCraftButton == null || !this.plusCraftButton.interactable)
			{
				return;
			}
			this.data.AddCount(delta);
			this.UpdateCount();
			return;
		}
		else
		{
			if (this.minusCraftButton == null || !this.minusCraftButton.interactable)
			{
				return;
			}
			this.data.AddCount(delta);
			this.HandleCountChange();
			return;
		}
	}

	// Token: 0x06004116 RID: 16662 RVA: 0x00136F9E File Offset: 0x0013519E
	private void Update()
	{
		if (this.data == null)
		{
			this.craftCountHold.Reset();
			return;
		}
		this.craftCountHold.Tick(HoldRepeatValueChanger.GetPointerHoldDirection(this.plusCraftButton, this.minusCraftButton), new Action<int>(this.ChangeCount));
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x00136FDC File Offset: 0x001351DC
	private void OnInfCraftButtonPressed()
	{
		Action action = this.onInfCraftButtonPressed;
		if (action != null)
		{
			action();
		}
		this.UpdateCount();
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x00136FF5 File Offset: 0x001351F5
	private void OnQueueUpButtonPressed()
	{
		if (!this.queueUpButton.interactable)
		{
			return;
		}
		Action<CraftElementBase> action = this.onQueueUpButtonPressed;
		if (action == null)
		{
			return;
		}
		action(this.data.CraftQueueElement);
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x00137020 File Offset: 0x00135220
	private void OnQueueDownButtonPressed()
	{
		if (!this.queueDownButton.interactable)
		{
			return;
		}
		Action<CraftElementBase> action = this.onQueueDownButtonPressed;
		if (action == null)
		{
			return;
		}
		action(this.data.CraftQueueElement);
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x0013704B File Offset: 0x0013524B
	private void UpdateProgress()
	{
		this.craftProgress.fillAmount = this.data.CraftQueueElement.ProgressTimeNormalized;
		this.craftProgressRed.fillAmount = this.data.CraftQueueElement.ProgressTimeNormalizedFailed;
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040032EB RID: 13035
	private Action onPress;

	// Token: 0x040032EC RID: 13036
	private Action onOver;

	// Token: 0x040032ED RID: 13037
	private Action onOut;

	// Token: 0x040032EE RID: 13038
	private Action onPressPlusQueue;

	// Token: 0x040032EF RID: 13039
	private Action onPressMinusQueue;

	// Token: 0x040032F0 RID: 13040
	private Action onInfCraftButtonPressed;

	// Token: 0x040032F1 RID: 13041
	private Action<CraftElementBase> onQueueUpButtonPressed;

	// Token: 0x040032F2 RID: 13042
	private Action<CraftElementBase> onQueueDownButtonPressed;

	// Token: 0x040032F3 RID: 13043
	private Action onHide;

	// Token: 0x040032F4 RID: 13044
	[SerializeField]
	private UIItemCell outputItem;

	// Token: 0x040032F5 RID: 13045
	[SerializeField]
	private LazyButton infCraftButton;

	// Token: 0x040032F6 RID: 13046
	[SerializeField]
	private LazyButton minusCraftButton;

	// Token: 0x040032F7 RID: 13047
	[SerializeField]
	private LazyButton plusCraftButton;

	// Token: 0x040032F8 RID: 13048
	[SerializeField]
	private Image selectionFrame;

	// Token: 0x040032F9 RID: 13049
	[SerializeField]
	private UITalentIcon talentIcon;

	// Token: 0x040032FA RID: 13050
	[SerializeField]
	private LazyButton queueUpButton;

	// Token: 0x040032FB RID: 13051
	[SerializeField]
	private LazyButton queueDownButton;

	// Token: 0x040032FC RID: 13052
	[SerializeField]
	private Image craftProgress;

	// Token: 0x040032FD RID: 13053
	[SerializeField]
	private Image craftProgressRed;

	// Token: 0x040032FE RID: 13054
	[SerializeField]
	private RectTransform customTooltipTarget;

	// Token: 0x040032FF RID: 13055
	private LazyButton widgetButton;

	// Token: 0x04003300 RID: 13056
	private bool subscribedToCraftQueueElementEvents;

	// Token: 0x04003301 RID: 13057
	private readonly HoldRepeatValueChanger craftCountHold = new HoldRepeatValueChanger();
}
