using System;
using LazyBearTechnology;

// Token: 0x020007F7 RID: 2039
public class UIWorkbenchAdditionWorldIconWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x0600345A RID: 13402 RVA: 0x000FBF93 File Offset: 0x000FA193
	public string IconId { get; }

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x0600345B RID: 13403 RVA: 0x000FBF9B File Offset: 0x000FA19B
	public bool IsInRange { get; }

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x0600345C RID: 13404 RVA: 0x000FBFA3 File Offset: 0x000FA1A3
	public bool ShowBackground { get; }

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x0600345D RID: 13405 RVA: 0x000FBFAB File Offset: 0x000FA1AB
	public string CrossIconId { get; }

	// Token: 0x0600345E RID: 13406 RVA: 0x000FBFB3 File Offset: 0x000FA1B3
	public UIWorkbenchAdditionWorldIconWidgetData(string iconId, bool isInRange)
		: this(iconId, isInRange, true, null)
	{
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x000FBFBF File Offset: 0x000FA1BF
	private UIWorkbenchAdditionWorldIconWidgetData(string iconId, bool isInRange, bool showBackground, string crossIconId)
	{
		this.IconId = iconId;
		this.IsInRange = isInRange;
		this.ShowBackground = showBackground;
		this.CrossIconId = crossIconId;
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x000FBFE4 File Offset: 0x000FA1E4
	public static UIWorkbenchAdditionWorldIconWidgetData StationWithoutCaretaker()
	{
		return new UIWorkbenchAdditionWorldIconWidgetData("i_no_zombie_delivery", true, false, null);
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x000FBFF3 File Offset: 0x000FA1F3
	public static UIWorkbenchAdditionWorldIconWidgetData NoCaretakerInZone()
	{
		return new UIWorkbenchAdditionWorldIconWidgetData(ZombieDeliveryIndication.GetStationIconId(), false, false, "i_red_cross_big_icon");
	}
}
