using System;
using System.Collections.Generic;

// Token: 0x02000274 RID: 628
[Serializable]
public class CraftSystemData
{
	// Token: 0x0600104F RID: 4175 RVA: 0x00052670 File Offset: 0x00050870
	public void RestoreActiveCrafts(WorldData worldData)
	{
		this.activeCrafts = new List<CraftComponent>();
		List<GameSceneData> list = ((worldData != null) ? worldData.gameSceneDataList : null);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneData gameSceneData = list[i];
			List<WgoData> list2 = ((gameSceneData != null) ? gameSceneData.wgoDataList : null);
			if (list2 != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					WgoData wgoData = list2[j];
					CraftComponent craftComponent = ((wgoData != null) ? wgoData.CraftComponent : null);
					if (craftComponent != null && craftComponent.ShouldRegisterInCraftSystem())
					{
						this.activeCrafts.Add(craftComponent);
					}
				}
			}
		}
	}

	// Token: 0x040012A7 RID: 4775
	[NonSerialized]
	public List<CraftComponent> activeCrafts = new List<CraftComponent>();

	// Token: 0x040012A8 RID: 4776
	public List<ZombieCraftActivity> zombieCraftActivities = new List<ZombieCraftActivity>();

	// Token: 0x040012A9 RID: 4777
	public List<ZombieHPActivity> zombieHPActivities = new List<ZombieHPActivity>();
}
