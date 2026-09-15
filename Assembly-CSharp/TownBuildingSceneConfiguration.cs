using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003C4 RID: 964
[Serializable]
public class TownBuildingSceneConfiguration
{
	// Token: 0x04001915 RID: 6421
	[Tooltip("Место где стоит за прилавком персонаж")]
	public string gdPointTent;

	// Token: 0x04001916 RID: 6422
	[Tooltip("Место куда идет персонаж от прилавка когда надо домой")]
	public string gdPointHomeOutside;

	// Token: 0x04001917 RID: 6423
	[Tooltip("Место куда телепортируется персонаж когда дошел до gdPointHomeOutside")]
	public string gdPointHomeInside;

	// Token: 0x04001918 RID: 6424
	public List<TownBuildingTierSceneConfiguration> tierDataList = new List<TownBuildingTierSceneConfiguration>();
}
