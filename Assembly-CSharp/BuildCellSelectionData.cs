using System;
using UnityEngine;

// Token: 0x02000149 RID: 329
public struct BuildCellSelectionData
{
	// Token: 0x060007DB RID: 2011 RVA: 0x00026C13 File Offset: 0x00024E13
	public BuildCellSelectionData(Vector3 coords, int state)
	{
		this.coords = coords;
		this.state = state;
	}

	// Token: 0x040009D3 RID: 2515
	public int state;

	// Token: 0x040009D4 RID: 2516
	public Vector3 coords;
}
