using System;
using LazyBearTechnology;

// Token: 0x020009C8 RID: 2504
public class UIFightingSquadHudGroupWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060042A3 RID: 17059 RVA: 0x0013C746 File Offset: 0x0013A946
	// (set) Token: 0x060042A4 RID: 17060 RVA: 0x0013C74E File Offset: 0x0013A94E
	public FightingLevel FightingLevel { get; private set; }

	// Token: 0x060042A5 RID: 17061 RVA: 0x0013C757 File Offset: 0x0013A957
	public UIFightingSquadHudGroupWidgetData(FightingLevel fightingLevel)
	{
		this.FightingLevel = fightingLevel;
	}
}
