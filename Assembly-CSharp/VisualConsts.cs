using System;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public static class VisualConsts
{
	// Token: 0x0600220F RID: 8719 RVA: 0x0009FEC8 File Offset: 0x0009E0C8
	public static Vector3 GetLayerOffset(int layer)
	{
		float num = 0.0001f * (float)layer;
		return new Vector3(0f, num, -num * 0.75f);
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x0009FEF4 File Offset: 0x0009E0F4
	public static Vector3 GetFightDecalLayerOffset(int layer)
	{
		float num = 0.002f * (float)layer;
		return new Vector3(0f, num, -num * 0.75f);
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x0009FF20 File Offset: 0x0009E120
	public static Vector3 ProjectGroundPointToElevation(Vector3 groundPoint, float elevationY)
	{
		float num = elevationY - groundPoint.y;
		return new Vector3(groundPoint.x, elevationY, groundPoint.z - num * 0.75f);
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x0009FF50 File Offset: 0x0009E150
	public static Vector3 ProjectElevationPointToGround(Vector3 elevatedPoint, float groundY)
	{
		float num = elevatedPoint.y - groundY;
		return new Vector3(elevatedPoint.x, groundY, elevatedPoint.z + num * 0.75f);
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x0009FF80 File Offset: 0x0009E180
	public static Vector3 SnapGroundPointToReferenceGridPhase(Vector3 groundPoint, Vector3 refGroundPoint, float groundY)
	{
		Vector3 vector = groundPoint - refGroundPoint;
		Vector2 build_GRID_SIZE_WORLD_UNIT = BuildConsts.BUILD_GRID_SIZE_WORLD_UNIT;
		return refGroundPoint + new Vector3(Mathf.Round(vector.x / build_GRID_SIZE_WORLD_UNIT.x) * build_GRID_SIZE_WORLD_UNIT.x, groundY, Mathf.Round(vector.z / build_GRID_SIZE_WORLD_UNIT.y) * build_GRID_SIZE_WORLD_UNIT.y);
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06002214 RID: 8724 RVA: 0x0009FFD9 File Offset: 0x0009E1D9
	public static Vector3 XYZ_STEP
	{
		get
		{
			return new Vector3(0.01f, 0.01666667f, 0.0125f);
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06002215 RID: 8725 RVA: 0x0009FFEF File Offset: 0x0009E1EF
	public static Vector3 XYZ_STEP_INV
	{
		get
		{
			return new Vector3(100f, 59.99999f, 80f);
		}
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000A0008 File Offset: 0x0009E208
	public static Vector3 GetRoundedPosXZ(Vector3 pos, int pixelSize = 2, int step = 1)
	{
		float num = 0.01f * (float)pixelSize * (float)step;
		float num2 = 0.0125f * (float)pixelSize * (float)step;
		return new Vector3(Mathf.Round(pos.x / num) * num, pos.y, Mathf.Round(pos.z / num2) * num2);
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000A0058 File Offset: 0x0009E258
	public static Vector3 GetRoundedPosXZMPRounding(Vector3 pos, int pixelSize = 2, int step = 1, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
	{
		float num = 0.01f * (float)pixelSize * (float)step;
		float num2 = 0.0125f * (float)pixelSize * (float)step;
		return new Vector3((float)Math.Round((double)(pos.x / num), midpointRounding) * num, pos.y, (float)Math.Round((double)(pos.z / num2), midpointRounding) * num2);
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000A00AC File Offset: 0x0009E2AC
	public static Vector3 GetRoundedPosXZ(Vector3 pos, Vector2Int step)
	{
		float num = 0.01f * (float)step.x * 2f;
		float num2 = 0.0125f * (float)step.y * 2f;
		return new Vector3(Mathf.Round(pos.x / num) * num, pos.y, Mathf.Round(pos.z / num2) * num2);
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000A010C File Offset: 0x0009E30C
	public static Vector3 GetRoundedPosY(Vector3 pos, int step = 1, int pixelSize = 2)
	{
		float num = 0.01666667f * (float)pixelSize * (float)step;
		pos.y = Mathf.Round(pos.y / num) * num;
		return pos;
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000A013C File Offset: 0x0009E33C
	public static Vector3 GetRoundedPosYMPRounding(Vector3 pos, int step = 1, int pixelSize = 2, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
	{
		float num = 0.01666667f * (float)pixelSize * (float)step;
		pos.y = (float)Math.Round((double)(pos.y / num), midpointRounding) * num;
		return pos;
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000A0170 File Offset: 0x0009E370
	public static Vector3 GetRoundedPosXYZ(Vector3 pos, int pixelSize = 2, int step = 1)
	{
		Vector3 roundedPosXZ = VisualConsts.GetRoundedPosXZ(pos, pixelSize, step);
		return new Vector3(roundedPosXZ.x, VisualConsts.GetRoundedPosY(pos, step, pixelSize).y, roundedPosXZ.z);
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000A01A4 File Offset: 0x0009E3A4
	public static Vector3 GetRoundedPosXYZMPRounding(Vector3 pos, int pixelSize = 2, int step = 1, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
	{
		Vector3 roundedPosXZMPRounding = VisualConsts.GetRoundedPosXZMPRounding(pos, pixelSize, step, midpointRounding);
		return new Vector3(roundedPosXZMPRounding.x, VisualConsts.GetRoundedPosYMPRounding(pos, step, pixelSize, MidpointRounding.AwayFromZero).y, roundedPosXZMPRounding.z);
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000A01DC File Offset: 0x0009E3DC
	public static Vector3 GetRoundedPosXYZCustomStepY(Vector3 pos, int pixelSize = 2, int stepXZ = 1, int stepY = 1)
	{
		Vector3 roundedPosXZ = VisualConsts.GetRoundedPosXZ(pos, pixelSize, stepXZ);
		return new Vector3(roundedPosXZ.x, VisualConsts.GetRoundedPosY(pos, stepY, 2).y, roundedPosXZ.z);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000A0210 File Offset: 0x0009E410
	public static Vector3 GetRoundedPosXYZCustomStepY(Vector3 pos, Vector2Int stepXZ, int stepY)
	{
		Vector3 roundedPosXZ = VisualConsts.GetRoundedPosXZ(pos, stepXZ);
		return new Vector3(roundedPosXZ.x, VisualConsts.GetRoundedPosY(pos, stepY, 2).y, roundedPosXZ.z);
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000A0244 File Offset: 0x0009E444
	public static Bounds GetGreaterRoundedBoundsXZ(Bounds bounds, int step = 1)
	{
		Bounds bounds2 = bounds;
		bounds2.max = VisualConsts.GetRoundedPosXZ(bounds2.max, 2, step);
		bounds2.min = VisualConsts.GetRoundedPosXZ(bounds2.min, 2, step);
		float num = 0.02f * (float)step;
		float num2 = 0.025f * (float)step;
		if (bounds2.min.x - bounds.min.x > 0.001f)
		{
			bounds2.min += Vector3.left * num;
		}
		if (bounds2.min.z - bounds.min.z > 0.001f)
		{
			bounds2.min += Vector3.back * num2;
		}
		if (bounds2.max.x - bounds.max.x < -0.001f)
		{
			bounds2.max += Vector3.right * num;
		}
		if (bounds2.max.z - bounds.max.z < -0.001f)
		{
			bounds2.max += Vector3.forward * num2;
		}
		return bounds2;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000A0384 File Offset: 0x0009E584
	public static Bounds GetGreaterRoundedBoundsXZ(Bounds bounds, Vector2Int step)
	{
		Bounds bounds2 = bounds;
		bounds2.max = VisualConsts.GetRoundedPosXZ(bounds2.max, step);
		bounds2.min = VisualConsts.GetRoundedPosXZ(bounds2.min, step);
		float num = 0.02f * (float)step.x;
		float num2 = 0.025f * (float)step.y;
		if (bounds2.min.x - bounds.min.x > 0.001f)
		{
			bounds2.min += Vector3.left * num;
		}
		if (bounds2.min.z - bounds.min.z > 0.001f)
		{
			bounds2.min += Vector3.back * num2;
		}
		if (bounds2.max.x - bounds.max.x < -0.001f)
		{
			bounds2.max += Vector3.right * num;
		}
		if (bounds2.max.z - bounds.max.z < -0.001f)
		{
			bounds2.max += Vector3.forward * num2;
		}
		return bounds2;
	}

	// Token: 0x04001EA8 RID: 7848
	public const int GRAPHICAL_PIXEL_SIZE = 2;

	// Token: 0x04001EA9 RID: 7849
	public const float CAMERA_ANGLE = 0.6435011f;

	// Token: 0x04001EAA RID: 7850
	public const float Y_TO_Z = 0.75f;

	// Token: 0x04001EAB RID: 7851
	public const float Y_SCALE = 1.666667f;

	// Token: 0x04001EAC RID: 7852
	public const float Z_SCALE = 1.25f;

	// Token: 0x04001EAD RID: 7853
	public const float X_STEP = 0.01f;

	// Token: 0x04001EAE RID: 7854
	public const float Y_STEP = 0.01666667f;

	// Token: 0x04001EAF RID: 7855
	public const float Z_STEP = 0.0125f;

	// Token: 0x04001EB0 RID: 7856
	public const int GRID_SIZE = 48;

	// Token: 0x04001EB1 RID: 7857
	public const int Y_TERRAIN_GRID_SIZE = 36;

	// Token: 0x04001EB2 RID: 7858
	public const float BOUNDS_EPSILON = 0.001f;

	// Token: 0x04001EB3 RID: 7859
	public const float LAYER_Y_MICRO_OFFSET = 0.0001f;

	// Token: 0x04001EB4 RID: 7860
	public const float FIGHT_DECAL_LAYER_Y_OFFSET = 0.002f;

	// Token: 0x04001EB5 RID: 7861
	public const int FIGHT_BLOOD_LAYER_MIN = 12;

	// Token: 0x04001EB6 RID: 7862
	public const int FIGHT_BLOOD_LAYER_MAX = 39;

	// Token: 0x04001EB7 RID: 7863
	public const int FIGHT_GUTS_LAYER_MIN = 40;

	// Token: 0x04001EB8 RID: 7864
	public const int FIGHT_GUTS_LAYER_MAX = 69;

	// Token: 0x04001EB9 RID: 7865
	public const int FIGHT_GORE_LAYER_MIN = 70;

	// Token: 0x04001EBA RID: 7866
	public const int FIGHT_GORE_LAYER_MAX = 99;
}
