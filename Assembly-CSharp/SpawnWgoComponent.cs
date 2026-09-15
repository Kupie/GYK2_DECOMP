using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BF RID: 959
[Serializable]
public class SpawnWgoComponent : IComponent
{
	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x060019B5 RID: 6581 RVA: 0x000792E3 File Offset: 0x000774E3
	// (set) Token: 0x060019B6 RID: 6582 RVA: 0x000792EB File Offset: 0x000774EB
	public List<SpawnStage> SpawnStages
	{
		get
		{
			return this.spawnStages;
		}
		set
		{
			this.spawnStages = value;
		}
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000792F4 File Offset: 0x000774F4
	public SpawnStage FindStageById(string id)
	{
		for (int i = 0; i < this.spawnStages.Count; i++)
		{
			SpawnStage spawnStage = this.spawnStages[i];
			for (int j = 0; j < spawnStage.variations.Count; j++)
			{
				if (this.spawnStages[i].variations[j].wgoPartId.StartsWith(id))
				{
					return spawnStage;
				}
			}
		}
		return null;
	}

	// Token: 0x040018FC RID: 6396
	[SerializeField]
	private List<SpawnStage> spawnStages = new List<SpawnStage>();
}
