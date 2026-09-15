using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class SpriteGlyphSet : GlyphSet
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0000E2A7 File Offset: 0x0000C4A7
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0000E2AF File Offset: 0x0000C4AF
		public List<SpriteGlyphSet.Entry> glyphs
		{
			get
			{
				return this._glyphs;
			}
			set
			{
				this._glyphs = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public override int glyphCount
		{
			get
			{
				if (this._glyphs == null)
				{
					return 0;
				}
				return this._glyphs.Count;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0000E2CF File Offset: 0x0000C4CF
		public override GlyphSet.EntryBase GetEntry(int index)
		{
			if (this._glyphs == null)
			{
				return null;
			}
			if (index >= this._glyphs.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._glyphs[index];
		}

		// Token: 0x0400031A RID: 794
		[Tooltip("The list of glyphs.")]
		[SerializeField]
		private List<SpriteGlyphSet.Entry> _glyphs;

		// Token: 0x0200006C RID: 108
		[Serializable]
		public class Entry : GlyphSet.EntryBase<Sprite>
		{
		}
	}
}
