using System;
using Unity.Collections;

// Token: 0x020006FE RID: 1790
public class ChunkVisibilityJobBuffer
{
	// Token: 0x06002F4B RID: 12107 RVA: 0x000E312D File Offset: 0x000E132D
	public void EnsureCapacity(int required)
	{
		if (this.capacity >= required)
		{
			return;
		}
		this.Dispose();
		this.capacity = required;
		this.data = new NativeArray<BurstableBounds>(this.capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.results = new NativeArray<byte>(this.capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x000E316C File Offset: 0x000E136C
	public void Dispose()
	{
		if (this.data.IsCreated)
		{
			this.data.Dispose();
		}
		if (this.results.IsCreated)
		{
			this.results.Dispose();
		}
		this.capacity = 0;
	}

	// Token: 0x04002637 RID: 9783
	public NativeArray<BurstableBounds> data;

	// Token: 0x04002638 RID: 9784
	public NativeArray<byte> results;

	// Token: 0x04002639 RID: 9785
	private int capacity;
}
