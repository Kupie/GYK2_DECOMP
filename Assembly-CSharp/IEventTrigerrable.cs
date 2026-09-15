using System;

// Token: 0x020004A7 RID: 1191
public interface IEventTrigerrable
{
	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06001F93 RID: 8083
	string TriggerableId { get; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06001F94 RID: 8084
	GlobalEventsSystem.Event.Type Type { get; }

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06001F95 RID: 8085
	// (set) Token: 0x06001F96 RID: 8086
	SGuid UniqueId { get; set; }

	// Token: 0x06001F97 RID: 8087
	bool OnTriggerPassed();
}
