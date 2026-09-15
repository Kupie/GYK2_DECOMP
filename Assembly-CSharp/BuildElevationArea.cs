using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public readonly struct BuildElevationArea
{
	// Token: 0x06000B66 RID: 2918 RVA: 0x00038C7C File Offset: 0x00036E7C
	public BuildElevationArea(Rect groundRect, float elevationY, float groundY)
	{
		this.GroundRect = groundRect;
		this.ElevationY = elevationY;
		this.GroundY = groundY;
	}

	// Token: 0x04000C8D RID: 3213
	public readonly Rect GroundRect;

	// Token: 0x04000C8E RID: 3214
	public readonly float ElevationY;

	// Token: 0x04000C8F RID: 3215
	public readonly float GroundY;
}
