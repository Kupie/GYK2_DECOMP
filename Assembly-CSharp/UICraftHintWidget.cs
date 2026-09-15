using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007D9 RID: 2009
public class UICraftHintWidget : LazyWidget<UICraftHintWidgetData>, IBubbleLayoutAlwaysActive, IDelayedUIHide
{
	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x060033B5 RID: 13237 RVA: 0x00028294 File Offset: 0x00026494
	public bool ShouldDelayHide
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x000F9568 File Offset: 0x000F7768
	public override void Draw(UICraftHintWidgetData data)
	{
		if (this.isCompletionAnimationPlaying)
		{
			return;
		}
		this.ResetCompletionState();
		if (this.subscribedCraftComponent != null && this.subscribedCraftComponent != data.CraftComponent)
		{
			this.UnsubscribeFromDataChanges();
			this.zombieProgressBar.DOKill(false);
		}
		base.Draw(data);
		this.RedrawCraftHint();
		this.SubscribeToDataChanges();
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x000F95C0 File Offset: 0x000F77C0
	public override void CustomUpdate()
	{
		if (this.data == null)
		{
			return;
		}
		this.UpdateCellsFillStatus();
		this.TryPlayCompletionAnimation(false);
		this.UpdateQuality();
		this.UpdateAutoCraftTickProgress();
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x000F95E8 File Offset: 0x000F77E8
	public override void Hide()
	{
		if (this.isCompletionAnimationPlaying)
		{
			this.isCompletionHideRequested = true;
			return;
		}
		if (this.HasPendingCompletionAnimation() && this.TryPlayCompletionAnimation(true))
		{
			this.isCompletionHideRequested = true;
			return;
		}
		this.UnsubscribeFromDataChanges();
		this.zombieProgressBar.DOKill(false);
		this.KillCompletionAnimation();
		this.HideCells();
		base.Hide();
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x000F9643 File Offset: 0x000F7843
	private void OnDisable()
	{
		if (this.isCompletionAnimationPlaying)
		{
			this.ForceCancelDelayedHide();
		}
		this.UnsubscribeFromDataChanges();
		this.zombieProgressBar.DOKill(false);
		this.KillCompletionAnimation();
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x000F966C File Offset: 0x000F786C
	public void AddHideAfterDelayCallback(Action callback)
	{
		if (this.isCompletionAnimationPlaying)
		{
			this.isCompletionHideRequested = true;
			this.hideAfterDelayCallbacks.Add(callback);
			return;
		}
		if (this.HasPendingCompletionAnimation() && this.TryPlayCompletionAnimation(true))
		{
			this.isCompletionHideRequested = true;
			this.hideAfterDelayCallbacks.Add(callback);
			return;
		}
		if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x000F96C4 File Offset: 0x000F78C4
	public void ForceCancelDelayedHide()
	{
		Sequence sequence = this.completionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.completionSequence = null;
		this.CleanupCompletionCells();
		this.ReleaseCompletionHold();
		this.isCompletionAnimationPlaying = false;
		this.isCompletionHideRequested = false;
		this.isCompletionAnimationPlayedForPreFinish = false;
		this.hideAfterDelayCallbacks.Clear();
		if (this.canvasGroup != null)
		{
			this.canvasGroup.alpha = 1f;
		}
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x000F9734 File Offset: 0x000F7934
	private void SubscribeToDataChanges()
	{
		CraftComponent craftComponent = this.data.CraftComponent;
		if (this.subscribedToDataChanges && this.subscribedCraftComponent == craftComponent)
		{
			return;
		}
		this.UnsubscribeFromDataChanges();
		craftComponent.OnStatusChanged += this.RedrawCraftHint;
		craftComponent.OnCurCraftIndexUpdate += this.RedrawCraftHint;
		craftComponent.OnCraftAddedToQueue += this.RedrawCraftHint;
		craftComponent.OnCraftRemovedFromQueue += this.RedrawCraftHint;
		craftComponent.OnZombieSubTicksChanged += this.UpdateZombieProgressWithAnimation;
		this.subscribedCraftComponent = craftComponent;
		this.subscribedToDataChanges = true;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x000F97D0 File Offset: 0x000F79D0
	private void UnsubscribeFromDataChanges()
	{
		if (!this.subscribedToDataChanges || this.subscribedCraftComponent == null)
		{
			return;
		}
		this.subscribedCraftComponent.OnStatusChanged -= this.RedrawCraftHint;
		this.subscribedCraftComponent.OnCurCraftIndexUpdate -= this.RedrawCraftHint;
		this.subscribedCraftComponent.OnCraftAddedToQueue -= this.RedrawCraftHint;
		this.subscribedCraftComponent.OnCraftRemovedFromQueue -= this.RedrawCraftHint;
		this.subscribedCraftComponent.OnZombieSubTicksChanged -= this.UpdateZombieProgressWithAnimation;
		this.subscribedCraftComponent = null;
		this.subscribedToDataChanges = false;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x000F9870 File Offset: 0x000F7A70
	private void UpdateItemSize(bool big)
	{
		if (big)
		{
			this.layoutElement.minWidth = this.bigLayoutSize.x;
			this.layoutElement.minHeight = this.bigLayoutSize.y;
			this.rectTransform.sizeDelta = this.bigLayoutSize;
			return;
		}
		this.layoutElement.minWidth = this.defaultLayoutSize.x;
		this.layoutElement.minHeight = this.defaultLayoutSize.y;
		this.rectTransform.sizeDelta = this.defaultLayoutSize;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x000F98FC File Offset: 0x000F7AFC
	private void UpdateStatusIcon(CraftStatus craftStartStatus)
	{
		ZombieWgoData zombieWgoData = this.data.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			if (zombieWgoData.CrafterCurrentOrder != null && (this.data.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft || this.data.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp))
			{
				this.craftResultItem.UpdateStatusIcon(CraftStatus.NotEnoughSpaceInMultiInventory, ItemType.None);
				return;
			}
			if (craftStartStatus == CraftStatus.DoesntHaveRequiredTool)
			{
				this.craftResultItem.UpdateStatusIcon(craftStartStatus, this.data.CraftElement.ParamsData.RequiredToolType);
				return;
			}
			if (this.TryUpdateZombieNotEnoughMasteryStatusIcon(zombieWgoData))
			{
				return;
			}
			if (zombieWgoData.CrafterCurrentOrder != null)
			{
				this.craftResultItem.UpdateStatusIcon(zombieWgoData.CrafterCurrentOrder);
				return;
			}
		}
		if (craftStartStatus == CraftStatus.DoesntHaveRequiredTool)
		{
			this.craftResultItem.UpdateStatusIcon(craftStartStatus, this.data.CraftElement.ParamsData.RequiredToolType);
			return;
		}
		this.craftResultItem.UpdateStatusIcon(craftStartStatus, ItemType.None);
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x000F99DC File Offset: 0x000F7BDC
	private bool TryUpdateZombieNotEnoughMasteryStatusIcon(ZombieWgoData zombieWgoData)
	{
		CraftComponentStatus status = this.data.CraftComponent.Status;
		if (status == CraftComponentStatus.ReadyToFinishAutoCraft || status == CraftComponentStatus.WaitingForWorkerPickUp || status == CraftComponentStatus.WaitingForOutputDrop)
		{
			return false;
		}
		WgoData wgoData = this.data.CraftComponent.CraftableObject as WgoData;
		if (wgoData == null || this.data.CraftComponent.CurrentCraftElement == null)
		{
			return false;
		}
		if (zombieWgoData.CrafterIsEnoughMastery(wgoData))
		{
			return false;
		}
		Sprite craftStatusIcon = CraftStatusIconHelper.GetCraftStatusIcon(CraftStatus.NotEnoughMastery, ItemType.None, wgoData.Definition.talent);
		if (craftStatusIcon == null)
		{
			return false;
		}
		this.craftResultItem.UpdateStatusIcon(craftStatusIcon, null);
		return true;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x000F9A78 File Offset: 0x000F7C78
	private void TryUpdatePlantingDigStatusIcon()
	{
		if (!this.data.IsPlantingCraft || !this.craftResultItem.gameObject.activeSelf)
		{
			return;
		}
		if (this.craftResultItem.StatusIcon != null && this.craftResultItem.StatusIcon.gameObject.activeSelf)
		{
			return;
		}
		Sprite plantingDigStatusIcon = CraftStatusIconHelper.GetPlantingDigStatusIcon();
		if (plantingDigStatusIcon == null)
		{
			return;
		}
		this.craftResultItem.UpdateStatusIcon(plantingDigStatusIcon, new Vector2?(new Vector2(0f, -1f)));
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x000F9B00 File Offset: 0x000F7D00
	private void HideCells()
	{
		this.progressBarWidget.Hide();
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x000F9B10 File Offset: 0x000F7D10
	private bool TryPlayCompletionAnimation(bool refreshCellsBeforeStart = false)
	{
		if (this.isCompletionAnimationPlaying || this.data == null || !base.gameObject.activeInHierarchy)
		{
			return false;
		}
		CraftComponent craftComponent = this.data.CraftComponent;
		CraftElementBase currentCraftElement = craftComponent.CurrentCraftElement;
		if (!craftComponent.HasPreFinishUpdate)
		{
			this.isCompletionAnimationPlayedForPreFinish = false;
			this.completionAnimationPlayedElement = null;
			return false;
		}
		if (!this.CanPlayCompletionAnimation(currentCraftElement))
		{
			return false;
		}
		if (this.isCompletionAnimationPlayedForPreFinish || this.completionAnimationPlayedElement == currentCraftElement || !this.AreCellsVisible())
		{
			return false;
		}
		if (refreshCellsBeforeStart)
		{
			this.UpdateCellsFillStatus();
		}
		List<RectTransform> visibleSuccessCells = this.GetVisibleSuccessCells();
		if (visibleSuccessCells.Count == 0)
		{
			return false;
		}
		this.EnsureCanvasGroup();
		craftComponent.AddPreFinishHold();
		this.isCompletionHoldAdded = true;
		this.isCompletionAnimationPlaying = true;
		this.isCompletionAnimationPlayedForPreFinish = true;
		this.completionAnimationPlayedElement = currentCraftElement;
		this.PlayCompletionAnimation(visibleSuccessCells);
		return true;
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x00028294 File Offset: 0x00026494
	private bool CanPlayCompletionAnimation(CraftElementBase craftElement)
	{
		return false;
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x000F9BD8 File Offset: 0x000F7DD8
	private bool HasPendingCompletionAnimation()
	{
		if (this.data == null || !base.gameObject.activeInHierarchy)
		{
			return false;
		}
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		return this.data.CraftComponent.HasPreFinishUpdate && this.CanPlayCompletionAnimation(currentCraftElement) && !this.isCompletionAnimationPlayedForPreFinish && this.completionAnimationPlayedElement != currentCraftElement && this.AreCellsVisible();
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x000F9C44 File Offset: 0x000F7E44
	private List<RectTransform> GetVisibleSuccessCells()
	{
		List<RectTransform> list = new List<RectTransform>();
		RectTransform greenFillRect = this.progressBarWidget.GreenFillRect;
		if (greenFillRect != null && greenFillRect.gameObject.activeInHierarchy)
		{
			list.Add(greenFillRect);
		}
		return list;
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x000F9C84 File Offset: 0x000F7E84
	private void PlayCompletionAnimation(List<RectTransform> successCells)
	{
		Sequence sequence = this.completionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.completionSequence = DOTween.Sequence();
		RectTransform rectTransform = ((this.completionProgressCellsParent != null) ? this.completionProgressCellsParent : (base.transform.parent as RectTransform));
		if (rectTransform == null)
		{
			rectTransform = base.transform as RectTransform;
		}
		RectTransform rectTransform2 = this.craftResultItem.Icon.transform as RectTransform;
		this.canvasGroup.alpha = 1f;
		foreach (RectTransform rectTransform3 in successCells)
		{
			UICraftHintCompletionProgressCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftHintCompletionProgressCell>(rectTransform);
			if (!(elementFromPool == null))
			{
				RectTransform rectTransform4 = elementFromPool.RectTransform;
				rectTransform4.SetAsLastSibling();
				elementFromPool.ShowOver(rectTransform3, rectTransform);
				this.completionProgressCells.Add(elementFromPool);
				Vector2 vector = this.GetLocalCenter(rectTransform2, rectTransform) + global::UnityEngine.Random.insideUnitCircle * this.completionCellTargetRandomPixels;
				float num = this.completionCellFlyDuration * global::UnityEngine.Random.Range(1f - this.completionCellFlyDurationRandomFactor, 1f + this.completionCellFlyDurationRandomFactor);
				float num2 = this.completionCellFadeInDuration + global::UnityEngine.Random.Range(0f, this.completionCellFlyStartRandomDelay);
				this.completionSequence.Insert(0f, elementFromPool.CanvasGroup.DOFade(1f, this.completionCellFadeInDuration));
				this.completionSequence.Insert(num2, rectTransform4.DOAnchorPos(vector, num, false).SetEase(Ease.InCubic));
				this.completionSequence.Insert(num2 + num * this.completionCellFadeOutStart, elementFromPool.CanvasGroup.DOFade(0f, num * (1f - this.completionCellFadeOutStart)));
			}
		}
		this.completionSequence.Insert(this.completionCellFadeInDuration, this.canvasGroup.DOFade(0f, this.completionWidgetFadeOutDuration));
		this.completionSequence.OnComplete(new TweenCallback(this.CompleteCompletionAnimation));
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x000F9EBC File Offset: 0x000F80BC
	private Vector2 GetLocalCenter(RectTransform rect, RectTransform parent)
	{
		Vector3 vector = rect.TransformPoint(rect.rect.center);
		return parent.InverseTransformPoint(vector);
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x000F9EF0 File Offset: 0x000F80F0
	private void CompleteCompletionAnimation()
	{
		this.ReleaseCompletionHold();
		this.CleanupCompletionCells();
		this.isCompletionAnimationPlaying = false;
		if (this.isCompletionHideRequested)
		{
			this.isCompletionHideRequested = false;
			this.UnsubscribeFromDataChanges();
			this.zombieProgressBar.DOKill(false);
			this.HideCells();
			this.InvokeHideAfterDelayCallbacks();
			base.Hide();
			return;
		}
		this.hideAfterDelayCallbacks.Clear();
		this.RedrawCraftHint();
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x000F9F56 File Offset: 0x000F8156
	private void ResetCompletionState()
	{
		if (this.isCompletionAnimationPlaying)
		{
			return;
		}
		this.KillCompletionAnimation();
		this.EnsureCanvasGroup();
		this.canvasGroup.alpha = 1f;
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x000F9F80 File Offset: 0x000F8180
	private void KillCompletionAnimation()
	{
		Sequence sequence = this.completionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.completionSequence = null;
		this.CleanupCompletionCells();
		this.ReleaseCompletionHold();
		this.isCompletionAnimationPlaying = false;
		this.isCompletionHideRequested = false;
		this.isCompletionAnimationPlayedForPreFinish = false;
		this.hideAfterDelayCallbacks.Clear();
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x000F9FD4 File Offset: 0x000F81D4
	private void CleanupCompletionCells()
	{
		foreach (UICraftHintCompletionProgressCell uicraftHintCompletionProgressCell in this.completionProgressCells)
		{
			if (uicraftHintCompletionProgressCell != null)
			{
				UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftHintCompletionProgressCell>(uicraftHintCompletionProgressCell);
			}
		}
		this.completionProgressCells.Clear();
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x000FA040 File Offset: 0x000F8240
	private void ReleaseCompletionHold()
	{
		if (!this.isCompletionHoldAdded)
		{
			return;
		}
		this.isCompletionHoldAdded = false;
		UICraftHintWidgetData data = this.data;
		if (data == null)
		{
			return;
		}
		data.CraftComponent.ReleasePreFinishHold();
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x000FA068 File Offset: 0x000F8268
	private void InvokeHideAfterDelayCallbacks()
	{
		List<Action> list = new List<Action>(this.hideAfterDelayCallbacks);
		this.hideAfterDelayCallbacks.Clear();
		foreach (Action action in list)
		{
			if (action != null)
			{
				action();
			}
		}
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x000FA0D0 File Offset: 0x000F82D0
	private void EnsureCanvasGroup()
	{
		if (this.canvasGroup != null)
		{
			return;
		}
		if (!base.TryGetComponent<CanvasGroup>(out this.canvasGroup))
		{
			this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		}
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x000FA100 File Offset: 0x000F8300
	private void ReDrawCells()
	{
		if (!this.AreCellsVisible())
		{
			this.HideCells();
			return;
		}
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		if (currentCraftElement == null)
		{
			return;
		}
		this.progressBarWidget.Apply(currentCraftElement.TotalProgressTicks, currentCraftElement.SucceededProgressTicks, currentCraftElement.FailedProgressTicks, currentCraftElement.Def as CraftDef);
		this.progessCellContainer.RefreshContentFitter();
		LazySingleton<UICraftHintWidgetForceUpdateCanvasesScheduler>.Instance.RequestUpdate();
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x000FA170 File Offset: 0x000F8370
	private void UpdateCellsFillStatus()
	{
		if (!this.AreCellsVisible())
		{
			this.HideCells();
			return;
		}
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		if (currentCraftElement == null)
		{
			return;
		}
		this.progressBarWidget.Apply(currentCraftElement.TotalProgressTicks, currentCraftElement.SucceededProgressTicks, currentCraftElement.FailedProgressTicks, currentCraftElement.Def as CraftDef);
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x000FA1C9 File Offset: 0x000F83C9
	private void UpdateQuality()
	{
		this.craftResultItem.UpdateQualityIcon(this.showingElement.GetCurrentQuality());
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x000FA1E1 File Offset: 0x000F83E1
	private void RedrawCraftHint(CraftComponentStatus craftStartStatus)
	{
		this.RedrawCraftHint();
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x000FA1E1 File Offset: 0x000F83E1
	private void RedrawCraftHint(CraftElementBase craftElementBase)
	{
		this.RedrawCraftHint();
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x000FA1EC File Offset: 0x000F83EC
	private void RedrawCraftHint()
	{
		if ((this.data.CraftComponent.IsStarted && this.data.CraftComponent.CurrentCraftElement == null) || this.data.CraftComponent.CraftElementsQueue.Count == 0 || this.data.CraftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing)
		{
			this.Hide();
			return;
		}
		this.isItemBig = false;
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		if (currentCraftElement == null)
		{
			this.Hide();
			return;
		}
		bool flag = false;
		CraftElementSurvey craftElementSurvey = currentCraftElement as CraftElementSurvey;
		OutputPreview outputPreview;
		if (craftElementSurvey != null)
		{
			outputPreview = craftElementSurvey.GetSelectedItemOutputPreview();
		}
		else
		{
			outputPreview = currentCraftElement.Def.GetOutputPreview(this.data.CraftComponent.CraftableObject as WgoData);
			CraftDef craftDef = currentCraftElement.Def as CraftDef;
			flag = craftDef != null && craftDef.isObjDestroyCraft;
		}
		if (outputPreview == null)
		{
			this.Hide();
			return;
		}
		int count = outputPreview.count;
		outputPreview.count = currentCraftElement.Count * count;
		if (!flag && this.data.CraftComponent.CraftsIn.Count == 1 && this.data.CraftComponent.CraftElementsQueue.Count == 2)
		{
			outputPreview.count += this.data.CraftComponent.CraftElementsQueue[1].Count * count;
		}
		if (currentCraftElement.Def.id.StartsWith("pocket_extract_item") && currentCraftElement.CustomItems != null && currentCraftElement.CustomItems.Count > 0)
		{
			outputPreview.itemId = currentCraftElement.CustomItems[0].id;
			outputPreview.count = currentCraftElement.CustomItems[0].Count;
			outputPreview.customIconId = string.Empty;
		}
		this.showingElement = this.data.CraftComponent.CurrentCraftElement;
		this.craftResultItem.gameObject.SetActive(true);
		int num = (this.data.IsPlantingCraft ? (-1) : currentCraftElement.GetCurrentQuality());
		this.craftResultItem.DrawCraftOutput(outputPreview, num, currentCraftElement.CraftStatus, currentCraftElement.ParamsData.RequiredToolType, 1, ItemRelatedWidgetState.NotSet, false);
		this.isItemBig = outputPreview.IsBigItemOutput;
		this.UpdateItemSize(this.isItemBig);
		if (this.ShouldShowZombieProgressBar())
		{
			this.zombieProgressBar.transform.parent.gameObject.SetActive(true);
			if (this.data.CraftComponent.IsAutoCraftable)
			{
				this.zombieProgressBar.DOKill(false);
				this.zombieProgressBar.value = this.data.CraftComponent.AutoCraftTickProgressNormalized;
			}
			else
			{
				this.UpdateZombieProgress(this.data.CraftComponent.ZombieSubTicks, true);
			}
		}
		else
		{
			this.zombieProgressBar.DOKill(false);
			this.zombieProgressBar.transform.parent.gameObject.SetActive(false);
		}
		if (currentCraftElement.Def.id == "sawmill_wood_zombie_craft" || currentCraftElement.Def.id == "mine_zombie_craft" || currentCraftElement.Def.id == "sand_zombie_craft" || currentCraftElement.Def.id == "clay_zombie_craft" || currentCraftElement.Def.id == "carrier_stone_zombie_craft" || currentCraftElement.Def.id == "carrier_marble_zombie_craft")
		{
			ZombieWgoData zombieWgoData = this.data.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				if (!zombieWgoData.HasToolForWork(zombieWgoData.AttachedWgoData, currentCraftElement.Def))
				{
					this.craftResultItem.gameObject.SetActive(true);
					this.craftResultItem.UpdateStatusIcon(CraftStatus.DoesntHaveRequiredTool, this.data.CraftElement.ParamsData.RequiredToolType);
					goto IL_0432;
				}
				if (this.TryUpdateZombieNotEnoughMasteryStatusIcon(zombieWgoData))
				{
					this.craftResultItem.gameObject.SetActive(true);
					goto IL_0432;
				}
				if (this.data.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp)
				{
					this.craftResultItem.gameObject.SetActive(true);
					this.craftResultItem.UpdateStatusIcon(LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_wait", null), null);
					goto IL_0432;
				}
				goto IL_0432;
			}
		}
		this.UpdateStatusIcon(currentCraftElement.CraftStatus);
		IL_0432:
		this.TryUpdatePlantingDigStatusIcon();
		this.UpdateQuality();
		this.ReDrawCells();
		this.craftResultItem.SetNativeSizeForIcon();
		this.UpdateItemSize(this.isItemBig);
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x000FA654 File Offset: 0x000F8854
	private bool AreCellsVisible()
	{
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		return currentCraftElement != null && currentCraftElement.IsStarted && this.data.CraftComponent.Status != CraftComponentStatus.ReadyToFinishAutoCraft && this.data.CraftComponent.Status != CraftComponentStatus.WaitingForWorkerPickUp && this.data.CraftComponent.Status != CraftComponentStatus.WaitingForOutputDrop;
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x000FA6BC File Offset: 0x000F88BC
	private bool ShouldShowZombieProgressBar()
	{
		if (this.data.CraftElement == null || !this.data.CraftElement.IsStarted)
		{
			return false;
		}
		CraftComponentStatus status = this.data.CraftComponent.Status;
		return status != CraftComponentStatus.ReadyToFinishAutoCraft && status != CraftComponentStatus.WaitingForWorkerPickUp && status != CraftComponentStatus.WaitingForOutputDrop && (this.data.CraftComponent.IsAutoCraftable || this.data.Worker is ZombieWgoData || this.data.CraftComponent.ZombieSubTicks > 0);
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x000FA744 File Offset: 0x000F8944
	private void UpdateAutoCraftTickProgress()
	{
		if (!this.data.CraftComponent.IsAutoCraftable || !this.zombieProgressBar.transform.parent.gameObject.activeSelf)
		{
			return;
		}
		this.zombieProgressBar.value = this.data.CraftComponent.AutoCraftTickProgressNormalized;
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x000FA79C File Offset: 0x000F899C
	private void UpdateZombieProgressWithAnimation(int progress)
	{
		if (this.data.CraftComponent.IsAutoCraftable)
		{
			return;
		}
		float num = (float)progress / (float)ConstDef.Get("zombie_craft_sub_ticks_count").IntValue;
		if (num > this.zombieProgressBar.value)
		{
			this.zombieProgressBar.DOKill(false);
			this.zombieProgressBar.DOValue(num, 0.25f, false);
			return;
		}
		this.zombieProgressBar.DOKill(false);
		this.zombieProgressBar.value = num;
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x000FA818 File Offset: 0x000F8A18
	private void UpdateZombieProgress(int progress, bool instant)
	{
		if (instant)
		{
			this.zombieProgressBar.DOKill(false);
			float num = (float)progress / (float)ConstDef.Get("zombie_craft_sub_ticks_count").IntValue;
			this.zombieProgressBar.value = num;
			return;
		}
		this.UpdateZombieProgressWithAnimation(progress);
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400293F RID: 10559
	[SerializeField]
	private UIItemCell craftResultItem;

	// Token: 0x04002940 RID: 10560
	[SerializeField]
	private RectTransform progessCellContainer;

	// Token: 0x04002941 RID: 10561
	[SerializeField]
	private ProgressBarWiget_Simplified progressBarWidget;

	// Token: 0x04002942 RID: 10562
	[SerializeField]
	private Slider zombieProgressBar;

	// Token: 0x04002943 RID: 10563
	[SerializeField]
	private Vector2 defaultLayoutSize;

	// Token: 0x04002944 RID: 10564
	[SerializeField]
	private Vector2 bigLayoutSize;

	// Token: 0x04002945 RID: 10565
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x04002946 RID: 10566
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04002947 RID: 10567
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04002948 RID: 10568
	[SerializeField]
	private RectTransform completionProgressCellsParent;

	// Token: 0x04002949 RID: 10569
	[SerializeField]
	private float completionCellFadeInDuration = 0.1f;

	// Token: 0x0400294A RID: 10570
	[SerializeField]
	private float completionWidgetFadeOutDuration = 0.15f;

	// Token: 0x0400294B RID: 10571
	[SerializeField]
	private float completionCellFlyDuration = 0.45f;

	// Token: 0x0400294C RID: 10572
	[SerializeField]
	private float completionCellFlyDurationRandomFactor = 0.15f;

	// Token: 0x0400294D RID: 10573
	[SerializeField]
	private float completionCellFlyStartRandomDelay = 0.05f;

	// Token: 0x0400294E RID: 10574
	[SerializeField]
	private float completionCellTargetRandomPixels = 5f;

	// Token: 0x0400294F RID: 10575
	[SerializeField]
	[Range(0f, 1f)]
	private float completionCellFadeOutStart = 0.7f;

	// Token: 0x04002950 RID: 10576
	private readonly List<UICraftHintCompletionProgressCell> completionProgressCells = new List<UICraftHintCompletionProgressCell>();

	// Token: 0x04002951 RID: 10577
	private readonly List<Action> hideAfterDelayCallbacks = new List<Action>();

	// Token: 0x04002952 RID: 10578
	private bool subscribedToDataChanges;

	// Token: 0x04002953 RID: 10579
	private CraftComponent subscribedCraftComponent;

	// Token: 0x04002954 RID: 10580
	private CraftElementBase showingElement;

	// Token: 0x04002955 RID: 10581
	private float durationTime;

	// Token: 0x04002956 RID: 10582
	private bool isItemBig;

	// Token: 0x04002957 RID: 10583
	private Sequence completionSequence;

	// Token: 0x04002958 RID: 10584
	private bool isCompletionAnimationPlaying;

	// Token: 0x04002959 RID: 10585
	private bool isCompletionHoldAdded;

	// Token: 0x0400295A RID: 10586
	private bool isCompletionHideRequested;

	// Token: 0x0400295B RID: 10587
	private bool isCompletionAnimationPlayedForPreFinish;

	// Token: 0x0400295C RID: 10588
	private CraftElementBase completionAnimationPlayedElement;
}
