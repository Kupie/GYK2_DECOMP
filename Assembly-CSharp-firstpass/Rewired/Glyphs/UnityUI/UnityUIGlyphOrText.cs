using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x02000071 RID: 113
	public class UnityUIGlyphOrText : GlyphOrTextBase<Image, Sprite, Text>
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0000E578 File Offset: 0x0000C778
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0000E599 File Offset: 0x0000C799
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

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0000E5B6 File Offset: 0x0000C7B6
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0000E5D3 File Offset: 0x0000C7D3
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
