using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B61 RID: 2913
	[Name("On Zombie Went Back To Sand", 0)]
	[Category("Game/Zombie")]
	public class Flow_OnZombieWentBackToSand : GKCustomFlowNode
	{
		// Token: 0x06004D10 RID: 19728 RVA: 0x0016B324 File Offset: 0x00169524
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x0016B374 File Offset: 0x00169574
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

		// Token: 0x04003DE7 RID: 15847
		private FlowInput @in;

		// Token: 0x04003DE8 RID: 15848
		private FlowOutput @out;
	}
}
