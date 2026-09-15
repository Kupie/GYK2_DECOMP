using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200015A RID: 346
	[Serializable]
	public class TextStyleData
	{
		// Token: 0x04000487 RID: 1159
		public bool containsFontColor;

		// Token: 0x04000488 RID: 1160
		public Color fontColor = Color.white;

		// Token: 0x04000489 RID: 1161
		public bool containsOutline;

		// Token: 0x0400048A RID: 1162
		public bool eightSide;

		// Token: 0x0400048B RID: 1163
		public Color outlineColor = Color.white;

		// Token: 0x0400048C RID: 1164
		public bool containsSecondOutline;

		// Token: 0x0400048D RID: 1165
		public Color secondOutlineColor = Color.white;

		// Token: 0x0400048E RID: 1166
		public bool containsShadow;

		// Token: 0x0400048F RID: 1167
		public Color shadowOutlineColor = Color.white;

		// Token: 0x04000490 RID: 1168
		public bool containsOverlayTexture;

		// Token: 0x04000491 RID: 1169
		public Texture overlayTexture;

		// Token: 0x04000492 RID: 1170
		public bool containsLineSpacing = true;

		// Token: 0x04000493 RID: 1171
		public float lineSpacing;

		// Token: 0x04000494 RID: 1172
		[Space(10f)]
		[Tooltip("If true, this style wouldn't be affected by any locale font changes (for Asian fonts).")]
		public bool isAlwaysStaticFont;
	}
}
