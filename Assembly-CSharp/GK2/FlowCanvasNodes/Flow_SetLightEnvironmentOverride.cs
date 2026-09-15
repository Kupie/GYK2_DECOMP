using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BFE RID: 3070
	[Name("Set Light Environment Override", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SetLightEnvironmentOverride : GKCustomFlowNode
	{
		// Token: 0x06004F0F RID: 20239 RVA: 0x00174DF0 File Offset: 0x00172FF0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangePreset), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.reset)
			{
				this.presetName = base.AddValueInput<string>("presetName", "");
			}
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00174E60 File Offset: 0x00173060
		private void ChangePreset(Flow flow)
		{
			if (!this.reset)
			{
				if (!string.IsNullOrEmpty(this.presetName.value))
				{
					EnvironmentEngine.Instance.ApplyOverridePreset(this.presetName.value, 1f);
				}
			}
			else
			{
				EnvironmentEngine.Instance.ApplyOverridePreset(null, 0f);
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004F11 RID: 20241 RVA: 0x00174EBF File Offset: 0x001730BF
		public override string name
		{
			get
			{
				return (this.reset ? "Reset" : "Set") + " Light Environment Override";
			}
		}

		// Token: 0x04004061 RID: 16481
		[FlowNode.GatherPortsCallbackAttribute]
		public bool reset;

		// Token: 0x04004062 RID: 16482
		private FlowInput @in;

		// Token: 0x04004063 RID: 16483
		private FlowOutput @out;

		// Token: 0x04004064 RID: 16484
		private ValueInput<string> presetName;
	}
}
