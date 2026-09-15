using System;
using System.Collections.Generic;

// Token: 0x0200043C RID: 1084
[Serializable]
public class MovementSystemData
{
	// Token: 0x06001CA1 RID: 7329 RVA: 0x00085B7C File Offset: 0x00083D7C
	public void RestoreMovingObjects(WorldData worldData)
	{
		this.movingObjects = new List<MovementComponent>();
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
					MovementComponent movementComponent = ((wgoData != null) ? wgoData.MovementComponent : null);
					if (movementComponent != null && movementComponent.ShouldRegisterInMovementSystem())
					{
						this.movingObjects.Add(movementComponent);
					}
				}
			}
		}
	}

	// Token: 0x04001ABC RID: 6844
	[NonSerialized]
	public List<MovementComponent> movingObjects = new List<MovementComponent>();
}
