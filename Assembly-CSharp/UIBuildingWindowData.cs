using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000907 RID: 2311
public class UIBuildingWindowData : LazyWidgetDataBase
{
	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x00121DBF File Offset: 0x0011FFBF
	// (set) Token: 0x06003C9A RID: 15514 RVA: 0x00121DC7 File Offset: 0x0011FFC7
	public Wgo AssignedWgo { get; private set; }

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06003C9B RID: 15515 RVA: 0x00121DD0 File Offset: 0x0011FFD0
	// (set) Token: 0x06003C9C RID: 15516 RVA: 0x00121DD8 File Offset: 0x0011FFD8
	public Action<BuildData, List<NeedItemData>> OnBuildPressed { get; private set; }

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x06003C9D RID: 15517 RVA: 0x00121DE1 File Offset: 0x0011FFE1
	// (set) Token: 0x06003C9E RID: 15518 RVA: 0x00121DE9 File Offset: 0x0011FFE9
	public Func<BuildData, List<NeedItemData>, bool> CanBuild { get; private set; }

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x06003C9F RID: 15519 RVA: 0x00121DF2 File Offset: 0x0011FFF2
	// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x00121DFA File Offset: 0x0011FFFA
	public PlayerData PlayerData { get; private set; }

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x00121E03 File Offset: 0x00120003
	// (set) Token: 0x06003CA2 RID: 15522 RVA: 0x00121E0B File Offset: 0x0012000B
	public List<Inventory> AdditionalInventories { get; private set; }

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x00121E14 File Offset: 0x00120014
	// (set) Token: 0x06003CA4 RID: 15524 RVA: 0x00121E1C File Offset: 0x0012001C
	public Dictionary<string, List<BuildData>> TabSortedBuilds { get; private set; }

	// Token: 0x06003CA5 RID: 15525 RVA: 0x00121E28 File Offset: 0x00120028
	public UIBuildingWindowData(Wgo wgo, PlayerData playerData, List<BuildData> buildsData, Action<BuildData, List<NeedItemData>> onBuildPressed, Func<BuildData, List<NeedItemData>, bool> canBuild, List<Inventory> additionalInventories = null)
	{
		this.AssignedWgo = wgo;
		this.PlayerData = playerData;
		this.OnBuildPressed = onBuildPressed;
		this.CanBuild = canBuild;
		this.AdditionalInventories = additionalInventories;
		this.TabSortedBuilds = new Dictionary<string, List<BuildData>>();
		for (int i = 0; i < buildsData.Count; i++)
		{
			BuildData buildData = buildsData[i];
			if (buildData != null)
			{
				string text = (string.IsNullOrEmpty(buildData.Definition.tab) ? "tab_building_default" : buildData.Definition.tab);
				List<BuildData> list;
				if (this.TabSortedBuilds.TryGetValue(text, out list))
				{
					list.Add(buildData);
				}
				else
				{
					this.TabSortedBuilds.Add(text, new List<BuildData> { buildData });
				}
			}
		}
		if (this.AssignedWgo != null && this.AssignedWgo.Data.Definition.interactionType == WGODef.InteractionType.FightBuilder && LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.ActiveFight)
		{
			return;
		}
		foreach (KeyValuePair<string, List<BuildData>> keyValuePair in this.TabSortedBuilds)
		{
			keyValuePair.Value.Add(BuildData.GetDataForRemove());
		}
	}
}
