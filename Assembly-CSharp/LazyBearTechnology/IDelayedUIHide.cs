using System;

namespace LazyBearTechnology
{
	// Token: 0x02000C56 RID: 3158
	public interface IDelayedUIHide
	{
		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600507E RID: 20606
		bool ShouldDelayHide { get; }

		// Token: 0x0600507F RID: 20607
		void AddHideAfterDelayCallback(Action callback);

		// Token: 0x06005080 RID: 20608
		void ForceCancelDelayedHide();
	}
}
