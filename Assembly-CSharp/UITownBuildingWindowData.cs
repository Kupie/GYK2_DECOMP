using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A6E RID: 2670
public class UITownBuildingWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x0600487A RID: 18554 RVA: 0x001575C6 File Offset: 0x001557C6
	// (set) Token: 0x0600487B RID: 18555 RVA: 0x001575CE File Offset: 0x001557CE
	public Wgo AssignedWgo { get; private set; }

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x0600487C RID: 18556 RVA: 0x001575D7 File Offset: 0x001557D7
	// (set) Token: 0x0600487D RID: 18557 RVA: 0x001575DF File Offset: 0x001557DF
	public List<TownBuildingDef> BuildsToDisplay { get; private set; }

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x0600487E RID: 18558 RVA: 0x001575E8 File Offset: 0x001557E8
	// (set) Token: 0x0600487F RID: 18559 RVA: 0x001575F0 File Offset: 0x001557F0
	public Action<TownBuildingDef, List<NeedItemData>> OnBuildPressed { get; private set; }

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x06004880 RID: 18560 RVA: 0x001575F9 File Offset: 0x001557F9
	// (set) Token: 0x06004881 RID: 18561 RVA: 0x00157601 File Offset: 0x00155801
	public PlayerData PlayerData { get; private set; }

	// Token: 0x06004882 RID: 18562 RVA: 0x0015760A File Offset: 0x0015580A
	public UITownBuildingWindowData(Wgo wgo, PlayerData playerData, List<TownBuildingDef> buildsData, Action<TownBuildingDef, List<NeedItemData>> onBuildPressed)
	{
		this.AssignedWgo = wgo;
		this.PlayerData = playerData;
		this.BuildsToDisplay = buildsData;
		this.OnBuildPressed = onBuildPressed;
	}
}
