using System;
using LazyBearTechnology;

// Token: 0x02000A40 RID: 2624
public class UISurveyResultWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x060046C8 RID: 18120 RVA: 0x0014F0F1 File Offset: 0x0014D2F1
	// (set) Token: 0x060046C9 RID: 18121 RVA: 0x0014F0F9 File Offset: 0x0014D2F9
	public ItemDef ItemDef { get; private set; }

	// Token: 0x060046CA RID: 18122 RVA: 0x0014F102 File Offset: 0x0014D302
	public UISurveyResultWindowData(ItemDef itemDef)
	{
		this.ItemDef = itemDef;
	}
}
