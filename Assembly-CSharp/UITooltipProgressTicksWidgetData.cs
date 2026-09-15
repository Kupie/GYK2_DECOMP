using System;
using LazyBearTechnology;

// Token: 0x02000855 RID: 2133
public class UITooltipProgressTicksWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x060036AA RID: 13994 RVA: 0x00109028 File Offset: 0x00107228
	// (set) Token: 0x060036AB RID: 13995 RVA: 0x00109030 File Offset: 0x00107230
	public int MasteryValue { get; private set; }

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x060036AC RID: 13996 RVA: 0x00109039 File Offset: 0x00107239
	// (set) Token: 0x060036AD RID: 13997 RVA: 0x00109041 File Offset: 0x00107241
	public int MasteryLock { get; private set; }

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x060036AE RID: 13998 RVA: 0x0010904A File Offset: 0x0010724A
	// (set) Token: 0x060036AF RID: 13999 RVA: 0x00109052 File Offset: 0x00107252
	public bool IsStarCraft { get; private set; }

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x060036B0 RID: 14000 RVA: 0x0010905B File Offset: 0x0010725B
	// (set) Token: 0x060036B1 RID: 14001 RVA: 0x00109063 File Offset: 0x00107263
	public TalentDef TalentDef { get; private set; }

	// Token: 0x060036B2 RID: 14002 RVA: 0x0010906C File Offset: 0x0010726C
	public UITooltipProgressTicksWidgetData(int masteryValue, int mastery, bool starCraft, TalentDef talentDef)
	{
		this.MasteryValue = masteryValue;
		this.MasteryLock = mastery;
		this.IsStarCraft = starCraft;
		this.TalentDef = talentDef;
	}
}
