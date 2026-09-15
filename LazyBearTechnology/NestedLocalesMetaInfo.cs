using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x02000105 RID: 261
	[Serializable]
	public class NestedLocalesMetaInfo
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x0001C13A File Offset: 0x0001A33A
		public void AddEntry(int idxToInsert, string localeIDToInsert)
		{
			this.hasMetaInfo = true;
			this.idsToInsert.Add(idxToInsert);
			this.localeIDsToInsert.Add(localeIDToInsert);
		}

		// Token: 0x04000265 RID: 613
		public bool hasMetaInfo;

		// Token: 0x04000266 RID: 614
		public List<int> idsToInsert = new List<int>();

		// Token: 0x04000267 RID: 615
		public List<string> localeIDsToInsert = new List<string>();
	}
}
