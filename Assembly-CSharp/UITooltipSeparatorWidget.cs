using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public class UITooltipSeparatorWidget : LazyWidget<UITooltipSeparatorWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x0600341D RID: 13341 RVA: 0x000FB684 File Offset: 0x000F9884
	public override void Draw(UITooltipSeparatorWidgetData data)
	{
		this.up.sizeDelta = new Vector2(this.up.sizeDelta.x, (float)data.spaceUp);
		this.down.sizeDelta = new Vector2(this.down.sizeDelta.x, (float)data.spaceDown);
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x00002318 File Offset: 0x00000518
	public override void Hide()
	{
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040029A1 RID: 10657
	[SerializeField]
	private RectTransform up;

	// Token: 0x040029A2 RID: 10658
	[SerializeField]
	private RectTransform down;
}
