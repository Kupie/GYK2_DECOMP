using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C00 RID: 3072
	[Name("Set Post Process Profile", 0)]
	[Category("Game/Camera")]
	[Color("313c8f")]
	public class Flow_SetPostProcessProfile : GKCustomFlowNode
	{
		// Token: 0x06004F17 RID: 20247 RVA: 0x0017502C File Offset: 0x0017322C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeProfile), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.reset)
			{
				this.profileName = base.AddValueInput<string>("profileName", "");
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x0017509C File Offset: 0x0017329C
		private void ChangeProfile(Flow flow)
		{
			if (!this.reset)
			{
				if (!string.IsNullOrEmpty(this.profileName.value))
				{
					CameraSystem.Instance.MainCamera.SetPostProcessProfile(this.profileName.value);
				}
			}
			else
			{
				CameraSystem.Instance.MainCamera.ResetPostProcessProfile();
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06004F19 RID: 20249 RVA: 0x001750FA File Offset: 0x001732FA
		public override string name
		{
			get
			{
				return (this.reset ? "Reset" : "Set") + " Post Process Profile";
			}
		}

		// Token: 0x0400406B RID: 16491
		[FlowNode.GatherPortsCallbackAttribute]
		public bool reset;

		// Token: 0x0400406C RID: 16492
		private FlowInput @in;

		// Token: 0x0400406D RID: 16493
		private FlowOutput @out;

		// Token: 0x0400406E RID: 16494
		private ValueInput<string> profileName;
	}
}
