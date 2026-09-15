using System;
using UnityEngine;

// Token: 0x02000B0B RID: 2827
public static class TownUtils
{
	// Token: 0x06004B4D RID: 19277 RVA: 0x00163C7C File Offset: 0x00161E7C
	public static void RepairWholeTown()
	{
		foreach (WsoData wsoData in MainGame.Instance.GameSave.worldData.GetGameSceneDataById("RuinedTemple").wsoDataList)
		{
			TownUtils.RepairHouse(wsoData, -1);
		}
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x00163CE8 File Offset: 0x00161EE8
	public static int RepairHouse(WsoData wsoData, int stageIdx = -1)
	{
		if (!wsoData.Definition.ReplacementConfig)
		{
			return 0;
		}
		WsoRepairablePartData componentData = wsoData.GetComponentData<WsoRepairablePartData>();
		if (componentData == null)
		{
			Debug.LogWarning("[Wso] Cannot repair - no WsoRepairablePartData component on " + wsoData.id);
			return 0;
		}
		if (wsoData.Definition.ReplacementConfig == null)
		{
			Debug.LogWarning(string.Format("[Wso] Cannot repair - no replacement config found for {0}", wsoData));
			return 0;
		}
		int num = ((stageIdx == -1) ? componentData.RepairAllStages(wsoData.Definition.ReplacementConfig) : componentData.RepairStage(stageIdx, wsoData.Definition.ReplacementConfig));
		if (num > 0)
		{
			wsoData.NotifyRepairStateChanged();
			Debug.Log(string.Format("[Wso] Repaired {0} parts of {1}", num, wsoData.id));
		}
		if (componentData.AreAllStagesRepaired() && !componentData.isTownQualityAdded)
		{
			componentData.isTownQualityAdded = true;
			MainGame instance = MainGame.Instance;
			bool flag;
			if (instance == null)
			{
				flag = null != null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				flag = ((gameSave != null) ? gameSave.townSystem : null) != null;
			}
			if (flag)
			{
				MainGame.Instance.GameSave.townSystem.Quality += wsoData.Definition.townQuality;
			}
		}
		return num;
	}
}
