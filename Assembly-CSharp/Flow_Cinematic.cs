using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004BF RID: 1215
[Name("Cinematic", 0)]
[Category("Game/Cutscenes")]
[Color("8a8a8a")]
public class Flow_Cinematic : GKCustomFlowNode
{
	// Token: 0x06002055 RID: 8277 RVA: 0x000992F8 File Offset: 0x000974F8
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeCinematic), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
		this.enable = base.AddValueInput<bool>("enable", "");
		this.immediate = base.AddValueInput<bool>("immediate", "");
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x00099390 File Offset: 0x00097590
	private void ChangeCinematic(Flow flow)
	{
		UICinematic uicinematic = LazyUI.Get<UICinematic>();
		if (this.enable.value)
		{
			uicinematic.EnableCinematic(delegate
			{
				this.onFinished.Call(flow);
			}, this.immediate.value);
		}
		else
		{
			uicinematic.DisableCinematic(delegate
			{
				this.onFinished.Call(flow);
			}, this.immediate.value);
		}
		this.@out.Call(flow);
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06002057 RID: 8279 RVA: 0x00099411 File Offset: 0x00097611
	public override string name
	{
		get
		{
			if (!this.enable.value)
			{
				return "Disable Cinematic";
			}
			return "Enable Cinematic";
		}
	}

	// Token: 0x04001CF4 RID: 7412
	private FlowInput @in;

	// Token: 0x04001CF5 RID: 7413
	private FlowOutput @out;

	// Token: 0x04001CF6 RID: 7414
	private FlowOutput onFinished;

	// Token: 0x04001CF7 RID: 7415
	private ValueInput<bool> enable;

	// Token: 0x04001CF8 RID: 7416
	private ValueInput<bool> immediate;
}
