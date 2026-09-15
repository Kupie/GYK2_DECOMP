using System;
using LazyBearTechnology;

// Token: 0x02000A44 RID: 2628
public class UITutorialListWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x060046E1 RID: 18145 RVA: 0x0014F51D File Offset: 0x0014D71D
	public UITutorialListOpenSource OpenSource { get; }

	// Token: 0x060046E2 RID: 18146 RVA: 0x0014F525 File Offset: 0x0014D725
	public UITutorialListWindowData(UITutorialListOpenSource openSource)
	{
		this.OpenSource = openSource;
	}
}
