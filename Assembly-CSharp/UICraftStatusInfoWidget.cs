using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000849 RID: 2121
public class UICraftStatusInfoWidget : LazyWidget<UICraftStatusInfoWidgetData>
{
	// Token: 0x06003668 RID: 13928 RVA: 0x001080F8 File Offset: 0x001062F8
	public override void Redraw()
	{
		base.Redraw();
		string text = this.data.StatusIcon.name.Replace("(Clone)", "");
		this.label.text = text.FontIcon() + ": " + this.data.Text;
		this.label.alignment = this.data.TextAlignmentOptions;
		if (this.data.TextStyle != null)
		{
			this.data.TextStyle.ApplyStyle(this.label, false, null, null, null);
		}
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002B72 RID: 11122
	[SerializeField]
	private TextMeshProUGUI label;
}
