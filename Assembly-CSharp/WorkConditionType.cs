using System;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public enum WorkConditionType
{
	// Token: 0x040019AE RID: 6574
	[Tooltip("Player is currently working on this WGO")]
	IsInWork,
	// Token: 0x040019AF RID: 6575
	[Tooltip("Player is not working on this WGO")]
	IsNotInWork,
	// Token: 0x040019B0 RID: 6576
	[Tooltip("WGO has an assigned worker")]
	HasWorker,
	// Token: 0x040019B1 RID: 6577
	[Tooltip("WGO does not have an assigned worker")]
	HasNoWorker
}
