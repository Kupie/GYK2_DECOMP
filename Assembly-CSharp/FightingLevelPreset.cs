using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200032F RID: 815
[CreateAssetMenu(fileName = "FightingLevelPreset", menuName = "GK2/Fighting/FightingLevelPreset")]
public class FightingLevelPreset : ScriptableObject
{
	// Token: 0x04001632 RID: 5682
	[Header("Lines")]
	public List<FightingLevelPreset.FightingLineData> lines = new List<FightingLevelPreset.FightingLineData>();

	// Token: 0x02000330 RID: 816
	[Serializable]
	public class FightingLineData
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00069B88 File Offset: 0x00067D88
		public float TotalTime
		{
			get
			{
				List<FightingPhaseData> list = this.phases;
				if (list == null)
				{
					return 0f;
				}
				return list.Sum((FightingPhaseData phase) => (float)phase.duration);
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x00069BBE File Offset: 0x00067DBE
		public int TotalNeededToSpawn
		{
			get
			{
				List<FightingPhaseData> list = this.phases;
				if (list == null)
				{
					return 0;
				}
				return list.Sum(delegate(FightingPhaseData phase)
				{
					if (!(phase is FightingPhaseSpawnEnemiesData))
					{
						return 0;
					}
					return (phase as FightingPhaseSpawnEnemiesData).TotalEnemiesToSpawn;
				});
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00069BF0 File Offset: 0x00067DF0
		public float SecondsAtEndOfLastSpawnEnemyPhase()
		{
			if (this.phases == null || this.phases.Count == 0)
			{
				return -1f;
			}
			float num = 0f;
			float num2 = -1f;
			foreach (FightingPhaseData fightingPhaseData in this.phases)
			{
				num += (float)fightingPhaseData.duration;
				if (fightingPhaseData is FightingPhaseSpawnEnemiesData)
				{
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x04001633 RID: 5683
		public bool isEnabled = true;

		// Token: 0x04001634 RID: 5684
		public int maxSpawnedCountAtOnce = 2;

		// Token: 0x04001635 RID: 5685
		[SerializeReference]
		public List<FightingPhaseData> phases = new List<FightingPhaseData>();
	}
}
