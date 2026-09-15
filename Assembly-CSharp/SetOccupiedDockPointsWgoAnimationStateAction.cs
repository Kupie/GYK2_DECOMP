using System;
using UnityEngine;

// Token: 0x020003D3 RID: 979
[Serializable]
public class SetOccupiedDockPointsWgoAnimationStateAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A0C RID: 6668 RVA: 0x0007A2C8 File Offset: 0x000784C8
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (context.WgoData == null)
		{
			return;
		}
		global::AnimationState animationState = (conditionMet ? this.animationState : global::AnimationState.Idle);
		foreach (DockPointData dockPointData in context.WgoData.MainWgoPartData.DockPointDataList)
		{
			if (dockPointData.IsOccupied)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(dockPointData.OccupiedBy);
				if (wgoData != null)
				{
					wgoData.SetStateToAnimator(animationState);
				}
			}
		}
	}

	// Token: 0x0400195C RID: 6492
	[Tooltip("Animation state set")]
	public global::AnimationState animationState;
}
