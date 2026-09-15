using System;

namespace LazyBearTechnology
{
	// Token: 0x020000F6 RID: 246
	[Serializable]
	public class PlaylistWeight
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x000179B9 File Offset: 0x00015BB9
		public PlaylistWeight()
		{
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000179C1 File Offset: 0x00015BC1
		public PlaylistWeight(string playlistId, string trackId, float weight)
		{
			this.playlistId = playlistId;
			this.trackId = trackId;
			this.weight = weight;
		}

		// Token: 0x04000217 RID: 535
		public string playlistId;

		// Token: 0x04000218 RID: 536
		public string trackId;

		// Token: 0x04000219 RID: 537
		public float weight;
	}
}
