using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	public class ControllerElementGlyphSelectorOptionsSO : ControllerElementGlyphSelectorOptionsSOBase
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000D3E5 File Offset: 0x0000B5E5
		public override ControllerElementGlyphSelectorOptions options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x040002FD RID: 765
		[SerializeField]
		private ControllerElementGlyphSelectorOptions _options;
	}
}
