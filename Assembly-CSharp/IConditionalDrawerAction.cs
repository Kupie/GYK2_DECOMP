using System;

// Token: 0x020003FD RID: 1021
public interface IConditionalDrawerAction
{
	// Token: 0x06001AB0 RID: 6832
	void Execute(ConditionalDrawerContext context, bool conditionMet);

	// Token: 0x06001AB1 RID: 6833
	void Reset(ConditionalDrawerContext context);
}
