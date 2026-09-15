using System;
using System.Collections.Generic;

// Token: 0x0200049B RID: 1179
[Serializable]
public class ConveyorSystemData
{
	// Token: 0x06001F5F RID: 8031 RVA: 0x00094932 File Offset: 0x00092B32
	public void PrepareForGame(WorldData worldData)
	{
		this.RestoreConveyorComponents(worldData);
		this.graphStartElements = new List<ConveyorComponent>();
		this.graphEndElements = new List<ConveyorComponent>();
		this.workbenchElements = new List<ConveyorWorkbenchComponent>();
		this.splitterElements = new List<ConveyorSplitterComponent>();
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x00094968 File Offset: 0x00092B68
	public void RestoreConveyorComponents(WorldData worldData)
	{
		this.conveyorComponents = new List<ConveyorComponent>();
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
					ConveyorWgoData conveyorWgoData = list2[j] as ConveyorWgoData;
					if (conveyorWgoData != null)
					{
						ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
						if (conveyorComponent != null)
						{
							this.conveyorComponents.Add(conveyorComponent);
						}
					}
				}
			}
		}
	}

	// Token: 0x04001C19 RID: 7193
	public bool isInitialized;

	// Token: 0x04001C1A RID: 7194
	public bool isPaused;

	// Token: 0x04001C1B RID: 7195
	[NonSerialized]
	public List<ConveyorComponent> conveyorComponents = new List<ConveyorComponent>();

	// Token: 0x04001C1C RID: 7196
	public List<ZombieCraftActivity> zombieCraftActivities = new List<ZombieCraftActivity>();

	// Token: 0x04001C1D RID: 7197
	public float timer;

	// Token: 0x04001C1E RID: 7198
	[NonSerialized]
	public List<ConveyorComponent> graphStartElements = new List<ConveyorComponent>();

	// Token: 0x04001C1F RID: 7199
	[NonSerialized]
	public List<ConveyorComponent> graphEndElements = new List<ConveyorComponent>();

	// Token: 0x04001C20 RID: 7200
	[NonSerialized]
	public List<ConveyorWorkbenchComponent> workbenchElements = new List<ConveyorWorkbenchComponent>();

	// Token: 0x04001C21 RID: 7201
	[NonSerialized]
	public List<ConveyorSplitterComponent> splitterElements = new List<ConveyorSplitterComponent>();
}
