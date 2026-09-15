using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B69 RID: 2921
	[Name("Send Sawmill Worker Back", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendSawmillWorkerToPutTarget : GKCustomFlowNode
	{
		// Token: 0x06004D29 RID: 19753 RVA: 0x0016BD80 File Offset: 0x00169F80
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x0016BDD0 File Offset: 0x00169FD0
		private void DoAction(Flow flow)
		{
			WgoData selfWgoData = base.SelfWgoData;
			ZombieWgoData zombieWgoData = selfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				MainGame.WorldData.GetWgoData("builder_sawmill").SetGameRes(zombieWgoData.GameResStr.Get("sawmill_point", ""), 0);
				zombieWgoData.GameResStr.Remove("sawmill_point");
				zombieWgoData.CaretakerPortableItem = new Item(selfWgoData.CraftComponent.CurrentCraftElement.Def.outputItems.chanceOutputItems[0].id, 1);
				zombieWgoData.AttachedWgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Set("wait_for_zombie_at_sawmill", 0f);
				zombieWgoData.FireEvent("sawmill_craft_end");
				GDPointData gdpointData = MainGame.WorldData.GetWgoData("sawmill_wood_container").GetGDPointData("zombie_sawmill_wood_container_gd_point");
				zombieWgoData.MovementComponent.StartPath(gdpointData.Position, zombieWgoData.WorldZoneData.navigationGraph, zombieWgoData.WorldId, 1.5f, "sawmill_on_went_to_container", null, MovementComponent.DestinationType.Position);
			}
			else
			{
				Debug.LogError("No zombie linked to sawmill object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DF7 RID: 15863
		private FlowInput @in;

		// Token: 0x04003DF8 RID: 15864
		private FlowOutput @out;
	}
}
