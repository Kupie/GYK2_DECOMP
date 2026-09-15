using System;
using LazyBearTechnology;

// Token: 0x02000A0B RID: 2571
public class UIPorterStationWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x0600452B RID: 17707 RVA: 0x00147367 File Offset: 0x00145567
	// (set) Token: 0x0600452C RID: 17708 RVA: 0x0014736F File Offset: 0x0014556F
	public PorterStationDef PorterStationDef { get; private set; }

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x0600452D RID: 17709 RVA: 0x00147378 File Offset: 0x00145578
	// (set) Token: 0x0600452E RID: 17710 RVA: 0x00147380 File Offset: 0x00145580
	public WgoData Station { get; private set; }

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x0600452F RID: 17711 RVA: 0x00147389 File Offset: 0x00145589
	// (set) Token: 0x06004530 RID: 17712 RVA: 0x00147391 File Offset: 0x00145591
	public Action<UIItemCell> OnItemCellPress { get; private set; }

	// Token: 0x06004531 RID: 17713 RVA: 0x0014739C File Offset: 0x0014559C
	public UIPorterStationWindowData(WgoData station)
	{
		this.PorterStationDef = GameBalance.Me.GetData<PorterStationDef>(station.id);
		this.Station = station;
		this.OnItemCellPress = delegate(UIItemCell cell)
		{
			bool flag = station.GetGameResInt(cell.DisplayingItem.id) == 1;
			station.SetGameRes(cell.DisplayingItem.id, flag ? 0 : 1);
		};
	}
}
