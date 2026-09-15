using System;
using System.Collections.Generic;

// Token: 0x02000721 RID: 1825
public class WgoDelayedEventsSystem : ICustomUpdatable
{
	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000E5131 File Offset: 0x000E3331
	private static WgoDelayedEventSystemData WgoDelayedEventSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.wgoDelayedEventSystemData;
		}
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x000E5144 File Offset: 0x000E3344
	public void CustomUpdate(float deltaTime)
	{
		MainGame instance = MainGame.Instance;
		bool flag;
		if (instance == null)
		{
			flag = null != null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				flag = null != null;
			}
			else
			{
				WgoDelayedEventSystemData wgoDelayedEventSystemData = gameSave.wgoDelayedEventSystemData;
				flag = ((wgoDelayedEventSystemData != null) ? wgoDelayedEventSystemData.wgoUniqueIds : null) != null;
			}
		}
		if (!flag)
		{
			return;
		}
		List<SGuid> wgoUniqueIds = WgoDelayedEventsSystem.WgoDelayedEventSystemData.wgoUniqueIds;
		for (int i = wgoUniqueIds.Count - 1; i >= 0; i--)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(wgoUniqueIds[i]);
			if (wgoData != null)
			{
				wgoData.UpdateDelayedEvents(deltaTime);
			}
		}
	}
}
