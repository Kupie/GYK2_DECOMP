using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5F RID: 2911
	[Name("Send Mine Worker To Work", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendMineWorkerToWork : GKCustomFlowNode
	{
		// Token: 0x06004D09 RID: 19721 RVA: 0x0016B038 File Offset: 0x00169238
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x0016B088 File Offset: 0x00169288
		private void DoAction(Flow flow)
		{
			WgoData selfWgoData = base.SelfWgoData;
			WgoData wgoData = MainGame.WorldData.GetWgoData("builder_mine");
			ZombieWgoData zombieWgoData = selfWgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				GDPointData gdpointData = null;
				List<GDPointData> list = new List<GDPointData>();
				for (int i = 0; i < 2147483647; i++)
				{
					gdpointData = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(string.Format("mine_zombie_{0}", i + 1));
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
					Debug.LogError("Can't find gd point for zombie on mine");
				}
				else
				{
					zombieWgoData.GameResStr.Set("mine_point", gdpointData.Id);
					zombieWgoData.MovementComponent.StartPath(gdpointData.Position, gdpointData.GameSceneDataId, gdpointData.GameSceneDataId, MovementType.GDGraph, 1.5f, "mine_craft_start", null, null, MovementComponent.DestinationType.Position);
				}
			}
			else
			{
				Debug.LogError("No zombie linked to mine object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DE3 RID: 15843
		private FlowInput @in;

		// Token: 0x04003DE4 RID: 15844
		private FlowOutput @out;
	}
}
