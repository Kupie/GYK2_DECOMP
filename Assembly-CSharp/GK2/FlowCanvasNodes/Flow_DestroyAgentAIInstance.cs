using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8B RID: 2955
	[Name("Destroy Agent AI Instance", 0)]
	[Category("Game/Fighting")]
	[Description("Explicitly destroys a runtime-created AgentAI instance to free memory.")]
	public class Flow_DestroyAgentAIInstance : GKCustomFlowNode
	{
		// Token: 0x06004D97 RID: 19863 RVA: 0x0016DF90 File Offset: 0x0016C190
		protected override void RegisterPorts()
		{
			this.aiInstance = base.AddValueInput<AgentAI>("AI Instance", "");
			base.AddFlowInput("In", delegate(Flow f)
			{
				this.Execute();
				f.Call(this.outFlow);
			}, "");
			this.outFlow = base.AddFlowOutput("Out", "");
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0016DFE6 File Offset: 0x0016C1E6
		private void Execute()
		{
			if (this.aiInstance.value == null)
			{
				return;
			}
			global::UnityEngine.Object.Destroy(this.aiInstance.value);
		}

		// Token: 0x04003E87 RID: 16007
		private ValueInput<AgentAI> aiInstance;

		// Token: 0x04003E88 RID: 16008
		private FlowOutput outFlow;
	}
}
