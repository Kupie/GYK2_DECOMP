using System;

// Token: 0x020003FE RID: 1022
public interface IConditionalDrawerCondition
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06001AB2 RID: 6834
	ConditionalEventType EventType { get; }

	// Token: 0x06001AB3 RID: 6835
	bool Evaluate(ConditionalDrawerContext context);

	// Token: 0x06001AB4 RID: 6836
	bool IsValid(ConditionalDrawerContext context);
}
