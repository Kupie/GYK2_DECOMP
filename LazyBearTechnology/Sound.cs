using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x020000D2 RID: 210
	[Serializable]
	public class Sound
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00012318 File Offset: 0x00010518
		public float AveragePlayingTime
		{
			get
			{
				if (this.averagePlayingTime <= -1f)
				{
					float num = 0f;
					int num2 = 0;
					foreach (Sample sample in this.samples)
					{
						if (sample.isEnabled)
						{
							num2++;
							num += sample.clip.length;
						}
					}
					this.averagePlayingTime = num / (float)num2;
				}
				return this.averagePlayingTime;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000123A4 File Offset: 0x000105A4
		public Sample RandomSample
		{
			get
			{
				int num = 0;
				while (num++ < 100)
				{
					Sample randomSample = this.GetRandomSample();
					if (randomSample.isEnabled)
					{
						return randomSample;
					}
				}
				Debug.LogError("Error: Sound.RandomSample() couldn't find any enabled sample. Returning null.");
				return null;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000123DC File Offset: 0x000105DC
		private Sample GetRandomSample()
		{
			if (!this.randomWithoutRepeat)
			{
				return this.samples.GetRandom<Sample>();
			}
			if (this.samples.Count <= 2)
			{
				return this.samples.GetRandom<Sample>();
			}
			Sample random;
			do
			{
				random = this.samples.GetRandom<Sample>();
			}
			while (random == this.previousSample);
			this.previousSample = random;
			return random;
		}

		// Token: 0x04000149 RID: 329
		public string id;

		// Token: 0x0400014A RID: 330
		public AudioSettings3DType audioSettings3DType;

		// Token: 0x0400014B RID: 331
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x0400014C RID: 332
		[Range(-1f, 1f)]
		public float panning;

		// Token: 0x0400014D RID: 333
		public bool loop;

		// Token: 0x0400014E RID: 334
		public AudioMixerGroup group;

		// Token: 0x0400014F RID: 335
		public List<Sample> samples = new List<Sample>();

		// Token: 0x04000150 RID: 336
		public bool randomWithoutRepeat = true;

		// Token: 0x04000151 RID: 337
		private Sample previousSample;

		// Token: 0x04000152 RID: 338
		private float averagePlayingTime = -1f;
	}
}
