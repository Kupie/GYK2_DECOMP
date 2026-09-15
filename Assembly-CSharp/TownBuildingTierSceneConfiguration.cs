using System;
using UnityEngine;

// Token: 0x020003C5 RID: 965
[Serializable]
public class TownBuildingTierSceneConfiguration
{
	// Token: 0x04001919 RID: 6425
	[Tooltip("Палатка")]
	public TownBuildingObjectConfiguration tent = new TownBuildingObjectConfiguration();

	// Token: 0x0400191A RID: 6426
	[Tooltip("Вывеска")]
	public TownBuildingObjectConfiguration sign = new TownBuildingObjectConfiguration();

	// Token: 0x0400191B RID: 6427
	[Tooltip("Двор")]
	public TownBuildingObjectConfiguration yard = new TownBuildingObjectConfiguration();

	// Token: 0x0400191C RID: 6428
	[Tooltip("Декор №1")]
	public TownBuildingObjectConfiguration decor1 = new TownBuildingObjectConfiguration();

	// Token: 0x0400191D RID: 6429
	[Tooltip("Декор №2")]
	public TownBuildingObjectConfiguration decor2 = new TownBuildingObjectConfiguration();

	// Token: 0x0400191E RID: 6430
	[Tooltip("Декор №3")]
	public TownBuildingObjectConfiguration decor3 = new TownBuildingObjectConfiguration();
}
