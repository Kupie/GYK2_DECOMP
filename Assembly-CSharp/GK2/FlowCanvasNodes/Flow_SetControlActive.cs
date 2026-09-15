using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF3 RID: 3059
	[Name("Set Control Active", 0)]
	[Category("Game/Cutscenes")]
	[Color("8a8a8a")]
	public class Flow_SetControlActive : GKCustomFlowNode
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x001741E9 File Offset: 0x001723E9
		public override string name
		{
			get
			{
				return (this.isEnable.value ? "Control Enable" : "Control Disable") + (this.isAffectCinematic ? " \n/w Cinematic" : "");
			}
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x00174220 File Offset: 0x00172420
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetControl), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.isEnable = base.AddValueInput<bool>("isEnable".CapitalizeFirst(), "");
			if (this.isAffectCinematic)
			{
				this.onFinish = base.AddFlowOutput("onFinish".CapitalizeFirst(), "");
			}
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x001742B0 File Offset: 0x001724B0
		private void SetControl(Flow flow)
		{
			if (!this.isAffectCinematic)
			{
				MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFlow, this.isEnable.value);
				GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerChangeControlByFlow, this.isEnable.value.ToString());
				this.@out.Call(flow);
				return;
			}
			UICinematic uicinematic = LazyUI.Get<UICinematic>();
			if (this.isEnable.value)
			{
				uicinematic.DisableCinematic(delegate
				{
					MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFlow, this.isEnable.value);
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerChangeControlByFlow, this.isEnable.value.ToString());
					this.onFinish.Call(flow);
				}, this.isImmediateCinematic);
				this.@out.Call(flow);
				return;
			}
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFlow, this.isEnable.value);
			uicinematic.EnableCinematic(delegate
			{
				this.onFinish.Call(flow);
			}, this.isImmediateCinematic);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerChangeControlByFlow, this.isEnable.value.ToString());
			this.@out.Call(flow);
		}

		// Token: 0x0400402E RID: 16430
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isAffectCinematic;

		// Token: 0x0400402F RID: 16431
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("isAffectCinematic", 1)]
		public bool isImmediateCinematic;

		// Token: 0x04004030 RID: 16432
		private FlowInput @in;

		// Token: 0x04004031 RID: 16433
		private FlowOutput @out;

		// Token: 0x04004032 RID: 16434
		protected FlowOutput onFinish;

		// Token: 0x04004033 RID: 16435
		private ValueInput<bool> isEnable;
	}
}
