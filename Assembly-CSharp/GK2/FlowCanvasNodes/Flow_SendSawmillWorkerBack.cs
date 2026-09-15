using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B68 RID: 2920
	[Name("Send Sawmill Worker Back", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendSawmillWorkerBack : GKCustomFlowNode
	{
		// Token: 0x06004D26 RID: 19750 RVA: 0x0016BBE0 File Offset: 0x00169DE0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0016BC30 File Offset: 0x00169E30
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
				zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
				MovementComponent movementComponent = zombieWgoData.MovementComponent;
				Vector3 position = gdpointData.Position;
				WorldZoneData worldZoneData = zombieWgoData.WorldZoneData;
				IEnumerable<LazyConsts.Navigation.Graph> enumerable = ((worldZoneData != null) ? worldZoneData.MovementGraphs : null);
				WorldZoneData worldZoneData2 = zombieWgoData.WorldZoneData;
				movementComponent.StartPath(position, NavigationGraphMaskUtils.ToGraphMask(enumerable, (worldZoneData2 != null) ? worldZoneData2.navigationGraph : LazyConsts.Navigation.Graph.None), zombieWgoData.WorldId, 1.5f, "sawmill_on_went_back", null, MovementComponent.DestinationType.Position);
			}
			else
			{
				Debug.LogError("No zombie linked to sawmill object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DF5 RID: 15861
		private FlowInput @in;

		// Token: 0x04003DF6 RID: 15862
		private FlowOutput @out;
	}
}
