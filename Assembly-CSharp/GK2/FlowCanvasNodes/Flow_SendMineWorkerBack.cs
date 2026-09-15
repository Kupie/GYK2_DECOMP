using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5D RID: 2909
	[Name("Send Mine Worker Back", 0)]
	[Category("Game/Quests")]
	public class Flow_SendMineWorkerBack : GKCustomFlowNode
	{
		// Token: 0x06004D03 RID: 19715 RVA: 0x0016ADC4 File Offset: 0x00168FC4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0016AE14 File Offset: 0x00169014
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				WgoData attachedWgoData = zombieWgoData.AttachedWgoData;
				MainGame.WorldData.GetWgoData("builder_mine").SetGameRes(zombieWgoData.GameResStr.Get("mine_point", ""), 0);
				zombieWgoData.GameResStr.Remove("mine_point");
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_mine", 0f);
				zombieWgoData.FireEvent("mine_craft_end");
				GDPointData gdpointData = attachedWgoData.GetGDPointData("zombie_mine_crafter_gd_point");
				zombieWgoData.MovementComponent.StartPath(gdpointData.Position, zombieWgoData.WorldZoneData.navigationGraph, zombieWgoData.WorldId, 1.5f, "mine_on_went_back", null, MovementComponent.DestinationType.Position);
			}
			else
			{
				Debug.LogError("No zombie linked to mine object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DDE RID: 15838
		private FlowInput @in;

		// Token: 0x04003DDF RID: 15839
		private FlowOutput @out;
	}
}
