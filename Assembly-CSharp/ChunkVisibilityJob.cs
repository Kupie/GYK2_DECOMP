using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x020006FD RID: 1789
[BurstCompile]
public struct ChunkVisibilityJob : IJobParallelFor
{
	// Token: 0x06002F48 RID: 12104 RVA: 0x000E2EF8 File Offset: 0x000E10F8
	public void Execute(int index)
	{
		float3 min = this.chunkableDataArray[index].Min;
		float3 max = this.chunkableDataArray[index].Max;
		this.ExpandBounds(ref min, ref max);
		if (this.IsInsidePlane(this.plane0, min, max, 0f) && this.IsInsidePlane(this.plane1, min, max, 0f) && this.IsInsidePlane(this.plane2, min, max, 0f) && this.IsInsidePlane(this.plane3, min, max, 0f) && this.IsInsidePlane(this.plane4, min, max, 0f) && this.IsInsidePlane(this.plane5, min, max, 0f))
		{
			this.visibilityStateResults[index] = 2;
			return;
		}
		this.visibilityStateResults[index] = ((this.IsInsidePlane(this.plane0, min, max, this.prewarmPlanePadding) && this.IsInsidePlane(this.plane1, min, max, this.prewarmPlanePadding) && this.IsInsidePlane(this.plane2, min, max, this.prewarmPlanePadding) && this.IsInsidePlane(this.plane3, min, max, this.prewarmPlanePadding) && this.IsInsidePlane(this.plane4, min, max, this.prewarmPlanePadding) && this.IsInsidePlane(this.plane5, min, max, this.prewarmPlanePadding)) ? 1 : 0);
	}

	// Token: 0x06002F49 RID: 12105 RVA: 0x000E3064 File Offset: 0x000E1264
	private void ExpandBounds(ref float3 min, ref float3 max)
	{
		if (this.expandFactor <= 1f)
		{
			return;
		}
		float num = max.x - min.x;
		float num2 = max.z - min.z;
		float num3 = num * (this.expandFactor - 1f) * 0.5f;
		float num4 = num2 * (this.expandFactor - 1f) * 0.5f;
		min.x -= num3;
		max.x += num3;
		min.z -= num4;
		max.z += num4;
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x000E30F0 File Offset: 0x000E12F0
	private bool IsInsidePlane(BurstablePlane plane, float3 min, float3 max, float padding)
	{
		float3 @float = math.select(min, max, plane.normal > 0f);
		return math.dot(plane.normal, @float) + plane.distance > -padding;
	}

	// Token: 0x0400262A RID: 9770
	private const byte OUT_OF_RANGE_STATE = 0;

	// Token: 0x0400262B RID: 9771
	private const byte PREWARM_STATE = 1;

	// Token: 0x0400262C RID: 9772
	private const byte VISIBLE_STATE = 2;

	// Token: 0x0400262D RID: 9773
	public NativeArray<BurstableBounds> chunkableDataArray;

	// Token: 0x0400262E RID: 9774
	public NativeArray<byte> visibilityStateResults;

	// Token: 0x0400262F RID: 9775
	public float expandFactor;

	// Token: 0x04002630 RID: 9776
	public float prewarmPlanePadding;

	// Token: 0x04002631 RID: 9777
	public BurstablePlane plane0;

	// Token: 0x04002632 RID: 9778
	public BurstablePlane plane1;

	// Token: 0x04002633 RID: 9779
	public BurstablePlane plane2;

	// Token: 0x04002634 RID: 9780
	public BurstablePlane plane3;

	// Token: 0x04002635 RID: 9781
	public BurstablePlane plane4;

	// Token: 0x04002636 RID: 9782
	public BurstablePlane plane5;
}
