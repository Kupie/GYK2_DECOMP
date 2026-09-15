using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA3 RID: 2979
	[Name("Fade", 0)]
	[Category("Game/Cutscenes")]
	public class Flow_Fade : GKCustomFlowNode
	{
		// Token: 0x06004DDC RID: 19932 RVA: 0x0016F688 File Offset: 0x0016D888
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoFade), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.fadeType == Flow_Fade.FadeType.InOut)
			{
				this.onMiddle = base.AddFlowOutput("onMiddle".CapitalizeFirst(), "");
			}
			this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
			this.instant = base.AddValueInput<bool>("instant?", "");
			this.fadeTime = base.AddValueInput<float>("fadeTime", "");
			this.fadeTime.SetDefaultAndSerializedValue(0.5f);
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0016F754 File Offset: 0x0016D954
		private void DoFade(Flow flow)
		{
			UIFade fadeElement = LazyUI.Get<UIFade>();
			if (!this.instant.value)
			{
				switch (this.fadeType)
				{
				case Flow_Fade.FadeType.In:
					fadeElement.FadeIn(this.fadeTime.value, delegate
					{
						this.onFinished.Call(flow);
					}, FadeFlag.FlowScript, false);
					break;
				case Flow_Fade.FadeType.Out:
					fadeElement.FadeOut(this.fadeTime.value, delegate
					{
						this.onFinished.Call(flow);
					}, FadeFlag.FlowScript);
					break;
				case Flow_Fade.FadeType.InOut:
				{
					Action <>9__3;
					fadeElement.FadeIn(this.fadeTime.value / 2f, delegate
					{
						this.onMiddle.Call(flow);
						UIBasicFade fadeElement2 = fadeElement;
						float num = this.fadeTime.value / 2f;
						Action action;
						if ((action = <>9__3) == null)
						{
							action = (<>9__3 = delegate
							{
								this.onFinished.Call(flow);
							});
						}
						fadeElement2.FadeOut(num, action, FadeFlag.FlowScript);
					}, FadeFlag.FlowScript, false);
					break;
				}
				}
			}
			else
			{
				switch (this.fadeType)
				{
				case Flow_Fade.FadeType.In:
					fadeElement.FadeInInstant(FadeFlag.FlowScript);
					break;
				case Flow_Fade.FadeType.Out:
					fadeElement.FadeOutInstant(FadeFlag.FlowScript);
					break;
				case Flow_Fade.FadeType.InOut:
					fadeElement.FadeInInstant(FadeFlag.FlowScript);
					fadeElement.FadeOutInstant(FadeFlag.FlowScript);
					break;
				}
			}
			this.@out.Call(flow);
			if (this.instant.value)
			{
				this.onFinished.Call(flow);
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06004DDE RID: 19934 RVA: 0x0016F8A8 File Offset: 0x0016DAA8
		public override string name
		{
			get
			{
				string text;
				switch (this.fadeType)
				{
				case Flow_Fade.FadeType.In:
					text = "Fade In";
					break;
				case Flow_Fade.FadeType.Out:
					text = "Fade Out";
					break;
				case Flow_Fade.FadeType.InOut:
					text = "Fade In Out";
					break;
				default:
					text = base.name;
					break;
				}
				return text;
			}
		}

		// Token: 0x04003EE5 RID: 16101
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_Fade.FadeType fadeType;

		// Token: 0x04003EE6 RID: 16102
		private FlowInput @in;

		// Token: 0x04003EE7 RID: 16103
		private FlowOutput @out;

		// Token: 0x04003EE8 RID: 16104
		private FlowOutput onMiddle;

		// Token: 0x04003EE9 RID: 16105
		private FlowOutput onFinished;

		// Token: 0x04003EEA RID: 16106
		private ValueInput<bool> instant;

		// Token: 0x04003EEB RID: 16107
		private ValueInput<float> fadeTime;

		// Token: 0x02000BA4 RID: 2980
		public enum FadeType
		{
			// Token: 0x04003EED RID: 16109
			In,
			// Token: 0x04003EEE RID: 16110
			Out,
			// Token: 0x04003EEF RID: 16111
			InOut
		}
	}
}
