using System;

// Token: 0x0200027B RID: 635
public interface IAgentDecisionStep<TContext>
{
	// Token: 0x0600107B RID: 4219
	MobCommand TryCreateCommand(TContext context);
}
