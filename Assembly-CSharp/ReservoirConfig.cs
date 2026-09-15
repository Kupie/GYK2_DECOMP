using System;
using UnityEngine;

// Token: 0x02000413 RID: 1043
[CreateAssetMenu(fileName = "ReservoirConfig", menuName = "ScriptableObjects/ReservoirConfig")]
public class ReservoirConfig : ScriptableObject
{
	// Token: 0x04001A5D RID: 6749
	[SerializeField]
	public float fishSpawnHorOffset = 1.75f;

	// Token: 0x04001A5E RID: 6750
	[SerializeField]
	public float fishSpawnVertOffset = 0.2f;

	// Token: 0x04001A5F RID: 6751
	[SerializeField]
	public float progressRange = 0.7f;
}
