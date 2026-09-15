using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000847 RID: 2119
public class UITooltip : UIBasicBubble, ILazyGUIElement
{
	// Token: 0x06003622 RID: 13858 RVA: 0x00103DF4 File Offset: 0x00101FF4
	public void Init()
	{
		UITooltip.instance = this;
		LazyWindowsStackController.OnWindowClosed += delegate(LazyWidgetBase _)
		{
			UITooltip.Hide();
		};
		LazyWindowsStackController.OnWindowOpened += delegate(LazyWidgetBase _)
		{
			UITooltip.Hide();
		};
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 700;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00103E78 File Offset: 0x00102078
	public static void Show(List<LazyWidgetDataBase> dataList, RectTransform target, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool follow = true, TooltipPlacementPriority placementPriority = TooltipPlacementPriority.TopRight)
	{
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(dataList, target, forceCornerPosition, follow, default(Vector2), placementPriority);
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x00103EA0 File Offset: 0x001020A0
	public static void Hide()
	{
		UITooltip.instance.DoHideAnimation();
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x00103EAC File Offset: 0x001020AC
	public static void HideImmediately()
	{
		UITooltip.instance.HideTooltip();
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x00103EB8 File Offset: 0x001020B8
	public static bool IsTooltipShowingAtTarget(RectTransform target)
	{
		return UITooltip.instance != null && UITooltip.instance.gameObject.activeSelf && UITooltip.instance.target == target;
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00103EEC File Offset: 0x001020EC
	private void ShowAtTargetAndCalculateOffsets(List<LazyWidgetDataBase> dataList, RectTransform target, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool follow = true, Vector2 appearOffset = default(Vector2), TooltipPlacementPriority placementPriority = TooltipPlacementPriority.TopRight)
	{
		this.appearOffset = appearOffset;
		this.placementPriority = placementPriority;
		Vector2 vector;
		Vector2 vector2;
		this.CalculateTargetOffsets(target, out vector, out vector2);
		this.ShowAtTarget(dataList, target, vector, vector2, forceCornerPosition, follow);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00103F24 File Offset: 0x00102124
	private void CalculateTargetOffsets(RectTransform target, out Vector2 targetOffset, out Vector2 targetAlternativeDownOffset)
	{
		target.GetWorldCorners(this.targetWorldCorners);
		float num = Mathf.Min(new float[]
		{
			this.targetWorldCorners[0].x,
			this.targetWorldCorners[1].x,
			this.targetWorldCorners[2].x,
			this.targetWorldCorners[3].x
		});
		float num2 = Mathf.Max(new float[]
		{
			this.targetWorldCorners[0].x,
			this.targetWorldCorners[1].x,
			this.targetWorldCorners[2].x,
			this.targetWorldCorners[3].x
		});
		float num3 = Mathf.Min(new float[]
		{
			this.targetWorldCorners[0].y,
			this.targetWorldCorners[1].y,
			this.targetWorldCorners[2].y,
			this.targetWorldCorners[3].y
		});
		float num4 = Mathf.Max(new float[]
		{
			this.targetWorldCorners[0].y,
			this.targetWorldCorners[1].y,
			this.targetWorldCorners[2].y,
			this.targetWorldCorners[3].y
		});
		Vector2 vector = new Vector2((num + num2) * 0.5f, num4);
		Vector2 vector2 = new Vector2((num + num2) * 0.5f, num3);
		targetOffset = vector - target.position + this.offsetAdd * LazyUI.ScaleFactor;
		if (this.appearOffset != Vector2.zero)
		{
			targetOffset += this.appearOffset * LazyUI.ScaleFactor;
		}
		targetAlternativeDownOffset = vector2 - vector + this.alternativeDownOffsetAdd * LazyUI.ScaleFactor;
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x00104154 File Offset: 0x00102354
	private void ShowAtTarget(List<LazyWidgetDataBase> dataList, RectTransform target, Vector2 offset, Vector2 alternativeDownOffset = default(Vector2), UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool follow = true)
	{
		if (target == null)
		{
			Debug.LogError("Null target for tooltip");
			return;
		}
		bool flag = !base.gameObject.activeSelf || this.target != target;
		this.ShowAtPosition(dataList, target.position + offset, alternativeDownOffset, forceCornerPosition);
		if (follow)
		{
			this.target = target;
			this.offset = offset;
			this.alternativeDownOffset = alternativeDownOffset;
		}
		else
		{
			this.target = null;
		}
		if (flag && base.gameObject.activeSelf)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x001041F4 File Offset: 0x001023F4
	private void ShowAtPosition(List<LazyWidgetDataBase> dataList, Vector2 screenPosition, Vector3 alternativeDownPosition = default(Vector3), UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto)
	{
		if (dataList.Count == 0)
		{
			return;
		}
		while (dataList.Count > 0 && dataList[dataList.Count - 1] is UITooltipSeparatorWidgetData)
		{
			dataList.RemoveAt(dataList.Count - 1);
		}
		this.target = null;
		this.widgetContainer.ConstructWidgets(dataList, null);
		base.gameObject.SetActive(true);
		base.transform.localScale = Vector3.one;
		Canvas.ForceUpdateCanvases();
		((RectTransform)base.transform).RefreshContentFitter();
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.RootTransform);
		if (alternativeDownPosition != default(Vector3))
		{
			if (forceCornerPosition - UIBasicBubble.ForceCornerPosition.TopCenter <= 2)
			{
				this.UpdatePositionAndCorner(screenPosition + alternativeDownPosition, forceCornerPosition);
			}
			else
			{
				this.UpdatePositionAndCorner(screenPosition, alternativeDownPosition);
			}
		}
		else
		{
			this.UpdatePositionAndCorner(screenPosition, forceCornerPosition);
		}
		this.DoAppearAnimation();
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x001042DC File Offset: 0x001024DC
	private void Update()
	{
		if (this.target != null)
		{
			this.CalculateTargetOffsets(this.target, out this.offset, out this.alternativeDownOffset);
			this.UpdatePositionAndCorner(this.target.position + this.offset, this.alternativeDownOffset);
		}
		this.widgetContainer.CustomUpdate();
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x00104348 File Offset: 0x00102548
	protected override void UpdatePositionAndCorner(Vector3 screenPos, Vector3 offsetForDownPosition)
	{
		Bounds currentScreenBounds = this.GetCurrentScreenBounds();
		Vector3 vector = screenPos + offsetForDownPosition;
		object obj = this.placementPriority == TooltipPlacementPriority.BottomRight || this.placementPriority == TooltipPlacementPriority.BottomLeft;
		bool flag = this.placementPriority == TooltipPlacementPriority.TopLeft || this.placementPriority == TooltipPlacementPriority.BottomLeft;
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection = (flag ? UIBasicBubble.BubbleCornerDirection.RightDown : UIBasicBubble.BubbleCornerDirection.LeftDown);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection2 = (flag ? UIBasicBubble.BubbleCornerDirection.LeftDown : UIBasicBubble.BubbleCornerDirection.RightDown);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection3 = (flag ? UIBasicBubble.BubbleCornerDirection.RightUp : UIBasicBubble.BubbleCornerDirection.LeftUp);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection4 = (flag ? UIBasicBubble.BubbleCornerDirection.LeftUp : UIBasicBubble.BubbleCornerDirection.RightUp);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection5 = this.PickBestHorizontalCorner(screenPos, bubbleCornerDirection, bubbleCornerDirection2, currentScreenBounds);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection6 = this.PickBestHorizontalCorner(vector, bubbleCornerDirection3, bubbleCornerDirection4, currentScreenBounds);
		object obj2 = obj;
		Vector3 vector2 = ((obj2 != null) ? vector : screenPos);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection7 = ((obj2 != null) ? bubbleCornerDirection6 : bubbleCornerDirection5);
		Vector3 vector3 = ((obj2 != null) ? screenPos : vector);
		UIBasicBubble.BubbleCornerDirection bubbleCornerDirection8 = ((obj2 != null) ? bubbleCornerDirection5 : bubbleCornerDirection6);
		if (this.IsCandidateInsideScreen(vector2, bubbleCornerDirection7, currentScreenBounds))
		{
			base.ApplyCornerIndex(vector2, Vector3.zero, bubbleCornerDirection7);
			return;
		}
		if (this.IsCandidateInsideScreen(vector3, bubbleCornerDirection8, currentScreenBounds))
		{
			base.ApplyCornerIndex(vector3, Vector3.zero, bubbleCornerDirection8);
			return;
		}
		float candidateVisibleArea = this.GetCandidateVisibleArea(vector2, bubbleCornerDirection7, currentScreenBounds);
		float candidateVisibleArea2 = this.GetCandidateVisibleArea(vector3, bubbleCornerDirection8, currentScreenBounds);
		base.ApplyCornerIndex((candidateVisibleArea >= candidateVisibleArea2) ? vector2 : vector3, Vector3.zero, (candidateVisibleArea >= candidateVisibleArea2) ? bubbleCornerDirection7 : bubbleCornerDirection8);
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00104468 File Offset: 0x00102668
	private UIBasicBubble.BubbleCornerDirection PickBestHorizontalCorner(Vector3 screenPos, UIBasicBubble.BubbleCornerDirection preferredCorner, UIBasicBubble.BubbleCornerDirection otherCorner, Bounds screenBounds)
	{
		if (this.IsCandidateInsideScreen(screenPos, preferredCorner, screenBounds))
		{
			return preferredCorner;
		}
		if (this.IsCandidateInsideScreen(screenPos, otherCorner, screenBounds))
		{
			return otherCorner;
		}
		float candidateVisibleArea = this.GetCandidateVisibleArea(screenPos, preferredCorner, screenBounds);
		float candidateVisibleArea2 = this.GetCandidateVisibleArea(screenPos, otherCorner, screenBounds);
		if (candidateVisibleArea < candidateVisibleArea2)
		{
			return otherCorner;
		}
		return preferredCorner;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x001044AC File Offset: 0x001026AC
	private bool IsCandidateInsideScreen(Vector3 screenPos, UIBasicBubble.BubbleCornerDirection corner, Bounds screenBounds)
	{
		Rect candidateRect = this.GetCandidateRect(screenPos, corner);
		return candidateRect.xMin >= screenBounds.min.x && candidateRect.xMax <= screenBounds.max.x && candidateRect.yMin >= screenBounds.min.y && candidateRect.yMax <= screenBounds.max.y;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x0010451C File Offset: 0x0010271C
	private float GetCandidateVisibleArea(Vector3 screenPos, UIBasicBubble.BubbleCornerDirection corner, Bounds screenBounds)
	{
		Rect candidateRect = this.GetCandidateRect(screenPos, corner);
		float num = Mathf.Max(0f, Mathf.Min(candidateRect.xMax, screenBounds.max.x) - Mathf.Max(candidateRect.xMin, screenBounds.min.x));
		float num2 = Mathf.Max(0f, Mathf.Min(candidateRect.yMax, screenBounds.max.y) - Mathf.Max(candidateRect.yMin, screenBounds.min.y));
		return num * num2;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x001045AC File Offset: 0x001027AC
	private Rect GetCandidateRect(Vector3 screenPos, UIBasicBubble.BubbleCornerDirection corner)
	{
		base.RootTransform.GetWorldCorners(this.bubbleWorldCorners);
		Rect rect = new Rect(this.bubbleWorldCorners[0], this.bubbleWorldCorners[2] - this.bubbleWorldCorners[0]);
		Vector2 vector = screenPos - (this.corners[(int)corner].rectTransform.position - base.RootTransform.position);
		rect.position += vector - base.RootTransform.position;
		return rect;
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x00104664 File Offset: 0x00102864
	private Bounds GetCurrentScreenBounds()
	{
		Rect safeArea = Screen.safeArea;
		return new Bounds(safeArea.center, safeArea.size);
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x00104694 File Offset: 0x00102894
	private void HideTooltip()
	{
		base.gameObject.SetActive(false);
		this.target = null;
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x001046AC File Offset: 0x001028AC
	private void DoAppearAnimation()
	{
		base.gameObject.SetActive(true);
		Sequence sequence = this.activeTweenSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.activeTweenSequence = DOTween.Sequence();
		this.canvasGroup.alpha = 0f;
		this.activeTweenSequence.Join(this.canvasGroup.DOFade(1f, 0f));
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00104714 File Offset: 0x00102914
	private void DoHideAnimation()
	{
		Sequence sequence = this.activeTweenSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.activeTweenSequence = DOTween.Sequence();
		this.activeTweenSequence.Join(this.canvasGroup.DOFade(0f, 0f));
		this.activeTweenSequence.AppendCallback(new TweenCallback(this.HideTooltip));
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x00104778 File Offset: 0x00102978
	public static void ShowSimpleInfo(Transform transform, string text, Vector2 appearOffset = default(Vector2), string header = null)
	{
		List<LazyWidgetDataBase> list = UITooltip.BuildSimpleTooltipWidgets(text, header);
		if (list.Count == 0)
		{
			return;
		}
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, appearOffset, TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x001047AC File Offset: 0x001029AC
	public static void ShowPerk(PerkDef perkDef, RectTransform target, Vector2 appearOffset = default(Vector2))
	{
		if (perkDef == null || target == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddPerkWidgets(list, perkDef, true, "");
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, target, UIBasicBubble.ForceCornerPosition.Auto, true, appearOffset, TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x001047EC File Offset: 0x001029EC
	private static List<LazyWidgetDataBase> BuildSimpleTooltipWidgets(string text, string header)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		string text2 = text ?? string.Empty;
		if (string.IsNullOrEmpty(header))
		{
			UITooltip.TrySplitHeaderAndBody(text2, out header, out text2);
		}
		if (!string.IsNullOrEmpty(header))
		{
			list.Add(new UITooltipTextWidgetData(UITooltip.ApplyBracketBold(header), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle));
			if (!string.IsNullOrEmpty(text2))
			{
				list.Add(new UITooltipSeparatorWidgetData());
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			TextAlignmentOptions textAlignmentOptions = (string.IsNullOrEmpty(header) ? TextAlignmentOptions.TopJustified : TextAlignmentOptions.Center);
			list.Add(new UITooltipTextWidgetData(UITooltip.ApplyBracketBold(text2), textAlignmentOptions, UITooltip.instance.descriptionTextStyle));
		}
		return list;
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00104894 File Offset: 0x00102A94
	private static bool TrySplitHeaderAndBody(string text, out string header, out string body)
	{
		header = null;
		body = text;
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		int num = text.IndexOf("\r\n\r\n", StringComparison.Ordinal);
		int num2 = 4;
		int num3 = text.IndexOf("\n\n", StringComparison.Ordinal);
		if (num3 >= 0 && (num < 0 || num3 < num))
		{
			num = num3;
			num2 = 2;
		}
		if (num <= 0)
		{
			return false;
		}
		header = text.Substring(0, num).Trim();
		body = text.Substring(num + num2).Trim();
		if (string.IsNullOrEmpty(header))
		{
			header = null;
			body = text;
			return false;
		}
		return true;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00104914 File Offset: 0x00102B14
	private static string ApplyBracketBold(string text)
	{
		if (string.IsNullOrEmpty(text) || UITooltip.instance.headerBoldTextStyle == null)
		{
			return text;
		}
		return UITooltip.instance.headerBoldTextStyle.ColorizeTags(text);
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x00104944 File Offset: 0x00102B44
	public static void ShowExtensionInfo(UICraftPreviewItemCell previewItemCell, string extensionId)
	{
		Transform transform = previewItemCell.transform;
		string text = LLBase.L("tt_extension");
		string text2 = LLBase.L(extensionId);
		UITooltip.ShowSimpleInfo(transform, text, default(Vector2), text2);
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x00104978 File Offset: 0x00102B78
	public static void ShowMultiAnswerIcon(UIMultiAnswerIcon multiAnswerIcon)
	{
		if (multiAnswerIcon == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list;
		if (multiAnswerIcon.VendorOrderDef != null)
		{
			list = new List<LazyWidgetDataBase>();
			ItemDef data = GameBalance.Me.GetData<ItemDef>(multiAnswerIcon.VendorOrderDef.itemId);
			string text = UITooltip.TryConstructHeaderWithPrefix(string.Format("[{0}]x{1}", data.GetHeader(), multiAnswerIcon.VendorOrderDef.count), LLBase.L("ui_order_header_tooltip"));
			list.Add(new UITooltipTextWidgetData(text, TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle));
			UITooltip.AddItemWidgets(list, data, false, "", true, false, null);
			UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, multiAnswerIcon.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
			return;
		}
		if (multiAnswerIcon.ItemCount == null || multiAnswerIcon.ItemCount.Def == null)
		{
			return;
		}
		list = new List<LazyWidgetDataBase>();
		UITooltip.AddItemWidgets(list, multiAnswerIcon.ItemCount.Def, true, "", false, false, null);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, multiAnswerIcon.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x00104A8C File Offset: 0x00102C8C
	public static void ShowOrderWidget(UIVendorOrderWidget orderWidget)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddItemWidgets(list, GameBalance.Me.GetData<ItemDef>(orderWidget.Data.VendorOrderData.Definition.itemId), true, "", false, false, null);
		UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(string.Format("{0}: {1}+{2}", LLBase.L("ui_vendor_order_reward_tooltip"), "happiness".FontIcon(), orderWidget.Data.VendorOrderData.Definition.happinessReward.EvaluateInt()), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
		list.Add(uitooltipTextWidgetData);
		if (orderWidget.Data.VendorOrderData.Definition.isUrgent)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData(LLBase.L("ui_vendor_order_urgent_tooltip") + ": " + "day_pride".FontIcon(), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
			list.Add(uitooltipTextWidgetData2);
		}
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, orderWidget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x00104B9C File Offset: 0x00102D9C
	public static void ShowItemCell(UIItemCell uiItemCell)
	{
		if (uiItemCell == null)
		{
			return;
		}
		if (uiItemCell.CustomTooltipShowAction != null)
		{
			Action<UIItemCell> customTooltipShowAction = uiItemCell.CustomTooltipShowAction;
			if (customTooltipShowAction == null)
			{
				return;
			}
			customTooltipShowAction(uiItemCell);
			return;
		}
		else
		{
			if (uiItemCell.DisplayingOutputPreview != null)
			{
				UITooltip.ShowCraftOutputCell(uiItemCell);
				return;
			}
			if (uiItemCell.DisplayingItem == null || string.IsNullOrEmpty(uiItemCell.DisplayingItem.id) || uiItemCell.DisplayingItem.id == "empty")
			{
				return;
			}
			List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
			UITooltip.AddItemWidgets(list, uiItemCell.DisplayingItem.Definition, true, "", false, false, null);
			if (!string.IsNullOrEmpty(uiItemCell.ExtraRedTooltipLocId))
			{
				if (list.Count > 0)
				{
					List<LazyWidgetDataBase> list2 = list;
					if (!(list2[list2.Count - 1] is UITooltipSeparatorWidgetData))
					{
						list.Add(new UITooltipSeparatorWidgetData());
					}
				}
				list.Add(new UITooltipTextWidgetData(LLBase.L(uiItemCell.ExtraRedTooltipLocId), TextAlignmentOptions.Center, UITooltip.instance.redEffectTextStyle));
			}
			UITooltip uitooltip = UITooltip.instance;
			List<LazyWidgetDataBase> list3 = list;
			RectTransform rectTransform = uiItemCell.transform as RectTransform;
			UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto;
			bool flag = true;
			TooltipPlacementPriority tooltipPlacementPriority = UITooltip.ResolvePlacementPriority(uiItemCell);
			uitooltip.ShowAtTargetAndCalculateOffsets(list3, rectTransform, forceCornerPosition, flag, default(Vector2), tooltipPlacementPriority);
			return;
		}
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x00104CB2 File Offset: 0x00102EB2
	private static TooltipPlacementPriority ResolvePlacementPriority(UIItemCell uiItemCell)
	{
		if (uiItemCell == null || !LazyInput.IsGamepadActive)
		{
			return TooltipPlacementPriority.TopRight;
		}
		return uiItemCell.TooltipPlacementPriority;
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x00104CCC File Offset: 0x00102ECC
	public static void ShowCraftOutputCell(UIItemCell uiItemCell)
	{
		if (uiItemCell == null || uiItemCell.DisplayingOutputPreview == null)
		{
			return;
		}
		if (AlchemyMixDef.IsUnknownMixResult(uiItemCell.DisplayingOutputPreview.craftId))
		{
			return;
		}
		ItemDef itemDef = null;
		CraftDefBase craftDefBase = GameBalance.GetCraftDef(uiItemCell.DisplayingOutputPreview.craftId);
		if (craftDefBase == null)
		{
			craftDefBase = GameBalance.GetAlchemyMixDef(uiItemCell.DisplayingOutputPreview.craftId);
		}
		using (List<ChanceOutputItem>.Enumerator enumerator = craftDefBase.outputItems.chanceOutputItems.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				ChanceOutputItem chanceOutputItem = enumerator.Current;
				if (chanceOutputItem.isStarGroup)
				{
					itemDef = GameBalance.Me.starGroupItemsCache[chanceOutputItem.id][0];
				}
				else
				{
					itemDef = GameBalance.Me.GetData<ItemDef>(chanceOutputItem.id);
				}
			}
		}
		if (itemDef == null)
		{
			foreach (GroupChanceOutputItem groupChanceOutputItem in GameBalance.GetCraftDef(uiItemCell.DisplayingOutputPreview.craftId).outputItems.groupChanceOutputItems)
			{
				using (List<ChanceOutputItem>.Enumerator enumerator = groupChanceOutputItem.chanceItems.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ChanceOutputItem chanceOutputItem2 = enumerator.Current;
						if (chanceOutputItem2.isStarGroup)
						{
							itemDef = GameBalance.Me.starGroupItemsCache[chanceOutputItem2.id][0];
						}
						else
						{
							itemDef = GameBalance.Me.GetData<ItemDef>(chanceOutputItem2.id);
						}
					}
				}
			}
		}
		if (itemDef == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		List<LazyWidgetDataBase> list2 = list;
		ItemDef itemDef2 = itemDef;
		bool flag = true;
		string text = "";
		bool flag2 = false;
		bool flag3 = false;
		CraftDef craftDef = craftDefBase as CraftDef;
		UITooltip.AddItemWidgets(list2, itemDef2, flag, text, flag2, flag3, (craftDef != null) ? craftDef.GetPossibleResultingItemDefs(true) : null);
		UITooltip uitooltip = UITooltip.instance;
		List<LazyWidgetDataBase> list3 = list;
		RectTransform rectTransform = uiItemCell.transform as RectTransform;
		UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto;
		bool flag4 = true;
		TooltipPlacementPriority tooltipPlacementPriority = UITooltip.ResolvePlacementPriority(uiItemCell);
		uitooltip.ShowAtTargetAndCalculateOffsets(list3, rectTransform, forceCornerPosition, flag4, default(Vector2), tooltipPlacementPriority);
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x00104ECC File Offset: 0x001030CC
	public static void ShowTalentLevelUpWidget(TalentLevelUpWidget widget)
	{
		if (widget == null || widget.Data == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddTalentLevelUpWidgets(list, widget.Data.Def);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, widget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x00104F24 File Offset: 0x00103124
	public static void ShowGardenFertilizerSlot(UIGardenBedSlot fertilizerSlot)
	{
		if (fertilizerSlot == null || fertilizerSlot.PerkData == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddPerkWidgets(list, fertilizerSlot.PerkData.Definition, true, "");
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, fertilizerSlot.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x00104F84 File Offset: 0x00103184
	public static void ShowPerkWidget(PerkWidget perkWidget)
	{
		if (perkWidget == null || perkWidget.PerkData == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddPerkWidgets(list, perkWidget.PerkData.Definition, true, "");
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, perkWidget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00104FE4 File Offset: 0x001031E4
	public static void ShowLinkedEntity(LinkedEntityWidget entityWidget)
	{
		if (entityWidget == null || entityWidget.WidgetData == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		string text = UITooltip.TryConstructHeaderWithPrefix(entityWidget.WidgetData.GetLinkedEntityHeader(), entityWidget.WidgetData.GetLinkedEntityPrefix());
		list.Add(new UITooltipTextWidgetData(text, TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle));
		switch (entityWidget.WidgetData.LinkedEntityType)
		{
		case LinkedEntityType.CraftDef:
		{
			string text2 = LLBase.L("ui_crafted_at") + ": ";
			string text3 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text2, false, true) ?? "";
			for (int i = 0; i < entityWidget.WidgetData.CraftDef.craftsIn.Count; i++)
			{
				if (i > 0)
				{
					text3 += ", ";
				}
				text3 += LLBase.L(entityWidget.WidgetData.CraftDef.craftsIn[i]);
			}
			bool flag = false;
			if (entityWidget.WidgetData.CraftDef.id.StartsWith("fake_"))
			{
				List<LazyWidgetDataBase> list2 = list;
				if (!(list2[list2.Count - 1] is UITooltipSeparatorWidgetData))
				{
					list.Add(new UITooltipSeparatorWidgetData());
				}
				UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(text3, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				list.Add(uitooltipTextWidgetData);
			}
			else
			{
				ItemDef itemDef = entityWidget.WidgetData.CraftDef.TryGetResultingItemDef(true);
				if (entityWidget.WidgetData.CraftDef.id.EndsWith("_boost"))
				{
					UITooltip.AddBoostCraftWidgets(list, entityWidget.WidgetData.CraftDef, "");
				}
				else
				{
					if (itemDef != null)
					{
						UITooltip.AddItemWidgets(list, itemDef, false, "", entityWidget.WidgetData.NotShowStudyWidgetInItemTooltips, entityWidget.WidgetData.ShowCraftedAtFromCraftDefInsteadOfItem, entityWidget.WidgetData.CraftDef.GetPossibleResultingItemDefs(true));
						flag = itemDef.type == ItemType.Preach;
					}
					else
					{
						string text4 = entityWidget.WidgetData.CraftDef.id + "_d";
						string text5 = LLBase.L(text4);
						if (text5 != text4)
						{
							UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData(text5, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
							list.Add(uitooltipTextWidgetData2);
						}
					}
					if (entityWidget.WidgetData.ShowCraftedAtFromCraftDefInsteadOfItem && entityWidget.WidgetData.CraftDef.craftsIn.Count > 0)
					{
						List<LazyWidgetDataBase> list3 = list;
						if (!(list3[list3.Count - 1] is UITooltipSeparatorWidgetData))
						{
							list.Add(new UITooltipSeparatorWidgetData());
						}
						UITooltipTextWidgetData uitooltipTextWidgetData3 = new UITooltipTextWidgetData(text3, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
						list.Add(uitooltipTextWidgetData3);
					}
				}
			}
			UITooltip.TryAddRequiredExtensionWidget(list, entityWidget.WidgetData.CraftDef);
			if (!flag)
			{
				UITooltip.TryAddNeedItemsWidget(list, entityWidget.WidgetData.CraftDef.needItems);
			}
			break;
		}
		case LinkedEntityType.ItemDef:
			UITooltip.AddItemWidgets(list, entityWidget.WidgetData.ItemDef, false, "", entityWidget.WidgetData.NotShowStudyWidgetInItemTooltips, entityWidget.WidgetData.ShowCraftedAtFromCraftDefInsteadOfItem, null);
			break;
		case LinkedEntityType.BuildingDef:
		{
			UITooltip.AddBuildingWidgets(list, entityWidget.WidgetData.BuildingDef, false, "");
			WGODef data = GameBalance.Me.GetData<WGODef>(entityWidget.WidgetData.BuildingDef.wgoId);
			if (data != null && GameBalance.Me.IsWorkbenchExtensionId(data.id))
			{
				string text6 = LLBase.L("ui_hint_extention_for") + " ";
				string text7 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text6, false, true) ?? "";
				for (int j = 0; j < GameBalance.Me.workbenchParentsByExtensionCache[data.id].Count; j++)
				{
					if (j > 0)
					{
						text7 += ", ";
					}
					text7 += LLBase.L(GameBalance.Me.workbenchParentsByExtensionCache[data.id][j].id);
				}
				List<LazyWidgetDataBase> list4 = list;
				if (!(list4[list4.Count - 1] is UITooltipSeparatorWidgetData))
				{
					list.Add(new UITooltipSeparatorWidgetData());
				}
				UITooltipTextWidgetData uitooltipTextWidgetData4 = new UITooltipTextWidgetData(text7, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				list.Add(uitooltipTextWidgetData4);
			}
			UITooltip.TryAddNeedItemsWidget(list, entityWidget.WidgetData.BuildingDef.needItems);
			break;
		}
		case LinkedEntityType.PerkDef:
			UITooltip.AddPerkWidgets(list, entityWidget.WidgetData.PerkDef, false, "");
			break;
		case LinkedEntityType.Item:
			UITooltip.AddItemWidgets(list, entityWidget.WidgetData.Item.Definition, false, "", entityWidget.WidgetData.NotShowStudyWidgetInItemTooltips, entityWidget.WidgetData.ShowCraftedAtFromCraftDefInsteadOfItem, null);
			break;
		case LinkedEntityType.GameRes:
		case LinkedEntityType.DayNumber:
			return;
		case LinkedEntityType.AlchemyFormula:
			UITooltip.AddItemWidgets(list, entityWidget.WidgetData.AlchemyFormulaDef.ItemDef, false, "", entityWidget.WidgetData.NotShowStudyWidgetInItemTooltips, true, null);
			if (entityWidget.WidgetData.AlchemyFormulaDef.craftsIn.Count > 0)
			{
				string text8 = LLBase.L("ui_crafted_at") + ": ";
				string text9 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text8, false, true) ?? "";
				for (int k = 0; k < entityWidget.WidgetData.AlchemyFormulaDef.craftsIn.Count; k++)
				{
					if (k > 0)
					{
						text9 += ", ";
					}
					text9 += LLBase.L(entityWidget.WidgetData.AlchemyFormulaDef.craftsIn[k]);
				}
				List<LazyWidgetDataBase> list5 = list;
				if (!(list5[list5.Count - 1] is UITooltipSeparatorWidgetData))
				{
					list.Add(new UITooltipSeparatorWidgetData());
				}
				UITooltipTextWidgetData uitooltipTextWidgetData5 = new UITooltipTextWidgetData(text9, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				list.Add(uitooltipTextWidgetData5);
			}
			break;
		case LinkedEntityType.Order:
			UITooltip.AddItemWidgets(list, entityWidget.WidgetData.ItemDef, false, "", entityWidget.WidgetData.NotShowStudyWidgetInItemTooltips, entityWidget.WidgetData.ShowCraftedAtFromCraftDefInsteadOfItem, null);
			break;
		case LinkedEntityType.TownBuildingDef:
			UITooltip.AddTownBuildingWidgets(list, entityWidget.WidgetData.TownBuildingDef, false);
			UITooltip.TryAddNeedItemsWidget(list, entityWidget.WidgetData.TownBuildingDef.needItems);
			break;
		default:
			Debug.LogError("Type:[LinkedEntityType] is not implemented for tooltip!!!");
			return;
		}
		if (list.Count > 1 && !(list[1] is UITooltipSeparatorWidgetData))
		{
			list.Insert(1, new UITooltipSeparatorWidgetData());
		}
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, entityWidget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x00105678 File Offset: 0x00103878
	public static void ShowCraftRequirementDescription(UICraftRequirementWidget craftRequirementWidget, string requirementId, CraftDefBase craftDef, List<PerkData> perks, Item toolForWork)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddCraftRequirementWidgets(list, requirementId, craftDef, perks, toolForWork);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, craftRequirementWidget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x001056B8 File Offset: 0x001038B8
	public static void ShowProgressTicksInfo(UIProgressCellsInfoWidget progressCellsInfoWidget, int masteryValue, int masteryLock, bool isStarCraft, TalentDef talentDef)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltipProgressTicksWidgetData uitooltipProgressTicksWidgetData = new UITooltipProgressTicksWidgetData(masteryValue, masteryLock, isStarCraft, talentDef);
		list.Add(uitooltipProgressTicksWidgetData);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, progressCellsInfoWidget.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x00105700 File Offset: 0x00103900
	public static void ShowProgressTickBonus(ProgressCell progressCell, PerkDef perkDefBonus = null, ItemDef itemDefBonus = null)
	{
		if (perkDefBonus == null && itemDefBonus == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltipProgressCellWidgetData uitooltipProgressCellWidgetData = new UITooltipProgressCellWidgetData(perkDefBonus, itemDefBonus);
		list.Add(uitooltipProgressCellWidgetData);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, progressCell.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x0010574C File Offset: 0x0010394C
	public static void ShowAlchemyBoostInfo(RectTransform target, CraftDef boost)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltip.AddBoostCraftWidgets(list, boost, "");
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, target, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x00105784 File Offset: 0x00103984
	public static void ShowCraftInfo(UIItemCell uiItemCell, WgoData wgoData, CraftDef craftDef, List<NeedItemData> customItems = null, RectTransform customTarget = null, UICraftStatusInfoWidgetData statusInfoWidgetData = null)
	{
		if (uiItemCell == null || uiItemCell.DisplayingOutputPreview == null)
		{
			return;
		}
		if (!craftDef.id.StartsWith("mix"))
		{
			List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
			bool flag = craftDef.addItemsToWgoOnFinish.chanceOutputItems.Count > 0 && craftDef.addItemsToWgoOnFinish.chanceOutputItems[0].id == "alchemy_flask";
			string text;
			if (flag)
			{
				text = string.Format("{0} {1}{2}", LLBase.L("ui_place"), "alchemy_flask".FontIcon(), craftDef.addItemsToWgoOnFinish.chanceOutputItems[0].count.EvaluateInt(wgoData));
			}
			else
			{
				BalanceBaseObject data = GameBalance.Me.GetData<TalentDef>(wgoData.Definition.talent);
				IWorker worker = wgoData.Worker;
				if (worker == null)
				{
					worker = MainGame.PlayerController;
				}
				bool flag2 = worker.GetMasteryLevelForTalentBranch(wgoData.Definition.talent, craftDef) >= craftDef.talentLock;
				string text2;
				if (craftDef.isStarCraft)
				{
					text2 = UITooltip.instance.headerBoldTextStyleGold.ApplyStyleToString(string.Format("1-{0}", craftDef.talentLock), false, true) ?? "";
				}
				else if (flag2)
				{
					text2 = UITooltip.instance.headerBoldTextStyle.ApplyStyleToString(craftDef.talentLock.ToString(), false, true) ?? "";
				}
				else
				{
					text2 = UITooltip.instance.headerBoldTextStyleNotEnough.ApplyStyleToString(craftDef.talentLock.ToString(), false, true) ?? "";
				}
				string text3 = (data.id.FontIcon() + " " + text2).NOBR();
				text = LLBase.L(craftDef.id) + " " + text3;
			}
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(text, TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			list.Add(uitooltipTextWidgetData);
			if (!flag)
			{
				ItemDef itemDef = craftDef.TryGetResultingItemDef(true);
				if (itemDef != null && itemDef.type != ItemType.Preach)
				{
					int count = list.Count;
					UITooltip.AddItemEffectWidgets(list, itemDef, craftDef.GetPossibleResultingItemDefs(true));
					if (list.Count > count)
					{
						list.Insert(count, new UITooltipSeparatorWidgetData());
					}
				}
			}
			UITooltipNeedsItemWidgetData uitooltipNeedsItemWidgetData;
			if (customItems != null)
			{
				uitooltipNeedsItemWidgetData = new UITooltipNeedsItemWidgetData(craftDef, wgoData.CraftComponent, customItems);
			}
			else
			{
				uitooltipNeedsItemWidgetData = new UITooltipNeedsItemWidgetData(craftDef, wgoData.CraftComponent, null);
			}
			if (uitooltipNeedsItemWidgetData.CraftItemCellsData.Count > 0)
			{
				List<LazyWidgetDataBase> list2 = list;
				if (!(list2[list2.Count - 1] is UITooltipSeparatorWidgetData))
				{
					list.Add(new UITooltipSeparatorWidgetData());
				}
				list.Add(uitooltipNeedsItemWidgetData);
			}
			if (statusInfoWidgetData != null)
			{
				list.Add(statusInfoWidgetData);
			}
			UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, (customTarget == null) ? (uiItemCell.transform as RectTransform) : customTarget, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
			return;
		}
		if (AlchemyMixDef.IsUnknownMixResult(craftDef.id))
		{
			return;
		}
		UITooltip.ShowMixCraftInfo(uiItemCell, wgoData, craftDef, customTarget);
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x00105A58 File Offset: 0x00103C58
	public static void ShowExhumeButtonWidget(LazyButton exhumeButton)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(LLBase.L("ui_cant_exhume") ?? "", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
		list.Add(uitooltipTextWidgetData);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, exhumeButton.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x00105ABC File Offset: 0x00103CBC
	public static void ShowResurrectionPrepareButtonWidget(LazyButton exhumeButton, string reason)
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(LLBase.L(reason) ?? "", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
		list.Add(uitooltipTextWidgetData);
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, exhumeButton.transform as RectTransform, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x00105B1C File Offset: 0x00103D1C
	private static void ShowMixCraftInfo(UIItemCell uiItemCell, WgoData wgoData, CraftDef craftDef, RectTransform customTarget = null)
	{
		if (uiItemCell == null || uiItemCell.DisplayingOutputPreview == null)
		{
			return;
		}
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		TalentDef data = GameBalance.Me.GetData<TalentDef>(wgoData.Definition.talent);
		string text = UITooltip.instance.headerBoldTextStyle.ApplyStyleToString(craftDef.talentLock.ToString(), false, true) ?? "";
		UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(string.Concat(new string[]
		{
			LLBase.L(craftDef.id),
			" ",
			data.id.FontIcon(),
			" ",
			text
		}), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
		list.Add(uitooltipTextWidgetData);
		list.Add(new UIMixInfoWidgetData(GameBalance.GetAlchemyMixDef(craftDef.id)));
		string text2 = string.Empty;
		foreach (NeedItemData needItemData in craftDef.needItems)
		{
			if (!needItemData.IsGroup && needItemData.ItemDef != null && needItemData.ItemDef.isFuel)
			{
				text2 += string.Format("{0}{1}, ", needItemData.id.FontIcon(), needItemData.GetCount(wgoData));
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			text2 = text2.Remove(text2.Length - 2, 2);
			list.Add(new UITooltipTextWidgetData(text2 ?? "", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
		}
		UITooltip.instance.ShowAtTargetAndCalculateOffsets(list, (customTarget == null) ? (uiItemCell.transform as RectTransform) : customTarget, UIBasicBubble.ForceCornerPosition.Auto, true, default(Vector2), TooltipPlacementPriority.TopRight);
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x00105CF0 File Offset: 0x00103EF0
	private static void TryAddNeedItemsWidget(List<LazyWidgetDataBase> widgetData, List<NeedItemData> needItems)
	{
		if (needItems == null || needItems.Count == 0)
		{
			return;
		}
		UITooltipNeedsItemWidgetData uitooltipNeedsItemWidgetData = new UITooltipNeedsItemWidgetData(needItems, MainGame.PlayerController.WorkerMultiInventory, null);
		if (uitooltipNeedsItemWidgetData.CraftItemCellsData.Count == 0)
		{
			return;
		}
		if (widgetData.Count > 0 && !(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
		{
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		widgetData.Add(uitooltipNeedsItemWidgetData);
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x00105D5C File Offset: 0x00103F5C
	private static void TryAddRequiredExtensionWidget(List<LazyWidgetDataBase> widgetData, CraftDef craftDef)
	{
		if (craftDef == null || string.IsNullOrEmpty(craftDef.extensionNeedId))
		{
			return;
		}
		string text = LLBase.L("required_extension") + " ";
		string text2 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text, false, true) + LLBase.L(craftDef.extensionNeedId);
		if (widgetData.Count > 0)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = widgetData[widgetData.Count - 1] as UITooltipTextWidgetData;
			if (uitooltipTextWidgetData != null && uitooltipTextWidgetData.Text.Contains(LLBase.L("ui_crafted_at")))
			{
				widgetData[widgetData.Count - 1] = new UITooltipTextWidgetData(uitooltipTextWidgetData.Text + "\n" + text2, uitooltipTextWidgetData.TextAlignmentOptions, uitooltipTextWidgetData.TextStyle);
				return;
			}
		}
		if (widgetData.Count > 0 && !(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
		{
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		widgetData.Add(new UITooltipTextWidgetData(text2, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle));
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x00105E60 File Offset: 0x00104060
	private static void AddBuildingWidgets(List<LazyWidgetDataBase> widgetData, BuildingDef buildingDef, bool addHeader = true, string headerPrefix = "")
	{
		if (addHeader)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(UITooltip.TryConstructHeaderWithPrefix(buildingDef.GetHeader(), headerPrefix), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
		}
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x00105E98 File Offset: 0x00104098
	private static void AddTownBuildingWidgets(List<LazyWidgetDataBase> widgetData, TownBuildingDef townBuildingDef, bool addHeader = true)
	{
		if (addHeader)
		{
			string text = UITooltip.TryConstructHeaderWithPrefix(townBuildingDef.GetHeader(), townBuildingDef.GetHeaderPrefix());
			widgetData.Add(new UITooltipTextWidgetData(text, TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle));
		}
		string text2 = townBuildingDef.id + "_d";
		string text3 = LLBase.L(text2);
		if (text3 != text2)
		{
			widgetData.Add(new UITooltipTextWidgetData(text3, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle));
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x00105F14 File Offset: 0x00104114
	private static void AddTalentLevelUpWidgets(List<LazyWidgetDataBase> widgetData, TalentLevelUpDef def)
	{
		if (!string.IsNullOrEmpty(def.linkedPerk))
		{
			PerkDef data = GameBalance.Me.GetData<PerkDef>(def.linkedPerk);
			if (data != null)
			{
				UITooltip.AddPerkWidgets(widgetData, data, true, "");
				return;
			}
		}
		else
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(string.Format("{0}: {1} +{2}", LLBase.L("ui_add_mastery"), def.talentId.FontIcon(), def.talentValueAdd), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
		}
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x00105F98 File Offset: 0x00104198
	private static void AddPerkWidgets(List<LazyWidgetDataBase> widgetData, PerkDef perkDef, bool addHeader = true, string headerPrefix = "")
	{
		if (addHeader)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(UITooltip.TryConstructHeaderWithPrefix(perkDef.GetHeader(), headerPrefix), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		UITooltip.TryAddDescriptionWidget(widgetData, perkDef.id);
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00105FE8 File Offset: 0x001041E8
	private static void AddBoostCraftWidgets(List<LazyWidgetDataBase> widgetData, CraftDef boost, string headerPrefix = "")
	{
		UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(UITooltip.TryConstructHeaderWithPrefix(LLBase.L(boost.id), headerPrefix), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
		widgetData.Add(uitooltipTextWidgetData);
		widgetData.Add(new UITooltipSeparatorWidgetData());
		UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData(boost.Description, TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
		widgetData.Add(uitooltipTextWidgetData2);
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00106050 File Offset: 0x00104250
	private static void AddItemEffectWidgets(List<LazyWidgetDataBase> widgetData, ItemDef itemDef, IReadOnlyList<ItemDef> qualityVariants = null)
	{
		if (itemDef == null)
		{
			return;
		}
		IReadOnlyList<ItemDef> readOnlyList = UITooltip.ResolveItemEffectVariants(itemDef, qualityVariants);
		itemDef = readOnlyList[0];
		int num = itemDef.quality;
		int num2 = itemDef.quality;
		int num3 = Mathf.Abs((int)itemDef.GetGameResOnUse("energy"));
		int num4 = num3;
		int num5 = itemDef.talentBonus;
		int num6 = itemDef.talentBonus;
		int num7 = ((itemDef.damage != null) ? itemDef.damage.EvaluateInt() : 0);
		int num8 = num7;
		int num9 = itemDef.redSkulls;
		int num10 = itemDef.redSkulls;
		int num11 = itemDef.whiteSkulls;
		int num12 = itemDef.whiteSkulls;
		int num13 = itemDef.bagSizeX;
		int num14 = itemDef.bagSizeX;
		int num15 = itemDef.bagSizeY;
		int num16 = itemDef.bagSizeY;
		int num17 = itemDef.talentValue;
		int num18 = itemDef.talentValue;
		int num19 = itemDef.whiteSkullsMinCollar;
		int num20 = itemDef.whiteSkullsMaxCollar;
		int num21 = itemDef.redSkullsMinCollar;
		int num22 = itemDef.redSkullsMaxCollar;
		bool flag = itemDef.whiteSkullsMaxCollar > itemDef.whiteSkullsMinCollar;
		bool flag2 = itemDef.redSkullsMaxCollar > itemDef.redSkullsMinCollar;
		Dictionary<string, Vector2Int> dictionary = new Dictionary<string, Vector2Int>();
		GameRes gameResOnUse = itemDef.GetGameResOnUse();
		for (int i = 0; i < gameResOnUse.List.Count; i++)
		{
			int num23 = (int)gameResOnUse.List[i].value;
			dictionary[gameResOnUse.List[i].type] = new Vector2Int(num23, num23);
		}
		for (int j = 1; j < readOnlyList.Count; j++)
		{
			ItemDef itemDef2 = readOnlyList[j];
			num = Math.Min(num, itemDef2.quality);
			num2 = Math.Max(num2, itemDef2.quality);
			int num24 = Mathf.Abs((int)itemDef2.GetGameResOnUse("energy"));
			num3 = Math.Min(num3, num24);
			num4 = Math.Max(num4, num24);
			num5 = Math.Min(num5, itemDef2.talentBonus);
			num6 = Math.Max(num6, itemDef2.talentBonus);
			int num25 = ((itemDef2.damage != null) ? itemDef2.damage.EvaluateInt() : 0);
			num7 = Math.Min(num7, num25);
			num8 = Math.Max(num8, num25);
			num9 = Math.Min(num9, itemDef2.redSkulls);
			num10 = Math.Max(num10, itemDef2.redSkulls);
			num11 = Math.Min(num11, itemDef2.whiteSkulls);
			num12 = Math.Max(num12, itemDef2.whiteSkulls);
			num13 = Math.Min(num13, itemDef2.bagSizeX);
			num14 = Math.Max(num14, itemDef2.bagSizeX);
			num15 = Math.Min(num15, itemDef2.bagSizeY);
			num16 = Math.Max(num16, itemDef2.bagSizeY);
			num17 = Math.Min(num17, itemDef2.talentValue);
			num18 = Math.Max(num18, itemDef2.talentValue);
			if (itemDef2.whiteSkullsMaxCollar > itemDef2.whiteSkullsMinCollar)
			{
				if (!flag)
				{
					num19 = itemDef2.whiteSkullsMinCollar;
					num20 = itemDef2.whiteSkullsMaxCollar;
					flag = true;
				}
				else
				{
					num19 = Math.Min(num19, itemDef2.whiteSkullsMinCollar);
					num20 = Math.Max(num20, itemDef2.whiteSkullsMaxCollar);
				}
			}
			if (itemDef2.redSkullsMaxCollar > itemDef2.redSkullsMinCollar)
			{
				if (!flag2)
				{
					num21 = itemDef2.redSkullsMinCollar;
					num22 = itemDef2.redSkullsMaxCollar;
					flag2 = true;
				}
				else
				{
					num21 = Math.Min(num21, itemDef2.redSkullsMinCollar);
					num22 = Math.Max(num22, itemDef2.redSkullsMaxCollar);
				}
			}
			GameRes gameResOnUse2 = itemDef2.GetGameResOnUse();
			for (int k = 0; k < gameResOnUse2.List.Count; k++)
			{
				string type = gameResOnUse2.List[k].type;
				int num26 = (int)gameResOnUse2.List[k].value;
				Vector2Int vector2Int;
				if (dictionary.TryGetValue(type, out vector2Int))
				{
					dictionary[type] = new Vector2Int(Math.Min(vector2Int.x, num26), Math.Max(vector2Int.y, num26));
				}
				else
				{
					dictionary[type] = new Vector2Int(num26, num26);
				}
			}
		}
		if (itemDef.itemGroupIds.Contains("bodypart"))
		{
			string skullsRangeAsString = ItemDef.GetSkullsRangeAsString(num9, num10, num11, num12, UITooltip.instance.redEffectTextStyle, UITooltip.instance.descriptionTextStyle);
			if (!string.IsNullOrEmpty(skullsRangeAsString))
			{
				UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(LLBase.L("ui_skulls") + ": " + skullsRangeAsString, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData);
			}
		}
		if (!string.IsNullOrEmpty(itemDef.qualityIcon) && num2 != 0)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData(string.Concat(new string[]
			{
				LLBase.L("icon_" + itemDef.qualityIcon),
				": ",
				itemDef.qualityIcon.FontIcon(),
				" ",
				UITooltip.FormatStyledIntRange(num, num2)
			}), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData2);
		}
		if (itemDef.isTool)
		{
			if (itemDef.HasGameResOnUse("energy"))
			{
				string text = (GameResDisplayConfig.GetConfigForRes("energy", GameResIconType.Common).iconName.FontIcon() + UITooltip.FormatStyledIntRange(num3, num4)).NOBR();
				UITooltipTextWidgetData uitooltipTextWidgetData3 = new UITooltipTextWidgetData(LLBase.L("ui_energy_cons") + ": " + text, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData3);
			}
			if (num6 > 0 && itemDef.talentIds.Count > 0)
			{
				UITooltipTextWidgetData uitooltipTextWidgetData4 = new UITooltipTextWidgetData(string.Concat(new string[]
				{
					LLBase.L("ui_mastery"),
					": +",
					itemDef.talentIds[0].FontIcon(),
					" ",
					UITooltip.FormatStyledIntRange(num5, num6)
				}), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData4);
			}
		}
		if (itemDef.isWeapon && (num7 != 0 || num8 != 0))
		{
			string text2 = "equip_icon_sword";
			ItemType type2 = itemDef.type;
			if (type2 != ItemType.Bow)
			{
				if (type2 == ItemType.Pike)
				{
					text2 = "squad_equip_icon-spear";
				}
			}
			else
			{
				text2 = "equip_icon_arrow";
			}
			UITooltipTextWidgetData uitooltipTextWidgetData5 = new UITooltipTextWidgetData(LLBase.L("ui_attack") + ": " + text2.FontIcon() + UITooltip.FormatStyledIntRange(num7, num8), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData5);
		}
		if (itemDef.type == ItemType.BodyArmor && num2 != 0)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData6 = new UITooltipTextWidgetData(LLBase.L("ui_defence") + ": " + "equip_icon_armor".FontIcon() + UITooltip.FormatStyledIntRange(num, num2), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData6);
		}
		if (itemDef.CanBeUsed)
		{
			List<PerkDef> perksOnUse = itemDef.GetPerksOnUse();
			bool flag3 = gameResOnUse.IsEmpty();
			if (!flag3 || perksOnUse.Count > 0)
			{
				string text3 = LLBase.L("ui_effect_on_use");
				if (!flag3 && !itemDef.isTool)
				{
					text3 += ": ";
					bool flag4 = true;
					for (int l = 0; l < gameResOnUse.List.Count; l++)
					{
						string type3 = gameResOnUse.List[l].type;
						Vector2Int vector2Int2 = dictionary[type3];
						GameResIconConfig configForRes = GameResDisplayConfig.GetConfigForRes(type3, GameResIconType.Common);
						string text4 = UITooltip.FormatOnUseResRange((configForRes != null) ? configForRes.iconName.FontIcon() : type3.FontIcon(), vector2Int2.x, vector2Int2.y, type3 == "insanity");
						if (!flag4)
						{
							text4 = " " + text4;
						}
						flag4 = false;
						text3 += text4;
					}
				}
				if (perksOnUse.Count > 0)
				{
					if (text3 != LLBase.L("ui_effect_on_use"))
					{
						text3 += ", ";
					}
					else
					{
						text3 += ": ";
					}
					string text5 = string.Empty;
					for (int m = 0; m < perksOnUse.Count; m++)
					{
						string text6 = LLBase.L(perksOnUse[m].id);
						string text7 = perksOnUse[m].id + "_d";
						string text8 = LLBase.L(text7);
						if (text8 != text7)
						{
							string text9 = UITooltip.instance.perkDescriptionTextStyle.ApplyStyleToString(text6, false, true);
							text5 = string.Concat(new string[] { text5, text9, " (", text8, ")" });
						}
						else
						{
							text5 += text6;
						}
						text3 += text5;
						if (m < perksOnUse.Count - 1)
						{
							text3 += ", ";
						}
					}
				}
				UITooltipTextWidgetData uitooltipTextWidgetData7 = new UITooltipTextWidgetData(text3, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData7);
			}
		}
		if (!string.IsNullOrEmpty(itemDef.bodyLinkedPerk))
		{
			PerkDef data = GameBalance.Me.GetData<PerkDef>(itemDef.bodyLinkedPerk);
			string text10 = LLBase.L("ui_body_linked_perk");
			text10 += ": ";
			string text11 = string.Empty;
			string text12 = LLBase.L(data.id);
			string text13 = data.id + "_d";
			string text14 = LLBase.L(text13);
			if (text14 != text13)
			{
				string text15 = UITooltip.instance.perkDescriptionTextStyle.ApplyStyleToString(text12, false, true);
				text11 = string.Concat(new string[] { text11, text15, " (", text14, ")" });
			}
			else
			{
				text11 += text12;
			}
			text10 += text11;
			UITooltipTextWidgetData uitooltipTextWidgetData8 = new UITooltipTextWidgetData(text10, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData8);
		}
		if (itemDef.type == ItemType.Collar)
		{
			if (flag)
			{
				string text16 = UITooltip.instance.descriptionTextStyle.ApplyStyleToString("skull".FontIcon() + UITooltip.FormatIntRange(num19, num20), false, true);
				UITooltipTextWidgetData uitooltipTextWidgetData9 = new UITooltipTextWidgetData(LLBase.L("ui_skulls_white") + ": " + text16, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData9);
			}
			if (flag2)
			{
				string text17 = UITooltip.instance.descriptionTextStyle.ApplyStyleToString("rskull".FontIcon() + UITooltip.FormatIntRange(num21, num22), false, true);
				UITooltipTextWidgetData uitooltipTextWidgetData10 = new UITooltipTextWidgetData(LLBase.L("ui_skulls_red") + ": " + text17, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData10);
			}
		}
		if (itemDef.type == ItemType.Embalm)
		{
			string skullsRangeAsString2 = ItemDef.GetSkullsRangeAsString(num9, num10, num11, num12, UITooltip.instance.redEffectTextStyle, UITooltip.instance.descriptionTextStyle);
			if (!string.IsNullOrEmpty(skullsRangeAsString2))
			{
				UITooltipTextWidgetData uitooltipTextWidgetData11 = new UITooltipTextWidgetData(LLBase.L("ui_embalming_effect") + ":\n" + skullsRangeAsString2, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData11);
			}
		}
		if (itemDef.type == ItemType.Bag)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData12 = new UITooltipTextWidgetData(string.Concat(new string[]
			{
				LLBase.L("ui_bag_size"),
				": ",
				UITooltip.FormatStyledIntRange(num13, num14),
				"x",
				UITooltip.FormatStyledIntRange(num15, num16)
			}), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData12);
		}
		if (num18 > 0 && !string.IsNullOrEmpty(itemDef.talentType))
		{
			UITooltipTextWidgetData uitooltipTextWidgetData13;
			if (itemDef.isSeed)
			{
				uitooltipTextWidgetData13 = new UITooltipTextWidgetData(string.Concat(new string[]
				{
					LLBase.L("item_tooltip_required_mastery"),
					" ",
					itemDef.talentType.FontIcon(),
					" ",
					UITooltip.FormatStyledIntRange(num17, num18)
				}), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			}
			else
			{
				uitooltipTextWidgetData13 = new UITooltipTextWidgetData(string.Concat(new string[]
				{
					LLBase.L("ui_mastery"),
					": ",
					itemDef.talentType.FontIcon(),
					" ",
					UITooltip.FormatStyledIntRange(num17, num18)
				}), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			}
			widgetData.Add(uitooltipTextWidgetData13);
		}
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x00106C9C File Offset: 0x00104E9C
	private static IReadOnlyList<ItemDef> ResolveItemEffectVariants(ItemDef itemDef, IReadOnlyList<ItemDef> qualityVariants)
	{
		if (qualityVariants != null && qualityVariants.Count > 1 && UITooltip.AreNumericQualityVariants(qualityVariants))
		{
			return qualityVariants;
		}
		return new ItemDef[] { itemDef };
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x00106CC0 File Offset: 0x00104EC0
	private static bool AreNumericQualityVariants(IReadOnlyList<ItemDef> items)
	{
		ItemDef itemDef = items[0];
		List<PerkDef> perksOnUse = itemDef.GetPerksOnUse();
		GameRes gameResOnUse = itemDef.GetGameResOnUse();
		bool flag = itemDef.itemGroupIds.Contains("bodypart");
		for (int i = 1; i < items.Count; i++)
		{
			ItemDef itemDef2 = items[i];
			if (itemDef2.type != itemDef.type || itemDef2.qualityIcon != itemDef.qualityIcon || itemDef2.isTool != itemDef.isTool || itemDef2.isWeapon != itemDef.isWeapon || itemDef2.isSeed != itemDef.isSeed || itemDef2.isBag != itemDef.isBag || itemDef2.talentType != itemDef.talentType || itemDef2.bodyLinkedPerk != itemDef.bodyLinkedPerk || itemDef2.CanBeUsed != itemDef.CanBeUsed || itemDef2.itemGroupIds.Contains("bodypart") != flag)
			{
				return false;
			}
			if (!UITooltip.AreSameStrings(itemDef.talentIds, itemDef2.talentIds))
			{
				return false;
			}
			if (!UITooltip.AreSamePerks(perksOnUse, itemDef2.GetPerksOnUse()))
			{
				return false;
			}
			if (!UITooltip.AreSameResTypes(gameResOnUse, itemDef2.GetGameResOnUse()))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x00106E04 File Offset: 0x00105004
	private static bool AreSameStrings(List<string> a, List<string> b)
	{
		if (a == b)
		{
			return true;
		}
		if (a == null || b == null || a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x00106E58 File Offset: 0x00105058
	private static bool AreSamePerks(List<PerkDef> a, List<PerkDef> b)
	{
		if (a == b)
		{
			return true;
		}
		if (a == null || b == null || a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i] && (a[i] == null || b[i] == null || a[i].id != b[i].id))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x00106ED8 File Offset: 0x001050D8
	private static bool AreSameResTypes(GameRes a, GameRes b)
	{
		if (a == b)
		{
			return true;
		}
		if (a == null || b == null || a.List.Count != b.List.Count)
		{
			return false;
		}
		for (int i = 0; i < a.List.Count; i++)
		{
			if (!b.Has(a.List[i].type))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x00106F4E File Offset: 0x0010514E
	private static string FormatIntRange(int min, int max)
	{
		if (min != max)
		{
			return string.Format("{0}-{1}", min, max);
		}
		return min.ToString();
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x00106F72 File Offset: 0x00105172
	private static string FormatStyledIntRange(int min, int max)
	{
		return UITooltip.instance.descriptionTextStyle.ApplyStyleToString(UITooltip.FormatIntRange(min, max), false, true);
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00106F8C File Offset: 0x0010518C
	private static string FormatOnUseResRange(string icon, int min, int max, bool isInsanity)
	{
		TextStyle textStyle;
		string text;
		if (min >= 0 && max >= 0)
		{
			textStyle = (isInsanity ? UITooltip.instance.blackEffectTextStyle : UITooltip.instance.greenEffectTextStyle);
			text = ((min == max) ? string.Format("+{0}", min) : string.Format("+{0}-{1}", min, max));
		}
		else if (min <= 0 && max <= 0)
		{
			textStyle = (isInsanity ? UITooltip.instance.blackEffectTextStyle : UITooltip.instance.redEffectTextStyle);
			text = ((min == max) ? min.ToString() : string.Format("{0}-{1}", min, max));
		}
		else
		{
			textStyle = (isInsanity ? UITooltip.instance.blackEffectTextStyle : UITooltip.instance.descriptionTextStyle);
			string text2 = ((max > 0) ? string.Format("+{0}", max) : max.ToString());
			text = string.Format("{0}-{1}", min, text2);
		}
		return (icon + textStyle.ApplyStyleToString(text, false, true)).NOBR();
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x00107094 File Offset: 0x00105294
	private static void AddItemWidgets(List<LazyWidgetDataBase> widgetData, ItemDef itemDef, bool addHeader = true, string headerPrefix = "", bool notShowStudyWidget = false, bool nowShowCraftedAt = false, IReadOnlyList<ItemDef> qualityVariants = null)
	{
		if (addHeader)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(UITooltip.TryConstructHeaderWithPrefix(itemDef.GetHeader(), headerPrefix), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		if (itemDef.type == ItemType.Preach)
		{
			SermonDef sermonDef = GameBalance.GetSermonDef(itemDef.id);
			if (sermonDef != null)
			{
				widgetData.Add(new UITooltipTextWidgetData(UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_need_start"), false, true) + " " + string.Format("{0}{1}", "happiness_cross".FontIcon(), sermonDef.minParishioners), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
				widgetData.Add(new UITooltipTextWidgetData(UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_need_success"), false, true) + " " + string.Format("{0}{1}", "happiness_cross".FontIcon(), sermonDef.sermonDifficulty), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
				widgetData.Add(new UITooltipTextWidgetData(string.Concat(new string[]
				{
					UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_base_reward"), false, true),
					"\n",
					string.Format("{0}{1:0.#}/", "faith".FontIcon(), sermonDef.baseFaithReward.EvaluateFloat()),
					"happiness_cross".FontIcon(),
					" + ",
					Trading.FormatMoney(sermonDef.baseMoneyReward.EvaluateInt(), false, " ", null),
					"/",
					"happiness_cross".FontIcon()
				}), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
				string text = string.Empty;
				float num = sermonDef.successRewardSmileFaith.EvaluateFloat();
				float num2 = sermonDef.successRewardCemeteryFaith.EvaluateFloat();
				float num3 = sermonDef.successRewardChurchFaith.EvaluateFloat();
				float num4 = sermonDef.successRewardSmileMoney.EvaluateFloat();
				float num5 = sermonDef.successRewardCemeteryMoney.EvaluateFloat();
				float num6 = sermonDef.successRewardChurchMoney.EvaluateFloat();
				if (num > 0f)
				{
					text += string.Format("{0}{1:0.#}/{2}", "faith".FontIcon(), num, "happiness".FontIcon());
				}
				if (num2 > 0f)
				{
					text += string.Format(" + {0}{1:0.#}/{2}", "faith".FontIcon(), num2, "wskull".FontIcon());
				}
				if (num3 > 0f)
				{
					text += string.Format(" + {0}{1:0.#}/{2}", "faith".FontIcon(), num3, "cross".FontIcon());
				}
				if (num4 > 0f)
				{
					text = string.Concat(new string[]
					{
						text,
						" + ",
						Trading.FormatMoney((int)num4, false, " ", null),
						"/",
						"happiness".FontIcon()
					});
				}
				if (num5 > 0f)
				{
					text = string.Concat(new string[]
					{
						text,
						" + ",
						Trading.FormatMoney((int)num5, false, " ", null),
						"/",
						"wskull".FontIcon()
					});
				}
				if (num6 > 0f)
				{
					text = string.Concat(new string[]
					{
						text,
						" + ",
						Trading.FormatMoney((int)num6, false, " ", null),
						"/",
						"cross".FontIcon()
					});
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (text.StartsWith(" +"))
					{
						text = text.Substring(3);
					}
					widgetData.Add(new UITooltipTextWidgetData(UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_success_reward"), false, true) + "\n" + text, TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
				}
				if (sermonDef.successRewardItem.HasOutputItems)
				{
					foreach (ChanceOutputItem chanceOutputItem in sermonDef.successRewardItem.chanceOutputItems)
					{
						widgetData.Add(new UITooltipTextWidgetData(string.Format("{0} {1}{2}", UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_item"), false, true), LLBase.L(chanceOutputItem.id), chanceOutputItem.count), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
					}
					foreach (GroupChanceOutputItem groupChanceOutputItem in sermonDef.successRewardItem.groupChanceOutputItems)
					{
						if (groupChanceOutputItem.chanceItems.Count > 0)
						{
							widgetData.Add(new UITooltipTextWidgetData(string.Format("{0} {1}{2}", UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_item"), false, true), LLBase.L(groupChanceOutputItem.chanceItems[0].id), groupChanceOutputItem.chanceItems[0].count), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
						}
					}
				}
				if (!string.IsNullOrEmpty(sermonDef.successRewardBuff))
				{
					PerkDef data = GameBalance.Me.GetData<PerkDef>(sermonDef.successRewardBuff);
					if (data != null)
					{
						widgetData.Add(new UITooltipTextWidgetData(string.Concat(new string[]
						{
							UITooltip.instance.prayDescriptionSelectedTextStyle.ApplyStyleToString(LLBase.L("ui_sermon_tooltip_buff"), false, true),
							" ",
							LLBase.L(sermonDef.successRewardBuff),
							"(",
							PerkSystemData.GetFormattedDuration(data.duration),
							")"
						}), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle));
					}
				}
				if (!nowShowCraftedAt && GameBalance.Me.craftInItemsCacheShownInTooltips.ContainsKey(itemDef.id) && GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id].Count > 0)
				{
					string text2 = LLBase.L("ui_crafted_at") + ": ";
					string text3 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text2, false, true) ?? "";
					for (int i = 0; i < GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id].Count; i++)
					{
						if (i > 0)
						{
							text3 += ", ";
						}
						text3 += LLBase.L(GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id][i]);
					}
					if (!(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
					{
						widgetData.Add(new UITooltipSeparatorWidgetData());
					}
					UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData(text3, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
					widgetData.Add(uitooltipTextWidgetData2);
				}
				return;
			}
			Debug.LogError("SermonDef is null for itemDef: " + itemDef.id + ". Show common tooltip");
		}
		if (UITooltip.TryAddDescriptionWidget(widgetData, itemDef.id))
		{
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		UITooltip.AddItemEffectWidgets(widgetData, itemDef, qualityVariants);
		SurveyDef surveyDefForItemOrNull = GameBalance.GetSurveyDefForItemOrNull(itemDef.id);
		if (surveyDefForItemOrNull != null && MainGame.Instance.GameSave.knowledgeSystem.IsSurveyCompleted(surveyDefForItemOrNull))
		{
			string text4 = itemDef.GetRunesAsString();
			if (!string.IsNullOrEmpty(text4))
			{
				text4 = UITooltip.instance.descriptionTextStyle.ApplyStyleToString(text4, false, true);
				UITooltipTextWidgetData uitooltipTextWidgetData3 = new UITooltipTextWidgetData(LLBase.L("item_runes") + ": \n" + text4, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData3);
			}
		}
		else if (surveyDefForItemOrNull != null && !string.IsNullOrEmpty(itemDef.GetRunesAsString()))
		{
			UITooltipTextWidgetData uitooltipTextWidgetData4 = new UITooltipTextWidgetData(LLBase.L("tut_survey_runes") + "???", TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData4);
		}
		if (surveyDefForItemOrNull != null)
		{
			if (surveyDefForItemOrNull.isScienceFuelCraft)
			{
				int num7 = ((surveyDefForItemOrNull.outputItems.chanceOutputItems.Count == 0) ? 0 : surveyDefForItemOrNull.outputItems.chanceOutputItems[0].count.EvaluateInt());
				if (num7 > 0)
				{
					UITooltipTextWidgetData uitooltipTextWidgetData5 = new UITooltipTextWidgetData(LLBase.L("hint_science_decompose") + " " + "science".FontIcon() + UITooltip.instance.descriptionTextStyle.ApplyStyleToString(num7.ToString(), false, true), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
					widgetData.Add(uitooltipTextWidgetData5);
				}
			}
			else if (!MainGame.Instance.GameSave.knowledgeSystem.IsSurveyCompleted(surveyDefForItemOrNull) && !notShowStudyWidget)
			{
				string text5 = string.Empty;
				if (surveyDefForItemOrNull.techRed > 0)
				{
					text5 += "tech_red".FontIcon();
				}
				if (surveyDefForItemOrNull.techGreen > 0)
				{
					text5 += "tech_green".FontIcon();
				}
				if (surveyDefForItemOrNull.techBlue > 0)
				{
					text5 += "tech_blue".FontIcon();
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text5 = " (" + text5 + ")";
					if (!(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
					{
						widgetData.Add(new UITooltipSeparatorWidgetData());
					}
					UITooltipTextWidgetData uitooltipTextWidgetData6 = new UITooltipTextWidgetData(LLBase.L("hint_survey_noun") + ":\n" + LLBase.L("survey_not_complete") + text5, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
					widgetData.Add(uitooltipTextWidgetData6);
				}
			}
		}
		if (!nowShowCraftedAt && GameBalance.Me.craftInItemsCacheShownInTooltips.ContainsKey(itemDef.id) && GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id].Count > 0)
		{
			string text6 = LLBase.L("ui_crafted_at") + ": ";
			string text7 = UITooltip.instance.selectedSmallDescriptionTextStyle.ApplyStyleToString(text6, false, true) ?? "";
			for (int j = 0; j < GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id].Count; j++)
			{
				if (j > 0)
				{
					text7 += ", ";
				}
				text7 += LLBase.L(GameBalance.Me.craftInItemsCacheShownInTooltips[itemDef.id][j]);
			}
			if (!(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
			{
				widgetData.Add(new UITooltipSeparatorWidgetData());
			}
			UITooltipTextWidgetData uitooltipTextWidgetData7 = new UITooltipTextWidgetData(text7, TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData7);
		}
		UITooltip.TryAddItemUseHintWidget(widgetData, itemDef);
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x00107B74 File Offset: 0x00105D74
	private static string TryConstructHeaderWithPrefix(string text, string headerPrefix)
	{
		if (!string.IsNullOrEmpty(headerPrefix))
		{
			text = headerPrefix + ": " + text;
		}
		return text;
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x00107B90 File Offset: 0x00105D90
	private static bool TryAddDescriptionWidget(List<LazyWidgetDataBase> widgetData, string id)
	{
		string text = id + "_d";
		string text2 = LLBase.L(text);
		if (text2 != text)
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(text2, TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
			return true;
		}
		return false;
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x00107BDC File Offset: 0x00105DDC
	private static bool TryAddItemUseHintWidget(List<LazyWidgetDataBase> widgetData, ItemDef itemDef)
	{
		string itemUseHintLocId = UITooltip.GetItemUseHintLocId(itemDef);
		if (string.IsNullOrEmpty(itemUseHintLocId))
		{
			return false;
		}
		if (widgetData.Count > 0 && !(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
		{
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		widgetData.Add(new UITooltipTextWidgetData(LLBase.L(itemUseHintLocId), TextAlignmentOptions.Center, UITooltip.instance.smallDescriptionTextStyle));
		return true;
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00107C44 File Offset: 0x00105E44
	private static string GetItemUseHintLocId(ItemDef itemDef)
	{
		if (itemDef.HasOnUseFunction("SuperUnlockBodyCustomization"))
		{
			return "hint_use_clothes";
		}
		if (itemDef.isSeed)
		{
			return "hint_use_seeds";
		}
		if (itemDef.HasOnUseFunction("OpenNotesWindow"))
		{
			return "hint_use_read";
		}
		return null;
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x00107C7C File Offset: 0x00105E7C
	private static void TryAddCraftNeedsWidgets(List<LazyWidgetDataBase> widgetData, CraftDef craftDef)
	{
		if (craftDef == null)
		{
			return;
		}
		ItemDef itemDef = craftDef.TryGetResultingItemDef(true);
		if (itemDef != null && itemDef.type == ItemType.Preach)
		{
			return;
		}
		UITooltipNeedsItemWidgetData uitooltipNeedsItemWidgetData = new UITooltipNeedsItemWidgetData(craftDef, new MultiInventory(MainGame.PlayerData, true), null);
		if (uitooltipNeedsItemWidgetData.CraftItemCellsData.Count == 0)
		{
			return;
		}
		if (widgetData.Count == 0 || !(widgetData[widgetData.Count - 1] is UITooltipSeparatorWidgetData))
		{
			widgetData.Add(new UITooltipSeparatorWidgetData());
		}
		widgetData.Add(uitooltipNeedsItemWidgetData);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00107CF4 File Offset: 0x00105EF4
	private static void AddCraftRequirementWidgets(List<LazyWidgetDataBase> widgetData, string requirementId, CraftDefBase craftDef, List<PerkData> perks, Item toolForWork)
	{
		if (!(requirementId == "energy"))
		{
			if (!(requirementId == "insanity"))
			{
				return;
			}
		}
		else
		{
			UITooltipTextWidgetData uitooltipTextWidgetData = new UITooltipTextWidgetData(LLBase.L("tt_energy_spendings"), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
			widgetData.Add(uitooltipTextWidgetData);
			widgetData.Add(new UITooltipSeparatorWidgetData());
			if (!craftDef.energyPerTick.EvaluateFloat().EqualsTo(0f, 1E-05f))
			{
				UITooltipTextWidgetData uitooltipTextWidgetData2 = new UITooltipTextWidgetData("energy".FontIcon() + craftDef.energyPerTick.EvaluateFloat().ToString() + "\n", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData2);
			}
			if (!toolForWork.IsEmpty && !toolForWork.Definition.GetGameResOnUse("energy").EqualsTo(0f, 1E-05f))
			{
				UITooltipTextWidgetData uitooltipTextWidgetData3 = new UITooltipTextWidgetData("energy".FontIcon() + (toolForWork.Definition.GetGameResOnUse("energy") * -1f).ToString() + "\n", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData3);
			}
			using (List<PerkData>.Enumerator enumerator = perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PerkData perkData = enumerator.Current;
					if (!perkData.Definition.energyAdd.EqualsTo(0f, 1E-05f))
					{
						UITooltipTextWidgetData uitooltipTextWidgetData4 = new UITooltipTextWidgetData(string.Concat(new string[]
						{
							LLBase.L(perkData.id),
							": ",
							"energy".FontIcon(),
							perkData.Definition.energyAdd.ToString(),
							"\n"
						}), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
						widgetData.Add(uitooltipTextWidgetData4);
					}
				}
				return;
			}
		}
		UITooltipTextWidgetData uitooltipTextWidgetData5 = new UITooltipTextWidgetData(LLBase.L("tt_insanity_spendings"), TextAlignmentOptions.Center, UITooltip.instance.headerTextStyle);
		widgetData.Add(uitooltipTextWidgetData5);
		widgetData.Add(new UITooltipSeparatorWidgetData());
		if (!craftDef.insanityPerTick.EvaluateFloat().EqualsTo(0f, 1E-05f))
		{
			UITooltipTextWidgetData uitooltipTextWidgetData6 = new UITooltipTextWidgetData("insanity".FontIcon() + craftDef.insanityPerTick.EvaluateFloat().ToString() + "\n", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData6);
		}
		if (!toolForWork.IsEmpty && !toolForWork.Definition.GetGameResOnUse("insanity").EqualsTo(0f, 1E-05f))
		{
			UITooltipTextWidgetData uitooltipTextWidgetData7 = new UITooltipTextWidgetData("insanity".FontIcon() + toolForWork.Definition.GetGameResOnUse("insanity").ToString() + "\n", TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
			widgetData.Add(uitooltipTextWidgetData7);
		}
		foreach (PerkData perkData2 in perks)
		{
			if (!perkData2.Definition.insanityAdd.EqualsTo(0f, 1E-05f))
			{
				UITooltipTextWidgetData uitooltipTextWidgetData8 = new UITooltipTextWidgetData(string.Concat(new string[]
				{
					LLBase.L(perkData2.id),
					": ",
					"insanity".FontIcon(),
					perkData2.Definition.insanityAdd.ToString(),
					"\n"
				}), TextAlignmentOptions.Center, UITooltip.instance.descriptionTextStyle);
				widgetData.Add(uitooltipTextWidgetData8);
			}
		}
	}

	// Token: 0x04002B55 RID: 11093
	private static UITooltip instance;

	// Token: 0x04002B56 RID: 11094
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04002B57 RID: 11095
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04002B58 RID: 11096
	[SerializeField]
	private LazyWidgetContainer widgetContainer;

	// Token: 0x04002B59 RID: 11097
	[SerializeField]
	private TextStyle headerTextStyle;

	// Token: 0x04002B5A RID: 11098
	[SerializeField]
	private TextStyle headerBoldTextStyle;

	// Token: 0x04002B5B RID: 11099
	[SerializeField]
	private TextStyle descriptionTextStyle;

	// Token: 0x04002B5C RID: 11100
	[SerializeField]
	private TextStyle prayDescriptionSelectedTextStyle;

	// Token: 0x04002B5D RID: 11101
	[SerializeField]
	private TextStyle smallDescriptionTextStyle;

	// Token: 0x04002B5E RID: 11102
	[SerializeField]
	private TextStyle selectedSmallDescriptionTextStyle;

	// Token: 0x04002B5F RID: 11103
	[SerializeField]
	private TextStyle perkDescriptionTextStyle;

	// Token: 0x04002B60 RID: 11104
	[SerializeField]
	private TextStyle greenEffectTextStyle;

	// Token: 0x04002B61 RID: 11105
	[SerializeField]
	private TextStyle redEffectTextStyle;

	// Token: 0x04002B62 RID: 11106
	[SerializeField]
	private TextStyle blackEffectTextStyle;

	// Token: 0x04002B63 RID: 11107
	[SerializeField]
	private TextStyle headerBoldTextStyleNotEnough;

	// Token: 0x04002B64 RID: 11108
	[SerializeField]
	private TextStyle headerBoldTextStyleGold;

	// Token: 0x04002B65 RID: 11109
	[SerializeField]
	private Vector2 offsetAdd;

	// Token: 0x04002B66 RID: 11110
	[SerializeField]
	private Vector2 alternativeDownOffsetAdd;

	// Token: 0x04002B67 RID: 11111
	private Sequence activeTweenSequence;

	// Token: 0x04002B68 RID: 11112
	private RectTransform target;

	// Token: 0x04002B69 RID: 11113
	private Vector2 offset;

	// Token: 0x04002B6A RID: 11114
	private Vector2 alternativeDownOffset;

	// Token: 0x04002B6B RID: 11115
	private Vector2 appearOffset;

	// Token: 0x04002B6C RID: 11116
	private TooltipPlacementPriority placementPriority;

	// Token: 0x04002B6D RID: 11117
	private readonly Vector3[] targetWorldCorners = new Vector3[4];

	// Token: 0x04002B6E RID: 11118
	private readonly Vector3[] bubbleWorldCorners = new Vector3[4];
}
