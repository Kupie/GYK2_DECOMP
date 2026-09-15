using System;
using System.Collections.Generic;

// Token: 0x020003BD RID: 957
[Serializable]
public class SpawnStage
{
	// Token: 0x040018F8 RID: 6392
	public string stageName;

	// Token: 0x040018F9 RID: 6393
	public string craftId;

	// Token: 0x040018FA RID: 6394
	public List<SpawnVariation> variations = new List<SpawnVariation>();
}
