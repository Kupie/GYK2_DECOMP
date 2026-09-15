using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	public class SpriteAtlasInfo
	{
		// Token: 0x040001E3 RID: 483
		public string atlasName;

		// Token: 0x040001E4 RID: 484
		public string path;

		// Token: 0x040001E5 RID: 485
		public List<string> spriteNames = new List<string>();
	}
}
