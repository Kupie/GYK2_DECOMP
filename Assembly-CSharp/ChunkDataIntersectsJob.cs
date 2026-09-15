using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// Token: 0x02000150 RID: 336
[BurstCompile]
public struct ChunkDataIntersectsJob : IJobParallelFor
{
	// Token: 0x06000802 RID: 2050 RVA: 0x00027570 File Offset: 0x00025770
	public void Execute(int index)
	{
		this.results[index] = this.sourceData.Intersects(this.objectsData[index]);
	}

	// Token: 0x040009FB RID: 2555
	public BurstableBounds sourceData;

	// Token: 0x040009FC RID: 2556
	public NativeArray<BurstableBounds> objectsData;

	// Token: 0x040009FD RID: 2557
	public NativeArray<bool> results;
}
