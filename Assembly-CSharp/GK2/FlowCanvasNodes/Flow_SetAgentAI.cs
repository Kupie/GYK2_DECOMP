using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF1 RID: 3057
	[Name("Set Agent AI", 0)]
	[Category("Game/Fighting")]
	[Description("Sets a specific AgentAI instance to a single WGO.")]
	[Color("f47dff")]
	public class Flow_SetAgentAI : GKCustomFlowNode
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004EE1 RID: 20193 RVA: 0x00173F52 File Offset: 0x00172152
		public override string name
		{
			get
			{
				if (!this.setNull)
				{
					return "Set Agent AI";
				}
				return "Set Agent AI Null";
			}
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00173F68 File Offset: 0x00172168
		protected override void RegisterPorts()
		{
			this.wgoData = base.AddValueInput<WgoData>("WGO", "");
			if (!this.setNull)
			{
				this.aiInstance = base.AddValueInput<AgentAI>("AI Instance", "");
			}
			base.AddFlowInput("In", delegate(Flow f)
			{
				this.Execute();
				f.Call(this.outFlow);
			}, "");
			this.outFlow = base.AddFlowOutput("Out", "");
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00173FDC File Offset: 0x001721DC
		private void Execute()
		{
			if (this.wgoData.value == null)
			{
				return;
			}
			if (!this.setNull && this.aiInstance.value == null)
			{
				return;
			}
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.wgoData.value.UniqueId);
			if (wgoViewGlobal == null)
			{
				return;
			}
			FightingAgent componentInChildren = wgoViewGlobal.GetComponentInChildren<FightingAgent>();
			if (componentInChildren == null)
			{
				Debug.LogError("[Flow_SetAgentAI]: WGO " + this.wgoData.value.id + " does not have a FightingAgent component.");
				return;
			}
			componentInChildren.SetAgentAI(this.setNull ? null : this.aiInstance.value);
			componentInChildren.StopCommandExecution(false);
		}

		// Token: 0x04004028 RID: 16424
		[FlowNode.GatherPortsCallbackAttribute]
		public bool setNull;

		// Token: 0x04004029 RID: 16425
		private ValueInput<WgoData> wgoData;

		// Token: 0x0400402A RID: 16426
		private ValueInput<AgentAI> aiInstance;

		// Token: 0x0400402B RID: 16427
		private FlowOutput outFlow;
	}
}
