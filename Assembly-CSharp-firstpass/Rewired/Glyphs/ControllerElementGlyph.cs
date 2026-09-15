using System;

namespace Rewired.Glyphs
{
	// Token: 0x02000059 RID: 89
	public abstract class ControllerElementGlyph : ControllerElementGlyphBase
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0000CD2A File Offset: 0x0000AF2A
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x0000CD32 File Offset: 0x0000AF32
		public ActionElementMap actionElementMap
		{
			get
			{
				return this._actionElementMap;
			}
			set
			{
				this._actionElementMap = value;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0000CD3B File Offset: 0x0000AF3B
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0000CD43 File Offset: 0x0000AF43
		public ControllerElementIdentifier controllerElementIdentifier
		{
			get
			{
				return this._controllerElementIdentifier;
			}
			set
			{
				this._controllerElementIdentifier = value;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0000CD54 File Offset: 0x0000AF54
		public AxisRange axisRange
		{
			get
			{
				return this._axisRange;
			}
			set
			{
				this._axisRange = value;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000CD60 File Offset: 0x0000AF60
		protected override void Update()
		{
			base.Update();
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._actionElementMap == null && this.controllerElementIdentifier == null)
			{
				this.Hide();
				return;
			}
			if (this.actionElementMap != null)
			{
				this.ShowGlyphsOrText(this._actionElementMap);
			}
			else if (this.controllerElementIdentifier != null)
			{
				this.ShowGlyphsOrText(this._controllerElementIdentifier, this.axisRange);
			}
			this.EvaluateObjectVisibility();
		}

		// Token: 0x040002EB RID: 747
		[NonSerialized]
		private ActionElementMap _actionElementMap;

		// Token: 0x040002EC RID: 748
		[NonSerialized]
		private ControllerElementIdentifier _controllerElementIdentifier;

		// Token: 0x040002ED RID: 749
		[NonSerialized]
		private AxisRange _axisRange;
	}
}
