using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B6A RID: 2922
	[Name("Send Sawmill Worker To Work", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendSawmillWorkerToWork : GKCustomFlowNode
	{
		// Token: 0x06004D2C RID: 19756 RVA: 0x0016BEF8 File Offset: 0x0016A0F8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x0016BF48 File Offset: 0x0016A148
		private void DoAction(Flow flow)
		{
			WgoData selfWgoData = base.SelfWgoData;
			WgoData wgoData = MainGame.WorldData.GetWgoData("builder_sawmill");
			ZombieWgoData zombieWgoData = selfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				GDPointData gdpointData = null;
				List<GDPointData> list = new List<GDPointData>();
				for (int i = 0; i < 2147483647; i++)
				{
					gdpointData = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(string.Format("sawmill_zombie_{0}", i + 1));
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
					Debug.LogError("Can't find gd point for zombie on sawmill");
				}
				else
				{
					zombieWgoData.GameResStr.Set("sawmill_point", gdpointData.Id);
					zombieWgoData.AttachedWgoData.SetGameRes("stuff_disabled", 1);
					zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
					MovementComponent movementComponent = zombieWgoData.MovementComponent;
					Vector3 position = gdpointData.Position;
					WorldZoneData worldZoneData = zombieWgoData.WorldZoneData;
					IEnumerable<LazyConsts.Navigation.Graph> enumerable = ((worldZoneData != null) ? worldZoneData.MovementGraphs : null);
					WorldZoneData worldZoneData2 = zombieWgoData.WorldZoneData;
					movementComponent.StartPath(position, NavigationGraphMaskUtils.ToGraphMask(enumerable, (worldZoneData2 != null) ? worldZoneData2.navigationGraph : LazyConsts.Navigation.Graph.None), zombieWgoData.WorldId, 1.5f, "sawmill_craft_start", null, MovementComponent.DestinationType.Position);
				}
			}
			else
			{
				Debug.LogError("No zombie linked to sawmill object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DF9 RID: 15865
		private FlowInput @in;

		// Token: 0x04003DFA RID: 15866
		private FlowOutput @out;
	}
}
