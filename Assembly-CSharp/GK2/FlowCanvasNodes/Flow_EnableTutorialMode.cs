using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B9F RID: 2975
	[Name("Enable Tutorial Mode", 0)]
	[Category("Game/Tutorial")]
	public class Flow_EnableTutorialMode : GKCustomFlowNode
	{
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x0016F461 File Offset: 0x0016D661
		public override string name
		{
			get
			{
				if (!this.isEnable)
				{
					return "Disable Tutorial Mode";
				}
				return "Enable Tutorial Mode";
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0016F478 File Offset: 0x0016D678
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.CallMethod), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.isEnable)
			{
				this.wgoDataInput = base.AddValueInput<List<WgoData>>("wgos", "");
			}
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0016F4E8 File Offset: 0x0016D6E8
		protected virtual void CallMethod(Flow flow)
		{
			if (this.isEnable)
			{
				List<string> list = new List<string>();
				foreach (WgoData wgoData in this.wgoDataInput.value)
				{
					if (wgoData != null)
					{
						list.Add(wgoData.UniqueId.Id);
					}
				}
				MainGame.PlayerData.SetTutorialModeState(true);
				MainGame.PlayerData.AddToTutorialModeExcludedList(list);
			}
			else
			{
				MainGame.PlayerData.SetTutorialModeState(false);
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003EDB RID: 16091
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isEnable;

		// Token: 0x04003EDC RID: 16092
		private FlowInput @in;

		// Token: 0x04003EDD RID: 16093
		private FlowOutput @out;

		// Token: 0x04003EDE RID: 16094
		private ValueInput<List<WgoData>> wgoDataInput;
	}
}
