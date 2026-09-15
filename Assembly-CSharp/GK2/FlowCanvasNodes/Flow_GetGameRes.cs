using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB4 RID: 2996
	[Name("Get GameRes", 0)]
	[Category("Game/GameRes")]
	public class Flow_GetGameRes : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E18 RID: 19992 RVA: 0x00170870 File Offset: 0x0016EA70
		protected override void RegisterPorts()
		{
			if (!this.getFromPlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeGameRes), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.gameResName = base.AddValueInput<string>("gameResName".CapitalizeFirst(), "");
			this.gameResValueOut = base.AddValueOutput<float>("Value", () => this.gameResValOut, "");
			this.gameResNameOut = base.AddValueOutput<string>("paramName", () => this.gameResName.value, "");
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0017092C File Offset: 0x0016EB2C
		private void ChangeGameRes(Flow flow)
		{
			if (this.getFromPlayer)
			{
				this.gameResValOut = base.PlayerData.GetRes(this.gameResName.value, 0f);
			}
			else
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					this.gameResValOut = wgoData.GetGameRes(this.gameResName.value);
				}
				else
				{
					Debug.LogError("Flow_AddGameRes: cannot get GameRes [" + this.gameResName.value + "] from null WGO");
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x001709B2 File Offset: 0x0016EBB2
		public override string name
		{
			get
			{
				return "Get GameRes From " + (this.getFromPlayer ? "Player" : "WgoData");
			}
		}

		// Token: 0x04003F35 RID: 16181
		[FlowNode.GatherPortsCallbackAttribute]
		public bool getFromPlayer;

		// Token: 0x04003F36 RID: 16182
		private FlowInput @in;

		// Token: 0x04003F37 RID: 16183
		private FlowOutput @out;

		// Token: 0x04003F38 RID: 16184
		private ValueInput<WgoData> wgo;

		// Token: 0x04003F39 RID: 16185
		private ValueInput<string> gameResName;

		// Token: 0x04003F3A RID: 16186
		private ValueOutput<string> gameResNameOut;

		// Token: 0x04003F3B RID: 16187
		private ValueOutput<float> gameResValueOut;

		// Token: 0x04003F3C RID: 16188
		private float gameResValOut;
	}
}
