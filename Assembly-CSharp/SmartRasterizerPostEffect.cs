using System;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000178 RID: 376
[PostProcess(typeof(SmartRasterizerPostRenderer), PostProcessEvent.BeforeTransparent, "Custom/Smart Rasterizer", true)]
[Serializable]
public sealed class SmartRasterizerPostEffect : PostProcessEffectSettings
{
	// Token: 0x06000962 RID: 2402 RVA: 0x0002FD7A File Offset: 0x0002DF7A
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return base.IsEnabledAndSupported(context) && PlatformFeatures.Current.renderMode != PlatformRenderMode.Lightweight;
	}
}
