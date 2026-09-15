using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x02000072 RID: 114
	public class UnityUIGlyphOrTextTMPro : GlyphOrTextBase<Image, Sprite, TMP_Text>
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0000E619 File Offset: 0x0000C819
		protected override string textString
		{
			get
			{
				if (!(base.textComponent != null))
				{
					return string.Empty;
				}
				return base.textComponent.text;
			}
			set
			{
				if (base.textComponent == null)
				{
					return;
				}
				base.textComponent.text = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0000E636 File Offset: 0x0000C836
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0000E653 File Offset: 0x0000C853
		protected override Sprite glyphGraphic
		{
			get
			{
				if (!(base.glyphComponent != null))
				{
					return null;
				}
				return base.glyphComponent.sprite;
			}
			set
			{
				if (base.glyphComponent == null)
				{
					return;
				}
				base.glyphComponent.sprite = value;
			}
		}
	}
}
