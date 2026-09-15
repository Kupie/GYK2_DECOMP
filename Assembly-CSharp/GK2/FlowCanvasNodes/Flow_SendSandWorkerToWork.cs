using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B64 RID: 2916
	[Name("Send Sand Worker To Work", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendSandWorkerToWork : GKCustomFlowNode
	{
		// Token: 0x06004D19 RID: 19737 RVA: 0x0016B610 File Offset: 0x00169810
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0016B660 File Offset: 0x00169860
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
					gdpointData = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(string.Format("sand_zombie_{0}", i + 1));
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
					Debug.LogError("Can't find gd point for zombie on sand");
				}
				else
				{
					NNInfo nearest = AstarPath.active.graphs[16].GetNearest(gdpointData.Position, NearestNodeConstraint.Walkable);
					ref NNInfo nearest2 = AstarPath.active.graphs[11].GetNearest(gdpointData.Position, NearestNodeConstraint.Walkable);
					float num = Vector3.Distance(nearest.position, gdpointData.Position);
					float num2 = Vector3.Distance(nearest2.position, gdpointData.Position);
					GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask((num < num2) ? LazyConsts.Navigation.Graph.SandClay : LazyConsts.Navigation.Graph.RuinedTemple);
					zombieWgoData.GameResStr.Set("sand_point", gdpointData.Id);
					zombieWgoData.AttachedWgoData.SetGameRes("stuff_disabled", 1);
					zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
					zombieWgoData.MovementComponent.StartPath(gdpointData.Position, graphMask, zombieWgoData.WorldId, 1.5f, "sand_craft_start", null, MovementComponent.DestinationType.Position);
				}
			}
			else
			{
				Debug.LogError("No zombie linked to sand object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DED RID: 15853
		private FlowInput @in;

		// Token: 0x04003DEE RID: 15854
		private FlowOutput @out;
	}
}
