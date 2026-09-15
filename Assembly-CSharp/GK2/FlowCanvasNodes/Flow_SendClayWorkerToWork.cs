using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B59 RID: 2905
	[Name("Send Clay Worker To Work", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendClayWorkerToWork : GKCustomFlowNode
	{
		// Token: 0x06004CF6 RID: 19702 RVA: 0x0016A8B8 File Offset: 0x00168AB8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0016A908 File Offset: 0x00168B08
		private void DoAction(Flow flow)
		{
			WgoData selfWgoData = base.SelfWgoData;
			WgoData wgoData = MainGame.WorldData.GetWgoData("builder_clay_sand");
			ZombieWgoData zombieWgoData = selfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				GDPointData gdpointData = null;
				List<GDPointData> list = new List<GDPointData>();
				for (int i = 0; i < 2147483647; i++)
				{
					gdpointData = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(string.Format("clay_zombie_{0}", i + 1));
					if (gdpointData == null)
					{
						break;
					}
					list.Add(gdpointData);
				}
				for (int j = 0; j < list.Count; j++)
				{
					GDPointData gdpointData2 = list[j];
					if (wgoData.GetGameResInt(gdpointData2.Id) == 0)
					{
						gdpointData = gdpointData2;
						wgoData.SetGameRes(gdpointData2.Id, 1);
						break;
					}
				}
				if (gdpointData == null)
				{
					Debug.LogError("Can't find gd point for zombie on clay");
				}
				else
				{
					NNInfo nearest = AstarPath.active.graphs[16].GetNearest(gdpointData.Position, NearestNodeConstraint.Walkable);
					ref NNInfo nearest2 = AstarPath.active.graphs[11].GetNearest(gdpointData.Position, NearestNodeConstraint.Walkable);
					float num = Vector3.Distance(nearest.position, gdpointData.Position);
					float num2 = Vector3.Distance(nearest2.position, gdpointData.Position);
					GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask((num < num2) ? LazyConsts.Navigation.Graph.SandClay : LazyConsts.Navigation.Graph.RuinedTemple);
					zombieWgoData.GameResStr.Set("clay_point", gdpointData.Id);
					zombieWgoData.AttachedWgoData.SetGameRes("stuff_disabled", 1);
					zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
					zombieWgoData.MovementComponent.StartPath(gdpointData.Position, graphMask, zombieWgoData.WorldId, 1.5f, "clay_craft_start", null, MovementComponent.DestinationType.Position);
				}
			}
			else
			{
				Debug.LogError("No zombie linked to clay object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DD6 RID: 15830
		private FlowInput @in;

		// Token: 0x04003DD7 RID: 15831
		private FlowOutput @out;
	}
}
