using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D0 RID: 976
[Serializable]
public class SetActiveManyAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A05 RID: 6661 RVA: 0x0007A108 File Offset: 0x00078308
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (this.targets == null || this.targets.Count == 0)
		{
			return;
		}
		bool flag = (this.invertLogic ? (!conditionMet) : conditionMet);
		foreach (GameObject gameObject in this.targets)
		{
			gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x0007A180 File Offset: 0x00078380
	public override void Reset(ConditionalDrawerContext context)
	{
		if (this.targets != null && this.targets.Count > 0)
		{
			foreach (GameObject gameObject in this.targets)
			{
				gameObject.SetActive(this.defaultActiveState);
			}
		}
	}

	// Token: 0x04001950 RID: 6480
	[Tooltip("The list of GameObjects to activate/deactivate")]
	public List<GameObject> targets;

	// Token: 0x04001951 RID: 6481
	[Tooltip("If true, the logic is inverted (active when condition is false)")]
	public bool invertLogic;

	// Token: 0x04001952 RID: 6482
	[Tooltip("Default active state when reset")]
	public bool defaultActiveState = true;
}
