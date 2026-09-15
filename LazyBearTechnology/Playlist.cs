using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x020000D0 RID: 208
	[Serializable]
	public class Playlist
	{
		// Token: 0x04000139 RID: 313
		public string id;

		// Token: 0x0400013A RID: 314
		public bool shuffled;

		// Token: 0x0400013B RID: 315
		public bool weightRandomized;

		// Token: 0x0400013C RID: 316
		public bool loop;

		// Token: 0x0400013D RID: 317
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x0400013E RID: 318
		public float fadeDuration;

		// Token: 0x0400013F RID: 319
		public AudioMixerGroup group;

		// Token: 0x04000140 RID: 320
		public List<Track> tracks = new List<Track>();
	}
}
