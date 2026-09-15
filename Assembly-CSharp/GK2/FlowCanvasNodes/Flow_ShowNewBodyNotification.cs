using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C0A RID: 3082
	[Name("Show New Body Notification", 0)]
	[Category("Game/Quests")]
	public class Flow_ShowNewBodyNotification : GKCustomFlowNode
	{
		// Token: 0x06004F3C RID: 20284 RVA: 0x0017584C File Offset: 0x00173A4C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ShowWindow), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.bodyId = base.AddValueInput<string>("bodyId".CapitalizeFirst(), "");
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001758B6 File Offset: 0x00173AB6
		private void ShowWindow(Flow flow)
		{
			LazySingleton<UINotificator>.Instance.ShowNewBodyNotification(this.bodyId.value);
			this.@out.Call(flow);
		}

		// Token: 0x04004092 RID: 16530
		private FlowInput @in;

		// Token: 0x04004093 RID: 16531
		private FlowOutput @out;

		// Token: 0x04004094 RID: 16532
		private ValueInput<string> bodyId;
	}
}
