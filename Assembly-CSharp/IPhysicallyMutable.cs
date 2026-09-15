using System;

// Token: 0x02000375 RID: 885
public interface IPhysicallyMutable
{
	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06001789 RID: 6025
	// (set) Token: 0x0600178A RID: 6026
	bool IsMuted { get; set; }

	// Token: 0x0600178B RID: 6027
	void Mute(Action onUnmuted = null);
}
