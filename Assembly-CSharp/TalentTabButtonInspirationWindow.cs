using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200092E RID: 2350
public class TalentTabButtonInspirationWindow : TalentTabButton
{
	// Token: 0x06003DF7 RID: 15863 RVA: 0x00128160 File Offset: 0x00126360
	public override void UpdateState(bool isActive, Canvas parentCanvas)
	{
		this.isActive = isActive;
		if (isActive)
		{
			this.backgroundImage.sprite = this.activeBackSprite;
			this.SetActionIndicatorState(false);
			this.activeObject.SetActive(true);
			this.inactiveObject.SetActive(false);
		}
		else
		{
			this.backgroundImage.sprite = this.inactiveBackSprite;
			this.activeObject.SetActive(false);
			this.inactiveObject.SetActive(true);
		}
		this.backgroundImage.SetNativeSize();
		this.DrawTalentIcon();
		this.button.interactable = !isActive;
		base.UpdateSorting(parentCanvas);
		Image[] array = this.imageIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = (isActive ? null : this.inactiveIconMaterial);
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00128224 File Offset: 0x00126424
	protected override void DrawTalentIcon()
	{
		TalentTabButtonInspirationWindow.TabTalentViewData tabTalentViewData = this.viewDataList.Find((TalentTabButtonInspirationWindow.TabTalentViewData p) => p.talentId == base.TalentId);
		Image[] array = this.imageIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = tabTalentViewData.sprite;
		}
		this.DrawMasteryValue();
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00128274 File Offset: 0x00126474
	public override void SetActionIndicatorState(bool isActive)
	{
		GameObject[] array = this.actionIndicators;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(isActive);
		}
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x001282A0 File Offset: 0x001264A0
	public override void DrawMasteryValue()
	{
		if (MainGame.Instance.gameState == MainGame.GameState.MainMenu)
		{
			return;
		}
		TextMeshProUGUI[] array = this.valueLabels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = MainGame.PlayerController.GetMasteryLevelForTalentBranch(this.talentId, null).ToString();
		}
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x001282F0 File Offset: 0x001264F0
	protected override void OnSelect()
	{
		if (this.inactiveImage != null)
		{
			this.inactiveImage.sprite = this.inactiveBackSpriteSelected;
			Image[] array = this.imageIcons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].material = null;
			}
		}
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x0012833C File Offset: 0x0012653C
	protected override void OnDeselect()
	{
		if (this.inactiveImage != null)
		{
			this.inactiveImage.sprite = this.inactiveBackSprite;
			Image[] array = this.imageIcons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].material = this.inactiveIconMaterial;
			}
		}
	}

	// Token: 0x06003DFD RID: 15869 RVA: 0x0012838B File Offset: 0x0012658B
	protected override void OnDisable()
	{
		this.OnDeselect();
	}

	// Token: 0x040030CB RID: 12491
	[SerializeField]
	private Image[] imageIcons;

	// Token: 0x040030CC RID: 12492
	[SerializeField]
	private List<TalentTabButtonInspirationWindow.TabTalentViewData> viewDataList;

	// Token: 0x040030CD RID: 12493
	[SerializeField]
	private TextMeshProUGUI[] valueLabels;

	// Token: 0x040030CE RID: 12494
	[SerializeField]
	private GameObject activeObject;

	// Token: 0x040030CF RID: 12495
	[SerializeField]
	private GameObject inactiveObject;

	// Token: 0x040030D0 RID: 12496
	[SerializeField]
	protected GameObject[] actionIndicators;

	// Token: 0x040030D1 RID: 12497
	[SerializeField]
	private Image inactiveImage;

	// Token: 0x040030D2 RID: 12498
	[SerializeField]
	private Material inactiveIconMaterial;

	// Token: 0x0200092F RID: 2351
	[Serializable]
	private class TabTalentViewData
	{
		// Token: 0x040030D3 RID: 12499
		public Sprite sprite;

		// Token: 0x040030D4 RID: 12500
		public string talentId;
	}
}
