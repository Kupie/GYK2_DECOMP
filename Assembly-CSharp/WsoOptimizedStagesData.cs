using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
[Serializable]
public class WsoOptimizedStagesData : WsoComponentDataBase
{
	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06002788 RID: 10120 RVA: 0x000B9752 File Offset: 0x000B7952
	public IReadOnlyList<WsoOptimizedStageEntry> Stages
	{
		get
		{
			return this.stages;
		}
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06002789 RID: 10121 RVA: 0x000B975A File Offset: 0x000B795A
	public bool IsOptimized
	{
		get
		{
			return this.stages.Count > 0;
		}
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000B976A File Offset: 0x000B796A
	public WsoOptimizedStagesData()
	{
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000B977D File Offset: 0x000B797D
	public WsoOptimizedStagesData(List<WsoOptimizedStageEntry> entries)
	{
		this.stages = entries ?? new List<WsoOptimizedStageEntry>();
	}

	// Token: 0x040021A4 RID: 8612
	[SerializeField]
	private List<WsoOptimizedStageEntry> stages = new List<WsoOptimizedStageEntry>();
}
