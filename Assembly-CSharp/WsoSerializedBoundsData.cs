using System;
using UnityEngine;

// Token: 0x020005DA RID: 1498
public class WsoSerializedBoundsData : WsoComponentDataBase
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06002798 RID: 10136 RVA: 0x000B9A28 File Offset: 0x000B7C28
	// (set) Token: 0x06002799 RID: 10137 RVA: 0x000B9A30 File Offset: 0x000B7C30
	public ChunkBoundsPair Bounds
	{
		get
		{
			return this.bounds;
		}
		set
		{
			this.bounds = value;
		}
	}

	// Token: 0x040021A7 RID: 8615
	[SerializeField]
	private ChunkBoundsPair bounds;
}
