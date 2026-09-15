using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B58 RID: 2904
	[Name("Send Clay Worker Back", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendClayWorkerBack : GKCustomFlowNode
	{
		// Token: 0x06004CF3 RID: 19699 RVA: 0x0016A770 File Offset: 0x00168970
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0016A7C0 File Offset: 0x001689C0
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				WgoData attachedWgoData = zombieWgoData.AttachedWgoData;
				MainGame.WorldData.GetWgoData("builder_clay_sand").SetGameRes(zombieWgoData.GameResStr.Get("clay_point", ""), 0);
				zombieWgoData.GameResStr.Remove("clay_point");
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_clay", 0f);
				zombieWgoData.FireEvent("clay_craft_end");
				GDPointData gdpointData = attachedWgoData.GetGDPointData("zombie_clay_sand_crafter_gd_point");
				zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
				zombieWgoData.MovementComponent.StartPath(gdpointData.Position, zombieWgoData.WorldZoneData.navigationGraph, zombieWgoData.WorldId, 1.5f, "clay_on_went_back", null, MovementComponent.DestinationType.Position);
			}
			else
			{
				Debug.LogError("No zombie linked to clay object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DD4 RID: 15828
		private FlowInput @in;

		// Token: 0x04003DD5 RID: 15829
		private FlowOutput @out;
	}
}
