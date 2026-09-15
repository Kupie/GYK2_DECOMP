using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
[Serializable]
public class CPCloudsDensity : ControllableParameter
{
	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06004BFF RID: 19455 RVA: 0x00166EA5 File Offset: 0x001650A5
	// (set) Token: 0x06004C00 RID: 19456 RVA: 0x00166EAC File Offset: 0x001650AC
	public static float AppliedDensity { get; private set; }

	// Token: 0x06004C01 RID: 19457 RVA: 0x00166EB4 File Offset: 0x001650B4
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		float num = v * this.density;
		CPCloudsDensity.values[this] = new CPCloudsDensity.Clouds(num, weatherComponent);
	}

	// Token: 0x06004C02 RID: 19458 RVA: 0x00166EDC File Offset: 0x001650DC
	public static void ApplyParameters()
	{
		if (CPCloudsDensity.values.Count == 0)
		{
			return;
		}
		CPCloudsDensity.Clouds clouds = null;
		CPCloudsDensity.Clouds clouds2 = null;
		foreach (KeyValuePair<CPCloudsDensity, CPCloudsDensity.Clouds> keyValuePair in CPCloudsDensity.values)
		{
			if (keyValuePair.Value.weatherComponent.Animating == WeatherComponent.AnimationType.FadeIn)
			{
				clouds = keyValuePair.Value;
			}
			else if (keyValuePair.Value.weatherComponent.Animating == WeatherComponent.AnimationType.FadeOut)
			{
				clouds2 = keyValuePair.Value;
			}
		}
		float num = 0f;
		foreach (CPCloudsDensity.Clouds clouds3 in CPCloudsDensity.values.Values)
		{
			num = Mathf.Max(num, clouds3.value);
		}
		if (clouds != null && clouds2 != null && clouds.weatherComponent.intensity > 0f && clouds2.weatherComponent.intensity > 0f)
		{
			CPCloudsDensity controllableParameterOfType = clouds.weatherComponent.GetControllableParameterOfType<CPCloudsDensity>();
			num = Mathf.Lerp(clouds2.weatherComponent.GetControllableParameterOfType<CPCloudsDensity>().density, controllableParameterOfType.density, clouds.weatherComponent.intensity);
		}
		CPCloudsDensity.AppliedDensity = num;
		if (LazySingletonSO<GlobalResources>.Instance != null && LazySingletonSO<GlobalResources>.Instance.cloudsMaterial != null)
		{
			LazySingletonSO<GlobalResources>.Instance.cloudsMaterial.SetFloat("_CDensitydef05", num);
		}
	}

	// Token: 0x06004C03 RID: 19459 RVA: 0x00167064 File Offset: 0x00165264
	public static float ResolveDensityForClouds()
	{
		if (CPCloudsDensity.values.Count > 0)
		{
			return CPCloudsDensity.AppliedDensity;
		}
		Material material = ((LazySingletonSO<GlobalResources>.Instance != null) ? LazySingletonSO<GlobalResources>.Instance.cloudsMaterial : null);
		if (material != null && material.HasProperty("_CDensitydef05"))
		{
			return material.GetFloat("_CDensitydef05");
		}
		return CPCloudsDensity.AppliedDensity;
	}

	// Token: 0x04003D33 RID: 15667
	[Range(0f, 1f)]
	public float density = 0.5f;

	// Token: 0x04003D35 RID: 15669
	private static readonly Dictionary<CPCloudsDensity, CPCloudsDensity.Clouds> values = new Dictionary<CPCloudsDensity, CPCloudsDensity.Clouds>();

	// Token: 0x02000B21 RID: 2849
	private class Clouds
	{
		// Token: 0x06004C06 RID: 19462 RVA: 0x001670E5 File Offset: 0x001652E5
		public Clouds(float value, WeatherComponent weatherComponent)
		{
			this.value = value;
			this.weatherComponent = weatherComponent;
		}

		// Token: 0x04003D36 RID: 15670
		public float value;

		// Token: 0x04003D37 RID: 15671
		public WeatherComponent weatherComponent;
	}
}
