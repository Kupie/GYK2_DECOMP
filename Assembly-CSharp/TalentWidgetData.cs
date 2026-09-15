using System;
using LazyBearTechnology;

// Token: 0x02000935 RID: 2357
public class TalentWidgetData : LazyWidgetDataBase
{
	// Token: 0x06003E14 RID: 15892 RVA: 0x001288FF File Offset: 0x00126AFF
	public TalentWidgetData(string talentId, int masteryValue)
	{
		this.TalentId = talentId;
		this.MasteryValue = masteryValue;
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06003E15 RID: 15893 RVA: 0x00128915 File Offset: 0x00126B15
	// (set) Token: 0x06003E16 RID: 15894 RVA: 0x0012891D File Offset: 0x00126B1D
	public string TalentId { get; private set; }

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06003E17 RID: 15895 RVA: 0x00128926 File Offset: 0x00126B26
	// (set) Token: 0x06003E18 RID: 15896 RVA: 0x0012892E File Offset: 0x00126B2E
	public int MasteryValue { get; private set; }
}
