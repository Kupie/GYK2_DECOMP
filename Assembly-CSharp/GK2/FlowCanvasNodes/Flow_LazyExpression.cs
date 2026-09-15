using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD5 RID: 3029
	[Name("Lazy Expression", 0)]
	[Category("Game/Environment")]
	public class Flow_LazyExpression : GKCustomFlowNode
	{
		// Token: 0x06004E87 RID: 20103 RVA: 0x00172238 File Offset: 0x00170438
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.GoToSleep), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.expression = base.AddValueInput<string>("expression".CapitalizeFirst(), "");
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x001722A2 File Offset: 0x001704A2
		private void GoToSleep(Flow flow)
		{
			new LazyExpression(this.expression.value).Evaluate();
			this.@out.Call(flow);
		}

		// Token: 0x04003FB3 RID: 16307
		private FlowInput @in;

		// Token: 0x04003FB4 RID: 16308
		private FlowOutput @out;

		// Token: 0x04003FB5 RID: 16309
		private ValueInput<string> expression;
	}
}
