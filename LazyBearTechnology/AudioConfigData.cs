using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x020000C9 RID: 201
	[Serializable]
	public class AudioConfigData
	{
		// Token: 0x04000121 RID: 289
		public List<SoundData> sounds = new List<SoundData>();

		// Token: 0x04000122 RID: 290
		public List<PlaylistData> playlists = new List<PlaylistData>();
	}
}
