using System;
using UnityEngine;

// Token: 0x020003CF RID: 975
[Serializable]
public class SetActiveAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A02 RID: 6658 RVA: 0x0007A0A0 File Offset: 0x000782A0
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (this.target == null)
		{
			return;
		}
		bool flag = (this.invertLogic ? (!conditionMet) : conditionMet);
		this.target.SetActive(flag);
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x0007A0D8 File Offset: 0x000782D8
	public override void Reset(ConditionalDrawerContext context)
	{
		if (this.target != null)
		{
			this.target.SetActive(this.defaultActiveState);
		}
	}

	// Token: 0x0400194D RID: 6477
	[Tooltip("The target GameObject to activate/deactivate")]
	public GameObject target;

	// Token: 0x0400194E RID: 6478
	[Tooltip("If true, the logic is inverted (active when condition is false)")]
	public bool invertLogic;

	// Token: 0x0400194F RID: 6479
	[Tooltip("Default active state when reset")]
	public bool defaultActiveState = true;
}
