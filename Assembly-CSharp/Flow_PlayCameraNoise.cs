using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004CF RID: 1231
[Name("Shake Camera", 0)]
[Category("Game/Cutscenes")]
public class Flow_PlayCameraNoise : GKCustomFlowNode
{
	// Token: 0x06002087 RID: 8327 RVA: 0x0009A0E8 File Offset: 0x000982E8
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ShakeCamera), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.duration = base.AddValueInput<float>("duration".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x0009A170 File Offset: 0x00098370
	private void ShakeCamera(Flow flow)
	{
		if (this.duration.value == 0f)
		{
			Debug.LogError("Flow_PlayCameraNoise: Noise duration can't be 0.");
			this.onFinished.Call(flow);
		}
		else if (this.useAdditionalOptions)
		{
			CameraSystem.Instance.ActiveCameraController.ShakeCamera(this.type, this.duration.value, this.fadeDuration, this.amplitude, delegate
			{
				this.onFinished.Call(flow);
			});
		}
		else
		{
			CameraSystem.Instance.ActiveCameraController.ShakeCamera(this.type, this.duration.value, 0f, 1f, delegate
			{
				this.onFinished.Call(flow);
			});
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D25 RID: 7461
	private FlowInput @in;

	// Token: 0x04001D26 RID: 7462
	private FlowOutput @out;

	// Token: 0x04001D27 RID: 7463
	private FlowOutput onFinished;

	// Token: 0x04001D28 RID: 7464
	public CameraNoiseType type;

	// Token: 0x04001D29 RID: 7465
	public bool useAdditionalOptions;

	// Token: 0x04001D2A RID: 7466
	[ShowIf("useAdditionalOptions", 1)]
	public float fadeDuration;

	// Token: 0x04001D2B RID: 7467
	[ShowIf("useAdditionalOptions", 1)]
	public float amplitude = 1f;

	// Token: 0x04001D2C RID: 7468
	private ValueInput<float> duration;
}
