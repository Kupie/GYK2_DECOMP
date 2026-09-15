using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B76 RID: 2934
	[Name("Apply WgoData Part State", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_ApplyWgoDataPartState : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D4F RID: 19791 RVA: 0x0016CC88 File Offset: 0x0016AE88
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeWgoViewState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.wgoPartId = base.AddValueInput<string>("wgoPartId", "");
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x0016CCF4 File Offset: 0x0016AEF4
		protected virtual void ChangeWgoViewState(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData == null)
			{
				Debug.LogError(string.Format("[{0}]: not found wgoData: {1}", "GKCustomFlowNode", this.wgoId));
			}
			else
			{
				wgoData.ApplyWgoPartState(this.wgoPartId.value, -1);
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004D51 RID: 19793 RVA: 0x0016CD45 File Offset: 0x0016AF45
		public override string name
		{
			get
			{
				return "Apply WgoData Part State";
			}
		}

		// Token: 0x04003E36 RID: 15926
		protected FlowInput @in;

		// Token: 0x04003E37 RID: 15927
		protected FlowOutput @out;

		// Token: 0x04003E38 RID: 15928
		protected ValueInput<string> wgoPartId;
	}
}
