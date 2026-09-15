using System;
using LazyBearTechnology;

// Token: 0x0200097E RID: 2430
public class StarQualityWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x0600403A RID: 16442 RVA: 0x001338C9 File Offset: 0x00131AC9
	// (set) Token: 0x0600403B RID: 16443 RVA: 0x001338D1 File Offset: 0x00131AD1
	public CraftParamsData CraftParams { get; private set; }

	// Token: 0x0600403C RID: 16444 RVA: 0x001338DA File Offset: 0x00131ADA
	public StarQualityWidgetData(CraftParamsData craftParams)
	{
		this.CraftParams = craftParams;
	}
}
