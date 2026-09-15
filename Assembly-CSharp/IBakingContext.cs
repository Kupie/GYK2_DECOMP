using System;
using System.Collections.Generic;

// Token: 0x020006EC RID: 1772
public interface IBakingContext
{
	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06002ED6 RID: 11990
	List<BakedChunkableObjectComponentData> GetBakedData { get; }
}
