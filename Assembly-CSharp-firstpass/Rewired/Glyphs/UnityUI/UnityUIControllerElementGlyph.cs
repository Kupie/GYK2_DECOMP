using System;
using UnityEngine;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x0200006D RID: 109
	[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Controller Element Glyph")]
	public class UnityUIControllerElementGlyph : ControllerElementGlyph
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0000E310 File Offset: 0x0000C510
		protected override GameObject GetDefaultGlyphOrTextPrefab()
		{
			return UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab;
		}
	}
}
