using System;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
[Serializable]
public class TemporaryObjectCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06001A80 RID: 6784 RVA: 0x00028294 File Offset: 0x00026494
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.None;
		}
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x0007BB38 File Offset: 0x00079D38
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		bool flag = context.WgoData != null && context.WgoData.isTempObject;
		TemporaryObjectConditionType temporaryObjectConditionType = this.conditionType;
		bool flag2;
		if (temporaryObjectConditionType != TemporaryObjectConditionType.IsTemporary)
		{
			flag2 = temporaryObjectConditionType == TemporaryObjectConditionType.IsNotTemporary && !flag;
		}
		else
		{
			flag2 = flag;
		}
		return flag2;
	}

	// Token: 0x040019A8 RID: 6568
	[Tooltip("Temporary object condition")]
	public TemporaryObjectConditionType conditionType;
}
