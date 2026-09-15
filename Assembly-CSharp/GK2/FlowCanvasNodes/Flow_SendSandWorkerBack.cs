using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B63 RID: 2915
	[Name("Send Sand Worker Back", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendSandWorkerBack : GKCustomFlowNode
	{
		// Token: 0x06004D16 RID: 19734 RVA: 0x0016B4C8 File Offset: 0x001696C8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0016B518 File Offset: 0x00169718
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				WgoData attachedWgoData = zombieWgoData.AttachedWgoData;
				MainGame.WorldData.GetWgoData("builder_clay_sand").SetGameRes(zombieWgoData.GameResStr.Get("sand_point", ""), 0);
				zombieWgoData.GameResStr.Remove("sand_point");
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_sand", 0f);
				zombieWgoData.FireEvent("sand_craft_end");
				GDPointData gdpointData = attachedWgoData.GetGDPointData("zombie_clay_sand_crafter_gd_point");
				zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
				zombieWgoData.MovementComponent.StartPath(gdpointData.Position, zombieWgoData.WorldZoneData.navigationGraph, zombieWgoData.WorldId, 1.5f, "sand_on_went_back", null, MovementComponent.DestinationType.Position);
			}
			else
			{
				Debug.LogError("No zombie linked to sand object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DEB RID: 15851
		private FlowInput @in;

		// Token: 0x04003DEC RID: 15852
		private FlowOutput @out;
	}
}
