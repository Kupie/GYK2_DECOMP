using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public static class BuildConsts
{
	// Token: 0x04000967 RID: 2407
	public static readonly Vector2Int BUILD_GRID_DIVIDER = new Vector2Int(3, 4);

	// Token: 0x04000968 RID: 2408
	public static readonly Vector2Int BUILD_GRID_SIZE = new Vector2Int(48 / BuildConsts.BUILD_GRID_DIVIDER.x, 48 / BuildConsts.BUILD_GRID_DIVIDER.y);

	// Token: 0x04000969 RID: 2409
	public static readonly Vector2 BUILD_GRID_SIZE_WORLD_UNIT = Vector2.Scale(BuildConsts.BUILD_GRID_SIZE, new Vector2(0.01f, 0.0125f)) * 2f;

	// Token: 0x0400096A RID: 2410
	public const float CORRECTION_BUILD_GRID_SCALE_Z = 0.6f;

	// Token: 0x0400096B RID: 2411
	public static readonly Vector2 CELL_SIZE = BuildConsts.BUILD_GRID_SIZE * new Vector2(0.01f, 0.0125f) * 2f;

	// Token: 0x0400096C RID: 2412
	public static readonly Vector3 CASTING_BOX_HALF_EXTENTS = new Vector3(BuildConsts.CELL_SIZE.x, 8f, BuildConsts.CELL_SIZE.y) / 2f - VisualConsts.XYZ_STEP;
}
