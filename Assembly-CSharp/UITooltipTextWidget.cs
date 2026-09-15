using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000856 RID: 2134
public class UITooltipTextWidget : LazyWidget<UITooltipTextWidgetData>
{
	// Token: 0x060036B3 RID: 14003 RVA: 0x00109094 File Offset: 0x00107294
	public override void Redraw()
	{
		base.Redraw();
		this.label.text = this.data.Text;
		this.label.alignment = this.data.TextAlignmentOptions;
		if (this.data.TextStyle != null)
		{
			this.textStyleComponent.SetTextStyle(this.data.TextStyle);
			this.textStyleComponent.ApplyStyle();
		}
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x00109107 File Offset: 0x00107307
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UITooltipTextWidgetData("some text in widget", TextAlignmentOptions.Center, null));
	}

	// Token: 0x04002BA2 RID: 11170
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002BA3 RID: 11171
	[SerializeField]
	private TextStyleComponent textStyleComponent;
}
