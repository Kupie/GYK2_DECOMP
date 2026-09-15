using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B67 RID: 2919
	[Name("On Zombie Went To Sawmill Work Point", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentToSawmillWorkPoint : GKCustomFlowNode
	{
		// Token: 0x06004D23 RID: 19747 RVA: 0x0016BB1C File Offset: 0x00169D1C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x0016BB6C File Offset: 0x00169D6C
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_sawmill", 0f);
				zombieWgoData.SetCustomAnimationState(global::AnimationState.ToolAxe);
				zombieWgoData.AttachedWgoData.TryAddWorkerCutUnit(zombieWgoData);
			}
			else
			{
				Debug.LogError("Object is not zombie!!!");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DF3 RID: 15859
		private FlowInput @in;

		// Token: 0x04003DF4 RID: 15860
		private FlowOutput @out;
	}
}
