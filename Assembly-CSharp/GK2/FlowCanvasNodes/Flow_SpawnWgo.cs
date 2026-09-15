using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C17 RID: 3095
	[Name("Spawn WgoData", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SpawnWgo : GKCustomFlowNode
	{
		// Token: 0x06004F5E RID: 20318 RVA: 0x00176050 File Offset: 0x00174250
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SpawnWgoData), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.id = base.AddValueInput<string>("id".CapitalizeFirst(), "");
			this.customTag = base.AddValueInput<string>("customTag".CapitalizeFirst(), "");
			this.position = base.AddValueInput<Vector3>("position".CapitalizeFirst(), "");
			this.worldId = base.AddValueInput<string>("worldId".CapitalizeFirst(), "");
			this.wgoData = base.AddValueOutput<WgoData>("wgoData".CapitalizeFirst(), () => this.spawnedData, "");
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00176134 File Offset: 0x00174334
		private void SpawnWgoData(Flow flow)
		{
			string text = this.worldId.value;
			if (string.IsNullOrEmpty(text))
			{
				text = MainGame.PlayerData.currentGameSceneId;
			}
			MainGame.Instance.GameSave.worldData.AddWgoData(this.id.value, this.position.value, text, this.customTag.value, out this.spawnedData, false);
			this.@out.Call(flow);
		}

		// Token: 0x040040C0 RID: 16576
		private FlowInput @in;

		// Token: 0x040040C1 RID: 16577
		private FlowOutput @out;

		// Token: 0x040040C2 RID: 16578
		private ValueInput<string> id;

		// Token: 0x040040C3 RID: 16579
		private ValueInput<string> customTag;

		// Token: 0x040040C4 RID: 16580
		private new ValueInput<Vector3> position;

		// Token: 0x040040C5 RID: 16581
		private ValueInput<string> worldId;

		// Token: 0x040040C6 RID: 16582
		private ValueOutput<WgoData> wgoData;

		// Token: 0x040040C7 RID: 16583
		private WgoData spawnedData;
	}
}
