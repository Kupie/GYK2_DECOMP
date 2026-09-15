using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE3 RID: 3043
	[Name("Drop Overhead", 0)]
	[Category("Game/Player")]
	[Color("313c8f")]
	public class Flow_PlayerDropOverhead : GKCustomFlowNode
	{
		// Token: 0x06004EB1 RID: 20145 RVA: 0x00172BDC File Offset: 0x00170DDC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.TryDropOverhead), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueOutput<Item>("item", () => this.lastDroppedItem, "");
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00172C50 File Offset: 0x00170E50
		private void TryDropOverhead(Flow flow)
		{
			this.lastDroppedItem = null;
			if (base.PlayerData.HasMultipleOverheadItems)
			{
				this.@out.Call(flow);
				return;
			}
			if (base.PlayerData.HasOverheadItem)
			{
				this.lastDroppedItem = base.PlayerData.overheadItem;
				base.PlayerData.DropOverheadItem();
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003FDB RID: 16347
		private FlowInput @in;

		// Token: 0x04003FDC RID: 16348
		private FlowOutput @out;

		// Token: 0x04003FDD RID: 16349
		private Item lastDroppedItem;

		// Token: 0x04003FDE RID: 16350
		private ValueOutput<Item> item;
	}
}
