using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5E RID: 2910
	[Name("Send Mine Worker To Enter Point", 0)]
	[Category("Game/Zombie")]
	public class Flow_SendMineWorkerToMineEnterPoint : GKCustomFlowNode
	{
		// Token: 0x06004D06 RID: 19718 RVA: 0x0016AEFC File Offset: 0x001690FC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0016AF4C File Offset: 0x0016914C
		private void DoAction(Flow flow)
		{
			ZombieWgoData zombieWgoData = base.SelfWgoData.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById("mine_zombie_enter");
				if (this.directionToMine)
				{
					zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
					zombieWgoData.MovementComponent.StartPath(gdpointDataById.Position, zombieWgoData.WorldZoneData.navigationGraph, zombieWgoData.WorldId, 1.5f, "mine_on_went_to_enter_dir_to_mine", null, MovementComponent.DestinationType.Position);
					zombieWgoData.AttachedWgoData.SetGameRes("stuff_disabled", 1);
				}
				else
				{
					zombieWgoData.AttachedWgoData.TryRemoveWorkerCutUnit(zombieWgoData);
					zombieWgoData.MovementComponent.StartPath(gdpointDataById.Position, gdpointDataById.GameSceneDataId, gdpointDataById.GameSceneDataId, MovementType.GDGraph, 1.5f, "mine_on_went_to_enter_dir_to_home", null, null, MovementComponent.DestinationType.Position);
				}
			}
			else
			{
				Debug.LogError("No zombie linked to mine object");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003DE0 RID: 15840
		public bool directionToMine;

		// Token: 0x04003DE1 RID: 15841
		private FlowInput @in;

		// Token: 0x04003DE2 RID: 15842
		private FlowOutput @out;
	}
}
