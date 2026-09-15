using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000181 RID: 385
	public class LazyRandom
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0002A14B File Offset: 0x0002834B
		public int Seed
		{
			get
			{
				return this.seed;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002A154 File Offset: 0x00028354
		public LazyRandom(int? seed = null)
		{
			this.seed = seed ?? global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			this.random = new global::System.Random(this.seed);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002A1A4 File Offset: 0x000283A4
		public int Range(int minValue, int maxValue)
		{
			int num = Mathf.RoundToInt((float)this.random.NextDouble() * (float)(maxValue - minValue) + (float)minValue);
			if (num == maxValue)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002A1D4 File Offset: 0x000283D4
		public float Range(float minValue, float maxValue)
		{
			return (float)this.random.NextDouble() * (maxValue - minValue) + minValue;
		}

		// Token: 0x04000541 RID: 1345
		private int seed;

		// Token: 0x04000542 RID: 1346
		private global::System.Random random;
	}
}
