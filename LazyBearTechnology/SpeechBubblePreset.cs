using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000141 RID: 321
	[Serializable]
	public class SpeechBubblePreset
	{
		// Token: 0x040003B8 RID: 952
		public Color backgroundColor = Color.white;

		// Token: 0x040003B9 RID: 953
		public TextStyle textStyle;

		// Token: 0x040003BA RID: 954
		public TextStyle highlightedTextColorStyle;

		// Token: 0x040003BB RID: 955
		public Sprite backgroundSprite;

		// Token: 0x040003BC RID: 956
		public Sprite cornerSprite;

		// Token: 0x040003BD RID: 957
		public bool centered;

		// Token: 0x040003BE RID: 958
		public Vector2[] cornerOffset;

		// Token: 0x040003BF RID: 959
		public Vector2[] spriteOffset;
	}
}
