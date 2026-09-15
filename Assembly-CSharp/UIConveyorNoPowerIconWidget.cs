using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007D6 RID: 2006
public class UIConveyorNoPowerIconWidget : LazyWidget<UIConveyorNoPowerIconWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x060033A8 RID: 13224 RVA: 0x000F939B File Offset: 0x000F759B
	public override void Redraw()
	{
		if (this.iconLabel == null)
		{
			return;
		}
		this.iconLabel.color = UIConveyorNoPowerIconWidget.TemporaryTint;
		this.iconLabel.text = "gear".FontIcon();
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400293A RID: 10554
	private const string IconId = "gear";

	// Token: 0x0400293B RID: 10555
	private static readonly Color TemporaryTint = Color.red;

	// Token: 0x0400293C RID: 10556
	[SerializeField]
	private TextMeshProUGUI iconLabel;
}
