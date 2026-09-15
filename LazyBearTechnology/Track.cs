using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000D4 RID: 212
	[Serializable]
	public class Track
	{
		// Token: 0x04000157 RID: 343
		public string id;

		// Token: 0x04000158 RID: 344
		public AudioClip clip;

		// Token: 0x04000159 RID: 345
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x0400015A RID: 346
		public bool loop;

		// Token: 0x0400015B RID: 347
		[SerializeField]
		[Range(-3f, 3f)]
		public float pitch = 1f;

		// Token: 0x0400015C RID: 348
		[Range(-1f, 1f)]
		public float panning;

		// Token: 0x0400015D RID: 349
		public float weight = 1f;
	}
}
