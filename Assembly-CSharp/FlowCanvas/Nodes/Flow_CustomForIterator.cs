using System;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{
	// Token: 0x02000B4C RID: 2892
	[Name("Custom For Iterator", 0)]
	[Category("Flow Controllers/Iterators")]
	[FlowNode.ContextDefinedInputsAttribute(new Type[] { typeof(int) })]
	[FlowNode.ContextDefinedOutputsAttribute(new Type[] { typeof(int) })]
	public class Flow_CustomForIterator : FlowControlNode
	{
		// Token: 0x06004CD7 RID: 19671 RVA: 0x0016A21C File Offset: 0x0016841C
		protected override void RegisterPorts()
		{
			ValueInput<int> n = base.AddValueInput<int>("Loops", "");
			base.AddValueOutput<int>("Index", () => this.current, "");
			FlowOutput fCurrent = base.AddFlowOutput("Do", "");
			FlowOutput fFinish = base.AddFlowOutput("Done", "");
			base.AddFlowInput("In", delegate(Flow f)
			{
				this.current = 0;
				fCurrent.Call(f);
			}, "");
			base.AddFlowInput("Iterate", delegate(Flow f)
			{
				this.current++;
				if (this.current < n.value)
				{
					fCurrent.Call(f);
					return;
				}
				fFinish.Call(f);
			}, "");
		}

		// Token: 0x04003DC9 RID: 15817
		private int current;
	}
}
