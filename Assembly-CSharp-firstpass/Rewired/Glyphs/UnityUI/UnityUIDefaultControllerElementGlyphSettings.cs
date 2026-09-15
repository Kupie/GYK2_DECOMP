using System;
using UnityEngine;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x0200006F RID: 111
	[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Default Controller Element Glyph Settings")]
	public class UnityUIDefaultControllerElementGlyphSettings : DefaultControllerElementGlyphSettingsBase
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x0000E45D File Offset: 0x0000C65D
		protected override void SetDefaultGlyphOrTextPrefab()
		{
			UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab = base.glyphOrTextPrefab;
		}
	}
}
