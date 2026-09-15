using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000933 RID: 2355
public class TalentWidget : LazyWidget<TalentWidgetData>
{
	// Token: 0x06003E0D RID: 15885 RVA: 0x001287D8 File Offset: 0x001269D8
	public override void Redraw()
	{
		base.Redraw();
		this.iconLabel.text = this.data.TalentId.FontIcon();
		TalentWidget.TalentViewData talentViewData = this.viewDataList.Find((TalentWidget.TalentViewData p) => p.talentId == this.data.TalentId);
		this.backgroundImage.sprite = talentViewData.backSprite;
		this.backgroundImage.SetNativeSize();
		talentViewData.textStyle.ApplyStyle(this.valueLabel, false, null, null, null);
		this.valueLabel.text = this.data.MasteryValue.ToString();
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x00128888 File Offset: 0x00126A88
	public void DisableGamepadNavigation()
	{
		GamepadNavigationItem[] componentsInChildren = base.GetComponentsInChildren<GamepadNavigationItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = false;
		}
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x001288B4 File Offset: 0x00126AB4
	public void EnableGamepadNavigation()
	{
		GamepadNavigationItem[] componentsInChildren = base.GetComponentsInChildren<GamepadNavigationItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = true;
		}
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040030DE RID: 12510
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x040030DF RID: 12511
	[SerializeField]
	private TextMeshProUGUI iconLabel;

	// Token: 0x040030E0 RID: 12512
	[SerializeField]
	private TextMeshProUGUI valueLabel;

	// Token: 0x040030E1 RID: 12513
	[SerializeField]
	private List<TalentWidget.TalentViewData> viewDataList;

	// Token: 0x02000934 RID: 2356
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x040030E2 RID: 12514
		public Sprite backSprite;

		// Token: 0x040030E3 RID: 12515
		public TextStyle textStyle;

		// Token: 0x040030E4 RID: 12516
		public string talentId;
	}
}
