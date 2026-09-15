using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200080F RID: 2063
public class UIMultiAnswerOption : MonoBehaviour
{
	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x060034EF RID: 13551 RVA: 0x000FEB08 File Offset: 0x000FCD08
	public GamepadNavigationItem Item
	{
		get
		{
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x060034F0 RID: 13552 RVA: 0x000FEB10 File Offset: 0x000FCD10
	public CanvasGroup Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x060034F1 RID: 13553 RVA: 0x000FEB18 File Offset: 0x000FCD18
	public bool Available
	{
		get
		{
			return this.available;
		}
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x000FEB20 File Offset: 0x000FCD20
	private void Awake()
	{
		this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.OnItemOver), new UnityAction(this.OnItemOut), new UnityAction(this.OnItemPress));
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x000FEB54 File Offset: 0x000FCD54
	public void Show(AnswerVisualData visualData, UIMultiAnswer multiAnswer)
	{
		this.label.text = this.hightlightedTextStyle.TranslateAndColorizeTags(visualData.id);
		this.visualData = visualData;
		this.multiAnswer = multiAnswer;
		foreach (UIBubbleCorner uibubbleCorner in this.corners)
		{
			uibubbleCorner.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x000FEBE0 File Offset: 0x000FCDE0
	public void ShowIcons()
	{
		UIMultiAnswerOption.<>c__DisplayClass47_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		this.iconsCount = 0;
		AnswerVisualData answerVisualData = this.visualData;
		SmartRes smartRes;
		if (answerVisualData == null)
		{
			smartRes = null;
		}
		else
		{
			AnswerData answerData = answerVisualData.answerData;
			smartRes = ((answerData != null) ? answerData.lockRes : null);
		}
		SmartRes smartRes2 = smartRes;
		AnswerVisualData answerVisualData2 = this.visualData;
		SmartRes smartRes3;
		if (answerVisualData2 == null)
		{
			smartRes3 = null;
		}
		else
		{
			AnswerData answerData2 = answerVisualData2.answerData;
			smartRes3 = ((answerData2 != null) ? answerData2.costRes : null);
		}
		SmartRes smartRes4 = smartRes3;
		AnswerVisualData answerVisualData3 = this.visualData;
		string text;
		if (answerVisualData3 == null)
		{
			text = null;
		}
		else
		{
			AnswerData answerData3 = answerVisualData3.answerData;
			text = ((answerData3 != null) ? answerData3.dayNumber : null);
		}
		string text2 = text;
		AnswerVisualData answerVisualData4 = this.visualData;
		string text3;
		if (answerVisualData4 == null)
		{
			text3 = null;
		}
		else
		{
			AnswerData answerData4 = answerVisualData4.answerData;
			text3 = ((answerData4 != null) ? answerData4.order : null);
		}
		string text4 = text3;
		bool flag = !string.IsNullOrEmpty(text2);
		bool flag2 = !string.IsNullOrEmpty(text4);
		CS$<>8__locals1.playerData = MainGame.PlayerData;
		if (smartRes2 != null || smartRes4 != null || flag || flag2)
		{
			int num = 0;
			num += UIMultiAnswerOption.<ShowIcons>g__CountItems|47_0(smartRes2);
			num += UIMultiAnswerOption.<ShowIcons>g__CountItems|47_0(smartRes4);
			num += (flag ? 1 : 0);
			num += (flag2 ? 1 : 0);
			this.iconsCount += num;
			this.leftIconGroup.Show(num);
			this.leftArrow.gameObject.SetActive(false);
			this.lockIconImage.gameObject.SetActive(smartRes2 != null || flag || flag2);
			UIMultiAnswerOption.<>c__DisplayClass47_1 CS$<>8__locals2;
			CS$<>8__locals2.leftIndex = 0;
			List<GameResAtom> list;
			if (smartRes2 == null)
			{
				list = null;
			}
			else
			{
				GameRes gameRes = smartRes2.gameRes;
				list = ((gameRes != null) ? gameRes.List : null);
			}
			this.<ShowIcons>g__ProcessGameRes|47_2(list, true, ref CS$<>8__locals1, ref CS$<>8__locals2);
			List<GameResAtom> list2;
			if (smartRes4 == null)
			{
				list2 = null;
			}
			else
			{
				GameRes gameRes2 = smartRes4.gameRes;
				list2 = ((gameRes2 != null) ? gameRes2.List : null);
			}
			this.<ShowIcons>g__ProcessGameRes|47_2(list2, false, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<ShowIcons>g__ProcessItems|47_1((smartRes2 != null) ? smartRes2.items : null, true, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<ShowIcons>g__ProcessItems|47_1((smartRes4 != null) ? smartRes4.items : null, false, ref CS$<>8__locals1, ref CS$<>8__locals2);
			if (flag)
			{
				this.leftIconGroup.Icons[CS$<>8__locals2.leftIndex].SetupAsDay(text2, UIMultiAnswerIcon.DisplayType.DayNumber);
				if (MainGame.Instance.GameSave.environmentData.CurrentDayNumber != ConstDef.Get(text2).IntValue)
				{
					this.available = false;
				}
			}
			if (flag2)
			{
				this.leftIconGroup.Icons[CS$<>8__locals2.leftIndex].SetupAsOrder(text4, UIMultiAnswerIcon.DisplayType.Order);
				if (!MainGame.Instance.GameSave.vendorSystem.IsOrderFinished(text4))
				{
					this.available = false;
				}
			}
		}
		else
		{
			AnswerVisualData answerVisualData5 = this.visualData;
			if (answerVisualData5 != null)
			{
				AnswerData answerData5 = answerVisualData5.answerData;
				if (answerData5 != null && answerData5.notAvailable)
				{
					this.available = false;
				}
			}
			this.leftIconGroup.HideAll();
			this.leftArrow.gameObject.SetActive(true);
		}
		AnswerVisualData answerVisualData6 = this.visualData;
		SmartRes smartRes5;
		if (answerVisualData6 == null)
		{
			smartRes5 = null;
		}
		else
		{
			AnswerData answerData6 = answerVisualData6.answerData;
			smartRes5 = ((answerData6 != null) ? answerData6.rewardRes : null);
		}
		SmartRes smartRes6 = smartRes5;
		AnswerVisualData answerVisualData7 = this.visualData;
		SmartRes smartRes7;
		if (answerVisualData7 == null)
		{
			smartRes7 = null;
		}
		else
		{
			AnswerData answerData7 = answerVisualData7.answerData;
			smartRes7 = ((answerData7 != null) ? answerData7.fakeRewardRes : null);
		}
		SmartRes smartRes8 = smartRes7;
		if (smartRes6 != null || smartRes8 != null)
		{
			int num2 = 0;
			num2 += UIMultiAnswerOption.<ShowIcons>g__CountItems|47_3(smartRes6);
			num2 += UIMultiAnswerOption.<ShowIcons>g__CountItems|47_3(smartRes8);
			this.iconsCount += num2;
			this.rightIconGroup.Show(num2);
			this.rightArrow.gameObject.SetActive(true);
			UIMultiAnswerOption.<>c__DisplayClass47_2 CS$<>8__locals3;
			CS$<>8__locals3.rightIndex = 0;
			List<GameResAtom> list3;
			if (smartRes6 == null)
			{
				list3 = null;
			}
			else
			{
				GameRes gameRes3 = smartRes6.gameRes;
				list3 = ((gameRes3 != null) ? gameRes3.List : null);
			}
			this.<ShowIcons>g__ProcessGameRes|47_5(list3, ref CS$<>8__locals1, ref CS$<>8__locals3);
			List<GameResAtom> list4;
			if (smartRes8 == null)
			{
				list4 = null;
			}
			else
			{
				GameRes gameRes4 = smartRes8.gameRes;
				list4 = ((gameRes4 != null) ? gameRes4.List : null);
			}
			this.<ShowIcons>g__ProcessGameRes|47_5(list4, ref CS$<>8__locals1, ref CS$<>8__locals3);
			this.<ShowIcons>g__ProcessItems|47_4((smartRes6 != null) ? smartRes6.items : null, ref CS$<>8__locals1, ref CS$<>8__locals3);
			this.<ShowIcons>g__ProcessItems|47_4((smartRes8 != null) ? smartRes8.items : null, ref CS$<>8__locals1, ref CS$<>8__locals3);
		}
		else
		{
			this.rightIconGroup.HideAll();
			this.rightArrow.gameObject.SetActive(false);
		}
		if (!this.available)
		{
			this.lockIconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("ui_reply_lock_red", null);
			this.textStyleComponent.SetTextStyle(this.greyTextStyle);
		}
		else
		{
			this.lockIconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("ui_reply_lock_grn", null);
		}
		this.multiAnswer.MaxIconsCount = this.iconsCount;
		this.optionLayoutElement.minHeight = ((this.iconsCount > 0) ? 38f : 28f);
		this.midSectionElement.minHeight = this.optionLayoutElement.minHeight;
		this.optionLayoutElement.preferredWidth = 282f;
		this.midSectionElement.minWidth = 130f;
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x000FF066 File Offset: 0x000FD266
	public void ChangeInteractableByAnimation(bool isInteractable)
	{
		this.interactableByAnimation = isInteractable;
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x000FF070 File Offset: 0x000FD270
	public void AnimateAppearing(float animDuration)
	{
		this.canvas.alpha = 0f;
		this.labelCanvas.alpha = 1f;
		this.labelCanvas.ignoreParentGroups = true;
		this.canvas.DOFade(1f, animDuration).SetEase(Ease.Linear).onComplete = delegate
		{
			this.labelCanvas.ignoreParentGroups = false;
		};
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x00002318 File Offset: 0x00000518
	public void UpdateAnswerPosition()
	{
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x000FF0D4 File Offset: 0x000FD2D4
	public void OnItemOver()
	{
		if (!this.isOver)
		{
			this.isOver = true;
			this.ChangeColor(this.highlightedColor);
			Debug.Log(string.Format("[ma]:OnItemOver {0} - {1}", this.visualData.id, this.highlightedColor));
			if (this.multiAnswer != null && this.multiAnswer.IsScrollModeActive)
			{
				return;
			}
			this.contentRectTransform.sizeDelta = new Vector2(284f, this.contentRectTransform.sizeDelta.y);
			this.optionLayoutElement.preferredWidth = 284f;
			this.midSectionElement.minWidth = 132f;
		}
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x000FF188 File Offset: 0x000FD388
	public void OnItemOut()
	{
		if (this.isOver)
		{
			this.isOver = false;
			this.ChangeColor(this.originalColor);
			Debug.Log(string.Format("[ma]:OnItemOut {0} - {1}", this.visualData.id, this.originalColor));
			if (this.multiAnswer != null && this.multiAnswer.IsScrollModeActive)
			{
				return;
			}
			this.contentRectTransform.sizeDelta = new Vector2(282f, this.contentRectTransform.sizeDelta.y);
			this.optionLayoutElement.preferredWidth = 282f;
			this.midSectionElement.minWidth = 130f;
		}
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x000FF23C File Offset: 0x000FD43C
	public void OnItemPress()
	{
		if (!this.forceSelectionNoHandle && (!this.available || !this.interactableByAnimation))
		{
			return;
		}
		if (this.forceSelectionNoHandle)
		{
			this.multiAnswer.OnAnswerSelect(this.visualData.id);
			return;
		}
		AnswerData answerData = this.visualData.answerData;
		if (answerData != null)
		{
			if (answerData.costRes != null)
			{
				this.HandlePrice(this.visualData.answerData.costRes);
			}
			if (answerData.rewardRes != null)
			{
				this.HandleRewards(this.visualData.answerData.rewardRes);
			}
		}
		Debug.Log("[ma]: " + this.visualData.id);
		this.multiAnswer.OnAnswerSelect(this.visualData.id);
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x000FF2FC File Offset: 0x000FD4FC
	public void ChangeColor(Color color)
	{
		this.KillColorTweens();
		this.activeColorTweens.Add(this.optionButtonImage.DOColor(color, 0.5f));
		if (this.rightArrow.gameObject.activeInHierarchy)
		{
			this.activeColorTweens.Add(this.rightArrow.GetComponent<Image>().DOColor(color, 1f));
		}
		if (this.leftArrow.gameObject.activeInHierarchy)
		{
			this.activeColorTweens.Add(this.leftArrow.GetComponent<Image>().DOColor(color, 1f));
		}
		foreach (UIBubbleCorner uibubbleCorner in this.corners)
		{
			if (uibubbleCorner.gameObject.activeInHierarchy)
			{
				this.activeColorTweens.Add(uibubbleCorner.customImage.DOColor(color, 1f));
			}
		}
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x000FF3FC File Offset: 0x000FD5FC
	private void OnDestroy()
	{
		this.KillColorTweens();
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x000FF404 File Offset: 0x000FD604
	private void KillColorTweens()
	{
		foreach (Tween tween in this.activeColorTweens)
		{
			tween.Kill(false);
		}
		this.activeColorTweens.Clear();
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x000FF460 File Offset: 0x000FD660
	private void HandlePrice(SmartRes priceSmartRes)
	{
		if (priceSmartRes.items != null)
		{
			foreach (ItemCount itemCount in priceSmartRes.items)
			{
				MainGame.PlayerData.Inventory.RemoveItemById(itemCount.itemId, itemCount.count, null, null, false);
			}
		}
		if (priceSmartRes.gameRes != null)
		{
			MainGame.PlayerData.SubRes(this.visualData.answerData.costRes.gameRes);
		}
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x000FF500 File Offset: 0x000FD700
	private void HandleRewards(SmartRes rewardSmartRes)
	{
		if (rewardSmartRes.items != null)
		{
			List<Item> list = ItemCount.CreateItems(rewardSmartRes.items);
			if (!MainGame.PlayerData.Inventory.AddItemsToInventory(list))
			{
				foreach (Item item in list)
				{
					MainGame.Instance.dropSystem.DropItem(item, MainGame.PlayerData.currentGameSceneId, MainGame.PlayerController.PlayerData.position.Value, null);
				}
			}
		}
		if (rewardSmartRes.gameRes != null)
		{
			Debug.Log(string.Format("[Is playerdata is null - {0}], is gameRes is null - [{1}]", MainGame.PlayerData == null, this.visualData.answerData.rewardRes.gameRes == null));
			MainGame.PlayerData.AddRes(this.visualData.answerData.rewardRes.gameRes);
		}
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x000FF608 File Offset: 0x000FD808
	public float GetExtraWidth()
	{
		return this.GetRightIconsExtraWidth();
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x000FF610 File Offset: 0x000FD810
	public float GetLeftIconsExtraWidth()
	{
		return UIMultiAnswerOption.GetActiveIconGroupExtraWidth(this.leftIconGroup);
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x000FF61D File Offset: 0x000FD81D
	public float GetRightIconsExtraWidth()
	{
		return UIMultiAnswerOption.GetActiveIconGroupExtraWidth(this.rightIconGroup);
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x000FF62C File Offset: 0x000FD82C
	private static float GetActiveIconGroupExtraWidth(UIMultiAnswerIconGroup iconGroup)
	{
		if (!iconGroup.gameObject.activeInHierarchy)
		{
			return 0f;
		}
		int num = 0;
		using (List<UIMultiAnswerIcon>.Enumerator enumerator = iconGroup.Icons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.gameObject.activeInHierarchy)
				{
					num++;
				}
			}
		}
		if (num != 0)
		{
			return 35f * (float)num + (float)num;
		}
		return 0f;
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x000FF6B0 File Offset: 0x000FD8B0
	public RectTransform GetCornerTransform(UIBasicBubble.BubbleCornerDirection corner)
	{
		return this.corners[(int)corner].rectTransform;
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x000FF6C3 File Offset: 0x000FD8C3
	public RectTransform GetBoundsRectTransform()
	{
		return (RectTransform)base.transform;
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x000FF6F4 File Offset: 0x000FD8F4
	[CompilerGenerated]
	internal static int <ShowIcons>g__CountItems|47_0(SmartRes res)
	{
		if (res != null)
		{
			GameRes gameRes = res.gameRes;
			int? num;
			if (gameRes == null)
			{
				num = null;
			}
			else
			{
				List<GameResAtom> list = gameRes.List;
				num = ((list != null) ? new int?(list.Count) : null);
			}
			int? num2 = num;
			int valueOrDefault = num2.GetValueOrDefault();
			List<ItemCount> items = res.items;
			return valueOrDefault + ((items != null) ? items.Count : 0);
		}
		return 0;
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x000FF754 File Offset: 0x000FD954
	[CompilerGenerated]
	private void <ShowIcons>g__ProcessItems|47_1(List<ItemCount> items, bool isLock, ref UIMultiAnswerOption.<>c__DisplayClass47_0 A_3, ref UIMultiAnswerOption.<>c__DisplayClass47_1 A_4)
	{
		if (items == null)
		{
			return;
		}
		foreach (ItemCount itemCount in items)
		{
			UIMultiAnswerIcon uimultiAnswerIcon = this.leftIconGroup.Icons[A_4.leftIndex];
			UIMultiAnswerIcon.DisplayType displayType = (isLock ? UIMultiAnswerIcon.DisplayType.Lock : UIMultiAnswerIcon.DisplayType.Price);
			uimultiAnswerIcon.Setup(itemCount, displayType);
			if (!A_3.playerData.Inventory.Data.HasItemQuantityInInventory(itemCount.itemId, itemCount.count))
			{
				this.available = false;
			}
			int leftIndex = A_4.leftIndex;
			A_4.leftIndex = leftIndex + 1;
		}
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000FF804 File Offset: 0x000FDA04
	[CompilerGenerated]
	private void <ShowIcons>g__ProcessGameRes|47_2(List<GameResAtom> resList, bool isLock, ref UIMultiAnswerOption.<>c__DisplayClass47_0 A_3, ref UIMultiAnswerOption.<>c__DisplayClass47_1 A_4)
	{
		if (resList == null)
		{
			return;
		}
		foreach (GameResAtom gameResAtom in resList)
		{
			UIMultiAnswerIcon uimultiAnswerIcon = this.leftIconGroup.Icons[A_4.leftIndex];
			UIMultiAnswerIcon.DisplayType displayType = (isLock ? UIMultiAnswerIcon.DisplayType.Lock : UIMultiAnswerIcon.DisplayType.Price);
			uimultiAnswerIcon.Setup(gameResAtom, displayType);
			if (!A_3.playerData.IsEnoughRes(gameResAtom))
			{
				this.available = false;
			}
			int leftIndex = A_4.leftIndex;
			A_4.leftIndex = leftIndex + 1;
		}
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x000FF89C File Offset: 0x000FDA9C
	[CompilerGenerated]
	internal static int <ShowIcons>g__CountItems|47_3(SmartRes res)
	{
		if (res != null)
		{
			GameRes gameRes = res.gameRes;
			int? num;
			if (gameRes == null)
			{
				num = null;
			}
			else
			{
				List<GameResAtom> list = gameRes.List;
				num = ((list != null) ? new int?(list.Count) : null);
			}
			int? num2 = num;
			int valueOrDefault = num2.GetValueOrDefault();
			List<ItemCount> items = res.items;
			return valueOrDefault + ((items != null) ? items.Count : 0);
		}
		return 0;
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x000FF8FC File Offset: 0x000FDAFC
	[CompilerGenerated]
	private void <ShowIcons>g__ProcessItems|47_4(List<ItemCount> items, ref UIMultiAnswerOption.<>c__DisplayClass47_0 A_2, ref UIMultiAnswerOption.<>c__DisplayClass47_2 A_3)
	{
		if (items == null)
		{
			return;
		}
		foreach (ItemCount itemCount in items)
		{
			this.rightIconGroup.Icons[A_3.rightIndex].Setup(itemCount, UIMultiAnswerIcon.DisplayType.Reward);
			int rightIndex = A_3.rightIndex;
			A_3.rightIndex = rightIndex + 1;
		}
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x000FF974 File Offset: 0x000FDB74
	[CompilerGenerated]
	private void <ShowIcons>g__ProcessGameRes|47_5(List<GameResAtom> resList, ref UIMultiAnswerOption.<>c__DisplayClass47_0 A_2, ref UIMultiAnswerOption.<>c__DisplayClass47_2 A_3)
	{
		if (resList == null)
		{
			return;
		}
		foreach (GameResAtom gameResAtom in resList)
		{
			this.rightIconGroup.Icons[A_3.rightIndex].Setup(gameResAtom, UIMultiAnswerIcon.DisplayType.Reward);
			int rightIndex = A_3.rightIndex;
			A_3.rightIndex = rightIndex + 1;
		}
	}

	// Token: 0x04002A5F RID: 10847
	private const float OFFSET_ONE_ICON = 35f;

	// Token: 0x04002A60 RID: 10848
	private const float MIN_HEIGHT_OPTION = 28f;

	// Token: 0x04002A61 RID: 10849
	private const float MIN_HEIGHT_OPTION_EXTENDED = 38f;

	// Token: 0x04002A62 RID: 10850
	private const float PREFERRED_WIDTH_OPTION_DEFAULT = 282f;

	// Token: 0x04002A63 RID: 10851
	private const int TEXT_PADDING_SIDE_DEFAULT = 9;

	// Token: 0x04002A64 RID: 10852
	private const float MIN_WIDTH_MID_SECTION_DEFAULT = 130f;

	// Token: 0x04002A65 RID: 10853
	private const string LOCK_ICON_GREEN = "ui_reply_lock_grn";

	// Token: 0x04002A66 RID: 10854
	private const string LOCK_ICON_RED = "ui_reply_lock_red";

	// Token: 0x04002A67 RID: 10855
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002A68 RID: 10856
	[SerializeField]
	private CanvasGroup canvas;

	// Token: 0x04002A69 RID: 10857
	[SerializeField]
	private CanvasGroup labelCanvas;

	// Token: 0x04002A6A RID: 10858
	[SerializeField]
	private TextStyleComponent textStyleComponent;

	// Token: 0x04002A6B RID: 10859
	[SerializeField]
	private TextStyle greyTextStyle;

	// Token: 0x04002A6C RID: 10860
	[SerializeField]
	private TextStyle hightlightedTextStyle;

	// Token: 0x04002A6D RID: 10861
	[Space]
	[SerializeField]
	private List<UIBubbleCorner> corners;

	// Token: 0x04002A6E RID: 10862
	[SerializeField]
	private List<Image> cornerImages;

	// Token: 0x04002A6F RID: 10863
	[SerializeField]
	private Image optionButtonImage;

	// Token: 0x04002A70 RID: 10864
	[SerializeField]
	private Color originalColor;

	// Token: 0x04002A71 RID: 10865
	[SerializeField]
	private Color highlightedColor;

	// Token: 0x04002A72 RID: 10866
	[SerializeField]
	private LayoutElement optionLayoutElement;

	// Token: 0x04002A73 RID: 10867
	[SerializeField]
	private LayoutElement midSectionElement;

	// Token: 0x04002A74 RID: 10868
	[SerializeField]
	private RectTransform contentRectTransform;

	// Token: 0x04002A75 RID: 10869
	[Space]
	[SerializeField]
	private UIMultiAnswerIconGroup leftIconGroup;

	// Token: 0x04002A76 RID: 10870
	[SerializeField]
	private UIMultiAnswerIconGroup rightIconGroup;

	// Token: 0x04002A77 RID: 10871
	[Space]
	[SerializeField]
	private RectTransform leftArrow;

	// Token: 0x04002A78 RID: 10872
	[SerializeField]
	private RectTransform rightArrow;

	// Token: 0x04002A79 RID: 10873
	[SerializeField]
	private Image lockIconImage;

	// Token: 0x04002A7A RID: 10874
	[Space]
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x04002A7B RID: 10875
	public int iconsCount;

	// Token: 0x04002A7C RID: 10876
	private RectTransform currentBackground;

	// Token: 0x04002A7D RID: 10877
	private Image currentBackgroundImage;

	// Token: 0x04002A7E RID: 10878
	private UIMultiAnswer multiAnswer;

	// Token: 0x04002A7F RID: 10879
	private AnswerVisualData visualData;

	// Token: 0x04002A80 RID: 10880
	private bool isOver;

	// Token: 0x04002A81 RID: 10881
	private bool isLockIcons;

	// Token: 0x04002A82 RID: 10882
	private bool available = true;

	// Token: 0x04002A83 RID: 10883
	private bool forceSelectionNoHandle;

	// Token: 0x04002A84 RID: 10884
	private bool interactableByAnimation = true;

	// Token: 0x04002A85 RID: 10885
	private List<Tween> activeColorTweens = new List<Tween>();
}
