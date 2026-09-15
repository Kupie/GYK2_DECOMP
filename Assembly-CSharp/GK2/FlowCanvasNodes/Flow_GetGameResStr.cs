using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB5 RID: 2997
	[Name("Get GameResStr", 0)]
	[Category("Game/GameRes")]
	public class Flow_GetGameResStr : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E1E RID: 19998 RVA: 0x001709E8 File Offset: 0x0016EBE8
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.GetGameResStr), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.findByValue)
			{
				this.gameResStrKey = base.AddValueInput<string>("gameResStrKey".CapitalizeFirst(), "");
			}
			else
			{
				this.gameResStrValue = base.AddValueInput<string>("gameResStrValue".CapitalizeFirst(), "");
			}
			this.hasGameResStrOut = base.AddValueOutput<bool>("Has", () => this.hasGameRestStr, "");
			this.gameResStrKeyOut = base.AddValueOutput<string>("Key", () => this.gameResStrKOut, "");
			this.gameResStrValueOut = base.AddValueOutput<string>("Value", () => this.gameResStrValOut, "");
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x00170AE4 File Offset: 0x0016ECE4
		private void GetGameResStr(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				if (!this.findByValue)
				{
					this.gameResStrKOut = this.gameResStrKey.value;
					this.hasGameRestStr = wgoData.GameResStr.Has(this.gameResStrKOut);
					this.gameResStrValOut = wgoData.GameResStr.Get(this.gameResStrKOut, "");
				}
				else
				{
					this.gameResStrValOut = this.gameResStrValue.value;
					this.gameResStrKOut = wgoData.GameResStr.GetKeyByValue(this.gameResStrValOut, "");
					this.hasGameRestStr = wgoData.GameResStr.Has(this.gameResStrKOut);
				}
			}
			else
			{
				Debug.LogError("Flow_AddGameRes: cannot get GameRes [" + this.gameResStrKey.value + "] from null WGO");
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004E20 RID: 20000 RVA: 0x00170BBD File Offset: 0x0016EDBD
		public override string name
		{
			get
			{
				return "Get GameResStr From WgoData";
			}
		}

		// Token: 0x04003F3D RID: 16189
		[FlowNode.GatherPortsCallbackAttribute]
		public bool findByValue;

		// Token: 0x04003F3E RID: 16190
		private FlowInput @in;

		// Token: 0x04003F3F RID: 16191
		private FlowOutput @out;

		// Token: 0x04003F40 RID: 16192
		private ValueInput<WgoData> wgo;

		// Token: 0x04003F41 RID: 16193
		private ValueInput<string> gameResStrKey;

		// Token: 0x04003F42 RID: 16194
		private ValueInput<string> gameResStrValue;

		// Token: 0x04003F43 RID: 16195
		private ValueOutput<string> gameResStrKeyOut;

		// Token: 0x04003F44 RID: 16196
		private ValueOutput<string> gameResStrValueOut;

		// Token: 0x04003F45 RID: 16197
		private ValueOutput<bool> hasGameResStrOut;

		// Token: 0x04003F46 RID: 16198
		private string gameResStrKOut;

		// Token: 0x04003F47 RID: 16199
		private string gameResStrValOut;

		// Token: 0x04003F48 RID: 16200
		private bool hasGameRestStr;
	}
}
