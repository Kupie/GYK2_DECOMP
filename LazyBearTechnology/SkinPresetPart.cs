using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000171 RID: 369
	[Serializable]
	public class SkinPresetPart
	{
		// Token: 0x040004F4 RID: 1268
		public int id;

		// Token: 0x040004F5 RID: 1269
		public Color color = Color.white;

		// Token: 0x040004F6 RID: 1270
		public float hue;

		// Token: 0x040004F7 RID: 1271
		public float saturation = 1f;

		// Token: 0x040004F8 RID: 1272
		public float velocity = 1f;

		// Token: 0x040004F9 RID: 1273
		public Texture2D palette;
	}
}
