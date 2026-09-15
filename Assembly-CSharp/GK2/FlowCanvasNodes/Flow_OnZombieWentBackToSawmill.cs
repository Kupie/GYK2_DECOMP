using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B66 RID: 2918
	[Name("On Zombie Went Back To Sawmill", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentBackToSawmill : GKCustomFlowNode
	{
		// Token: 0x06004D20 RID: 19744 RVA: 0x0016BA54 File Offset: 0x00169C54
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x0016BAA4 File Offset: 0x00169CA4
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

		// Token: 0x04003DF1 RID: 15857
		private FlowInput @in;

		// Token: 0x04003DF2 RID: 15858
		private FlowOutput @out;
	}
}
