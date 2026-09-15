using System;
using System.Collections;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x020000F0 RID: 240
	public interface IBalance
	{
		// Token: 0x06000432 RID: 1074
		void InitBalance();

		// Token: 0x06000433 RID: 1075
		void ClearBalance();

		// Token: 0x06000434 RID: 1076
		Dictionary<string, IList> GetAllDataListsAndGoogleTabs();

		// Token: 0x06000435 RID: 1077
		Dictionary<string, IList> GetAllTabs();
	}
}
