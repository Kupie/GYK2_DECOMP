using System;
using LazyBearTechnology;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000179 RID: 377
public sealed class SmartRasterizerPostRenderer : PostProcessEffectRenderer<SmartRasterizerPostEffect>
{
	// Token: 0x06000964 RID: 2404 RVA: 0x0002FDA0 File Offset: 0x0002DFA0
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(LazySingletonSO<GlobalResources>.Instance.smartRasterizerShader);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false, null, false);
	}
}
