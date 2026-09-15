using System;

namespace LazyBearTechnology
{
	// Token: 0x0200013C RID: 316
	public interface ILazyCustomSerialize
	{
		// Token: 0x0600064B RID: 1611
		void OnLazyPreSerialize();

		// Token: 0x0600064C RID: 1612
		void OnLazyPostDeserialize();
	}
}
