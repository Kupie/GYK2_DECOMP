using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200091B RID: 2331
public class TalentExpProgressWidget : LazyWidget<TalentExpProgressWidgetData>
{
	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06003D75 RID: 15733 RVA: 0x00125AC6 File Offset: 0x00123CC6
	public string PointIconId
	{
		get
		{
			TalentExpProgressWidget.TalentViewData talentViewData = this.currentViewData;
			if (talentViewData == null)
			{
				return null;
			}
			return talentViewData.pointIconId;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06003D76 RID: 15734 RVA: 0x00125AD9 File Offset: 0x00123CD9
	public bool HasPendingBarReset
	{
		get
		{
			return this.isResettingFullBar || this.pendingFillAnimations > 0;
		}
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x00125AEE File Offset: 0x00123CEE
	public override void Init()
	{
		base.Init();
		this.barElementPrefab.gameObject.SetActive(false);
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x00125B07 File Offset: 0x00123D07
	public override void Hide()
	{
		this.ResetFillAnimationState();
		base.Hide();
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x00125B18 File Offset: 0x00123D18
	public override void Redraw()
	{
		base.Redraw();
		this.ResetFillAnimationState();
		this.currentViewData = this.viewDatas.Find((TalentExpProgressWidget.TalentViewData d) => d.talentId == this.data.TalentId);
		this.RebuildBarVisuals(this.data.CurExp, this.data.TotalExp);
		this.SetPointsLabel(this.data.TalentExpPoints);
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00125B7B File Offset: 0x00123D7B
	public bool TryGetFlyTarget(out Vector3 worldPosition)
	{
		return this.TryGetFlyTarget(0, out worldPosition);
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x00125B88 File Offset: 0x00123D88
	public bool TryGetFlyTarget(int upcomingOffset, out Vector3 worldPosition)
	{
		this.RebuildBarsLayout();
		int num = (this.isAnimatingFill ? this.displayedFilledCount : ((this.data != null) ? this.data.CurExp : 0));
		int num2 = (this.isAnimatingFill ? this.displayedTotal : ((this.data != null) ? this.data.TotalExp : this.shownBarElements.Count));
		int num3 = num + upcomingOffset;
		if (num2 > 0 && num3 >= num2)
		{
			return TalentExpProgressWidget.TryGetRectCenter(this.GetBarsParent(), out worldPosition);
		}
		if (num3 >= 0 && num3 < this.shownBarElements.Count)
		{
			return TalentExpProgressWidget.TryGetRectCenter(this.shownBarElements[num3].transform as RectTransform, out worldPosition);
		}
		return TalentExpProgressWidget.TryGetRectCenter(this.GetBarsParent(), out worldPosition);
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x00125C50 File Offset: 0x00123E50
	private static bool TryGetRectCenter(RectTransform rect, out Vector3 worldPosition)
	{
		worldPosition = default(Vector3);
		if (rect == null)
		{
			return false;
		}
		worldPosition = rect.TransformPoint(rect.rect.center);
		return true;
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x00125C8F File Offset: 0x00123E8F
	private RectTransform GetBarsParent()
	{
		if (!(this.barElementPrefab != null))
		{
			return null;
		}
		return this.barElementPrefab.rectTransform.parent as RectTransform;
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x00125CB8 File Offset: 0x00123EB8
	private void RebuildBarsLayout()
	{
		RectTransform barsParent = this.GetBarsParent();
		if (barsParent != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(barsParent);
		}
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x00125CDB File Offset: 0x00123EDB
	public void PrepareFillAnimation()
	{
		this.isAnimatingFill = true;
		this.displayedFilledCount = this.data.CurExp;
		this.displayedTotal = this.data.TotalExp;
		this.displayedPoints = this.data.TalentExpPoints;
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00125D17 File Offset: 0x00123F17
	public void SetFillAnimationTarget(TalentExpProgressWidgetData targetData)
	{
		this.animationTargetData = targetData;
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x00125D20 File Offset: 0x00123F20
	public void ApplyArrivedExpPoint(Action onApplied = null)
	{
		TalentExpProgressWidget.<>c__DisplayClass35_0 CS$<>8__locals1 = new TalentExpProgressWidget.<>c__DisplayClass35_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.onApplied = onApplied;
		if (!this.isAnimatingFill || this.displayedTotal <= 0)
		{
			Action onApplied2 = CS$<>8__locals1.onApplied;
			if (onApplied2 == null)
			{
				return;
			}
			onApplied2();
			return;
		}
		else if (this.isResettingFullBar || (this.displayedFilledCount >= this.displayedTotal && this.pendingFillAnimations > 0))
		{
			this.queuedFillsDuringReset++;
			Action onApplied3 = CS$<>8__locals1.onApplied;
			if (onApplied3 == null)
			{
				return;
			}
			onApplied3();
			return;
		}
		else
		{
			if (this.displayedFilledCount >= this.displayedTotal)
			{
				if (this.animationTargetData == null)
				{
					Action onApplied4 = CS$<>8__locals1.onApplied;
					if (onApplied4 == null)
					{
						return;
					}
					onApplied4();
					return;
				}
				else
				{
					this.displayedFilledCount = 0;
					this.displayedTotal = this.animationTargetData.TotalExp;
					this.RebuildBarVisuals(this.displayedFilledCount, this.displayedTotal);
				}
			}
			int num = this.displayedFilledCount;
			this.displayedFilledCount++;
			if (num >= 0 && num < this.shownBarElements.Count)
			{
				this.pendingFillAnimations++;
				this.shownBarElements[num].PlayFillFromCenter(this.activeColor, this.barFillFromCenterDuration, delegate
				{
					CS$<>8__locals1.<>4__this.NotifyFillAnimationFinished();
					if (!CS$<>8__locals1.<>4__this.isAnimatingFill)
					{
						return;
					}
					base.<ApplyArrivedExpPoint>g__AfterFillAppeared|0();
				}, new Action(this.NotifyFillAnimationFinished));
				return;
			}
			CS$<>8__locals1.<ApplyArrivedExpPoint>g__AfterFillAppeared|0();
			return;
		}
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x00125E63 File Offset: 0x00124063
	private void NotifyFillAnimationFinished()
	{
		this.pendingFillAnimations = Mathf.Max(0, this.pendingFillAnimations - 1);
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x00125E7C File Offset: 0x0012407C
	public void ResetFillAnimationState()
	{
		this.KillBarResetTween();
		this.isAnimatingFill = false;
		this.isResettingFullBar = false;
		this.queuedFillsDuringReset = 0;
		this.pendingFillAnimations = 0;
		this.animationTargetData = null;
		for (int i = 0; i < this.shownBarElements.Count; i++)
		{
			TalentExpProgressWidgetBar talentExpProgressWidgetBar = this.shownBarElements[i];
			if (talentExpProgressWidgetBar != null)
			{
				talentExpProgressWidgetBar.StopFillAnimation();
			}
		}
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x00125EE0 File Offset: 0x001240E0
	private void PlayFullBarResetThenUpdatePoints(Action onComplete)
	{
		this.isResettingFullBar = true;
		int pointsToShow = this.displayedPoints;
		this.KillBarResetTween();
		this.barResetTween = DOVirtual.DelayedCall(this.pointsLabelDelayAfterBarReset, delegate
		{
			this.barResetTween = null;
			if (!this.isAnimatingFill)
			{
				return;
			}
			this.displayedFilledCount = 0;
			this.displayedTotal = ((this.animationTargetData != null) ? this.animationTargetData.TotalExp : this.displayedTotal);
			this.RebuildBarVisuals(this.displayedFilledCount, this.displayedTotal);
			this.SetPointsLabel(pointsToShow);
			this.isResettingFullBar = false;
			this.ApplyQueuedFills(onComplete);
		}, true).SetLink(base.gameObject, LinkBehaviour.KillOnDisable);
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x00125F44 File Offset: 0x00124144
	private void ApplyQueuedFills(Action onComplete)
	{
		int num = this.queuedFillsDuringReset;
		this.queuedFillsDuringReset = 0;
		if (num == 0)
		{
			if (onComplete != null)
			{
				onComplete();
			}
			return;
		}
		for (int i = 0; i < num; i++)
		{
			this.ApplyArrivedExpPoint(onComplete);
		}
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x00125F80 File Offset: 0x00124180
	private void KillBarResetTween()
	{
		Tween tween = this.barResetTween;
		this.barResetTween = null;
		if (tween != null && tween.IsActive())
		{
			tween.Kill(false);
		}
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00125FB0 File Offset: 0x001241B0
	private void RebuildBarVisuals(int filledCount, int totalCount)
	{
		foreach (TalentExpProgressWidgetBar talentExpProgressWidgetBar in this.shownBarElements)
		{
			if (talentExpProgressWidgetBar != null)
			{
				talentExpProgressWidgetBar.StopFillAnimation();
			}
			UIPrefabsPooler.Instance.ReleaseElementToPool<TalentExpProgressWidgetBar>(talentExpProgressWidgetBar);
		}
		this.shownBarElements.Clear();
		for (int i = 0; i < filledCount; i++)
		{
			this.shownBarElements.Add(this.CreateBarElement(this.activeColor));
		}
		for (int j = 0; j < totalCount - filledCount; j++)
		{
			this.shownBarElements.Add(this.CreateBarElement(this.inactiveColor));
		}
		for (int k = 0; k < this.shownBarElements.Count; k++)
		{
			this.shownBarElements[k].transform.SetSiblingIndex(k);
		}
		this.RebuildBarsLayout();
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x0012609C File Offset: 0x0012429C
	private TalentExpProgressWidgetBar CreateBarElement(Color color)
	{
		TalentExpProgressWidgetBar elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<TalentExpProgressWidgetBar>(this.barElementPrefab.transform.parent);
		elementFromPool.ResetVisual(color);
		elementFromPool.image.sprite = this.currentViewData.barSprite;
		elementFromPool.gameObject.SetActive(true);
		return elementFromPool;
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x001260EC File Offset: 0x001242EC
	private void SetPointsLabel(int points)
	{
		this.talentExpPointsLabel.text = string.Format("{0}{1}", this.currentViewData.pointIconId.FontIcon(), points);
		if (points > 0)
		{
			this.expPointStyleComponent.SetTextStyle(this.hasPointsStyle);
			return;
		}
		this.expPointStyleComponent.SetTextStyle(this.noPointsStyle);
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003045 RID: 12357
	[SerializeField]
	private Image barElementPrefab;

	// Token: 0x04003046 RID: 12358
	[SerializeField]
	private Color activeColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04003047 RID: 12359
	[SerializeField]
	private Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04003048 RID: 12360
	[SerializeField]
	private TextMeshProUGUI talentExpPointsLabel;

	// Token: 0x04003049 RID: 12361
	[SerializeField]
	private TextStyleComponent expPointStyleComponent;

	// Token: 0x0400304A RID: 12362
	[SerializeField]
	private TextStyle hasPointsStyle;

	// Token: 0x0400304B RID: 12363
	[SerializeField]
	private TextStyle noPointsStyle;

	// Token: 0x0400304C RID: 12364
	[SerializeField]
	private List<TalentExpProgressWidget.TalentViewData> viewDatas = new List<TalentExpProgressWidget.TalentViewData>();

	// Token: 0x0400304D RID: 12365
	[SerializeField]
	private float pointsLabelDelayAfterBarReset = 0.5f;

	// Token: 0x0400304E RID: 12366
	[SerializeField]
	private float barFillFromCenterDuration = 0.16f;

	// Token: 0x0400304F RID: 12367
	private TalentExpProgressWidget.TalentViewData currentViewData;

	// Token: 0x04003050 RID: 12368
	private List<TalentExpProgressWidgetBar> shownBarElements = new List<TalentExpProgressWidgetBar>();

	// Token: 0x04003051 RID: 12369
	private bool isAnimatingFill;

	// Token: 0x04003052 RID: 12370
	private bool isResettingFullBar;

	// Token: 0x04003053 RID: 12371
	private int displayedFilledCount;

	// Token: 0x04003054 RID: 12372
	private int displayedTotal;

	// Token: 0x04003055 RID: 12373
	private int displayedPoints;

	// Token: 0x04003056 RID: 12374
	private int queuedFillsDuringReset;

	// Token: 0x04003057 RID: 12375
	private int pendingFillAnimations;

	// Token: 0x04003058 RID: 12376
	private TalentExpProgressWidgetData animationTargetData;

	// Token: 0x04003059 RID: 12377
	private Tween barResetTween;

	// Token: 0x0200091C RID: 2332
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x0400305A RID: 12378
		public Sprite barSprite;

		// Token: 0x0400305B RID: 12379
		public string talentId;

		// Token: 0x0400305C RID: 12380
		public string pointIconId;
	}
}
