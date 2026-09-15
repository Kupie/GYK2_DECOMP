using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BFC RID: 3068
	[Name("Set GameResStr", 0)]
	[Category("Game/GameRes")]
	[Color("FFFFFF")]
	public class Flow_SetGameResStr : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004F07 RID: 20231 RVA: 0x00174B98 File Offset: 0x00172D98
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeGameResStr), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.gameResStrKey = base.AddValueInput<string>("gameResStrKey".CapitalizeFirst(), "");
			if (!this.remove)
			{
				this.gameResStrValue = base.AddValueInput<string>("gameResStrValue".CapitalizeFirst(), "");
			}
			this.gameResStrKeyOut = base.AddValueOutput<string>("key", () => this.gameResStrKey.value, "");
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x00174C50 File Offset: 0x00172E50
		private void ChangeGameResStr(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				if (!this.remove)
				{
					wgoData.GameResStr.Set(this.gameResStrKey.value, this.gameResStrValue.value);
				}
				else
				{
					wgoData.GameResStr.Remove(this.gameResStrKey.value);
				}
			}
			else
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Flow_AddGameRes: Tried to ",
					this.remove ? "remove" : "set",
					" GameResStr ",
					this.remove ? "from" : "to",
					" null WGO"
				}));
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004F09 RID: 20233 RVA: 0x00174D0C File Offset: 0x00172F0C
		public override string name
		{
			get
			{
				return (this.remove ? "Remove" : "Set") + " GameResStr" + (this.remove ? " From" : " To") + " WgoData";
			}
		}

		// Token: 0x04004056 RID: 16470
		[FlowNode.GatherPortsCallbackAttribute]
		public bool remove;

		// Token: 0x04004057 RID: 16471
		private FlowInput @in;

		// Token: 0x04004058 RID: 16472
		private FlowOutput @out;

		// Token: 0x04004059 RID: 16473
		private ValueInput<WgoData> wgo;

		// Token: 0x0400405A RID: 16474
		private ValueInput<string> gameResStrKey;

		// Token: 0x0400405B RID: 16475
		private ValueInput<string> gameResStrValue;

		// Token: 0x0400405C RID: 16476
		private ValueOutput<string> gameResStrKeyOut;
	}
}
