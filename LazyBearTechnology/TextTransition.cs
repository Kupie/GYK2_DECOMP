using System;
using TMPro;

namespace LazyBearTechnology
{
	// Token: 0x02000150 RID: 336
	[Serializable]
	public class TextTransition
	{
		// Token: 0x04000444 RID: 1092
		public TextMeshProUGUI targetLabel;

		// Token: 0x04000445 RID: 1093
		public TextStyle defaultStyle;

		// Token: 0x04000446 RID: 1094
		public TextStyle highlightedStyle;

		// Token: 0x04000447 RID: 1095
		public TextStyle pressedStyle;

		// Token: 0x04000448 RID: 1096
		public TextStyle selectedStyle;

		// Token: 0x04000449 RID: 1097
		public TextStyle disabledStyle;
	}
}
