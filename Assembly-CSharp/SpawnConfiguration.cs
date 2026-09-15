using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BC RID: 956
[CreateAssetMenu(fileName = "SpawnConfiguration", menuName = "GK2/SpawnConfiguration")]
public class SpawnConfiguration : ScriptableObject
{
	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x060019B0 RID: 6576 RVA: 0x00079246 File Offset: 0x00077446
	public List<SpawnStage> SpawnStages
	{
		get
		{
			return this.spawnStages;
		}
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00079250 File Offset: 0x00077450
	public int FindStageById(string id)
	{
		for (int i = 0; i < this.spawnStages.Count; i++)
		{
			SpawnStage spawnStage = this.spawnStages[i];
			for (int j = 0; j < spawnStage.variations.Count; j++)
			{
				if (this.spawnStages[i].variations[j].wgoPartId.StartsWith(id))
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x040018F7 RID: 6391
	[SerializeField]
	private List<SpawnStage> spawnStages = new List<SpawnStage>();
}
