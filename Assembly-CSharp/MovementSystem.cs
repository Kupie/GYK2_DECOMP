using System;

// Token: 0x0200043B RID: 1083
[Serializable]
public class MovementSystem : ICustomUpdatable
{
	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00085AF0 File Offset: 0x00083CF0
	private static MovementSystemData MovementSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.movementSystemData;
		}
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x00085B04 File Offset: 0x00083D04
	public void CustomUpdate(float deltaTime)
	{
		for (int i = 0; i < MovementSystem.MovementSystemData.movingObjects.Count; i++)
		{
			MovementSystem.MovementSystemData.movingObjects[i].Update(deltaTime);
		}
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x00085B41 File Offset: 0x00083D41
	public void AddMovingObject(MovementComponent movingObject)
	{
		if (MovementSystem.MovementSystemData.movingObjects.Contains(movingObject))
		{
			return;
		}
		MovementSystem.MovementSystemData.movingObjects.Add(movingObject);
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x00085B66 File Offset: 0x00083D66
	public void RemoveMovingObject(MovementComponent movingObject)
	{
		MovementSystem.MovementSystemData.movingObjects.Remove(movingObject);
	}
}
