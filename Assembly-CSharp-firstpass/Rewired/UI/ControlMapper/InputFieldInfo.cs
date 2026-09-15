using System;
using Rewired.Glyphs.UnityUI;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000C3 RID: 195
	[AddComponentMenu("")]
	public class InputFieldInfo : UIElementInfo
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0001D53D File Offset: 0x0001B73D
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0001D545 File Offset: 0x0001B745
		public UnityUIControllerElementGlyph glyphOrText { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0001D54E File Offset: 0x0001B74E
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0001D556 File Offset: 0x0001B756
		public int actionId { get; set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001D55F File Offset: 0x0001B75F
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0001D567 File Offset: 0x0001B767
		public AxisRange axisRange
		{
			get
			{
				return this._axisRange;
			}
			set
			{
				this._axisRange = value;
				if (this.glyphOrText != null)
				{
					this.glyphOrText.axisRange = value;
				}
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0001D58A File Offset: 0x0001B78A
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0001D592 File Offset: 0x0001B792
		public int actionElementMapId
		{
			get
			{
				return this._actionElementMapId;
			}
			set
			{
				this._actionElementMapId = value;
				if (this.glyphOrText != null)
				{
					this.glyphOrText.actionElementMap = ReInput.mapping.GetActionElementMap(value);
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0001D5BF File Offset: 0x0001B7BF
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0001D5C7 File Offset: 0x0001B7C7
		public ControllerType controllerType { get; set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001D5D0 File Offset: 0x0001B7D0
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
		public int controllerId { get; set; }

		// Token: 0x0400050F RID: 1295
		private int _actionElementMapId;

		// Token: 0x04000510 RID: 1296
		private AxisRange _axisRange;
	}
}
