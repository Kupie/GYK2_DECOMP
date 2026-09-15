using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x0200005D RID: 93
	[Serializable]
	public class ControllerElementGlyphSelectorOptions
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000D35E File Offset: 0x0000B55E
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0000D366 File Offset: 0x0000B566
		public bool useLastActiveController
		{
			get
			{
				return this._useLastActiveController;
			}
			set
			{
				this._useLastActiveController = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000D36F File Offset: 0x0000B56F
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0000D377 File Offset: 0x0000B577
		public ControllerType[] controllerTypeOrder
		{
			get
			{
				return this._controllerTypeOrder;
			}
			set
			{
				this._controllerTypeOrder = value;
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000D380 File Offset: 0x0000B580
		public virtual bool TryGetControllerTypeOrder(int index, out ControllerType controllerType)
		{
			if (index >= this._controllerTypeOrder.Length)
			{
				controllerType = ControllerType.Keyboard;
				return false;
			}
			controllerType = this._controllerTypeOrder[index];
			return true;
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000D39D File Offset: 0x0000B59D
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x0000D3B7 File Offset: 0x0000B5B7
		public static ControllerElementGlyphSelectorOptions defaultOptions
		{
			get
			{
				if (ControllerElementGlyphSelectorOptions.s_defaultOptions == null)
				{
					return ControllerElementGlyphSelectorOptions.s_defaultOptions = new ControllerElementGlyphSelectorOptions();
				}
				return ControllerElementGlyphSelectorOptions.s_defaultOptions;
			}
			set
			{
				ControllerElementGlyphSelectorOptions.s_defaultOptions = value;
			}
		}

		// Token: 0x040002FA RID: 762
		[Tooltip("Determines if the Player's last active controller is used for glyph selection.")]
		[SerializeField]
		private bool _useLastActiveController = true;

		// Token: 0x040002FB RID: 763
		[Tooltip("List of controller type priority. First in list corresponds to highest priority. This determines which controller types take precedence when displaying glyphs. If use last active controller is enabled, the active controller will always take priority, however, if there is no last active controller, selection will fall back based on this priority. In addition, keyboard and mouse are treated as a single controller for the purposes of glyph handling, so to prioritze keyboard over mouse or vice versa, the one that is lower in the list will take precedence.")]
		[SerializeField]
		private ControllerType[] _controllerTypeOrder = new ControllerType[]
		{
			ControllerType.Joystick,
			ControllerType.Custom,
			ControllerType.Mouse,
			ControllerType.Keyboard
		};

		// Token: 0x040002FC RID: 764
		private static ControllerElementGlyphSelectorOptions s_defaultOptions;
	}
}
