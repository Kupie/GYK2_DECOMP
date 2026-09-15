using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x020000CC RID: 204
	[Serializable]
	public class PlaylistData
	{
		// Token: 0x0400012C RID: 300
		public string id;

		// Token: 0x0400012D RID: 301
		public float volume;

		// Token: 0x0400012E RID: 302
		public List<TrackData> tracks = new List<TrackData>();
	}
}
