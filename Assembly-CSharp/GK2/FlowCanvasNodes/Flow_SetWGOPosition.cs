using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C08 RID: 3080
	[Name("Teleport WGO", 0)]
	[Category("Game/Environment")]
	[Color("f47dff")]
	public class Flow_SetWGOPosition : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004F34 RID: 20276 RVA: 0x00175614 File Offset: 0x00173814
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Teleport), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.gdPointDataOutput = base.AddValueOutput<GDPointData>("GDPointData", () => this.GetGDPointData(), "");
			if (this.findGdPointById)
			{
				this.gdPointId = base.AddValueInput<string>("gdPointId", "");
				return;
			}
			this.gdPointData = base.AddValueInput<GDPointData>("gdPointData", "");
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001756C0 File Offset: 0x001738C0
		private GDPointData GetGDPointData()
		{
			if (!this.findGdPointById)
			{
				return this.gdPointData.value;
			}
			return MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.gdPointId.value);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x001756FC File Offset: 0x001738FC
		private void Teleport(Flow flow)
		{
			GDPointData gdpointData = this.GetGDPointData();
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				if (this.fxBefore)
				{
					WorldFX.Spawn(wgoData.Position, "puff_npc", null, default(Vector3));
				}
				wgoData.Position = gdpointData.Position;
				if (this.fxAfter)
				{
					WorldFX.Spawn(wgoData.Position, "puff_npc", null, default(Vector3));
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004086 RID: 16518
		private FlowInput @in;

		// Token: 0x04004087 RID: 16519
		private FlowOutput @out;

		// Token: 0x04004088 RID: 16520
		private ValueInput<string> gdPointId;

		// Token: 0x04004089 RID: 16521
		private ValueInput<GDPointData> gdPointData;

		// Token: 0x0400408A RID: 16522
		private ValueOutput<GDPointData> gdPointDataOutput;

		// Token: 0x0400408B RID: 16523
		[FlowNode.GatherPortsCallbackAttribute]
		public bool findGdPointById;

		// Token: 0x0400408C RID: 16524
		[FlowNode.GatherPortsCallbackAttribute]
		public bool fxBefore;

		// Token: 0x0400408D RID: 16525
		[FlowNode.GatherPortsCallbackAttribute]
		public bool fxAfter;
	}
}
