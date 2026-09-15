using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005DB RID: 1499
[Serializable]
public class WsoStageData
{
	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x0600279B RID: 10139 RVA: 0x000B9A41 File Offset: 0x000B7C41
	public int StageIndex
	{
		get
		{
			return this.stageIndex;
		}
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x0600279C RID: 10140 RVA: 0x000B9A49 File Offset: 0x000B7C49
	public bool IsRepaired
	{
		get
		{
			return this.isRepaired;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x0600279D RID: 10141 RVA: 0x000B9A51 File Offset: 0x000B7C51
	public IReadOnlyList<ConstructorPartStateData> PartsData
	{
		get
		{
			return this.partsData;
		}
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000B9A59 File Offset: 0x000B7C59
	public WsoStageData()
	{
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000B9A6C File Offset: 0x000B7C6C
	public WsoStageData(int stageIndex)
	{
		this.stageIndex = stageIndex;
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000B9A86 File Offset: 0x000B7C86
	public void AddPartData(ConstructorPartStateData partData)
	{
		this.partsData.Add(partData);
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000B9A94 File Offset: 0x000B7C94
	public int RepairParts(ConstructorPartReplacementConfig config)
	{
		if (config == null || this.isRepaired)
		{
			return 0;
		}
		int num = 0;
		foreach (ConstructorPartStateData constructorPartStateData in this.partsData)
		{
			if (!constructorPartStateData.IsRepaired && !constructorPartStateData.IsDeleted)
			{
				if (config.ShouldDeleteModel(constructorPartStateData.OriginalModelId))
				{
					constructorPartStateData.SetDeleted();
					num++;
				}
				else
				{
					constructorPartStateData.SetRepaired();
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.isRepaired = this.CheckAllPartsRepaired();
		}
		return num;
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000B9B3C File Offset: 0x000B7D3C
	public void ResetParts()
	{
		foreach (ConstructorPartStateData constructorPartStateData in this.partsData)
		{
			constructorPartStateData.Reset();
		}
		this.isRepaired = false;
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x000B9B94 File Offset: 0x000B7D94
	private bool CheckAllPartsRepaired()
	{
		foreach (ConstructorPartStateData constructorPartStateData in this.partsData)
		{
			if (!constructorPartStateData.IsRepaired && !constructorPartStateData.IsDeleted)
			{
				return false;
			}
		}
		return this.partsData.Count > 0;
	}

	// Token: 0x040021A8 RID: 8616
	[Tooltip("Index of this stage in the Wso (based on hierarchy order)")]
	[SerializeField]
	private int stageIndex;

	// Token: 0x040021A9 RID: 8617
	[Tooltip("Whether all parts in this stage have been repaired")]
	[SerializeField]
	private bool isRepaired;

	// Token: 0x040021AA RID: 8618
	[Tooltip("List of constructor part data within this stage")]
	[SerializeField]
	private List<ConstructorPartStateData> partsData = new List<ConstructorPartStateData>();
}
