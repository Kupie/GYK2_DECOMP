using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000230 RID: 560
[CreateAssetMenu(menuName = "GK2/GameResDisplayConfig", fileName = "GameResDisplayConfig")]
public class GameResDisplayConfig : LazySingletonSO<GameResDisplayConfig>
{
	// Token: 0x06000D37 RID: 3383 RVA: 0x000441A0 File Offset: 0x000423A0
	public static GameResIconConfig GetConfigForRes(string res, GameResIconType iconType)
	{
		if (iconType == null)
		{
			return null;
		}
		foreach (GameResDisplayData gameResDisplayData in LazySingletonSO<GameResDisplayConfig>.Instance.datas)
		{
			if (gameResDisplayData.atomType == res)
			{
				foreach (GameResIconConfig gameResIconConfig in gameResDisplayData.iconConfigs)
				{
					if (gameResIconConfig.iconType.value == iconType.value)
					{
						return gameResIconConfig;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x0400105D RID: 4189
	[SerializeField]
	private List<GameResDisplayData> datas = new List<GameResDisplayData>();
}
