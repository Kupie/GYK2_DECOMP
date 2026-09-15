using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B6D RID: 2925
	[Name("Add Interaction Event", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	[Icon("Dialogue", false, "")]
	public class Flow_AddInteractionEvent : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x0016C30E File Offset: 0x0016A50E
		public override string name
		{
			get
			{
				return (this.remove ? "Remove" : "Add") + " Interaction Event";
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0016C330 File Offset: 0x0016A530
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.AddEvent), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			base.RegisterPorts();
			this.eventId = base.AddValueInput<string>("eventId", "");
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0016C39C File Offset: 0x0016A59C
		private void AddEvent(Flow flow)
		{
			if (!this.remove)
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					wgoData.AddInteractionEvent(this.eventId.value, this.isFake);
				}
			}
			else
			{
				WgoData wgoData2 = base.GetWgoData();
				if (wgoData2 != null)
				{
					wgoData2.RemoveInteractionEvent(this.eventId.value);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003E07 RID: 15879
		[FlowNode.GatherPortsCallbackAttribute]
		public bool remove;

		// Token: 0x04003E08 RID: 15880
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isFake;

		// Token: 0x04003E09 RID: 15881
		private FlowInput @in;

		// Token: 0x04003E0A RID: 15882
		private FlowOutput @out;

		// Token: 0x04003E0B RID: 15883
		private ValueInput<string> eventId;
	}
}
