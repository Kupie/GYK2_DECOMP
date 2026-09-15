using System;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x0200014F RID: 335
	[Serializable]
	public class IconTransition
	{
		// Token: 0x0400043E RID: 1086
		public Image targetImage;

		// Token: 0x0400043F RID: 1087
		public Sprite defaultSprite;

		// Token: 0x04000440 RID: 1088
		public Sprite highlightedSprite;

		// Token: 0x04000441 RID: 1089
		public Sprite pressedSprite;

		// Token: 0x04000442 RID: 1090
		public Sprite selectedSprite;

		// Token: 0x04000443 RID: 1091
		public Sprite disabledSprite;
	}
}
