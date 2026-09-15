using System;
using UnityEngine;

// Token: 0x020003E4 RID: 996
[Serializable]
public class FightingAgentCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06001A53 RID: 6739 RVA: 0x0007B478 File Offset: 0x00079678
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.FightingAgentChanged;
		}
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x0007B47C File Offset: 0x0007967C
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		bool flag = false;
		FightingAgentConditionType fightingAgentConditionType = this.conditionType;
		if (fightingAgentConditionType != FightingAgentConditionType.IsActive)
		{
			if (fightingAgentConditionType == FightingAgentConditionType.IsNotActive)
			{
				flag = !context.IsFightingAgentActive;
			}
		}
		else
		{
			flag = context.IsFightingAgentActive;
		}
		Debug.Log(string.Format("Evaluate FightingAgentCondition [{0}]", flag));
		fightingAgentConditionType = this.conditionType;
		bool flag2;
		if (fightingAgentConditionType != FightingAgentConditionType.IsActive)
		{
			flag2 = fightingAgentConditionType == FightingAgentConditionType.IsNotActive && !context.IsFightingAgentActive;
		}
		else
		{
			flag2 = context.IsFightingAgentActive;
		}
		return flag2;
	}

	// Token: 0x04001984 RID: 6532
	[Tooltip("FightingAgent condition")]
	public FightingAgentConditionType conditionType;
}
