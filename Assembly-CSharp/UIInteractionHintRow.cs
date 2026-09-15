using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007EE RID: 2030
public class UIInteractionHintRow : LazyWidget<UIInteractionHintRowWidgetData>
{
	// Token: 0x06003430 RID: 13360 RVA: 0x000FB920 File Offset: 0x000F9B20
	public override void Redraw()
	{
		InteractionInfo interactionInfo = this.data.InteractionInfo;
		this.customIcon.gameObject.SetActive(false);
		this.label.text = ((interactionInfo.isEnoughMastery && interactionInfo.isItemEquipped) ? interactionInfo.text : "icon_lock".FontIcon());
		if (interactionInfo.assignedTalent != null)
		{
			if (!interactionInfo.isItemEquipped)
			{
				this.toolIcon.Draw(interactionInfo.equippedItemType, interactionInfo.isItemEquipped, interactionInfo.assignedTalent);
				this.talentIcon.Hide();
			}
			else
			{
				this.talentIcon.Draw(interactionInfo.assignedTalent, interactionInfo.masteryLock, interactionInfo.isEnoughMastery);
				this.toolIcon.Hide();
			}
		}
		else
		{
			this.talentIcon.Hide();
			if (interactionInfo.equippedItemType != ItemType.None)
			{
				this.toolIcon.Draw(interactionInfo.equippedItemType, interactionInfo.isItemEquipped, interactionInfo.assignedTalent);
			}
			else
			{
				this.toolIcon.Hide();
			}
		}
		if (!string.IsNullOrEmpty(interactionInfo.customIconId))
		{
			Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(interactionInfo.customIconId, null);
			if (sprite != null)
			{
				this.customIcon.sprite = sprite;
				this.customIcon.SetNativeSize();
				this.customIcon.gameObject.SetActive(true);
			}
		}
		if (this.labelLayoutElement != null)
		{
			if (LazyInput.IsGamepadActive)
			{
				this.labelLayoutElement.preferredHeight = this.heightGamepad;
				return;
			}
			this.labelLayoutElement.preferredHeight = this.heightKeyboard;
		}
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x000FBAA0 File Offset: 0x000F9CA0
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIInteractionHintRowWidgetData(new InteractionInfo(LLBase.L("hint_interaction"))));
	}

	// Token: 0x040029B0 RID: 10672
	public TextMeshProUGUI label;

	// Token: 0x040029B1 RID: 10673
	public LayoutElement labelLayoutElement;

	// Token: 0x040029B2 RID: 10674
	public float heightGamepad = 16f;

	// Token: 0x040029B3 RID: 10675
	public float heightKeyboard = 12f;

	// Token: 0x040029B4 RID: 10676
	public UIToolIcon toolIcon;

	// Token: 0x040029B5 RID: 10677
	public UITalentIcon talentIcon;

	// Token: 0x040029B6 RID: 10678
	public Image customIcon;
}
