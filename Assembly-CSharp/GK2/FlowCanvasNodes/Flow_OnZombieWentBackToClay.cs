using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B56 RID: 2902
	[Name("On Zombie Went Back To Clay", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentBackToClay : GKCustomFlowNode
	{
		// Token: 0x06004CED RID: 19693 RVA: 0x0016A5CC File Offset: 0x001687CC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x0016A61C File Offset: 0x0016881C
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				zombieWgoData.CaretakerPortableItem = Item.Empty;
				zombieWgoData.AttachedWgoData.SetGameRes("stuff_disabled", 0);
				zombieWgoData.AttachedWgoData.FireEvent("craft_finish");
				zombieWgoData.MovableDirection = Direction.Down.ConvertToVector2XZ();
				zombieWgoData.AttachedWgoData.TryAddWorkerCutUnit(zombieWgoData);
			}
			else
			{
				Debug.LogError("Object is not zombie!!!");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DD0 RID: 15824
		private FlowInput @in;

		// Token: 0x04003DD1 RID: 15825
		private FlowOutput @out;
	}
}
