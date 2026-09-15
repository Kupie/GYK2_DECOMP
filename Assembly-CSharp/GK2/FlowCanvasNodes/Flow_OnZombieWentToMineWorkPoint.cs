using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5C RID: 2908
	[Name("On Zombie Went To Mine Work Point", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentToMineWorkPoint : GKCustomFlowNode
	{
		// Token: 0x06004D00 RID: 19712 RVA: 0x0016AD00 File Offset: 0x00168F00
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0016AD50 File Offset: 0x00168F50
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_mine", 0f);
				zombieWgoData.SetCustomAnimationState(global::AnimationState.ToolPickaxe);
				zombieWgoData.AttachedWgoData.TryAddWorkerCutUnit(zombieWgoData);
			}
			else
			{
				Debug.LogError("Object is not zombie!!!");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DDC RID: 15836
		private FlowInput @in;

		// Token: 0x04003DDD RID: 15837
		private FlowOutput @out;
	}
}
