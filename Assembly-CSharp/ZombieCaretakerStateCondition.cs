using System;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
[Serializable]
public class ZombieCaretakerStateCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06001A8B RID: 6795 RVA: 0x0007BD67 File Offset: 0x00079F67
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.CaretakerStateChanged;
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x0007BD70 File Offset: 0x00079F70
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		CaretakerStateConditionType caretakerStateConditionType = this.conditionType;
		bool flag;
		if (caretakerStateConditionType != CaretakerStateConditionType.IsOnStation)
		{
			flag = caretakerStateConditionType == CaretakerStateConditionType.IsNotOnStation && !context.IsZombieCareTakerOnStation;
		}
		else
		{
			flag = context.IsZombieCareTakerOnStation;
		}
		return flag;
	}

	// Token: 0x040019B6 RID: 6582
	[Tooltip("Caretaker state condition")]
	public CaretakerStateConditionType conditionType;
}
