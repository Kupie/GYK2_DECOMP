using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x020000CA RID: 202
	[Serializable]
	public class SoundData
	{
		// Token: 0x04000123 RID: 291
		public string id;

		// Token: 0x04000124 RID: 292
		public float volume;

		// Token: 0x04000125 RID: 293
		public float panning;

		// Token: 0x04000126 RID: 294
		public List<SampleData> samples = new List<SampleData>();
	}
}
