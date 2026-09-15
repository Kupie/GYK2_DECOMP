using System;

// Token: 0x020003CD RID: 973
[Serializable]
public abstract class ConditionalDrawerActionBase : IConditionalDrawerAction
{
	// Token: 0x060019FD RID: 6653
	public abstract void Execute(ConditionalDrawerContext context, bool conditionMet);

	// Token: 0x060019FE RID: 6654 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Reset(ConditionalDrawerContext context)
	{
	}
}
