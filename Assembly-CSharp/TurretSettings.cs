using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000340 RID: 832
[CreateAssetMenu(fileName = "TurretSettings", menuName = "GK2/Fighting/Turret Settings")]
public class TurretSettings : ScriptableObject
{
	// Token: 0x0400166B RID: 5739
	public float scanInterval = 0.5f;

	// Token: 0x0400166C RID: 5740
	public List<float> shotSampleTimings = new List<float>();
}
