using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
[Serializable]
public class WsoRepairablePartData : WsoComponentDataBase
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x0600278C RID: 10124 RVA: 0x000B97A0 File Offset: 0x000B79A0
	public IReadOnlyList<WsoStageData> Stages
	{
		get
		{
			return this.stages;
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x000B97A8 File Offset: 0x000B79A8
	public void AddStage(WsoStageData stageData)
	{
		this.stages.Add(stageData);
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000B97B6 File Offset: 0x000B79B6
	public WsoStageData GetStage(int stageIndex)
	{
		if (stageIndex < 0 || stageIndex >= this.stages.Count)
		{
			return null;
		}
		return this.stages[stageIndex];
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000B97D8 File Offset: 0x000B79D8
	public int GetTotalPartsCount()
	{
		int num = 0;
		foreach (WsoStageData wsoStageData in this.stages)
		{
			num += wsoStageData.PartsData.Count;
		}
		return num;
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000B9838 File Offset: 0x000B7A38
	public bool AreAllStagesRepaired()
	{
		using (List<WsoStageData>.Enumerator enumerator = this.stages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsRepaired)
				{
					return false;
				}
			}
		}
		return this.stages.Count > 0;
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000B98A0 File Offset: 0x000B7AA0
	public bool IsAnyStageRepaired()
	{
		using (List<WsoStageData>.Enumerator enumerator = this.stages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsRepaired)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000B98FC File Offset: 0x000B7AFC
	public int RepairStage(int stageIndex, ConstructorPartReplacementConfig config)
	{
		if (config == null)
		{
			return 0;
		}
		WsoStageData stage = this.GetStage(stageIndex);
		if (stage == null)
		{
			Debug.LogWarning(string.Format("[WsoRepairablePartData] Stage index '{0}' not found", stageIndex));
			return 0;
		}
		return stage.RepairParts(config);
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000B9940 File Offset: 0x000B7B40
	public int RepairAllStages(ConstructorPartReplacementConfig config)
	{
		if (config == null)
		{
			return 0;
		}
		int num = 0;
		foreach (WsoStageData wsoStageData in this.stages)
		{
			num += wsoStageData.RepairParts(config);
		}
		return num;
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000B99A4 File Offset: 0x000B7BA4
	public void ResetStage(int stageIndex)
	{
		WsoStageData stage = this.GetStage(stageIndex);
		if (stage == null)
		{
			return;
		}
		stage.ResetParts();
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000B99B8 File Offset: 0x000B7BB8
	public void ResetAllStages()
	{
		foreach (WsoStageData wsoStageData in this.stages)
		{
			wsoStageData.ResetParts();
		}
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000B9A08 File Offset: 0x000B7C08
	public void ClearStages()
	{
		this.stages.Clear();
	}

	// Token: 0x040021A5 RID: 8613
	[Tooltip("List of repairable stages within this Wso")]
	[SerializeField]
	private List<WsoStageData> stages = new List<WsoStageData>();

	// Token: 0x040021A6 RID: 8614
	public bool isTownQualityAdded;
}
