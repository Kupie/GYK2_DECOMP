using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200032E RID: 814
[CreateAssetMenu(fileName = "FightingGlobal", menuName = "GK2/Fighting/Global")]
public class FightingGlobal : ScriptableObject
{
	// Token: 0x0400162B RID: 5675
	[FormerlySerializedAs("defaultZombieAIAgentSettings")]
	public FightingAgentSettings defaultFightingAgentSettings;

	// Token: 0x0400162C RID: 5676
	[Range(0f, 1f)]
	public float bloodDecalSpawnProbability = 0.5f;

	// Token: 0x0400162D RID: 5677
	[Range(0f, 1f)]
	public float bonesDecalSpawnProbability = 1f;

	// Token: 0x0400162E RID: 5678
	[Range(0f, 1f)]
	public float gutsDecalSpawnProbability = 1f;

	// Token: 0x0400162F RID: 5679
	public float decalsLifeTime = 10f;

	// Token: 0x04001630 RID: 5680
	[Range(0f, 1f)]
	public float decalOverlapCullFactor = 0.35f;

	// Token: 0x04001631 RID: 5681
	[Min(0f)]
	public int maxActiveDecals = 256;
}
