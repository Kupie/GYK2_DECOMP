using System;
using LazyBearTechnology;

// Token: 0x0200099E RID: 2462
public class UIProgressCellsInfoWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x060041C2 RID: 16834 RVA: 0x001396EF File Offset: 0x001378EF
	// (set) Token: 0x060041C3 RID: 16835 RVA: 0x001396F7 File Offset: 0x001378F7
	public int MasteryValue { get; private set; }

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x060041C4 RID: 16836 RVA: 0x00139700 File Offset: 0x00137900
	// (set) Token: 0x060041C5 RID: 16837 RVA: 0x00139708 File Offset: 0x00137908
	public int MasteryLock { get; private set; }

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x060041C6 RID: 16838 RVA: 0x00139711 File Offset: 0x00137911
	// (set) Token: 0x060041C7 RID: 16839 RVA: 0x00139719 File Offset: 0x00137919
	public TalentDef TalentDef { get; private set; }

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x060041C8 RID: 16840 RVA: 0x00139722 File Offset: 0x00137922
	// (set) Token: 0x060041C9 RID: 16841 RVA: 0x0013972A File Offset: 0x0013792A
	public bool IsStarCraft { get; private set; }

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x060041CA RID: 16842 RVA: 0x00139733 File Offset: 0x00137933
	// (set) Token: 0x060041CB RID: 16843 RVA: 0x0013973B File Offset: 0x0013793B
	public string TooltipHeaderLngId { get; private set; }

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x060041CC RID: 16844 RVA: 0x00139744 File Offset: 0x00137944
	// (set) Token: 0x060041CD RID: 16845 RVA: 0x0013974C File Offset: 0x0013794C
	public int PerksMasteryBonus { get; private set; }

	// Token: 0x060041CE RID: 16846 RVA: 0x00139755 File Offset: 0x00137955
	public UIProgressCellsInfoWidgetData(int masteryValue, int masteryLock, TalentDef talentDef, bool isStarCraft, string tooltipHeaderLngId, int perksMasteryBonus = 0)
	{
		this.MasteryValue = masteryValue;
		this.MasteryLock = masteryLock;
		this.TalentDef = talentDef;
		this.IsStarCraft = isStarCraft;
		this.TooltipHeaderLngId = tooltipHeaderLngId;
		this.PerksMasteryBonus = perksMasteryBonus;
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x0013978C File Offset: 0x0013798C
	public string FormatPlayerMasteryValue()
	{
		if (this.PerksMasteryBonus == 0)
		{
			return this.MasteryValue.ToString();
		}
		int num = this.MasteryValue - this.PerksMasteryBonus;
		if (this.PerksMasteryBonus > 0)
		{
			return string.Format("{0}+{1}", num, this.PerksMasteryBonus);
		}
		return string.Format("{0}{1}", num, this.PerksMasteryBonus);
	}
}
