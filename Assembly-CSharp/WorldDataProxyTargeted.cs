using System;

// Token: 0x02000757 RID: 1879
public class WorldDataProxyTargeted : WorldData, IProxyClassTargeted<WorldData>
{
	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000E8BC5 File Offset: 0x000E6DC5
	// (set) Token: 0x060030C6 RID: 12486 RVA: 0x000E8BCD File Offset: 0x000E6DCD
	public WorldData Target { get; set; }
}
