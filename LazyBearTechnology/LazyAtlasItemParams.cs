using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200011F RID: 287
	[Serializable]
	public class LazyAtlasItemParams
	{
		// Token: 0x040002C1 RID: 705
		[LazyAtlasTableCell]
		public string name;

		// Token: 0x040002C2 RID: 706
		[LazyAtlasTableCell]
		public int offsetX;

		// Token: 0x040002C3 RID: 707
		[LazyAtlasTableCell]
		public int offsetY;

		// Token: 0x040002C4 RID: 708
		[Tooltip("Extra Advance")]
		[LazyAtlasTableCell]
		public int extraAdvance;
	}
}
