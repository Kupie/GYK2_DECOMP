using System;
using LazyBearTechnology;

// Token: 0x020007E2 RID: 2018
public class HpBarWidgetData : LazyWidgetDataBase
{
	// Token: 0x06003400 RID: 13312 RVA: 0x000FADF4 File Offset: 0x000F8FF4
	public HpBarWidgetData(HPComponent hpComponent)
	{
		this.hpComponent = hpComponent;
	}

	// Token: 0x04002974 RID: 10612
	public HPComponent hpComponent;
}
