using System;
using LazyBearTechnology;

// Token: 0x020009D0 RID: 2512
public class UISquadHudElementWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06004317 RID: 17175 RVA: 0x0013ECCD File Offset: 0x0013CECD
	// (set) Token: 0x06004318 RID: 17176 RVA: 0x0013ECD5 File Offset: 0x0013CED5
	public WgoData Fighter { get; private set; }

	// Token: 0x06004319 RID: 17177 RVA: 0x0013ECDE File Offset: 0x0013CEDE
	public UISquadHudElementWidgetData(WgoData fighter)
	{
		this.Fighter = fighter;
	}
}
