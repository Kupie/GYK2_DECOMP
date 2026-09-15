using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007F0 RID: 2032
public class UIInteractionHintWidget : LazyWidget<UIInteractionHintWidgetData>
{
	// Token: 0x06003436 RID: 13366 RVA: 0x000FBAFC File Offset: 0x000F9CFC
	public override void Redraw()
	{
		this.row1.Draw(this.data.RowData1);
		if (this.data.RowData2 != null)
		{
			this.row2.Draw(this.data.RowData2);
		}
		else
		{
			this.row2.Hide();
		}
		((RectTransform)base.transform).EnableLayoutGroupsAndRefreshContentFitter();
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x00002318 File Offset: 0x00000518
	public override void Hide()
	{
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x000FBB5F File Offset: 0x000F9D5F
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIInteractionHintWidgetData(new UIInteractionHintRowWidgetData(new InteractionInfo(LLBase.L("hint_interaction")))));
	}

	// Token: 0x040029B8 RID: 10680
	[SerializeField]
	private UIInteractionHintRow row1;

	// Token: 0x040029B9 RID: 10681
	[SerializeField]
	private UIInteractionHintRow row2;
}
