using System;

// Token: 0x02000440 RID: 1088
public class WgoCustomDeathSystem : ICustomUpdatable
{
	// Token: 0x06001CB6 RID: 7350 RVA: 0x000865B8 File Offset: 0x000847B8
	public void CustomUpdate(float deltaTime)
	{
		for (int i = MainGame.Instance.GameSave.wgoCustomDeathSystemData.wgoCustomDeathData.Count - 1; i >= 0; i--)
		{
			WgoCustomDeathSystemData.WgoCustomDeathData wgoCustomDeathData = MainGame.Instance.GameSave.wgoCustomDeathSystemData.wgoCustomDeathData[i];
			wgoCustomDeathData.remainingTime -= deltaTime;
			if (wgoCustomDeathData.remainingTime <= 0f)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(wgoCustomDeathData.wgoUniqueId);
				if (wgoData == null)
				{
					MainGame.Instance.GameSave.wgoCustomDeathSystemData.wgoCustomDeathData.RemoveAt(i);
				}
				else
				{
					wgoData.TriggerCustomDeathMoment();
					MainGame.Instance.GameSave.wgoCustomDeathSystemData.wgoCustomDeathData.RemoveAt(i);
				}
			}
		}
	}
}
