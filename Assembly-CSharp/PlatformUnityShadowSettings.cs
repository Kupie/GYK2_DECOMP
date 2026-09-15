using System;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public readonly struct PlatformUnityShadowSettings
{
	// Token: 0x060031DC RID: 12764 RVA: 0x000EF108 File Offset: 0x000ED308
	public PlatformUnityShadowSettings(ShadowQuality shadowQuality, ShadowResolution shadowResolution, float shadowDistance, int shadowCascades, LightShadows sunLightShadows, int pixelLightCount, LightShadows? localLightShadows = null)
	{
		this.shadowQuality = shadowQuality;
		this.shadowResolution = shadowResolution;
		this.shadowDistance = shadowDistance;
		this.shadowCascades = shadowCascades;
		this.sunLightShadows = sunLightShadows;
		this.pixelLightCount = pixelLightCount;
		this.localLightShadows = localLightShadows;
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x060031DD RID: 12765 RVA: 0x000EF13F File Offset: 0x000ED33F
	public bool IsDisabled
	{
		get
		{
			return this.shadowQuality == ShadowQuality.Disable;
		}
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x000EF14C File Offset: 0x000ED34C
	public void Apply(Light sunLight)
	{
		if (this.IsDisabled)
		{
			QualitySettings.shadows = ShadowQuality.Disable;
			if (sunLight != null)
			{
				sunLight.shadows = LightShadows.None;
			}
			return;
		}
		QualitySettings.shadows = this.shadowQuality;
		QualitySettings.shadowResolution = this.shadowResolution;
		QualitySettings.shadowDistance = this.shadowDistance;
		QualitySettings.shadowCascades = this.shadowCascades;
		QualitySettings.pixelLightCount = this.pixelLightCount;
		if (sunLight != null)
		{
			sunLight.shadows = this.sunLightShadows;
		}
	}

	// Token: 0x0400281F RID: 10271
	public readonly ShadowQuality shadowQuality;

	// Token: 0x04002820 RID: 10272
	public readonly ShadowResolution shadowResolution;

	// Token: 0x04002821 RID: 10273
	public readonly float shadowDistance;

	// Token: 0x04002822 RID: 10274
	public readonly int shadowCascades;

	// Token: 0x04002823 RID: 10275
	public readonly LightShadows sunLightShadows;

	// Token: 0x04002824 RID: 10276
	public readonly int pixelLightCount;

	// Token: 0x04002825 RID: 10277
	public readonly LightShadows? localLightShadows;
}
