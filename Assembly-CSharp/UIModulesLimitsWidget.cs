using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public class UIModulesLimitsWidget : LazyWidget<UIModulesLimitsWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x06003440 RID: 13376 RVA: 0x000FBC18 File Offset: 0x000F9E18
	public override void Redraw()
	{
		base.Redraw();
		this.label.text = string.Format("{0}{1}{2}", this.data.ModulesCount, this.slashStyle.ApplyStyleToString("/", true, true), this.data.ModulesLimit);
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040029BC RID: 10684
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040029BD RID: 10685
	[SerializeField]
	private TextStyle slashStyle;
}
