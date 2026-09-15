using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B62 RID: 2914
	[Name("On Zombie Went To Sand Work Point", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentToSandWorkPoint : GKCustomFlowNode
	{
		// Token: 0x06004D13 RID: 19731 RVA: 0x0016B3EC File Offset: 0x001695EC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x0016B43C File Offset: 0x0016963C
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				Debug.Log("Zombie went to sand work point");
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_sand", 0f);
				zombieWgoData.SetCustomAnimationState(global::AnimationState.ToolShovel);
				zombieWgoData.direction.Value = Direction.Left.ConvertToVector2XZ();
				zombieWgoData.AttachedWgoData.TryAddWorkerCutUnit(zombieWgoData);
			}
			else
			{
				Debug.LogError("Object is not zombie!!!");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DE9 RID: 15849
		private FlowInput @in;

		// Token: 0x04003DEA RID: 15850
		private FlowOutput @out;
	}
}
