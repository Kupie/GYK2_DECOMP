using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C0B RID: 3083
	[Name("Show Simple Text With Icon Notification", 0)]
	[Category("Game/Quests")]
	public class Flow_ShowSimpleTextWithIconNotification : GKCustomFlowNode
	{
		// Token: 0x06004F3F RID: 20287 RVA: 0x001758DC File Offset: 0x00173ADC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ShowWindow), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.locale = base.AddValueInput<string>("locale".CapitalizeFirst(), "");
			this.iconId = base.AddValueInput<string>("iconId".CapitalizeFirst(), "");
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00175961 File Offset: 0x00173B61
		private void ShowWindow(Flow flow)
		{
			LazySingleton<UINotificator>.Instance.ShowSimpleTextWithIconNotification(this.locale.value, this.iconId.value);
			this.@out.Call(flow);
		}

		// Token: 0x04004095 RID: 16533
		private FlowInput @in;

		// Token: 0x04004096 RID: 16534
		private FlowOutput @out;

		// Token: 0x04004097 RID: 16535
		private ValueInput<string> locale;

		// Token: 0x04004098 RID: 16536
		private ValueInput<string> iconId;
	}
}
