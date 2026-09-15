using System;
using LazyBearTechnology;

// Token: 0x020007EB RID: 2027
public class UITooltipSeparatorWidgetData : LazyWidgetDataBase
{
	// Token: 0x06003421 RID: 13345 RVA: 0x000F93E5 File Offset: 0x000F75E5
	public UITooltipSeparatorWidgetData()
	{
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000FB6E7 File Offset: 0x000F98E7
	public UITooltipSeparatorWidgetData(int spaceDown, int spaceUp)
	{
		this.spaceDown = spaceDown;
		this.spaceUp = spaceUp;
	}

	// Token: 0x040029A3 RID: 10659
	public int spaceDown;

	// Token: 0x040029A4 RID: 10660
	public int spaceUp;
}
