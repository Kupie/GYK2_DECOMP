using System;

// Token: 0x02000958 RID: 2392
public class TechTreeElementBaseWidgetData : TreeElementBaseWidgetData
{
	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06003F16 RID: 16150 RVA: 0x0012DDD7 File Offset: 0x0012BFD7
	public TechState VisualTechState
	{
		get
		{
			return this.techDef.TechState;
		}
	}

	// Token: 0x040031A2 RID: 12706
	public Action<TechTreeElementBaseWidgetData> onTechClicked;
}
