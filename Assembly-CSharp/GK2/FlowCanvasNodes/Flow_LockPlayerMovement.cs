using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD6 RID: 3030
	[Name("Lock Player Movement", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_LockPlayerMovement : GKCustomFlowNode
	{
		// Token: 0x06004E8A RID: 20106 RVA: 0x001722C8 File Offset: 0x001704C8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.LockMovement), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x00172317 File Offset: 0x00170517
		private void LockMovement(Flow flow)
		{
			MainGame.PlayerController.PhysicalBody.LockMovement(this.isLock);
			this.@out.Call(flow);
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x0017233A File Offset: 0x0017053A
		public override string name
		{
			get
			{
				return (this.isLock ? "Lock" : "Unlock") + " Player Movement";
			}
		}

		// Token: 0x04003FB6 RID: 16310
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isLock;

		// Token: 0x04003FB7 RID: 16311
		private FlowInput @in;

		// Token: 0x04003FB8 RID: 16312
		private FlowOutput @out;
	}
}
