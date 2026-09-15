using System;
using LazyBearTechnology;

// Token: 0x0200084C RID: 2124
public class UIMixInfoWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06003678 RID: 13944 RVA: 0x00108460 File Offset: 0x00106660
	// (set) Token: 0x06003679 RID: 13945 RVA: 0x00108468 File Offset: 0x00106668
	public AlchemyMixDef MixDef { get; set; }

	// Token: 0x0600367A RID: 13946 RVA: 0x00108471 File Offset: 0x00106671
	public UIMixInfoWidgetData(AlchemyMixDef mixDef)
	{
		this.MixDef = mixDef;
	}
}
