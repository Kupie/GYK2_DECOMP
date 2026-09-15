using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000063 RID: 99
	public abstract class GlyphOrTextBase<TGlyphComponent, TGlyphGraphic, TTextComponent> : GlyphOrTextBase where TGlyphComponent : Behaviour where TGlyphGraphic : class where TTextComponent : Behaviour
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0000D457 File Offset: 0x0000B657
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x0000D45F File Offset: 0x0000B65F
		public TTextComponent textComponent
		{
			get
			{
				return this._textComponent;
			}
			set
			{
				this._textComponent = value;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000D468 File Offset: 0x0000B668
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x0000D470 File Offset: 0x0000B670
		public TGlyphComponent glyphComponent
		{
			get
			{
				return this._glyphComponent;
			}
			set
			{
				this._glyphComponent = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600059C RID: 1436
		// (set) Token: 0x0600059D RID: 1437
		protected abstract TGlyphGraphic glyphGraphic { get; set; }

		// Token: 0x0600059E RID: 1438 RVA: 0x0000D47C File Offset: 0x0000B67C
		public override void ShowText(string text)
		{
			if (this._textComponent == null)
			{
				return;
			}
			if (!string.Equals(this.textString, text, StringComparison.Ordinal))
			{
				this.textString = text;
			}
			if (!this._textComponent.gameObject.activeSelf)
			{
				this._textComponent.gameObject.SetActive(true);
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
			}
			this.Hide(GlyphOrTextBase.TypeFlags.Glyph);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0000D500 File Offset: 0x0000B700
		public override void ShowGlyph(object glyph)
		{
			if (glyph != null && !(glyph is TGlyphGraphic))
			{
				Debug.LogError("Rewired: Glyph does not implement " + typeof(TGlyphGraphic).Name + ".");
				return;
			}
			this.ShowGlyph((TGlyphGraphic)((object)glyph));
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000D540 File Offset: 0x0000B740
		public virtual void ShowGlyph(TGlyphGraphic glyph)
		{
			if (this._glyphComponent == null)
			{
				return;
			}
			if (this.glyphGraphic != glyph)
			{
				this.glyphGraphic = glyph;
			}
			if (!this._glyphComponent.gameObject.activeSelf)
			{
				this._glyphComponent.gameObject.SetActive(true);
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
			}
			this.Hide(GlyphOrTextBase.TypeFlags.Text);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		protected override void Hide(GlyphOrTextBase.TypeFlags flags)
		{
			if (this._textComponent != null && (flags & GlyphOrTextBase.TypeFlags.Text) != GlyphOrTextBase.TypeFlags.None && this._textComponent.gameObject.activeSelf)
			{
				this._textComponent.gameObject.SetActive(false);
			}
			if (this._glyphComponent != null && (flags & GlyphOrTextBase.TypeFlags.Glyph) != GlyphOrTextBase.TypeFlags.None && this._glyphComponent.gameObject.activeSelf)
			{
				this._glyphComponent.gameObject.SetActive(false);
			}
			if ((this._glyphComponent == null || !this._glyphComponent.gameObject.activeSelf) && (this._textComponent == null || !this._textComponent.gameObject.activeSelf))
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000305 RID: 773
		[SerializeField]
		private TTextComponent _textComponent;

		// Token: 0x04000306 RID: 774
		[SerializeField]
		private TGlyphComponent _glyphComponent;
	}
}
