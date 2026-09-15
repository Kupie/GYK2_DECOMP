using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B89 RID: 2953
	[Name("Create ZombieWgoData", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_CreateZombieWgoData : GKCustomFlowNode
	{
		// Token: 0x06004D8F RID: 19855 RVA: 0x0016DD30 File Offset: 0x0016BF30
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SpawnWgoData), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.id = base.AddValueInput<string>("id".CapitalizeFirst(), "");
			this.worldId = base.AddValueInput<string>("worldId".CapitalizeFirst(), "");
			this.zombieData = base.AddValueOutput<ZombieWgoData>("zombieData".CapitalizeFirst(), () => this.spawnedData, "");
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0016DDDC File Offset: 0x0016BFDC
		private void SpawnWgoData(Flow flow)
		{
			string text = this.worldId.value;
			if (string.IsNullOrEmpty(text))
			{
				text = MainGame.PlayerData.currentGameSceneId;
			}
			Item item = GameBalance.Me.GetData<BodyDef>(this.id.value).GenerateItem();
			this.spawnedData = MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, text, item, "collar_bronze", null, true);
			this.@out.Call(flow);
		}

		// Token: 0x04003E7C RID: 15996
		private FlowInput @in;

		// Token: 0x04003E7D RID: 15997
		private FlowOutput @out;

		// Token: 0x04003E7E RID: 15998
		private ValueInput<string> id;

		// Token: 0x04003E7F RID: 15999
		private ValueInput<string> worldId;

		// Token: 0x04003E80 RID: 16000
		private ValueOutput<ZombieWgoData> zombieData;

		// Token: 0x04003E81 RID: 16001
		private ZombieWgoData spawnedData;
	}
}
