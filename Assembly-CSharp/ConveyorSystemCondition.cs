using System;
using UnityEngine;

// Token: 0x020003DD RID: 989
[Serializable]
public class ConveyorSystemCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06001A44 RID: 6724 RVA: 0x0007B2B4 File Offset: 0x000794B4
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ConveyorSystemChanged;
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0007B2BC File Offset: 0x000794BC
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		bool hasEnoughPower = MainGame.Instance.conveyorSystem.HasEnoughPower;
		return this.conditionType == ConveyorSystemConditionType.HasEnoughPower && hasEnoughPower;
	}

	// Token: 0x04001973 RID: 6515
	[Tooltip("The type of conveyor system condition to check")]
	public ConveyorSystemConditionType conditionType;
}
