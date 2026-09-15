using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C10 RID: 3088
	[Name("Sleep Fade", 0)]
	[Category("Game/Cutscenes")]
	public class Flow_SleepFade : GKCustomFlowNode
	{
		// Token: 0x06004F4C RID: 20300 RVA: 0x00175BB0 File Offset: 0x00173DB0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoSleepFade), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
			this.instant = base.AddValueInput<bool>("instant?", "");
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00175C30 File Offset: 0x00173E30
		private void DoSleepFade(Flow flow)
		{
			UISleepFade uisleepFade = LazyUI.Get<UISleepFade>();
			if (this.instant.value)
			{
				Flow_SleepFade.FadeType fadeType = this.fadeType;
				if (fadeType != Flow_SleepFade.FadeType.In)
				{
					if (fadeType != Flow_SleepFade.FadeType.Out)
					{
						throw new ArgumentOutOfRangeException();
					}
					uisleepFade.FadeOutInstant(FadeFlag.Common);
				}
				else
				{
					uisleepFade.FadeInInstant(FadeFlag.Common);
				}
				this.onFinished.Call(flow);
			}
			else
			{
				Flow_SleepFade.FadeType fadeType = this.fadeType;
				if (fadeType != Flow_SleepFade.FadeType.In)
				{
					if (fadeType != Flow_SleepFade.FadeType.Out)
					{
						throw new ArgumentOutOfRangeException();
					}
					uisleepFade.FadeOut(delegate
					{
						this.onFinished.Call(flow);
					}, false);
				}
				else
				{
					uisleepFade.FadeIn(delegate
					{
						this.onFinished.Call(flow);
					}, FadeFlag.Common, true, this.animType, false);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004F4E RID: 20302 RVA: 0x00175CF8 File Offset: 0x00173EF8
		public override string name
		{
			get
			{
				Flow_SleepFade.FadeType fadeType = this.fadeType;
				string text;
				if (fadeType != Flow_SleepFade.FadeType.In)
				{
					if (fadeType != Flow_SleepFade.FadeType.Out)
					{
						text = base.name;
					}
					else
					{
						text = "Sleep Fade Out";
					}
				}
				else
				{
					text = "Sleep Fade In";
				}
				return text;
			}
		}

		// Token: 0x040040A5 RID: 16549
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_SleepFade.FadeType fadeType;

		// Token: 0x040040A6 RID: 16550
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("fadeType", 0)]
		public SleepAnimType animType;

		// Token: 0x040040A7 RID: 16551
		private FlowInput @in;

		// Token: 0x040040A8 RID: 16552
		private FlowOutput @out;

		// Token: 0x040040A9 RID: 16553
		private FlowOutput onFinished;

		// Token: 0x040040AA RID: 16554
		private ValueInput<bool> instant;

		// Token: 0x02000C11 RID: 3089
		public enum FadeType
		{
			// Token: 0x040040AC RID: 16556
			In,
			// Token: 0x040040AD RID: 16557
			Out
		}
	}
}
