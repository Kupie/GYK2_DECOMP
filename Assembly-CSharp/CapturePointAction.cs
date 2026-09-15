using System;

// Token: 0x02000308 RID: 776
[Serializable]
public abstract class CapturePointAction
{
	// Token: 0x060014B9 RID: 5305 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Execute(LazyConsts.Fighting.TeamType teamType, FightingCapturePoint point)
	{
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Execute(LazyConsts.Fighting.TeamType teamType, FightingLevel level)
	{
	}

	// Token: 0x04001575 RID: 5493
	public LazyConsts.Fighting.TeamType teamType;
}
