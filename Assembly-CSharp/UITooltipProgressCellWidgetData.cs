using System;
using LazyBearTechnology;

// Token: 0x02000853 RID: 2131
public class UITooltipProgressCellWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x060036A1 RID: 13985 RVA: 0x00108E09 File Offset: 0x00107009
	// (set) Token: 0x060036A2 RID: 13986 RVA: 0x00108E11 File Offset: 0x00107011
	public PerkDef PerkDefBonus { get; private set; }

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x060036A3 RID: 13987 RVA: 0x00108E1A File Offset: 0x0010701A
	// (set) Token: 0x060036A4 RID: 13988 RVA: 0x00108E22 File Offset: 0x00107022
	public ItemDef ItemDefBonus { get; private set; }

	// Token: 0x060036A5 RID: 13989 RVA: 0x00108E2B File Offset: 0x0010702B
	public UITooltipProgressCellWidgetData(PerkDef perkDefBonus, ItemDef itemDefBonus)
	{
		this.PerkDefBonus = perkDefBonus;
		this.ItemDefBonus = itemDefBonus;
	}
}
