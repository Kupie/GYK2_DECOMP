using System;
using JetBrains.Annotations;
using LazyBearTechnology;

// Token: 0x02000701 RID: 1793
public interface IChunkableObject
{
	// Token: 0x06002F4F RID: 12111
	BurstableBounds GetChunkableData();

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06002F50 RID: 12112
	// (set) Token: 0x06002F51 RID: 12113
	[CanBeNull]
	MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x06002F52 RID: 12114
	void UpdateChunkVisibility(bool isVisible);

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000E31A5 File Offset: 0x000E13A5
	bool IgnoreChunkVisibility
	{
		get
		{
			return this.IgnoreMultiFlag != null && this.IgnoreMultiFlag.ResultFlag;
		}
	}
}
