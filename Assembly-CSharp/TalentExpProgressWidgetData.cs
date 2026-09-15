using System;
using LazyBearTechnology;

// Token: 0x02000921 RID: 2337
public class TalentExpProgressWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x00126730 File Offset: 0x00124930
	// (set) Token: 0x06003DA2 RID: 15778 RVA: 0x00126738 File Offset: 0x00124938
	public int CurExp { get; private set; }

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x00126741 File Offset: 0x00124941
	// (set) Token: 0x06003DA4 RID: 15780 RVA: 0x00126749 File Offset: 0x00124949
	public int TotalExp { get; private set; }

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x00126752 File Offset: 0x00124952
	// (set) Token: 0x06003DA6 RID: 15782 RVA: 0x0012675A File Offset: 0x0012495A
	public int TalentExpPoints { get; private set; }

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x00126763 File Offset: 0x00124963
	// (set) Token: 0x06003DA8 RID: 15784 RVA: 0x0012676B File Offset: 0x0012496B
	public string TalentId { get; private set; }

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00126774 File Offset: 0x00124974
	public TalentExpProgressWidgetData(TalentData talentData)
	{
		this.CurExp = talentData.curExp;
		this.TotalExp = talentData.talentExpLevelBalanceData.GetExpForLevel(talentData.curTalentLevel);
		this.TalentExpPoints = talentData.talentExpPoints;
		this.TalentId = talentData.id;
	}
}
