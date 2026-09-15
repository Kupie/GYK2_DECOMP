using System;
using Pathfinding;

// Token: 0x0200054F RID: 1359
public class GDPointNode : PointNode
{
	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000A346E File Offset: 0x000A166E
	public GDPointData GdPointData { get; }

	// Token: 0x060022E9 RID: 8937 RVA: 0x000A3476 File Offset: 0x000A1676
	public GDPointNode(AstarPath aStarPath, GDPointData gdPointData)
		: base(aStarPath)
	{
		this.GdPointData = gdPointData;
	}
}
