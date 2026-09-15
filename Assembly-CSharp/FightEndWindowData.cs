using System;
using LazyBearTechnology;

// Token: 0x020009BD RID: 2493
public class FightEndWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x0600426D RID: 17005 RVA: 0x0013B6B9 File Offset: 0x001398B9
	// (set) Token: 0x0600426E RID: 17006 RVA: 0x0013B6C1 File Offset: 0x001398C1
	public FightDef FightDefinition { get; private set; }

	// Token: 0x0600426F RID: 17007 RVA: 0x0013B6CA File Offset: 0x001398CA
	public FightEndWindowData(FightDef fightDefinition)
	{
		this.FightDefinition = fightDefinition;
	}
}
