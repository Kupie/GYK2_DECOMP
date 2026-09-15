using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BDA RID: 3034
	[Name("Call On Item Buy Expressions", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_OnItemBuyExpressions : GKCustomFlowNode
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x00172628 File Offset: 0x00170828
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.CallOnBuyExpressions), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueInput<Item>("item", "");
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x00172690 File Offset: 0x00170890
		private void CallOnBuyExpressions(Flow flow)
		{
			Item value = this.item.value;
			if (value != null)
			{
				foreach (LazyExpression lazyExpression in value.Definition.expressionsOnBuy)
				{
					lazyExpression.Evaluate(value);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003FC2 RID: 16322
		private FlowInput @in;

		// Token: 0x04003FC3 RID: 16323
		private FlowOutput @out;

		// Token: 0x04003FC4 RID: 16324
		private ValueInput<Item> item;
	}
}
