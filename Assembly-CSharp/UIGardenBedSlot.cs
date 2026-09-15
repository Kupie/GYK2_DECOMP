using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009D8 RID: 2520
public class UIGardenBedSlot : MonoBehaviour
{
	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x06004351 RID: 17233 RVA: 0x0013FE4F File Offset: 0x0013E04F
	public PerkData PerkData
	{
		get
		{
			return this.perkData;
		}
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0013FE58 File Offset: 0x0013E058
	private void Awake()
	{
		this.button.onEnter.AddListener(new UnityAction(this.OnOver));
		this.button.onExit.AddListener(new UnityAction(this.OnOut));
		this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.button.ForceOnEnter), new UnityAction(this.button.ForceOnExit), null);
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x0013FECC File Offset: 0x0013E0CC
	public void DrawFertilizerSlot(PerkData perkData, Action<UIGardenBedSlot> onPress, bool allowInteraction = true)
	{
		this.perkData = perkData;
		this.onPress = onPress;
		this.itemCell.DrawCustom(perkData.Definition.IconId, 1, true, false);
		this.itemCell.CustomTooltipShowAction = delegate(UIItemCell _)
		{
			UITooltip.ShowGardenFertilizerSlot(this);
		};
		this.itemCell.ShowMouseSelectionFrame = allowInteraction;
		this.statusImage.gameObject.SetActive(false);
		this.gamepadNavigationItem.Active = true;
		this.button.interactable = true;
		TalentDef data = GameBalance.Me.GetData<TalentDef>("talent_green");
		this.masteryLabel.text = string.Format("+<space=1px>{0}<space=2px>{1}", data.id.FontIcon(), perkData.Definition.craftMasteryBonus);
		this.masteryLabel.gameObject.SetActive(true);
		UIMouseTooltip.Attach(this.masteryLabel.gameObject, "tt_garden_3_add", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x0013FFCC File Offset: 0x0013E1CC
	public void DrawSeedSlot(Item seed)
	{
		this.perkData = null;
		this.onPress = null;
		this.itemCell.Draw(seed, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.itemCell.ClearCallbacks();
		this.itemCell.ShowMouseSelectionFrame = false;
		this.itemCell.TooltipPlacementPriority = TooltipPlacementPriority.BottomRight;
		this.masteryLabel.gameObject.SetActive(false);
		this.statusImage.gameObject.SetActive(false);
		this.gamepadNavigationItem.Active = true;
		this.button.interactable = true;
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x0014005C File Offset: 0x0013E25C
	public void DrawEmpty(Action<UIGardenBedSlot> onPress, bool allowInteraction = true)
	{
		this.onPress = (allowInteraction ? onPress : null);
		this.perkData = null;
		this.itemCell.DrawEmpty(!allowInteraction, true, !allowInteraction);
		this.itemCell.ClearCallbacks();
		if (allowInteraction)
		{
			this.itemCell.OnItemCellPress = new Action<UIItemCell>(this.OnPress);
		}
		this.statusImage.gameObject.SetActive(allowInteraction);
		if (allowInteraction)
		{
			this.statusImage.sprite = this.backEmpty;
		}
		this.gamepadNavigationItem.Active = allowInteraction;
		this.button.interactable = allowInteraction;
		this.masteryLabel.gameObject.SetActive(false);
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x00140104 File Offset: 0x0013E304
	public void DrawLocked()
	{
		this.perkData = null;
		this.onPress = null;
		this.itemCell.DrawEmpty(true, true, true);
		this.statusImage.gameObject.SetActive(true);
		this.statusImage.sprite = this.backLocked;
		this.gamepadNavigationItem.Active = false;
		this.button.interactable = false;
		this.masteryLabel.gameObject.SetActive(false);
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x00140178 File Offset: 0x0013E378
	private void OnOver()
	{
		if (this.perkData == null)
		{
			return;
		}
		UITooltip.ShowGardenFertilizerSlot(this);
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x00140189 File Offset: 0x0013E389
	private void OnPress(UIItemCell cell)
	{
		this.onPress(this);
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x00140197 File Offset: 0x0013E397
	private void OnDisable()
	{
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x04003476 RID: 13430
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04003477 RID: 13431
	[SerializeField]
	private Sprite backDefault;

	// Token: 0x04003478 RID: 13432
	[SerializeField]
	private Sprite backLocked;

	// Token: 0x04003479 RID: 13433
	[SerializeField]
	private Sprite backEmpty;

	// Token: 0x0400347A RID: 13434
	[SerializeField]
	private LazyButton button;

	// Token: 0x0400347B RID: 13435
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x0400347C RID: 13436
	[SerializeField]
	private TextMeshProUGUI masteryLabel;

	// Token: 0x0400347D RID: 13437
	[SerializeField]
	private Image statusImage;

	// Token: 0x0400347E RID: 13438
	public int slotIndex;

	// Token: 0x0400347F RID: 13439
	private Action<UIGardenBedSlot> onPress;

	// Token: 0x04003480 RID: 13440
	private PerkData perkData;
}
