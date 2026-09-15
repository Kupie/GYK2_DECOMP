using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007F4 RID: 2036
public class UIQualityTooltipWidget : LazyWidget<UIQualityTooltipWidgetData>
{
	// Token: 0x06003446 RID: 13382 RVA: 0x000FBCB8 File Offset: 0x000F9EB8
	public override void Draw(UIQualityTooltipWidgetData data)
	{
		base.Draw(data);
		this.iconLabel.text = data.IconName.FontIcon();
		this.label.text = (data.HasValueOverride ? data.ValueText : data.WgoData.Quality.ToInvariantCultureString());
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x00002318 File Offset: 0x00000518
	public override void Hide()
	{
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040029C0 RID: 10688
	[SerializeField]
	private TextMeshProUGUI iconLabel;

	// Token: 0x040029C1 RID: 10689
	[SerializeField]
	private TextMeshProUGUI label;
}
