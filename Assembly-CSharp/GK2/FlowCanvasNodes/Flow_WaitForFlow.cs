using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C2B RID: 3115
	[Name("Wait For Flow", 0)]
	[Description("This block is like DoOnce one, but waits flow as many as set")]
	public class Flow_WaitForFlow : GKCustomFlowNode
	{
		// Token: 0x06004FA2 RID: 20386 RVA: 0x00177610 File Offset: 0x00175810
		protected override void RegisterPorts()
		{
			this.activated = new bool[this.inputFlowsWaitCount];
			for (int i = 0; i < this.inputFlowsWaitCount; i++)
			{
				int j = i;
				base.AddFlowInput(j.ToString(), delegate(Flow flow)
				{
					this.Check(j, flow);
				}, "");
			}
			this.reset = base.AddFlowInput("reset".CapitalizeFirst(), new FlowHandler(this.Reset), "");
			this.waitEnded = base.AddFlowOutput("waitEnded".CapitalizeFirst(), "");
			this.whileWaiting = base.AddFlowOutput("whileWaiting".CapitalizeFirst(), "");
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001776D4 File Offset: 0x001758D4
		private void Check(int index, Flow flow)
		{
			this.activated[index] = true;
			if (this.CheckFlowsActivated())
			{
				for (int i = 0; i < this.inputFlowsWaitCount; i++)
				{
					this.activated[i] = false;
				}
				this.waitEnded.Call(flow);
				return;
			}
			this.whileWaiting.Call(flow);
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x00177728 File Offset: 0x00175928
		private bool CheckFlowsActivated()
		{
			for (int i = 0; i < this.inputFlowsWaitCount; i++)
			{
				if (!this.activated[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x00177754 File Offset: 0x00175954
		private void Reset(Flow flow)
		{
			for (int i = 0; i < this.inputFlowsWaitCount; i++)
			{
				this.activated[i] = false;
			}
			this.waitEnded.Call(flow);
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x00177787 File Offset: 0x00175987
		public override string name
		{
			get
			{
				return string.Format("Wait For {0} Flow", this.inputFlowsWaitCount);
			}
		}

		// Token: 0x0400411C RID: 16668
		[SerializeField]
		[ExposeField]
		[MinValue(2)]
		[DelayedField]
		[FlowNode.GatherPortsCallbackAttribute]
		public int inputFlowsWaitCount = 2;

		// Token: 0x0400411D RID: 16669
		private FlowInput reset;

		// Token: 0x0400411E RID: 16670
		private FlowOutput waitEnded;

		// Token: 0x0400411F RID: 16671
		private FlowOutput whileWaiting;

		// Token: 0x04004120 RID: 16672
		private bool[] activated;
	}
}
