using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD0 RID: 3024
	[Name("Has WGO Data In World", 0)]
	[Category("Game/Script")]
	[Color("70f1ff")]
	[Icon("FS", false, "")]
	public class Flow_HasWgoDataInWorld : GKCustomFlowNode
	{
		// Token: 0x06004E76 RID: 20086 RVA: 0x00171DBC File Offset: 0x0016FFBC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.HasWgoDataInWorld), "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			this.wgoId = base.AddValueInput<string>("wgoId".CapitalizeFirst(), "");
			this.customTag = base.AddValueInput<string>("customTag", "");
			this.wgoIdOut = base.AddValueOutput<string>("wgoId", () => this.wgoId.value, "");
			this.customTagOut = base.AddValueOutput<string>("customTag", () => this.customTag.value, "");
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x00171E9C File Offset: 0x0017009C
		private void HasWgoDataInWorld(Flow flow)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoId.value);
			if (wgoData == null)
			{
				wgoData = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(this.customTag.value);
			}
			if (wgoData != null)
			{
				this.yes.Call(flow);
				return;
			}
			this.no.Call(flow);
		}

		// Token: 0x04003F9C RID: 16284
		private FlowInput @in;

		// Token: 0x04003F9D RID: 16285
		private FlowOutput yes;

		// Token: 0x04003F9E RID: 16286
		private FlowOutput no;

		// Token: 0x04003F9F RID: 16287
		private ValueInput<string> wgoId;

		// Token: 0x04003FA0 RID: 16288
		private ValueInput<string> customTag;

		// Token: 0x04003FA1 RID: 16289
		private ValueOutput<string> wgoIdOut;

		// Token: 0x04003FA2 RID: 16290
		private ValueOutput<string> customTagOut;
	}
}
