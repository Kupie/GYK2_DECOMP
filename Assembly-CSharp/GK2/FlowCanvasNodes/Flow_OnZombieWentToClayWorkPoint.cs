using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B57 RID: 2903
	[Name("On Zombie Went To Clay Work Point", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentToClayWorkPoint : GKCustomFlowNode
	{
		// Token: 0x06004CF0 RID: 19696 RVA: 0x0016A694 File Offset: 0x00168894
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0016A6E4 File Offset: 0x001688E4
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				Debug.Log("Zombie went to clay work point");
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_clay", 0f);
				zombieWgoData.SetCustomAnimationState(global::AnimationState.ToolShovel);
				zombieWgoData.direction.Value = Direction.Down.ConvertToVector2XZ();
				zombieWgoData.AttachedWgoData.TryAddWorkerCutUnit(zombieWgoData);
			}
			else
			{
				Debug.LogError("Object is not zombie!!!");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DD2 RID: 15826
		private FlowInput @in;

		// Token: 0x04003DD3 RID: 15827
		private FlowOutput @out;
	}
}
