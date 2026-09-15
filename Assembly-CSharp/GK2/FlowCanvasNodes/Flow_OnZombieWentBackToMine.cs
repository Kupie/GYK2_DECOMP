using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5B RID: 2907
	[Name("On Zombie Went Back To Mine", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentBackToMine : GKCustomFlowNode
	{
		// Token: 0x06004CFD RID: 19709 RVA: 0x0016AC38 File Offset: 0x00168E38
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0016AC88 File Offset: 0x00168E88
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

		// Token: 0x04003DDA RID: 15834
		private FlowInput @in;

		// Token: 0x04003DDB RID: 15835
		private FlowOutput @out;
	}
}
