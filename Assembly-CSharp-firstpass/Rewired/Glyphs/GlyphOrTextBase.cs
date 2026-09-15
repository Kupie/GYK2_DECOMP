using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000061 RID: 97
	public abstract class GlyphOrTextBase : MonoBehaviour
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000591 RID: 1425
		// (set) Token: 0x06000592 RID: 1426
		protected abstract string textString { get; set; }

		// Token: 0x06000593 RID: 1427
		public abstract void ShowText(string text);

		// Token: 0x06000594 RID: 1428
		public abstract void ShowGlyph(object glyph);

		// Token: 0x06000595 RID: 1429 RVA: 0x0000D44E File Offset: 0x0000B64E
		public virtual void Hide()
		{
			this.Hide(GlyphOrTextBase.TypeFlags.All);
		}

		// Token: 0x06000596 RID: 1430
		protected abstract void Hide(GlyphOrTextBase.TypeFlags flags);

		// Token: 0x02000062 RID: 98
		[Flags]
		protected enum TypeFlags
		{
			// Token: 0x04000301 RID: 769
			None = 0,
			// Token: 0x04000302 RID: 770
			Glyph = 1,
			// Token: 0x04000303 RID: 771
			Text = 2,
			// Token: 0x04000304 RID: 772
			All = -1
		}
	}
}
