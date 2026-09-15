using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000D1 RID: 209
	[Serializable]
	public class Sample
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000122A7 File Offset: 0x000104A7
		public float Pitch
		{
			get
			{
				if (this.pitchVariation != 0f)
				{
					return global::UnityEngine.Random.Range(this.pitch - this.pitchVariation, this.pitch + this.pitchVariation);
				}
				return this.pitch;
			}
		}

		// Token: 0x04000141 RID: 321
		public bool isEnabled = true;

		// Token: 0x04000142 RID: 322
		public AudioClip clip;

		// Token: 0x04000143 RID: 323
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x04000144 RID: 324
		[Range(-3f, 3f)]
		public float pitch = 1f;

		// Token: 0x04000145 RID: 325
		[Range(-1f, 1f)]
		public float panning;

		// Token: 0x04000146 RID: 326
		public float pitchVariation;

		// Token: 0x04000147 RID: 327
		public string filename = "";

		// Token: 0x04000148 RID: 328
		public float weight = 1f;
	}
}
