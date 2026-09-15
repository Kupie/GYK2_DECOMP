using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BEE RID: 3054
	[Name("Replace WGO Data", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_ReplaceWgoData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004ED9 RID: 20185 RVA: 0x00173D6C File Offset: 0x00171F6C
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ReplaceWGOData), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.newWgoId = base.AddValueInput<string>("newWgoId", "");
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x00173DD8 File Offset: 0x00171FD8
		protected virtual void ReplaceWGOData(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData == null)
			{
				Debug.LogError(string.Format("[{0}]: not found wgoData: {1}", "GKCustomFlowNode", this.wgoId));
			}
			else if (this.transferDataToNewWgo)
			{
				MainGame.Instance.GameSave.WorldData.ChangeWgoData(wgoData, this.newWgoId.value);
			}
			else
			{
				MainGame.Instance.GameSave.WorldData.ReplaceWgoData(wgoData, this.newWgoId.value);
			}
			this.@out.Call(flow);
		}

		// Token: 0x0400401E RID: 16414
		[FlowNode.GatherPortsCallbackAttribute]
		public bool transferDataToNewWgo;

		// Token: 0x0400401F RID: 16415
		protected FlowInput @in;

		// Token: 0x04004020 RID: 16416
		protected FlowOutput @out;

		// Token: 0x04004021 RID: 16417
		protected ValueInput<string> newWgoId;
	}
}
