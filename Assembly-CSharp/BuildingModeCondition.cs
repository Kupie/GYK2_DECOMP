using System;
using UnityEngine;

// Token: 0x020003D9 RID: 985
[Serializable]
public class BuildingModeCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06001A3B RID: 6715 RVA: 0x0007B187 File Offset: 0x00079387
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.BuildingModeChanged;
		}
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return true;
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x0007B190 File Offset: 0x00079390
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		BuildingModeConditionType buildingModeConditionType = this.conditionType;
		bool flag;
		if (buildingModeConditionType != BuildingModeConditionType.IsActive)
		{
			flag = buildingModeConditionType == BuildingModeConditionType.IsNotActive && !context.IsBuildingModeActive;
		}
		else
		{
			flag = context.IsBuildingModeActive;
		}
		return flag;
	}

	// Token: 0x04001969 RID: 6505
	[Tooltip("Building mode condition")]
	public BuildingModeConditionType conditionType;
}
