using System;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class WorldFxColliderTriggerComponent : ColliderTriggerComponentBase
{
	// Token: 0x06000F5F RID: 3935 RVA: 0x0004EDE2 File Offset: 0x0004CFE2
	private void Awake()
	{
		this.onEnter.PlayTarget = this.playTarget;
		this.onExit.PlayTarget = this.playTarget;
		base.Init(this.onEnter, this.onExit);
	}

	// Token: 0x04001240 RID: 4672
	[SerializeField]
	private Transform playTarget;

	// Token: 0x04001241 RID: 4673
	[SerializeField]
	private WorldFxColliderTriggerData onEnter;

	// Token: 0x04001242 RID: 4674
	[SerializeField]
	private WorldFxColliderTriggerData onExit;
}
