using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C15 RID: 3093
	[Name("Spawn Custom Zone Enemies", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_SpawnCustomZoneEnemies : GKCustomFlowNode
	{
		// Token: 0x06004F58 RID: 20312 RVA: 0x00175F08 File Offset: 0x00174108
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Spawn), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.spawnerName = base.AddValueInput<string>("spawnerName", "");
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00175F6D File Offset: 0x0017416D
		private void Spawn(Flow flow)
		{
			LazySingleton<FightingGameController>.Instance.ActivateCustomSpawner(this.spawnerName.value);
			this.@out.Call(flow);
		}

		// Token: 0x040040B8 RID: 16568
		private FlowInput @in;

		// Token: 0x040040B9 RID: 16569
		private FlowOutput @out;

		// Token: 0x040040BA RID: 16570
		private ValueInput<string> spawnerName;
	}
}
