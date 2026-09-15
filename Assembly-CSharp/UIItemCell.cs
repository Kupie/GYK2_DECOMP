using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008BE RID: 2238
public class UIItemCell : MonoBehaviour
{
	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00116534 File Offset: 0x00114734
	public LazyButton LazyButton
	{
		get
		{
			return this.lazyButton;
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06003A2B RID: 14891 RVA: 0x0011653C File Offset: 0x0011473C
	public Image StarIcon
	{
		get
		{
			return this.starIcon;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06003A2C RID: 14892 RVA: 0x00116544 File Offset: 0x00114744
	public Image StatusIcon
	{
		get
		{
			return this.statusIcon;
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06003A2D RID: 14893 RVA: 0x0011654C File Offset: 0x0011474C
	// (set) Token: 0x06003A2E RID: 14894 RVA: 0x00116554 File Offset: 0x00114754
	public int Value
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06003A2F RID: 14895 RVA: 0x0011655D File Offset: 0x0011475D
	// (set) Token: 0x06003A30 RID: 14896 RVA: 0x00116565 File Offset: 0x00114765
	public bool NoSelectionFrames
	{
		get
		{
			return this.noSelectionFrames;
		}
		set
		{
			this.noSelectionFrames = value;
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06003A31 RID: 14897 RVA: 0x0011656E File Offset: 0x0011476E
	// (set) Token: 0x06003A32 RID: 14898 RVA: 0x00116576 File Offset: 0x00114776
	public bool ShowMouseSelectionFrame
	{
		get
		{
			return this.showMouseSelectionFrame;
		}
		set
		{
			this.showMouseSelectionFrame = value;
		}
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x06003A33 RID: 14899 RVA: 0x0011657F File Offset: 0x0011477F
	// (set) Token: 0x06003A34 RID: 14900 RVA: 0x00116587 File Offset: 0x00114787
	public TooltipPlacementPriority TooltipPlacementPriority
	{
		get
		{
			return this.tooltipPlacementPriority;
		}
		set
		{
			this.tooltipPlacementPriority = value;
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x06003A35 RID: 14901 RVA: 0x00116590 File Offset: 0x00114790
	// (set) Token: 0x06003A36 RID: 14902 RVA: 0x00116598 File Offset: 0x00114798
	public int HasItemCount
	{
		get
		{
			return this.hasItemCount;
		}
		set
		{
			this.hasItemCount = value;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x06003A37 RID: 14903 RVA: 0x001165A1 File Offset: 0x001147A1
	public Item DisplayingItem
	{
		get
		{
			return this.displayingItem;
		}
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x06003A38 RID: 14904 RVA: 0x001165A9 File Offset: 0x001147A9
	public OutputPreview DisplayingOutputPreview
	{
		get
		{
			return this.displayingOutputPreview;
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x06003A39 RID: 14905 RVA: 0x001165B1 File Offset: 0x001147B1
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			this.TryInitGamepadNavigationItem();
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x06003A3A RID: 14906 RVA: 0x001165BF File Offset: 0x001147BF
	public bool IsInteractable
	{
		get
		{
			return this.isInteractable;
		}
	}

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x06003A3B RID: 14907 RVA: 0x001165C7 File Offset: 0x001147C7
	public Image Background
	{
		get
		{
			return this.background;
		}
	}

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x001165CF File Offset: 0x001147CF
	public Image Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x001165D7 File Offset: 0x001147D7
	private void Awake()
	{
		this.TryInitGamepadNavigationItem();
		this.CacheDefaultStatusIconPosition();
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x001165E8 File Offset: 0x001147E8
	public void SetWidgetState(ItemRelatedWidgetState state)
	{
		this.widgetState = state;
		this.isSelected = state == ItemRelatedWidgetState.Selected;
		switch (state)
		{
		case ItemRelatedWidgetState.Default:
		case ItemRelatedWidgetState.Selected:
			this.background.color = Color.white;
			this.nonInteractableItemImg.gameObject.SetActive(false);
			break;
		case ItemRelatedWidgetState.Disabled:
			this.background.color = this.backColorEmpty;
			this.nonInteractableItemImg.gameObject.SetActive(true);
			break;
		case ItemRelatedWidgetState.Inactive:
			this.background.color = this.backColorEmpty;
			this.nonInteractableItemImg.gameObject.SetActive(false);
			break;
		case ItemRelatedWidgetState.NotSet:
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
		this.RefreshSelectionFrame();
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x001166A8 File Offset: 0x001148A8
	private void RefreshSelectionFrame()
	{
		if (this.isSelected && !this.noSelectionFrames)
		{
			if (this.selection != null)
			{
				this.selection.gameObject.SetActive(true);
			}
			if (this.selectionNonInteractable != null)
			{
				this.selectionNonInteractable.gameObject.SetActive(false);
			}
			return;
		}
		if (!this.isHovered)
		{
			this.HideSelectionFrame();
		}
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x00116712 File Offset: 0x00114912
	private void HideSelectionFrame()
	{
		if (this.selection != null)
		{
			this.selection.gameObject.SetActive(false);
		}
		if (this.selectionNonInteractable != null)
		{
			this.selectionNonInteractable.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x00116754 File Offset: 0x00114954
	public void Draw(Item item, bool isNeedItem = false, int hasItemCount = -1, bool isCraftResult = false, int multiplier = 1, bool drawAsNonInteractable = false, int price = 0, bool drawCounter = true, bool forceNonEmpty = false, bool forceDrawCounter = false, ItemRelatedWidgetState customState = ItemRelatedWidgetState.NotSet, bool noSelectionFrames = false)
	{
		if (customState == ItemRelatedWidgetState.NotSet)
		{
			this.SetWidgetState(drawAsNonInteractable ? ItemRelatedWidgetState.Disabled : ItemRelatedWidgetState.Default);
		}
		else
		{
			this.SetWidgetState(drawAsNonInteractable ? ItemRelatedWidgetState.Disabled : customState);
		}
		if (item == null || (item.IsEmpty && !forceNonEmpty))
		{
			this.DrawEmpty(drawAsNonInteractable, false, noSelectionFrames);
			return;
		}
		this.displayingItem = item;
		this.noSelectionFrames = noSelectionFrames;
		this.showMouseSelectionFrame = true;
		this.tooltipPlacementPriority = TooltipPlacementPriority.TopRight;
		this.isInteractable = !drawAsNonInteractable;
		string id = item.id;
		this.value = item.Count;
		this.hasItemCount = hasItemCount;
		this.isNeedItem = isNeedItem;
		this.drawCounter = drawCounter;
		Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(item.Definition.iconId, null);
		if (sprite == null)
		{
			this.DrawPlaceHolder(id, isCraftResult, noSelectionFrames);
		}
		else
		{
			this.icon.gameObject.SetActive(true);
			this.iconLabelPlaceholder.gameObject.SetActive(false);
			this.icon.BlueColorReplace(this.colors.NormalColor);
			this.icon.sprite = sprite;
		}
		this.UpdateCountLabel(multiplier, forceDrawCounter);
		if (item.Definition.qualityType == ItemDef.QualityType.Star)
		{
			this.starIcon.gameObject.SetActive(true);
			Sprite sprite2 = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + item.Definition.quality.ToString(), null);
			this.starIcon.sprite = sprite2;
		}
		else
		{
			this.starIcon.gameObject.SetActive(false);
		}
		if (price != 0)
		{
			this.UpdatePriceLabel(price);
		}
		else
		{
			this.ClearPriceLabel();
		}
		this.UpdateStatusIcon(CraftStatus.OK, ItemType.None);
		this.CheckPreviousTooltip();
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x001168F4 File Offset: 0x00114AF4
	public void Draw(ItemCount itemCount, bool isNeedItem = false, int hasItemCount = -1, bool isCraftResult = false, int multiplier = 1, bool drawAsNonInteractable = false, int price = 0, bool drawCounter = true, ItemRelatedWidgetState customState = ItemRelatedWidgetState.NotSet, bool noSelectionFrames = false)
	{
		if (customState == ItemRelatedWidgetState.NotSet)
		{
			this.SetWidgetState(drawAsNonInteractable ? ItemRelatedWidgetState.Disabled : ItemRelatedWidgetState.Default);
		}
		else
		{
			this.SetWidgetState(drawAsNonInteractable ? ItemRelatedWidgetState.Disabled : customState);
		}
		if (itemCount == null || itemCount.IsEmpty)
		{
			this.DrawEmpty(drawAsNonInteractable, false, noSelectionFrames);
			return;
		}
		this.noSelectionFrames = noSelectionFrames;
		this.showMouseSelectionFrame = true;
		this.tooltipPlacementPriority = TooltipPlacementPriority.TopRight;
		this.displayingItemCount = itemCount;
		this.isInteractable = !drawAsNonInteractable;
		string itemId = itemCount.itemId;
		this.value = itemCount.count;
		this.hasItemCount = hasItemCount;
		this.isNeedItem = isNeedItem;
		this.drawCounter = drawCounter;
		Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemCount.Def.iconId, null);
		if (sprite == null)
		{
			this.DrawPlaceHolder(itemId, isCraftResult, noSelectionFrames);
		}
		else
		{
			this.icon.gameObject.SetActive(true);
			this.iconLabelPlaceholder.gameObject.SetActive(false);
			this.icon.BlueColorReplace(this.colors.NormalColor);
			this.icon.sprite = sprite;
		}
		this.UpdateCountLabel(multiplier, false);
		if (itemCount.Def.qualityType == ItemDef.QualityType.Star)
		{
			this.starIcon.gameObject.SetActive(true);
			Sprite sprite2 = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + itemCount.Def.quality.ToString(), null);
			this.starIcon.sprite = sprite2;
		}
		else
		{
			this.starIcon.gameObject.SetActive(false);
		}
		if (price != 0)
		{
			this.UpdatePriceLabel(price);
		}
		else
		{
			this.ClearPriceLabel();
		}
		this.UpdateStatusIcon(CraftStatus.OK, ItemType.None);
		this.CheckPreviousTooltip();
	}

	// Token: 0x06003A43 RID: 14915 RVA: 0x00116A90 File Offset: 0x00114C90
	public void DrawPlaceHolder(string itemId, bool isCraftResult, bool noSelectionFrames = false)
	{
		this.iconLabelPlaceholder.gameObject.SetActive(true);
		this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(isCraftResult ? "i_b_blueprint_placeholder" : "i_placeholder", null);
		this.icon.gameObject.SetActive(true);
		this.iconLabelPlaceholder.text = itemId;
		this.iconLabelPlaceholder.gameObject.SetActive(true);
		this.starIcon.gameObject.SetActive(false);
		this.noSelectionFrames = noSelectionFrames;
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x00116B1C File Offset: 0x00114D1C
	public void DrawCraftOutput(OutputPreview outputPreview, int customQuality = -1, CraftStatus craftStatus = CraftStatus.OK, ItemType requiredToolType = ItemType.None, int multiplier = 1, ItemRelatedWidgetState customState = ItemRelatedWidgetState.NotSet, bool noSelectionFrames = false)
	{
		if (customState == ItemRelatedWidgetState.NotSet)
		{
			this.SetWidgetState(ItemRelatedWidgetState.Default);
		}
		else
		{
			this.SetWidgetState(customState);
		}
		if (outputPreview == null)
		{
			this.DrawEmpty(false, false, noSelectionFrames);
			return;
		}
		this.noSelectionFrames = noSelectionFrames;
		this.showMouseSelectionFrame = true;
		this.tooltipPlacementPriority = TooltipPlacementPriority.TopRight;
		this.displayingOutputPreview = outputPreview;
		this.isInteractable = true;
		string itemId = outputPreview.itemId;
		this.value = outputPreview.count;
		this.drawCounter = true;
		Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(outputPreview.IconId, null);
		if (sprite == null)
		{
			this.DrawPlaceHolder(itemId, true, noSelectionFrames);
		}
		else
		{
			this.icon.gameObject.SetActive(true);
			this.iconLabelPlaceholder.gameObject.SetActive(false);
			this.icon.BlueColorReplace(this.colors.NormalColor);
			this.icon.sprite = sprite;
		}
		this.UpdateQualityIcon((customQuality > -1) ? customQuality : outputPreview.quality);
		this.UpdateCountLabel(multiplier, false);
		this.ClearPriceLabel();
		this.UpdateStatusIcon(craftStatus, requiredToolType);
		this.CheckPreviousTooltip();
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x00116C28 File Offset: 0x00114E28
	public void DrawCustom(string iconId, int count, bool interactable = false, bool noSelectionFrames = false)
	{
		this.SetWidgetState(ItemRelatedWidgetState.Default);
		if (string.IsNullOrEmpty(iconId))
		{
			this.DrawEmpty(false, false, false);
			return;
		}
		this.noSelectionFrames = noSelectionFrames;
		this.showMouseSelectionFrame = true;
		this.tooltipPlacementPriority = TooltipPlacementPriority.TopRight;
		this.isInteractable = interactable;
		this.value = count;
		this.drawCounter = true;
		Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(iconId, null);
		if (sprite == null)
		{
			this.DrawPlaceHolder(iconId, true, noSelectionFrames);
		}
		else
		{
			this.icon.gameObject.SetActive(true);
			this.iconLabelPlaceholder.gameObject.SetActive(false);
			this.icon.BlueColorReplace(this.colors.NormalColor);
			this.icon.sprite = sprite;
		}
		this.UpdateCountLabel(1, false);
		this.starIcon.gameObject.SetActive(false);
		this.statusIcon.gameObject.SetActive(false);
		this.ClearPriceLabel();
		this.CheckPreviousTooltip();
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x00116D15 File Offset: 0x00114F15
	public void DrawEmptyInteractable(bool drawAsNonInteractable = false, bool noSelectionFrames = false)
	{
		this.SetWidgetState(ItemRelatedWidgetState.Default);
		this.DrawEmpty(false, false, noSelectionFrames);
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x00116D27 File Offset: 0x00114F27
	public void DrawEmptyWithState(ItemRelatedWidgetState widgetState, bool drawAsNonInteractable = false, bool noSelectionFrames = false)
	{
		this.SetWidgetState(widgetState);
		this.DrawEmpty(drawAsNonInteractable, false, noSelectionFrames);
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x00116D3C File Offset: 0x00114F3C
	public void DrawEmpty(bool drawAsNonInteractable = false, bool resetWidgetState = true, bool noSelectionFrames = false)
	{
		this.Flush(resetWidgetState);
		this.noSelectionFrames = noSelectionFrames;
		this.isInteractable = !drawAsNonInteractable;
		this.icon.gameObject.SetActive(false);
		this.iconLabelPlaceholder.gameObject.SetActive(false);
		this.icon.BlueColorReplace(this.colors.NormalColor);
		this.icon.sprite = null;
		this.countLabel.text = string.Empty;
		this.starIcon.gameObject.SetActive(false);
		this.statusIcon.gameObject.SetActive(false);
		this.ClearPriceLabel();
		this.CheckPreviousTooltip();
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x00116DE4 File Offset: 0x00114FE4
	public void Flush(bool resetWidgetState = true)
	{
		this.isSelected = false;
		this.StopSelectionBlinking();
		if (resetWidgetState)
		{
			this.SetWidgetState(ItemRelatedWidgetState.NotSet);
		}
		this.noSelectionFrames = false;
		this.showMouseSelectionFrame = true;
		this.tooltipPlacementPriority = TooltipPlacementPriority.TopRight;
		this.ClearPriceLabel();
		this.displayingItem = null;
		this.displayingItemCount = null;
		this.displayingOutputPreview = null;
		this.ExtraRedTooltipLocId = null;
		if (this.GamepadNavigationItem != null)
		{
			this.GamepadNavigationItem.group = 0;
		}
		this.ClearCallbacks();
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x00116E5F File Offset: 0x0011505F
	public void OnMultiplierChange(int multiplier, bool forceDraw = false)
	{
		this.UpdateCountLabel(multiplier, forceDraw);
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00116E6C File Offset: 0x0011506C
	public void UpdateCountLabel(int multiplier = 1, bool forceDraw = false)
	{
		int num = this.value * multiplier;
		bool flag = this.displayingItem != null && this.displayingItem.Definition.CanItemBeEquipped();
		if (this.isNeedItem || !flag)
		{
			this.DrawCount(num, forceDraw);
			return;
		}
		this.countLabel.text = "item_icon-hand".FontIcon();
		this.countLabel.gameObject.SetActive(true);
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x00116ED8 File Offset: 0x001150D8
	public void UpdateHappinessStatusIcons(Vendor vendor, int extraSoldCount = 0, float extraUsedHappiness = 0f)
	{
		Item item = this.DisplayingItem;
		if (vendor.HasHappinessForItem((item != null) ? item.id : null, extraSoldCount, extraUsedHappiness))
		{
			this.statusIcon.gameObject.SetActive(true);
			this.statusIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("widget_item_cell_happiness", null);
			this.statusIcon.SetNativeSize();
			this.SetStatusIconPosition(null);
			return;
		}
		this.statusIcon.gameObject.SetActive(false);
		this.SetStatusIconPosition(null);
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x00116F68 File Offset: 0x00115168
	public void UpdateCountLabelAsNormal(int leftCount, int rightCount)
	{
		this.countLabel.text = leftCount.ToString() + "/" + rightCount.ToString();
		this.countLabel.gameObject.SetActive(true);
		this.countLabelStyle.SetTextStyle(this.countLabelNormal);
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x00116FBA File Offset: 0x001151BA
	public void UpdateCountLabelAsRegularItem(int multiplier = 1)
	{
		this.isNeedItem = false;
		this.DrawCount(this.value * multiplier, true);
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x00116FD4 File Offset: 0x001151D4
	public void UpdateStatusIcon(OrderBase orderBase)
	{
		this.statusIcon.gameObject.SetActive(true);
		this.statusIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(orderBase.GetStatusIcon(), null);
		this.statusIcon.SetNativeSize();
		this.SetStatusIconPosition(null);
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x00117028 File Offset: 0x00115228
	public void UpdateStatusIcon(Sprite sprite, Vector2? anchoredPosition = null)
	{
		this.statusIcon.gameObject.SetActive(true);
		this.statusIcon.sprite = sprite;
		this.statusIcon.SetNativeSize();
		this.SetStatusIconPosition(anchoredPosition);
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x0011705C File Offset: 0x0011525C
	public void UpdateStatusIcon(CraftStatus craftStatus = CraftStatus.OK, ItemType requiredItemType = ItemType.None)
	{
		if (craftStatus == CraftStatus.OK || craftStatus == CraftStatus.NotEnoughMastery || craftStatus == CraftStatus.NotEnoughEnergy || craftStatus == CraftStatus.NotEnoughInsanity)
		{
			this.statusIcon.gameObject.SetActive(false);
			this.SetStatusIconPosition(null);
			return;
		}
		this.statusIcon.gameObject.SetActive(true);
		if (craftStatus == CraftStatus.DoesntHaveRequiredTool)
		{
			this.statusIcon.sprite = CraftStatusIconHelper.GetCraftStatusIcon(craftStatus, requiredItemType, "");
			this.statusIcon.SetNativeSize();
		}
		else
		{
			this.statusIcon.sprite = CraftStatusIconHelper.GetCraftStatusIcon(craftStatus, ItemType.None, "");
			this.statusIcon.SetNativeSize();
		}
		this.SetStatusIconPosition(null);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x00117104 File Offset: 0x00115304
	private void CacheDefaultStatusIconPosition()
	{
		if (this.hasCachedStatusIconPosition || this.statusIcon == null)
		{
			return;
		}
		this.defaultStatusIconAnchoredPosition = this.statusIcon.rectTransform.anchoredPosition;
		this.hasCachedStatusIconPosition = true;
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0011713C File Offset: 0x0011533C
	private void SetStatusIconPosition(Vector2? anchoredPosition = null)
	{
		if (this.statusIcon == null)
		{
			return;
		}
		this.CacheDefaultStatusIconPosition();
		this.statusIcon.rectTransform.anchoredPosition = anchoredPosition ?? this.defaultStatusIconAnchoredPosition;
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x00117188 File Offset: 0x00115388
	public void SetNativeSizeForIcon()
	{
		this.icon.SetNativeSize();
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x00117198 File Offset: 0x00115398
	public void UpdateQualityIcon(int quality)
	{
		if (quality >= 0)
		{
			this.starIcon.gameObject.SetActive(true);
			Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + quality.ToString(), null);
			this.starIcon.sprite = sprite;
			return;
		}
		this.starIcon.gameObject.SetActive(false);
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x001171F5 File Offset: 0x001153F5
	public void ClearPriceLabel()
	{
		if (this.priceLabel == null)
		{
			Debug.LogError("Item has no price field", this);
			return;
		}
		this.priceLabel.text = "";
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x00117221 File Offset: 0x00115421
	public void UpdatePriceLabel(int price)
	{
		if (this.priceLabel == null)
		{
			Debug.LogError("Item has no price field", this);
			return;
		}
		this.priceLabel.text = Trading.FormatMoney(price, false, "\n", null);
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x00117258 File Offset: 0x00115458
	private void TryInitGamepadNavigationItem()
	{
		if (this.gamepadNavigationItem == null)
		{
			this.gamepadNavigationItem = base.GetComponent<GamepadNavigationItem>();
			if (this.gamepadNavigationItem != null)
			{
				this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.OnGamepadOver), new UnityAction(this.OnGamepadOut), new UnityAction(this.OnGamepadPress));
			}
		}
	}

	// Token: 0x06003A59 RID: 14937 RVA: 0x001172BC File Offset: 0x001154BC
	private void CheckPreviousTooltip()
	{
		if (this.isHovered && (this.displayingItem == null || this.displayingItem.IsEmpty))
		{
			if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
			{
				this.HideUITooltip(false);
				return;
			}
		}
		else if (this.isHovered)
		{
			this.ShowUITooltip();
		}
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x00117310 File Offset: 0x00115510
	private void OnDisable()
	{
		this.StopSelectionBlinking();
		if (this.isHovered)
		{
			if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
			{
				this.HideUITooltip(true);
			}
			this.isHovered = false;
		}
		this.icon.BlueColorReplace(this.colors.NormalColor);
		this.isHovered = false;
		this.HideSelectionFrame();
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0011736E File Offset: 0x0011556E
	private void OnDestroy()
	{
		this.StopSelectionBlinking();
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x00117378 File Offset: 0x00115578
	public void OnOver()
	{
		this.StopSelectionBlinking();
		if (!this.noSelectionFrames)
		{
			if (LazyInput.IsGamepadActive || this.showMouseSelectionFrame)
			{
				if (this.nonInteractableItemImg != null && this.nonInteractableItemImg.gameObject.activeSelf && this.selectionNonInteractable != null)
				{
					this.selectionNonInteractable.gameObject.SetActive(true);
				}
				else
				{
					this.selection.gameObject.SetActive(true);
				}
			}
			this.icon.BlueColorReplace(this.colors.HighlightedColorMouse);
		}
		this.ShowUITooltip();
		Action<UIItemCell> onItemCellOver = this.OnItemCellOver;
		if (onItemCellOver == null)
		{
			return;
		}
		onItemCellOver(this);
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x00117421 File Offset: 0x00115621
	public void OnOut()
	{
		this.icon.BlueColorReplace(this.colors.NormalColor);
		this.HideUITooltip(false);
		this.RefreshSelectionFrame();
		Action<UIItemCell> onItemCellOut = this.OnItemCellOut;
		if (onItemCellOut == null)
		{
			return;
		}
		onItemCellOut(this);
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x00117457 File Offset: 0x00115657
	public void OnPress()
	{
		if (this.widgetState == ItemRelatedWidgetState.Disabled)
		{
			return;
		}
		Action onWidgetPress = this.OnWidgetPress;
		if (onWidgetPress != null)
		{
			onWidgetPress();
		}
		if (!this.isInteractable)
		{
			return;
		}
		Action<UIItemCell> onItemCellPress = this.OnItemCellPress;
		if (onItemCellPress == null)
		{
			return;
		}
		onItemCellPress(this);
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x00117490 File Offset: 0x00115690
	public void OnPress2()
	{
		if (this.widgetState == ItemRelatedWidgetState.Disabled)
		{
			return;
		}
		Action onWidgetPress = this.OnWidgetPress;
		if (onWidgetPress != null)
		{
			onWidgetPress();
		}
		if (!this.isInteractable)
		{
			return;
		}
		if (!LazyInput.IsGamepadActive && !Input.GetMouseButtonDown(1))
		{
			return;
		}
		Action<UIItemCell> onItemCellPress = this.OnItemCellPress2;
		if (onItemCellPress == null)
		{
			return;
		}
		onItemCellPress(this);
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x001174E2 File Offset: 0x001156E2
	public void OnDown()
	{
		if (!this.isInteractable || this.widgetState == ItemRelatedWidgetState.Disabled)
		{
			return;
		}
		if (!LazyInput.IsGamepadActive && !Input.GetMouseButtonDown(0))
		{
			return;
		}
		Action<UIItemCell> onItemCellDown = this.OnItemCellDown;
		if (onItemCellDown == null)
		{
			return;
		}
		onItemCellDown(this);
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x00117517 File Offset: 0x00115717
	public void OnGamepadOver()
	{
		if (this.widgetState != ItemRelatedWidgetState.Disabled)
		{
			Action onWidgetPress = this.OnWidgetPress;
			if (onWidgetPress != null)
			{
				onWidgetPress();
			}
		}
		this.OnOver();
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x00117539 File Offset: 0x00115739
	public void OnGamepadOut()
	{
		this.OnOut();
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x00117541 File Offset: 0x00115741
	public void OnGamepadPress()
	{
		this.OnPress();
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x00117549 File Offset: 0x00115749
	public void OnGamepadPress2()
	{
		this.OnPress2();
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x00117551 File Offset: 0x00115751
	public void ShowUITooltip()
	{
		this.isHovered = true;
		UITooltip.ShowItemCell(this);
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x00117560 File Offset: 0x00115760
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

	// Token: 0x06003A67 RID: 14951 RVA: 0x00117577 File Offset: 0x00115777
	public void ClearCallbacks()
	{
		this.OnItemCellOver = null;
		this.OnItemCellOut = null;
		this.OnItemCellPress = null;
		this.OnItemCellPress2 = null;
		this.OnItemCellDown = null;
		this.OnWidgetPress = null;
		this.CustomTooltipShowAction = null;
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x001175AC File Offset: 0x001157AC
	public void StartSelectionBlinking()
	{
		this.StopSelectionBlinking();
		if (this.noSelectionFrames)
		{
			return;
		}
		this.selectionBlinkCancellationTokenSource = new CancellationTokenSource();
		this.SelectionBlinking(this.selectionBlinkCancellationTokenSource).Forget();
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x001175E8 File Offset: 0x001157E8
	private void StopSelectionBlinking()
	{
		if (this.selectionBlinkCancellationTokenSource != null)
		{
			CancellationTokenSource cancellationTokenSource = this.selectionBlinkCancellationTokenSource;
			this.selectionBlinkCancellationTokenSource = null;
			cancellationTokenSource.Cancel();
		}
		if (this.isSelected)
		{
			this.RefreshSelectionFrame();
			return;
		}
		if (this.selection != null)
		{
			this.selection.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x00117640 File Offset: 0x00115840
	private UniTaskVoid SelectionBlinking(CancellationTokenSource cancellationTokenSource)
	{
		UIItemCell.<SelectionBlinking>d__122 <SelectionBlinking>d__;
		<SelectionBlinking>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SelectionBlinking>d__.<>4__this = this;
		<SelectionBlinking>d__.cancellationTokenSource = cancellationTokenSource;
		<SelectionBlinking>d__.<>1__state = -1;
		<SelectionBlinking>d__.<>t__builder.Start<UIItemCell.<SelectionBlinking>d__122>(ref <SelectionBlinking>d__);
		return <SelectionBlinking>d__.<>t__builder.Task;
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0011768C File Offset: 0x0011588C
	private void DrawCount(int actualValue, bool forceDraw)
	{
		this.countLabel.text = (this.isNeedItem ? (this.hasItemCount.ToString() + "/" + actualValue.ToString()) : actualValue.ToString());
		this.countLabel.gameObject.SetActive((this.drawCounter && (this.isNeedItem || actualValue > 1)) || forceDraw);
		this.countLabelStyle.SetTextStyle((this.isNeedItem && this.hasItemCount < actualValue) ? this.countLabelRed : this.countLabelNormal);
	}

	// Token: 0x04002DDC RID: 11740
	private const int SELECTION_BLINK_COUNT = 3;

	// Token: 0x04002DDD RID: 11741
	private const float SELECTION_BLINK_DELAY = 0.5f;

	// Token: 0x04002DDE RID: 11742
	public Action<UIItemCell> OnItemCellOver;

	// Token: 0x04002DDF RID: 11743
	public Action<UIItemCell> OnItemCellOut;

	// Token: 0x04002DE0 RID: 11744
	public Action<UIItemCell> OnItemCellPress;

	// Token: 0x04002DE1 RID: 11745
	public Action<UIItemCell> OnItemCellPress2;

	// Token: 0x04002DE2 RID: 11746
	public Action<UIItemCell> OnItemCellDown;

	// Token: 0x04002DE3 RID: 11747
	public Action<UIItemCell> CustomTooltipShowAction;

	// Token: 0x04002DE4 RID: 11748
	public string ExtraRedTooltipLocId;

	// Token: 0x04002DE5 RID: 11749
	public Action OnWidgetPress;

	// Token: 0x04002DE6 RID: 11750
	[SerializeField]
	private ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.NotSet;

	// Token: 0x04002DE7 RID: 11751
	[Space]
	private Item displayingItem;

	// Token: 0x04002DE8 RID: 11752
	private ItemCount displayingItemCount;

	// Token: 0x04002DE9 RID: 11753
	private OutputPreview displayingOutputPreview;

	// Token: 0x04002DEA RID: 11754
	[SerializeField]
	private Image background;

	// Token: 0x04002DEB RID: 11755
	[SerializeField]
	private Color backColorEmpty;

	// Token: 0x04002DEC RID: 11756
	[SerializeField]
	private Image icon;

	// Token: 0x04002DED RID: 11757
	[SerializeField]
	private Image starIcon;

	// Token: 0x04002DEE RID: 11758
	[SerializeField]
	private TextMeshProUGUI iconLabelPlaceholder;

	// Token: 0x04002DEF RID: 11759
	[SerializeField]
	private Image selection;

	// Token: 0x04002DF0 RID: 11760
	[SerializeField]
	private Image selectionNonInteractable;

	// Token: 0x04002DF1 RID: 11761
	[SerializeField]
	private TextMeshProUGUI countLabel;

	// Token: 0x04002DF2 RID: 11762
	[SerializeField]
	private TextMeshProUGUI priceLabel;

	// Token: 0x04002DF3 RID: 11763
	[SerializeField]
	private ImageColors colors;

	// Token: 0x04002DF4 RID: 11764
	[SerializeField]
	private Image nonInteractableItemImg;

	// Token: 0x04002DF5 RID: 11765
	[SerializeField]
	private LazyButton lazyButton;

	// Token: 0x04002DF6 RID: 11766
	[SerializeField]
	private TextStyleComponent countLabelStyle;

	// Token: 0x04002DF7 RID: 11767
	[SerializeField]
	private TextStyle countLabelNormal;

	// Token: 0x04002DF8 RID: 11768
	[SerializeField]
	private TextStyle countLabelRed;

	// Token: 0x04002DF9 RID: 11769
	[SerializeField]
	[Space]
	private Image statusIcon;

	// Token: 0x04002DFA RID: 11770
	private Vector2 defaultStatusIconAnchoredPosition;

	// Token: 0x04002DFB RID: 11771
	private bool hasCachedStatusIconPosition;

	// Token: 0x04002DFC RID: 11772
	private int value;

	// Token: 0x04002DFD RID: 11773
	private int hasItemCount;

	// Token: 0x04002DFE RID: 11774
	private bool isNeedItem;

	// Token: 0x04002DFF RID: 11775
	private bool drawCounter;

	// Token: 0x04002E00 RID: 11776
	private bool noSelectionFrames;

	// Token: 0x04002E01 RID: 11777
	private bool showMouseSelectionFrame = true;

	// Token: 0x04002E02 RID: 11778
	private TooltipPlacementPriority tooltipPlacementPriority;

	// Token: 0x04002E03 RID: 11779
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x04002E04 RID: 11780
	private bool isHovered;

	// Token: 0x04002E05 RID: 11781
	private bool isInteractable;

	// Token: 0x04002E06 RID: 11782
	private bool isSelected;

	// Token: 0x04002E07 RID: 11783
	private CancellationTokenSource selectionBlinkCancellationTokenSource;
}
