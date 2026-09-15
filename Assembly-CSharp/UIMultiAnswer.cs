using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using LinqTools;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000807 RID: 2055
public class UIMultiAnswer : MonoBehaviour
{
	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x0600349E RID: 13470 RVA: 0x000FD00F File Offset: 0x000FB20F
	public static bool IsShowing
	{
		get
		{
			return UIMultiAnswer.multiAnswers.Count != 0;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x0600349F RID: 13471 RVA: 0x000FD01E File Offset: 0x000FB21E
	public bool IsScrollModeActive
	{
		get
		{
			return this.isScrollModeActive;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x060034A0 RID: 13472 RVA: 0x000FD026 File Offset: 0x000FB226
	public bool IsSideDisplayActive
	{
		get
		{
			return this.isSideDisplayActive;
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x060034A1 RID: 13473 RVA: 0x000FD02E File Offset: 0x000FB22E
	// (set) Token: 0x060034A2 RID: 13474 RVA: 0x000FD036 File Offset: 0x000FB236
	public int MaxIconsCount
	{
		get
		{
			return this.maxIconsCount;
		}
		set
		{
			if (value > this.maxIconsCount)
			{
				this.maxIconsCount = value;
			}
		}
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x000FD048 File Offset: 0x000FB248
	public void Init()
	{
		UIMultiAnswer.instance = this;
		this.gamepadController.loopVerticalNavigation = true;
		base.gameObject.SetActive(false);
		this.answerOptionPrefab.gameObject.SetActive(false);
		this.DisableScrollRectMode();
		this.SetSortingOrder(false);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x000FD086 File Offset: 0x000FB286
	public void OnAnswerSelect(string answerId)
	{
		if (!this.interactable)
		{
			return;
		}
		LazyAudio.PlayAndForget("gui_click");
		Debug.Log("answer is interactable, invoking action");
		Action<string> action = this.onChosen;
		if (action != null)
		{
			action(answerId);
		}
		this.StartDisappearAnimation();
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x000FD0C0 File Offset: 0x000FB2C0
	public static void ForceDisableAll()
	{
		for (int i = UIMultiAnswer.multiAnswers.Count - 1; i >= 0; i--)
		{
			UIMultiAnswer uimultiAnswer = UIMultiAnswer.multiAnswers[i];
			uimultiAnswer.ChangeInteractable(false);
			uimultiAnswer.gamepadController.Disable();
			uimultiAnswer.DisableBubble();
		}
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x000FD108 File Offset: 0x000FB308
	private void ShowAnswers(List<AnswerVisualData> answers, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool isOverBlackout = false)
	{
		this.visualData = this.FormVisibleAnswers(answers);
		this.forcedCornerPosition = forceCornerPosition;
		if (this.visualData.Count == 0)
		{
			Debug.LogError("Multianswer doesn't have available answers");
			this.onChosen("Error");
			return;
		}
		this.CreateAnswerOptions(this.visualData);
		this.SetSortingOrder(isOverBlackout);
		base.gameObject.SetActive(true);
		this.DisableScrollRectMode();
		this.contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		(this.contentSizeFitter.transform as RectTransform).RefreshContentFitter();
		this.ApplyBubblePosition();
		if (!this.subscribedGamepadEvents)
		{
			LazyInput.OnInputChanged += this.HandleGamepadState;
			this.subscribedGamepadEvents = true;
		}
		this.HandleGamepadState();
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x000FD1C3 File Offset: 0x000FB3C3
	private void SetSortingOrder(bool isOverBlackout)
	{
		Canvas component = base.GetComponent<Canvas>();
		component.overrideSorting = true;
		component.sortingOrder = (isOverBlackout ? 900 : 350);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x000FD1E8 File Offset: 0x000FB3E8
	private void HandleGamepadState()
	{
		if (this == null || base.gameObject == null || this.answerOptions == null)
		{
			LazyInput.OnInputChanged -= this.HandleGamepadState;
			return;
		}
		if (!base.gameObject.activeSelf || this.answerOptions.Count == 0)
		{
			return;
		}
		LazyInput.ClearKeyDown(GameKey.Select);
		LazyInput.ClearKey(GameKey.Select);
		if (LazyInput.IsGamepadActive)
		{
			this.gamepadController.Enable();
			if (this.isScrollModeActive)
			{
				this.autoScroll.enabled = true;
				this.autoScroll.SkipNextAutoscroll = true;
			}
			this.gamepadController.ReinitItems(false, null, null);
			using (List<UIMultiAnswerOption>.Enumerator enumerator = this.answerOptions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIMultiAnswerOption uimultiAnswerOption = enumerator.Current;
					if (!(uimultiAnswerOption == null))
					{
						this.gamepadController.SetFocusedItem(uimultiAnswerOption.Item);
						break;
					}
				}
				return;
			}
		}
		this.autoScroll.enabled = false;
		this.scrollRect.DOKill(false);
		this.gamepadController.Disable();
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x000FD318 File Offset: 0x000FB518
	private void ApplyBubblePosition()
	{
		Vector2 vector = CameraSystem.WorldToScreenPoint(this.targetTransform.position);
		if (this.answerOptions.IsNullOrEmpty<UIMultiAnswerOption>())
		{
			Debug.LogError("Answers options are empty");
			return;
		}
		float num2;
		RectTransform rectTransform;
		RectTransform rectTransform2;
		RectTransform rectTransform3;
		RectTransform rectTransform4;
		float num = this.CalculateBubbleHeightScreen(out num2, out rectTransform, out rectTransform2, out rectTransform3, out rectTransform4);
		float num3 = 0f;
		for (int i = 0; i < this.answerOptions.Count; i++)
		{
			float extraWidth = this.answerOptions[i].GetExtraWidth();
			if (extraWidth > num3)
			{
				num3 = extraWidth;
			}
		}
		float maxSideDisplayIconsOffsetLocal = this.GetMaxSideDisplayIconsOffsetLocal(true);
		float maxSideDisplayIconsOffsetLocal2 = this.GetMaxSideDisplayIconsOffsetLocal(false);
		bool flag;
		bool flag2;
		this.DecideBubbleOrientation(vector, num, out flag, out flag2);
		this.isScrollModeActive = flag;
		this.isSideDisplayActive = flag2;
		bool flag3 = (flag2 ? UIMultiAnswer.DecideHorizontalSide(vector, num2, num3, this.sideDisplayHorizontalOffset, maxSideDisplayIconsOffsetLocal, maxSideDisplayIconsOffsetLocal2) : UIMultiAnswer.IsLeftSide(vector, num2, num3));
		float num4 = num;
		if (flag)
		{
			this.ApplyScrollRootPadding();
			float availableHeightScreen = UIMultiAnswer.GetAvailableHeightScreen(vector, this.opensDownward);
			float num5 = this.CalculateScrollViewportHeightLocal(availableHeightScreen);
			float num6 = num / LazyUI.ScaleFactor;
			num5 = Mathf.Min(num5, num6);
			num4 = availableHeightScreen;
			this.EnableScrollRectMode(num5);
			this.RebuildScrollLayout();
			this.ApplyViewportHeight(num5);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform as RectTransform);
			this.ApplyInitialScrollPosition();
		}
		Vector2 vector2 = vector;
		float num7;
		if (this.isSideDisplayActive)
		{
			float sideDisplayIconsOffsetScreen = UIMultiAnswer.GetSideDisplayIconsOffsetScreen(flag3, maxSideDisplayIconsOffsetLocal, maxSideDisplayIconsOffsetLocal2);
			vector2.x = UIMultiAnswer.ApplySideDisplayHorizontalOffset(vector.x, flag3, this.sideDisplayHorizontalOffset + sideDisplayIconsOffsetScreen);
			vector2.y = UIMultiAnswer.GetSideDisplayAnchorScreenY(num, this.opensDownward);
			num7 = 0f;
		}
		else
		{
			num7 = UIMultiAnswer.CalculateScreenClampOffsetY(vector2, num4, this.opensDownward);
		}
		Vector2 anchorRelativePosition = this.GetAnchorRelativePosition(flag, flag3, rectTransform, rectTransform2, rectTransform3, rectTransform4);
		base.transform.position = new Vector2(0f, -num7) + vector2 - anchorRelativePosition * LazyUI.ScaleFactor;
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x000FD508 File Offset: 0x000FB708
	private float CalculateBubbleHeightScreen(out float bubbleWidthScreen, out RectTransform ul, out RectTransform ur, out RectTransform dl, out RectTransform dr)
	{
		UIMultiAnswerOption uimultiAnswerOption = this.answerOptions[0];
		UIMultiAnswerOption uimultiAnswerOption2 = this.answerOptions.Last<UIMultiAnswerOption>();
		RectTransform boundsRectTransform = uimultiAnswerOption.GetBoundsRectTransform();
		RectTransform boundsRectTransform2 = uimultiAnswerOption2.GetBoundsRectTransform();
		ul = uimultiAnswerOption.GetCornerTransform(UIBasicBubble.BubbleCornerDirection.LeftUp);
		ur = uimultiAnswerOption.GetCornerTransform(UIBasicBubble.BubbleCornerDirection.RightUp);
		dl = uimultiAnswerOption2.GetCornerTransform(UIBasicBubble.BubbleCornerDirection.LeftDown);
		dr = uimultiAnswerOption2.GetCornerTransform(UIBasicBubble.BubbleCornerDirection.RightDown);
		Vector3[] array = new Vector3[4];
		Vector3[] array2 = new Vector3[4];
		boundsRectTransform.GetWorldCorners(array);
		boundsRectTransform2.GetWorldCorners(array2);
		Vector2 vector = base.transform.InverseTransformPoint(array[1]);
		Vector2 vector2 = base.transform.InverseTransformPoint(array[2]);
		Vector2 vector3 = base.transform.InverseTransformPoint(array2[0]);
		float num = vector.y - vector3.y;
		float num2 = vector2.x - vector.x;
		bubbleWidthScreen = num2 * LazyUI.ScaleFactor;
		return num * LazyUI.ScaleFactor;
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x000FD5FC File Offset: 0x000FB7FC
	private static float GetAvailableHeightScreen(Vector2 screenPos, bool opensDown)
	{
		float num = (float)Screen.height - screenPos.y;
		float y = screenPos.y;
		if (!opensDown)
		{
			return y;
		}
		return num;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x000FD624 File Offset: 0x000FB824
	private static bool IsLeftSide(Vector2 screenPos, float bubbleWidthScreen, float extraWidth)
	{
		return screenPos.x + bubbleWidthScreen + extraWidth * LazyUI.ScaleFactor < (float)Screen.width;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x000FD63E File Offset: 0x000FB83E
	private void DecideBubbleOrientation(Vector2 screenPos, float bubbleHeightScreen, out bool needsScroll, out bool useSideDisplay)
	{
		useSideDisplay = false;
		if (this.overflowMode == UIMultiAnswer.OverflowMode.FullDisplayOnSide)
		{
			this.DecideFullDisplayOnSideOrientation(screenPos, bubbleHeightScreen, out needsScroll, out useSideDisplay);
			return;
		}
		this.DecideScrollOrientation(screenPos, bubbleHeightScreen, out needsScroll);
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x000FD664 File Offset: 0x000FB864
	private void DecideFullDisplayOnSideOrientation(Vector2 screenPos, float bubbleHeightScreen, out bool needsScroll, out bool useSideDisplay)
	{
		useSideDisplay = false;
		if (bubbleHeightScreen > (float)Screen.height)
		{
			this.DecideScrollOrientation(screenPos, bubbleHeightScreen, out needsScroll);
			return;
		}
		float num = (float)Screen.height - screenPos.y;
		float y = screenPos.y;
		UIBasicBubble.ForceCornerPosition forceCornerPosition = this.forcedCornerPosition;
		if (forceCornerPosition != UIBasicBubble.ForceCornerPosition.Auto)
		{
			if (forceCornerPosition - UIBasicBubble.ForceCornerPosition.BottomRight > 1)
			{
				throw new NotImplementedException();
			}
			if (bubbleHeightScreen <= num)
			{
				this.opensDownward = true;
				needsScroll = false;
				return;
			}
			if (bubbleHeightScreen <= y)
			{
				this.opensDownward = false;
				needsScroll = false;
				return;
			}
			useSideDisplay = true;
			this.opensDownward = true;
			needsScroll = false;
			return;
		}
		else
		{
			if (bubbleHeightScreen <= num)
			{
				this.opensDownward = true;
				needsScroll = false;
				return;
			}
			if (bubbleHeightScreen <= y)
			{
				this.opensDownward = false;
				needsScroll = false;
				return;
			}
			useSideDisplay = true;
			this.opensDownward = true;
			needsScroll = false;
			return;
		}
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x000FD70C File Offset: 0x000FB90C
	private void DecideScrollOrientation(Vector2 screenPos, float bubbleHeightScreen, out bool needsScroll)
	{
		float num = (float)Screen.height - screenPos.y;
		float y = screenPos.y;
		UIBasicBubble.ForceCornerPosition forceCornerPosition = this.forcedCornerPosition;
		if (forceCornerPosition != UIBasicBubble.ForceCornerPosition.Auto)
		{
			if (forceCornerPosition - UIBasicBubble.ForceCornerPosition.BottomRight <= 1)
			{
				this.opensDownward = true;
				needsScroll = bubbleHeightScreen > num;
				return;
			}
			throw new NotImplementedException();
		}
		else
		{
			if (bubbleHeightScreen <= num)
			{
				this.opensDownward = true;
				needsScroll = false;
				return;
			}
			if (bubbleHeightScreen <= y)
			{
				this.opensDownward = false;
				needsScroll = false;
				return;
			}
			this.opensDownward = true;
			needsScroll = true;
			return;
		}
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x000FD77C File Offset: 0x000FB97C
	private float GetMaxSideDisplayIconsOffsetLocal(bool leftIcons)
	{
		float num = 0f;
		for (int i = 0; i < this.answerOptions.Count; i++)
		{
			float num2 = (leftIcons ? this.answerOptions[i].GetLeftIconsExtraWidth() : this.answerOptions[i].GetRightIconsExtraWidth());
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x000FD7D4 File Offset: 0x000FB9D4
	private static float GetSideDisplayIconsOffsetScreen(bool isLeftSide, float maxLeftIconsOffsetLocal, float maxRightIconsOffsetLocal)
	{
		return (isLeftSide ? maxLeftIconsOffsetLocal : maxRightIconsOffsetLocal) * LazyUI.ScaleFactor;
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x000FD7E4 File Offset: 0x000FB9E4
	private static bool DecideHorizontalSide(Vector2 screenPos, float bubbleWidthScreen, float extraWidth, float horizontalOffset, float maxLeftIconsOffsetLocal, float maxRightIconsOffsetLocal)
	{
		float num = bubbleWidthScreen + extraWidth * LazyUI.ScaleFactor;
		float num2 = horizontalOffset + maxLeftIconsOffsetLocal * LazyUI.ScaleFactor;
		float num3 = horizontalOffset + maxRightIconsOffsetLocal * LazyUI.ScaleFactor;
		bool flag = screenPos.x + num2 + num <= (float)Screen.width;
		bool flag2 = screenPos.x - num3 - num >= 0f;
		if (flag)
		{
			return true;
		}
		if (flag2)
		{
			return false;
		}
		float num4 = (float)Screen.width - screenPos.x - num2;
		float num5 = screenPos.x - num3;
		return num4 >= num5;
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x000FD864 File Offset: 0x000FBA64
	private static float ApplySideDisplayHorizontalOffset(float npcScreenX, bool isLeftSide, float horizontalOffset)
	{
		if (!isLeftSide)
		{
			return npcScreenX - horizontalOffset;
		}
		return npcScreenX + horizontalOffset;
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x000FD870 File Offset: 0x000FBA70
	private static float GetSideDisplayAnchorScreenY(float bubbleHeightScreen, bool opensDown)
	{
		float num = (float)Screen.height * 0.5f;
		if (!opensDown)
		{
			return num + bubbleHeightScreen * 0.5f;
		}
		return num - bubbleHeightScreen * 0.5f;
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x000FD8A0 File Offset: 0x000FBAA0
	private static RectTransform GetActiveCorner(bool opensDown, bool isLeftSide, RectTransform ul, RectTransform ur, RectTransform dl, RectTransform dr)
	{
		if (opensDown)
		{
			if (!isLeftSide)
			{
				return dr;
			}
			return dl;
		}
		else
		{
			if (!isLeftSide)
			{
				return ur;
			}
			return ul;
		}
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x000FD8B4 File Offset: 0x000FBAB4
	private Vector3 GetViewportAnchorWorldPosition(bool isLeftSide)
	{
		RectTransform viewport = this.scrollRect.viewport;
		Vector3[] array = new Vector3[4];
		viewport.GetWorldCorners(array);
		if (this.opensDownward)
		{
			if (!isLeftSide)
			{
				return array[3];
			}
			return array[0];
		}
		else
		{
			if (!isLeftSide)
			{
				return array[2];
			}
			return array[1];
		}
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x000FD908 File Offset: 0x000FBB08
	private Vector2 GetAnchorRelativePosition(bool needsScroll, bool isLeftSide, RectTransform ul, RectTransform ur, RectTransform dl, RectTransform dr)
	{
		Vector3 position = UIMultiAnswer.GetActiveCorner(this.opensDownward, isLeftSide, ul, ur, dl, dr).position;
		if (needsScroll)
		{
			Vector3 viewportAnchorWorldPosition = this.GetViewportAnchorWorldPosition(isLeftSide);
			position = new Vector3(position.x, viewportAnchorWorldPosition.y, position.z);
		}
		return base.transform.InverseTransformPoint(position);
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x000FD963 File Offset: 0x000FBB63
	private void RebuildScrollLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.scrollRect.transform as RectTransform);
		Canvas.ForceUpdateCanvases();
	}

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x060034B9 RID: 13497 RVA: 0x000FD97F File Offset: 0x000FBB7F
	private VerticalLayoutGroup RootVerticalLayoutGroup
	{
		get
		{
			if (this.rootVerticalLayoutGroup == null)
			{
				this.rootVerticalLayoutGroup = base.GetComponent<VerticalLayoutGroup>();
			}
			return this.rootVerticalLayoutGroup;
		}
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x000FD9A4 File Offset: 0x000FBBA4
	private float CalculateScrollViewportHeightLocal(float availableHeightScreen)
	{
		float num = availableHeightScreen - 20f;
		return Mathf.Max(0f, num / LazyUI.ScaleFactor);
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x000FD9CC File Offset: 0x000FBBCC
	private static float CalculateScreenClampOffsetY(Vector2 anchorScreenPos, float heightScreen, bool opensDown)
	{
		if (opensDown)
		{
			float num = anchorScreenPos.y + heightScreen - (float)Screen.height;
			return Mathf.Max(0f, num);
		}
		float num2 = anchorScreenPos.y - heightScreen;
		return Mathf.Min(0f, num2);
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x000FDA0C File Offset: 0x000FBC0C
	private void CacheRootPaddingDefaults()
	{
		if (this.rootPaddingTopDefault >= 0)
		{
			return;
		}
		VerticalLayoutGroup verticalLayoutGroup = this.RootVerticalLayoutGroup;
		this.rootPaddingTopDefault = verticalLayoutGroup.padding.top;
		this.rootPaddingBottomDefault = verticalLayoutGroup.padding.bottom;
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x000FDA4C File Offset: 0x000FBC4C
	private void ApplyScrollRootPadding()
	{
		this.CacheRootPaddingDefaults();
		VerticalLayoutGroup verticalLayoutGroup = this.RootVerticalLayoutGroup;
		if (this.opensDownward)
		{
			verticalLayoutGroup.padding.top = 0;
			return;
		}
		verticalLayoutGroup.padding.bottom = 0;
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x000FDA87 File Offset: 0x000FBC87
	private void RestoreScrollRootPadding()
	{
		if (this.rootPaddingTopDefault < 0)
		{
			return;
		}
		VerticalLayoutGroup verticalLayoutGroup = this.RootVerticalLayoutGroup;
		verticalLayoutGroup.padding.top = this.rootPaddingTopDefault;
		verticalLayoutGroup.padding.bottom = this.rootPaddingBottomDefault;
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x000FDABC File Offset: 0x000FBCBC
	private void ApplyViewportHeight(float heightLocal)
	{
		this.scrollViewportLayoutElement.minHeight = heightLocal;
		this.scrollViewportLayoutElement.preferredHeight = heightLocal;
		this.scrollViewportLayoutElement.flexibleHeight = 0f;
		this.scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightLocal);
		LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.minHeight = heightLocal;
			component.preferredHeight = heightLocal;
			component.flexibleHeight = 0f;
		}
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000FDB31 File Offset: 0x000FBD31
	private void ApplyInitialScrollPosition()
	{
		this.scrollRect.DOKill(false);
		this.scrollRect.verticalNormalizedPosition = 1f;
		this.autoScroll.SkipNextAutoscroll = true;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x000FDB5C File Offset: 0x000FBD5C
	private void ResetRectMaskPadding()
	{
		if (this.rectMask2D == null)
		{
			return;
		}
		this.rectMask2D.padding = Vector4.zero;
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000FDB80 File Offset: 0x000FBD80
	private List<AnswerVisualData> FormVisibleAnswers(List<AnswerVisualData> answers)
	{
		List<AnswerVisualData> list = new List<AnswerVisualData>();
		foreach (AnswerVisualData answerVisualData in answers)
		{
			if (!string.IsNullOrEmpty(answerVisualData.id))
			{
				KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
				if ((!answerVisualData.hiddenByDefault || knowledgeSystem.unlockedPhrases.Contains(answerVisualData.id)) && !knowledgeSystem.blackListPhrases.Contains(answerVisualData.id))
				{
					list.Add(answerVisualData);
				}
			}
		}
		return list;
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x000FDC20 File Offset: 0x000FBE20
	private void CreateAnswerOptions(List<AnswerVisualData> visualData)
	{
		this.answerOptions = new List<UIMultiAnswerOption>();
		float num = 0.3f / (float)visualData.Count;
		StringBuilder stringBuilder = new StringBuilder();
		float elementsSize = 0f;
		for (int i = 0; i < visualData.Count; i++)
		{
			stringBuilder.Clear();
			stringBuilder.Append("#").Append(i).Append(": ")
				.Append(visualData[i].id);
			UIMultiAnswerOption answerOption = this.answerOptionPrefab.Copy(null, true, stringBuilder.ToString());
			answerOption.Show(visualData[i], this);
			if (i == visualData.Count - 1)
			{
				this.bottomOption = answerOption;
			}
			if (i > 0)
			{
				LazyTimer.AddTimer(0f, delegate
				{
					elementsSize += answerOption.gameObject.GetComponent<RectTransform>().sizeDelta.y;
				}, null);
			}
			answerOption.Canvas.alpha = 0f;
			float animationDelay = num * (float)(visualData.Count - 1 - i);
			if (animationDelay <= 0f)
			{
				answerOption.AnimateAppearing(0.3f);
			}
			else
			{
				LazyTimer.AddTimer(animationDelay, delegate
				{
					answerOption.AnimateAppearing(0.3f - animationDelay);
				}, null);
			}
			this.answerOptions.Add(answerOption);
		}
		this.ChangeInteractable(false);
		LazyTimer.AddTimer(0.3f, delegate
		{
			this.ChangeInteractable(true);
			if (LazyInput.IsGamepadActive)
			{
				this.HandleGamepadState();
			}
		}, null);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
		foreach (UIMultiAnswerOption uimultiAnswerOption in this.answerOptions)
		{
			uimultiAnswerOption.ShowIcons();
		}
		for (int j = 0; j < this.answerOptions.Count; j++)
		{
			this.answerOptions[j].UpdateAnswerPosition();
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000FDE40 File Offset: 0x000FC040
	protected void OnDestroy()
	{
		if (this.subscribedGamepadEvents)
		{
			LazyInput.OnInputChanged -= this.HandleGamepadState;
			this.subscribedGamepadEvents = false;
		}
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000FDE64 File Offset: 0x000FC064
	private void ChangeInteractable(bool isInteractable)
	{
		this.interactable = isInteractable;
		foreach (UIMultiAnswerOption uimultiAnswerOption in this.answerOptions)
		{
			uimultiAnswerOption.ChangeInteractableByAnimation(isInteractable);
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000FDEBC File Offset: 0x000FC0BC
	private void StartDisappearAnimation()
	{
		this.ChangeInteractable(false);
		this.gamepadController.Disable();
		this.canvas.DOFade(0f, 0.2f).OnComplete(new TweenCallback(this.DisableBubble));
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x000FDEF8 File Offset: 0x000FC0F8
	private void DisableBubble()
	{
		LazyInput.OnInputChanged -= this.HandleGamepadState;
		Action action = this.onDisappeared;
		if (action != null)
		{
			action();
		}
		UIMultiAnswer.multiAnswers.Remove(this);
		if (this.subscribedGamepadEvents)
		{
			LazyInput.OnInputChanged -= this.HandleGamepadState;
			this.subscribedGamepadEvents = false;
		}
		CanvasGroup[] componentsInChildren = base.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DOKill(false);
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x000FDF80 File Offset: 0x000FC180
	private void Update()
	{
		if (LazyInput.IsGamepadActive && this.interactable && !LazyWindowsStackController.HasAnyModalWindowOpened)
		{
			if (LazyInput.GetKeyDown(GameKey.Up))
			{
				LazyInput.ClearKeyDown(GameKey.Up);
				this.gamepadController.Navigate(GUIDirection.Up);
			}
			if (LazyInput.GetKeyDown(GameKey.Down))
			{
				LazyInput.ClearKeyDown(GameKey.Down);
				this.gamepadController.Navigate(GUIDirection.Down);
			}
			if (LazyInput.GetKeyDown(GameKey.Right))
			{
				LazyInput.ClearKeyDown(GameKey.Right);
				this.gamepadController.Navigate(GUIDirection.Right);
			}
			if (LazyInput.GetKeyDown(GameKey.Left))
			{
				LazyInput.ClearKeyDown(GameKey.Left);
				this.gamepadController.Navigate(GUIDirection.Left);
			}
			if (LazyInput.GetKeyDown(GameKey.DpadUp))
			{
				LazyInput.ClearKeyDown(GameKey.DpadUp);
				this.gamepadController.Navigate(GUIDirection.Up);
			}
			if (LazyInput.GetKeyDown(GameKey.DpadDown))
			{
				LazyInput.ClearKeyDown(GameKey.DpadDown);
				this.gamepadController.Navigate(GUIDirection.Down);
			}
			if (LazyInput.GetKeyDown(GameKey.DpadRight))
			{
				LazyInput.ClearKeyDown(GameKey.DpadRight);
				this.gamepadController.Navigate(GUIDirection.Right);
			}
			if (LazyInput.GetKeyDown(GameKey.DpadLeft))
			{
				LazyInput.ClearKeyDown(GameKey.DpadLeft);
				this.gamepadController.Navigate(GUIDirection.Left);
			}
			if (LazyInput.GetKeyDown(GameKey.Select))
			{
				LazyInput.ClearAllKeysDown();
				this.gamepadController.SelectFocusedItem();
			}
		}
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x000FE0D8 File Offset: 0x000FC2D8
	private void HandleClosedAllWindows()
	{
		this.SetControlState(true);
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x000FE0E1 File Offset: 0x000FC2E1
	private void SetControlState(bool isControllable)
	{
		this.canvas.interactable = isControllable;
		this.isControllable = isControllable;
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x000FE0F8 File Offset: 0x000FC2F8
	private void DisableScrollRectMode()
	{
		this.isScrollModeActive = false;
		this.isSideDisplayActive = false;
		this.scrollRect.enabled = false;
		this.scrollRect.vertical = false;
		this.autoScroll.enabled = false;
		this.scrollViewportLayoutElement.minHeight = 0f;
		this.scrollViewportLayoutElement.preferredHeight = -1f;
		this.scrollViewportLayoutElement.flexibleHeight = -1f;
		LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.minHeight = -1f;
			component.preferredHeight = -1f;
			component.flexibleHeight = -1f;
		}
		this.contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		this.RestoreScrollRootPadding();
		this.ResetRectMaskPadding();
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x000FE1B8 File Offset: 0x000FC3B8
	private void EnableScrollRectMode(float viewportHeightLocal)
	{
		this.isScrollModeActive = true;
		this.scrollRect.vertical = true;
		this.scrollRect.enabled = true;
		this.autoScroll.enabled = LazyInput.IsGamepadActive;
		this.contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
		this.ApplyViewportHeight(viewportHeightLocal);
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x000FE208 File Offset: 0x000FC408
	public static void ShowAnswers(List<AnswerVisualData> answers, Transform targetTransform, WgoData dialogParticipant, Action<string> onChosen, Action onDisappeared, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool isMainMultianswer = false, bool isOverBlackout = false)
	{
		if (UIMultiAnswer.instance == null)
		{
			Debug.LogError("MultiAnswer.ShowAnswers error: instance is null");
			return;
		}
		UIMultiAnswer uimultiAnswer = UIMultiAnswer.instance.Copy(null, true, "");
		uimultiAnswer.targetTransform = targetTransform;
		uimultiAnswer.dialogParticipant = dialogParticipant;
		uimultiAnswer.onChosen = onChosen;
		uimultiAnswer.onDisappeared = onDisappeared;
		uimultiAnswer.isMainMultianswer = isMainMultianswer;
		uimultiAnswer.ShowAnswers(answers, forceCornerPosition, isOverBlackout);
		UIMultiAnswer.multiAnswers.Add(uimultiAnswer);
	}

	// Token: 0x04002A15 RID: 10773
	private const float DISAPPEAR_ANIM_TIME = 0.2f;

	// Token: 0x04002A16 RID: 10774
	private const float APPEAR_ANIM_TIME = 0.3f;

	// Token: 0x04002A17 RID: 10775
	private const float SCROLL_RECT_PADDING = 20f;

	// Token: 0x04002A18 RID: 10776
	[SerializeField]
	private float sideDisplayHorizontalOffset = 70f;

	// Token: 0x04002A19 RID: 10777
	[SerializeField]
	private UIMultiAnswerOption answerOptionPrefab;

	// Token: 0x04002A1A RID: 10778
	[SerializeField]
	private CanvasGroup canvas;

	// Token: 0x04002A1B RID: 10779
	[SerializeField]
	protected GamepadNavigationController gamepadController;

	// Token: 0x04002A1C RID: 10780
	[SerializeField]
	private ContentSizeFitter contentSizeFitter;

	// Token: 0x04002A1D RID: 10781
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002A1E RID: 10782
	[SerializeField]
	private AutoScroll autoScroll;

	// Token: 0x04002A1F RID: 10783
	[SerializeField]
	private LayoutElement scrollViewportLayoutElement;

	// Token: 0x04002A20 RID: 10784
	[SerializeField]
	private RectMask2D rectMask2D;

	// Token: 0x04002A21 RID: 10785
	[SerializeField]
	private UIMultiAnswer.OverflowMode overflowMode;

	// Token: 0x04002A22 RID: 10786
	private static UIMultiAnswer instance;

	// Token: 0x04002A23 RID: 10787
	private static List<UIMultiAnswer> multiAnswers = new List<UIMultiAnswer>();

	// Token: 0x04002A24 RID: 10788
	private Transform targetTransform;

	// Token: 0x04002A25 RID: 10789
	private WgoData dialogParticipant;

	// Token: 0x04002A26 RID: 10790
	private Action<string> onChosen;

	// Token: 0x04002A27 RID: 10791
	private Action onDisappeared;

	// Token: 0x04002A28 RID: 10792
	private bool isMainMultianswer;

	// Token: 0x04002A29 RID: 10793
	private bool isControllable = true;

	// Token: 0x04002A2A RID: 10794
	[SerializeField]
	private List<UIMultiAnswerOption> answerOptions;

	// Token: 0x04002A2B RID: 10795
	private List<AnswerVisualData> visualData;

	// Token: 0x04002A2C RID: 10796
	private UIBasicBubble.ForceCornerPosition forcedCornerPosition;

	// Token: 0x04002A2D RID: 10797
	private UIMultiAnswerOption bottomOption;

	// Token: 0x04002A2E RID: 10798
	private bool interactable;

	// Token: 0x04002A2F RID: 10799
	private int maxIconsCount;

	// Token: 0x04002A30 RID: 10800
	private bool subscribedGamepadEvents;

	// Token: 0x04002A31 RID: 10801
	private bool opensDownward;

	// Token: 0x04002A32 RID: 10802
	private bool isScrollModeActive;

	// Token: 0x04002A33 RID: 10803
	private bool isSideDisplayActive;

	// Token: 0x04002A34 RID: 10804
	private VerticalLayoutGroup rootVerticalLayoutGroup;

	// Token: 0x04002A35 RID: 10805
	private int rootPaddingTopDefault = -1;

	// Token: 0x04002A36 RID: 10806
	private int rootPaddingBottomDefault = -1;

	// Token: 0x02000808 RID: 2056
	public enum OverflowMode
	{
		// Token: 0x04002A38 RID: 10808
		FullDisplayOnSide,
		// Token: 0x04002A39 RID: 10809
		Scroll
	}
}
