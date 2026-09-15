using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004DD RID: 1245
[Name("Text Screen With Fade", 0)]
[Category("Game/UI")]
public class Flow_TextScreenFade : GKCustomFlowNode
{
	// Token: 0x060020AA RID: 8362 RVA: 0x0009AC00 File Offset: 0x00098E00
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoFade), "");
		this.onMiddle = base.AddFlowOutput("onMiddle".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		if (this.severalPhrases)
		{
			this.ids = base.AddValueInput<List<string>>("ids", "");
		}
		else
		{
			this.text = base.AddValueInput<string>("text", "");
		}
		this.screenFadeTime = base.AddValueInput<float>("screenFadeTime".CapitalizeFirst(), "");
		this.screenFadeTime.SetDefaultAndSerializedValue(0.6f);
		this.textFadeTime = base.AddValueInput<float>("textFadeTime".CapitalizeFirst(), "");
		this.textFadeTime.SetDefaultAndSerializedValue(0.6f);
		this.textHoldTime = base.AddValueInput<float>("textHoldTime".CapitalizeFirst(), "");
		this.textHoldTime.SetDefaultAndSerializedValue(1f);
		this.pauseTime = base.AddValueInput<float>("pauseTime".CapitalizeFirst(), "");
		this.pauseTime.SetDefaultAndSerializedValue(1f);
		this.textStyle = base.AddValueInput<TextStyle>("textStyle".CapitalizeFirst(), "");
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x0009AD88 File Offset: 0x00098F88
	private void DoFade(Flow flow)
	{
		UIFadeWithText uifadeWithText = LazyUI.Get<UIFadeWithText>();
		if (this.severalPhrases)
		{
			uifadeWithText.ScreenTextFade(this.ids.value, this.screenFadeTime.value, this.textFadeTime.value, this.pauseTime.value, this.textHoldTime.value, delegate
			{
				this.onMiddle.Call(flow);
			}, delegate
			{
				this.onFinished.Call(flow);
			}, this.textStyle.value);
		}
		else
		{
			uifadeWithText.ScreenTextFade(this.text.value, this.screenFadeTime.value, this.textFadeTime.value, this.pauseTime.value, this.textHoldTime.value, delegate
			{
				this.onMiddle.Call(flow);
			}, delegate
			{
				this.onFinished.Call(flow);
			}, this.textStyle.value);
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D64 RID: 7524
	protected const float FADE_DURATION = 0.6f;

	// Token: 0x04001D65 RID: 7525
	[FlowNode.GatherPortsCallbackAttribute]
	public bool severalPhrases;

	// Token: 0x04001D66 RID: 7526
	private FlowInput @in;

	// Token: 0x04001D67 RID: 7527
	private FlowOutput @out;

	// Token: 0x04001D68 RID: 7528
	private FlowOutput onMiddle;

	// Token: 0x04001D69 RID: 7529
	private FlowOutput onFinished;

	// Token: 0x04001D6A RID: 7530
	private ValueInput<string> text;

	// Token: 0x04001D6B RID: 7531
	private ValueInput<List<string>> ids;

	// Token: 0x04001D6C RID: 7532
	private ValueInput<float> screenFadeTime;

	// Token: 0x04001D6D RID: 7533
	private ValueInput<float> textFadeTime;

	// Token: 0x04001D6E RID: 7534
	private ValueInput<float> textHoldTime;

	// Token: 0x04001D6F RID: 7535
	private ValueInput<float> pauseTime;

	// Token: 0x04001D70 RID: 7536
	private ValueInput<TextStyle> textStyle;
}
