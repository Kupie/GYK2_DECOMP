using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public struct BuildCellBuffUsageData
{
	// Token: 0x060007DC RID: 2012 RVA: 0x00026C23 File Offset: 0x00024E23
	public BuildCellBuffUsageData(Vector3 coords, int state)
	{
		this.coords = coords;
		this.state = state;
	}

	// Token: 0x040009D5 RID: 2517
	public int state;

	// Token: 0x040009D6 RID: 2518
	public Vector3 coords;
}
