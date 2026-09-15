using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class ReplacementKeysMetadata
	{
		// Token: 0x04000268 RID: 616
		public string id;

		// Token: 0x04000269 RID: 617
		public List<int> keysIndexes = new List<int>();

		// Token: 0x0400026A RID: 618
		public List<string> keys = new List<string>();
	}
}
