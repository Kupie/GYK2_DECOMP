using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200080B RID: 2059
public class UIMultiAnswerIcon : MonoBehaviour
{
	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000FE31A File Offset: 0x000FC51A
	public ItemCount ItemCount
	{
		get
		{
			return this.itemCount;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x060034D6 RID: 13526 RVA: 0x000FE322 File Offset: 0x000FC522
	public VendorOrderDef VendorOrderDef
	{
		get
		{
			return this.vendorOrderDef;
		}
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x000FE32C File Offset: 0x000FC52C
	private void Awake()
	{
		this.lazyButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.lazyButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.lazyButton.SetCallbacksIntoGamepadNavigationItem();
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x000707FD File Offset: 0x0006E9FD
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x000FE37C File Offset: 0x000FC57C
	public void Setup(ItemCount item, UIMultiAnswerIcon.DisplayType displayType)
	{
		this.lazyButton.interactable = true;
		this.iconGameObject.SetActive(true);
		this.textRes.gameObject.SetActive(false);
		this.itemCount = item;
		bool flag = item.Def.qualityType == ItemDef.QualityType.Star && item.Def.quality > 0;
		bool flag2 = (this.iconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(item.Def.iconId, UIMultiAnswerIcon.PLACEHOLDER_ICON_NAME));
		this.itemStar.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + item.Def.quality.ToString(), null);
		this.itemStar.gameObject.SetActive(flag);
		if (displayType == UIMultiAnswerIcon.DisplayType.Reward)
		{
			this.textStyleComponentItemCount.SetTextStyle(this.availableTextStyle);
		}
		else
		{
			this.textStyleComponentItemCount.SetTextStyle((MainGame.PlayerData.Inventory.Data.GetTotalCountInInventory(item.itemId, null, false) >= item.count) ? this.availableTextStyle : this.unavailableTextStyle);
		}
		int num = MainGame.PlayerData.Inventory.Data.GetTotalCountInInventory(item.itemId, null, false);
		num = ((num > this.itemCount.count) ? this.itemCount.count : num);
		this.countLabel.text = string.Empty;
		switch (displayType)
		{
		case UIMultiAnswerIcon.DisplayType.Price:
		{
			TextMeshProUGUI textMeshProUGUI = this.countLabel;
			textMeshProUGUI.text += string.Format("{0}/{1}", num, item.count);
			this.countLabel.gameObject.SetActive(true);
			return;
		}
		case UIMultiAnswerIcon.DisplayType.Reward:
			this.countLabel.text = string.Format("{0}", item.count);
			this.countLabel.gameObject.SetActive(flag2);
			return;
		case UIMultiAnswerIcon.DisplayType.Lock:
		{
			TextMeshProUGUI textMeshProUGUI2 = this.countLabel;
			textMeshProUGUI2.text += string.Format("{0}/{1}", num, item.count);
			this.countLabel.gameObject.SetActive(true);
			return;
		}
		default:
			this.countLabel.gameObject.SetActive(flag2);
			return;
		}
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x000FE5C6 File Offset: 0x000FC7C6
	private void OnDestroy()
	{
		this.KillColorTweens();
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x000FE5D0 File Offset: 0x000FC7D0
	private void KillColorTweens()
	{
		foreach (Tween tween in this.activeColorTweens)
		{
			tween.Kill(false);
		}
		this.activeColorTweens.Clear();
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x000FE62C File Offset: 0x000FC82C
	public void ChangeColor(bool selected)
	{
		this.KillColorTweens();
		this.activeColorTweens.Add(this.decorImage.DOColor(selected ? this.selectedDecorColor : this.normalDecorColor, 0.5f));
		this.activeColorTweens.Add(this.backImage.DOColor(selected ? this.selectedArrowColor : this.normalArrowColor, 0.5f));
		if (this.arrowImage != null)
		{
			this.activeColorTweens.Add(this.arrowImage.DOColor(selected ? this.selectedArrowColor : this.normalArrowColor, 0.5f));
		}
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x000FE6D4 File Offset: 0x000FC8D4
	public void Setup(GameResAtom res, UIMultiAnswerIcon.DisplayType displayType)
	{
		this.lazyButton.interactable = false;
		this.iconGameObject.SetActive(false);
		this.textRes.gameObject.SetActive(true);
		float res2 = MainGame.PlayerData.GetRes(res.type, 0f);
		this.textRes.text = res.ToFormattedString(false, delegate(string s, string s1)
		{
			if (displayType == UIMultiAnswerIcon.DisplayType.Reward)
			{
				s1 = "+" + s1;
			}
			return s + "\n" + s1;
		}, false, true, null);
		if (displayType != UIMultiAnswerIcon.DisplayType.Reward)
		{
			this.textStyleComponentRes.SetTextStyle((res2 >= res.value) ? this.availableTextStyle : this.unavailableTextStyle);
			return;
		}
		this.textStyleComponentRes.SetTextStyle(this.availableTextStyle);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x000FE78C File Offset: 0x000FC98C
	public void SetupAsDay(string dayNumber, UIMultiAnswerIcon.DisplayType displayType)
	{
		this.lazyButton.interactable = true;
		this.iconGameObject.SetActive(true);
		this.textRes.gameObject.SetActive(false);
		this.iconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(dayNumber, UIMultiAnswerIcon.PLACEHOLDER_ICON_NAME);
		this.textStyleComponentItemCount.SetTextStyle((MainGame.Instance.GameSave.environmentData.CurrentDayNumber == ConstDef.Get(dayNumber).IntValue) ? this.availableTextStyle : this.unavailableTextStyle);
		this.countLabel.text = string.Empty;
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x000FE828 File Offset: 0x000FCA28
	public void SetupAsOrder(string order, UIMultiAnswerIcon.DisplayType displayType)
	{
		this.vendorOrderDef = GameBalance.Me.GetData<VendorOrderDef>(order);
		this.lazyButton.interactable = true;
		this.iconGameObject.SetActive(true);
		this.textRes.gameObject.SetActive(false);
		ItemDef data = GameBalance.Me.GetData<ItemDef>(this.vendorOrderDef.itemId);
		this.iconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(data.iconId, UIMultiAnswerIcon.PLACEHOLDER_ICON_NAME);
		this.itemStar.gameObject.SetActive(false);
		this.countLabel.gameObject.SetActive(false);
		this.textStyleComponentItemCount.SetTextStyle(MainGame.Instance.GameSave.vendorSystem.IsOrderFinished(order) ? this.availableTextStyle : this.unavailableTextStyle);
		this.countLabel.text = string.Empty;
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x000FE907 File Offset: 0x000FCB07
	private void OnOver()
	{
		if (!this.lazyButton.interactable)
		{
			return;
		}
		this.ShowUITooltip();
		this.iconImage.BlueColorReplace(this.selectedItemIconColor);
		this.ChangeColor(true);
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x000FE935 File Offset: 0x000FCB35
	private void OnOut()
	{
		if (!this.lazyButton.interactable)
		{
			return;
		}
		this.HideUITooltip(false);
		this.iconImage.BlueColorReplace(this.normalItemIconColor);
		this.ChangeColor(false);
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x000FE964 File Offset: 0x000FCB64
	private void OnDisable()
	{
		if (this.isHovered)
		{
			if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
			{
				this.HideUITooltip(true);
			}
			this.iconImage.BlueColorReplace(this.normalItemIconColor);
			this.decorImage.color = this.normalDecorColor;
		}
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x000FE9B4 File Offset: 0x000FCBB4
	public void ShowUITooltip()
	{
		this.isHovered = true;
		UITooltip.ShowMultiAnswerIcon(this);
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x000FE9C3 File Offset: 0x000FCBC3
	public void HideUITooltip(bool immediately = false)
	{
		this.isHovered = false;
		if (immediately)
		{
			UITooltip.HideImmediately();
			return;
		}
		UITooltip.Hide();
	}

	// Token: 0x04002A3F RID: 10815
	private static string PLACEHOLDER_ICON_NAME = "placeholder_icon";

	// Token: 0x04002A40 RID: 10816
	[SerializeField]
	private LazyButton lazyButton;

	// Token: 0x04002A41 RID: 10817
	[SerializeField]
	private GameObject iconGameObject;

	// Token: 0x04002A42 RID: 10818
	[SerializeField]
	private Image iconImage;

	// Token: 0x04002A43 RID: 10819
	[SerializeField]
	private Image backImage;

	// Token: 0x04002A44 RID: 10820
	[SerializeField]
	private Image decorImage;

	// Token: 0x04002A45 RID: 10821
	[SerializeField]
	private Image arrowImage;

	// Token: 0x04002A46 RID: 10822
	[SerializeField]
	private Image itemStar;

	// Token: 0x04002A47 RID: 10823
	[SerializeField]
	private TextMeshProUGUI countLabel;

	// Token: 0x04002A48 RID: 10824
	[SerializeField]
	private TextMeshProUGUI textRes;

	// Token: 0x04002A49 RID: 10825
	[SerializeField]
	private TextStyleComponent textStyleComponentRes;

	// Token: 0x04002A4A RID: 10826
	[SerializeField]
	private TextStyleComponent textStyleComponentItemCount;

	// Token: 0x04002A4B RID: 10827
	[SerializeField]
	private TextStyle availableTextStyle;

	// Token: 0x04002A4C RID: 10828
	[SerializeField]
	private TextStyle unavailableTextStyle;

	// Token: 0x04002A4D RID: 10829
	[SerializeField]
	private Color normalItemIconColor;

	// Token: 0x04002A4E RID: 10830
	[SerializeField]
	private Color selectedItemIconColor;

	// Token: 0x04002A4F RID: 10831
	[SerializeField]
	private Color normalDecorColor;

	// Token: 0x04002A50 RID: 10832
	[SerializeField]
	private Color selectedDecorColor;

	// Token: 0x04002A51 RID: 10833
	[SerializeField]
	private Color normalArrowColor;

	// Token: 0x04002A52 RID: 10834
	[SerializeField]
	private Color selectedArrowColor;

	// Token: 0x04002A53 RID: 10835
	private bool isHovered;

	// Token: 0x04002A54 RID: 10836
	private ItemCount itemCount;

	// Token: 0x04002A55 RID: 10837
	private VendorOrderDef vendorOrderDef;

	// Token: 0x04002A56 RID: 10838
	private List<Tween> activeColorTweens = new List<Tween>();

	// Token: 0x0200080C RID: 2060
	public enum DisplayType
	{
		// Token: 0x04002A58 RID: 10840
		Price,
		// Token: 0x04002A59 RID: 10841
		Reward,
		// Token: 0x04002A5A RID: 10842
		Lock,
		// Token: 0x04002A5B RID: 10843
		DayNumber,
		// Token: 0x04002A5C RID: 10844
		Order
	}
}
